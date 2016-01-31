/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


 
using System.Collections.Generic;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;
 
namespace USBFS_v1_60
{
    [CyCompDevCustomizer()]
    public partial class CyCustomizer : ICyParamEditHook_v1, ICyBootLoaderSupport
    {
        public const string PAPAM_TAB_NAME_DEVICE = "Device";
        public const string PAPAM_TAB_NAME_STRING = "String";
        public const string PAPAM_TAB_NAME_HID = "Hid";
        public const string PAPAM_TAB_NAME_AUDIO = "Audio";
        public const string PAPAM_TAB_NAME_ADVANCED = "Advanced";
        public const string PAPAM_TAB_BUILTIN = "Built-in";

        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {  
            CyUSBFSParameters parameters = new CyUSBFSParameters(edit);
            CyAdvancedEditingControl advControl = new CyAdvancedEditingControl(parameters);
            CyParamExprDelegate ParamCommitted = delegate(ICyParamEditor custEditor, CyCompDevParam param)
                             {
                                 parameters.GetAdvancedParams();
                                 if (param.TabName == PAPAM_TAB_NAME_ADVANCED)
                                     ((CyAdvancedPage)(advControl.DisplayControl)).InitFields();
                             };

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
            editor.UseBigEditor = true;
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_DEVDESC,
                                 new CyDeviceDescriptorEditingControl(parameters), ParamCommitted, 
                                 PAPAM_TAB_NAME_DEVICE);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_STRDESC,
                                 new CyStringDescriptorEditingControl(parameters), ParamCommitted, 
                                 PAPAM_TAB_NAME_STRING);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_HIDDESC, new CyHIDDescriptorEditingControl(parameters),
                                 ParamCommitted, PAPAM_TAB_NAME_HID);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_AUDIODESC,
                                 new CyAudioDescriptorEditingControl(parameters), ParamCommitted, PAPAM_TAB_NAME_AUDIO);
            editor.AddCustomPage(Properties.Resources.PAGE_TITLE_ADV, advControl,
                                 ParamCommitted, PAPAM_TAB_NAME_ADVANCED);
            editor.AddDefaultPage(Properties.Resources.PAGE_TITLE_BUILTIN, PAPAM_TAB_BUILTIN);

            DialogResult result = editor.ShowDialog();
            return result;
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

        #region ICyBootLoaderSupport Members

        /// <summary>
        /// The bootloader requires that the communication component is configured for
        /// both transfer in and out of the PSoC device. This method lets the implementing
        /// component inform PSoC Creator if it is currently configured to handle input and
        /// output.
        /// <param name="query">The ICyInstQuery for the relevant instance of the component.</param>
        /// </summary>
        public CyCustErr IsBootloaderReady(ICyInstQuery_v1 query)
        {
			CyCustErr supportBootloader = CyCustErr.OK;

            CyCompDevParam param = query.GetCommittedParam(CyUSBFSParameters.PARAM_DEVICEDESCRIPTORS);
            //Extract parameters
            CyUSBFSParameters parameters = new CyUSBFSParameters();
            string devDescParam;
            param.TryGetValueAs(out devDescParam);
            parameters.m_serializedDeviceDesc = devDescParam;
            parameters.DeserializeDescriptors();
            if (parameters.CheckBootloaderReady() == false)
                supportBootloader = new CyCustErr(Properties.Resources.MSG_BOOTLOADER_SUPPORT);
			return supportBootloader;
		}
		
		#endregion

        public static List<CyCustErr> GetErrors(ICyInstEdit_v1 edit, string tabName)
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            if (edit != null)
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == tabName)
                    {
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                errs.Add(new CyCustErr(errMsg));
                            }
                        }
                    }
                }

            return errs;
        }
    }


    public class CyDeviceDescriptorEditingControl : ICyParamEditingControl
    {
        private CyDeviceDescriptorPage m_control;

        public CyDeviceDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyDeviceDescriptorPage(parameters);
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
            // Save all changes before closing the customizer
            m_control.m_parameters.RecalcDescriptors(m_control.m_parameters.m_hidReportTree.m_nodes[0]);
            m_control.m_parameters.RecalcDescriptors(m_control.m_parameters.m_deviceTree.m_nodes[0]);
            m_control.m_parameters.SerializedDeviceDesc =
                CyDescriptorTree.SerializeDescriptors(m_control.m_parameters.m_deviceTree,
                                                      m_control.m_parameters.m_serializer,
                                                      m_control.m_parameters.m_customSerNamespace);
            m_control.m_parameters.SetParam_rm_ep_isr();

            //If there are empty fields
            if (m_control.m_parameters.m_emptyFields.Count > 0)
                errorList.Add(new CyCustErr(Properties.Resources.MSG_UNDEFINED_HIDREPORT));
            errorList.AddRange(CyCustomizer.GetErrors(m_control.m_parameters.m_inst, 
                                                      CyCustomizer.PAPAM_TAB_NAME_DEVICE));
            return errorList.ToArray();
        }

        #endregion
    }

    public class CyStringDescriptorEditingControl : ICyParamEditingControl
    {
        private CyStringDescriptorPage m_control;

        public CyStringDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyStringDescriptorPage(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            // Save all changes before closing the customizer
            m_control.m_parameters.SerializedStringDesc =
                CyDescriptorTree.SerializeDescriptors(m_control.m_parameters.m_stringTree,
                                                      m_control.m_parameters.m_serializer,
                                                      m_control.m_parameters.m_customSerNamespace);

            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.PAPAM_TAB_NAME_STRING);
        }

        #endregion
    }

    public class CyHIDDescriptorEditingControl : ICyParamEditingControl
    {
        private CyHidDescriptorPage m_control;

        public CyHIDDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyHidDescriptorPage(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            // Save all changes before closing the customizer
            m_control.m_parameters.SerializedHIDReportDesc =
                CyDescriptorTree.SerializeDescriptors(m_control.m_parameters.m_hidReportTree,
                                                      m_control.m_parameters.m_serializer,
                                                      m_control.m_parameters.m_customSerNamespace);

            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.PAPAM_TAB_NAME_HID);
        }

        #endregion
    }

    public class CyAudioDescriptorEditingControl : ICyParamEditingControl
    {
        private CyAudioDescriptorPage m_control;

        public CyAudioDescriptorEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyAudioDescriptorPage(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            // Save all changes before closing the customizer
            m_control.m_parameters.SerializedAudioDesc =
                CyDescriptorTree.SerializeDescriptors(m_control.m_parameters.m_audioTree,
                                                      m_control.m_parameters.m_serializer,
                                                      m_control.m_parameters.m_customSerNamespace);

            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.PAPAM_TAB_NAME_AUDIO);
        }

        #endregion
    }

    public class CyAdvancedEditingControl : ICyParamEditingControl
    {
        private CyAdvancedPage m_control;

        public CyAdvancedEditingControl(CyUSBFSParameters parameters)
        {
            m_control = new CyAdvancedPage(parameters);
            m_control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return m_control; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            return CyCustomizer.GetErrors(m_control.m_parameters.m_inst, CyCustomizer.PAPAM_TAB_NAME_ADVANCED);
        }

        #endregion
    }
}