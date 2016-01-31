/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace I2S_v1_0
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
        ICyInstEdit_v1 m_inst;
        public cyi2sbasictab m_basicParams;
        public cyi2sadvancedtab m_advancedParams;

        [NonSerialized()]
        public bool m_bGlobalEditMode = false;

        #region Component Parameters

        // Basic tab parameters
        public E_DIRECTION m_direction;
        public int m_dataBits;
        public int m_wordSelectPeriod;

        // Advanced tab parameters
        public E_DATA_INTERLEAVING m_rxDataInterleaving;
        public E_DATA_INTERLEAVING m_txDataInterleaving;
        public E_DMA_PRESENT m_rxDMA;
        public E_DMA_PRESENT m_txDMA;
        public byte m_interruptSource;
        public bool[] m_ISReg = new bool[8] { false, false, false, false, false, false, false, false };

        #endregion

        public CyI2SParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }

        #region Getting parameters

        public void GetParams(ICyInstEdit_v1 inst)
        {
            // Basic tab parameters
            // Direction
            m_direction = (E_DIRECTION)Convert.ToByte(inst.GetCommittedParam(CyParamNames.DIRECTION).Value);

            // DataBits
            if (DataBitsValidated(inst.GetCommittedParam(CyParamNames.DATA_BITS).Value))
            {
                m_dataBits = int.Parse(inst.GetCommittedParam(CyParamNames.DATA_BITS).Value);
            }

            // Word Select Period
            if (WordSelectPeriodValidated(inst.GetCommittedParam(CyParamNames.WORD_SELECT).Value))
            {
                m_wordSelectPeriod = int.Parse(inst.GetCommittedParam(CyParamNames.WORD_SELECT).Value);
            }
            m_basicParams.GetParams();

            // Advanced tab parameters
            // Data Interleaving GroupBoxes
            m_rxDataInterleaving = (E_DATA_INTERLEAVING)Convert.ToByte(
                inst.GetCommittedParam(CyParamNames.RX_DATA_INTERLEAVING).Value);
            m_txDataInterleaving = (E_DATA_INTERLEAVING)Convert.ToByte(
                inst.GetCommittedParam(CyParamNames.TX_DATA_INTERLEAVING).Value);
            
            // DMA Request
            m_rxDMA = (E_DMA_PRESENT)Convert.ToByte(inst.GetCommittedParam(CyParamNames.RX_DMA_PRESENT).Value);
            m_txDMA = (E_DMA_PRESENT)Convert.ToByte(inst.GetCommittedParam(CyParamNames.TX_DMA_PRESENT).Value);
            
            // Interrupt Source
            if (InterruptSourceValidated(inst.GetCommittedParam(CyParamNames.INTERRUPT_SOURCE).Value))
            {
                for (int i = 0; i < m_ISReg.Length; i++)
                { m_ISReg[i] = false; }
                m_interruptSource = byte.Parse(inst.GetCommittedParam(CyParamNames.INTERRUPT_SOURCE).Expr.ToString());
                for (int i = 0; i < 8; i++)
                {
                    if ((m_interruptSource & 0x01) == 1) m_ISReg[i] = true;
                    m_interruptSource >>= 1;
                }
            }
            m_advancedParams.GetParams();
        }

        #endregion

        #region Setting parameters

        public void SetParams(string paramName)
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
            CommitParams(m_inst);
        }

        public void CommitParams(ICyInstEdit_v1 inst)
        {
            if (!inst.CommitParamExprs())
            {
                MessageBox.Show("Error in Committing Parameters");
            }
        }

        #endregion

        #region Basic parameters validators

        public bool DataBitsValidated(string value)
        {
            if (value != "" && value != null)
            {
                int var = int.Parse(value);
                if (var >= 8 && var <= 32)
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        public bool WordSelectPeriodValidated(string value)
        {
            if (value != "" && value != null)
            {
                if (value == "16" || value == "32" || value == "48" || value == "64")
                    return true;
                else
                    return false;
            }
            else
            { return false; }
        }

        #endregion

        #region Advanced parameters validators

        public bool InterruptSourceValidated(string value)
        {
            if (value != "" && value != null)
            {
                int numValue = int.Parse(value);
                if (numValue < 0 || numValue > 63)
                    return false;
                else
                    return true;
            }
            else
            { return false; }
        }

        #endregion
    }
}
