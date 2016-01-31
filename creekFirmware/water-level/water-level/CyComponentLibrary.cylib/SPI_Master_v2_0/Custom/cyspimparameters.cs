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
using System.Windows.Forms;
using System.ComponentModel;

namespace SPI_Master_v2_0
{
    #region Component Parameter Names
    public class CyParamNames
    {
        // Basic tab parameners
        public const string MODE = "Mode";
        public const string BIDIRECT_MODE = "BidirectMode"; 
        public const string NUMBER_OF_DATA_BITS = "NumberOfDataBits";
        public const string SHIFT_DIR = "ShiftDir";
        public const string DESIRED_BIT_RATE = "DesiredBitRate";

        // Advanced tab parameters
        public const string CLOCK_INTERNAL = "ClockInternal";
        public const string INTERRUPT_ON_SPI_DONE = "InterruptOnSPIDone";
        public const string INTERRUPT_ON_TX_EMPTY = "InterruptOnTXEmpty";
        public const string INTERRUPT_ON_TX_NOT_FULL = "InterruptOnTXNotFull";
        public const string INTERRUPT_ON_RX_FULL = "InterruptOnRXFull";
        public const string INTERRUPT_ON_RX_NOT_EMPTY = "InterruptOnRXNotEmpty";
        public const string INTERRUPT_ON_RX_OVERRUN = "InterruptOnRXOverrun";
        public const string INTERRUPT_ON_BYTE_COMPLETE = "InterruptOnByteComplete";
        public const string RX_BUFFER_SIZE = "RxBufferSize";
        public const string TX_BUFFER_SIZE = "TxBufferSize";
        public const string USE_INTERNAL_INTERRUPT = "UseInternalInterrupt";
        public const string USE_TX_INTERNAL_INTERRUPT = "UseTxInternalInterrupt";
        public const string USE_RX_INTERNAL_INTERRUPT = "UseRxInternalInterrupt";
    }
    #endregion

    #region Component Parameters Range
    public class CyParamRange
    {
        // Basic Tab Constants
        public const byte NUM_BITS_MIN = 2;
        public const byte NUM_BITS_MAX = 16;
        public const int FREQUENCY_MIN = 1;
        public const int FREQUENCY_MAX = 33000000;

        // Advanced Tab Constants
        public const byte BUFFER_SIZE_MIN = 4;
        public const byte BUFFER_SIZE_MAX = 255;
    }
    #endregion

    #region Component types
    public enum E_SPI_MODES { Mode_00 = 1, Mode_01 = 2, Mode_10 = 3, Mode_11 = 4 };
    public enum E_B_SPI_MASTER_SHIFT_DIRECTION
    {
        [Description("MSB First")]
        MSB_First = 0,
        [Description("LSB First")]
        LSB_First = 1
    };
    #endregion

    public class CySPIMParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CySPIMControl m_basicTab;
        public CySPIMControlAdv m_advTab;

        /// <summary>
        /// During first getting of parameters this variable is false, what means that assigning
        /// values to form controls will not immediatly overwrite parameters with the same values.
        /// </summary>
        public bool m_bGlobalEditMode = false;

        public List<string> m_modeList;
        public List<string> m_shiftDirectionList;

        #region Component Parameters
        // Basic tab parameters
        private E_SPI_MODES m_mode;
        private bool m_bidirectMode;
        private byte? m_numberOfDataBits;
        private E_B_SPI_MASTER_SHIFT_DIRECTION m_shiftDir;
        private decimal? m_desiredBitRate;

        // Advanced tab parameters
        private bool m_clockInternal;
        private bool m_interruptOnTXFull;
        private bool m_interruptOnTXNotFull;
        private bool m_interruptOnSPIDone;
        private bool m_interruptOnRXOverrun;
        private bool m_interruptOnRXEmpty;
        private bool m_interruptOnRXNotEmpty;
        private bool m_interruptOnByteComplete;
        private uint? m_rxBufferSize;
        private uint? m_txBufferSize;
        private bool m_useTxInternalInterrupt;
        private bool m_useRxInternalInterrupt;
        public bool m_useInternalInterrupt = false;

        #endregion

