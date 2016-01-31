/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using cyboot_v1_0;

namespace cyboot_v1_0
{
    public class CyCustomizer : ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members

        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            throw new NotImplementedException();
        }

        public bool EditParamsOnDrop
        {
            get { throw new NotImplementedException(); }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
