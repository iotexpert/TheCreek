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

namespace GraphicLCDCtrl_v1_50
{
    #region Symbol parameter names
    public class CyParamNames
    {
        public const string SYNC_PULSE_POLARITY_LOW = "SyncPulsePolarityLow";
        public const string TRANSITION_DOTCLK_FALLING = "TransitionDotclkFalling";
        public const string HORIZ_SYNC_WIDTH = "HorizSyncWidth";
        public const string HORIZ_BACK_PORCH = "HorizBackPorch";
        public const string HORIZ_ACTIVE_REGION = "HorizActiveRegion";
        public const string HORIZ_FRONT_PORCH = "HorizFrontPorch";
        public const string VERT_SYNC_WIDTH = "VertSyncWidth";
        public const string VERT_BACK_PORCH = "VertBackPorch";
        public const string VERT_ACTIVE_REGION = "VertActiveRegion";
        public const string VERT_FRONT_PORCH = "VertFrontPorch";
        public const string INTERRUPT_GEN = "InterruptGen";
    }
    #endregion

    #region Symbol parameters range
    public class CyParamRanges
    {
        // Horizontal timing constants
        public const UInt16 H_SYNC_WIDTH_MIN = 1;
        public const UInt16 H_SYNC_WIDTH_MAX = 256;
        public const UInt16 H_BACK_PORCH_MIN = 6;
        public const UInt16 H_BACK_PORCH_MAX = 256;
        public const UInt16 H_ACTIVE_REGION_MIN = 4;
        public const UInt16 H_ACTIVE_REGION_MAX = 1024;
        public const UInt16 H_FRONT_PORCH_MIN = 1;
        public const UInt16 H_FRONT_PORCH_MAX = 256;

        // Vertical timing constants
        public const UInt16 V_SYNC_WIDTH_MIN = 1;
        public const UInt16 V_SYNC_WIDTH_MAX = 256;
        public const UInt16 V_BACK_PORCH_MIN = 1;
        public const UInt16 V_BACK_PORCH_MAX = 256;
        public const UInt16 V_ACTIVE_REGION_MIN = 1;
        public const UInt16 V_ACTIVE_REGION_MAX = 1024;
        public const UInt16 V_FRONT_PORCH_MIN = 1;
        public const UInt16 V_FRONT_PORCH_MAX = 256;

        public const byte INTERRUPT_GEN_MAX = 2;
    }
    #endregion

    public class CyGraphicLCDCtrlParameters
    {
        public ICyInstEdit_v1 m_inst;
        public CyGraphicLCDCtrlBasic m_basicTab;
        /// <summary>
        /// During first getting of parameters this variable is false, what means that assigning
        /// values to form controls will not immediatly overwrite parameters with the same values
        /// </summary>
        public bool m_globalEditMode = false;

        #region Symbol parameters
        public bool m_syncPulsePolarityLow;
        public bool m_transitionDotclkFalling;
        public UInt16 m_hSyncWidth;
        public UInt16 m_hBackPorch;
        public UInt16 m_hActiveRegion;
        public UInt16 m_hFrontPorch;
        public UInt16 m_vSyncWidth;
        public UInt16 m_vBackPorch;
        public UInt16 m_vActiveRegion;
        public UInt16 m_vFrontPorch;
        public byte m_interruptGen;
        #endregion

        #region Constructor(s)
        public CyGraphicLCDCtrlParameters(ICyInstEdit_v1 inst)
        {
            this.m_inst = inst;
        }
        #endregion

