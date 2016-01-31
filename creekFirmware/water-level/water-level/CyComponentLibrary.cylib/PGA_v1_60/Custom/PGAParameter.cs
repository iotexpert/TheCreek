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

namespace PGA_v1_60
{
    public class PgaParameters
    {
	
	    
		public const string PGA_GAIN = "Gain";
		public const string PGA_POWER = "Power";
		public const string PGA_VREF_INPUT = "Vref_Input";
		
        public CyCompDevParam Pga_Gain = null;
        public CyCompDevParam Pga_Power = null;
        public CyCompDevParam Pga_Vref_Input = null;
   
        public PgaParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Pga_Gain = inst.GetCommittedParam(PGA_GAIN);
            Pga_Power = inst.GetCommittedParam(PGA_POWER);
            Pga_Vref_Input = inst.GetCommittedParam(PGA_VREF_INPUT);
         }
        
        
    }
}


