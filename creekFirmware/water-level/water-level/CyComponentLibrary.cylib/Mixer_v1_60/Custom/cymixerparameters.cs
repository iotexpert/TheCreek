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
using CyDesigner.Extensions.Common;
using Mixer_v1_60;

namespace Mixer_v1_60
{
    public class cymixerparameters
    {
        #region Param Name Constants
        public const string POWER = "Power";
        public const string MIXER_TYPE = "Mixer_Type";
        public const string LO_SOURCE = "LO_Source";
        public const string SIGNAL_FREQUENCY = "Signal_Frequency";
        public const string INTERNAL_LO = "Internal";
        public const string EXTERNAL_LO = "External";
        public const string LO_CLOCK_FREQ = "LO_clock_freq";
        public const int MIXERTYPE_UP = 0;
        public const int MIXERTYPE_DOWN = 1;
        public const double LO_LIMIT_UPMIXER = 1000000.00;
        public const double LO_LIMIT_DOWNMIXER = 4000000.00;
        #endregion

        public CyCompDevParam Power = null;
        public CyCompDevParam Mixer_Type = null;
        public CyCompDevParam LO_Source = null;
        public CyCompDevParam Signal_Frequency = null;
        public CyCompDevParam LO_clock_freq = null;

        public cymixerparameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        // Get parameter values
        private void GetParams(ICyInstEdit_v1 inst)
        {
            Power = inst.GetCommittedParam(POWER);
            Mixer_Type = inst.GetCommittedParam(MIXER_TYPE);
            LO_Source = inst.GetCommittedParam(LO_SOURCE);
            Signal_Frequency = inst.GetCommittedParam(SIGNAL_FREQUENCY);
            LO_clock_freq = inst.GetCommittedParam(LO_CLOCK_FREQ);
        }
    }
}