        #region Getting parameters
        public void GetParams(ICyInstEdit_v1 inst, CyCompDevParam param)
        {
            if (param != null)
            {
                if (param.ErrorCount == 0)
                {
                    switch (param.Name)
                    {
                        // Sync Pulse Polarity
                        case CyParamNames.SYNC_PULSE_POLARITY_LOW:
                            GetSyncPulsePolarityLow();
                            break;
                        // Transition Dotclk Edge
                        case CyParamNames.TRANSITION_DOTCLK_FALLING:
                            GetTransitionDotclkFalling();
                            break;
                        // Horizontal Timing
                        case CyParamNames.HORIZ_SYNC_WIDTH:
                            GetHSyncWidth();
                            break;
                        case CyParamNames.HORIZ_ACTIVE_REGION:
                            GetHActiveRegion();
                            break;
                        case CyParamNames.HORIZ_BACK_PORCH:
                            GetHBackPorch();
                            break;
                        case CyParamNames.HORIZ_FRONT_PORCH:
                            GetHFrontPorch();
                            break;
                        // Vertical Timing
                        case CyParamNames.VERT_SYNC_WIDTH:
                            GetVSyncWidth();
                            break;
                        case CyParamNames.VERT_ACTIVE_REGION:
                            GetVActiveRegion();
                            break;
                        case CyParamNames.VERT_BACK_PORCH:
                            GetVBackPorch();
                            break;
                        case CyParamNames.VERT_FRONT_PORCH:
                            GetVFrontPorch();
                            break;
                        // Interrupt Generation
                        case CyParamNames.INTERRUPT_GEN:
                            GetInterruptGen();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                // Sync Pulse Polarity
                GetSyncPulsePolarityLow();
                // Transition Dotclk Edge
                GetTransitionDotclkFalling();
                // Horizontal Timing
                GetHSyncWidth();
                GetHActiveRegion();
                GetHBackPorch();
                GetHFrontPorch();
                // Vertical Timing
                GetVSyncWidth();
                GetVActiveRegion();
                GetVBackPorch();
                GetVFrontPorch();
                // Interrupt Generation
                GetInterruptGen();
            }

            m_basicTab.GetParams();
        }

        private void GetSyncPulsePolarityLow()
        {
            m_inst.GetCommittedParam(CyParamNames.SYNC_PULSE_POLARITY_LOW).TryGetValueAs<bool>(
                out m_syncPulsePolarityLow);
        }

        private void GetTransitionDotclkFalling()
        {
            m_inst.GetCommittedParam(CyParamNames.TRANSITION_DOTCLK_FALLING).TryGetValueAs<bool>(
                out m_transitionDotclkFalling);
        }

        private void GetHSyncWidth()
        {
            m_inst.GetCommittedParam(CyParamNames.HORIZ_SYNC_WIDTH).TryGetValueAs<UInt16>(out m_hSyncWidth);
        }

        private void GetHActiveRegion()
        {
            m_inst.GetCommittedParam(CyParamNames.HORIZ_ACTIVE_REGION).TryGetValueAs<UInt16>(out m_hActiveRegion);
        }

        private void GetHBackPorch()
        {
            m_inst.GetCommittedParam(CyParamNames.HORIZ_BACK_PORCH).TryGetValueAs<UInt16>(out m_hBackPorch);
        }

        private void GetHFrontPorch()
        {
            m_inst.GetCommittedParam(CyParamNames.HORIZ_FRONT_PORCH).TryGetValueAs<UInt16>(out m_hFrontPorch);
        }

        private void GetVSyncWidth()
        {
            m_inst.GetCommittedParam(CyParamNames.VERT_SYNC_WIDTH).TryGetValueAs<UInt16>(out m_vSyncWidth);
        }

        private void GetVActiveRegion()
        {
            m_inst.GetCommittedParam(CyParamNames.VERT_ACTIVE_REGION).TryGetValueAs<UInt16>(out m_vActiveRegion);
        }

        private void GetVBackPorch()
        {
            m_inst.GetCommittedParam(CyParamNames.VERT_BACK_PORCH).TryGetValueAs<UInt16>(out m_vBackPorch);
        }

        private void GetVFrontPorch()
        {
            m_inst.GetCommittedParam(CyParamNames.VERT_FRONT_PORCH).TryGetValueAs<UInt16>(out m_vFrontPorch);
        }

        private void GetInterruptGen()
        {
            m_inst.GetCommittedParam(CyParamNames.INTERRUPT_GEN).TryGetValueAs<byte>(out m_interruptGen);
        }
        #endregion

        #region Setting parameters
        public void SetParams(string paramName)
        {
            if (m_globalEditMode)
            {
                string value = null;
                switch (paramName)
                {
                    case CyParamNames.SYNC_PULSE_POLARITY_LOW:
                        value = m_syncPulsePolarityLow.ToString().ToLower();
                        break;
                    case CyParamNames.TRANSITION_DOTCLK_FALLING:
                        value = m_transitionDotclkFalling.ToString().ToLower();
                        break;
                    case CyParamNames.HORIZ_SYNC_WIDTH:
                        value = m_hSyncWidth.ToString();
                        break;
                    case CyParamNames.HORIZ_BACK_PORCH:
                        value = m_hBackPorch.ToString();
                        break;
                    case CyParamNames.HORIZ_ACTIVE_REGION:
                        value = m_hActiveRegion.ToString();
                        break;
                    case CyParamNames.HORIZ_FRONT_PORCH:
                        value = m_hFrontPorch.ToString();
                        break;
                    case CyParamNames.VERT_SYNC_WIDTH:
                        value = m_vSyncWidth.ToString();
                        break;
                    case CyParamNames.VERT_BACK_PORCH:
                        value = m_vBackPorch.ToString();
                        break;
                    case CyParamNames.VERT_ACTIVE_REGION:
                        value = m_vActiveRegion.ToString();
                        break;
                    case CyParamNames.VERT_FRONT_PORCH:
                        value = m_vFrontPorch.ToString();
                        break;
                    case CyParamNames.INTERRUPT_GEN:
                        value = m_interruptGen.ToString().ToLower();
                        break;
                    default:
                        break;
                }
                if (value != null)
                {
                    m_inst.SetParamExpr(paramName, value);
                    m_inst.CommitParamExprs();
                }
            }
        }
        #endregion
    }
}
