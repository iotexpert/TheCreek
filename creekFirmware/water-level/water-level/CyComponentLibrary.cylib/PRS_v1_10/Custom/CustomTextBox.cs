/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



namespace CyCustomizer.PRS_v1_10
{
    public class CyTextBox : System.Windows.Forms.TextBox
    {
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message m, System.Windows.Forms.Keys k)
        {
            if (m.Msg == 256 && k == System.Windows.Forms.Keys.Enter)
            {
                System.Windows.Forms.SendKeys.Send("\t");
                return true;
            }
            return base.ProcessCmdKey(ref m, k);
        }
    }
}
