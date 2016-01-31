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
using System.Windows.Forms;

namespace CyCustomizer.UART_v1_20
{
    public class UARTParameters
    {
        private ICyInstEdit_v1 m_Instance;

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
        public CyCompDevParam TXBufferSize = null;
        public CyCompDevParam PSOC32bit = null;

        private int m_exClockFrequency;
        public int ExternalClockFrequency
        {
            get { return m_exClockFrequency; }
            set
            {
                m_exClockFrequency = value;
                m_Instance.SetParamExpr("ExClockFrequency", m_exClockFrequency.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private int m_TXBufferSizeMax;
        public int TXBufferSizeMax
        {
            get { return m_TXBufferSizeMax; }
        }

        private int m_RXBufferSizeMax;
        public int RXBufferSizeMax
        {
            get { return m_RXBufferSizeMax; }
        }

        private bool m_Use23Polling;
        public bool Use23Polling
        {
            get { return m_Use23Polling; }
            set
            {
                m_Use23Polling = value;
                m_Instance.SetParamExpr("Use23Polling", m_Use23Polling.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private bool m_BreakDetect;
        public bool BreakDetect
        {
            get { return m_BreakDetect; }
            set
            {
                m_BreakDetect = value;
                m_Instance.SetParamExpr("BreakDetect", m_BreakDetect.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private bool m_CRCoutputsEn;
        public bool CRCoutputsEn
        {
            get { return m_CRCoutputsEn; }
            set
            {
                m_CRCoutputsEn = value;
                m_Instance.SetParamExpr("CRCoutputsEn", m_CRCoutputsEn.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private bool m_rxEnable;
        public bool RXEnable
        {
            get { return m_rxEnable; }
            set
            {
                m_rxEnable = value;
                m_Instance.SetParamExpr("RXEnable", m_rxEnable.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private bool m_txEnable;
        public bool TXEnable
        {
            get { return m_txEnable; }
            set
            {
                m_txEnable = value;
                m_Instance.SetParamExpr("TXEnable", m_txEnable.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        private bool m_halfDuplexEnable;
        public bool HalfDuplexEnable
        {
            get { return m_halfDuplexEnable; }
            set
            {
                m_halfDuplexEnable = value;
                m_Instance.SetParamExpr("HalfDuplexEn", m_halfDuplexEnable.ToString());
                m_Instance.CommitParamExprs();
            }
        }

        public UARTParameters(ICyInstEdit_v1 inst)
        {
            m_Instance = inst;
            SetBuffersSizeMax(inst);
            GetParams(inst);
        }

        private void SetBuffersSizeMax(ICyInstEdit_v1 inst)
        {
            string deviceName = inst.CurrentDevice;
            string deviceFamily = string.Empty;

            if (deviceName.Length >= 5)
            {
                deviceFamily = new string(deviceName[4], 1);
            }
            else
            {
                deviceFamily = "0";
            }

            switch (deviceFamily)
            {
                case "0":
                    {
                        m_RXBufferSizeMax = 256;
                        m_TXBufferSizeMax = 256;
                        m_Instance.SetParamExpr("PSOC32bit", "false");
                        break;
                    }
                case "3":
                    {
                        m_RXBufferSizeMax = 256;
                        m_TXBufferSizeMax = 256;
                        m_Instance.SetParamExpr("PSOC32bit", "false");
                        break;
                    }
                case "5":
                    {
                        m_RXBufferSizeMax = 65536;
                        m_TXBufferSizeMax = 65536;
                        m_Instance.SetParamExpr("PSOC32bit", "true");
                        break;
                    }
            }
            

			m_Instance.CommitParamExprs();
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
            TXBufferSize = inst.GetCommittedParam("TXBufferSize");
            PSOC32bit = inst.GetCommittedParam("PSOC32bit");

            m_halfDuplexEnable = bool.Parse(inst.GetCommittedParam("HalfDuplexEn").Value);
            m_txEnable = bool.Parse(inst.GetCommittedParam("TXEnable").Value);
            m_rxEnable = bool.Parse(inst.GetCommittedParam("RXEnable").Value);
            m_BreakDetect = bool.Parse(inst.GetCommittedParam("BreakDetect").Value);
            m_Use23Polling = bool.Parse(inst.GetCommittedParam("Use23Polling").Value);
            m_CRCoutputsEn = bool.Parse(inst.GetCommittedParam("CRCoutputsEn").Value);
            m_exClockFrequency = int.Parse(inst.GetCommittedParam("ExClockFrequency").Value);
        }

        private void CommitParams(ICyInstEdit_v1 inst)
        {
            inst.CommitParamExprs();
        }
    }
}
