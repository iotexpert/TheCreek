/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace PrISM_v1_20
{
    public class CyCustomTextBox : System.Windows.Forms.TextBox
    {
        // This method intercepts the Enter Key signal before the containing Form does
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message m, System.Windows.Forms.Keys k)
        {
            // detect the pushing of Enter Key
            if (m.Msg == 256 && k == System.Windows.Forms.Keys.Enter)
            {
                // Execute an alternative action: here we tabulate in order to focus on the next control in the formular
                System.Windows.Forms.SendKeys.Send("\t");
                // return true to stop any further interpretation of this key action
                return true;
            }
            // if not pushing Enter Key, then process the signal as usual
            return base.ProcessCmdKey(ref m, k);
        }
    }
}
