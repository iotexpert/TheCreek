/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;

namespace PGA_Inv_v1_60
{
    public class Pga_InvParameters
    {
	
	    
		public const string INV_PGA_GAIN = "Inverting_Gain";
		public const string PGA_POWER = "Power";
		
        public CyCompDevParam Inv_Pga_Gain = null;
        public CyCompDevParam Pga_Power = null;

        public Pga_InvParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Inv_Pga_Gain = inst.GetCommittedParam(INV_PGA_GAIN);
            Pga_Power = inst.GetCommittedParam(PGA_POWER);
        }
        
        
    }
}


