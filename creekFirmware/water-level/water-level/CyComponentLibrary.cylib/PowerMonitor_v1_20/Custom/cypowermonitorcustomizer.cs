/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;

namespace PowerMonitor_v1_20
{
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyShapeCustomize_v1, ICyAPICustomize_v1, ICyDRCProvider_v1
    {
        #region ICyParamEditHook_v1 Members
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
            ICyExpressMgr_v1 mgr)
        {
            CyParameters.GlobalEditMode = false;

            CyParameters parameters = new CyParameters(edit);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyGeneralTab basicTab = new CyGeneralTab(parameters);
            CyVoltagesTab voltagesTab = new CyVoltagesTab(parameters);
            CyCurrentsTab currentsTab = new CyCurrentsTab(parameters);
            CyAuxiliaryTab auxTab = new CyAuxiliaryTab(parameters);

            CyParamExprDelegate exprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                CyParameters.GlobalEditMode = false;
                basicTab.UpdateUI(true);
                CyParameters.GlobalEditMode = true;
            };

            editor.AddCustomPage(Resources.BasicTabDisplayName, basicTab, exprDelegate, basicTab.TabName);
            editor.AddCustomPage(Resources.VoltagesTabDisplayName, voltagesTab, exprDelegate, voltagesTab.TabName);
            editor.AddCustomPage(Resources.CurrentsTabDisplayName, currentsTab, exprDelegate, currentsTab.TabName);
            editor.AddCustomPage(Resources.AuxTabDisplayName, auxTab, exprDelegate, auxTab.TabName);

            editor.AddDefaultPage(Resources.BuiltInTabDisplayName, "Built-in");
            editor.UseBigEditor = true;


            basicTab.UpdateUI(false);
            CyParameters.GlobalEditMode = true;
            CyEditingWrapperControl.m_runMode = true;

            return editor.ShowDialog();
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyShapeCustomize_v1 Members
        const string PGOOD_INDIVIDUAL = "1";
        const string GENERATED_SHAPEA = "SymbolShapeA";
        const string GENERATED_SHAPEB = "SymbolShapeB";
        const string GENERATED_HEADERB = "SymbolHeaderB";
        const string SINGLE = "0";
        const string DIFFERENTIAL = "1";
        const string vTermName = "v";
        const string auxTermName = "aux";
        const string auxrtnTermName = "aux_rtn";
        const string PGOOD_BUS = "pgood_bus";
        const string TRUE = "1";

