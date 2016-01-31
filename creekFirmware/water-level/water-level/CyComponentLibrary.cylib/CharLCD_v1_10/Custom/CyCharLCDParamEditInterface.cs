/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;


namespace CharLCD_v1_10
{
    [CyCompDevCustomizer]
    public partial class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members

        // Type of dialog box.  Advanced opens with a button on the generic form.  Complete opens immediately
        CyCompDevParamEditorMode mode = CyCompDevParamEditorMode.COMPLETE;
        // Run Instance Customizer on Instance Selection
        bool editOnDrop = false;
        

        /// <summary>
        /// Edit Params initializes the data in the customizer as well as 
        /// instantiating the dialog.  
        /// </summary>
        /// <param name="obj"> Is a component object containing the parameter information for the object. </param>
        /// <returns>A DialogResult object associated with the form.  OK means accept changes.  Cancel means reject changes.</returns>
        DialogResult ICyParamEditHook_v1.EditParams(ICyInstEdit_v1 obj, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            ICyParamEditor editor = obj.CreateParamEditor(new CyCharacterLCDEditingControl(obj));
            System.Windows.Forms.DialogResult result = editor.ShowDialog();
            //editor.InterceptHelpRequest = new CyHelpDelegate(InterceptHelp);
            return result;
        }

        CyCompDevParamEditorMode ICyParamEditHook_v1.GetEditorMode()
        {
            return this.mode;
        }

        bool ICyParamEditHook_v1.EditParamsOnDrop
        {
            get { return editOnDrop; }
        }
        #endregion
    }

    public class CyCharacterLCDEditingControl : ICyParamEditingControl
    {
        CyCharLCDControl control;

        public CyCharacterLCDEditingControl(ICyInstEdit_v1 obj)
        {
            this.control = new CyCharLCDControl(obj);
            this.control.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members

        Control ICyParamEditingControl.DisplayControl
        {
            get { return control; }
        }

        IEnumerable<CyCustErr> ICyParamEditingControl.GetErrors()
        {
            // Hack : Update this method to return actual errors?
            return new CyCustErr[] { };
        }

        #endregion
    }
}
