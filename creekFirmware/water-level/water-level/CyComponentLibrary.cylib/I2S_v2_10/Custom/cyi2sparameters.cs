/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;
using System.Drawing;

namespace I2S_v2_10
{
    #region Component Parameter Names
    public class CyParamNames
    {
        // Basic tab parameners
        public const string DIRECTION = "Direction";
        public const string DATA_BITS = "DataBits";
        public const string WORD_SELECT = "WordSelect";

        // Advanced tab parameters
        public const string RX_DATA_INTERLEAVING = "RxDataInterleaving";
        public const string TX_DATA_INTERLEAVING = "TxDataInterleaving";
        public const string RX_DMA_PRESENT = "RxDMA_present";
        public const string TX_DMA_PRESENT = "TxDMA_present";
        public const string INTERRUPT_SOURCE = "InterruptSource";
    }
    #endregion

    #region Component Enums
    public enum E_DIRECTION { Rx = 1, Tx = 2, Rx_and_Tx = 3 }
    public enum E_DMA_PRESENT { DMA = 1, NO_DMA = 2 }
    public enum E_DATA_INTERLEAVING { Separate, Interleaved }
    #endregion

    public class CyI2SParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CyI2SBasic m_basicParams;
        public CyI2SAdvanced m_advancedParams;

        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        public bool m_bGlobalEditMode = false;
        // Array to keep Interrupt Source CheckBoxes state
        public bool[] m_isReg = new bool[8] { false, false, false, false, false, false, false, false };

        #region Component Parameters
        // Basic tab parameters
        private E_DIRECTION m_direction;
        private int m_dataBits;
        private int m_wordSelectPeriod;

        // Advanced tab parameters
        private E_DATA_INTERLEAVING m_rxDataInterleaving;
        private E_DATA_INTERLEAVING m_txDataInterleaving;
        private E_DMA_PRESENT m_rxDMA;
        private E_DMA_PRESENT m_txDMA;
        private byte m_interruptSource;
        #endregion

        #region Constructor(s)
        public CyI2SParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }
        #endregion

        #region Class Properties
        // Basic tab parameters
        public E_DIRECTION Direction
        {
            get
            { return m_direction; }
            set
            { m_direction = value; }
        }

        public int DataBits
        {
            get
            { return m_dataBits; }
            set
            {
                if ((value >= 8) && (value <= 32))
                { m_dataBits = value; }
            }
        }

        public int WordSelectPeriod
        {
            get
            { return m_wordSelectPeriod; }
            set
            {
                if (value == 16 || value == 32 || value == 48 || value == 64)
                { m_wordSelectPeriod = value; }
            }
        }

        // Advanced tab parameters
        public E_DATA_INTERLEAVING RxDataInterleaving
        {
            get
            { return m_rxDataInterleaving; }
            set
            { m_rxDataInterleaving = value; }
        }

        public E_DATA_INTERLEAVING TxDataInterleaving
        {
            get
            { return m_txDataInterleaving; }
            set
            { m_txDataInterleaving = value; }
        }

        public E_DMA_PRESENT RxDMA
        {
            get
            { return m_rxDMA; }
            set
            { m_rxDMA = value; }
        }

        public E_DMA_PRESENT TxDMA
        {
            get
            { return m_txDMA; }
            set
            { m_txDMA = value; }
        }

        public byte InterruptSource
        {
            get
            { return m_interruptSource; }
            set
            {
                if ((value >= 0) && (value <= 63))
                { m_interruptSource = value; }
            }
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst)
        {
            // Basic tab parameters
            this.Direction = (E_DIRECTION)Convert.ToByte(inst.GetCommittedParam(CyParamNames.DIRECTION).Value);
            int previousDataBits = 0;
            try
            {
                previousDataBits = this.DataBits;
                this.DataBits = int.Parse(inst.GetCommittedParam(CyParamNames.DATA_BITS).Value);
            }
            catch (Exception)
            { this.DataBits = previousDataBits; }
            int previousWordSelectPeriod = 0;
            try
            {
                previousWordSelectPeriod = this.WordSelectPeriod;
                this.WordSelectPeriod = int.Parse(inst.GetCommittedParam(CyParamNames.WORD_SELECT).Value);
            }
            catch (Exception)
            { this.WordSelectPeriod = previousWordSelectPeriod; }
            m_basicParams.GetParams();

            // Advanced tab parameters
            this.RxDataInterleaving = (E_DATA_INTERLEAVING)Convert.ToByte(inst.GetCommittedParam(
                CyParamNames.RX_DATA_INTERLEAVING).Value);
            this.TxDataInterleaving = (E_DATA_INTERLEAVING)Convert.ToByte(inst.GetCommittedParam(
                CyParamNames.TX_DATA_INTERLEAVING).Value);
            this.RxDMA = (E_DMA_PRESENT)Convert.ToByte(inst.GetCommittedParam(CyParamNames.RX_DMA_PRESENT).Value);
            this.TxDMA = (E_DMA_PRESENT)Convert.ToByte(inst.GetCommittedParam(CyParamNames.TX_DMA_PRESENT).Value);
            // Initializing InterruptSource
            for (int i = 0; i < m_isReg.Length; i++)
            { m_isReg[i] = false; }
            byte previousInterruptSource = 0;
            try
            {
                previousInterruptSource = this.InterruptSource;
                this.InterruptSource = byte.Parse(inst.GetCommittedParam(
                    CyParamNames.INTERRUPT_SOURCE).Expr.ToString());
            }
            catch (Exception)
            {
                this.InterruptSource = previousInterruptSource;
            }
            // Splitting byte value to bits and saving to m_ISReg
            for (int i = 0; i < 8; i++)
            {
                if ((this.InterruptSource & 0x01) == 1) m_isReg[i] = true;
                this.InterruptSource >>= 1;
            }
            if (m_isReg[5]) this.RxDataInterleaving = E_DATA_INTERLEAVING.Separate;
            if (m_isReg[2]) this.TxDataInterleaving = E_DATA_INTERLEAVING.Separate;

            m_advancedParams.GetParams();
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_bGlobalEditMode)
            {
                object value = null;
                switch (paramName)
                {
                    // Basic tab parameters
                    case CyParamNames.DIRECTION:
                        value = m_direction;
                        break;
                    case CyParamNames.DATA_BITS:
                        value = m_dataBits;
                        break;
                    case CyParamNames.WORD_SELECT:
                        value = m_wordSelectPeriod;
                        break;
                    // Advanced tab parameters
                    case CyParamNames.RX_DATA_INTERLEAVING:
                        value = m_rxDataInterleaving;
                        break;
                    case CyParamNames.TX_DATA_INTERLEAVING:
                        value = m_txDataInterleaving;
                        break;
                    case CyParamNames.RX_DMA_PRESENT:
                        value = m_rxDMA;
                        break;
                    case CyParamNames.TX_DMA_PRESENT:
                        value = m_txDMA;
                        break;
                    case CyParamNames.INTERRUPT_SOURCE:
                        value = m_interruptSource;
                        break;
                    default:
                        return;
                }
                m_inst.SetParamExpr(paramName, value.ToString());
                m_inst.CommitParamExprs();
            }
        }
        #endregion
    }
}
