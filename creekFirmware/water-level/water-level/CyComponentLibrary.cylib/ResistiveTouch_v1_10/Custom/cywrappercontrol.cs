/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;

namespace ResistiveTouch_v1_10
{
    /// <summary>
    /// Wrapper for tabs
    /// </summary>
    public class CyEditingWrapperControl : UserControl, ICyParamEditingControl
    {
        public static bool RUN_MODE = false;
        protected CyParameters m_parameters = null;

        public virtual string TabName
        {
            get { return "Empty"; }
        }

        public CyEditingWrapperControl()
        {
            this.Load += new EventHandler(CyEditingWrapperControl_Load);
        }

        void CyEditingWrapperControl_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            if (RUN_MODE)
                this.Dock = DockStyle.Fill;
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        // Gets any errors that exist for parameters on the DisplayControl.
        public virtual IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_parameters.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_parameters.m_inst.GetCommittedParam(paramName);
                if (param.TabName.Equals(TabName))
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            yield return new CyCustErr(errMsg);
                        }
                    }
                }
            }

        }
        #endregion
    }
}