        public CyCustErr CustomizeShapes(ICyInstQuery_v1 instQuery, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            CyCustErr err = CyCustErr.OK;
            string iTermName;

            // We leave the symbol as it is for symbol preview
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;

            err = shapeEdit.ShapesRemove("PMon_B_Body");
            if (err.IsNotOk) return err;

            err = shapeEdit.ShapesRemove("PMon_A_Body");
            if (err.IsNotOk) return err;

            // Remove voltage and curren terminals
            for (int i = 0; i < 32; i++)
            {
                iTermName = "i" + (i + 1).ToString() + "_rtn" + (i + 1).ToString();
                err = termEdit.TerminalDelete(vTermName + (i + 1).ToString());
                if (err.IsNotOk) return err;
                err = termEdit.TerminalDelete(iTermName);
                if (err.IsNotOk) return err;
            }

            // Remove auxiliary voltage terminals
            for (int i = 0; i < 4; i++)
            {
                err = termEdit.TerminalDelete(auxTermName + (i + 1).ToString());
                if (err.IsNotOk) return err;
                err = termEdit.TerminalDelete(auxrtnTermName + (i + 1).ToString());
                if (err.IsNotOk) return err;
            }

            // Add term shapes
            List<string> shapeTags = new List<string>();
            shapeTags.Add(GENERATED_SHAPEA);
            shapeTags.Add(GENERATED_SHAPEB);
            shapeTags.Add(GENERATED_HEADERB);
            List<string> shapeTags1 = new List<string>();
            shapeTags1.Add("AUX");

            CyParameters parameters = new CyParameters(instQuery);

            float inTermX = -60F;
            float inTermY = -66F;
            float auxInTermSingleX = 60F;
            float auxInTermY = 12F;
            float auxInTermDiffX = 60F;
            float auxAnnotationX = 41.08693F;
            float auxAnnotationY = 6.308677F;
            float auxRtnAnnotationX = 32.1521454F;
            float vTermAnnotationX = -53.25537F;

            float iTermAnnotationX = 235.09086F;
            float iTermAnnotationY = -30.0303268F;//-83.8964F;
            float vTermAnnotationY = iTermAnnotationY;
            float symbolABodyWidth = 108F;
            float symbolBBodyWidth = 72F;
            float symbolATopX = -54F;
            float symbolATopY = -72F;
            float symbolBTopX = 234F;
            float symbolBTopY = -72F;
            float offset = 12.0F;
            float vTermPosY = inTermY + 42;
            float iTermPosY = vTermPosY;
            float iInTermPosX = 228F;
            string inputTermName;
            bool symbolBVisible = false;

            // if Pgood is configured as individual, then rename the existing terminal to match
            // the terminal width
            if (parameters.PgoodConfig == CyEPgoodType.individual)
            {
                string suffix = string.Format("[{0}:1]", parameters.NumConverters);
                string pgoodTermName = PGOOD_BUS + suffix;
                string pgood_bus = termEdit.GetTermName(PGOOD_BUS);
                err = termEdit.TerminalRename(pgood_bus, pgoodTermName);
                if (err.IsNotOk) return err;
            }

            Font type = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            StringFormat format1 = new StringFormat(format);
            format.Alignment = StringAlignment.Center;
            format1.Alignment = StringAlignment.Far;

            // add voltage input terminals to the symbol
            for (int i = 0; i < parameters.NumConverters; i++)
            {
                err = termEdit.TerminalAdd((vTermName + (i + 1).ToString()), CyCompDevTermDir.INOUT,
                    CyCompDevTermType.ANALOG, new PointF(inTermX, vTermPosY), string.Empty, 0f, false);
                if (err.IsNotOk) return err;
                vTermPosY = vTermPosY + offset;

                // add terminal names
                PointF loc = new PointF(vTermAnnotationX, vTermAnnotationY);
                err = shapeEdit.CreateAnnotation(shapeTags1, (vTermName + (i + 1).ToString()), loc, type, format1);
                if (err.IsNotOk) return err;
                err = shapeEdit.SetFillColor(shapeTags1[0], Color.Black);
                if (err.IsNotOk) return err;
                err = shapeEdit.ClearOutline(shapeTags1[0]);
                if (err.IsNotOk) return err;
                vTermAnnotationY = vTermAnnotationY + offset;
            }

            // add current input terminals to the symbol
            for (int i = 0; i < parameters.NumConverters; i++)
            {
                if (parameters.VoltagesTable[i].m_voltageMeasurementType == CyEVInternalType.Differential ||
                    parameters.CurrentsTable[i].m_currentMeasurementType != CyECurrentMeasurementInternalType.None)
                {
                    // Set the flag
                    symbolBVisible = true;
                    inputTermName = "i" + (i + 1).ToString() + "_rtn" + (i + 1).ToString();
                    err = termEdit.TerminalAdd(inputTermName, CyCompDevTermDir.INOUT, CyCompDevTermType.ANALOG,
                        new PointF(iInTermPosX, iTermPosY), string.Empty, 0f, false);
                    if (err.IsNotOk) return err;
                    iTermPosY = iTermPosY + offset;

                    // Name the terminals based on the voltage/current measurement chosen.
                    if (parameters.VoltagesTable[i].m_voltageMeasurementType == CyEVInternalType.Differential)
                    {
                        inputTermName = "rtn";
                    }
                    else if (parameters.CurrentsTable[i].m_currentMeasurementType !=
                        CyECurrentMeasurementInternalType.None)
                    {
                        inputTermName = "i";
                    }
                    // add terminal names
                    PointF loc = new PointF(iTermAnnotationX, iTermAnnotationY);
                    err = shapeEdit.CreateAnnotation(shapeTags1, (inputTermName + (i + 1).ToString()), loc, type,
                        format1);
                    if (err.IsNotOk) return err;
                    err = shapeEdit.SetFillColor(shapeTags1[0], Color.Black);
                    if (err.IsNotOk) return err;
                    err = shapeEdit.ClearOutline(shapeTags1[0]);
                    if (err.IsNotOk) return err;
                    iTermAnnotationY = iTermAnnotationY + offset;
                }
            }

            format.Alignment = StringAlignment.Center;
            // Add aux channels
            for (int i = 0; i < parameters.NumAuxChannels; i++)
            {
                err = termEdit.TerminalAdd((auxTermName + (i + 1).ToString()), CyCompDevTermDir.INOUT,
                    CyCompDevTermType.ANALOG, new PointF(auxInTermSingleX, auxInTermY), string.Empty, 180f, false);
                if (err.IsNotOk) return err;
                //auxInTermSingleY = auxInTermSingleY + offset;
                auxInTermY = auxInTermY + offset;

                // Add aux channel terminal names
                PointF loc = new PointF(auxAnnotationX, auxAnnotationY);
                err = shapeEdit.CreateAnnotation(shapeTags1, (auxTermName + (i + 1).ToString()), loc, type, format);
                if (err.IsNotOk) return err;
                err = shapeEdit.SetFillColor(shapeTags1[0], Color.Black);
                if (err.IsNotOk) return err;
                err = shapeEdit.ClearOutline(shapeTags1[0]);
                if (err.IsNotOk) return err;
                auxAnnotationY = auxAnnotationY + offset;
            }

            // add aux_rtn terminals
            for (int i = 0; i < parameters.NumAuxChannels; i++)
            {
                if ((parameters.AuxTable[i].m_adcRange != CyEAdcRangeInternalType.SignleEnded_4V) && 
                (parameters.AuxTable[i].m_adcRange != CyEAdcRangeInternalType.SignleEnded_2V))
                {
                    err = termEdit.TerminalAdd((auxrtnTermName + (i + 1).ToString()), CyCompDevTermDir.INOUT,
                                               CyCompDevTermType.ANALOG, new PointF(auxInTermDiffX, auxInTermY),
                        string.Empty, 180f, false);
                    if (err.IsNotOk) return err;
                    //auxInTermDiffY = auxInTermDiffY + offset;
                    //auxInTermSingleY = auxInTermSingleY + offset;
                    auxInTermY = auxInTermY + offset;
                    // add terminal names
                    PointF loc = new PointF(auxRtnAnnotationX, auxAnnotationY);
                    err = shapeEdit.CreateAnnotation(shapeTags1, (auxrtnTermName + (i + 1).ToString()), loc, type,
                        format);
                    if (err.IsNotOk) return err;
                    err = shapeEdit.SetFillColor(shapeTags1[0], Color.Black);
                    if (err.IsNotOk) return err;
                    err = shapeEdit.ClearOutline(shapeTags1[0]);
                    if (err.IsNotOk) return err;
                    auxAnnotationY = auxAnnotationY + offset;
                }
            }

            // calculate the body hieght based on the terminals added
            float bottomY;
            //float bottomY1 = (12f + 2*offset + (parameters.NumConverters * offset) + 12f);
            float bottomY1 = (parameters.NumConverters < 3) ? (78f) : (48f + (parameters.NumConverters * offset));
            float bottomY2 = ((parameters.NumAuxChannels > 0) ? (((-1) * (symbolATopY - auxInTermY))) :
                (12f + (4 * offset)));

            if (bottomY1 > bottomY2)
                bottomY = bottomY1;
            else
                bottomY = bottomY2;

            // Create first symbol body
            PointF symbolALoc = new PointF(symbolATopX, symbolATopY);
            err = shapeEdit.CreateRectangle(shapeTags, symbolALoc, symbolABodyWidth, bottomY);
            if (err.IsNotOk) return err;
            // Fill color
            err = shapeEdit.SetFillColor(shapeTags[0], Color.Gainsboro);
            if (err.IsNotOk) return err;
            // Set the outline width
            err = shapeEdit.SetOutlineWidth(shapeTags[0], 1F);
            if (err.IsNotOk) return err;
            err = shapeEdit.SendToBack(GENERATED_SHAPEA);
            if (err.IsNotOk) return err;

            if (symbolBVisible == true)
            {
                // Create second part of symbol body
                PointF symbolBLoc = new PointF(symbolBTopX, symbolBTopY);
                err = shapeEdit.CreateRectangle(shapeTags, symbolBLoc, symbolBBodyWidth, bottomY);
                if (err.IsNotOk) return err;
                // Fill color
                err = shapeEdit.SetFillColor(shapeTags[1], Color.Gainsboro);
                if (err.IsNotOk) return err;
                // Set the outline width
                err = shapeEdit.SetOutlineWidth(shapeTags[1], 1F);
                if (err.IsNotOk) return err;

                err = shapeEdit.SendToBack(GENERATED_SHAPEB);
                if (err.IsNotOk) return err;
            }
            else
            {
                err = shapeEdit.ShapesRemove("PMon_B_Header");
                if (err.IsNotOk) return err;

                err = shapeEdit.ShapesRemove("BHeaderText");
                if (err.IsNotOk) return err;
            }
            
            return err;
        }
        #endregion

