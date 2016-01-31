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

namespace VoltageFaultDetector_v1_0
{
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyAPICustomize_v1, ICyShapeCustomize_v1,
                                        ICyDRCProvider_v1
    {
        public const string BASIC_TAB_NAME = "General";
        public const string VOLTAGE_TAB_NAME = "Voltages";
        public const string BUILTIN_TAB_NAME = "Built-in";

        #region ICyParamEditHook_v1 Members
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery,
            ICyExpressMgr_v1 mgr)
        {
            CyParameters parameters = new CyParameters(edit, termQuery);
            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            CyBasicTab basicTab = new CyBasicTab(parameters);
            CyVoltagesTab voltagesTab = new CyVoltagesTab(parameters);

            CyParamExprDelegate exprDelegate = delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                parameters.GlobalEditMode = false;
                basicTab.UpdateUI();
                parameters.GlobalEditMode = true;
            };

            editor.AddCustomPage(Resources.GeneralTabDisplayName, basicTab, exprDelegate, BASIC_TAB_NAME);
            editor.AddCustomPage(Resources.VoltagesTabDisplayName, voltagesTab, exprDelegate, VOLTAGE_TAB_NAME);
            editor.AddDefaultPage(BUILTIN_TAB_NAME, BUILTIN_TAB_NAME);

            edit.NotifyWhenDesignUpdates(new CyDesignUpdated_v1(parameters.UpdateClock));

            basicTab.UpdateUI();
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
            const string BODY_SHAPE = "VFD_body";
            const string GENERATED_SHAPE = "VFD_body_gen";

            CyCustErr err = CyCustErr.OK;

            RectangleF bodyRect = shapeEdit.GetShapeBounds(BODY_SHAPE);
            err = shapeEdit.ShapesRemove(BODY_SHAPE);
            if (err.IsNotOk) return err;

            byte numVoltages = 0;
            err = instQuery.GetCommittedParam(CyParamNames.NUM_VOLTAGES).TryGetValueAs<byte>(out numVoltages);
            if (err.IsNotOk) return err;

            float offset = 12.0F;
            PointF bodyLoc = bodyRect.Location;
            float bodyWidth = bodyRect.Width;
            float basicBodyHeight = offset * 5F;
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

            CyParameters parameters = new CyParameters(query);

            string element = string.Empty;
            double dacRange = (parameters.DACRange == CyDACRangeType.V1) ? 1.024 : 4.096;
            
            // Voltages declarations
            string uvFaultThreshold = "uint8 " + query.InstanceName + "_UVFaultThreshold[" +
                parameters.NumVoltages.ToString() + "] = { ";
            string ovFaultThreshold = "uint8 " + query.InstanceName + "_OVFaultThreshold[" +
                parameters.NumVoltages.ToString() + "] = { ";
            string voltageScale = "uint16 " + query.InstanceName + "_VoltageScale[" +
                parameters.NumVoltages.ToString() + "] = { ";

            for (int i = parameters.VoltagesTable.Count - 1; i >= 0; i--)
            {
                double mult = dacRange;
                if (parameters.VoltagesTable[i].m_inputScalingFactor.HasValue)
                {
                    mult /= parameters.VoltagesTable[i].m_inputScalingFactor.Value;
                }

                uvFaultThreshold += ConvertValueToArrayElement(parameters.VoltagesTable[i].m_uvFaultThreshold, i,
                    parameters.VoltagesTable.Count, false, mult);

                ovFaultThreshold += ConvertValueToArrayElement(parameters.VoltagesTable[i].m_ovFaultThreshold, i,
                    parameters.VoltagesTable.Count, false, mult);

                voltageScale += ConvertValueToArrayElement(parameters.VoltagesTable[i].m_inputScalingFactor, i,
                    parameters.VoltagesTable.Count, true, mult);
            }

            // Voltages table arrays
            if (parameters.ExternalRef == false)
            {
                if (parameters.CompareType != CyCompareType.OV)
                {
                    paramDict.Add("UVFaultThreshold", uvFaultThreshold);
                }
                if (parameters.CompareType != CyCompareType.UV)
                {
                    paramDict.Add("OVFaultThreshold", ovFaultThreshold);
                }
                paramDict.Add("VoltageScale", voltageScale);
            }

            // Mask
            UInt32 statusMask = ~(0xFFFFFFFF << parameters.NumVoltages);
            paramDict.Add("StatusMask", String.Format("0x{0}u", statusMask.ToString("X8")));

            for (int i = 0; i < customizers.Count; i++)
            {
                customizers[i].MacroDictionary = paramDict;
            }

            return customizers;
        }

        private string ConvertValueToArrayElement(double? elem, int i, int count, bool isScale, double mult)
        {
            string element = string.Empty;

            if (elem.HasValue)
            {
                if (isScale)
                {
                    element = (elem * 1000).ToString();
                }
                else
                {
                    element = ((UInt16)Math.Round(elem.Value * 256.0 / mult)).ToString();
                }
            }
            else
            {
                element = "0";
            }
            element = (i > 0) ? element + ", " : element + " };";

            return element;
        }
        #endregion

        #region ICyDRCProvider_v1 Members
        /// <summary>
        /// DRC checks if there are empty cells in the Voltages table.
        /// Note: Voltage Name column can have empty cells.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IEnumerable<CyDRCInfo_v1> GetDRCs(ICyDRCProviderArgs_v1 args)
        {
            CyParameters parameters = new CyParameters(args.InstQueryV1);

            if (parameters.CheckTableNullValues() == false)
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Resources.DrcNullValuesError);
            }
            if (!parameters.CheckSiliconRevisionVsExtRef(args.DeviceQueryV1))
            {
                yield return new CyDRCInfo_v1(CyDRCInfo_v1.CyDRCType_v1.Error, Resources.DRCExtRefError);
            }
        }
        #endregion
    }
}
