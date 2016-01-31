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

namespace SPI_Master_v1_20
{
    public class SPIMParameters
    {
        public CyCompDevParam ClockInternal = null;
        public CyCompDevParam DesiredBitRate = null;
        public CyCompDevParam InterruptOnTXFull = null;
        public CyCompDevParam InterruptOnTXNotFull = null;
        public CyCompDevParam InterruptOnSPIDone = null;
        public CyCompDevParam InterruptOnRXOverrun = null;
        public CyCompDevParam InterruptOnRXEmpty = null;
        public CyCompDevParam InterruptOnRXNotEmpty = null;
        public CyCompDevParam InterruptOnByteComplete = null;
        public CyCompDevParam Mode = null;
        public CyCompDevParam NumberOfDataBits = null;
        public CyCompDevParam RxBufferSize = null;
        public CyCompDevParam ShiftDir = null;
        public CyCompDevParam TxBufferSize = null;
        public CyCompDevParam UseInternalInterrupt = null;
        public CySPIMControl generalParams;

        public SPIMParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            
            ClockInternal = inst.GetCommittedParam("ClockInternal");
            DesiredBitRate = inst.GetCommittedParam("DesiredBitRate");
            InterruptOnTXFull = inst.GetCommittedParam("InterruptOnTXFull");
            InterruptOnTXNotFull = inst.GetCommittedParam("InterruptOnTXNotFull");
            InterruptOnSPIDone = inst.GetCommittedParam("InterruptOnSPIDone");
            InterruptOnRXOverrun = inst.GetCommittedParam("InterruptOnRXOverrun");
            InterruptOnRXEmpty = inst.GetCommittedParam("InterruptOnRXEmpty");
            InterruptOnRXNotEmpty = inst.GetCommittedParam("InterruptOnRXNotEmpty");
            InterruptOnByteComplete = inst.GetCommittedParam("InterruptOnByteComplete");
            Mode = inst.GetCommittedParam("Mode");
            NumberOfDataBits = inst.GetCommittedParam("NumberOfDataBits");
            RxBufferSize = inst.GetCommittedParam("RxBufferSize");
            ShiftDir = inst.GetCommittedParam("ShiftDir");
            TxBufferSize = inst.GetCommittedParam("TxBufferSize");
            UseInternalInterrupt = inst.GetCommittedParam("UseInternalInterrupt");

        }
    }
}
