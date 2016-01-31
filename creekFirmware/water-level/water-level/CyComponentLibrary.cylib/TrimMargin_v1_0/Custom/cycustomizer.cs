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

namespace TrimMargin_v1_0
{
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyAPICustomize_v1, ICyShapeCustomize_v1, ICyDRCProvider_v1
    {
        public const string BASIC_TAB_NAME = "General";
        public const string BASIC_TAB_DISPLAY_NAME = "General";
        public const string VOLTAGE_TAB_NAME = "Voltages";
        public const string VOLTAGE_TAB_DISPLAY_NAME = "Voltages";
        public const string HARDWARE_TAB_DISPLAY_NAME = "Hardware";
        public const string BUILTIN_TAB_NAME = "Built-in";

        private const int MAX_LINE_LENGTH = 120;

        #region ICyParamEditHook_v1 Members
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
            ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit);
            parameters.m_term = termQuery;
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyVoltagesTab voltagesTab = new CyVoltagesTab(parameters);
            CyHardwareTab hardwareTab = new CyHardwareTab(parameters);
            
            CyParamExprDelegate exprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GlobalEditMode = false;
                voltagesTab.UpdateUI();
                hardwareTab.UpdateUI();
                parameters.GlobalEditMode = true;
            };
            
            editor.AddCustomPage(VOLTAGE_TAB_DISPLAY_NAME, voltagesTab, exprDelegate, BASIC_TAB_NAME, VOLTAGE_TAB_NAME);
            editor.AddCustomPage(HARDWARE_TAB_DISPLAY_NAME, hardwareTab, exprDelegate, VOLTAGE_TAB_NAME);

            editor.AddDefaultPage(BUILTIN_TAB_NAME, BUILTIN_TAB_NAME);
            editor.UseBigEditor = true;

            edit.NotifyWhenDesignUpdates(hardwareTab.UpdateClockDependedValues);

            parameters.GlobalEditMode = true;
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

        bool UseBigEditor
        {
            get { return true; }
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }
        #endregion

        #region ICyShapeCustomize_v1 Members
        public CyCustErr CustomizeShapes(ICyInstQuery_v1 instQuery, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            const string BODY_SHAPE = "Symbol_body";
            const string GENERATED_SHAPE = "Symbol_body_gen";

            CyCustErr err = CyCustErr.OK;

            // We leave the symbol as it is for symbol preview
            //if (instQuery.IsPreviewCanvas)
            //    return CyCustErr.OK;

            RectangleF bodyRect = shapeEdit.GetShapeBounds(BODY_SHAPE);
            err = shapeEdit.ShapesRemove(BODY_SHAPE);
            if (err.IsNotOk) return err;

            byte numVoltages = 0;
            err = instQuery.GetCommittedParam(CyParamNames.NUM_CONVERTERS).TryGetValueAs<byte>(out numVoltages);
            if (err.IsNotOk) return err;

            float offset = 12.0F;
            PointF bodyLoc = bodyRect.Location;
            float bodyWidth = bodyRect.Width;
            float basicBodyHeight = offset * 2.5F;
            float realBodyHeight = (basicBodyHeight + (numVoltages * offset));

            err = shapeEdit.CreateRectangle(new string[] { GENERATED_SHAPE }, bodyLoc, bodyWidth, realBodyHeight);
            if (err.IsNotOk) return err;

            // Set the color and outline width of symbol body
            err = shapeEdit.SetFillColor(GENERATED_SHAPE, Color.Gainsboro);
            if (err.IsNotOk) return err;
            err = shapeEdit.SetOutlineWidth(GENERATED_SHAPE, 1F);
            if (err.IsNotOk) return err;

            err = shapeEdit.SendToBack(GENERATED_SHAPE);
            if (err.IsNotOk) return err;

            return err;
        }
        #endregion

        #region ICyAPICustomize_v1 Members
        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
            IEnumerable<CyAPICustomizer> apis)
        {
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            if (customizers.Count > 0) paramDict = customizers[0].MacroDictionary;

            CyParameters parameters = new CyParameters(query, termQuery);
            string instanceName = query.InstanceName.ToString();


            UInt32 pwmMax = Convert.ToUInt32(Math.Pow(2, (int)parameters.PWMResolution) - 1);
            string element = string.Empty;


            // Voltages declarations
            string vMarginLow = "const uint16 CYCODE " + instanceName + "_VMARGIN_LOW[" +
                parameters.NumConverters.ToString() + "] = { ";
            string vMarginHigh = "const uint16 CYCODE " + instanceName + "_VMARGIN_HIGH[" +
                parameters.NumConverters.ToString() + "] = { ";
            string polarity = "const uint8 CYCODE " + instanceName + "_POLARITY[" + parameters.NumConverters.ToString()
                + "] = { ";
            string vDDIO = "const uint16 CYCODE " + instanceName + "_VDDIO[" + parameters.NumConverters.ToString() +
                "] = { ";

            string r1 = "const uint32 CYCODE " + instanceName + "_R1[" + parameters.NumConverters.ToString() +
                "] = { ";
            string r2 = "const uint32 CYCODE " + instanceName + "_R2[" + parameters.NumConverters.ToString() +
                "] = { ";
            string r3 = "const uint32 CYCODE " + instanceName + "_R3[" + parameters.NumConverters.ToString() +
                "] = { ";

            string vAdj = "const uint16 CYCODE " + instanceName + "_VADJ[" + parameters.NumConverters.ToString() +
                "] = { ";
            string vNominal = "const uint16 CYCODE " + instanceName + "_VNOMINAL[" + parameters.NumConverters.ToString()
                + "] = { ";
            string vDelta = "const uint16 CYCODE " + instanceName + "_VDELTA[" + parameters.NumConverters.ToString() +
                "] = { ";

            string dutyCycleType  = string.Empty;
            if (parameters.PWMResolution > 8)
            {
                dutyCycleType = "uint16";
            }
            else
            {
                dutyCycleType = "uint8";
            }    
            string pwmNom = "const " + dutyCycleType + " CYCODE " + instanceName + "_DUTYCYCLE[" +
                parameters.NumConverters.ToString() + "] = { ";
            string pwmPre = "const " + dutyCycleType + " CYCODE " + instanceName + "_PRE_RUN_DUTYCYCLE[" +
                parameters.NumConverters.ToString() + "] = { ";
            string pwmPerMV = "const " + dutyCycleType + " CYCODE " + instanceName + "_DUTYCYCLES_PER_VOLT[" +
                parameters.NumConverters.ToString() + "] = { ";
            string pwmVMarginLow = "const " + dutyCycleType + " CYCODE " + instanceName + "_VMARGIN_LOW_DUTYCYCLE[" +
                parameters.NumConverters.ToString() + "] = { ";
            string pwmVMarginHigh = "const " + dutyCycleType + " CYCODE " + instanceName + "_VMARGIN_HIGH_DUTYCYCLE[" +
                parameters.NumConverters.ToString() + "] = { ";

            for (int i = 0; i < parameters.NumConverters; i++)
            {
                parameters.CalculateTableValues(i);
                bool last = (i == parameters.NumConverters - 1);

                element = Convert.ToUInt16(parameters.VoltagesTable[i].m_marginLow * 1000).ToString();
                AddStringElement(ref vMarginLow, element, last);

                element = Convert.ToUInt16(parameters.VoltagesTable[i].m_marginHigh * 1000).ToString();
                AddStringElement(ref vMarginHigh, element, last);

                element = Convert.ToByte(parameters.HardwareTable[i].m_polarity).ToString();
                AddStringElement(ref polarity, element, last);

                element = Convert.ToUInt16(parameters.HardwareTable[i].m_vddio * 1000).ToString();
                AddStringElement(ref vDDIO, element, last);

                element = Convert.ToUInt32(parameters.HardwareTable[i].m_r1 * 1000).ToString();
                AddStringElement(ref r1, element, last);

                element = Convert.ToUInt32(parameters.HardwareTable[i].m_r2 * 1000).ToString();
                AddStringElement(ref r2, element, last);

                element = Convert.ToUInt32(parameters.HardwareTable[i].m_r3 * 1000).ToString();
                AddStringElement(ref r3, element, last);

                element = Convert.ToUInt16(parameters.HardwareTable[i].m_controlVoltage * 1000).ToString();
                AddStringElement(ref vAdj, element, last);

                UInt16 pwmNomElement = Convert.ToUInt16((Math.Pow(2, (int)parameters.PWMResolution) - 1) *
                    parameters.HardwareTable[i].m_controlVoltage / parameters.HardwareTable[i].m_vddio);
                element = pwmNomElement.ToString();
                AddStringElement(ref pwmNom, element, last);

                // Generation pwmPre array
                if (parameters.HardwareTable[i].m_polarity == CyPWMPolarityType.Positive)
                {
                    element = pwmNomElement.ToString();
                }
                else
                {
//                    UInt32 pwmPreElement = Convert.ToUInt32(pwmNomElement * (
//                        (parameters.HardwareTable[i].m_calculatedR4 + parameters.HardwareTable[i].m_r3) /
//                        (parameters.HardwareTable[i].m_r2 * parameters.HardwareTable[i].m_r1 /
//                        (parameters.HardwareTable[i].m_r2 + parameters.HardwareTable[i].m_r1)) + 1)
//                        );
                    UInt32 pwmPreElement = Convert.ToUInt32(pwmNomElement * (
                        parameters.HardwareTable[i].m_calculatedR4 / (parameters.HardwareTable[i].m_r3 +
                        (parameters.HardwareTable[i].m_r2 * parameters.HardwareTable[i].m_r1 /
                        (parameters.HardwareTable[i].m_r2 + parameters.HardwareTable[i].m_r1))) + 1)
                        );

                    if (pwmPreElement > pwmMax) pwmPreElement = pwmMax;
                    element = pwmPreElement.ToString();
                }
                AddStringElement(ref pwmPre, element, last);

                // Generation VNominal array
                element = Convert.ToUInt16(parameters.VoltagesTable[i].m_nominalVoltage * 1000).ToString();
                AddStringElement(ref vNominal, element, last);

                // Generation VDelta array
                double vDeltaElement = 0;
                double pwmPerMVElement = 0;
                double vMaxnew = 0;
                double R2real = 0;
                if (parameters.HardwareTable[i].m_polarity == CyPWMPolarityType.Positive)
                {
                    vDeltaElement = (double)(parameters.HardwareTable[i].m_controlVoltage / pwmNomElement * 1000);
                    pwmPerMVElement = (double)(pwmNomElement / parameters.HardwareTable[i].m_controlVoltage );
                }
                else
                {
                    // R2real = VADJ * R1 / (VNOM - VADJ)
                    if ((parameters.VoltagesTable[i].m_nominalVoltage - parameters.HardwareTable[i].m_controlVoltage)
                        == 0)
                    {
                        R2real = (double)parameters.HardwareTable[i].m_r2;
                    }
                    else
                    {
                        R2real = (double)(parameters.HardwareTable[i].m_controlVoltage *
                                           parameters.HardwareTable[i].m_r1 /
                        (parameters.VoltagesTable[i].m_nominalVoltage - parameters.HardwareTable[i].m_controlVoltage));
                    }
                    // VMAX = VADJ * R1 / (R2real || (R3+R4)) + VADJ
                    vMaxnew = (double)(parameters.HardwareTable[i].m_controlVoltage * parameters.HardwareTable[i].m_r1 /
                        (R2real * (parameters.HardwareTable[i].m_r3 + parameters.HardwareTable[i].m_calculatedR4) /
                        (R2real + parameters.HardwareTable[i].m_r3 + parameters.HardwareTable[i].m_calculatedR4)) +
                        parameters.HardwareTable[i].m_controlVoltage);
                    // PWM_PER_MV = PWM_NOM / (VMAX - VNOM)
                    pwmPerMVElement = pwmNomElement / (vMaxnew - (double)parameters.VoltagesTable[i].m_nominalVoltage);
                    // VDELTA = (VMAX - VNOM) / PWM_NOM
                    vDeltaElement = (vMaxnew - (double)parameters.VoltagesTable[i].m_nominalVoltage) / pwmNomElement *
                                    1000 / 2;
                }

                double pwmVMarginLowElement = pwmNomElement + pwmPerMVElement * 
                    ((double)(parameters.VoltagesTable[i].m_nominalVoltage - parameters.VoltagesTable[i].m_marginLow));
                if (pwmVMarginLowElement < 0) pwmVMarginLowElement = 0;
                if (pwmVMarginLowElement > pwmMax) pwmVMarginLowElement = pwmMax;
                element = Convert.ToUInt16(pwmVMarginLowElement).ToString();
                AddStringElement(ref pwmVMarginLow, element, last);
                
                double pwmVMarginHighElement = pwmNomElement - pwmPerMVElement * 
                    ((double)(parameters.VoltagesTable[i].m_marginHigh - parameters.VoltagesTable[i].m_nominalVoltage));
                if (pwmVMarginHighElement < 0) pwmVMarginHighElement = 0;
                if (pwmVMarginHighElement > pwmMax) pwmVMarginHighElement = pwmMax;
                element = Convert.ToUInt16(pwmVMarginHighElement).ToString();
                AddStringElement(ref pwmVMarginHigh, element, last);

                // Don't want minus or zero value
                if (vDeltaElement < 1) vDeltaElement = 1;
                if (vDeltaElement > pwmMax) vDeltaElement = pwmMax;
                element = Convert.ToUInt16(vDeltaElement).ToString();
                AddStringElement(ref vDelta, element, last);
                if(pwmPerMVElement < 1) pwmPerMVElement = 1;
                if(pwmPerMVElement > pwmMax) pwmPerMVElement = pwmMax;
                element = Convert.ToUInt16(pwmPerMVElement).ToString();
                AddStringElement(ref pwmPerMV, element, last);
            }

            paramDict.Add("VMarginLowArray", WordWrap(vMarginLow));
            paramDict.Add("VMarginHighArray", WordWrap(vMarginHigh));
            paramDict.Add("PolarityArray", WordWrap(polarity));
            paramDict.Add("VDDIOArray", WordWrap(vDDIO));

            paramDict.Add("R1Array", WordWrap(r1));
            paramDict.Add("R2Array", WordWrap(r2));
            paramDict.Add("R3Array", WordWrap(r3));

            paramDict.Add("VAdjArray", WordWrap(vAdj));
            paramDict.Add("VNominalArray", WordWrap(vNominal));
            paramDict.Add("vDeltaArray", WordWrap(vDelta));

            paramDict.Add("PWMNomArray", WordWrap(pwmNom));
            paramDict.Add("PWMPreArray", WordWrap(pwmPre));
            paramDict.Add("PWMPerMVArray", WordWrap(pwmPerMV));

            paramDict.Add("pwmVMarginLowArray", WordWrap(pwmVMarginLow));
            paramDict.Add("pwmVMarginHighArray", WordWrap(pwmVMarginHigh));

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }
        void AddStringElement(ref string baseStr, string element, bool last)
        {
            baseStr += last == false ? element + ", " : element + " }";
        }

        private string WordWrap(string str)
        {
            string result = string.Empty;
            string line = string.Empty;
            string[] splitted = str.Split(',');
            for (int i = 0; i < splitted.Length; i++)
            {
                if ((line.Length + splitted[i].Length + 1) <= MAX_LINE_LENGTH)
                {
                    line += splitted[i] + ",";
                }
                else
                {
                    line += "\r\n";
                    result += line;
                    line = string.Empty;
                    line += splitted[i] + ",";
                }
            }
            result += line.Remove(line.Length - 1);

            return result;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CyParameters parameters = new CyParameters(args.InstQueryV1);

            if (parameters.CheckVoltagesTableNullValues() == false)
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, string.Format(Resources.
                    DrcNullValuesError, CyCustomizer.VOLTAGE_TAB_DISPLAY_NAME));
            }
            if (parameters.CheckHardwareTableNullValues() == false)
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, string.Format(Resources.
                    DrcNullValuesError, CyCustomizer.HARDWARE_TAB_DISPLAY_NAME));
            }
        }
        #endregion
    }
}
