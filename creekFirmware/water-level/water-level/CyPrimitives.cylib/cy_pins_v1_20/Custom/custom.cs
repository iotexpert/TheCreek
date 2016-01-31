/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.Common.Base;
using Cypress.Comps.PinsAndPorts.Common_v1_20;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_20
{
    public class CyCustomizer : CyPortCustomizer,
        ICyShapeCustomize_v1,
        ICyParamEditHook_v1
    {
        public const string BuiltinTabName = "Built-in";
        public const string ConfigureTabName = "Configure";
        public const string ResetTabName = "Reset";
        public const string MappingTabName = "Mapping";

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyPinsControl pinsControl = new CyPinsControl(edit);
            CyPORControl porControl = new CyPORControl();
            porControl.Edit = edit;
            CyMappingControl mappingControl = new CyMappingControl();
            mappingControl.Edit = edit;

            CyParamExprDelegate UpdateGUIFromExprViewChanging =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    pinsControl.UpdateFromExprs();
                    porControl.UpdateFromExprs();
                    mappingControl.UpdateFromExprs();
                };

            CyParamExprDelegate HandleParamChanged =
                delegate(ICyParamEditor custEditor, CyCompDevParam param)
                {
                    if (param.TabName != ConfigureTabName)
                    {
                        //Update the tree if a param on another tab changed. Examples: inst name needs to regenerate
                        //tree node names, layout mode changing can change errors with the per pin data, ...
                        pinsControl.UpdateFromExprs();
                    }

                    //Errors may need to be updated.
                    porControl.UpdateFromExprs();
                    mappingControl.UpdateFromExprs();
                };

            editor.ParamExprCommittedDelegate = HandleParamChanged;
            editor.AddCustomPage("Pins", pinsControl, UpdateGUIFromExprViewChanging, ConfigureTabName);
            editor.AddCustomPage(MappingTabName, mappingControl, UpdateGUIFromExprViewChanging, MappingTabName);
            editor.AddCustomPage(ResetTabName, porControl, UpdateGUIFromExprViewChanging, ResetTabName);
            editor.AddDefaultPage(BuiltinTabName, BuiltinTabName);
            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        #endregion

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            //The entire symbol is drawn here. 
            //In the special case where the width is one, no IRQ is used, and only one side has terminals on it 
            //we draw as small as possible and put the instance name on the opposite side as the terminals. 

            CyCustErr err;

            byte numPins;
            err = CyParamInfo.GetNumPinsValue(query, out numPins);
            if (err.IsNotOk) return err;

            bool usesInterrupts;
            err = CyParamInfo.GetUsesInterruptsValue(query, out usesInterrupts);
            if (err.IsNotOk) return err;

            CyStringArrayParamData pinTypes;
            err = CyStringArrayParamData.CreateBitVectorData(query, CyBitVectorParam.Formal_PinTypes, out pinTypes);
            if (err.IsNotOk) return err;

            CyPinsDrawer drawer = new CyPinsDrawer(query, shapeEdit, termEdit);

            bool displayAsBus;
            err = CyParamInfo.GetDisplayAsBusValue(query, out displayAsBus);
            if (err.IsNotOk) return err;

            bool isHomogeneous;
            err = CyParamInfo.GetIsHomogeneousValue_v2(query, out isHomogeneous);
            if (err.IsNotOk) return err;

            CyStringArrayParamData sioInfoData;
            err = CyStringArrayParamData.CreateBitVectorData(query, CyBitVectorParam.Local_SIOInfo, out sioInfoData);
            if (err.IsNotOk) return err;

            byte sioGroupCnt;
            err = CyParamInfo.GetSIOGroupCntValue(query, out sioGroupCnt);
            if (err.IsNotOk) return err;

            CyStringArrayParamData sioRefSelData;
            err = CyStringArrayParamData.CreateBitVectorData(query, CyBitVectorParam.Local_SIORefSels, out sioRefSelData);
            if (err.IsNotOk) return err;

            CyStringArrayParamData displayInputsData;
            err = CyStringArrayParamData.CreateBitVectorData(query, CyBitVectorParam.Formal_DisplayInputConnections,
                out displayInputsData);
            if (err.IsNotOk) return err;

            CyStringArrayParamData displayOutputsData;
            err = CyStringArrayParamData.CreateBitVectorData(query, CyBitVectorParam.Formal_DisplayOutputConnections,
                out displayOutputsData);
            if (err.IsNotOk) return err;

            if (displayAsBus && isHomogeneous)
            {
                err = drawer.DrawAsBus(isHomogeneous, numPins, sioGroupCnt, usesInterrupts,
                    pinTypes, sioInfoData, sioRefSelData, displayInputsData, displayOutputsData);
            }
            else
            {
                err = drawer.DrawAsIndividualTerminals(isHomogeneous, numPins, usesInterrupts,
                    pinTypes, sioInfoData, sioRefSelData, displayInputsData, displayOutputsData);
            }
            if (err.IsNotOk) return err;

            return CyCustErr.Ok;
        }

        class CyPinsDrawer
        {
            #region Data

            ICyInstQuery_v1 m_query;
            ICySymbolShapeEdit_v1 m_shapeEdit;
            ICyTerminalEdit_v1 m_termEdit;

            const int ParameterTextSize = 8;
            const int DecorativeTextSize = 6;
            const int OtherTextSize = 10;

            const string ThinLineTag = "ThinLineTag";
            const string BodyLineTag = "BodyLineTag";
            const string PinBodyTag = "PinBodyTag";
            readonly string[] PinBodyTags = new string[] { PinBodyTag, ThinLineTag };
            readonly Color PinBodyFillColor = Color.White;
            readonly Color PinBodyOutlineColor = Color.Gray;

            const string PinXTag = "PinXTag";
            readonly string[] PinXTags = new string[] { PinXTag, ThinLineTag };

            const string TextTag = "TextTag";
            readonly string[] TextTags = new string[] { TextTag };
            readonly Color TextFillColor = Color.Black;

            const string IndexTag = "IndexTag";
            readonly string[] IndexTags = new string[] { IndexTag };
            readonly Color IndexFillColor = Color.Black;

            const string DigitalTag = "DigitalTag";
            readonly string[] DigitalTags = new string[] { DigitalTag };
            readonly Color DigitalOutlineColor;

            const string AnalogTag = "AnalogTag";
            readonly string[] AnalogTags = new string[] { AnalogTag };
            readonly Color AnalogOutlineColor;

            const string ArrowLineTag = "ArrowLinesTag";
            readonly string[] ArrowLineTags = new string[] { ArrowLineTag };

            const string ArrowTag = "ArrowTag";
            readonly string[] ArrowTags = new string[] { ArrowTag, ThinLineTag };

            const string SIOTag = "SIOTag";
            readonly string[] SIOTags = new string[] { SIOTag };

            string GetTerminalName(string rootName, int index, int width)
            {
                if (width == 1)
                {
                    return string.Format("{0}_{1}", rootName, index);
                }
                return string.Format("{0}_{1}[{2}:0]", rootName, index, width - 1);
            }

            RectangleF GetPinBodyBounds(PointF reservedLoc)
            {
                float quarterUnit = 0.25f * m_shapeEdit.UserBaseUnit;
                float size = 1.5f * m_shapeEdit.UserBaseUnit;

                return new RectangleF(reservedLoc.X + quarterUnit, reservedLoc.Y + quarterUnit, size, size);
            }

            readonly float TopYDelta;
            readonly float BottomYDelta;
            readonly float SingleReservedSize;
            readonly float SIOBorderInflation;
            readonly Color TitleColor = Color.FromArgb(255, 192, 203); //From YFS159

            #endregion

            #region Constructors/Destructor

            public CyPinsDrawer(ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit, ICyTerminalEdit_v1 termEdit)
            {
                m_query = query;
                m_shapeEdit = shapeEdit;
                m_termEdit = termEdit;
                TopYDelta = m_shapeEdit.UserBaseUnit;
                BottomYDelta = 3 * TopYDelta;
                SingleReservedSize = 2 * m_shapeEdit.UserBaseUnit;
                SIOBorderInflation = m_shapeEdit.UserBaseUnit * 0.15f;

                DigitalOutlineColor = m_query.Preferences.DigitalWireColor;
                AnalogOutlineColor = m_query.Preferences.AnalogWireColor;
            }

            #endregion

            #region Drawing Methods

            /// <summary>
            /// Removes all terminals and shapes from the component.
            /// </summary>
            /// <returns></returns>
            CyCustErr Clear()
            {
                CyCustErr err;

                err = m_termEdit.RemoveAllTerminals();
                if (err.IsNotOk) return err;

                err = m_shapeEdit.RemoveAllShapes();
                if (err.IsNotOk) return err;

                return CyCustErr.Ok;
            }

            public CyCustErr DrawSWOnly()
            {
                CyCustErr err;

                err = Clear();
                if (err.IsNotOk) return err;

                PointF reservedLoc = new PointF(0, -m_shapeEdit.UserBaseUnit * 2);

                RectangleF bodyBounds = DrawPinBody(reservedLoc);
                RectangleF compBounds;

                bool usesInterrupts;
                err = CyParamInfo.GetUsesInterruptsValue(m_query, out usesInterrupts);
                if (err.IsNotOk) return err;

                err = DrawOuterComponentAndIRQ(usesInterrupts, false, reservedLoc.Y, out compBounds);
                if (err.IsNotOk) return err;

                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Near;
                fmt.Alignment = StringAlignment.Far;

                m_shapeEdit.CreateAnnotation(TextTags, "Width = `=$" + CyParamInfo.Formal_ParamName_NumPins + "`",
                    new PointF(compBounds.Right, compBounds.Bottom),
                    new Font(FontFamily.GenericSansSerif, ParameterTextSize, FontStyle.Regular), fmt);

                SetShapeStyles();
                return CyCustErr.Ok;
            }

            /// <summary>
            /// Used to center the first pin on (0,0) in the y-direction.
            /// </summary>
            float YOffset
            {
                get
                {
                    return SingleReservedSize;
                }
            }

            public CyCustErr DrawAsIndividualTerminals(bool isHomogeneous, byte width, bool usesInterrupts,
                CyStringArrayParamData pinTypes, CyStringArrayParamData sioInfos, CyStringArrayParamData sioRefSels,
                CyStringArrayParamData displayInputsData, CyStringArrayParamData displayOutputsData)
            {
                CyCustErr err;

                err = Clear();
                if (err.IsNotOk) return err;

                bool drawAsSinglePin;
                err = GetDrawAsSignglePin(isHomogeneous, false, width, usesInterrupts, pinTypes,
                    sioInfos, sioRefSels, out drawAsSinglePin);
                if (err.IsNotOk) return err;

                float y = -YOffset;
                for (int index = 0; index < width; index++)
                {
                    string displayInputs = displayInputsData.GetValue(index);
                    string displayOutputs = displayOutputsData.GetValue(index);
                    string pinType = pinTypes.GetValue(index);
                    string sioInfo = sioInfos.GetValue(index);
                    int sioGroupNum = GetSIOGroupNumber(sioInfos, index);
                    string sioRefSel = sioRefSels.GetValue(sioGroupNum);
                    CyPinTypeData pinTypeData;
                    err = CyPinTypeData.CreatePinTypeData(pinType, sioInfo, sioRefSel, out pinTypeData);
                    if (err.IsNotOk) return err;

                    PointF reservedLoc = new PointF(0, y);
                    y += SingleReservedSize * pinTypeData.NumTerminalsTall;
                    err = DrawPin(reservedLoc, pinTypeData, index, drawAsSinglePin, 1, 1, true, sioInfos,
                        displayInputs, displayOutputs);
                    if (err.IsNotOk) return err;
                }

                UpdateZOrders();

                if (drawAsSinglePin == false)
                {
                    DrawOuterComponentAndIRQ(usesInterrupts, true, y + YOffset);
                }

                SetShapeStyles();
                return CyCustErr.Ok;
            }

            int GetSIOGroupNumber(CyStringArrayParamData sioInfos, int index)
            {
                int sioCnt = 0;
                for (int i = 0; i < index; i++)
                {
                    string sioInfo = sioInfos.GetValue(i);

                    switch (sioInfo)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR: //only count each pair once
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            sioCnt++;
                            break;

                        default:
                            break;
                    }
                }

                return sioCnt;
            }

            private void UpdateZOrders()
            {
                //Ensures that the analog wires draw below the sio border and the digital inputs (if used).
                m_shapeEdit.SendToBack(SIOTag);
                m_shapeEdit.SendToBack(AnalogTag);
            }

            public CyCustErr DrawAsBus(bool isHomogeneous, byte width, byte sioGroupCnt, bool usesInterrupts,
                CyStringArrayParamData pinTypes, CyStringArrayParamData sioInfos, CyStringArrayParamData sioRefSels,
                CyStringArrayParamData displayInputsData, CyStringArrayParamData displayOutputsData)
            {
                CyCustErr err;

                err = Clear();
                if (err.IsNotOk) return err;

                bool drawAsSinglePin;
                err = GetDrawAsSignglePin(isHomogeneous, true, width, usesInterrupts, pinTypes,
                    sioInfos, sioRefSels, out drawAsSinglePin);
                if (err.IsNotOk) return err;

                //These values are homogeneous when displayed as a bus so just grabbing the first value is accurate.
                string pinType = pinTypes.GetValue(0);
                string sioInfo = sioInfos.GetValue(0);
                string sioRefSel = sioRefSels.GetValue(0);
                string displayInputs = displayInputsData.GetValue(0);
                string displayOutputs = displayOutputsData.GetValue(0);

                //This will cause it to draw correctly with the sio box drawn.
                if (sioInfo != CyPortConstants.SIOInfoValue_NOT_SIO)
                {
                    sioInfo = CyPortConstants.SIOInfoValue_SINGLE_SIO;
                }

                CyPinTypeData pinTypeData;
                err = CyPinTypeData.CreatePinTypeData(pinType, sioInfo, sioRefSel, out pinTypeData);
                if (err.IsNotOk) return err;

                PointF reservedLoc = new PointF(0, -YOffset);
                err = DrawPin(reservedLoc, pinTypeData, width - 1, drawAsSinglePin, width,
                    sioGroupCnt, false, sioInfos, displayInputs, displayOutputs);
                if (err.IsNotOk) return err;

                UpdateZOrders();

                if (drawAsSinglePin == false)
                {
                    DrawOuterComponentAndIRQ(usesInterrupts, true, pinTypeData.NumTerminalsTall * SingleReservedSize);
                }

                SetShapeStyles();
                return CyCustErr.Ok;
            }

            CyCustErr DrawOuterComponentAndIRQ(bool usesInterrupts, bool irqOnBottom, float height)
            {
                RectangleF ignored;
                return DrawOuterComponentAndIRQ(usesInterrupts, irqOnBottom, height, out ignored);
            }

            CyCustErr DrawOuterComponentAndIRQ(bool usesInterrupts, bool irqOnBottom, float height,
                out RectangleF outterCompBounds)
            {
                //This should only be called if not drawAsSinglePin.
                //Draw a box around the component, draw the inst name, and check if the irq terminal needs to be
                //added.

                float compBodyWidth = 3 * SingleReservedSize;
                float compBodyHeight = Math.Abs(height);
                PointF loc = new PointF(-SingleReservedSize, -YOffset);

                outterCompBounds = new RectangleF(loc.X, loc.Y, compBodyWidth, compBodyHeight);
                string compBodyTag = "CompBodyTag";
                string[] compBodyTags = new string[] { compBodyTag, BodyLineTag };
                m_shapeEdit.CreateRectangle(compBodyTags, loc, compBodyWidth, compBodyHeight);
                m_shapeEdit.SetFillColor(compBodyTag, Color.FromArgb(220, 220, 220)); //From YFS159
                m_shapeEdit.SetOutlineColor(compBodyTag, Color.Black);
                m_shapeEdit.SendToBack(compBodyTag);

                string compTitleTag = "CompTitleTag";
                string[] compTitleTags = new string[] { compTitleTag, BodyLineTag };
                m_shapeEdit.CreateRectangle(compTitleTags,
                    new PointF(loc.X, loc.Y - 2 * m_shapeEdit.UserBaseUnit),
                    compBodyWidth, 2 * m_shapeEdit.UserBaseUnit);
                m_shapeEdit.SetFillColor(compTitleTag, TitleColor);
                m_shapeEdit.SetOutlineColor(compTitleTag, Color.Black);
                m_shapeEdit.SendToBack(compTitleTag);

                PointF center = CyRectangleF.GetCenterCenter(new RectangleF(
                    loc.X, loc.Y - 2 * m_shapeEdit.UserBaseUnit, compBodyWidth, 2 * m_shapeEdit.UserBaseUnit));
                StringFormat titleFmt = new StringFormat();
                titleFmt.LineAlignment = StringAlignment.Center;
                titleFmt.Alignment = StringAlignment.Center;
                m_shapeEdit.CreateAnnotation(TextTags, "Pins", center,
                        new Font(FontFamily.GenericSansSerif, OtherTextSize, FontStyle.Regular), titleFmt);

                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Far;
                fmt.Alignment = StringAlignment.Far;
                DrawInstanceName(new PointF(loc.X + compBodyWidth, loc.Y - 2 * m_shapeEdit.UserBaseUnit), fmt);

                if (usesInterrupts)
                {
                    if (irqOnBottom)
                    {
                        PointF termLoc = new PointF(loc.X + compBodyWidth / 2,
                            loc.Y + compBodyHeight + m_shapeEdit.UserBaseUnit);
                        PointF endTermLoc = new PointF(termLoc.X, loc.Y + compBodyHeight);
                        m_shapeEdit.CreateLine(DigitalTags, termLoc, endTermLoc);
                        m_termEdit.TerminalAdd(CyTermInfo.RootTermName_Interrupt + "_0",
                            CyCompDevTermDir.OUTPUT, CyCompDevTermType.DIGITAL, termLoc, string.Empty, 90, false);

                        StringFormat irqFmt = new StringFormat();
                        irqFmt.LineAlignment = StringAlignment.Near;
                        irqFmt.Alignment = StringAlignment.Far;
                        m_shapeEdit.CreateAnnotation(TextTags, "irq",
                            new PointF(endTermLoc.X - m_shapeEdit.UserBaseUnit / 2.0f, endTermLoc.Y),
                            new Font(FontFamily.GenericSansSerif, ParameterTextSize, FontStyle.Regular), irqFmt);
                    }
                    else
                    {
                        PointF termLoc = new PointF(loc.X + compBodyWidth + m_shapeEdit.UserBaseUnit,
                            loc.Y + compBodyHeight / 2);
                        PointF endTermLoc = new PointF(termLoc.X - m_shapeEdit.UserBaseUnit, termLoc.Y);
                        m_shapeEdit.CreateLine(DigitalTags, termLoc, endTermLoc);
                        m_termEdit.TerminalAdd(CyTermInfo.RootTermName_Interrupt + "_0",
                            CyCompDevTermDir.OUTPUT, CyCompDevTermType.DIGITAL, termLoc, string.Empty, 0, false);

                        StringFormat irqFmt = new StringFormat();
                        irqFmt.LineAlignment = StringAlignment.Center;
                        irqFmt.Alignment = StringAlignment.Far;
                        m_shapeEdit.CreateAnnotation(TextTags, "irq", endTermLoc,
                            new Font(FontFamily.GenericSansSerif, ParameterTextSize, FontStyle.Regular), irqFmt);
                    }
                }
                return CyCustErr.Ok;
            }

            void SetShapeStyles()
            {
                m_shapeEdit.SetOutlineColor(AnalogTag, AnalogOutlineColor);
                m_shapeEdit.SetOutlineColor(DigitalTag, DigitalOutlineColor);
                m_shapeEdit.SetOutlineWidth(ThinLineTag, 0.25f);
                m_shapeEdit.SetOutlineWidth(BodyLineTag, 1);

                m_shapeEdit.SetOutlineWidth(AnalogTag, m_query.Preferences.WireSize);
                m_shapeEdit.SetOutlineWidth(DigitalTag, m_query.Preferences.WireSize);

                m_shapeEdit.SetOutlineColor(SIOTag, Color.Black);
                m_shapeEdit.SetFillColor(SIOTag, TitleColor);
                m_shapeEdit.SetOutlineWidth(SIOTag, 0.25f);

                m_shapeEdit.ClearOutline(TextTag);
                m_shapeEdit.SetFillColor(TextTag, TextFillColor);
            }

            /// <summary>
            /// If true there is no component box drawn around the pin and the name is drawn to one side instead
            /// of above the component.
            /// </summary>
            /// <param name="isHomogeneous"></param>
            /// <param name="drawAsBus"></param>
            /// <param name="width"></param>
            /// <param name="usesInterrupts"></param>
            /// <param name="pinTypes"></param>
            /// <param name="drawAsSinglePin"></param>
            /// <returns></returns>
            CyCustErr GetDrawAsSignglePin(bool isHomogeneous, bool drawAsBus,
                byte width, bool usesInterrupts, CyStringArrayParamData pinTypes,
                CyStringArrayParamData sioInfos, CyStringArrayParamData sioRefSels, out bool drawAsSinglePin)
            {
                CyCustErr err;
                drawAsSinglePin = default(bool);

                if ((width == 1 || (isHomogeneous && drawAsBus)) && !usesInterrupts)
                {
                    string pinType = pinTypes.GetValue(0);
                    string sioInfo = sioInfos.GetValue(0);
                    string sioRefSel = sioRefSels.GetValue(0);
                    CyPinTypeData pinTypeData;
                    err = CyPinTypeData.CreatePinTypeData(pinType, sioInfo, sioRefSel, out pinTypeData);
                    if (err.IsNotOk) return err;

                    drawAsSinglePin = pinTypeData.OnlyOneSideUsed;
                }
                else
                {
                    drawAsSinglePin = false;
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawPin(PointF reservedLoc, CyPinTypeData pinTypeData, int indexOfLeftMostBit,
                bool drawAsSinglePin, int width, int sioWidth, bool showIndex, CyStringArrayParamData sioInfos,
                string displayInputs, string displayOutputs)
            {
                CyCustErr err;

                if (pinTypeData.SIOVRefUsed)
                {
                    int sioGroupIndex = GetSIOGroupNumber(sioInfos, indexOfLeftMostBit);
                    err = DrawSIOVRef(reservedLoc, sioGroupIndex, sioWidth);
                    if (err.IsNotOk) return err;
                }
                if (pinTypeData.BidirectionalUsed)
                {
                    err = DrawBidirectional(reservedLoc, indexOfLeftMostBit, width);
                    if (err.IsNotOk) return err;
                }
                if (pinTypeData.AnalogUsed)
                {
                    err = DrawAnalog(reservedLoc, pinTypeData.DigitalInputUsed, indexOfLeftMostBit, width);
                    if (err.IsNotOk) return err;
                }
                if (pinTypeData.DigitalInputUsed)
                {
                    err = DrawDigitalInput(reservedLoc, indexOfLeftMostBit, width, displayInputs);
                    if (err.IsNotOk) return err;
                }
                if (pinTypeData.OutputEnableUsed)
                {
                    //The output enable width is always one. If drawn individually then it should be one.
                    //If drawn as bus then only one is displayed and replicated for all the signals.
                    err = DrawOutputEnable(reservedLoc, indexOfLeftMostBit, 1, displayOutputs);
                    if (err.IsNotOk) return err;
                }
                if (pinTypeData.DigitalOutputUsed)
                {
                    err = DrawDigitalOutput(reservedLoc, indexOfLeftMostBit, width, displayOutputs);
                    if (err.IsNotOk) return err;
                }

                RectangleF bodyBounds = DrawPinBody(reservedLoc);
                if (drawAsSinglePin)
                {
                    DrawInstanceName(pinTypeData.LeftSideUsed, reservedLoc);
                }
                if (showIndex)
                {
                    //This index is displayed for all pins that represent a single pin (i.e. not bus)
                    DrawIndexInPinBody(bodyBounds, indexOfLeftMostBit);
                }

                err = DrawSIOBorder(pinTypeData, bodyBounds);
                if (err.IsNotOk) return err;

                return CyCustErr.Ok;
            }

            RectangleF m_sioFirstBounds = RectangleF.Empty;
            private CyCustErr DrawSIOBorder(CyPinTypeData pinTypeData, RectangleF bodyBounds)
            {
                if (pinTypeData.IsSIO)
                {
                    RectangleF sioBounds = bodyBounds;
                    sioBounds.Inflate(SIOBorderInflation, SIOBorderInflation);

                    if (pinTypeData.IsFirstInSIOPair)
                    {
                        m_sioFirstBounds = sioBounds;
                    }
                    else
                    {
                        if (pinTypeData.IsSecondInSIOPair)
                        {
                            Debug.Assert(m_sioFirstBounds.IsEmpty == false);
                            sioBounds = RectangleF.Union(sioBounds, m_sioFirstBounds);
                            m_sioFirstBounds = RectangleF.Empty;
                        }

                        m_shapeEdit.CreateRectangle(SIOTags, sioBounds.Location, sioBounds.Width,
                           sioBounds.Height);
                    }
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawSIOVRef(PointF reservedLoc, int index, int width)
            {
                /*
                 * -------------|
                 * |  --------  |
                 * |  |      |  |
                 * |  |      |  |
                 * |  |      |  |
                 * |  --------  |
                 * --------------
                 *      start
                 *        |
                 *        |
                 *      middle-----------end
                 */

                PointF end;
                CyCustErr err;
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                PointF start = new PointF(pinBodyBounds.Left + pinBodyBounds.Width / 2,
                    pinBodyBounds.Bottom + SIOBorderInflation);
                PointF middle = new PointF(start.X, reservedLoc.Y + BottomYDelta);
                end = new PointF(reservedLoc.X + 2.5f * SingleReservedSize - m_shapeEdit.UserBaseUnit, middle.Y);

                err = m_shapeEdit.CreatePolyline(AnalogTags, start, middle, end);
                if (err.IsNotOk) return err;

                //Add Terminal
                err = m_termEdit.TerminalAdd(
                    GetTerminalName(CyTermInfo.RootTermName_SIOVRef, index, width), CyCompDevTermDir.INOUT,
                    CyCompDevTermType.ANALOG, new PointF(end.X + m_shapeEdit.UserBaseUnit, end.Y), string.Empty,
                    180, false);
                if (err.IsNotOk) return err;

                if (width > 1)
                {
                    AddTermWidthLabel(end, new PointF(end.X + m_shapeEdit.UserBaseUnit, end.Y), width);
                }


                return CyCustErr.Ok;
            }

            private CyCustErr DrawDigitalOutput(PointF reservedLoc, int index, int width, string displayOutputs)
            {
                /*                          _________
                 *                          |       |
                 *   start---------------end|       |
                 *                          |       |
                 *                          ---------
                 */

                CyCustErr err;
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                PointF start = new PointF(reservedLoc.X - 1.5f * SingleReservedSize,
                                          reservedLoc.Y + TopYDelta);

                PointF end = new PointF(pinBodyBounds.Left, start.Y);

                //If the terminal is not drawn, fill in the area with the line so that the term width
                //label has a place to go.
                if (displayOutputs == CyPortConstants.Display_FALSE)
                {
                    err = m_shapeEdit.CreateLine(DigitalTags,
                        new PointF(start.X, start.Y),
                        new PointF(end.X + m_shapeEdit.UserBaseUnit * .25f, end.Y));
                }
                else
                {
                    err = m_shapeEdit.CreateLine(DigitalTags,
                       new PointF(start.X + m_shapeEdit.UserBaseUnit, start.Y),
                       new PointF(end.X + m_shapeEdit.UserBaseUnit * .25f, end.Y));
                }
                if (err.IsNotOk) return err;

                float leftX = start.X + SingleReservedSize;
                float rightX = leftX + m_shapeEdit.UserBaseUnit / 2;
                float halfArrow = (rightX - leftX) / 2;

                err = m_shapeEdit.CreatePolyline(ArrowLineTags,
                    new PointF(rightX, end.Y),
                    new PointF(leftX, end.Y + halfArrow),
                    new PointF(leftX, end.Y - halfArrow));
                if (err.IsNotOk) return err;

                err = m_shapeEdit.ShapesConvertToClosed(ArrowTags, ArrowLineTags);
                if (err.IsNotOk) return err;

                err = m_shapeEdit.SetFillColor(ArrowTag, PinBodyFillColor);
                if (err.IsNotOk) return err;

                err = m_shapeEdit.SetOutlineColor(ArrowTag, PinBodyOutlineColor);
                if (err.IsNotOk) return err;

                if (displayOutputs == CyPortConstants.Display_TRUE)
                {
                    err = m_termEdit.TerminalAdd(GetTerminalName(CyTermInfo.RootTermName_Output, index, width),
                        CyCompDevTermDir.INPUT, CyCompDevTermType.DIGITAL,
                        new PointF(start.X, start.Y), string.Empty, 0, false);
                    if (err.IsNotOk) return err;
                }

                if (width > 1)
                {
                    AddTermWidthLabel(start, new PointF(start.X + m_shapeEdit.UserBaseUnit, start.Y), width);
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawOutputEnable(PointF reservedLoc, int index, int width, string displayOutputs)
            {
                /*                 _________
                 *         start   |       | 
                 *         |       |       |
                 *   end--- middle |       |
                 *                 |       |
                 *                 ---------    
                 */

                CyCustErr err;
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                PointF start = new PointF(reservedLoc.X - 0.25f * SingleReservedSize - 0.25f * m_shapeEdit.UserBaseUnit,
                    reservedLoc.Y + TopYDelta);
                PointF middle = new PointF(start.X, reservedLoc.Y + BottomYDelta);
                PointF end = new PointF(reservedLoc.X - 1.5f * SingleReservedSize + m_shapeEdit.UserBaseUnit, middle.Y);

                //If the terminal is not drawn, fill in the area with the line so that the term width
                //label has a place to go.
                if (displayOutputs == CyPortConstants.Display_FALSE)
                {
                    err = m_shapeEdit.CreatePolyline(DigitalTags, start, middle,
                        new PointF(end.X - m_shapeEdit.UserBaseUnit, end.Y));
                }
                else
                {
                    err = m_shapeEdit.CreatePolyline(DigitalTags, start, middle, end);
                }
                if (err.IsNotOk) return err;

                if (displayOutputs == CyPortConstants.Display_TRUE)
                {
                    err = m_termEdit.TerminalAdd(GetTerminalName(CyTermInfo.RootTermName_OE, index, width),
                        CyCompDevTermDir.INPUT, CyCompDevTermType.DIGITAL,
                        new PointF(end.X - m_shapeEdit.UserBaseUnit, end.Y), string.Empty, 0, false);
                    if (err.IsNotOk) return err;
                }

                if (width > 1)
                {
                    AddTermWidthLabel(new PointF(end.X - m_shapeEdit.UserBaseUnit, end.Y), end, width);
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawDigitalInput(PointF reservedLoc, int index, int width, string displayInputs)
            {
                /*   _________
                 *   |       |
                 *   |       |start---------------end
                 *   |       |
                 *   ---------
                 */

                CyCustErr err;
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);
                PointF start = new PointF(pinBodyBounds.Right, reservedLoc.Y + TopYDelta);
                PointF end = new PointF(reservedLoc.X + 2.5f * SingleReservedSize, start.Y);

                //If the terminal is not drawn, fill in the area with the line so that the term width
                //label has a place to go.
                if (displayInputs == CyPortConstants.Display_FALSE)
                {
                    err = m_shapeEdit.CreateLine(DigitalTags, start, new PointF(end.X, end.Y));
                }
                else
                {
                    err = m_shapeEdit.CreateLine(DigitalTags, start,
                        new PointF(end.X - m_shapeEdit.UserBaseUnit, end.Y));
                }
                if (err.IsNotOk) return err;

                float leftX = end.X - SingleReservedSize;
                float rightX = leftX + m_shapeEdit.UserBaseUnit / 2;
                float halfArrow = (rightX - leftX) / 2;

                err = m_shapeEdit.CreatePolyline(ArrowLineTags,
                    new PointF(rightX, end.Y),
                    new PointF(leftX, end.Y + halfArrow),
                    new PointF(leftX, end.Y - halfArrow));
                if (err.IsNotOk) return err;

                err = m_shapeEdit.ShapesConvertToClosed(ArrowTags, ArrowLineTags);
                if (err.IsNotOk) return err;

                err = m_shapeEdit.SetFillColor(ArrowTag, PinBodyFillColor);
                if (err.IsNotOk) return err;

                err = m_shapeEdit.SetOutlineColor(ArrowTag, PinBodyOutlineColor);
                if (err.IsNotOk) return err;

                if (displayInputs == CyPortConstants.Display_TRUE)
                {
                    err = m_termEdit.TerminalAdd(GetTerminalName(CyTermInfo.RootTermName_Input, index, width),
                        CyCompDevTermDir.OUTPUT, CyCompDevTermType.DIGITAL,
                        new PointF(end.X, end.Y), string.Empty, 0, false);
                    if (err.IsNotOk) return err;
                }

                if (width > 1)
                {
                    AddTermWidthLabel(new PointF(end.X - m_shapeEdit.UserBaseUnit, end.Y), end, width);
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawAnalog(PointF reservedLoc, bool digitalInputUsed, int index, int width)
            {
                PointF end;
                CyCustErr err;
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                if (digitalInputUsed)
                {
                    /*   _________
                     *   |       |
                     *   |       |     start
                     *   |       |         |
                     *   ---------   middle ----------end
                     * 
                     */

                    PointF start = new PointF(pinBodyBounds.Right + m_shapeEdit.UserBaseUnit * 0.5f,
                        reservedLoc.Y + TopYDelta);
                    PointF middle = new PointF(start.X, reservedLoc.Y + BottomYDelta);
                    end = new PointF(reservedLoc.X + 2.5f * SingleReservedSize - m_shapeEdit.UserBaseUnit, middle.Y);

                    err = m_shapeEdit.CreatePolyline(AnalogTags, start, middle, end);
                    if (err.IsNotOk) return err;
                }
                else
                {
                    /*   _________
                     *   |       |
                     *   |       |start---------------end
                     *   |       |
                     *   ---------
                     */

                    PointF start = new PointF(pinBodyBounds.Right, reservedLoc.Y + TopYDelta);
                    end = new PointF(reservedLoc.X + 2.5f * SingleReservedSize - m_shapeEdit.UserBaseUnit, start.Y);

                    err = m_shapeEdit.CreateLine(AnalogTags, start, end);
                    if (err.IsNotOk) return err;
                }

                //Add Terminal
                err = m_termEdit.TerminalAdd(
                    GetTerminalName(CyTermInfo.RootTermName_Analog, index, width), CyCompDevTermDir.INOUT,
                    CyCompDevTermType.ANALOG,
                    new PointF(end.X + m_shapeEdit.UserBaseUnit, end.Y), string.Empty, 180, false);
                if (err.IsNotOk) return err;

                if (width > 1)
                {
                    AddTermWidthLabel(end, new PointF(end.X + m_shapeEdit.UserBaseUnit, end.Y), width);
                }

                return CyCustErr.Ok;
            }

            private CyCustErr DrawBidirectional(PointF reservedLoc, int index, int width)
            {
                /*                          _________
                 *                          |       |
                 *   start---------------end|       |
                 *                          |       |
                 *                          ---------
                 */

                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                PointF start = new PointF(reservedLoc.X - 1.5f * SingleReservedSize,
                                          reservedLoc.Y + TopYDelta);

                PointF end = new PointF(pinBodyBounds.Left, start.Y);

                //Draw Line to Body
                m_shapeEdit.CreateLine(DigitalTags, new PointF(start.X + m_shapeEdit.UserBaseUnit, start.Y), end);

                //Draw Arrows
                float leftX = start.X + 1.5f * m_shapeEdit.UserBaseUnit;
                float rightX = leftX + m_shapeEdit.UserBaseUnit;
                float middleX = leftX + (rightX - leftX) / 2;
                float arrowHeight = m_shapeEdit.UserBaseUnit * .25f;

                m_shapeEdit.CreatePolyline(ArrowLineTags,
                    new PointF(rightX, start.Y),
                    new PointF(middleX, start.Y + arrowHeight),
                    new PointF(middleX, start.Y - arrowHeight));
                m_shapeEdit.ShapesConvertToClosed(ArrowTags, ArrowLineTags);

                m_shapeEdit.CreatePolyline(ArrowLineTags,
                    new PointF(leftX, start.Y),
                    new PointF(middleX, start.Y + arrowHeight),
                    new PointF(middleX, start.Y - arrowHeight));
                m_shapeEdit.ShapesConvertToClosed(ArrowTags, ArrowLineTags);

                m_shapeEdit.SetFillColor(ArrowTag, PinBodyFillColor);
                m_shapeEdit.SetOutlineColor(ArrowTag, PinBodyOutlineColor);

                //Add Terminal
                string termName = GetTerminalName(CyTermInfo.RootTermName_BiDir, index, width);
                m_termEdit.TerminalAdd(termName,
                    CyCompDevTermDir.INOUT, CyCompDevTermType.DIGITAL,
                    new PointF(start.X, start.Y), string.Empty, 0, false);
                CyCustErr err = m_termEdit.SetIsBurriedPin(termName, true);
                if (err.IsNotOk) return err;

                if (width > 1)
                {
                    AddTermWidthLabel(start, new PointF(start.X + m_shapeEdit.UserBaseUnit, start.Y), width);
                }

                return CyCustErr.Ok;
            }

            private void AddTermWidthLabel(PointF startTerm, PointF endTerm, int width)
            {
                Debug.Assert(width > 1);
                Debug.Assert(startTerm.Y == endTerm.Y);
                PointF centerTerm = new PointF(startTerm.X + (endTerm.X - startTerm.X) / 2, startTerm.Y);

                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Far;
                fmt.Alignment = StringAlignment.Center;

                m_shapeEdit.CreateAnnotation(TextTags, width.ToString(), centerTerm,
                    new Font(FontFamily.GenericSansSerif, DecorativeTextSize, FontStyle.Regular), fmt);

                float quarterUnit = m_shapeEdit.UserBaseUnit / 4;
                m_shapeEdit.CreateLine(new string[] { "WidthLine" },
                    new PointF(centerTerm.X + quarterUnit, centerTerm.Y - quarterUnit),
                    new PointF(centerTerm.X - quarterUnit, centerTerm.Y + quarterUnit));
            }

            private void DrawIndexInPinBody(RectangleF reservedBounds, int index)
            {
                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Center;
                fmt.Alignment = StringAlignment.Center;

                PointF center = new PointF(
                    reservedBounds.X + reservedBounds.Width / 2,
                    reservedBounds.Y + reservedBounds.Height / 2);

                string uniqueTag = Guid.NewGuid().ToString();
                m_shapeEdit.CreateAnnotation(new string[] { IndexTag, uniqueTag }, index.ToString(), center,
                    new Font(FontFamily.GenericSansSerif, DecorativeTextSize, FontStyle.Regular), fmt);
                m_shapeEdit.ClearOutline(IndexTag);
                m_shapeEdit.SetFillColor(IndexTag, IndexFillColor);
                RectangleF bounds = m_shapeEdit.GetShapeBounds(uniqueTag);

                m_shapeEdit.CreateRectangle(new string[] { "background" },
                    bounds.Location, bounds.Width, bounds.Height);
                m_shapeEdit.ClearOutline("background");
                m_shapeEdit.SetFillColor("background", Color.White);

                m_shapeEdit.BringToFront(uniqueTag);
            }

            private void DrawInstanceName(bool rightSide, PointF reservedLoc)
            {
                PointF loc;
                StringFormat fmt = new StringFormat();
                fmt.LineAlignment = StringAlignment.Center;

                if (rightSide)
                {
                    loc = new PointF(reservedLoc.X + SingleReservedSize, reservedLoc.Y + TopYDelta);
                    fmt.Alignment = StringAlignment.Near;
                }
                else
                {
                    loc = new PointF(reservedLoc.X, reservedLoc.Y + TopYDelta);
                    fmt.Alignment = StringAlignment.Far;
                }

                DrawInstanceName(loc, fmt);
            }

            private void DrawInstanceName(PointF loc, StringFormat fmt)
            {
                m_shapeEdit.CreateAnnotation(TextTags, m_query.InstanceName, loc,
                        new Font(FontFamily.GenericSansSerif, OtherTextSize, FontStyle.Regular), fmt);
            }

            private RectangleF DrawPinBody(PointF reservedLoc)
            {
                RectangleF pinBodyBounds = GetPinBodyBounds(reservedLoc);

                //Draw Pin Body
                m_shapeEdit.CreateRectangle(PinBodyTags, pinBodyBounds.Location,
                    pinBodyBounds.Width, pinBodyBounds.Height);
                m_shapeEdit.SetFillColor(PinBodyTag, PinBodyFillColor);
                m_shapeEdit.SetOutlineColor(PinBodyTag, PinBodyOutlineColor);

                //Draw X in Pin Body
                PointF leftTop = new PointF(pinBodyBounds.Left, pinBodyBounds.Top);
                PointF leftBottom = new PointF(pinBodyBounds.Left, pinBodyBounds.Bottom);
                PointF rightTop = new PointF(pinBodyBounds.Right, pinBodyBounds.Top);
                PointF rightBottom = new PointF(pinBodyBounds.Right, pinBodyBounds.Bottom);
                m_shapeEdit.CreateLine(PinXTags, leftTop, rightBottom);
                m_shapeEdit.CreateLine(PinXTags, rightTop, leftBottom);
                m_shapeEdit.SetOutlineColor(PinXTag, PinBodyOutlineColor);

                return pinBodyBounds;
            }

            #endregion

            #region CyPinTypeData Class

            class CyPinTypeData
            {
                public readonly bool AnalogUsed = false;
                public readonly bool DigitalInputUsed = false;
                public readonly bool DigitalOutputUsed = false;
                public readonly bool BidirectionalUsed = false;
                public readonly bool OutputEnableUsed = false;
                public readonly bool SIOVRefUsed = false;

                public readonly bool IsSingleSIO = false;
                public readonly bool IsFirstInSIOPair = false;
                public readonly bool IsSecondInSIOPair = false;

                public bool IsSIO
                {
                    get
                    {
                        return IsSingleSIO || IsFirstInSIOPair || IsSecondInSIOPair;
                    }
                }

                public bool RightSideUsed
                {
                    get { return AnalogUsed || DigitalInputUsed || SIOVRefUsed; }
                }

                public bool LeftSideUsed
                {
                    //We don't need to check OE because it cannot be used without using Digital Output.
                    get { return DigitalOutputUsed || BidirectionalUsed; }
                }

                public bool OnlyOneSideUsed
                {
                    get { return RightSideUsed ^ LeftSideUsed; }
                }

                public int NumTerminalsTall
                {
                    get
                    {
                        if (SIOVRefUsed) return 2;
                        if (AnalogUsed && DigitalInputUsed) return 2;
                        if (DigitalOutputUsed && OutputEnableUsed) return 2;
                        return 1;
                    }
                }

                public static CyCustErr CreatePinTypeData(string pinType, string sioInfo, string sioRefSel,
                    out CyPinTypeData data)
                {
                    CyCustErr err;
                    data = new CyPinTypeData(pinType, sioInfo, sioRefSel, out err);
                    return err;
                }

                CyPinTypeData(string pinType, string sioInfo, string sioRefSel, out CyCustErr err)
                {
                    err = CyCustErr.Ok;

                    switch (sioInfo)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            break;

                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            IsFirstInSIOPair = true;
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            IsSingleSIO = true;
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            IsSecondInSIOPair = true;
                            break;

                        default:
                            err = new CyCustErr(string.Format(Resource1.UnhandledSIOInfo, sioInfo));
                            return;
                    }

                    if (sioInfo == CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR ||
                        sioInfo == CyPortConstants.SIOInfoValue_SINGLE_SIO)
                    {
                        switch (sioRefSel)
                        {
                            case CyPortConstants.SIORefSelValue_VCC_IO:
                                break;

                            case CyPortConstants.SIORefSelValue_VOHREF:
                                SIOVRefUsed = true;
                                break;

                            default:
                                err = new CyCustErr(string.Format(Resource1.UnhandledSIORefSel, sioRefSel));
                                return;
                        }
                    }

                    switch (pinType)
                    {
                        case CyPortConstants.PinTypesValue_ANALOG:
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                            BidirectionalUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGIN:
                            DigitalInputUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                            DigitalInputUsed = true;
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT:
                            DigitalOutputUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                            DigitalOutputUsed = true;
                            DigitalInputUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                            DigitalOutputUsed = true;
                            DigitalInputUsed = true;
                            OutputEnableUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_OE:
                            DigitalOutputUsed = true;
                            OutputEnableUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                            DigitalOutputUsed = true;
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                            BidirectionalUsed = true;
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                            DigitalOutputUsed = true;
                            DigitalInputUsed = true;
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                            DigitalOutputUsed = true;
                            OutputEnableUsed = true;
                            AnalogUsed = true;
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                            DigitalOutputUsed = true;
                            OutputEnableUsed = true;
                            DigitalInputUsed = true;
                            AnalogUsed = true;
                            break;

                        default:
                            err = new CyCustErr(string.Format(Resource1.UnhandledPinType, pinType));
                            break;
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Unit Tests

        public static void RunUnitTest(ICyInstEdit_v1 instEdit)
        {
            CyPortCustomizer.BaseRunUnitTest(instEdit);
        }

        #endregion
    }
}

namespace Cypress.Comps.PinsAndPorts.Common_v1_20
{
    #region Constants

    public class CyPortConstants
    {
        public const string Display_FALSE = "0";
        public const string Display_TRUE = "1";
        public const string DefaultDisplay = Display_TRUE;

        public const string VTripValue_CMOS = "00";
        public const string VTripValue_LVTTL = "01";
        public const string VTripValue_CMOS_OR_LVTTL = "10";
        public const string DefaultVTripValue = VTripValue_CMOS;

        public const string InputsSynchronizedValue_DISABLED = "0";
        public const string InputsSynchronizedValue_ENABLED = "1";
        public const string DefaultInputsSynchronizedValue = InputsSynchronizedValue_ENABLED;

        public const string OutputsSynchronizedValue_DISABLED = "0";
        public const string OutputsSynchronizedValue_ENABLED = "1";
        public const string DefaultOutputsSynchronizedValue = OutputsSynchronizedValue_DISABLED;

        //The default value us dependant on the DriveMode. (ResPullUp or ResPullUpDown = 1, others = 0).
        public const string InitialDriveStateValue_LOW = "0";
        public const string InitialDriveStateValue_HIGH = "1";

        public const string SIOVTripValue_PT5 = "0";
        public const string SIOVTripValue_PT4_OR_ONE = "1";

        public const string SIORefSelValue_VCC_IO = "0";
        public const string SIORefSelValue_VOHREF = "1";
        public const string DefaultSIORefSelValue = SIORefSelValue_VCC_IO;

        public const string SIOInputBufferValue_SINGLE_ENDED = "0";
        public const string SIOInputBufferValue_DIFFERENTIAL = "1";

        public const string SIOOutputBufferValue_UNREGULATED = "0";
        public const string SIOOutputBufferValue_REGULATED = "1";

        public const string DriveCurrentValue_4SOURCE_8SINK = "0";
        public const string DriveCurrentValue_4SOURCE_25SINK = "1";
        public const string DefaultDriveCurrentValue = DriveCurrentValue_4SOURCE_8SINK;

        public const string OutputDriveLevelValue_VDDIO = "0";
        public const string OutputDriveLevelValue_VREF = "1";
        public const string DefaultOutputDriveLevelValue = OutputDriveLevelValue_VDDIO;

        public const string HotSwapValue_FALSE = "0";
        public const string HotSwapValue_TRUE = "1";
        public const string DefaultHotSwapValue = HotSwapValue_FALSE;

        public const string SIOInfoValue_NOT_SIO = "00";
        public const string SIOInfoValue_SINGLE_SIO = "01";
        public const string SIOInfoValue_FIRST_IN_SIO_PAIR = "10";
        public const string SIOInfoValue_SECOND_IN_SIO_PAIR = "11";
        public const string DefaultSIOInfoValue = SIOInfoValue_NOT_SIO;

        public const string SIOGroupValue_NOT_GROUPED = "0";
        public const string SIOGroupValue_GROUPED = "1";
        public const string DefaultSIOGroupValue = SIOGroupValue_NOT_GROUPED;

        public const string SlewRateValue_FAST = "0";
        public const string SlewRateValue_SLOW = "1";
        public const string DefaultSlewRateValue = SlewRateValue_FAST;

        public const string InterruptModeValue_NONE = "00";
        public const string InterruptModeValue_RISING_EDGE = "01";
        public const string InterruptModeValue_FALLING_EDGE = "10";
        public const string InterruptModeValue_ON_CHANGE = "11";
        public const string DefaultInterruptModeValue = InterruptModeValue_NONE;

        public const string ThresholdLevelValue_CMOS = "0000";
        public const string ThresholdLevelValue_LVTTL = "0001";
        public const string ThresholdLevelValue_CMOS_LVTTL = "0010";
        public const string ThresholdLevelValue_PT5_VDDIO = "0011";
        public const string ThresholdLevelValue_PT4_VDDIO = "0100";
        public const string ThresholdLevelValue_PT5_VREF = "0101";
        public const string ThresholdLevelValue_VREF = "0110";
        public const string ThresholdLevelValue_PT5_VDDIO_HYST = "0111";
        public const string ThresholdLevelValue_PT4_VDDIO_HYST = "1000";
        public const string ThresholdLevelValue_PT5_VREF_HYST = "1001";
        public const string ThresholdLevelValue_VREF_HYST = "1010";
        public const string DefaultThresholdLevelValue = ThresholdLevelValue_CMOS;

        //The default value us dependant on the PinType.
        public const string DriveModeValue_ANALOG_HI_Z = "000"; //High Impedance Analog
        public const string DriveModeValue_DIGITAL_HI_Z = "001"; //High Impedance Digital
        public const string DriveModeValue_RES_PULL_UP = "010";
        public const string DriveModeValue_RES_PULL_DOWN = "011";
        public const string DriveModeValue_OPEN_DRAIN_LO = "100";
        public const string DriveModeValue_OPEN_DRAIN_HI = "101";
        public const string DriveModeValue_CMOS_OUT = "110"; //Strong Drive
        public const string DriveModeValue_RES_PULL_UP_DOWN = "111";

        public const string LCDComSegValue_FALSE = "0";
        public const string LCDComSegValue_TRUE = "1";
        public const string DefaultLCDComSegValue = LCDComSegValue_FALSE;

        //The default value is dependant on the PinType. (Input or BiDir used = 1, others = 0).
        public const string InputBufferEnabledValue_FALSE = "0";
        public const string InputBufferEnabledValue_TRUE = "1";

        public const string EnableShieldingValue_FALSE = "0";
        public const string EnableShieldingValue_TRUE = "1";
        public const string DefaultEnableShieldingValue = EnableShieldingValue_FALSE;

        public const string SIOHysteresisValue_FALSE = "0";
        public const string SIOHysteresisValue_TRUE = "1";
        public const string DefaultSIOHysteresisValue = SIOHysteresisValue_FALSE;

        public const string LayoutModeValue_CONT_SPANNING = "1";
        public const string LayoutModeValue_CONT_NONSPANNING = "2";
        public const string LayoutModeValue_NONCONT_SPANNING = "3";
        public const string LayoutModeValue_NONCONT_NONSPANNING = "4";

        public const string PinTypesValue_DIGIN = "0000";
        public const string PinTypesValue_DIGOUT = "0001";
        public const string PinTypesValue_ANALOG = "0010";
        public const string PinTypesValue_BIDIRECTIONAL = "0011";
        public const string PinTypesValue_DIGIN_ANALOG = "0100";
        public const string PinTypesValue_DIGOUT_DIGIN = "0101";
        public const string PinTypesValue_DIGOUT_OE = "0110";
        public const string PinTypesValue_DIGOUT_DIGIN_OE = "0111";
        public const string PinTypesValue_DIGOUT_ANALOG = "1000";
        public const string PinTypesValue_BIDIRECTIONAL_ANALOG = "1001";
        public const string PinTypesValue_DIGOUT_DIGIN_ANALOG = "1010";
        public const string PinTypesValue_DIGOUT_OE_ANALOG = "1011";
        public const string PinTypesValue_DIGOUT_OE_DIGIN_ANALOG = "1100";
        public const string DefaultPinTypesValue = PinTypesValue_DIGIN;

        public const string DefaultIOVolatageValue = "None";


        public static bool IsBidir(string pinType)
        {
            switch (pinType)
            {
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    return true;

                case CyPortConstants.PinTypesValue_ANALOG:
                case CyPortConstants.PinTypesValue_DIGIN:
                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT:
                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    return false;

                default:
                    return false;
            }
        }

        public static bool IsOE(string pinType)
        {
            switch (pinType)
            {
                case CyPortConstants.PinTypesValue_ANALOG:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                case CyPortConstants.PinTypesValue_DIGIN:
                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT:
                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    return false;

                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsOutput(string pinType)
        {
            switch (pinType)
            {
                case CyPortConstants.PinTypesValue_ANALOG:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                case CyPortConstants.PinTypesValue_DIGIN:
                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                    return false;

                case CyPortConstants.PinTypesValue_DIGOUT:
                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsInput(string pinType)
        {
            switch (pinType)
            {
                case CyPortConstants.PinTypesValue_DIGIN:
                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    return true;

                case CyPortConstants.PinTypesValue_ANALOG:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT:
                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                    return false;

                default:
                    return false;
            }
        }

        public static bool IsAnalog(string pinType)
        {
            switch (pinType)
            {
                case CyPortConstants.PinTypesValue_ANALOG:
                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    return true;

                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                case CyPortConstants.PinTypesValue_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    return false;

                default:
                    return false;
            }
        }
    }

    #endregion

    public abstract class CyPortCustomizer :
        ICyExprEval_v1,
        ICyVerilogCustomize_v1,
        ICyInstValidateHook_v1,
        ICyPinDataProvider_v2,
        ICyToolTipCustomize_v1,
        ICyAPICustomize_v1
    {
        //-----------------------------

        #region Helper Methods

        public static CyCustErr GetParamValue<T>(ICyInstQuery_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnknownParam, paramName));
            }

            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }

            if (param.ErrorCount != 0)
            {
                err = new CyCustErr(param.ErrorMsgs);
            }

            return err;
        }

        public static CyCustErr GetParamValue<T>(ICyInstValidate_v1 query, string paramName, out T value)
        {
            CyCustErr err = CyCustErr.Ok;
            value = default(T);

            CyCompDevParam param = query.GetCommittedParam(paramName);
            if (param == null)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnknownParam, paramName));
            }

            err = param.TryGetValueAs<T>(out value);
            if (err.IsNotOk)
            {
                value = default(T);
                return err;
            }

            if (param.ErrorCount != 0)
            {
                err = new CyCustErr(param.ErrorMsgs);
            }

            return err;
        }

        protected CyCustErr SetTerminalsWidth(ICyTerminalEdit_v1 termEdit, IEnumerable<string> termBaseNames,
                                              byte width)
        {
            CyCustErr err = CyCustErr.Ok;
            foreach (string termBaseName in termBaseNames)
            {
                string termFullName = termEdit.GetTermName(termBaseName);
                err = SetTerminalWidth(termEdit, termFullName, termBaseName, width);
                if (err.IsNotOk) return err;
            }
            return err;
        }

        protected CyCustErr SetTerminalWidth(ICyTerminalEdit_v1 termEdit, string terminalFullName,
                                             string terminalBaseName, int width)
        {
            string newName;
            int leftIndex = width - 1;

            if (leftIndex == 0)
            {
                newName = terminalBaseName;
            }
            else
            {
                newName = string.Format("{0}[{1}:0]", terminalBaseName, leftIndex);
            }

            CyCustErr err = termEdit.TerminalRename(terminalFullName, newName);
            return err;
        }

        protected CyCustErr IsSIOBecauseOfHotSwap(string hotSwapString, string pinTypesString, int index,
            out bool isSIO)
        {
            isSIO = false;

            CyStringArrayParamData hotSwapData;
            CyCustErr err = CyStringArrayParamData.CreateInputOnlyBitVectorData(CyInputOnlyBitVectorParam.Formal_HotSwaps,
                hotSwapString, pinTypesString, out hotSwapData);
            if (err.IsNotOk) return err;

            isSIO = (hotSwapData.GetValue(index) == CyPortConstants.HotSwapValue_TRUE);
            return CyCustErr.Ok;
        }

        protected CyCustErr IsSIOBecauseOfThresholdLevels(string thresholdLevelString, string pinTypesString,
            int index, out bool isSIO)
        {
            isSIO = false;

            CyStringArrayParamData thresholdLevelData;
            CyCustErr err = CyStringArrayParamData.CreateInputOnlyBitVectorData(
                CyInputOnlyBitVectorParam.Formal_ThresholdLevels, thresholdLevelString, pinTypesString, out thresholdLevelData);
            if (err.IsNotOk) return err;

            string thresholdLevelValue = thresholdLevelData.GetValue(index);
            switch (thresholdLevelValue)
            {
                case CyPortConstants.ThresholdLevelValue_CMOS:
                case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                case CyPortConstants.ThresholdLevelValue_LVTTL:
                    break;

                case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                case CyPortConstants.ThresholdLevelValue_VREF:
                case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                    isSIO = true;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, thresholdLevelValue));
            }

            return CyCustErr.Ok;
        }

        protected CyCustErr IsSIOBecauseOfOutputDriveLevels(string outputDriveLevelString, string pinTypesString,
            int index, out bool isSIO)
        {
            isSIO = false;

            CyStringArrayParamData outputDriveLevelData;
            CyCustErr err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(
                CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels, outputDriveLevelString, pinTypesString,
                out outputDriveLevelData);
            if (err.IsNotOk) return err;

            isSIO = (outputDriveLevelData.GetValue(index) == CyPortConstants.OutputDriveLevelValue_VREF);
            return CyCustErr.Ok;
        }

        protected CyCustErr IsSIOBecauseOfDriveCurrents(string driveCurrentString, string pinTypesString,
            int index, out bool isSIO)
        {
            isSIO = false;

            CyStringArrayParamData driveCurrentsData;
            CyCustErr err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(
                CyOutputOnlyBitVectorParam.Formal_DriveCurrents, driveCurrentString, pinTypesString, out driveCurrentsData);
            if (err.IsNotOk) return err;

            isSIO = (driveCurrentsData.GetValue(index) == CyPortConstants.DriveCurrentValue_4SOURCE_25SINK);
            return CyCustErr.Ok;
        }

        protected CyCustErr IsSIO(string hotSwapString, string thresholdLevelString, string outputDriveLevelString,
            string driveCurrentString, string pinTypesString, int index, out bool isSIO)
        {
            /* 
             * A pin is SIO if any one of the following is true:
             * 1. HotSwaps(index) is true
             * 2. ThresholdLevels(index) is one of the following (i.e. not CMOS or LVTTL):
             *    0.5 * Vddio
             *    0.4 * Vddio
             *    0.5 * Vref
             *    Vref
             *    0.5 * Vddio with hysteresis
             *    0.4 * Vddio with hysteresis
             *    0.5 * Vref with hysteresis
             *    Vref with hysteresis
             * 3. OutputDriveLevels(index) is Vref
             * 4. DriveCurrents(index) is 4mA source, 25mA sink
             * 
             */

            CyCustErr err;
            isSIO = false;

            err = IsSIOBecauseOfHotSwap(hotSwapString, pinTypesString, index, out isSIO);
            if (err.IsNotOk) return err;
            if (isSIO) return CyCustErr.Ok;

            err = IsSIOBecauseOfThresholdLevels(thresholdLevelString, pinTypesString, index, out isSIO);
            if (err.IsNotOk) return err;
            if (isSIO) return CyCustErr.Ok;

            err = IsSIOBecauseOfOutputDriveLevels(outputDriveLevelString, pinTypesString, index, out isSIO);
            if (err.IsNotOk) return err;
            if (isSIO) return CyCustErr.Ok;

            err = IsSIOBecauseOfDriveCurrents(driveCurrentString, pinTypesString, index, out isSIO);
            if (err.IsNotOk) return err;
            if (isSIO) return CyCustErr.Ok;

            //Must not be SIO.
            isSIO = false;
            return CyCustErr.Ok;
        }

        public static string GetPinPoundDefineName(ICyInstQuery_v1 query, int index)
        {
            CyCustErr err;
            CyStringArrayParamData data;
            err = CyStringArrayParamData.CreatePinAliasData(query, true, out data);
            if (data == null)
            {
                data = new CyStringArrayParamData.CyAliasParamData(string.Empty);
            }

            string instName;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Builtin_ParamName_InstName, out instName);
            //Errors don't really matter for creating the data so they can be ignored here.

            string alias = data.GetValue(index);
            if (string.IsNullOrEmpty(alias))
            {
                return string.Format("{0}_{1}", instName, index);
            }
            return string.Format("{0}_{1}", instName, alias);
        }

        #endregion

        //-----------------------------

        #region ICyExprEval_v1 Members

        public virtual CyExprEvalFunc GetFunction(string exprFuncName)
        {
            switch (exprFuncName)
            {
                //`=UsesInterrupts($InterruptModes, $PinTypes, $NumPins)`
                case "UsesInterrupts":
                    return new CyExprEvalFunc(UsesInterrupts);

                //`=IsHomogeneous_v2($PinTypes, $sio_info, $sio_refsel, $DisplayOutputHWConnections, 
                //$DisplayInputHWConnections, $NumPins, $sio_group_cnt)`
                case "IsHomogeneous_v2":
                    return new CyExprEvalFunc(IsHomogeneous_v2);

                //`=CreatePinModeString($PinTypes, $NumPins)`
                case "CreatePinModeString":
                    return new CyExprEvalFunc(CreatePinModeString);

                //`=CreateInputBufferBitVector($ThresholdLevels, $sio_info, $PinTypes, $NumPins)`
                case "CreateInputBufferBitVector":
                    return new CyExprEvalFunc(CreateInputBufferBitVector);

                //`=CreateHysteresisCSV($ThresholdLevels, $sio_info, $PinTypes, $NumPins)`
                case "CreateHysteresisCSV":
                    return new CyExprEvalFunc(CreateHysteresisCSV);

                //`=CalculateSIOGroupCount($sio_info, $width)`
                case "CalculateSIOGroupCount":
                    return new CyExprEvalFunc(CalculateSIOGroupCount);

                //`=CreateSIOInfoCSV($SIOGroups, $HotSwaps, $ThresholdLevels, $OutputDriveLevels, 
                //                   $DriveCurrents, $PinTypes, $width)`
                case "CreateSIOInfoCSV":
                    return new CyExprEvalFunc(CreateSIOInfoCSV);

                //`=CreateOutputBufferBitVector($OutputDriveLevels, $sio_info, $PinTypes, $width)`
                case "CreateOutputBufferBitVector":
                    return new CyExprEvalFunc(CreateOutputBufferBitVector);

                //`=CreateSIORefSelBitVector($OutputDriveLevels, $ThresholdLevels, $sio_info, $PinTypes, $width)`
                case "CreateSIORefSelBitVector":
                    return new CyExprEvalFunc(CreateSIORefSelCSV);

                //`=CreateSIOVTripBitVector($ThresholdLevels, $sio_info, $PinTypes, $width)`
                case "CreateSIOVTripBitVector":
                    return new CyExprEvalFunc(CreateSIOVTripBitVector);

                //`=CreateVTripCSV($ThresholdLevels, $PinTypes, $width)`
                case "CreateVTripCSV":
                    return new CyExprEvalFunc(CreateVTripCSV);

                //`=CreateDriveModesCSV($DriveModes, $PinTypes, $NumPins)`
                case "CreateDriveModesCSV":
                    return new CyExprEvalFunc(CreateDriveModesCSV);

                //`=CreateInputBufferesEnabledCSV($InputBuffersEnabled, $PinTypes, $NumPins)`
                case "CreateInputBufferesEnabledCSV":
                    return new CyExprEvalFunc(CreateInputBufferesEnabledCSV);

                //`=CreateInitialDriveStatesCSV($InitialDriveStates, $DriveModes, $PinTypes, $NumPins)`
                case "CreateInitialDriveStatesCSV":
                    return new CyExprEvalFunc(CreateInitialDriveStatesCSV);

                //`=CreateCSVFromBitVector(bitVectorParamName, bitVectorString, $NumPins)`
                case "CreateCSVFromBitVector":
                    return new CyExprEvalFunc(CreateCSVFromBitVector);

                //`=CreateIOVoltagesCSV($IOVoltages, $NumPins)`
                case "CreateIOVoltagesCSV":
                    return new CyExprEvalFunc(CreateIOVoltagesCSV);

                //`=CreatePinAliasesCSV($PinAliases, $NumPins)`
                case "CreatePinAliasesCSV":
                    return new CyExprEvalFunc(CreatePinAliasesCSV);

                //`=CreateCSVFromInputOnlyBitVector(bitVectorParamName, bitVectorString, $PinTypes, $NumPins)`
                case "CreateCSVFromInputOnlyBitVector":
                    return new CyExprEvalFunc(CreateCSVFromInputOnlyBitVector);

                //`=CreateCSVFromOutputOnlyBitVector(bitVectorParamName, bitVectorString, $PinTypes, $NumPins)`
                case "CreateCSVFromOutputOnlyBitVector":
                    return new CyExprEvalFunc(CreateCSVFromOutputOnlyBitVector);

                //`=CreateOutputConnVisibleBitVector($PinTypes, $DisplayOutputHWConnections, $NumPins)`
                case "CreateOutputConnVisibleBitVector":
                    return new CyExprEvalFunc(CreateOutputConnVisibleBitVector);

                default:
                    //Return null to indicate that the customizer didn't process the 
                    //function call so CyDesigner should continue trying to process it.
                    return null;
            }
        }

        protected CyCustErr WrapCustFunctionErr(string functName, string errorParam, string errorMsg)
        {
            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.CustFunctError, errorParam, functName, errorMsg));
        }

        object CreateSIOInfoCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 7)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_CreateSIOInfoCVS, exprFuncName));
            }
            else
            {
                CyCustErr err;
                List<string> sioInfo = new List<string>();

                string sioGroupsString = typeConverter.GetAsString(args[0]);
                string hotSwapString = typeConverter.GetAsString(args[1]);
                string thresholdLevelString = typeConverter.GetAsString(args[2]);
                string outputDriveLevelString = typeConverter.GetAsString(args[3]);
                string driveCurrentString = typeConverter.GetAsString(args[4]);
                string pinTypesString = typeConverter.GetAsString(args[5]);
                byte width = typeConverter.GetAsByte(args[6]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_SIOGroups, sioGroupsString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_SIOGroups, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_HotSwaps, hotSwapString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_HotSwaps, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);
                err = ValidateOutputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_OutputDriveLevels, outputDriveLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_OutputDriveLevels, err.Message);
                err = ValidateOutputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_DriveCurrents, driveCurrentString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DriveCurrents, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_SIOGroups, sioGroupsString, out data);
                if (err.IsNotOk) return err;

                bool hasSeenFirstInPair = false;
                for (int i = 0; i < width; i++)
                {
                    bool isSIO;
                    err = IsSIO(hotSwapString, thresholdLevelString, outputDriveLevelString, driveCurrentString,
                        pinTypesString, i, out isSIO);

                    if (err.IsNotOk) return err;

                    if (isSIO)
                    {
                        string sioGroupValue = data.GetValue(i);
                        switch (sioGroupValue)
                        {
                            case CyPortConstants.SIOGroupValue_NOT_GROUPED:
                                sioInfo.Add(CyPortConstants.SIOInfoValue_SINGLE_SIO);
                                break;

                            case CyPortConstants.SIOGroupValue_GROUPED:
                                if (hasSeenFirstInPair)
                                {
                                    sioInfo.Add(CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR);
                                    hasSeenFirstInPair = false;
                                }
                                else
                                {
                                    //Make sure the second part of the group is available. If not mark as a single.
                                    //It would not be available if:
                                    // 1. we are currently looking at the last pin
                                    // 2. the next pin is not SIO
                                    // 3. the next pin is not grouped
                                    bool nextPinIsSIO;
                                    err = IsSIO(hotSwapString, thresholdLevelString, outputDriveLevelString,
                                        driveCurrentString, pinTypesString, i + 1, out nextPinIsSIO);
                                    if (err.IsNotOk) return err;

                                    if (i == width - 1 || nextPinIsSIO == false ||
                                        data.GetValue(i + 1) != CyPortConstants.SIOGroupValue_GROUPED)
                                    {
                                        sioInfo.Add(CyPortConstants.SIOInfoValue_SINGLE_SIO);
                                    }
                                    else
                                    {
                                        sioInfo.Add(CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR);
                                        hasSeenFirstInPair = true;
                                    }
                                }
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOGroup, sioGroupValue));
                        }
                    }
                    else
                    {
                        sioInfo.Add(CyPortConstants.SIOInfoValue_NOT_SIO);
                    }
                }

                return string.Join(", ", sioInfo.ToArray());
            }
        }

        object CreateHysteresisCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_CreateHysteresisCSV, exprFuncName));
            }
            else
            {
                List<string> hysteresisValues = new List<string>();

                CyCustErr err;
                string thresholdLevelString = typeConverter.GetAsString(args[0]);
                string sioInfosString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);

                CyStringArrayParamData thresholdData;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(
                    CyInputOnlyBitVectorParam.Formal_ThresholdLevels, thresholdLevelString, pinTypesString, out thresholdData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioInfos;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out sioInfos);

                for (int i = 0; i < width; i++)
                {
                    string threshold = null;
                    string sioInfoType = sioInfos.GetValue(i);
                    switch (sioInfoType)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            //No need to do anything
                            break;

                        //Unlike most SIO settings this one is set per pin not per pair so don't skip the paired pins.
                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            threshold = thresholdData.GetValue(i);
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoType));
                    }

                    if (threshold != null)
                    {
                        switch (threshold)
                        {
                            case CyPortConstants.ThresholdLevelValue_CMOS:
                            case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                            case CyPortConstants.ThresholdLevelValue_LVTTL:
                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                            case CyPortConstants.ThresholdLevelValue_VREF:
                                hysteresisValues.Add(CyPortConstants.SIOHysteresisValue_FALSE);
                                break;

                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                            case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                                hysteresisValues.Add(CyPortConstants.SIOHysteresisValue_TRUE);
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, threshold));
                        }
                    }
                    else
                    {
                        hysteresisValues.Add(CyPortConstants.SIOHysteresisValue_FALSE);
                    }
                }

                Debug.Assert(hysteresisValues.Count == width);
                return string.Join(", ", hysteresisValues.ToArray());
            }
        }

        object CreateVTripCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_CreateVTripCSV, exprFuncName));
            }
            else
            {
                List<string> vtripValues = new List<string>();

                CyCustErr err;
                string thresholdLevelString = typeConverter.GetAsString(args[0]);
                string pinTypesString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(
                    CyInputOnlyBitVectorParam.Formal_ThresholdLevels, thresholdLevelString, pinTypesString, out data);
                if (err.IsNotOk) return err;

                for (int i = 0; i < width; i++)
                {
                    string threshold = data.GetValue(i);
                    switch (threshold)
                    {
                        case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                            vtripValues.Add(CyPortConstants.VTripValue_CMOS_OR_LVTTL);
                            break;

                        case CyPortConstants.ThresholdLevelValue_CMOS:
                        case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                        case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                        case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                        case CyPortConstants.ThresholdLevelValue_VREF:
                        case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                        case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                        case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                        case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                            vtripValues.Add(CyPortConstants.VTripValue_CMOS);
                            break;

                        case CyPortConstants.ThresholdLevelValue_LVTTL:
                            vtripValues.Add(CyPortConstants.VTripValue_LVTTL);
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, threshold));
                    }
                }

                Debug.Assert(vtripValues.Count == width);
                return string.Join(", ", vtripValues.ToArray());
            }
        }

        object CreateSIOVTripBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_CreateSIOVTripBitVector, exprFuncName));
            }
            else
            {
                List<string> vtripValues = new List<string>();

                CyCustErr err;
                string thresholdLevelString = typeConverter.GetAsString(args[0]);
                string sioInfosString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);

                CyStringArrayParamData thresholdData;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(
                    CyInputOnlyBitVectorParam.Formal_ThresholdLevels, thresholdLevelString, pinTypesString, out thresholdData);

                CyStringArrayParamData sioInfos;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out sioInfos);

                for (int i = 0; i < width; i++)
                {
                    string thresholdFirst = null;
                    string thresholdSecond = null;
                    string sioInfoType = sioInfos.GetValue(i);
                    switch (sioInfoType)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            //No need to do anything
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            thresholdFirst = thresholdData.GetValue(i);
                            break;

                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            thresholdFirst = thresholdData.GetValue(i);
                            thresholdSecond = thresholdData.GetValue(i + 1);
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //skip the "grouped" pin that follows the first one as it is already handled
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoType));
                    }

                    //Take which ever value is different from the default (default will be used in the case where it
                    //direction of the pin makes it not apply to this param). If both are different from the default
                    //they will be the same or there will already be an error presented to the user saying that the
                    //values need to be the same.
                    string threshold = thresholdFirst;
                    if (thresholdFirst != null && thresholdFirst != CyPortConstants.DefaultThresholdLevelValue)
                    {
                        threshold = thresholdFirst;
                    }
                    else if (thresholdSecond != null)
                    {
                        threshold = thresholdSecond;
                    }

                    if (threshold != null)
                    {
                        switch (threshold)
                        {
                            case CyPortConstants.ThresholdLevelValue_CMOS:
                            case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                            case CyPortConstants.ThresholdLevelValue_LVTTL:
                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_VREF:
                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                                vtripValues.Add(CyPortConstants.SIOVTripValue_PT4_OR_ONE);
                                break;

                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                                vtripValues.Add(CyPortConstants.SIOVTripValue_PT5);
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, threshold));
                        }
                    }
                }

                int numSIOGroups = vtripValues.Count;
                if (numSIOGroups == 0)
                {
                    return "\"\"";
                }
                return string.Format("{0}'b{1}", numSIOGroups, string.Join("_", vtripValues.ToArray()));
            }
        }

        object CreateSIORefSelCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 5)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_CreateSIORefSelCSV, exprFuncName));
            }
            else
            {
                List<string> refSelValues = new List<string>();
                CyCustErr err;

                string outputDriveLevelsString = typeConverter.GetAsString(args[0]);
                string thresholdLevelsString = typeConverter.GetAsString(args[1]);
                string sioInfoString = typeConverter.GetAsString(args[2]);
                string pinTypesString = typeConverter.GetAsString(args[3]);
                byte width = typeConverter.GetAsByte(args[4]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfoString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateOutputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_OutputDriveLevels, outputDriveLevelsString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_OutputDriveLevels, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelsString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);

                CyStringArrayParamData driveLevelData;
                err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels,
                    outputDriveLevelsString, pinTypesString, out driveLevelData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData thresholdLevelData;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(CyInputOnlyBitVectorParam.Formal_ThresholdLevels,
                    thresholdLevelsString, pinTypesString, out thresholdLevelData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioInfos;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfoString,
                    out sioInfos);
                if (err.IsNotOk) return err;

                for (int i = 0; i < width; i++)
                {
                    string outputDriveLevel = null;
                    string thresholdLevel = null;
                    string sioInfoType = sioInfos.GetValue(i);
                    switch (sioInfoType)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            //No need to do anything
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            outputDriveLevel = driveLevelData.GetValue(i);
                            thresholdLevel = thresholdLevelData.GetValue(i);
                            bool vrefUsed;
                            err = _vrefUsed(outputDriveLevel, thresholdLevel, out vrefUsed);
                            if (err.IsNotOk) return err;

                            if (vrefUsed)
                            {
                                refSelValues.Add(CyPortConstants.SIORefSelValue_VOHREF);
                            }
                            else
                            {
                                refSelValues.Add(CyPortConstants.SIORefSelValue_VCC_IO);
                            }
                            break;

                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            //Need to also check the second part of the pair to see if SIOVRef is used.
                            outputDriveLevel = driveLevelData.GetValue(i);
                            thresholdLevel = thresholdLevelData.GetValue(i);
                            bool vrefUsedByFirst;
                            err = _vrefUsed(outputDriveLevel, thresholdLevel, out vrefUsedByFirst);
                            if (err.IsNotOk) return err;

                            outputDriveLevel = driveLevelData.GetValue(i + 1);
                            thresholdLevel = thresholdLevelData.GetValue(i + 1);
                            bool vrefUsedBySecond;
                            err = _vrefUsed(outputDriveLevel, thresholdLevel, out vrefUsedBySecond);
                            if (err.IsNotOk) return err;

                            if (vrefUsedByFirst || vrefUsedBySecond)
                            {
                                refSelValues.Add(CyPortConstants.SIORefSelValue_VOHREF);
                            }
                            else
                            {
                                refSelValues.Add(CyPortConstants.SIORefSelValue_VCC_IO);
                            }
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //skip the "grouped" pin that follows the first one as this value is set per pair
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoType));
                    }
                }

                return string.Join(", ", refSelValues.ToArray());
            }
        }

        CyCustErr _vrefUsed(string outputDriveLevel, string thresholdLevel, out bool vrefUsed)
        {
            vrefUsed = false;
            switch (outputDriveLevel)
            {
                case CyPortConstants.OutputDriveLevelValue_VDDIO:
                    break;

                case CyPortConstants.OutputDriveLevelValue_VREF:
                    vrefUsed = true;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledOutputDriveLevel, outputDriveLevel));
            }

            switch (thresholdLevel)
            {
                case CyPortConstants.ThresholdLevelValue_CMOS:
                case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                case CyPortConstants.ThresholdLevelValue_LVTTL:
                case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                    break;

                case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                case CyPortConstants.ThresholdLevelValue_VREF:
                case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                    vrefUsed = true;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, thresholdLevel));
            }

            return CyCustErr.Ok;
        }

        object CreateOutputBufferBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateOutputBufferBitVector, exprFuncName));
            }
            else
            {
                List<string> outputBufferValues = new List<string>();
                CyCustErr err;

                string outputDriveLevelsString = typeConverter.GetAsString(args[0]);
                string sioInfosString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateOutputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_OutputDriveLevels, outputDriveLevelsString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_OutputDriveLevels, err.Message);

                CyStringArrayParamData outputDriveLevelData;
                err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels,
                    outputDriveLevelsString, pinTypesString, out outputDriveLevelData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioInfos;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out sioInfos);
                if (err.IsNotOk) return err;

                for (int i = 0; i < width; i++)
                {
                    string outputDriveLevelFirst = null;
                    string outputDriveLevelSecond = null;
                    string sioInfoType = sioInfos.GetValue(i);
                    switch (sioInfoType)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            //No need to do anything
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            outputDriveLevelFirst = outputDriveLevelData.GetValue(i);
                            break;

                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            outputDriveLevelFirst = outputDriveLevelData.GetValue(i);
                            outputDriveLevelSecond = outputDriveLevelData.GetValue(i + 1);
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //skip the "grouped" pin that follows the first one as it is already handled
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoType));
                    }

                    //Take which ever value is different from the default (default will be used in the case where it
                    //direction of the pin makes it not apply to this param). If both are different from the default
                    //they will be the same or there will already be an error presented to the user saying that the
                    //values need to be the same.
                    string outputDriveLevel = outputDriveLevelFirst;
                    if (outputDriveLevelFirst != null && outputDriveLevelFirst !=
                        CyPortConstants.DefaultOutputDriveLevelValue)
                    {
                        outputDriveLevel = outputDriveLevelFirst;
                    }
                    else if (outputDriveLevelSecond != null)
                    {
                        outputDriveLevel = outputDriveLevelSecond;
                    }


                    if (outputDriveLevelFirst != null)
                    {
                        switch (outputDriveLevel)
                        {
                            case CyPortConstants.OutputDriveLevelValue_VDDIO:
                                outputBufferValues.Add(CyPortConstants.SIOOutputBufferValue_UNREGULATED);
                                break;

                            case CyPortConstants.OutputDriveLevelValue_VREF:
                                outputBufferValues.Add(CyPortConstants.SIOOutputBufferValue_REGULATED);
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledOutputDriveLevel,
                                    outputDriveLevel));
                        }
                    }
                }

                int numSIOGroups = outputBufferValues.Count;
                if (numSIOGroups == 0)
                {
                    return "\"\"";
                }
                return string.Format("{0}'b{1}", numSIOGroups, string.Join("_", outputBufferValues.ToArray()));
            }
        }

        object CreateInputBufferBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateInputBufferBitVector, exprFuncName));
            }
            else
            {
                List<string> inputBufferValues = new List<string>();
                CyCustErr err;

                string thresholdLevelString = typeConverter.GetAsString(args[0]);
                string sioInfosString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_ThresholdLevels, err.Message);

                CyStringArrayParamData thresholdData;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(CyInputOnlyBitVectorParam.Formal_ThresholdLevels,
                    thresholdLevelString, pinTypesString, out thresholdData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioInfos;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out sioInfos);

                for (int i = 0; i < width; i++)
                {
                    string thresholdFirst = null;
                    string thresholdSecond = null;
                    string sioInfoType = sioInfos.GetValue(i);
                    switch (sioInfoType)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            //No need to do anything
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            thresholdFirst = thresholdData.GetValue(i);
                            break;

                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            thresholdFirst = thresholdData.GetValue(i);
                            thresholdSecond = thresholdData.GetValue(i + 1);
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //skip the "grouped" pin that follows the first one as it is already handled
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoType));
                    }

                    //Take which ever value is different from the default (default will be used in the case where it
                    //direction of the pin makes it not apply to this param). If both are different from the default
                    //they will be the same or there will already be an error presented to the user saying that the
                    //values need to be the same.
                    string threshold = thresholdFirst;
                    if (thresholdFirst != null && thresholdFirst != CyPortConstants.DefaultThresholdLevelValue)
                    {
                        threshold = thresholdFirst;
                    }
                    else if (thresholdSecond != null)
                    {
                        threshold = thresholdSecond;
                    }

                    if (thresholdFirst != null)
                    {
                        switch (threshold)
                        {
                            case CyPortConstants.ThresholdLevelValue_CMOS:
                            case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                            case CyPortConstants.ThresholdLevelValue_LVTTL:
                                inputBufferValues.Add(CyPortConstants.SIOInputBufferValue_SINGLE_ENDED);
                                break;

                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                            case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                            case CyPortConstants.ThresholdLevelValue_VREF:
                            case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                                inputBufferValues.Add(CyPortConstants.SIOInputBufferValue_DIFFERENTIAL);
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledThreshold, threshold));
                        }
                    }
                }

                int numSIOGroups = inputBufferValues.Count;
                if (numSIOGroups == 0)
                {
                    return "\"\"";
                }
                return string.Format("{0}'b{1}", numSIOGroups, string.Join("_", inputBufferValues.ToArray()));
            }
        }

        object CalculateSIOGroupCount(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 2)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CalculateSIOGroupCount, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string sioInfosString = typeConverter.GetAsString(args[0]);
                byte width = typeConverter.GetAsByte(args[1]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out data);
                if (err.IsNotOk) return err;

                byte sioGroupCnt = 0;
                for (int i = 0; i < width; i++)
                {
                    string sioInfoValue = data.GetValue(i);
                    switch (sioInfoValue)
                    {
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            sioGroupCnt++;
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //skip the "grouped" pin that follows the first one as this value is set per pair
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoValue));
                    }
                }
                return sioGroupCnt;
            }
        }

        object CreatePinModeString(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 2)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreatePinModeString, exprFuncName));
            }
            else
            {
                CyCustErr err;
                StringBuilder pinModes = new StringBuilder();
                string pinTypesString = typeConverter.GetAsString(args[0]);
                byte width = typeConverter.GetAsByte(args[1]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesString,
                    out data);
                if (err.IsNotOk) return err;

                for (int i = 0; i < width; i++)
                {
                    string pinType = data.GetValue(i);

                    if (pinType == CyPortConstants.PinTypesValue_ANALOG)
                    {
                        pinModes.Append("A");
                    }
                    else if (pinType == CyPortConstants.PinTypesValue_BIDIRECTIONAL ||
                        pinType == CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_DIGIN ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG)
                    {
                        pinModes.Append("B");
                    }
                    else if (pinType == CyPortConstants.PinTypesValue_DIGIN ||
                        pinType == CyPortConstants.PinTypesValue_DIGIN_ANALOG)
                    {
                        pinModes.Append("I");
                    }
                    else if (pinType == CyPortConstants.PinTypesValue_DIGOUT ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_ANALOG ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_OE ||
                        pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG)
                    {
                        pinModes.Append("O");
                    }
                    else
                    {
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                    }
                }
                return "\"" + pinModes.ToString() + "\"";
            }
        }

        object UsesInterrupts(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_UsesInterrupts, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string interruptModesString = typeConverter.GetAsString(args[0]);
                string pinTypesString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputOnlyBitVectorParam(CyParamInfo.Formal_ParamName_InterruptMode, interruptModesString,
                    pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_InterruptMode, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(CyInputOnlyBitVectorParam.Formal_InterruptModes,
                interruptModesString, pinTypesString, out data);

                for (int i = 0; i < width; i++)
                {
                    string interruptMode = data.GetValue(i);
                    if (interruptMode != CyPortConstants.InterruptModeValue_NONE)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        object IsHomogeneous_v2(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 7)
            {
                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.ParamCntErr_IsHomogeneous_v2, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string pinTypesString = typeConverter.GetAsString(args[0]);
                string sioInfosString = typeConverter.GetAsString(args[1]);
                string sioRefSelsString = typeConverter.GetAsString(args[2]);
                string dspOutputConnString = typeConverter.GetAsString(args[3]);
                string dspInputConnString = typeConverter.GetAsString(args[4]);
                byte width = typeConverter.GetAsByte(args[5]);
                byte sioGroupCnt = typeConverter.GetAsByte(args[6]);

                //Validate params before they are used. (There is no width or sioGroupCnt validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIOInfo, sioInfosString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIOInfo, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Local_ParamName_SIORefSels, sioRefSelsString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Local_ParamName_SIORefSels, err.Message);
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_DisplayOutputHWConnections,
                    dspOutputConnString);
                if (err.IsNotOk)
                {
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DisplayOutputHWConnections,
                        err.Message);
                }
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_DisplayInputHWConnections,
                    dspInputConnString);
                if (err.IsNotOk)
                {
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DisplayInputHWConnections,
                        err.Message);
                }

                CyStringArrayParamData pinTypesData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesString,
                    out pinTypesData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioInfoData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIOInfo, sioInfosString,
                    out sioInfoData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData sioRefSelData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Local_SIORefSels, sioRefSelsString,
                    out sioRefSelData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData dspOutputConnData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_DisplayOutputConnections,
                    dspOutputConnString, out dspOutputConnData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData dspInputConnData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_DisplayInputConnections,
                    dspInputConnString, out dspInputConnData);
                if (err.IsNotOk) return err;

                string pinType = null;
                for (int i = 0; i < width; i++)
                {
                    string tempPinType = pinTypesData.GetValue(i);
                    if (pinType == null) pinType = tempPinType;
                    else if (pinType != tempPinType) return false;
                }

                string sioRefSel = null;
                for (int i = 0; i < sioGroupCnt; i++)
                {
                    string tempSIORefSel = sioRefSelData.GetValue(i);
                    if (sioRefSel == null) sioRefSel = tempSIORefSel;
                    else if (sioRefSel != tempSIORefSel) return false;
                }

                string dspOutput = null;
                for (int i = 0; i < width; i++)
                {
                    string tempDspOutput = dspOutputConnData.GetValue(i);
                    if (dspOutput == null) dspOutput = tempDspOutput;
                    else if (dspOutput != tempDspOutput) return false;
                }

                string dspInput = null;
                for (int i = 0; i < width; i++)
                {
                    string tempDspInput = dspInputConnData.GetValue(i);
                    if (dspInput == null) dspInput = tempDspInput;
                    else if (dspInput != tempDspInput) return false;
                }

                if (width > 1)
                {
                    for (int i = 0; i < width - 1; i++)
                    {
                        string sioInfo1 = sioInfoData.GetValue(i);
                        string sioInfo2 = sioInfoData.GetValue(i + 1);

                        switch (sioInfo1)
                        {
                            case CyPortConstants.SIOInfoValue_NOT_SIO:
                            case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                                if (sioInfo2 != sioInfo1)
                                {
                                    return false;
                                }
                                break;

                            case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                                if ((sioInfo2 == CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR ||
                                     sioInfo2 == CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR) == false)
                                {
                                    return false;
                                }
                                break;

                            default:
                                return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfo1));
                        }
                    }
                }

                return true;
            }
        }

        object CreateDriveModesCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateDriveModesCSV, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string driveModesString = typeConverter.GetAsString(args[0]);
                string pinTypesString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateDriveModesParam(driveModesString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DriveMode, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateDriveModeData(driveModesString, pinTypesString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateInputBufferesEnabledCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateInputBufferesEnabledCSV, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string inputBuffersEnabledString = typeConverter.GetAsString(args[0]);
                string pinTypesString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateInputBuffersEnabledParam(inputBuffersEnabledString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_InputBuffersEnabled, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateInputBufferEnabledData(inputBuffersEnabledString,
                    pinTypesString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateInitialDriveStatesCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateInitialDriveStatesCSV, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string initialDriveStatesString = typeConverter.GetAsString(args[0]);
                string driveModesString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);
                err = ValidateDriveModesParam(driveModesString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DriveMode, err.Message);
                err = ValidateInitialDriveStatesParam(initialDriveStatesString, driveModesString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_InitialDriveStates, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateInitialDriveStateData(initialDriveStatesString,
                    driveModesString, pinTypesString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateCSVFromBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateCSVFromBitVector, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string bitVectorParamName = typeConverter.GetAsString(args[0]);
                string bitVectorString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(bitVectorParamName, bitVectorString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, bitVectorParamName, err.Message);

                CyBitVectorParam paramType;
                err = CyStringArrayParamData.GetBitVectorParamType(bitVectorParamName, out paramType);
                if (err.IsNotOk) return err;

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateBitVectorData(paramType, bitVectorString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateCSVFromInputOnlyBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateCSVFromInputOnlyBitVector, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string bitVectorParamName = typeConverter.GetAsString(args[0]);
                string bitVectorString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateInputOnlyBitVectorParam(bitVectorParamName, bitVectorString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, bitVectorParamName, err.Message);

                CyInputOnlyBitVectorParam paramType;
                err = CyStringArrayParamData.GetInputOnlyBitVectorParamType(bitVectorParamName, out paramType);
                if (err.IsNotOk) return err;

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateInputOnlyBitVectorData(paramType, bitVectorString, pinTypesString,
                    out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateCSVFromOutputOnlyBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 4)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateCSVFromOutputOnlyBitVector, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string bitVectorParamName = typeConverter.GetAsString(args[0]);
                string bitVectorString = typeConverter.GetAsString(args[1]);
                string pinTypesString = typeConverter.GetAsString(args[2]);
                byte width = typeConverter.GetAsByte(args[3]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateOutputOnlyBitVectorParam(bitVectorParamName, bitVectorString, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, bitVectorParamName, err.Message);

                CyOutputOnlyBitVectorParam paramType;
                err = CyStringArrayParamData.GetOutputOnlyBitVectorParamType(bitVectorParamName, out paramType);
                if (err.IsNotOk) return err;

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(paramType, bitVectorString, pinTypesString,
                    out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateIOVoltagesCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 2)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateIOVoltagesCSV, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string ioVoltagesString = typeConverter.GetAsString(args[0]);
                byte width = typeConverter.GetAsByte(args[1]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateIOVoltagesParam(ioVoltagesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_IOVoltages, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreateIOVoltageData(ioVoltagesString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreatePinAliasesCSV(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 2)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreatePinAliasesCSV, exprFuncName));
            }
            else
            {
                CyCustErr err;
                string pinAliasesString = typeConverter.GetAsString(args[0]);
                byte width = typeConverter.GetAsByte(args[1]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidatePinAliasParam(pinAliasesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinAlisases, err.Message);

                CyStringArrayParamData data;
                err = CyStringArrayParamData.CreatePinAliasData(pinAliasesString, out data);
                if (err.IsNotOk) return err;

                List<string> csv = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    csv.Add(data.GetValue(i));
                }

                return string.Join(", ", csv.ToArray());
            }
        }

        object CreateOutputConnVisibleBitVector(string exprFuncName, object[] args, ICyExprTypeConverter typeConverter)
        {
            //Generate an error if the wrong number of arguements have been passed in.
            if (args.Length != 3)
            {
                return new CyCustErr(string.Format(
                    cy_pins_v1_20.Resource1.ParamCntErr_CreateOutputConnVisibleBitVector, exprFuncName));
            }
            else
            {
                List<string> outputConnVisible = new List<string>();
                CyCustErr err;

                string pinTypesString = typeConverter.GetAsString(args[0]);
                string displayOutputHWConnectionsString = typeConverter.GetAsString(args[1]);
                byte width = typeConverter.GetAsByte(args[2]);

                //Validate params before they are used. (There is no width validation to do.)
                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_PinTypes, pinTypesString);
                if (err.IsNotOk)
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_PinTypes, err.Message);

                err = ValidateBitVectorParam(CyParamInfo.Formal_ParamName_DisplayOutputHWConnections,
                    displayOutputHWConnectionsString);
                if (err.IsNotOk)
                {
                    return WrapCustFunctionErr(exprFuncName, CyParamInfo.Formal_ParamName_DisplayOutputHWConnections,
                        err.Message);
                }

                CyStringArrayParamData pinTypesData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesString,
                    out pinTypesData);
                if (err.IsNotOk) return err;

                CyStringArrayParamData displayOutputHWConnectionsData;
                err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_DisplayOutputConnections,
                    displayOutputHWConnectionsString, out displayOutputHWConnectionsData);
                if (err.IsNotOk) return err;

                for (int i = 0; i < width; i++)
                {
                    string pinType = pinTypesData.GetValue(i);
                    string dspHWConn = displayOutputHWConnectionsData.GetValue(i);

                    switch (pinType)
                    {
                        case CyPortConstants.PinTypesValue_ANALOG:
                        case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                        case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                        case CyPortConstants.PinTypesValue_DIGIN:
                        case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                            outputConnVisible.Add(CyPortConstants.Display_FALSE);
                            break;

                        case CyPortConstants.PinTypesValue_DIGOUT:
                        case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                        case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                        case CyPortConstants.PinTypesValue_DIGOUT_OE:
                        case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                        case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                            Debug.Assert(dspHWConn == "0" || dspHWConn == "1", "not a vaild value for a bit vector");
                            outputConnVisible.Add(dspHWConn);
                            break;

                        default:
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                    }
                }

                return string.Format("{0}'b{1}", width, string.Join("_", outputConnVisible.ToArray()));
            }
        }


        //------------------------------------

        protected CyCustErr ValidateBitVectorParam(string paramName, string value)
        {
            CyStringArrayParamData data;
            CyCustErr err;
            CyBitVectorParam paramType;
            err = CyStringArrayParamData.GetBitVectorParamType(paramName, out paramType);
            if (err.IsNotOk) return err;

            err = CyStringArrayParamData.CreateBitVectorData(paramType, value, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateInputOnlyBitVectorParam(string paramName, string value, string pinTypesValue)
        {
            CyStringArrayParamData data;
            CyCustErr err;
            CyInputOnlyBitVectorParam paramType;
            err = CyStringArrayParamData.GetInputOnlyBitVectorParamType(paramName, out paramType);
            if (err.IsNotOk) return err;

            err = CyStringArrayParamData.CreateInputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateOutputOnlyBitVectorParam(string paramName, string value, string pinTypesValue)
        {
            CyStringArrayParamData data;
            CyCustErr err;
            CyOutputOnlyBitVectorParam paramType;
            err = CyStringArrayParamData.GetOutputOnlyBitVectorParamType(paramName, out paramType);
            if (err.IsNotOk) return err;

            err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateDriveModesParam(string value, string pinTypesValue)
        {
            CyStringArrayParamData data;
            CyCustErr err;

            err = CyStringArrayParamData.CreateDriveModeData(value, pinTypesValue, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateInitialDriveStatesParam(string value, string driveModesValue,
            string pinTypesValue)
        {
            CyStringArrayParamData data;
            CyCustErr err;

            err = CyStringArrayParamData.CreateInitialDriveStateData(value, driveModesValue, pinTypesValue, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateInputBuffersEnabledParam(string value, string pinTypesValue)
        {
            CyStringArrayParamData data;
            CyCustErr err;

            err = CyStringArrayParamData.CreateInputBufferEnabledData(value, pinTypesValue, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidateIOVoltagesParam(string value)
        {
            CyStringArrayParamData data;
            CyCustErr err;

            err = CyStringArrayParamData.CreateIOVoltageData(value, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        protected CyCustErr ValidatePinAliasParam(string value)
        {
            CyStringArrayParamData data;
            CyCustErr err;

            err = CyStringArrayParamData.CreatePinAliasData(value, out data);
            if (err.IsNotOk) return err;

            err = data.Validate();
            return err;
        }

        #endregion

        //-----------------------------

        #region ICyVerilogCustomize_v1 Members

        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            //This is needed because it is the only way to get access to query.InstanceIdPath when the value has
            //been correctly calculated and it is the only way to create a valid verilog bit_vector value.
            //It is now also used to combine multiple terminals into a single signal.

            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_psoc3_pins_v1_10", query.InstanceName);

            //Add Generics.
            byte numPins;
            err = CyParamInfo.GetNumPinsValue(query, out numPins);
            if (err.IsNotOk) return err;

            byte sioGroupCnt;
            err = CyParamInfo.GetSIOGroupCntValue(query, out sioGroupCnt);
            if (err.IsNotOk) return err;

            vw.AddGeneric("id", "\"" + query.InstanceIdPath + "\"");
            foreach (string paramName in query.GetParamNames())
            {
                CyCompDevParam param = query.GetCommittedParam(paramName);
                if (param.IsHardware)
                {
                    if (CyStringArrayParamData.IsLocalStringArrayParam(param.Name))
                    {
                        CyStringArrayParamData data;
                        err = CyStringArrayParamData.CreateStringArrayData(query, param.Name, out data);
                        if (err.IsNotOk) return err;

                        byte width = (paramName == CyParamInfo.Local_ParamName_SIORefSels) ? sioGroupCnt : numPins;
                        string value = data.GetVerilog(width);
                        vw.AddGeneric(param.Name, value);
                    }
                    else
                    {
                        vw.AddGeneric(param.Name, param.Value);
                    }
                }
            }

            //Add Ports.
            bool displayAsBus;
            err = CyParamInfo.GetDisplayAsBusValue(query, out displayAsBus);
            if (err.IsNotOk) return err;

            foreach (string rootTermName in CyTermInfo.TerminalRootNames)
            {
                byte expectedWidth;
                err = CyTermInfo.GetExpectedTerminalWidth(query, rootTermName, out expectedWidth);
                if (err.IsNotOk) return err;

                Dictionary<int, string> indexToTerminalNameMapping = new Dictionary<int, string>();
                indexToTerminalNameMapping = GetAssociatedTerminals(rootTermName, termQuery);

                //Don't add an entry for analog terminals if no portion is hooked up
                if (indexToTerminalNameMapping.Count != 0 || CyTermInfo.IsAnalog(rootTermName) == false)
                {
                    List<string> bits = new List<string>();
                    for (int i = expectedWidth - 1; i >= 0; )
                    {
                        bool containsMapping = indexToTerminalNameMapping.ContainsKey(i);

                        if (containsMapping)
                        {
                            if (displayAsBus && rootTermName == CyTermInfo.RootTermName_OE)
                            {
                                string termName = indexToTerminalNameMapping[i];
                                string value = termQuery.GetTermSigSegName(termName);
                                for (int temp = 0; temp < expectedWidth; temp++)
                                {
                                    bits.Add(value);
                                }
                                int termWidth = termQuery.GetTermWidth(termName) * expectedWidth;
                                i -= termWidth;
                            }
                            else
                            {
                                string termName = indexToTerminalNameMapping[i];
                                string value = termQuery.GetTermSigSegName(termName);
                                bits.Add(value);
                                int termWidth = termQuery.GetTermWidth(termName);
                                i -= termWidth;
                            }
                        }
                        else
                        {
                            int temp = i + 1;
                            while (containsMapping == false && temp >= 0)
                            {
                                temp--;
                                containsMapping = indexToTerminalNameMapping.ContainsKey(temp);
                            }

                            //From i to temp + 1, there is nothing connected.
                            int unhookedWidth = i - temp;

                            if (CyTermInfo.IsInput(rootTermName))
                            {
                                if (CyTermInfo.RootTermName_OE == rootTermName)
                                {
                                    bits.Add(string.Format("{0}'b1", unhookedWidth));
                                }
                                else
                                {
                                    bits.Add(string.Format("{0}'b0", unhookedWidth));
                                }
                            }
                            else
                            {
                                string wireName =
                                    string.Format("tmp{0}_{1}__{2}_net", rootTermName.ToUpper(), i, query.InstanceName);

                                if (CyTermInfo.IsAnalog(rootTermName))
                                {
                                    vw.AddElectrical(wireName, unhookedWidth - 1, 0);
                                }
                                else
                                {
                                    vw.AddWire(wireName, unhookedWidth - 1, 0);
                                }
                                bits.Add(string.Format("{0}[{1}:0]", wireName, unhookedWidth - 1));
                            }

                            i = temp;
                        }
                    }

                    string assignment = string.Join(", ", bits.ToArray());
                    if (!string.IsNullOrEmpty(assignment))
                    {
                        if (CyTermInfo.RootTermName_OE == rootTermName)
                        {
                            // POST PR4 NOTE:
                            //     Prior to PR4, the OE signals are 'active low', meaning, a 0 will
                            //     enable the output, a 1 would turn it off. CDT 32449 is tracking the
                            //     PR4 change to the device to make it 'active high', as it should have
                            //     been.
                            string netOEName = "tmpOE__" + query.InstanceName + "_net";
                            vw.AddWire(netOEName, expectedWidth - 1, 0);
                            vw.AssignPreES3CondWire(netOEName, "~" + "{" + assignment + "}", "{" + assignment + "}");
                            vw.AddPort(rootTermName, netOEName);
                        }
                        else
                        {
                            vw.AddPort(rootTermName, "{" + assignment + "}");
                        }
                    }
                }
                else if (CyTermInfo.RootTermName_SIOVRef == rootTermName)
                {
                    string wireName =
                        string.Format("tmp{0}__{1}_net", rootTermName.ToUpper(), query.InstanceName);
                    vw.AddElectrical(wireName, 0, 0);
                    vw.AddPort(rootTermName, wireName);
                }
            }

            codeSnippet = vw.ToString();
            return err;
        }

        public static Dictionary<int, string> GetAssociatedTerminals(string portTermBaseName,
            ICyTerminalQuery_v1 termQuery)
        {
            //Key: index of the terminal when combined into a bus, Value: the associated terminal name
            Dictionary<int, string> termsInDescendingOrder = new Dictionary<int, string>();

            //Match portTermBaseName_# and portTermBaseName_#[#:#]
            Regex regex = new Regex("^" + portTermBaseName + @"_(?<Index>\d+)(\[\d+:\d+\])?$");

            foreach (string term in termQuery.GetTerminalNames())
            {
                //Collect all terminals with a name formatted as 'TermName_#'.
                Match m = regex.Match(term);
                if (m.Success)
                {
                    Debug.Assert(m.Value == term);
                    int index = int.Parse(m.Groups["Index"].Value);
                    termsInDescendingOrder.Add(index, term);
                }
            }

            return termsInDescendingOrder;
        }

        #endregion

        //-----------------------------

        #region ICyInstValidateHook_v1 Members

        public void ValidateParams(ICyInstValidate_v1 instVal)
        {
            //Per pin data needs to be validated here. Make sure to validate the GUI visible param not the 
            //corresponding verilog one as that is the only way for the param to have an error show up on it when the 
            //user is entering values.
            CyCustErr err;

            foreach (string paramName in instVal.GetParamNames())
            {
                CyStringArrayParamData data = null;
                CyCompDevParam param = instVal.GetCommittedParam(paramName);

                if (CyStringArrayParamData.IsStringArrayParamNeedingValidation(instVal, paramName))
                {
                    Debug.Assert(CyStringArrayParamData.IsFormalStringArrayParam(paramName));
                    err = CyStringArrayParamData.CreateStringArrayData(instVal, paramName, out data);
                    if (err.IsNotOk)
                    {
                        data = null;
                    }
                }

                if (data != null)
                {
                    err = data.Validate();
                    if (err.IsNotOk)
                    {
                        instVal.AddError(paramName, err);
                    }
                }
            }

            byte numPins;
            err = CyParamInfo.GetNumPinsValue(instVal, out numPins);
            if (err.IsOk)
            {
                CheckCannotUseAnalogOrLVTTLIfSIO(instVal, numPins);
                CheckCannotEnableInputBufferIfAnalogHiZ(instVal, numPins);
                CheckSIOLayoutRestrictions(instVal, numPins);
                CheckCannotBeContiguousAndAnalog(instVal, numPins);
                CheckSIOPairSettingsMatch(instVal, numPins);
            }
        }

        private void CheckSIOPairSettingsMatch(ICyInstValidate_v1 instVal, byte numPins)
        {
            CyStringArrayParamData sioInfoData;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Local_ParamName_SIOInfo,
                out sioInfoData);

            if (err.IsOk)
            {
                CyStringArrayParamData pinTypesData;
                err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Formal_ParamName_PinTypes,
                    out pinTypesData);

                if (err.IsOk)
                {
                    for (int i = 0; i < numPins; i++)
                    {
                        string sioInfo = sioInfoData.GetValue(i);
                        switch (sioInfo)
                        {
                            case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                            case CyPortConstants.SIOInfoValue_NOT_SIO:
                            case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                                break;

                            case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                                //Validate SIO Input/Output settings (except for hystersis) match.
                                string pinType1 = pinTypesData.GetValue(i);
                                string pinType2 = pinTypesData.GetValue(i + 1);

                                if (CyPortConstants.IsInput(pinType1) && CyPortConstants.IsInput(pinType2))
                                {
                                    CyStringArrayParamData thresholdData;
                                    err = CyStringArrayParamData.CreateStringArrayData(instVal,
                                        CyParamInfo.Formal_ParamName_ThresholdLevels, out thresholdData);
                                    if (err.IsOk)
                                    {
                                        string threshold1 = thresholdData.GetValue(i);
                                        string threshold2 = thresholdData.GetValue(i + 1);

                                        switch (threshold1)
                                        {
                                            case CyPortConstants.ThresholdLevelValue_CMOS:
                                            case CyPortConstants.ThresholdLevelValue_CMOS_LVTTL:
                                            case CyPortConstants.ThresholdLevelValue_LVTTL:
                                                if (threshold1 != threshold2)
                                                {
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i)));
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i + 1)));
                                                }
                                                break;

                                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO:
                                            case CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST:
                                                if (threshold2 != CyPortConstants.ThresholdLevelValue_PT4_VDDIO &&
                                                    threshold2 != CyPortConstants.ThresholdLevelValue_PT4_VDDIO_HYST)
                                                {
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i)));
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i + 1)));
                                                }
                                                break;

                                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO:
                                            case CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST:
                                                if (threshold2 != CyPortConstants.ThresholdLevelValue_PT5_VDDIO &&
                                                    threshold2 != CyPortConstants.ThresholdLevelValue_PT5_VDDIO_HYST)
                                                {
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i)));
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i + 1)));
                                                }
                                                break;

                                            case CyPortConstants.ThresholdLevelValue_PT5_VREF:
                                            case CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST:
                                                if (threshold2 != CyPortConstants.ThresholdLevelValue_PT5_VREF &&
                                                    threshold2 != CyPortConstants.ThresholdLevelValue_PT5_VREF_HYST)
                                                {
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i)));
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i + 1)));
                                                }
                                                break;

                                            case CyPortConstants.ThresholdLevelValue_VREF:
                                            case CyPortConstants.ThresholdLevelValue_VREF_HYST:
                                                if (threshold2 != CyPortConstants.ThresholdLevelValue_VREF &&
                                                    threshold2 != CyPortConstants.ThresholdLevelValue_VREF_HYST)
                                                {
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i)));
                                                    instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairInputsDontMatch, i + 1)));
                                                }
                                                break;

                                            default:
                                                Debug.Fail("unhandled");
                                                break;
                                        }
                                    }
                                }

                                if (CyPortConstants.IsOutput(pinType1) && CyPortConstants.IsOutput(pinType2))
                                {
                                    CyStringArrayParamData outputDriveLevelData;
                                    err = CyStringArrayParamData.CreateStringArrayData(instVal,
                                        CyParamInfo.Formal_ParamName_OutputDriveLevels, out outputDriveLevelData);
                                    if (err.IsOk)
                                    {
                                        string driveLevel1 = outputDriveLevelData.GetValue(i);
                                        string driveLevel2 = outputDriveLevelData.GetValue(i + 1);

                                        if (driveLevel1 != driveLevel2)
                                        {
                                            instVal.AddError(CyParamInfo.Formal_ParamName_OutputDriveLevels,
                                                        new CyCustErr(string.Format(
                                                        cy_pins_v1_20.Resource1.Err_SIOPairOutputsDontMatch, i)));
                                            instVal.AddError(CyParamInfo.Formal_ParamName_OutputDriveLevels,
                                                new CyCustErr(string.Format(
                                                cy_pins_v1_20.Resource1.Err_SIOPairOutputsDontMatch, i + 1)));
                                        }
                                    }
                                }
                                break;

                            default:
                                Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfo));
                                break;
                        }
                    }
                }
            }
        }

        private void CheckCannotBeContiguousAndAnalog(ICyInstValidate_v1 instVal, byte numPins)
        {
            if (numPins > 1)
            {
                CyCustErr err;
                bool isContiguous;
                err = CyParamInfo.GetIsContiguousValue(instVal, out isContiguous);

                if (err.IsOk && isContiguous)
                {
                    CyStringArrayParamData pinTypesData;
                    err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Formal_ParamName_PinTypes,
                        out pinTypesData);

                    if (err.IsOk)
                    {
                        for (int i = 0; i < numPins; i++)
                        {
                            string pinType = pinTypesData.GetValue(i);
                            if (pinType == CyPortConstants.PinTypesValue_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_DIGIN_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_DIGOUT_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG ||
                                pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_PinTypes,
                                   new CyCustErr(string.Format(
                                       cy_pins_v1_20.Resource1.Err_CannotBeContiguousAndAnalog, i)));

                                instVal.AddError(CyParamInfo.Formal_ParamName_LayoutMode,
                                   new CyCustErr(string.Format(
                                       cy_pins_v1_20.Resource1.Err_CannotBeContiguousAndAnalog, i)));
                            }
                        }
                    }
                }
            }
        }

        private static void CheckSIOLayoutRestrictions(ICyInstValidate_v1 instVal, byte numPins)
        {
            //If Contiguous, cannot have more than 2 SIO singles in a row and cannot have more than 1 if next
            //to an SIO group.
            bool isContiguous;
            CyCustErr err = CyParamInfo.GetIsContiguousValue(instVal, out isContiguous);
            if (err.IsOk && isContiguous)
            {
                CyStringArrayParamData sioInfoData;
                err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Local_ParamName_SIOInfo,
                    out sioInfoData);
                if (err.IsOk)
                {
                    int runOfSingleSIOs = 0;
                    bool foundPair = false;
                    for (int i = 0; i < numPins; i++)
                    {
                        string sioInfo = sioInfoData.GetValue(i);
                        switch (sioInfo)
                        {
                            case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                                runOfSingleSIOs++;
                                break;

                            case CyPortConstants.SIOInfoValue_NOT_SIO:
                                foundPair = false;
                                runOfSingleSIOs = 0;
                                break;

                            case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                                foundPair = true;
                                runOfSingleSIOs = 0;
                                break;

                            default:
                                Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfo));
                                foundPair = false;
                                runOfSingleSIOs = 0;
                                break;
                        }
                        if (runOfSingleSIOs == 2 && foundPair)
                        {
                            instVal.AddError(CyParamInfo.Formal_ParamName_SIOGroups,
                                new CyCustErr(cy_pins_v1_20.Resource1.Err_TooManySingleSIONextToSIOPair));
                            instVal.AddError(CyParamInfo.Formal_ParamName_LayoutMode,
                               new CyCustErr(cy_pins_v1_20.Resource1.Err_TooManySingleSIONextToSIOPair));
                        }
                        if (runOfSingleSIOs > 2)
                        {
                            instVal.AddError(CyParamInfo.Formal_ParamName_SIOGroups,
                                new CyCustErr(cy_pins_v1_20.Resource1.Err_TooManySingleSIOInARow));
                            instVal.AddError(CyParamInfo.Formal_ParamName_LayoutMode,
                                new CyCustErr(cy_pins_v1_20.Resource1.Err_TooManySingleSIOInARow));
                            break;
                        }
                    }
                }
            }
        }

        private static void CheckCannotEnableInputBufferIfAnalogHiZ(ICyInstValidate_v1 instVal, byte numPins)
        {
            //Cannot set InputBufferEnabled to true if DriveMode is Analog HiZ.
            CyStringArrayParamData driveModesData;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Formal_ParamName_DriveMode,
                out driveModesData);

            if (err.IsOk)
            {
                CyStringArrayParamData inputBuffersEnabled;
                err = CyStringArrayParamData.CreateStringArrayData(instVal,
                    CyParamInfo.Formal_ParamName_InputBuffersEnabled, out inputBuffersEnabled);

                if (err.IsOk)
                {
                    for (int i = 0; i < numPins; i++)
                    {
                        if (driveModesData.GetValue(i) == CyPortConstants.DriveModeValue_ANALOG_HI_Z &&
                            inputBuffersEnabled.GetValue(i) == CyPortConstants.InputBufferEnabledValue_TRUE)
                        {
                            instVal.AddError(CyParamInfo.Formal_ParamName_InputBuffersEnabled,
                                new CyCustErr(string.Format(
                                    cy_pins_v1_20.Resource1.Err_CannotEnableIBufAndAnalogHiZ, i)));

                            instVal.AddError(CyParamInfo.Formal_ParamName_DriveMode,
                                new CyCustErr(string.Format(
                                    cy_pins_v1_20.Resource1.Err_CannotEnableIBufAndAnalogHiZ, i)));
                        }
                    }
                }
            }
        }

        private void CheckCannotUseAnalogOrLVTTLIfSIO(ICyInstValidate_v1 instVal, byte numPins)
        {
            //Cannot use analog if SIO. Cannot use LVTTL if SIO.
            string hotSwapString;
            string thresholdLevelString;
            string outputDriveLevelString;
            string driveCurrentsString;
            string pinTypesString;
            CyStringArrayParamData thresholdLevelData;
            CyStringArrayParamData pinTypesData;

            CyCustErr err = _getSIOValidationDatas(instVal, out hotSwapString, out thresholdLevelString,
                out outputDriveLevelString, out driveCurrentsString, out pinTypesString, out pinTypesData,
                out thresholdLevelData);

            if (err.IsOk)
            {
                for (int i = 0; i < numPins; i++)
                {
                    bool isSIO;
                    err = IsSIO(hotSwapString, thresholdLevelString, outputDriveLevelString,
                        driveCurrentsString, pinTypesString, i, out isSIO);
                    if (err.IsOk)
                    {
                        string pinType = pinTypesData.GetValue(i);
                        if (isSIO && (pinType == CyPortConstants.PinTypesValue_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_DIGIN_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_DIGOUT_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG ||
                            pinType == CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG))
                        {
                            string errMsg = string.Format(cy_pins_v1_20.Resource1.Err_CannotUseAnalogAndSIO, i);

                            instVal.AddError(CyParamInfo.Formal_ParamName_PinTypes, new CyCustErr(errMsg));

                            bool tmpSIO;
                            err = IsSIOBecauseOfDriveCurrents(driveCurrentsString, pinTypesString, i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_DriveCurrents, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfHotSwap(hotSwapString, pinTypesString, i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_HotSwaps, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfOutputDriveLevels(outputDriveLevelString, pinTypesString,
                                i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_OutputDriveLevels, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfThresholdLevels(thresholdLevelString, pinTypesString, i,
                                out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels, new CyCustErr(errMsg));
                            }
                        }

                        if (isSIO && thresholdLevelData.GetValue(i) == CyPortConstants.ThresholdLevelValue_LVTTL)
                        {
                            string errMsg = string.Format(cy_pins_v1_20.Resource1.Err_CannotUseLVTTLAndSIO, i);

                            instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels, new CyCustErr(errMsg));

                            bool tmpSIO;
                            err = IsSIOBecauseOfDriveCurrents(driveCurrentsString, pinTypesString, i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_DriveCurrents, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfHotSwap(hotSwapString, pinTypesString, i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_HotSwaps, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfOutputDriveLevels(outputDriveLevelString, pinTypesString,
                                i, out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_OutputDriveLevels, new CyCustErr(errMsg));
                            }

                            err = IsSIOBecauseOfThresholdLevels(thresholdLevelString, pinTypesString, i,
                                out tmpSIO);
                            if (err.IsOk && tmpSIO)
                            {
                                instVal.AddError(CyParamInfo.Formal_ParamName_ThresholdLevels, new CyCustErr(errMsg));
                            }
                        }
                    }
                }
            }
        }

        CyCustErr _getSIOValidationDatas(ICyInstValidate_v1 instVal, out string hotSwapString,
            out string thresholdLevelString, out string outputDriveLevelString, out string driveCurrentsString,
            out string pinTypesString, out CyStringArrayParamData pinTypesData,
            out CyStringArrayParamData thresholdLevelData)
        {
            hotSwapString = thresholdLevelString = outputDriveLevelString = driveCurrentsString = pinTypesString = null;
            pinTypesData = thresholdLevelData = null;

            CyCustErr err;

            err = CyPortCustomizer.GetParamValue<string>(instVal, CyParamInfo.Formal_ParamName_HotSwaps,
                out hotSwapString);
            if (err.IsNotOk) return err;

            err = CyPortCustomizer.GetParamValue<string>(instVal, CyParamInfo.Formal_ParamName_ThresholdLevels,
                out thresholdLevelString);
            if (err.IsNotOk) return err;

            err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Formal_ParamName_ThresholdLevels,
                out thresholdLevelData);
            if (err.IsNotOk) return err;

            err = CyPortCustomizer.GetParamValue<string>(instVal, CyParamInfo.Formal_ParamName_OutputDriveLevels,
                out outputDriveLevelString);
            if (err.IsNotOk) return err;

            err = CyPortCustomizer.GetParamValue<string>(instVal, CyParamInfo.Formal_ParamName_DriveCurrents,
                out driveCurrentsString);
            if (err.IsNotOk) return err;

            err = CyPortCustomizer.GetParamValue<string>(instVal, CyParamInfo.Formal_ParamName_PinTypes,
                out pinTypesString);
            if (err.IsNotOk) return err;

            err = CyStringArrayParamData.CreateStringArrayData(instVal, CyParamInfo.Formal_ParamName_PinTypes,
                out pinTypesData);
            if (err.IsNotOk) return err;

            return CyCustErr.Ok;
        }

        #endregion

        //-----------------------------

        #region ICyPinDataProvider_v1 Members

        public string BaseTermName_Analog
        {
            get { return CyTermInfo.RootTermName_Analog; }
        }

        public CyCustErr GetWidth(ICyInstQuery_v1 instQuery, out byte width)
        {
            return CyParamInfo.GetNumPinsValue(instQuery, out width);
        }

        public CyCustErr GetUsesInterrupts(ICyInstQuery_v1 instQuery, out bool usesInterrupts)
        {
            return CyParamInfo.GetUsesInterruptsValue(instQuery, out usesInterrupts);
        }

        public CyCustErr GetContiguousSegments(ICyInstQuery_v1 instQuery,
            out IEnumerable<KeyValuePair<int, int>> contiguousSegments)
        {
            //Segements can be grouped one of 2 ways, either by being contiguous or by being in an SIO pair.

            CyCustErr err;
            contiguousSegments = new KeyValuePair<int, int>[] { };

            bool isContiguous;
            err = CyParamInfo.GetIsContiguousValue(instQuery, out isContiguous);
            if (err.IsNotOk) return err;

            byte width;
            err = CyParamInfo.GetNumPinsValue(instQuery, out width);
            if (err.IsNotOk) return err;

            List<KeyValuePair<int, int>> contSegments = new List<KeyValuePair<int, int>>();
            if (isContiguous)
            {
                contSegments.Add(new KeyValuePair<int, int>(width - 1, 0));
            }
            else
            {
                CyStringArrayParamData sioInfosData;
                err = CyStringArrayParamData.CreateStringArrayData(instQuery, CyParamInfo.Local_ParamName_SIOInfo,
                    out sioInfosData);
                if (err.IsNotOk) return err;

                for (int i = width - 1; i >= 0; i--)
                {
                    string sioInfo = sioInfosData.GetValue(i);
                    switch (sioInfo)
                    {
                        case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                            contSegments.Add(new KeyValuePair<int, int>(i + 1, i));
                            break;

                        case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                            //Already added in the SIOInfoValue_FIRST_IN_SIO_PAIR case so nothing to do here.
                            break;

                        case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                        case CyPortConstants.SIOInfoValue_NOT_SIO:
                            contSegments.Add(new KeyValuePair<int, int>(i, i));
                            break;

                        default:
                            Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfo));
                            break;
                    }
                }
            }

            contiguousSegments = contSegments;
            return CyCustErr.Ok;
        }

        public CyCustErr GetAlias(ICyInstQuery_v1 instQuery, int index, out string alias)
        {
            alias = null;

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreatePinAliasData(instQuery, true, out data);
            if (err.IsNotOk) return err;

            alias = data.GetValue(index);
            return CyCustErr.Ok;
        }

        public CyCustErr GetIOVoltage(ICyInstQuery_v1 instQuery, int index, out double ioVoltage)
        {
            ioVoltage = 0;

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateIOVoltageData(instQuery, true, out data);
            if (err.IsNotOk) return err;

            string ioVoltageString = data.GetValue(index);
            if (double.TryParse(ioVoltageString, out ioVoltage) == false)
            {
                ioVoltage = 0;
            }
            return CyCustErr.Ok;
        }

        public CyCustErr GetPinType(ICyInstQuery_v1 instQuery, int index, out CyPinDataPinType_v1 pinType)
        {
            pinType = default(CyPinDataPinType_v1);

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateBitVectorData(instQuery,
                CyBitVectorParam.Formal_PinTypes, out data);
            if (err.IsNotOk) return err;

            string pinTypeString = data.GetValue(index);
            switch (pinTypeString)
            {
                case CyPortConstants.PinTypesValue_ANALOG:
                    pinType = CyPinDataPinType_v1.Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                    pinType = CyPinDataPinType_v1.Bidir;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    pinType = CyPinDataPinType_v1.Bidir_Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGIN:
                    pinType = CyPinDataPinType_v1.DigIn;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                    pinType = CyPinDataPinType_v1.DigIn_Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT:
                    pinType = CyPinDataPinType_v1.DigOut;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    pinType = CyPinDataPinType_v1.DigOut_Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    pinType = CyPinDataPinType_v1.DigOut_DigIn;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    pinType = CyPinDataPinType_v1.DigOut_DigIn_Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    pinType = CyPinDataPinType_v1.DigOut_DigIn_OE;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    pinType = CyPinDataPinType_v1.DigOut_OE;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                    pinType = CyPinDataPinType_v1.DigOut_OE_Analog;
                    return CyCustErr.Ok;

                case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    pinType = CyPinDataPinType_v1.DigOut_OE_DigIn_Analog;
                    return CyCustErr.Ok;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinTypeString));
            }
        }

        public CyCustErr GetSIOInfo(ICyInstQuery_v1 instQuery, int index, out CyPinDataSIOType_v1 sioInfo)
        {
            sioInfo = default(CyPinDataSIOType_v1);

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateBitVectorData(instQuery, CyBitVectorParam.Local_SIOInfo, out data);
            if (err.IsNotOk) return err;

            string sioInfoString = data.GetValue(index);
            switch (sioInfoString)
            {
                case CyPortConstants.SIOInfoValue_FIRST_IN_SIO_PAIR:
                    sioInfo = CyPinDataSIOType_v1.FirstInSIOPair;
                    return CyCustErr.Ok;

                case CyPortConstants.SIOInfoValue_NOT_SIO:
                    sioInfo = CyPinDataSIOType_v1.NotSIO;
                    return CyCustErr.Ok;

                case CyPortConstants.SIOInfoValue_SECOND_IN_SIO_PAIR:
                    sioInfo = CyPinDataSIOType_v1.SecondInSIOPair;
                    return CyCustErr.Ok;

                case CyPortConstants.SIOInfoValue_SINGLE_SIO:
                    sioInfo = CyPinDataSIOType_v1.SingleSIO;
                    return CyCustErr.Ok;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSIOInfo, sioInfoString));
            }
        }

        public CyCustErr GetPORMode(ICyInstQuery_v1 instQuery, out CyPinDataPOR_v1 porMode)
        {
            porMode = default(CyPinDataPOR_v1);

            CyParamInfo.CyPOR por;
            CyCustErr err = CyParamInfo.GetPORValue(instQuery, out por);
            if (err.IsNotOk) return err;

            switch (por)
            {
                case CyParamInfo.CyPOR.Unspecified:
                    porMode = CyPinDataPOR_v1.Unspecified;
                    return CyCustErr.Ok;

                case CyParamInfo.CyPOR.HiZAnalog:
                    porMode = CyPinDataPOR_v1.HiZAnalog;
                    return CyCustErr.Ok;

                case CyParamInfo.CyPOR.PulledUp:
                    porMode = CyPinDataPOR_v1.PulledUp;
                    return CyCustErr.Ok;

                case CyParamInfo.CyPOR.PulledDown:
                    porMode = CyPinDataPOR_v1.PulledDown;
                    return CyCustErr.Ok;

                default:
                    return new CyCustErr(string.Format("Unhandled Power-On Reset Mode '{0}'.", por.ToString()));
            }
        }

        public CyCustErr GetAssociatedTerminalName(ICyTerminalQuery_v1 termQuery, string portTermBaseName, int index,
            out string termName, out int offset)
        {
            Dictionary<int, string> indexToTerminalNameMapping =
                CyPortCustomizer.GetAssociatedTerminals(portTermBaseName, termQuery);

            foreach (KeyValuePair<int, string> pair in indexToTerminalNameMapping)
            {
                int termWidth = termQuery.GetTermWidth(pair.Value);
                int leftIndex = pair.Key;
                int rightIndex = leftIndex - (termWidth - 1);

                if (rightIndex <= index && index <= leftIndex)
                {
                    //This is the correct terminal.
                    termName = pair.Value;
                    offset = index - rightIndex;
                    return CyCustErr.Ok;
                }
            }

            termName = null;
            offset = default(int);
            return new CyCustErr(cy_pins_v1_20.Resource1.TermNotFound);
        }

        public CyCustErr GetDriveMode(ICyInstQuery_v1 instQuery, int index, out CyPinDataDriveMode_v1 driveMode)
        {
            driveMode = default(CyPinDataDriveMode_v1);

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateDriveModeData(instQuery, true, out data);
            if (err.IsNotOk) return err;

            string driveModeString = data.GetValue(index);
            switch (driveModeString)
            {
                case CyPortConstants.DriveModeValue_ANALOG_HI_Z:
                    driveMode = CyPinDataDriveMode_v1.HighImpedanceAnalog;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_CMOS_OUT:
                    driveMode = CyPinDataDriveMode_v1.StrongDrive;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_DIGITAL_HI_Z:
                    driveMode = CyPinDataDriveMode_v1.HighImpedanceDigital;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_OPEN_DRAIN_HI:
                    driveMode = CyPinDataDriveMode_v1.OpenDrainDrivesHigh;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_OPEN_DRAIN_LO:
                    driveMode = CyPinDataDriveMode_v1.OpenDrainDrivesLow;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_RES_PULL_DOWN:
                    driveMode = CyPinDataDriveMode_v1.ResistivePullDown;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_RES_PULL_UP:
                    driveMode = CyPinDataDriveMode_v1.ResistivePullUp;
                    return CyCustErr.Ok;

                case CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN:
                    driveMode = CyPinDataDriveMode_v1.ReistivePullUpDown;
                    return CyCustErr.Ok;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledDriveMode, driveModeString));
            }
        }

        public CyCustErr GetSlewRate(ICyInstQuery_v1 instQuery, int index, out CyPinDataSlewRate_v1 slewRate)
        {
            slewRate = default(CyPinDataSlewRate_v1);

            CyStringArrayParamData data;
            CyCustErr err = CyStringArrayParamData.CreateOutputOnlyBitVectorData(instQuery, 
                CyOutputOnlyBitVectorParam.Formal_SlewRates, out data);
            if (err.IsNotOk) return err;

            string slewRateString = data.GetValue(index);
            switch (slewRateString)
            {
                case CyPortConstants.SlewRateValue_FAST:
                    slewRate = CyPinDataSlewRate_v1.Fast;
                    return CyCustErr.Ok;

                case CyPortConstants.SlewRateValue_SLOW:
                    slewRate = CyPinDataSlewRate_v1.Slow;
                    return CyCustErr.Ok;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledSlewRate, slewRateString));
            }
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1 Members

        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            StringBuilder toolTip = new StringBuilder();
            CyCustErr err;

            toolTip.Append(query.DefaultToolTipText + Environment.NewLine);

            byte width;
            err = CyParamInfo.GetNumPinsValue(query, out width);
            if (err.IsOk)
            {
                toolTip.Append("Width = " + width.ToString() + Environment.NewLine);
            }
            else
            {
                toolTip.Append("Width = " + string.Format("Error '{0}'.", err.Message) + Environment.NewLine);
            }

            CyParamInfo.CyPOR por;
            err = CyParamInfo.GetPORValue(query, out por);
            if (err.IsOk)
            {
                string porString;
                switch (por)
                {
                    case CyParamInfo.CyPOR.Unspecified:
                        porString = "Don't Care";
                        break;

                    case CyParamInfo.CyPOR.HiZAnalog:
                        porString = "High-Z Analog";
                        break;

                    case CyParamInfo.CyPOR.PulledUp:
                        porString = "Pulled-Up";
                        break;
                    case CyParamInfo.CyPOR.PulledDown:
                        porString = "Pulled-Down";
                        break;

                    default:
                        Debug.Fail("unhandled");
                        porString = string.Empty;
                        break;
                }

                toolTip.Append("Power-On Reset = " + porString + Environment.NewLine);
            }
            else
            {
                toolTip.Append("Power-On Reset = " + string.Format("Error '{0}'.", err.Message) + Environment.NewLine);
            }

            bool isContiguous;
            err = CyParamInfo.GetIsContiguousValue(query, out isContiguous);
            if (err.IsOk)
            {
                toolTip.Append("Layout Mode = " + ((isContiguous) ?
                    "Contiguous/Non-Spanning" : "Non-Contiguous/Spanning") + Environment.NewLine);
            }
            else
            {
                toolTip.Append("Layout Mode = " + string.Format("Error '{0}'.", err.Message) + Environment.NewLine);
            }

            toolTip.Append(Environment.NewLine);

            CyStringArrayParamData interruptData;
            err = CyStringArrayParamData.CreateStringArrayData(query, CyParamInfo.Formal_ParamName_InterruptMode,
                out interruptData);

            CyStringArrayParamData driveModeData;
            err = CyStringArrayParamData.CreateStringArrayData(query, CyParamInfo.Formal_ParamName_DriveMode,
                out driveModeData);

            CyStringArrayParamData ioVoltageData;
            err = CyStringArrayParamData.CreateStringArrayData(query, CyParamInfo.Formal_ParamName_IOVoltages,
                out ioVoltageData);

            for (int i = 0; i < width; i++)
            {
                string name = GetPinPoundDefineName(query, i);
                string driveMode = _getDriveModeString(driveModeData, i);
                string interruptMode = _getInterruptModeString(interruptData, i);
                string ioVoltage = _getIOVoltageString(ioVoltageData, i);

                toolTip.Append(string.Format("{0}: {1}{2}{3}{4}", name, driveMode, interruptMode, ioVoltage,
                    Environment.NewLine));
            }

            return toolTip.ToString().Trim();
        }

        static string _getIOVoltageString(CyStringArrayParamData ioVoltageData, int index)
        {
            if (ioVoltageData == null)
            {
                return string.Empty;
            }

            string voltage = ioVoltageData.GetValue(index);
            if (string.IsNullOrEmpty(voltage) ||
                CyPortConstants.DefaultIOVolatageValue.Equals(voltage, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }
            return string.Format(", {0}V", voltage);
        }

        static string _getInterruptModeString(CyStringArrayParamData interruptData, int index)
        {
            if (interruptData == null)
            {
                return string.Empty;
            }

            string mode = interruptData.GetValue(index);
            switch (mode)
            {
                case CyPortConstants.InterruptModeValue_FALLING_EDGE:
                    return ", Falling Edge";

                case CyPortConstants.InterruptModeValue_NONE:
                    return string.Empty;

                case CyPortConstants.InterruptModeValue_ON_CHANGE:
                    return ", Both Edges";

                case CyPortConstants.InterruptModeValue_RISING_EDGE:
                    return ", Rising Edge";

                default:
                    Debug.Fail("unhandled");
                    return string.Empty;
            }
        }

        static string _getDriveModeString(CyStringArrayParamData driveModeData, int index)
        {
            if (driveModeData == null)
            {
                return string.Empty;
            }

            string mode = driveModeData.GetValue(index);
            switch (mode)
            {
                case CyPortConstants.DriveModeValue_ANALOG_HI_Z:
                    return "High Impedance Analog";

                case CyPortConstants.DriveModeValue_CMOS_OUT:
                    return "Strong Drive";

                case CyPortConstants.DriveModeValue_DIGITAL_HI_Z:
                    return "High Impedance Digital";

                case CyPortConstants.DriveModeValue_OPEN_DRAIN_HI:
                    return "Open Drain, Drives High";

                case CyPortConstants.DriveModeValue_OPEN_DRAIN_LO:
                    return "Open Drain, Drives Low";

                case CyPortConstants.DriveModeValue_RES_PULL_DOWN:
                    return "Resistive Pull Down";

                case CyPortConstants.DriveModeValue_RES_PULL_UP:
                    return "Resistive Pull Up";

                case CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN:
                    return "Resistive Pull Up/Down";

                default:
                    Debug.Fail("unhandled");
                    return string.Empty;
            }
        }

        #endregion

        //-----------------------------

        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            //Only generate port level APIs for components that are contiguous and non-spanning.
            //All components get the aliases file with #defines.

            CyCustErr err;
            List<CyAPICustomizer> apiCustomizers = new List<CyAPICustomizer>(apis);

            string instanceName;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Builtin_ParamName_InstName,
                out instanceName);
            if (err.IsNotOk) return new CyAPICustomizer[] { };

            byte width;
            err = CyParamInfo.GetNumPinsValue(query, out width);
            if (err.IsNotOk) return new CyAPICustomizer[] { };

            #region API Gen Specific Params and Param Names

            string setDriveModeImplParamName = "SetDriveModeImpl_API_GEN";
            string driveModeImpl = string.Empty;
            for (byte count = 0; count < width; count++)
            {
                driveModeImpl += string.Format("\tCyPins_SetPinDriveMode({0}_{1}, mode);{2}",
                    instanceName, count, Environment.NewLine);
            }
            driveModeImpl = driveModeImpl.TrimEnd();

            string poundDefineMappingsParamName = "PoundDefineMappings_API_GEN";
            string poundDefineMappings = string.Empty;
            string poundDefineMappingsAliases = string.Empty;
            CyStringArrayParamData aliasData;
            err = CyStringArrayParamData.CreatePinAliasData(query, true, out aliasData);
            if (err.IsNotOk) return new CyAPICustomizer[] { };

            //Single underscores were required by TBEN.
            for (byte count = 0; count < width; count++)
            {
                poundDefineMappings += string.Format("#define {0}_{1}\t\t{0}__{1}__PC{2}",
                    instanceName, count, Environment.NewLine);

                string alias = aliasData.GetValue(count);
                if (string.IsNullOrEmpty(alias) == false)
                {
                    poundDefineMappingsAliases += string.Format("#define {0}_{1}\t\t{0}__{1}__PC{2}",
                    instanceName, alias, Environment.NewLine);
                }

            }
            poundDefineMappings += Environment.NewLine + poundDefineMappingsAliases;
            poundDefineMappings = poundDefineMappings.TrimEnd();

            #endregion

            Dictionary<string, string> paramDict;
            foreach (CyAPICustomizer file in apiCustomizers)
            {
                paramDict = file.MacroDictionary;
                paramDict[setDriveModeImplParamName] = driveModeImpl;
                paramDict[poundDefineMappingsParamName] = poundDefineMappings;
                file.MacroDictionary = paramDict;
            }

            bool isContiguous;
            CyCustErr contErr = CyParamInfo.GetIsContiguousValue(query, out isContiguous);

            bool isSpanning;
            CyCustErr spanningErr = CyParamInfo.GetIsSpanningValue(query, out isSpanning);

            if (contErr.IsOk && spanningErr.IsOk && isContiguous && isSpanning == false)
            {
                //Return all the APIs.
                return apiCustomizers;
            }
            else
            {
                //Only return the aliases file
                foreach (CyAPICustomizer cust in apis)
                {
                    string filename = Path.GetFileName(cust.OriginalName);
                    if (filename.Equals("aliases.h"))
                    {
                        return new CyAPICustomizer[] { cust };
                    }
                }

                Debug.Fail("shouldn't get here");
                return new CyAPICustomizer[] { };
            }
        }

        #endregion

        //-----------------------------

        #region Unit Tests

        const int TOTAL_NUM_TEST_PINS = 13;

        /// <summary>
        /// Tests editing parameters and checking that other paramaters get updated as expected.
        /// </summary>
        /// <param name="instEdit"></param>
        protected static void BaseRunUnitTest(ICyInstEdit_v1 instEdit)
        {
            CheckDefaultValueSettings(instEdit);
            CheckDirectionRestrictSettings(instEdit);
            CheckSIO(instEdit);
        }

        /// <summary>
        /// Check the default value of certain parameters (InputBufferEnabled, DriveMode, InitDriveState)
        /// are set correctly.
        /// </summary>
        /// <param name="instEdit"></param>
        static void CheckDefaultValueSettings(ICyInstEdit_v1 instEdit)
        {
            SetupDefaultValueTesting(instEdit);
            CheckInputBufferEnabled(instEdit);
            CheckDriveModes(instEdit);
            CheckInitDriveStates(instEdit);
        }

        /// <summary>
        /// Check Input/Output only settings are set to default when pin types are set backwards.
        /// Some parameters only applied to input pins while others only applied to output pins.
        /// This test is to make sure, if the pin type is set to input, all the parameters applied to
        /// output pin only  will be set to default regardless whatever the user set them to and vise versa.
        /// </summary>
        /// <param name="instEdit"></param>
        static void CheckDirectionRestrictSettings(ICyInstEdit_v1 instEdit)
        {
            CheckInputRestrictSettings(instEdit);
            CheckOutputRestrictSettings(instEdit);
        }

        /// <summary>
        /// Check all the rules applied to SIO pins.
        /// </summary>
        /// <param name="instEdit"></param>
        static void CheckSIO(ICyInstEdit_v1 instEdit)
        {
            bool result;
            string defaultSetupValue;

            //======================================================================================================
            //I'am an SIO when
            //Input && HotSwap == true ||
            string hotSwapsValue = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                                                   CyPortConstants.HotSwapValue_TRUE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_TRUE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_TRUE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_FALSE,
                                                   CyPortConstants.HotSwapValue_TRUE);

            string[] IsSIOHotSwapExpectedValues = new string[] {CyPortConstants.SIOInfoValue_SINGLE_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                         CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO,
                                                         CyPortConstants.SIOInfoValue_NOT_SIO};

            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, hotSwapsValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_SIOInfo, IsSIOHotSwapExpectedValues);

            //Set HotSwaps back to default value.
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultHotSwapValue, out defaultSetupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, defaultSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //======================================================================================================
            //Input && Input Threshold == .5 VDDIO, .4 VDDIO,  .5Vref, Vref ||
            string thresholdLevelsValue =
                string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                              CyPortConstants.ThresholdLevelValue_PT5_VDDIO,
                              CyPortConstants.ThresholdLevelValue_PT4_VDDIO,
                              CyPortConstants.ThresholdLevelValue_PT5_VREF,
                              CyPortConstants.ThresholdLevelValue_VREF,
                              CyPortConstants.ThresholdLevelValue_CMOS,
                              CyPortConstants.ThresholdLevelValue_CMOS_LVTTL,
                              CyPortConstants.ThresholdLevelValue_CMOS,
                              CyPortConstants.ThresholdLevelValue_CMOS,
                              CyPortConstants.ThresholdLevelValue_PT5_VDDIO,
                              CyPortConstants.ThresholdLevelValue_PT4_VDDIO,
                              CyPortConstants.ThresholdLevelValue_CMOS,
                              CyPortConstants.ThresholdLevelValue_LVTTL,
                              CyPortConstants.ThresholdLevelValue_PT4_VDDIO);

            string[] IsSIOThresholdLevelsExpectedValue = new string[] {CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                                  CyPortConstants.SIOInfoValue_SINGLE_SIO,
                                                                  CyPortConstants.SIOInfoValue_SINGLE_SIO,
                                                                  CyPortConstants.SIOInfoValue_SINGLE_SIO,
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                                  CyPortConstants.SIOInfoValue_NOT_SIO};

            result =
               instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_ThresholdLevels, thresholdLevelsValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_SIOInfo, IsSIOThresholdLevelsExpectedValue);

            //======================================================================================
            //Input Buffer Check.

            //Set up the HotSwaps to match threshold to achieve our test purpose. 
            //Set HotSwap == True so ThresholdLevel == CMOS will make SIO_InputBuffer SingleEnded.
            //Set HotSwap == False so ThresholdLevel == LVTTL will not throw error.
            string hotSwapsForThresholdLevelValue =
                string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_TRUE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_TRUE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE,
                              CyPortConstants.HotSwapValue_FALSE);

            result =
                instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, hotSwapsForThresholdLevelValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //SIO_InputBuffer
            //if(Threshold == CMOS) then SignleEnded
            //else if(Threshold != LVTTL) then Differential
            //else if (threshold == LVTTL) then not used.
            string[] SIOInputBufferExpectedValues = new string[] {CyPortConstants.SIOInputBufferValue_DIFFERENTIAL, 
                                                                  CyPortConstants.SIOInputBufferValue_DIFFERENTIAL, 
                                                                  CyPortConstants.SIOInputBufferValue_DIFFERENTIAL,
                                                                  CyPortConstants.SIOInputBufferValue_DIFFERENTIAL,
                                                                  CyPortConstants.SIOInputBufferValue_SINGLE_ENDED,
                                                                  CyPortConstants.SIOInputBufferValue_SINGLE_ENDED};

            //Check SIO_InputBuffer
            CheckWarpValuesBitVector(instEdit, "sio_ibuf", SIOInputBufferExpectedValues);

            //Set HotSwap and threshold back to default value.
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultHotSwapValue, out defaultSetupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, defaultSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultThresholdLevelValue, out defaultSetupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_ThresholdLevels, defaultSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //======================================================================================
            //I'm an SIO when
            //Ouput && Output Drive Level == Vref || Output && Input Sink Current == 25mA
            string driveLevelsValue =
                string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO);

            string outputDriveCurrentValue =
                 string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                              CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                              CyPortConstants.DriveCurrentValue_4SOURCE_8SINK);

            string[] IsSIOVrefExpectedValues = new string[] {CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO,
                                                             CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_SINGLE_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO, 
                                                             CyPortConstants.SIOInfoValue_NOT_SIO};

            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputDriveLevels, driveLevelsValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_DriveCurrents, outputDriveCurrentValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_SIOInfo, IsSIOVrefExpectedValues);

            //=====================================================================================
            //Check SIO_OutputBuffer
            //If(DriveLevel == Vref) Then Regulated
            //If(DriveLevel == VDDIO), then Unregulated]
            string[] SIOOutputBufferExpectedValues = new string[] {CyPortConstants.SIOOutputBufferValue_REGULATED, 
                                                                   CyPortConstants.SIOOutputBufferValue_UNREGULATED,
                                                                   CyPortConstants.SIOOutputBufferValue_REGULATED,
                                                                   CyPortConstants.SIOOutputBufferValue_UNREGULATED};

            CheckWarpValuesBitVector(instEdit, "sio_obuf", SIOOutputBufferExpectedValues);

            //set Output Driver level back to default
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultOutputDriveLevelValue, out defaultSetupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputDriveLevels, defaultSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //set Output Driver level back to default
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultDriveCurrentValue, out defaultSetupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_DriveCurrents, defaultSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();
        }

        /// <summary>
        /// Used to set up a test case with all input pin types and check the set up is complete and correct.
        /// </summary>
        /// <param name="instEdit"></param>
        static void SetupDefaultValueTesting(ICyInstEdit_v1 instEdit)
        {
            CyCustErr err;
            bool result;

            //The pin will be set up in the following order:

            //Digital In
            // DIGIN
            // DIGIN_ANALOG

            //Digital In/Out
            //BIDIRECTIONAL
            //BIDIRECTIONAL_ANALOG
            //DIGOUT_DIGIN
            //DIGOUT_DIGIN_ANALOG
            //DIGOUT_DIGIN_OE
            //DIGOUT_OE_DIGIN_ANALOG

            //Digital Out
            //DIGOUT
            //DIGOUT_ANALOG
            //DIGOUT_OE
            //DIGOUT_OE_ANALOG

            //Analog
            //ANALOG

            //We need 13 pins here so the layout mode has to be set to span to achive this
            result =
                instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_LayoutMode, CyPortConstants.LayoutModeValue_CONT_SPANNING);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string pins = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                CyPortConstants.PinTypesValue_DIGIN, CyPortConstants.PinTypesValue_DIGIN_ANALOG,
                CyPortConstants.PinTypesValue_BIDIRECTIONAL, CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG,
                CyPortConstants.PinTypesValue_DIGOUT_DIGIN, CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG,
                CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE, CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG,
                CyPortConstants.PinTypesValue_DIGOUT, CyPortConstants.PinTypesValue_DIGOUT_ANALOG,
                CyPortConstants.PinTypesValue_DIGOUT_OE, CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG,
                CyPortConstants.PinTypesValue_ANALOG);

            //Set number of pins.
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_NumPins, TOTAL_NUM_TEST_PINS.ToString());
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //Check if it is set up correctly.
            byte numPins;
            err = CyParamInfo.GetNumPinsValue(instEdit, out numPins);
            Trace.Assert(err.IsOk, err.Message);
            Trace.Assert(numPins == TOTAL_NUM_TEST_PINS);

            //Set up all the pin types at once.
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_PinTypes, pins);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //Check the pin set up is correct.
            string[] expectedValues = pins.Split(',');
            CheckWarpValuesNonBitVector(instEdit, "pin_types", expectedValues);
        }

        static void CheckInputBufferEnabled(ICyInstEdit_v1 instEdit)
        {
            //From the test settings, the default value for buffer enabled should be 
            //True for the first 8 pins, false for the rest 5 pins).
            string[] expectedValues = new string[] {CyPortConstants.InputBufferEnabledValue_TRUE, 
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_TRUE,
                                                    CyPortConstants.InputBufferEnabledValue_FALSE};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_InputBuffersEnabled, expectedValues);
        }

        static void CheckDriveModes(ICyInstEdit_v1 instEdit)
        {
            string[] expectedValues = new string[] {CyPortConstants.DriveModeValue_DIGITAL_HI_Z, 
                                                    CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                                                    CyPortConstants.DriveModeValue_OPEN_DRAIN_LO, 
                                                    CyPortConstants.DriveModeValue_OPEN_DRAIN_LO,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT, 
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_ANALOG_HI_Z};
            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_DriveMode, expectedValues);
        }

        static void CheckInitDriveStates(ICyInstEdit_v1 instEdit)
        {
            bool result;

            string driveModes = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                CyPortConstants.DriveModeValue_CMOS_OUT,
                CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                CyPortConstants.DriveModeValue_OPEN_DRAIN_HI,
                CyPortConstants.DriveModeValue_OPEN_DRAIN_LO,
                CyPortConstants.DriveModeValue_RES_PULL_DOWN,
                CyPortConstants.DriveModeValue_RES_PULL_UP,
                CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN,
                CyPortConstants.DriveModeValue_OPEN_DRAIN_HI,
                CyPortConstants.DriveModeValue_OPEN_DRAIN_LO,
                CyPortConstants.DriveModeValue_RES_PULL_UP,
                CyPortConstants.DriveModeValue_ANALOG_HI_Z);

            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_DriveMode, driveModes);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] expectedValues = new string[] {CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_HIGH,
                                                    CyPortConstants.InitialDriveStateValue_HIGH,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_LOW,
                                                    CyPortConstants.InitialDriveStateValue_HIGH,
                                                    CyPortConstants.InitialDriveStateValue_LOW};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_InitialDriveStates, expectedValues);

            string defaultDriveMode = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                                                    CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                                                    CyPortConstants.DriveModeValue_DIGITAL_HI_Z,
                                                    CyPortConstants.DriveModeValue_OPEN_DRAIN_LO,
                                                    CyPortConstants.DriveModeValue_OPEN_DRAIN_LO,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_CMOS_OUT,
                                                    CyPortConstants.DriveModeValue_ANALOG_HI_Z);

            //Set it back to default value in case it messed up the later test.
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InitialDriveStates, defaultDriveMode);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();
        }

        static void CheckInputRestrictSettings(ICyInstEdit_v1 instEdit)
        {
            bool result;
            string setupValue;

            //Input only parameters:
            //DisplayInputHWConnections(We don't test this parameter because we don't set it in Warp.It's front-end only).
            //HotSwaps
            //InputBuffersEnabled
            //InputSyncs
            //InterruptModes
            //ThresholdLevels

            //For all the above parameters, if pinType is one of the input types, 
            //they should be set to the user's preference. 
            //Otherwise, they should be set to default values.
            //From the test settings, the first 8 pins contains input types, so they should be set to user's preference,
            //the rest 5 pins contains output types only so they should be set to default.

            //HotSwaps=========================================================================================
            string hotswapSetupValue =
                string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_TRUE,
                CyPortConstants.HotSwapValue_FALSE,
                CyPortConstants.HotSwapValue_FALSE);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, hotswapSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] hotSwapsExpectedValues = new string[] {CyPortConstants.HotSwapValue_TRUE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_TRUE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_TRUE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_TRUE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_FALSE, 
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_FALSE,
                                                            CyPortConstants.HotSwapValue_FALSE};

            Check(instEdit, CyParamInfo.Formal_ParamName_HotSwaps, hotSwapsExpectedValues);

            //Restore back to default
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultHotSwapValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_HotSwaps, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //InputBuffersEnabled===================================================================================

            CreateUnitTestParamArrayValue(CyPortConstants.InputBufferEnabledValue_FALSE, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InputBuffersEnabled, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] InputBufferEnabledExpectedValues = new string[] {CyPortConstants.InputBufferEnabledValue_FALSE, 
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE,
                                                                      CyPortConstants.InputBufferEnabledValue_TRUE,
                                                                      CyPortConstants.InputBufferEnabledValue_TRUE,
                                                                      CyPortConstants.InputBufferEnabledValue_TRUE,
                                                                      CyPortConstants.InputBufferEnabledValue_TRUE,
                                                                      CyPortConstants.InputBufferEnabledValue_FALSE};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_InputBuffersEnabled, InputBufferEnabledExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.InputBufferEnabledValue_TRUE, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InputBuffersEnabled, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //InputSyncs==========================================================================================
            CreateUnitTestParamArrayValue(CyPortConstants.InputsSynchronizedValue_DISABLED, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InputsSynchronized, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] InputSyncsExpectedValues = new string[] {CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_DISABLED,
                                                              CyPortConstants.InputsSynchronizedValue_ENABLED,
                                                              CyPortConstants.InputsSynchronizedValue_ENABLED,
                                                              CyPortConstants.InputsSynchronizedValue_ENABLED,
                                                              CyPortConstants.InputsSynchronizedValue_ENABLED,
                                                              CyPortConstants.InputsSynchronizedValue_ENABLED};


            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_InputsSynchronized, InputSyncsExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultInputsSynchronizedValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InputsSynchronized, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //InterruptModes===================================================================================
            CreateUnitTestParamArrayValue(CyPortConstants.InterruptModeValue_RISING_EDGE, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InterruptMode, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] InterruptModesExpectedValues = new string[] {CyPortConstants.InterruptModeValue_RISING_EDGE, 
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_RISING_EDGE,
                                                                  CyPortConstants.InterruptModeValue_NONE,
                                                                  CyPortConstants.InterruptModeValue_NONE,
                                                                  CyPortConstants.InterruptModeValue_NONE,
                                                                  CyPortConstants.InterruptModeValue_NONE,
                                                                  CyPortConstants.InterruptModeValue_NONE};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_InterruptMode, InterruptModesExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultInterruptModeValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_InterruptMode, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();


            //ThresholdLevels===================================================================================
            CreateUnitTestParamArrayValue(CyPortConstants.ThresholdLevelValue_LVTTL, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_ThresholdLevels, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] ThresholdLevelsExpectedValues = new string[] {CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_CMOS_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_CMOS_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_CMOS_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_CMOS_LVTTL,
                                                                   CyPortConstants.ThresholdLevelValue_CMOS_LVTTL};

            Check(instEdit, CyParamInfo.Formal_ParamName_ThresholdLevels, ThresholdLevelsExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultThresholdLevelValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_ThresholdLevels, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();
        }

        static void CheckOutputRestrictSettings(ICyInstEdit_v1 instEdit)
        {
            bool result;
            string setupValue;

            //Output only parameters:
            //DisplayOutputHWConnections(We don't test this parameter because we don't set it in Warp.)
            //DriveCurrents
            //OutputDriveLevels
            //OutputSyncs
            //SlewRates

            //For all the above parameters, if pinType is one of the output types, 
            //they should be set to the user's preference.
            //Otherwise, they should be set to default values.
            //From the test settings, the first 2 pins contains input types only, so they should be set to default,
            //the rest 10 pins contains output types, so they should be set to use's preference. The last one
            //is analog, so should be set to default.

            //DriveCurrents===================================================================================
            string driveCurrentsSetupValue =
                string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                CyPortConstants.DriveCurrentValue_4SOURCE_8SINK);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_DriveCurrents, driveCurrentsSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] driveCurrentsExpectedValues = new string[] {CyPortConstants.DriveCurrentValue_4SOURCE_8SINK, 
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_25SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK,
                                                                 CyPortConstants.DriveCurrentValue_4SOURCE_8SINK};

            Check(instEdit, CyParamInfo.Formal_ParamName_DriveCurrents, driveCurrentsExpectedValues);

            //Set back to default.
            CreateUnitTestParamArrayValue(CyPortConstants.DefaultDriveCurrentValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_DriveCurrents, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //OutputDriveLevels==================================================================================
            string driveLevelSetupValue =
               string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}",
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VREF,
                              CyPortConstants.OutputDriveLevelValue_VDDIO,
                              CyPortConstants.OutputDriveLevelValue_VDDIO);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputDriveLevels, driveLevelSetupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] outputDriveLevelsExpectedValues = new string[] {CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VREF,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VREF,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VREF,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VREF,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VREF,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO,
                                                                     CyPortConstants.OutputDriveLevelValue_VDDIO};

            Check(instEdit, CyParamInfo.Formal_ParamName_OutputDriveLevels, outputDriveLevelsExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultOutputDriveLevelValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputDriveLevels, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //OutputSyncs=========================================================================================
            CreateUnitTestParamArrayValue(CyPortConstants.OutputsSynchronizedValue_ENABLED, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputsSynchronized, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] outputSyncsExpectedValues = new string[] {CyPortConstants.OutputsSynchronizedValue_DISABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_DISABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_ENABLED,
                                                               CyPortConstants.OutputsSynchronizedValue_DISABLED};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_OutputsSynchronized, outputSyncsExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultOutputsSynchronizedValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_OutputsSynchronized, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            //SlewRates=====================================================================================
            CreateUnitTestParamArrayValue(CyPortConstants.SlewRateValue_SLOW, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_SlewRate, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();

            string[] slewRatesExpectedValues = new string[] {CyPortConstants.SlewRateValue_FAST, 
                                                             CyPortConstants.SlewRateValue_FAST,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_SLOW,
                                                             CyPortConstants.SlewRateValue_FAST};

            CheckWarpValuesNonBitVector(instEdit, CyParamInfo.Local_ParamName_SlewRate, slewRatesExpectedValues);

            CreateUnitTestParamArrayValue(CyPortConstants.DefaultSlewRateValue, out setupValue);
            result = instEdit.SetParamExpr(CyParamInfo.Formal_ParamName_SlewRate, setupValue);
            Trace.Assert(result, "param was not set");
            instEdit.CommitParamExprs();
        }

        /// <summary>
        /// This function is used to check paramters which never gets send to warp. 
        /// These params (HotSwaps, ThresholdLevels, etc) are usually used to determine other params
        /// which are needed by warp. So verify with this method.
        /// </summary>
        /// <param name="instEdit"></param>
        /// <param name="paramName"></param>
        /// <param name="expectedVals"></param>
        static void Check(ICyInstEdit_v1 instEdit, string paramName, string[] expectedVals)
        {
            CyStringArrayParamData values;
            CyCustErr err = CyStringArrayParamData.CreateStringArrayData(instEdit, paramName, out values);
            Trace.Assert(err.IsOk);

            for (int i = 0; i < expectedVals.Length; i++)
            {
                Trace.Assert(values.GetValue(i).Trim() == expectedVals[i].Trim());
            }
        }

        /// <summary>
        /// This method is used to check some params which get send to warp but they are not bit vectors.
        /// </summary>
        /// <param name="instEdit"></param>
        /// <param name="warpName"></param>
        /// <param name="expectedValues"></param>
        static void CheckWarpValuesNonBitVector(ICyInstEdit_v1 instEdit, string warpName, string[] expectedValues)
        {
            CyCustErr err;
            string rawData;
            err = GetParamValue<string>(instEdit, warpName, out rawData);
            Trace.Assert(err.IsOk, "Fail to get value back from warp");
            string[] values = rawData.Split(',');
            Trace.Assert(values.Length == expectedValues.Length, "Fail to correctly split values");

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Trace.Assert(values[i].Trim() == expectedValues[i].Trim());
            }
        }

        /// <summary>
        /// This method is used to check some params which get send to warp and they are bit vectors.
        /// </summary>
        /// <param name="instEdit"></param>
        /// <param name="warpName"></param>
        /// <param name="expectedValues"></param>
        static void CheckWarpValuesBitVector(ICyInstEdit_v1 instEdit, string warpName, string[] expectedValues)
        {
            CyCustErr err;
            string rawData;
            err = GetParamValue<string>(instEdit, warpName, out rawData);
            Trace.Assert(err.IsOk, "Fail to get value back from warp");
            string[] tempValues = rawData.Split('b');
            Trace.Assert(tempValues.Length > 0);
            string[] values = tempValues[tempValues.Length - 1].Split('_');
            Trace.Assert(values.Length == expectedValues.Length, "Fail to correctly split values");

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Trace.Assert(values[i].Trim() == expectedValues[i].Trim());
            }
        }

        static void CreateUnitTestParamArrayValue(string paramValue, out string paramValues)
        {
            string[] values = new string[TOTAL_NUM_TEST_PINS];
            for (int i = 0; i < TOTAL_NUM_TEST_PINS; i++)
            {
                values[i] = paramValue;
            }
            paramValues = string.Join(",", values);
        }
        #endregion

        //-----------------------------
    }

    #region Component Info

    class CyTermInfo
    {
        public const string RootTermName_OE = "oe";
        public const string RootTermName_Output = "y";
        public const string RootTermName_Input = "fb";
        public const string RootTermName_Analog = "analog";
        public const string RootTermName_BiDir = "io";
        public const string RootTermName_SIOVRef = "siovref";
        public const string RootTermName_Interrupt = "interrupt";

        public static IEnumerable<string> TerminalRootNames
        {
            get
            {
                yield return RootTermName_OE;
                yield return RootTermName_Output;
                yield return RootTermName_Input;
                yield return RootTermName_Analog;
                yield return RootTermName_BiDir;
                yield return RootTermName_SIOVRef;
                yield return RootTermName_Interrupt;
            }
        }

        public static CyCustErr GetExpectedTerminalWidth(ICyInstQuery_v1 query, string rootTermName, out byte numPins)
        {
            if (rootTermName == RootTermName_OE ||
                rootTermName == RootTermName_Output ||
                rootTermName == RootTermName_Input ||
                rootTermName == RootTermName_Analog ||
                rootTermName == RootTermName_BiDir)
            {
                return CyParamInfo.GetNumPinsValue(query, out numPins);
            }

            if (rootTermName == RootTermName_SIOVRef)
            {
                return CyParamInfo.GetSIOGroupCntValue(query, out numPins);
            }

            if (rootTermName == RootTermName_Interrupt)
            {
                numPins = 1;
                return CyCustErr.OK;
            }

            numPins = default(byte);
            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnknownTerm, rootTermName));
        }

        internal static bool IsInput(string rootTermName)
        {
            return (rootTermName == RootTermName_Output || rootTermName == RootTermName_OE);
        }
        internal static bool IsAnalog(string rootTermName)
        {
            return (rootTermName == RootTermName_Analog || rootTermName == RootTermName_SIOVRef);
        }

    }

    class CyParamInfo
    {
        public enum CyPOR
        {
            Unspecified,
            HiZAnalog,
            PulledUp,
            PulledDown,
        }

        const int AcessModeValue_SW_ONLY = 1;
        const int AcessModeValue_HW_ONLY = 2;

        public const int PORValue_Unspecified = 4;
        public const int PORValue_HiZAnalog = 0;
        public const int PORValue_PulledUp = 2;
        public const int PORValue_PulledDown = 3;
        public const int DefaultPORValue = PORValue_Unspecified;

        public const string Builtin_ParamName_InstName = "INSTANCE_NAME";

        public const string Formal_ParamName_NumPins = "NumPins";
        public const string Formal_ParamName_PinTypes = "PinTypes";
        public const string Formal_ParamName_DisplayAsBus = "DisplayAsBus";
        public const string Formal_ParamName_SIOGroups = "SIOGroups";
        public const string Formal_ParamName_HotSwaps = "HotSwaps";
        public const string Formal_ParamName_ThresholdLevels = "ThresholdLevels";
        public const string Formal_ParamName_OutputDriveLevels = "OutputDriveLevels";
        public const string Formal_ParamName_DriveCurrents = "DriveCurrents";
        public const string Formal_ParamName_DisplayInputHWConnections = "DisplayInputHWConnections";
        public const string Formal_ParamName_DisplayOutputHWConnections = "DisplayOutputHWConnections";
        public const string Formal_ParamName_InputBuffersEnabled = "InputBuffersEnabled";
        public const string Formal_ParamName_LayoutMode = "LayoutMode";
        public const string Formal_ParamName_SlewRate = "SlewRates";
        public const string Formal_ParamName_InterruptMode = "InterruptModes";
        public const string Formal_ParamName_DriveMode = "DriveModes";
        public const string Formal_ParamName_PinAlisases = "PinAliases";
        public const string Formal_ParamName_InitialDriveStates = "InitialDriveStates";
        public const string Formal_ParamName_InputsSynchronized = "InputSyncs";
        public const string Formal_ParamName_IOVoltages = "IOVoltages";
        public const string Formal_ParamName_OutputsSynchronized = "OutputSyncs";
        public const string Formal_ParamName_PowerOnReset = "PowerOnResetState";

        public const string Local_ParamName_IsHomogeneous_v2 = "IsHomogeneous_v2";
        public const string Local_ParamName_UsesInterrupts = "UsesInterrupts";
        public const string Local_ParamName_InputBuffersEnabled = "ibuf_enabled";
        public const string Local_ParamName_SIOGroupCnt = "sio_group_cnt";
        public const string Local_ParamName_SlewRate = "slew_rate";
        public const string Local_ParamName_InterruptMode = "intr_mode";
        public const string Local_ParamName_DriveMode = "drive_mode";
        public const string Local_ParamName_SIOHysteresis = "sio_hyst";
        public const string Local_ParamName_PinAlisases = "pin_aliases";
        public const string Local_ParamName_InitialDriveStates = "init_dr_st";
        public const string Local_ParamName_InputsSynchronized = "input_sync";
        public const string Local_ParamName_IOVoltages = "io_voltage";
        public const string Local_ParamName_OutputsSynchronized = "output_sync";
        public const string Local_ParamName_SIOInfo = "sio_info";
        public const string Local_ParamName_VTrip = "vtrip";
        public const string Local_ParamName_SIORefSels = "sio_refsel";
        public const string Local_ParamName_PinTypes = "pin_types";

        public static CyCustErr GetPORValue(ICyInstQuery_v1 query, out CyPOR por)
        {
            por = CyPOR.Unspecified;
            int porValue;
            CyCustErr err = CyPortCustomizer.GetParamValue<int>(query, Formal_ParamName_PowerOnReset, out porValue);

            //Get an much info as possible. Only bail if a syntax error.
            if (porValue == PORValue_HiZAnalog) por = CyPOR.HiZAnalog;
            else if (porValue == PORValue_PulledDown) por = CyPOR.PulledDown;
            else if (porValue == PORValue_PulledUp) por = CyPOR.PulledUp;
            else if (porValue == PORValue_Unspecified) por = CyPOR.Unspecified;
            else return new CyCustErr(string.Format("Unhandled Power-On Reset mode '{0}'.", porValue));
            return err;
        }

        public static CyCustErr GetDisplayAsBusValue(ICyInstQuery_v1 query, out bool displayAsBus)
        {
            return CyPortCustomizer.GetParamValue<bool>(query, Formal_ParamName_DisplayAsBus, out displayAsBus);
        }

        public static CyCustErr GetNumPinsValue(ICyInstQuery_v1 query, out byte numPins)
        {
            return CyPortCustomizer.GetParamValue<byte>(query, Formal_ParamName_NumPins, out numPins);
        }

        public static CyCustErr GetNumPinsValue(ICyInstValidate_v1 query, out byte numPins)
        {
            return CyPortCustomizer.GetParamValue<byte>(query, Formal_ParamName_NumPins, out numPins);
        }

        public static CyCustErr GetSIOGroupCntValue(ICyInstQuery_v1 query, out byte count)
        {
            return CyPortCustomizer.GetParamValue<byte>(query, Local_ParamName_SIOGroupCnt, out count);
        }

        public static CyCustErr GetUsesInterruptsValue(ICyInstQuery_v1 query, out bool usesInterrupts)
        {
            return CyPortCustomizer.GetParamValue<bool>(query, Local_ParamName_UsesInterrupts, out usesInterrupts);
        }

        public static CyCustErr GetIsHomogeneousValue_v2(ICyInstQuery_v1 query, out bool isHomogeneous)
        {
            return CyPortCustomizer.GetParamValue<bool>(query, Local_ParamName_IsHomogeneous_v2, out isHomogeneous);
        }

        public static CyCustErr GetIsContiguousValue(ICyInstQuery_v1 query, out bool isContiguous)
        {
            isContiguous = default(bool);

            int mode;
            CyCustErr err = CyPortCustomizer.GetParamValue<int>(query, Formal_ParamName_LayoutMode, out mode);

            //Get an much info as possible. Only bail if a syntax error.
            switch (mode.ToString())
            {
                case CyPortConstants.LayoutModeValue_CONT_SPANNING:
                case CyPortConstants.LayoutModeValue_CONT_NONSPANNING:
                    isContiguous = true;
                    break;

                case CyPortConstants.LayoutModeValue_NONCONT_NONSPANNING:
                case CyPortConstants.LayoutModeValue_NONCONT_SPANNING:
                    isContiguous = false;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledLayoutMode, mode));
            }
            return err;
        }

        public static CyCustErr GetIsContiguousValue(ICyInstValidate_v1 instVal, out bool isContiguous)
        {
            isContiguous = default(bool);

            int mode;
            CyCustErr err = CyPortCustomizer.GetParamValue<int>(instVal, Formal_ParamName_LayoutMode, out mode);

            //Get an much info as possible. Only bail if a syntax error.
            switch (mode.ToString())
            {
                case CyPortConstants.LayoutModeValue_CONT_SPANNING:
                case CyPortConstants.LayoutModeValue_CONT_NONSPANNING:
                    isContiguous = true;
                    break;

                case CyPortConstants.LayoutModeValue_NONCONT_NONSPANNING:
                case CyPortConstants.LayoutModeValue_NONCONT_SPANNING:
                    isContiguous = false;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledLayoutMode, mode));
            }
            return err;
        }

        public static CyCustErr GetIsSpanningValue(ICyInstQuery_v1 query, out bool isSpanning)
        {
            isSpanning = default(bool);

            int mode;
            CyCustErr err = CyPortCustomizer.GetParamValue<int>(query, Formal_ParamName_LayoutMode, out mode);

            //Get an much info as possible. Only bail if a syntax error.
            switch (mode.ToString())
            {
                case CyPortConstants.LayoutModeValue_CONT_SPANNING:
                case CyPortConstants.LayoutModeValue_NONCONT_SPANNING:
                    isSpanning = true;
                    break;

                case CyPortConstants.LayoutModeValue_NONCONT_NONSPANNING:
                case CyPortConstants.LayoutModeValue_CONT_NONSPANNING:
                    isSpanning = false;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledLayoutMode, mode));
            }
            return err;
        }

        public static CyCustErr GetIsSpanningValue(ICyInstValidate_v1 instVal, out bool isSpanning)
        {
            isSpanning = default(bool);

            int mode;
            CyCustErr err = CyPortCustomizer.GetParamValue<int>(instVal, Formal_ParamName_LayoutMode, out mode);

            //Get an much info as possible. Only bail if a syntax error.
            switch (mode.ToString())
            {
                case CyPortConstants.LayoutModeValue_CONT_SPANNING:
                case CyPortConstants.LayoutModeValue_NONCONT_SPANNING:
                    isSpanning = true;
                    break;

                case CyPortConstants.LayoutModeValue_NONCONT_NONSPANNING:
                case CyPortConstants.LayoutModeValue_CONT_NONSPANNING:
                    isSpanning = false;
                    break;

                default:
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledLayoutMode, mode));
            }
            return err;
        }
    }

    #endregion

    #region Array Type Data Classes

    enum CyBitVectorParam
    {
        Formal_DisplayInputConnections,
        Formal_DisplayOutputConnections,
        Formal_PinTypes,
        Local_SIORefSels,
        Formal_SIOGroups,

        Local_SIOHysteresis,
        Local_SIOInfo,
        Local_PinTypes,
        Local_VTrips,
    }

    enum CyInputOnlyBitVectorParam
    {
        Formal_HotSwaps,
        Formal_InputsSynchronized,
        Formal_InterruptModes,
        Formal_ThresholdLevels,

        Local_InterruptModes,
        Local_InputsSynchronized,
    }

    enum CyOutputOnlyBitVectorParam
    {
        Formal_DriveCurrents,
        Formal_OutputDriveLevels,
        Formal_OutputsSynchronized,
        Formal_SlewRates,

        Local_OutputsSynchronized,
        Local_SlewRates,
    }

    internal abstract class CyStringArrayParamData
    {
        string m_defaultValue;
        bool m_useLastValueAsDefault;
        List<string> m_values;

        protected CyStringArrayParamData(string data, string defaultValue, bool useLastValueAsDefault)
        {
            m_defaultValue = defaultValue;
            m_useLastValueAsDefault = useLastValueAsDefault;
            m_values = new List<string>(data.Split(','));

            for (int i = 0; i < m_values.Count; i++)
            {
                m_values[i] = m_values[i].Trim();
            }
        }

        /// <summary>
        /// Used to determine how long each entery should be.
        /// </summary>
        protected string ExampleValue
        {
            get { return m_defaultValue; }
        }

        protected virtual string GetDefaultValue(int index)
        {
            return m_defaultValue;
        }

        protected bool UseLastValueAsDefault
        {
            get { return m_useLastValueAsDefault; }
        }

        public int Count
        {
            get { return m_values.Count; }
        }

        public string this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        public string GetValue(int index)
        {
            string valueAsString = _getLiteralValue(index);

            if (string.IsNullOrEmpty(valueAsString))
            {
                valueAsString = GetDefaultValue(index);
            }

            return valueAsString;
        }

        /// <summary>
        /// Gets the value at the specified index but doesn't convert the empty string to the default value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string _getLiteralValue(int index)
        {
            string valueAsString = string.Empty;

            if (index < m_values.Count)
            {
                valueAsString = m_values[index];

                if (m_useLastValueAsDefault && string.IsNullOrEmpty(valueAsString))
                {
                    for (int i = index - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(m_values[i]) == false)
                        {
                            valueAsString = m_values[i];
                            break;
                        }
                    }
                }
            }
            else if (m_useLastValueAsDefault && m_values.Count > 0)
            {
                for (int i = m_values.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(m_values[i]) == false)
                    {
                        valueAsString = m_values[i];
                        break;
                    }
                }
            }

            return valueAsString;
        }

        public void SetValue(int index, string value)
        {
            while (index >= m_values.Count)
            {
                m_values.Add(string.Empty);
            }
            m_values[index] = value;
        }

        /// <summary>
        /// Sets the value at the specified index (and possibly others) to perform the
        /// change without effecting any other values.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SafeSetValue(int index, string value)
        {
            string nextValue = GetValue(index + 1);
            SetValue(index, value);
            SetValue(index + 1, nextValue);
        }

        protected int NumberOfValuesContain(string value)
        {
            int cnt = 0;
            foreach (string str in m_values)
            {
                if (str == value) cnt++;
            }
            return cnt;
        }

        public abstract CyCustErr ValidateAt(int index);

        public virtual CyCustErr Validate()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            for (int i = 0; i < m_values.Count; i++)
            {
                CyCustErr err = ValidateAt(i);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }
            }

            if (errs.Count == 0) return CyCustErr.Ok;

            string errMsg = string.Empty;
            foreach (CyCustErr err in errs)
            {
                errMsg += err.Message + Environment.NewLine;
            }

            return new CyCustErr(errMsg);
        }

        public static string ParseOutRelatedMsgs(CyCustErr err, IEnumerable<int> indexes)
        {
            string msg = string.Empty;

            string[] individualMsgs = err.Message.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in individualMsgs)
            {
                if (str.StartsWith("Pin ") == false)
                {
                    msg += str + Environment.NewLine;
                }
            }

            foreach (int i in indexes)
            {
                string pinErrMsg = ParseOutErrMsgForIndex(err, i).Trim();
                if (string.IsNullOrEmpty(pinErrMsg) == false)
                {
                    msg += string.Format("Pin {0}: {1}{2}", i, pinErrMsg, Environment.NewLine);
                }
            }

            return msg.Trim();
        }

        public static string ParseOutErrMsgForIndex(CyCustErr err, int index)
        {
            Regex ex = new Regex(string.Format("^Pin {0}: ", index) + "(?<ErrMsg>.*)$", RegexOptions.Multiline);

            Match m = ex.Match(err.Message);
            if (m.Success)
            {
                string errMsg = m.Groups["ErrMsg"].Value;
                return errMsg;
            }
            return string.Empty;
        }

        public override string ToString()
        {
            string str = string.Join(",", m_values.ToArray());
            return str;
        }

        public abstract string GetVerilog(byte width);

        #region Pins Specific

        static string GetBitVectorParamDefaultValue(CyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyBitVectorParam.Formal_DisplayInputConnections:
                case CyBitVectorParam.Formal_DisplayOutputConnections:
                    return CyPortConstants.DefaultDisplay;

                case CyBitVectorParam.Local_PinTypes:
                case CyBitVectorParam.Formal_PinTypes:
                    return CyPortConstants.DefaultPinTypesValue;

                case CyBitVectorParam.Local_SIOHysteresis:
                    return CyPortConstants.DefaultSIOHysteresisValue;

                case CyBitVectorParam.Local_SIOInfo:
                    return CyPortConstants.DefaultSIOInfoValue;

                case CyBitVectorParam.Local_SIORefSels:
                    return CyPortConstants.DefaultSIORefSelValue;

                case CyBitVectorParam.Local_VTrips:
                    return CyPortConstants.DefaultVTripValue;

                case CyBitVectorParam.Formal_SIOGroups:
                    return CyPortConstants.DefaultSIOGroupValue;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledBitVectorParamType, paramType));
                    return null;
            }
        }

        static string GetInputOnlyBitVectorParamDefaultValue(CyInputOnlyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyInputOnlyBitVectorParam.Formal_HotSwaps:
                    return CyPortConstants.DefaultHotSwapValue;

                case CyInputOnlyBitVectorParam.Local_InputsSynchronized:
                case CyInputOnlyBitVectorParam.Formal_InputsSynchronized:
                    return CyPortConstants.DefaultInputsSynchronizedValue;

                case CyInputOnlyBitVectorParam.Local_InterruptModes:
                case CyInputOnlyBitVectorParam.Formal_InterruptModes:
                    return CyPortConstants.DefaultInterruptModeValue;

                case CyInputOnlyBitVectorParam.Formal_ThresholdLevels:
                    return CyPortConstants.DefaultThresholdLevelValue;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledInputOnlyBitVectorParamType, paramType));
                    return null;
            }
        }

        static string GetOutputOnlyBitVectorParamDefaultValue(CyOutputOnlyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyOutputOnlyBitVectorParam.Formal_DriveCurrents:
                    return CyPortConstants.DefaultDriveCurrentValue;

                case CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels:
                    return CyPortConstants.DefaultOutputDriveLevelValue;

                case CyOutputOnlyBitVectorParam.Local_OutputsSynchronized:
                case CyOutputOnlyBitVectorParam.Formal_OutputsSynchronized:
                    return CyPortConstants.DefaultOutputsSynchronizedValue;

                case CyOutputOnlyBitVectorParam.Local_SlewRates:
                case CyOutputOnlyBitVectorParam.Formal_SlewRates:
                    return CyPortConstants.DefaultSlewRateValue;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledOutputBitVectorParamType, paramType));
                    return null;
            }
        }

        //-----------------------------

        static string GetParamName(CyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyBitVectorParam.Formal_DisplayInputConnections:
                    return CyParamInfo.Formal_ParamName_DisplayInputHWConnections;

                case CyBitVectorParam.Formal_DisplayOutputConnections:
                    return CyParamInfo.Formal_ParamName_DisplayOutputHWConnections;

                case CyBitVectorParam.Formal_PinTypes:
                    return CyParamInfo.Formal_ParamName_PinTypes;

                case CyBitVectorParam.Local_SIOHysteresis:
                    return CyParamInfo.Local_ParamName_SIOHysteresis;

                case CyBitVectorParam.Local_SIOInfo:
                    return CyParamInfo.Local_ParamName_SIOInfo;

                case CyBitVectorParam.Local_SIORefSels:
                    return CyParamInfo.Local_ParamName_SIORefSels;

                case CyBitVectorParam.Formal_SIOGroups:
                    return CyParamInfo.Formal_ParamName_SIOGroups;

                case CyBitVectorParam.Local_PinTypes:
                    return CyParamInfo.Local_ParamName_PinTypes;

                case CyBitVectorParam.Local_VTrips:
                    return CyParamInfo.Local_ParamName_VTrip;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledBitVectorParamType, paramType));
                    return null;
            }
        }

        static string GetParamName(CyInputOnlyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyInputOnlyBitVectorParam.Formal_HotSwaps:
                    return CyParamInfo.Formal_ParamName_HotSwaps;

                case CyInputOnlyBitVectorParam.Formal_InputsSynchronized:
                    return CyParamInfo.Formal_ParamName_InputsSynchronized;

                case CyInputOnlyBitVectorParam.Formal_InterruptModes:
                    return CyParamInfo.Formal_ParamName_InterruptMode;

                case CyInputOnlyBitVectorParam.Formal_ThresholdLevels:
                    return CyParamInfo.Formal_ParamName_ThresholdLevels;

                case CyInputOnlyBitVectorParam.Local_InputsSynchronized:
                    return CyParamInfo.Local_ParamName_InputsSynchronized;

                case CyInputOnlyBitVectorParam.Local_InterruptModes:
                    return CyParamInfo.Local_ParamName_InterruptMode;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledInputOnlyBitVectorParamType, paramType));
                    return null;
            }
        }

        static string GetParamName(CyOutputOnlyBitVectorParam paramType)
        {
            switch (paramType)
            {
                case CyOutputOnlyBitVectorParam.Formal_DriveCurrents:
                    return CyParamInfo.Formal_ParamName_DriveCurrents;

                case CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels:
                    return CyParamInfo.Formal_ParamName_OutputDriveLevels;

                case CyOutputOnlyBitVectorParam.Formal_OutputsSynchronized:
                    return CyParamInfo.Formal_ParamName_OutputsSynchronized;

                case CyOutputOnlyBitVectorParam.Formal_SlewRates:
                    return CyParamInfo.Formal_ParamName_SlewRate;

                case CyOutputOnlyBitVectorParam.Local_OutputsSynchronized:
                    return CyParamInfo.Local_ParamName_OutputsSynchronized;

                case CyOutputOnlyBitVectorParam.Local_SlewRates:
                    return CyParamInfo.Local_ParamName_SlewRate;

                default:
                    Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledOutputBitVectorParamType, paramType));
                    return null;
            }
        }

        //-----------------------------

        public static CyCustErr GetBitVectorParamType(string paramName, out CyBitVectorParam paramType)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_DisplayInputHWConnections:
                    paramType = CyBitVectorParam.Formal_DisplayInputConnections;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_DisplayOutputHWConnections:
                    paramType = CyBitVectorParam.Formal_DisplayOutputConnections;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_PinTypes:
                    paramType = CyBitVectorParam.Formal_PinTypes;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_SIOHysteresis:
                    paramType = CyBitVectorParam.Local_SIOHysteresis;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_SIOInfo:
                    paramType = CyBitVectorParam.Local_SIOInfo;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_SIORefSels:
                    paramType = CyBitVectorParam.Local_SIORefSels;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_VTrip:
                    paramType = CyBitVectorParam.Local_VTrips;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_SIOGroups:
                    paramType = CyBitVectorParam.Formal_SIOGroups;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_PinTypes:
                    paramType = CyBitVectorParam.Local_PinTypes;
                    return CyCustErr.Ok;

                default:
                    paramType = default(CyBitVectorParam);
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledParamName, paramName));
            }
        }

        public static CyCustErr GetInputOnlyBitVectorParamType(string paramName,
            out CyInputOnlyBitVectorParam paramType)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_HotSwaps:
                    paramType = CyInputOnlyBitVectorParam.Formal_HotSwaps;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_InputsSynchronized:
                    paramType = CyInputOnlyBitVectorParam.Formal_InputsSynchronized;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_InterruptMode:
                    paramType = CyInputOnlyBitVectorParam.Formal_InterruptModes;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_ThresholdLevels:
                    paramType = CyInputOnlyBitVectorParam.Formal_ThresholdLevels;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_InputsSynchronized:
                    paramType = CyInputOnlyBitVectorParam.Local_InputsSynchronized;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_InterruptMode:
                    paramType = CyInputOnlyBitVectorParam.Local_InterruptModes;
                    return CyCustErr.Ok;

                default:
                    paramType = default(CyInputOnlyBitVectorParam);
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledParamName, paramName));
            }
        }

        public static CyCustErr GetOutputOnlyBitVectorParamType(string paramName,
            out CyOutputOnlyBitVectorParam paramType)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_DriveCurrents:
                    paramType = CyOutputOnlyBitVectorParam.Formal_DriveCurrents;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_OutputDriveLevels:
                    paramType = CyOutputOnlyBitVectorParam.Formal_OutputDriveLevels;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_OutputsSynchronized:
                    paramType = CyOutputOnlyBitVectorParam.Formal_OutputsSynchronized;
                    return CyCustErr.Ok;

                case CyParamInfo.Formal_ParamName_SlewRate:
                    paramType = CyOutputOnlyBitVectorParam.Formal_SlewRates;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_OutputsSynchronized:
                    paramType = CyOutputOnlyBitVectorParam.Local_OutputsSynchronized;
                    return CyCustErr.Ok;

                case CyParamInfo.Local_ParamName_SlewRate:
                    paramType = CyOutputOnlyBitVectorParam.Local_SlewRates;
                    return CyCustErr.Ok;

                default:
                    paramType = default(CyOutputOnlyBitVectorParam);
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledParamName, paramName));
            }
        }

        //-----------------------------

        public static bool IsFormalBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_DisplayInputHWConnections:
                case CyParamInfo.Formal_ParamName_DisplayOutputHWConnections:
                case CyParamInfo.Formal_ParamName_PinTypes:
                case CyParamInfo.Formal_ParamName_SIOGroups:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsFormalInputOnlyBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_HotSwaps:
                case CyParamInfo.Formal_ParamName_ThresholdLevels:
                case CyParamInfo.Formal_ParamName_InterruptMode:
                case CyParamInfo.Formal_ParamName_InputsSynchronized:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsFormalOutputOnlyBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Formal_ParamName_DriveCurrents:
                case CyParamInfo.Formal_ParamName_OutputDriveLevels:
                case CyParamInfo.Formal_ParamName_OutputsSynchronized:
                case CyParamInfo.Formal_ParamName_SlewRate:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsFormalPinAliasParam(string paramName)
        {
            return paramName == CyParamInfo.Formal_ParamName_PinAlisases;
        }

        public static bool IsFormalIOVoltageParam(string paramName)
        {
            return paramName == CyParamInfo.Formal_ParamName_IOVoltages;
        }

        public static bool IsFormalInitialDriveStateParam(string paramName)
        {
            return paramName == CyParamInfo.Formal_ParamName_InitialDriveStates;
        }

        public static bool IsFormalDriveModesParam(string paramName)
        {
            return paramName == CyParamInfo.Formal_ParamName_DriveMode;
        }

        public static bool IsFormalInputBuffersEnabledParam(string paramName)
        {
            return paramName == CyParamInfo.Formal_ParamName_InputBuffersEnabled;
        }

        //-----------------------------

        public static bool IsLocalBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Local_ParamName_SIOHysteresis:
                case CyParamInfo.Local_ParamName_SIOInfo:
                case CyParamInfo.Local_ParamName_VTrip:
                case CyParamInfo.Local_ParamName_PinTypes:
                case CyParamInfo.Local_ParamName_SIORefSels:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsLocalInputOnlyBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Local_ParamName_InterruptMode:
                case CyParamInfo.Local_ParamName_InputsSynchronized:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsLocalOutputOnlyBitVectorParam(string paramName)
        {
            switch (paramName)
            {
                case CyParamInfo.Local_ParamName_OutputsSynchronized:
                case CyParamInfo.Local_ParamName_SlewRate:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsLocalPinAliasParam(string paramName)
        {
            return paramName == CyParamInfo.Local_ParamName_PinAlisases;
        }

        public static bool IsLocalIOVoltageParam(string paramName)
        {
            return paramName == CyParamInfo.Local_ParamName_IOVoltages;
        }

        public static bool IsLocalInitialDriveStateParam(string paramName)
        {
            return paramName == CyParamInfo.Local_ParamName_InitialDriveStates;
        }

        public static bool IsLocalDriveModesParam(string paramName)
        {
            return paramName == CyParamInfo.Local_ParamName_DriveMode;
        }

        public static bool IsLocalInputBuffersEnabledParam(string paramName)
        {
            return paramName == CyParamInfo.Local_ParamName_InputBuffersEnabled;
        }

        //-----------------------------

        public static bool IsStringArrayParamNeedingValidation(ICyInstValidate_v1 instVal, string paramName)
        {
            bool isStringArrayParam = IsFormalStringArrayParam(paramName);
            if (isStringArrayParam)
            {
                CyCompDevParam param = instVal.GetCommittedParam(paramName);
                return (param.IsReadOnly == false);
            }
            return false;
        }

        public static bool IsStringArrayParam(string paramName)
        {
            bool result = IsFormalStringArrayParam(paramName);
            if (result) return result;
            result = IsLocalStringArrayParam(paramName);
            return result;
        }

        public static bool IsLocalStringArrayParam(string paramName)
        {
            bool result;
            result = IsLocalBitVectorParam(paramName);
            if (result) return result;

            result = IsLocalInputOnlyBitVectorParam(paramName);
            if (result) return result;

            result = IsLocalOutputOnlyBitVectorParam(paramName);
            if (result) return result;

            result = IsLocalPinAliasParam(paramName);
            if (result) return result;

            result = IsLocalIOVoltageParam(paramName);
            if (result) return result;

            result = IsLocalInitialDriveStateParam(paramName);
            if (result) return result;

            result = IsLocalDriveModesParam(paramName);
            if (result) return result;

            result = IsLocalInputBuffersEnabledParam(paramName);
            if (result) return result;

            return false;
        }

        public static bool IsFormalStringArrayParam(string paramName)
        {
            bool result;
            result = IsFormalBitVectorParam(paramName);
            if (result) return result;

            result = IsFormalInputOnlyBitVectorParam(paramName);
            if (result) return result;

            result = IsFormalOutputOnlyBitVectorParam(paramName);
            if (result) return result;

            result = IsFormalPinAliasParam(paramName);
            if (result) return result;

            result = IsFormalIOVoltageParam(paramName);
            if (result) return result;

            result = IsFormalInitialDriveStateParam(paramName);
            if (result) return result;

            result = IsFormalDriveModesParam(paramName);
            if (result) return result;

            result = IsFormalInputBuffersEnabledParam(paramName);
            if (result) return result;

            return false;
        }

        public static CyCustErr CreateStringArrayData(ICyInstQuery_v1 query, string paramName,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            if (IsFormalBitVectorParam(paramName) || IsLocalBitVectorParam(paramName))
            {
                CyBitVectorParam paramType;
                err = GetBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInputOnlyBitVectorParam(paramName) || IsLocalInputOnlyBitVectorParam(paramName))
            {
                CyInputOnlyBitVectorParam paramType;
                err = GetInputOnlyBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateInputOnlyBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalOutputOnlyBitVectorParam(paramName) || IsLocalOutputOnlyBitVectorParam(paramName))
            {
                CyOutputOnlyBitVectorParam paramType;
                err = GetOutputOnlyBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateOutputOnlyBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalPinAliasParam(paramName))
            {
                err = CreatePinAliasData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalPinAliasParam(paramName))
            {
                err = CreatePinAliasData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalIOVoltageParam(paramName))
            {
                err = CreateIOVoltageData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalIOVoltageParam(paramName))
            {
                err = CreateIOVoltageData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInitialDriveStateParam(paramName))
            {
                err = CreateInitialDriveStateData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalInitialDriveStateParam(paramName))
            {
                err = CreateInitialDriveStateData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalDriveModesParam(paramName))
            {
                err = CreateDriveModeData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalDriveModesParam(paramName))
            {
                err = CreateDriveModeData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInputBuffersEnabledParam(paramName))
            {
                err = CreateInputBufferEnabledData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalInputBuffersEnabledParam(paramName))
            {
                err = CreateInputBufferEnabledData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }

            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledParamName, paramName));
        }

        public static CyCustErr CreateStringArrayData(ICyInstValidate_v1 query, string paramName,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            if (IsFormalBitVectorParam(paramName) || IsLocalBitVectorParam(paramName))
            {
                CyBitVectorParam paramType;
                err = GetBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInputOnlyBitVectorParam(paramName) || IsLocalInputOnlyBitVectorParam(paramName))
            {
                CyInputOnlyBitVectorParam paramType;
                err = GetInputOnlyBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateInputOnlyBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalOutputOnlyBitVectorParam(paramName) || IsLocalOutputOnlyBitVectorParam(paramName))
            {
                CyOutputOnlyBitVectorParam paramType;
                err = GetOutputOnlyBitVectorParamType(paramName, out paramType);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                err = CreateOutputOnlyBitVectorData(query, paramType, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalPinAliasParam(paramName))
            {
                err = CreatePinAliasData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalPinAliasParam(paramName))
            {
                err = CreatePinAliasData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalIOVoltageParam(paramName))
            {
                err = CreateIOVoltageData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalIOVoltageParam(paramName))
            {
                err = CreateIOVoltageData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInitialDriveStateParam(paramName))
            {
                err = CreateInitialDriveStateData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalInitialDriveStateParam(paramName))
            {
                err = CreateInitialDriveStateData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalDriveModesParam(paramName))
            {
                err = CreateDriveModeData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalDriveModesParam(paramName))
            {
                err = CreateDriveModeData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsFormalInputBuffersEnabledParam(paramName))
            {
                err = CreateInputBufferEnabledData(query, true, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }
            else if (IsLocalInputBuffersEnabledParam(paramName))
            {
                err = CreateInputBufferEnabledData(query, false, out data);
                if (err.IsNotOk)
                {
                    errs.Add(err);
                }

                if (errs.Count == 0)
                {
                    return CyCustErr.Ok;
                }
                return CombineErrs(errs);
            }

            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledParamName, paramName));
        }

        //-----------------------------
        static CyCustErr CombineErrs(IEnumerable<CyCustErr> errs)
        {
            string errMsg = string.Empty;
            foreach (CyCustErr err in errs)
            {
                errMsg += err.Message + Environment.NewLine;
            }
            return new CyCustErr(errMsg.Trim());
        }

        public static CyCustErr CreateBitVectorData(ICyInstValidate_v1 query, CyBitVectorParam paramType,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            data = null;
            string value;
            CyCustErr err = CyPortCustomizer.GetParamValue<string>(query, GetParamName(paramType), out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreateBitVectorData(paramType, value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateBitVectorData(ICyInstQuery_v1 query, CyBitVectorParam paramType,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            data = null;
            string value;
            CyCustErr err = CyPortCustomizer.GetParamValue<string>(query, GetParamName(paramType), out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreateBitVectorData(paramType, value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateBitVectorData(CyBitVectorParam paramType, string paramValue,
            out CyStringArrayParamData data)
        {
            string defValue = GetBitVectorParamDefaultValue(paramType);
            switch (paramType)
            {
                case CyBitVectorParam.Formal_DisplayInputConnections:
                case CyBitVectorParam.Formal_DisplayOutputConnections:
                case CyBitVectorParam.Local_SIOHysteresis:
                case CyBitVectorParam.Local_SIOInfo:
                case CyBitVectorParam.Local_SIORefSels:
                case CyBitVectorParam.Local_VTrips:
                case CyBitVectorParam.Formal_SIOGroups:
                    data = new CyBitVectorParamData(paramValue, defValue);
                    break;

                case CyBitVectorParam.Formal_PinTypes:
                case CyBitVectorParam.Local_PinTypes:
                    data = new CyBitVectorParamData(paramValue, defValue, "1101", "1110", "1111");
                    break;

                default:
                    Debug.Fail("unhandled bit vector type");
                    data = new CyBitVectorParamData(paramValue, defValue);
                    break;
            }

            return CyCustErr.Ok;
        }

        //-----------------------------

        public static CyCustErr CreateInputOnlyBitVectorData(ICyInstValidate_v1 query,
            CyInputOnlyBitVectorParam paramType, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string paramName = GetParamName(paramType);
            err = CyPortCustomizer.GetParamValue<string>(query, paramName, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", paramName,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInputOnlyBitVectorData(ICyInstQuery_v1 query, CyInputOnlyBitVectorParam paramType,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string paramName = GetParamName(paramType);
            err = CyPortCustomizer.GetParamValue<string>(query, paramName, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", paramName,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInputOnlyBitVectorData(CyInputOnlyBitVectorParam paramType, string paramValue,
            string pinTypesValue, out CyStringArrayParamData data)
        {
            //These options must be set to a default values unless the pin is an input pin. 99% of the time this
            //means setting them to the default value of the param.
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;

            string defValue = GetInputOnlyBitVectorParamDefaultValue(paramType);
            string nonInputValue;

            switch (paramType)
            {
                case CyInputOnlyBitVectorParam.Formal_HotSwaps:
                case CyInputOnlyBitVectorParam.Formal_InputsSynchronized:
                case CyInputOnlyBitVectorParam.Formal_InterruptModes:
                case CyInputOnlyBitVectorParam.Local_InputsSynchronized:
                case CyInputOnlyBitVectorParam.Local_InterruptModes:
                    data = new CyBitVectorParamData(paramValue, defValue);
                    nonInputValue = defValue;
                    break;

                case CyInputOnlyBitVectorParam.Formal_ThresholdLevels:
                    data = new CyBitVectorParamData(paramValue, defValue, "1011", "1100", "1101", "1110", "1111");

                    //This param works slightly different than the others. Instead of using its default value when input
                    //is not used it needs to use a different option. This is because the default value is not the
                    //most generic option where as CMOS_LVTTL is.
                    nonInputValue = CyPortConstants.ThresholdLevelValue_CMOS_LVTTL;
                    break;

                default:
                    Debug.Fail("unhandled input only bit vector type");
                    data = new CyBitVectorParamData(paramValue, defValue);
                    nonInputValue = defValue;
                    break;
            }

            CyStringArrayParamData pinTypesData;
            err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesValue,
                out pinTypesData);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", GetParamName(paramType),
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            List<string> values = new List<string>();
            int maxNumValuesSet = Math.Max(data.Count, pinTypesData.Count);
            for (int i = 0; i < maxNumValuesSet; i++)
            {
                string pinType = pinTypesData.GetValue(i);
                switch (pinType)
                {
                    //These options are not Inputs so the values must be set to the default value.
                    case CyPortConstants.PinTypesValue_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT:
                    case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                        values.Add(nonInputValue);
                        break;

                    //These options ARE Inputs so what ever their value is set to is what should be used.
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                        values.Add(data.GetValue(i));
                        break;

                    default:
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                data.SetValue(i, values[i]);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        //-----------------------------

        public static CyCustErr CreateOutputOnlyBitVectorData(ICyInstValidate_v1 query,
            CyOutputOnlyBitVectorParam paramType, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string paramName = GetParamName(paramType);
            err = CyPortCustomizer.GetParamValue<string>(query, paramName, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", paramName,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateOutputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateOutputOnlyBitVectorData(ICyInstQuery_v1 query,
            CyOutputOnlyBitVectorParam paramType, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string paramName = GetParamName(paramType);
            err = CyPortCustomizer.GetParamValue<string>(query, paramName, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", paramName,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateOutputOnlyBitVectorData(paramType, value, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateOutputOnlyBitVectorData(CyOutputOnlyBitVectorParam paramType, string paramValue,
            string pinTypesValue, out CyStringArrayParamData data)
        {
            //These options must be set to their deafult values unless the pin is an output pin.
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;

            string defValue = GetOutputOnlyBitVectorParamDefaultValue(paramType);
            data = new CyBitVectorParamData(paramValue, defValue);

            CyStringArrayParamData pinTypesData;
            err = CyStringArrayParamData.CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesValue,
                out pinTypesData);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.", GetParamName(paramType),
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            List<string> values = new List<string>();
            int maxNumValuesSet = Math.Max(data.Count, pinTypesData.Count);
            for (int i = 0; i < maxNumValuesSet; i++)
            {
                string pinType = pinTypesData.GetValue(i);
                switch (pinType)
                {
                    //These options are not Outputs so the values must be set to the default value.
                    case CyPortConstants.PinTypesValue_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                        values.Add(defValue);
                        break;

                    //These options ARE Outputs so what ever their value is set to is what should be used.
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT:
                    case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                        values.Add(data.GetValue(i));
                        break;

                    default:
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                data.SetValue(i, values[i]);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        //-----------------------------

        public static CyCustErr CreatePinAliasData(ICyInstValidate_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string name = (formal) ? CyParamInfo.Formal_ParamName_PinAlisases : CyParamInfo.Local_ParamName_PinAlisases;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreatePinAliasData(value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreatePinAliasData(ICyInstQuery_v1 query, bool formal, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string name = (formal) ? CyParamInfo.Formal_ParamName_PinAlisases : CyParamInfo.Local_ParamName_PinAlisases;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreatePinAliasData(value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreatePinAliasData(string pinAliasValue, out CyStringArrayParamData data)
        {
            data = new CyAliasParamData(pinAliasValue);
            return CyCustErr.Ok;
        }

        public static CyCustErr CreateIOVoltageData(ICyInstValidate_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string name = (formal) ? CyParamInfo.Formal_ParamName_IOVoltages : CyParamInfo.Local_ParamName_IOVoltages;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreateIOVoltageData(value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateIOVoltageData(ICyInstQuery_v1 query, bool formal, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string value;
            string name = (formal) ? CyParamInfo.Formal_ParamName_IOVoltages : CyParamInfo.Local_ParamName_IOVoltages;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out value);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            err = CreateIOVoltageData(value, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateIOVoltageData(string ioVoltageValue, out CyStringArrayParamData data)
        {
            data = new CyIOVoltagesParamData(ioVoltageValue);
            return CyCustErr.Ok;
        }

        public static CyCustErr CreateInitialDriveStateData(ICyInstValidate_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string initDrStateValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_InitialDriveStates :
                CyParamInfo.Local_ParamName_InitialDriveStates;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out initDrStateValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string driveModeValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_DriveMode, out driveModeValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InitialDriveStates,
                    CyParamInfo.Formal_ParamName_DriveMode)));
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InitialDriveStates,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInitialDriveStateData(initDrStateValue, driveModeValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInitialDriveStateData(ICyInstQuery_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string initDrStateValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_InitialDriveStates :
                CyParamInfo.Local_ParamName_InitialDriveStates;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out initDrStateValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string driveModeValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_DriveMode, out driveModeValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InitialDriveStates,
                    CyParamInfo.Formal_ParamName_DriveMode)));
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InitialDriveStates,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInitialDriveStateData(initDrStateValue, driveModeValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInitialDriveStateData(string initialDriveStateValue, string driveModeValue,
            string pinTypesValue, out CyStringArrayParamData data)
        {
            CyStringArrayParamData driveModeData;
            CyCustErr err = CreateDriveModeData(driveModeValue, pinTypesValue, out driveModeData);
            data = new CyInitialDriveStateParamData(initialDriveStateValue, driveModeData);
            return err;
        }

        public static CyCustErr CreateDriveModeData(ICyInstValidate_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string driveModeValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_DriveMode : CyParamInfo.Local_ParamName_DriveMode;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out driveModeValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_DriveMode,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateDriveModeData(driveModeValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateDriveModeData(ICyInstQuery_v1 query, bool formal, out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string driveModeValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_DriveMode : CyParamInfo.Local_ParamName_DriveMode;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out driveModeValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes,
                out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_DriveMode,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateDriveModeData(driveModeValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateDriveModeData(string driveModesValue, string pinTypesValue,
            out CyStringArrayParamData data)
        {
            CyStringArrayParamData pinTypesData;
            CyCustErr err = CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesValue, out pinTypesData);
            data = new CyDriveModeParamData(driveModesValue, pinTypesData);
            return err;
        }

        public static CyCustErr CreateInputBufferEnabledData(ICyInstValidate_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string inputBufferEnabledValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_InputBuffersEnabled :
                CyParamInfo.Local_ParamName_InputBuffersEnabled;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out inputBufferEnabledValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InputBuffersEnabled,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInputBufferEnabledData(inputBufferEnabledValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInputBufferEnabledData(ICyInstQuery_v1 query, bool formal,
            out CyStringArrayParamData data)
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            CyCustErr err;
            data = null;

            string inputBufferEnabledValue;
            string name = (formal) ? CyParamInfo.Formal_ParamName_InputBuffersEnabled :
                CyParamInfo.Local_ParamName_InputBuffersEnabled;
            err = CyPortCustomizer.GetParamValue<string>(query, name, out inputBufferEnabledValue);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            string pinTypesValue;
            err = CyPortCustomizer.GetParamValue<string>(query, CyParamInfo.Formal_ParamName_PinTypes, out pinTypesValue);
            if (err.IsNotOk)
            {
                errs.Add(new CyCustErr(string.Format(
                    "Cannot determine {0} because {1} is in an error state.",
                    CyParamInfo.Formal_ParamName_InputBuffersEnabled,
                    CyParamInfo.Formal_ParamName_PinTypes)));
            }

            err = CreateInputBufferEnabledData(inputBufferEnabledValue, pinTypesValue, out data);
            if (err.IsNotOk)
            {
                errs.Add(err);
            }

            if (errs.Count == 0)
            {
                return CyCustErr.Ok;
            }
            return CombineErrs(errs);
        }

        public static CyCustErr CreateInputBufferEnabledData(string inputBuffersEnabledValue, string pinTypesValue,
            out CyStringArrayParamData data)
        {
            CyStringArrayParamData pinTypesData;
            CyCustErr err = CreateBitVectorData(CyBitVectorParam.Formal_PinTypes, pinTypesValue, out pinTypesData);
            data = new CyInputBuffersEnabledParamData(inputBuffersEnabledValue, pinTypesData);

            //This param is an input only param and needs to be adjusted as such.
            List<string> values = new List<string>();
            int maxNumValuesSet = Math.Max(data.Count, pinTypesData.Count);
            for (int i = 0; i < maxNumValuesSet; i++)
            {
                string pinType = pinTypesData.GetValue(i);
                switch (pinType)
                {
                    //These options are not Inputs so the values must be set to the default value.
                    case CyPortConstants.PinTypesValue_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT:
                    case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                        values.Add(CyInputBuffersEnabledParamData.GetDefaultValue(pinTypesData, i));
                        break;

                    //These options ARE Inputs so what ever their value is set to is what should be used.
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                        values.Add(data.GetValue(i));
                        break;

                    default:
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                data.SetValue(i, values[i]);
            }

            return err;
        }

        #endregion

        #region Inner Classes

        internal class CyBitVectorParamData : CyStringArrayParamData
        {
            /// <summary>
            /// Contains a list of values that are the correct length with the correct 
            /// number of bits but are still invalid.
            /// </summary>
            List<string> m_invalidBitVectors;

            public CyBitVectorParamData(string data, string defaultValue, params string[] invalidValues)
                : base(data, defaultValue, true)
            {
                m_invalidBitVectors = new List<string>(invalidValues);
            }

            public string GetVerilogBitVector(int width)
            {
                int totalWidth = width * GetDefaultValue(0).Length; //width * num chars per value
                if (totalWidth == 0) return "\"\"";

                string[] bits = new string[width];
                for (int i = 0; i < width; i++)
                {
                    string val = GetValue(i);
                    bits[i] = val;
                }

                string value = string.Format("{0}'b{1}", totalWidth, string.Join("_", bits));
                return value;
            }

            public override CyCustErr ValidateAt(int index)
            {
                //Values must only contain 1's and 0's and must all be the same length.

                string value = GetValue(index);

                if (value.Length != ExampleValue.Length)
                {
                    return new CyCustErr(
                        string.Format(cy_pins_v1_20.Resource1.Err_BitVectorValueIncorrectLength,
                        index,
                        ExampleValue.Length,
                        value,
                        value.Length));
                }

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != '0' && value[i] != '1')
                    {
                        return new CyCustErr(string.Format(
                        cy_pins_v1_20.Resource1.Err_BitVectorValueOnlyOnesAndZeros,
                        index,
                        value[i]));
                    }
                }

                if (m_invalidBitVectors.Contains(value))
                {
                    return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.Err_BitVectorValueInvalidOption,
                        index, value));
                }

                return CyCustErr.Ok;
            }

            public override string GetVerilog(byte width)
            {
                return GetVerilogBitVector(width);
            }
        }

        class CyInitialDriveStateParamData : CyBitVectorParamData
        {
            CyStringArrayParamData m_driveModeData;

            public CyInitialDriveStateParamData(string data, CyStringArrayParamData driveModeData)
                : base(data, CyPortConstants.InitialDriveStateValue_LOW)
            {
                m_driveModeData = driveModeData;
            }

            protected override string GetDefaultValue(int index)
            {
                string driveMode = m_driveModeData.GetValue(index);

                switch (driveMode)
                {
                    case CyPortConstants.DriveModeValue_ANALOG_HI_Z:
                    case CyPortConstants.DriveModeValue_CMOS_OUT:
                    case CyPortConstants.DriveModeValue_DIGITAL_HI_Z:
                    case CyPortConstants.DriveModeValue_OPEN_DRAIN_HI:
                    case CyPortConstants.DriveModeValue_OPEN_DRAIN_LO:
                    case CyPortConstants.DriveModeValue_RES_PULL_DOWN:
                        return CyPortConstants.InitialDriveStateValue_LOW;

                    case CyPortConstants.DriveModeValue_RES_PULL_UP:
                    case CyPortConstants.DriveModeValue_RES_PULL_UP_DOWN:
                        return CyPortConstants.InitialDriveStateValue_HIGH;

                    default:
                        Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledDriveMode, driveMode));
                        return string.Format(cy_pins_v1_20.Resource1.UnhandledDriveMode, driveMode);
                }
            }
        }

        class CyDriveModeParamData : CyBitVectorParamData
        {
            CyStringArrayParamData m_pinTypes;

            public CyDriveModeParamData(string data, CyStringArrayParamData pinTypes)
                : base(data, CyPortConstants.DriveModeValue_ANALOG_HI_Z)
            {
                m_pinTypes = pinTypes;
            }

            protected override string GetDefaultValue(int index)
            {
                string pinType = m_pinTypes.GetValue(index);

                switch (pinType)
                {
                    case CyPortConstants.PinTypesValue_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                        return CyPortConstants.DriveModeValue_DIGITAL_HI_Z;

                    case CyPortConstants.PinTypesValue_ANALOG:
                        return CyPortConstants.DriveModeValue_ANALOG_HI_Z;

                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                        return CyPortConstants.DriveModeValue_OPEN_DRAIN_LO;

                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT:
                        return CyPortConstants.DriveModeValue_CMOS_OUT;

                    default:
                        return string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType);
                }
            }
        }

        class CyInputBuffersEnabledParamData : CyBitVectorParamData
        {
            CyStringArrayParamData m_pinTypes;

            public CyInputBuffersEnabledParamData(string data, CyStringArrayParamData pinTypes)
                : base(data, CyPortConstants.InputBufferEnabledValue_FALSE)
            {
                m_pinTypes = pinTypes;
            }

            public static string GetDefaultValue(CyStringArrayParamData pinTypes, int index)
            {
                string pinType = pinTypes.GetValue(index);

                switch (pinType)
                {
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL:
                    case CyPortConstants.PinTypesValue_BIDIRECTIONAL_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_DIGIN_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_DIGIN_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT:
                    case CyPortConstants.PinTypesValue_DIGOUT_ANALOG:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE:
                    case CyPortConstants.PinTypesValue_DIGOUT_OE_ANALOG:
                        return CyPortConstants.InputBufferEnabledValue_TRUE;

                    case CyPortConstants.PinTypesValue_ANALOG:
                        return CyPortConstants.InputBufferEnabledValue_FALSE;

                    default:
                        Debug.Fail(string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType));
                        return string.Format(cy_pins_v1_20.Resource1.UnhandledPinType, pinType);
                }
            }

            protected override string GetDefaultValue(int index)
            {
                return GetDefaultValue(m_pinTypes, index);
            }
        }

        internal class CyAliasParamData : CyStringArrayParamData
        {
            public CyAliasParamData(string data)
                : base(data, string.Empty, false)
            {

            }

            public override CyCustErr ValidateAt(int index)
            {
                //Values must be valid C/C++ identifier names.

                string value = GetValue(index);
                if (string.IsNullOrEmpty(value) == false)
                {
                    string msg;
                    if (CyBasic.IsValidCCppIdentifierName(value, out msg) == false)
                    {
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.Err_NotValidCIdentifier,
                            index, value, msg));
                    }
                }
                return CyCustErr.Ok;
            }

            public override CyCustErr Validate()
            {
                //All the values must be unique or an empty string.

                CyCustErr err = base.Validate();
                if (err.IsNotOk) return err;

                for (int i = 0; i < Count; i++)
                {
                    string val = GetValue(i);
                    if (string.IsNullOrEmpty(val) == false)
                    {
                        int num = NumberOfValuesContain(val);
                        if (num > 1)
                        {
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.Err_AliasRepeated, val, num));
                        }
                    }
                }

                return CyCustErr.Ok;
            }

            public string GetVerilogString(int width)
            {
                if (width == 0) return string.Empty;

                string str = GetValue(0);
                for (int i = 1; i < width; i++)
                {
                    str += "," + GetValue(i);
                }
                return "\"" + str + "\"";
            }

            public override string GetVerilog(byte width)
            {
                return GetVerilogString(width);
            }
        }

        public class CyIOVoltagesParamData : CyStringArrayParamData
        {
            public CyIOVoltagesParamData(string data)
                : base(data, CyPortConstants.DefaultIOVolatageValue, true)
            {

            }

            public override CyCustErr ValidateAt(int index)
            {
                //Values must be voltages with up to one decimal place of percision between 1.7 and 5.5.

                string value = GetValue(index);
                if (string.IsNullOrEmpty(value) == false &&
                    CyPortConstants.DefaultIOVolatageValue.Equals(value, StringComparison.OrdinalIgnoreCase) == false)
                {
                    double volts;
                    bool result = double.TryParse(value, out volts);
                    if (result == false)
                    {
                        return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.Err_InvalidNumFormat,
                            index, value));
                    }

                    if (value.Contains("."))
                    {
                        string[] parts = value.Split('.');
                        Debug.Assert(parts.Length == 2, "how is having multiple decimal points a valid number");
                        if (parts[parts.Length - 1].Length > 1)
                        {
                            return new CyCustErr(string.Format(cy_pins_v1_20.Resource1.Err_InvalidIOVoltagePercision,
                                index,
                                value));
                        }
                    }
                }
                return CyCustErr.Ok;
            }

            public string GetVerilogString(int width)
            {
                List<string> vals = new List<string>();
                for (int i = 0; i < width; i++)
                {
                    string tempVal = GetValue(i);
                    if (CyPortConstants.DefaultIOVolatageValue.Equals(tempVal, StringComparison.OrdinalIgnoreCase))
                    {
                        tempVal = string.Empty;
                    }
                    vals.Add(tempVal);
                }
                return "\"" + string.Join(", ", vals.ToArray()) + "\"";
            }

            public override string GetVerilog(byte width)
            {
                return GetVerilogString(width);
            }
        }

        #endregion
    }

    #endregion
}
