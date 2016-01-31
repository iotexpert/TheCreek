/*******************************************************************************
* Copyright 2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace SPDIF_Tx_v1_0
{
    public class CyEditingWrapperControl : UserControl, ICyParamEditingControl
    {
        public static bool m_runMode = false;
        protected CySPDifTxParameters m_params = null;

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
            if (m_runMode)
            {
                this.Dock = DockStyle.Fill;
                this.AutoScroll = true;
            }
        }

        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get { return this; }
        }

        // Gets any errors that exist for parameters on the DisplayControl.
        public IEnumerable<CyCustErr> GetErrors()
        {
            foreach (string paramName in m_params.m_inst.GetParamNames())
            {
                CyCompDevParam param = m_params.m_inst.GetCommittedParam(paramName);
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