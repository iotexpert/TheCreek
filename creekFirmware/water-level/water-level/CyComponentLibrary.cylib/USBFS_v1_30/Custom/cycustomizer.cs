/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace USBFS_v1_30
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        private bool m_EditParamsOnDrop = false;
        private CyCompDevParamEditorMode m_Mode = CyCompDevParamEditorMode.COMPLETE;
        private ICyInstEdit_v1 m_Component = null;
        private CyUSBFSParameters Parameters;

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            //Debug.Assert(false);
            this.m_Component = edit;
            Parameters = new CyUSBFSParameters(edit);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param) { };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.AddCustomPage("Device Descriptor", new CyDeviceDescriptorEditingControl(Parameters), ParamCommitted,
                                 "Device Descriptor");
            editor.AddCustomPage("String Descriptor", new CyStringDescriptorEditingControl(Parameters), ParamCommitted,
                                 "String Descriptor");
            editor.AddCustomPage("HID Descriptor", new CyHIDDescriptorEditingControl(Parameters), ParamCommitted,
                                 "HID Descriptor");
            editor.AddDefaultPage("Advanced", "Advanced");
            editor.AddDefaultPage("Built-in", "Built-in");
            editor.ParamExprCommittedDelegate = ParamCommitted;
            DialogResult result = editor.ShowDialog();
            editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
            return result;
        }

        public bool EditParamsOnDrop
        {
            get { return m_EditParamsOnDrop; }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return m_Mode;
        }

        #endregion

        private bool InterceptHelp()
        {
            //Do whatever you want here.
            return true;
        }
    }


    public class CyDeviceDescriptorEditingControl : ICyParamEditingControl
    {
        private CyDeviceDescriptor m_control;

        public CyDeviceDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyDeviceDescriptor(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errorList = new List<CyCustErr>();
            m_control.Parameters.RecalcDescriptors(m_control.Parameters.HIDReportTree.Nodes[0]);
            m_control.Parameters.RecalcDescriptors(m_control.Parameters.DeviceTree.Nodes[0]);
            m_control.Parameters.SerializedDeviceDesc =
                DescriptorTree.SerializeDescriptors(m_control.Parameters.DeviceTree);
            m_control.Parameters.SetParam_rm_ep_isr();
            // set parameter mon_vbus in Advenced tab
            //m_control.Parameters.SetParam_mon_vbus();

            //If there are empty fields
            if (m_control.Parameters.EmptyFields.Count > 0)
                errorList.Add(
                    new CyCustErr(
                        "Undefined HID Report for the HID Class Descriptor.\r\n To fix this problem you can:" +
                        "\r\n1)  Configure the HID Class Descriptor to refer to a HID Report, or" +
                        "\r\n2)  Remove the HID Class Descriptor from the Configuration"));
            return errorList.ToArray();
        }

        #endregion
    }

    public class CyStringDescriptorEditingControl : ICyParamEditingControl
    {
        private CyStringDescriptor m_control;

        public CyStringDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyStringDescriptor(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            m_control.Parameters.SerializedStringDesc =
                DescriptorTree.SerializeDescriptors(m_control.Parameters.StringTree);

            return new CyCustErr[] {};
        }

        #endregion
    }

    public class CyHIDDescriptorEditingControl : ICyParamEditingControl
    {
        private CyHIDDescriptor m_control;

        public CyHIDDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyHIDDescriptor(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            m_control.Parameters.SerializedHIDReportDesc =
                DescriptorTree.SerializeDescriptors(m_control.Parameters.HIDReportTree);

            return new CyCustErr[] {};
        }

        #endregion
    }
}