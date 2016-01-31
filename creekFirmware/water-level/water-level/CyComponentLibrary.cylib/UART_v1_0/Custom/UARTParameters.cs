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

namespace CyCustomizer.UART_v1_0
{
    class UARTParameters
    {
        public CyCompDevParam Address1 = null;
        public CyCompDevParam Address2 = null;
        public CyCompDevParam BaudRate = null;
        public CyCompDevParam EnIntRXInterrupt = null;
        public CyCompDevParam EnIntTXInterrupt = null;
        public CyCompDevParam FlowControl = null;
        public CyCompDevParam HwTXEnSignal = null;
        public CyCompDevParam InternalClock = null;
        public CyCompDevParam InterruptOnTXComplete = null;
        public CyCompDevParam InterruptOnTXFifoEmpty = null;
        public CyCompDevParam InterruptOnTXFifoFull = null;
        public CyCompDevParam InterruptOnTXFifoNotFull = null;
        public CyCompDevParam IntOnAddressDetect = null;
        public CyCompDevParam IntOnAddressMatch = null;
        public CyCompDevParam IntOnBreak = null;
        public CyCompDevParam IntOnByteRcvd = null;
        public CyCompDevParam IntOnOverrunError = null;
        public CyCompDevParam IntOnParityError = null;
        public CyCompDevParam IntOnStopError = null;
        public CyCompDevParam NumDataBits = null;
        public CyCompDevParam NumStopBits = null;
        public CyCompDevParam ParityType = null;
        public CyCompDevParam RXAddressMode = null;
        public CyCompDevParam RXBufferSize = null;
        public CyCompDevParam RXEnable = null;
        public CyCompDevParam TXBufferSize = null;
        public CyCompDevParam TXEnable = null;

        public UARTParameters(ICyInstEdit_v1 inst)
        {
            GetParams(inst);
        }

        private void GetParams(ICyInstEdit_v1 inst)
        {
            Address1 = inst.GetCommittedParam("Address1");
            Address2 = inst.GetCommittedParam("Address2");
            BaudRate = inst.GetCommittedParam("BaudRate");
            EnIntRXInterrupt = inst.GetCommittedParam("EnIntRXInterrupt");
            EnIntTXInterrupt = inst.GetCommittedParam("EnIntTXInterrupt");
            FlowControl = inst.GetCommittedParam("FlowControl");
            HwTXEnSignal = inst.GetCommittedParam("HwTXEnSignal");
            InternalClock = inst.GetCommittedParam("InternalClock");
            InterruptOnTXComplete = inst.GetCommittedParam("InterruptOnTXComplete");
            InterruptOnTXFifoEmpty = inst.GetCommittedParam("InterruptOnTXFifoEmpty");
            InterruptOnTXFifoFull = inst.GetCommittedParam("InterruptOnTXFifoFull");
            InterruptOnTXFifoNotFull = inst.GetCommittedParam("InterruptOnTXFifoNotFull");
            IntOnAddressDetect = inst.GetCommittedParam("IntOnAddressDetect");
            IntOnAddressMatch = inst.GetCommittedParam("IntOnAddressMatch");
            IntOnBreak = inst.GetCommittedParam("IntOnBreak");
            IntOnByteRcvd = inst.GetCommittedParam("IntOnByteRcvd");
            IntOnOverrunError = inst.GetCommittedParam("IntOnOverrunError");
            IntOnParityError = inst.GetCommittedParam("IntOnParityError");
            IntOnStopError = inst.GetCommittedParam("IntOnStopError");
            NumDataBits = inst.GetCommittedParam("NumDataBits");
            NumStopBits = inst.GetCommittedParam("NumStopBits");
            ParityType = inst.GetCommittedParam("ParityType");
            RXAddressMode = inst.GetCommittedParam("RXAddressMode");
            RXBufferSize = inst.GetCommittedParam("RXBufferSize");
            RXEnable = inst.GetCommittedParam("RXEnable");
            TXBufferSize = inst.GetCommittedParam("TXBufferSize");
            TXEnable = inst.GetCommittedParam("TXEnable");
        }

        private void SetParam(ICyInstEdit_v1 inst, string ParamName, string value)
        {

        }

        private void CommitParams(ICyInstEdit_v1 inst)
        {
            inst.CommitParamExprs();
        }
    }
}