        #region ICyAPICustomize_v1 Members
        private const string DEFAULT_VALUE = "0";
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;

            string element = string.Empty;
            string instanceName = query.InstanceName.ToString();

            CyParameters parameters = new CyParameters(query);

            // Voltages declarations
            string voltageType = "uint8 " + instanceName + "_VoltageType[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            string uvWarnThreshold = "uint16 " + instanceName + "_UVWarnThreshold[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            string ovWarnThreshold = "uint16 " + instanceName + "_OVWarnThreshold[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            string uvFaultThreshold = "uint16 " + instanceName + "_UVFaultThreshold[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            string ovFaultThreshold = "uint16 " + instanceName + "_OVFaultThreshold[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            string voltageScale = "float " + instanceName + "_VoltageScale[" +
                parameters.VoltagesTable.Count.ToString() + "] = { ";
            // Currents declarations
            string currentType = "uint8 " + instanceName + "_CurrentType[" +
                parameters.CurrentsTable.Count.ToString() + "] = { ";

            string currentChanNum = "uint8 " + instanceName + "_ActiveCurrentChan[";
            string ocWarnThreshold = "float " + instanceName + "_OCWarnThreshold[" +
                parameters.CurrentsTable.Count.ToString() + "] = { ";
            string ocFaultThrehsold = "float " + instanceName + "_OCFaultThreshold[" +
                parameters.CurrentsTable.Count.ToString() + "] = { ";
            string rShunt = "float " + instanceName + "_RShunt[" +
                parameters.CurrentsTable.Count.ToString() + "] = { ";
            string csaGain = "float " + instanceName + "_CSAGain[" +
                parameters.CurrentsTable.Count.ToString() + "] = { ";
            // Aux declarations
            string auxVoltageType = "uint8 " + instanceName + "_AuxVoltageType[" +
                parameters.AuxTable.Count.ToString() + "] = { ";

            int activeSourcesCount = 0;
            int count = 0;
            for (int i = 0; i < parameters.VoltagesTable.Count; i++)
            {
                element = (parameters.VoltagesTable[i].m_voltageMeasurementType == CyEVInternalType.SingleEnded) ?
                    "0" : "1";
                //                if (element == "1")
                //                    activeSourcesCount++;
                voltageType += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";

                element = ((UInt16)(CyParameters.NullableDoubleToDouble(parameters.VoltagesTable[i].m_uvWarningTreshold) * 1000)).ToString();
                uvWarnThreshold += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";

                element = ((UInt16)(CyParameters.NullableDoubleToDouble(parameters.VoltagesTable[i].m_ovWarningTreshold) * 1000)).ToString();
                ovWarnThreshold += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";

                element = ((UInt16)(CyParameters.NullableDoubleToDouble(parameters.VoltagesTable[i].m_uvFaultTreshold) * 1000)).ToString();
                uvFaultThreshold += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";

                element = ((UInt16)(CyParameters.NullableDoubleToDouble(parameters.VoltagesTable[i].m_ovFaultTreshold) * 1000)).ToString();
                ovFaultThreshold += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";

                element = CyParameters.NullableDoubleToDouble(parameters.VoltagesTable[i].m_inputScalingFactor).ToString();
                voltageScale += (i < parameters.VoltagesTable.Count - 1) ? element + ", " : element + " };";
            }

            for (int i = 0; i < parameters.CurrentsTable.Count; i++)
            {
                switch (parameters.CurrentsTable[i].m_currentMeasurementType)
                {
                    case CyECurrentMeasurementInternalType.None:
                        element = "0";
                        break;
                    case CyECurrentMeasurementInternalType.Direct:
                        element = "1";
                        break;
                    case CyECurrentMeasurementInternalType.CSA:
                        element = "2";
                        break;
                    default:
                        break;
                }
                if (element.Equals("1") || element.Equals("2"))
                    activeSourcesCount++;

                currentType += (i < parameters.CurrentsTable.Count - 1) ? element + ", " : element + " };";

                element = CyParameters.NullableDoubleToDouble(parameters.CurrentsTable[i].m_ocWarningThreshold).ToString();
                ocWarnThreshold += (i < parameters.CurrentsTable.Count - 1) ? element + ", " : element + " };";

                element = CyParameters.NullableDoubleToDouble(parameters.CurrentsTable[i].m_ocFaulthTreshold).ToString();
                ocFaultThrehsold += (i < parameters.CurrentsTable.Count - 1) ? element + ", " : element + " };";

                element = CyParameters.NullableDoubleToDouble(parameters.CurrentsTable[i].m_shuntResistorValue).ToString();
                rShunt += (i < parameters.CurrentsTable.Count - 1) ? element + ", " : element + " };";

                element = CyParameters.NullableDoubleToDouble(parameters.CurrentsTable[i].m_csaGain).ToString();
                csaGain += (i < parameters.CurrentsTable.Count - 1) ? element + ", " : element + " };";
            }

            currentChanNum += activeSourcesCount.ToString() + "] = { ";
            for (int i = 0; i < parameters.CurrentsTable.Count; i++)
            {
                switch (parameters.CurrentsTable[i].m_currentMeasurementType)
                {
                    case CyECurrentMeasurementInternalType.None:
                        element = "0";
                        break;
                    case CyECurrentMeasurementInternalType.Direct:
                        element = "1";
                        break;
                    case CyECurrentMeasurementInternalType.CSA:
                        element = "2";
                        break;
                    default:
                        break;
                }
                if (element.Equals("1") || element.Equals("2"))
                {
                    count++;
                    currentChanNum += (count < activeSourcesCount) ? (i + 1).ToString() + ", " : (i + 1).ToString() +
                        " };";
                }
            }

            for (int i = 0; i < parameters.AuxTable.Count; i++)
            {
                switch (parameters.AuxTable[i].m_adcRange)
                {
                    case CyEAdcRangeInternalType.SignleEnded_4V:
                    case CyEAdcRangeInternalType.SignleEnded_2V:
                        element = "0";
                        break;
                    case CyEAdcRangeInternalType.Differential_64mV:
                        element = "1";
                        break;
                    case CyEAdcRangeInternalType.Differential_2048mV:
                        element = "2";
                        break;
                    case CyEAdcRangeInternalType.Differential_128mV:
                        element = "3";
                        break;
                    default:
                        break;
                }
                auxVoltageType += (i < parameters.AuxTable.Count - 1) ? element + ", " : element + " };";
            }

            string numCurrentSources = "#define NUM_CURRENT_SOURCES " + parameters.CurrentsTable.Count.ToString();
            string numActiveCurrentSources = "#define " + instanceName + "_NUM_CURRENT_SOURCES             (" +
                                              activeSourcesCount.ToString() + "u)";

            // Voltages table arrays
            paramDict.Add("VoltageType", voltageType);
            paramDict.Add("UVWarnThreshold", uvWarnThreshold);
            paramDict.Add("OVWarnThreshold", ovWarnThreshold);
            paramDict.Add("UVFaultThreshold", uvFaultThreshold);
            paramDict.Add("OVFaultThreshold", ovFaultThreshold);
            paramDict.Add("VoltateScale", voltageScale);
            // Currents table arrays
            paramDict.Add("CurrentType", currentType);
            paramDict.Add("OCWarnThreshold", ocWarnThreshold);
            paramDict.Add("OCFaultThrehsold", ocFaultThrehsold);
            paramDict.Add("RShunt", rShunt);
            paramDict.Add("CSAGain", csaGain);
            // Aux table arrays
            if (parameters.AuxTable.Count > 0)
                paramDict.Add("AuxVoltageType", auxVoltageType);
            // General
            paramDict.Add("NumCurrentSources", numCurrentSources);
            paramDict.Add("NumActiveCurrentSources", numActiveCurrentSources);
            if (activeSourcesCount > 0)
                paramDict.Add("ActiveCurrentChan", currentChanNum);

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CyParameters prms = new CyParameters(args.InstQueryV1);

            for (int i = 0; i < prms.VoltagesTable.Count; i++)
            {
                if (CyParameters.VoltageTableRowHasEmptyCells(prms,prms.VoltagesTable[i])) // this means that empty row was saved
                {
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Resources.VoltagesTableEmptyRowsDRC);
                    break;
                }
            }

            for (int i = 0; i < prms.CurrentsTable.Count; i++)
            {
                if (CyParameters.CurrentTableRowHasEmptyCells(prms,prms.CurrentsTable[i]))
                // this means that empty row was saved
                {
                    yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Resources.CurrentsTableEmptyRowsDRC);
                    break;
                }
            }

            #region Calculate pins count
            int pinsCount = prms.NumConverters + prms.NumAuxChannels;
            for (int i = 0; i < prms.NumConverters; i++)
                if (prms.VoltagesTable[i].m_voltageMeasurementType == CyEVInternalType.Differential ||
                     prms.CurrentsTable[i].m_currentMeasurementType != CyECurrentMeasurementInternalType.None)
                    pinsCount++;

            for (int i = 0; i < prms.NumAuxChannels; i++)
                if ((prms.AuxTable[i].m_adcRange != CyEAdcRangeInternalType.SignleEnded_4V) && 
                (prms.AuxTable[i].m_adcRange != CyEAdcRangeInternalType.SignleEnded_2V))
                    pinsCount++;
            #endregion

            if (pinsCount > CyParamRanges.MAX_PINS_IN_SYSTEM)
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Resources.PinsCountLimitation);
        }
        #endregion
    }
}
