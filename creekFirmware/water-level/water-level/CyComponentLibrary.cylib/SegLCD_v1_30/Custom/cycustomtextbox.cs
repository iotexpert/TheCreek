// ========================================
//
// Copyright Cypress Semiconductor Corporation, 2009
// All Rights Reserved
// UNPUBLISHED, LICENSED SOFTWARE.
//
// CONFIDENTIAL AND PROPRIETARY INFORMATION
// WHICH IS THE PROPERTY OF CYPRESS.
//
// Use of this file is governed
// by the license agreement included in the file
//
//     <install>/license/license.txt
//
// where <install> is the Cypress software
// installation root directory path.
//
// ========================================

using System;
using System.Collections.Generic;
using System.Text;

namespace SegLCD_v1_30
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