        #region Constructor(s)
        public CySPIMParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
            m_modeList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.MODE));
            m_shiftDirectionList = new List<string>(inst.GetPossibleEnumValues(CyParamNames.SHIFT_DIR));
        }
        #endregion

        #region Basic Tab Properties
        public E_SPI_MODES Mode
        {
            get { return m_mode; }
            set
            {
                if (value == E_SPI_MODES.Mode_00 || value == E_SPI_MODES.Mode_01
                    || value == E_SPI_MODES.Mode_10 || value == E_SPI_MODES.Mode_11)
                { m_mode = value; }
            }
        }

        public bool BidirectMode
        {
            get { return m_bidirectMode; }
            set { m_bidirectMode = value; }
        }

        public byte? NumberOfDataBits
        {
            get { return m_numberOfDataBits; }
            set { m_numberOfDataBits = value; }
        }

        public E_B_SPI_MASTER_SHIFT_DIRECTION ShiftDir
        {
            get { return m_shiftDir; }
            set
            {
                if (value == E_B_SPI_MASTER_SHIFT_DIRECTION.LSB_First
                    || value == E_B_SPI_MASTER_SHIFT_DIRECTION.MSB_First)
                { m_shiftDir = value; }
            }
        }

        public decimal? DesiredBitRate
        {
            get { return m_desiredBitRate; }
            set { m_desiredBitRate = value; }
        }
        #endregion

        #region Advanced Tab Properties
        public bool ClockInternal
        {
            get { return m_clockInternal; }
            set { m_clockInternal = value; }
        }

        public bool InterruptOnRXFull
        {
            get { return m_interruptOnTXFull; }
            set { m_interruptOnTXFull = value; }
        }

        public bool InterruptOnTXNotFull
        {
            get { return m_interruptOnTXNotFull; }
            set { m_interruptOnTXNotFull = value; }
        }

        public bool InterruptOnSPIDone
        {
            get { return m_interruptOnSPIDone; }
            set { m_interruptOnSPIDone = value; }
        }

        public bool InterruptOnRXOverrun
        {
            get { return m_interruptOnRXOverrun; }
            set { m_interruptOnRXOverrun = value; }
        }

        public bool InterruptOnTXEmpty
        {
            get { return m_interruptOnRXEmpty; }
            set { m_interruptOnRXEmpty = value; }
        }

        public bool InterruptOnRXNotEmpty
        {
            get { return m_interruptOnRXNotEmpty; }
            set { m_interruptOnRXNotEmpty = value; }
        }

        public bool InterruptOnByteComplete
        {
            get { return m_interruptOnByteComplete; }
            set { m_interruptOnByteComplete = value; }
        }

        public uint? RxBufferSize
        {
            get { return m_rxBufferSize.Value; }
            set { m_rxBufferSize = value; }
        }

        public uint? TxBufferSize
        {
            get { return m_txBufferSize.Value; }
            set { m_txBufferSize = value; }
        }

        public bool UseTxInternalInterrupt
        {
            get { return m_useTxInternalInterrupt; }
            set { m_useTxInternalInterrupt = value; }
        }

        public bool UseRxInternalInterrupt
        {
            get { return m_useRxInternalInterrupt; }
            set { m_useRxInternalInterrupt = value; }
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst)
        {
            m_bGlobalEditMode = false;
            // Basic Parameters
            // Mode
            if (!string.IsNullOrEmpty(inst.GetCommittedParam(CyParamNames.MODE).Value))
            {
                this.Mode = (E_SPI_MODES)Convert.ToByte(inst.GetCommittedParam(CyParamNames.MODE).Value);
            }
            // BidirectMode
            try
            {
                this.BidirectMode = Convert.ToBoolean(inst.GetCommittedParam(CyParamNames.BIDIRECT_MODE).Value);
            }
            catch (Exception) { }
            // NumberOfDataBits
            try
            {
                this.NumberOfDataBits = byte.Parse(inst.GetCommittedParam(CyParamNames.NUMBER_OF_DATA_BITS).Value);
            }
            catch (Exception) { }
            // ShiftDir
            if (!string.IsNullOrEmpty(inst.GetCommittedParam(CyParamNames.SHIFT_DIR).Value))
            {
                this.ShiftDir = (E_B_SPI_MASTER_SHIFT_DIRECTION)Convert.ToByte(
                    inst.GetCommittedParam(CyParamNames.SHIFT_DIR).Value);
            }
            // DesiredBitRate
            try
            {
                this.DesiredBitRate = decimal.Parse(inst.GetCommittedParam(CyParamNames.DESIRED_BIT_RATE).Value);
            }
            catch (Exception) { }

            // Advanced Parameters
            // ClockInternal
            try
            {
                this.ClockInternal = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.CLOCK_INTERNAL).Value);
            }
            catch (Exception) { }
            // InterruptOnTXFull
            try
            {
                this.InterruptOnRXFull = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_RX_FULL).Value);
            }
            catch (Exception) { }
            // InterruptOnTXNotFull
            try
            {
                this.InterruptOnTXNotFull = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_TX_NOT_FULL).Value);
            }
            catch (Exception) { }
            // InterruptOnSPIDone
            try
            {
                this.InterruptOnSPIDone = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_SPI_DONE).Value);
            }
            catch (Exception) { }
            // InterruptOnRXOverrun
            try
            {
                this.InterruptOnRXOverrun = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_RX_OVERRUN).Value);
            }
            catch (Exception) { }
            // InterruptOnRXEmpty
            try
            {
                this.InterruptOnTXEmpty = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_TX_EMPTY).Value);
            }
            catch (Exception) { }
            // InterruptOnRXNotEmpty
            try
            {
                this.InterruptOnRXNotEmpty = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_RX_NOT_EMPTY).Value);
            }
            catch (Exception) { }
            // InterruptOnByteComplete
            try
            {
                this.InterruptOnByteComplete = Convert.ToBoolean(inst.GetCommittedParam(
                CyParamNames.INTERRUPT_ON_BYTE_COMPLETE).Value);
            }
            catch (Exception) { }
            // RxBufferSize
            try
            {
                this.RxBufferSize = byte.Parse(inst.GetCommittedParam(CyParamNames.RX_BUFFER_SIZE).Value);
            }
            catch (Exception) { }
            // TxBufferSize
            try
            {
                this.TxBufferSize = byte.Parse(inst.GetCommittedParam(CyParamNames.TX_BUFFER_SIZE).Value);
            }
            catch (Exception) { }
            // UseInternalInterrupt
            // Backward compatibility. This parameter isn't in use anymore but have to be analyzed 
            // during update from versions lower than 2.0.
            try
            {
                m_useInternalInterrupt = Convert.ToBoolean(inst.GetCommittedParam(
                    CyParamNames.USE_INTERNAL_INTERRUPT).Value);
            }
            catch (Exception) { }
            if (m_useInternalInterrupt)
            {
                this.UseTxInternalInterrupt = m_useInternalInterrupt;
                this.UseRxInternalInterrupt = m_useInternalInterrupt;
            }
            else
            {
                // UseTXInternalInterrupt
                try
                {
                    this.UseTxInternalInterrupt = Convert.ToBoolean(inst.GetCommittedParam(
                        CyParamNames.USE_TX_INTERNAL_INTERRUPT).Value);
                }
                catch (Exception) { }

                // UseRXInternalInterrupt
                try
                {
                    this.UseRxInternalInterrupt = Convert.ToBoolean(inst.GetCommittedParam(
                    CyParamNames.USE_RX_INTERNAL_INTERRUPT).Value);
                }
                catch (Exception) { }
            }
            m_basicTab.GetParams();
            m_advTab.GetParams();
            m_bGlobalEditMode = true;
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_bGlobalEditMode)
            {
                string value = null;
                switch (paramName)
                {
                    // Basic tab parameters
                    case CyParamNames.MODE:
                        value = m_mode.ToString();
                        break;
                    case CyParamNames.BIDIRECT_MODE:
                        value = m_bidirectMode.ToString().ToLower();
                        break;
                    case CyParamNames.NUMBER_OF_DATA_BITS:
                        value = m_numberOfDataBits.ToString();
                        break;
                    case CyParamNames.SHIFT_DIR:
                        string str = CyEnumConverter.GetEnumDesc(m_shiftDir);
                        value = m_inst.ResolveEnumDisplayToId(paramName, str);
                        break;
                    case CyParamNames.DESIRED_BIT_RATE:
                        value = m_desiredBitRate.ToString();
                        break;
                    // Advanced tab parameters
                    case CyParamNames.CLOCK_INTERNAL:
                        value = m_clockInternal.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_RX_FULL:
                        value = m_interruptOnTXFull.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_TX_NOT_FULL:
                        value = m_interruptOnTXNotFull.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_SPI_DONE:
                        value = m_interruptOnSPIDone.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_RX_OVERRUN:
                        value = m_interruptOnRXOverrun.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_TX_EMPTY:
                        value = m_interruptOnRXEmpty.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_RX_NOT_EMPTY:
                        value = m_interruptOnRXNotEmpty.ToString().ToLower();
                        break;
                    case CyParamNames.INTERRUPT_ON_BYTE_COMPLETE:
                        value = m_interruptOnByteComplete.ToString().ToLower();
                        break;
                    case CyParamNames.RX_BUFFER_SIZE:
                        value = m_rxBufferSize.ToString();
                        break;
                    case CyParamNames.TX_BUFFER_SIZE:
                        value = m_txBufferSize.ToString();
                        break;
                    case CyParamNames.USE_INTERNAL_INTERRUPT:
                        value = false.ToString().ToLower();
                        break;
                    case CyParamNames.USE_TX_INTERNAL_INTERRUPT:
                        value = m_useTxInternalInterrupt.ToString().ToLower();
                        break;
                    case CyParamNames.USE_RX_INTERNAL_INTERRUPT:
                        value = m_useRxInternalInterrupt.ToString().ToLower();
                        break;
                    default:
                        return;
                }
                m_inst.SetParamExpr(paramName, value);
                m_inst.CommitParamExprs();
            }
        }
        #endregion
    }
}
