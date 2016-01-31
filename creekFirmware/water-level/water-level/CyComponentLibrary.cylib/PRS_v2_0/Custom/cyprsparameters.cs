/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;

namespace PRS_v2_0
{
    public class CyPRSParameters
    {
        #region Constants, Enum

        private enum CyRunMode
        {
            CLOCKED, API_SINGLE_STEP
        } ;

        public const string PARAM_RESOLUTION = "Resolution";
        public const string PARAM_SEEDVALUE = "SeedValue";
        public const string PARAM_SEEDVALUELOWER = "SeedValueLower";
        public const string PARAM_SEEDVALUEUPPER = "SeedValueUpper";
        public const string PARAM_POLYVALUE = "PolyValue";
        public const string PARAM_POLYVALUELOWER = "PolyValueLower";
        public const string PARAM_POLYVALUEUPPER = "PolyValueUpper";
        public const string PARAM_RUNMODE = "RunMode";
        private const string PARAM_RUNMODE_CLOCKED = "Clocked";
        private const string PARAM_RUNMODE_APISINGLESTEP = "APISingleStep";
        public const string PARAM_TIMEMULTIPLEXING = "TimeMultiplexing";
        public const string PARAM_WAKEUPBEHAVIOUR = "WakeupBehaviour";

        #endregion Constants

        #region Fields

        private readonly ICyInstEdit_v1 m_inst;

        private UInt64 m_seedValue = 1;      
        public UInt64 m_polyValue;
        public int m_runMode = 1;
        public uint m_resolution = 2;
        public bool m_timeMultiplexing;
        public int m_wakeupBehaviour;
        public bool m_editMode ;

        public CyAdvancedControl m_advancedPage;
        #endregion Fields

        #region Properties

        public UInt64 SeedValue
        {
            get { return m_seedValue; }
            set 
            {
                m_seedValue = value;
                SetParam(PARAM_SEEDVALUE);
            }
        }

        #endregion

        #region Constructors

        public CyPRSParameters(ICyInstEdit_v1 inst)
        {            
            this.m_inst = inst;
            m_editMode = true;
            GetParams("All");
        }

        #endregion Constructors

        #region Common functions

        public void GetParams(string paramName)
        {
            switch (paramName)
            {
                case PARAM_RESOLUTION:
                    LoadResolution();
                    break;

                case PARAM_SEEDVALUE:
                    LoadSeedValue();
                    break;

                case PARAM_POLYVALUE:
                    LoadPolyValue();
                    break;

                case PARAM_RUNMODE:
                    LoadRunMode();
                    break;

                case PARAM_TIMEMULTIPLEXING:
                    LoadTimeMultiplexing();
                    break;

                case PARAM_WAKEUPBEHAVIOUR:
                    LoadWakeupBehaviour();
                    break;

                default:
                    LoadResolution();
                    LoadSeedValue();
                    LoadPolyValue();
                    LoadRunMode();
                    LoadTimeMultiplexing();
                    LoadWakeupBehaviour();
                    break;
            }
        }

        private void LoadResolution()
        {
            uint.TryParse(m_inst.GetCommittedParam(PARAM_RESOLUTION).Value, out m_resolution);
        }

        private void LoadSeedValue()
        {
            if (m_resolution > 32)
            {
                m_seedValue = (Convert.ToUInt64(m_inst.GetCommittedParam(PARAM_SEEDVALUEUPPER).Value) << 32) +
                        Convert.ToUInt64(m_inst.GetCommittedParam(PARAM_SEEDVALUELOWER).Value);
            }
            else
            {
                UInt64.TryParse(m_inst.GetCommittedParam(PARAM_SEEDVALUELOWER).Value, out m_seedValue);
            }
        }

        private void LoadPolyValue()
        {
            if (m_resolution > 32)
            {
                m_polyValue = (Convert.ToUInt64(m_inst.GetCommittedParam(PARAM_POLYVALUEUPPER).Value) << 32) +
                        Convert.ToUInt64(m_inst.GetCommittedParam(PARAM_POLYVALUELOWER).Value);
            }
            else
            {
                UInt64.TryParse(m_inst.GetCommittedParam(PARAM_POLYVALUELOWER).Value, out m_polyValue);
            }
        }

        private void LoadRunMode()
        {
            m_inst.GetCommittedParam(PARAM_RUNMODE).TryGetValueAs(out m_runMode);
        }

        private void LoadTimeMultiplexing()
        {
            m_inst.GetCommittedParam(PARAM_TIMEMULTIPLEXING).TryGetValueAs(out m_timeMultiplexing);
        }

        private void LoadWakeupBehaviour()
        {
            m_inst.GetCommittedParam(PARAM_WAKEUPBEHAVIOUR).TryGetValueAs(out m_wakeupBehaviour);
        }

        public void SetParam(string paramName)
        {           
            if (m_editMode && (m_inst != null))
            {
                switch (paramName)
                {
                    case PARAM_POLYVALUE:
                        m_inst.SetParamExpr(PARAM_POLYVALUEUPPER, (m_polyValue >> 32).ToString() + "u");
                        m_inst.SetParamExpr(PARAM_POLYVALUELOWER, (m_polyValue & 0xFFFFFFFF).ToString() + "u");
                        break;
                    case PARAM_SEEDVALUE:
                        m_inst.SetParamExpr(PARAM_SEEDVALUEUPPER, (m_seedValue >> 32).ToString() + "u");
                        m_inst.SetParamExpr(PARAM_SEEDVALUELOWER, (m_seedValue & 0xFFFFFFFF).ToString() + "u");
                        break;
                    case PARAM_RESOLUTION:
                        m_inst.SetParamExpr(PARAM_RESOLUTION, m_resolution.ToString());
                        break;
                    case PARAM_RUNMODE:
                        if (m_runMode == (int)CyRunMode.CLOCKED)
                            m_inst.SetParamExpr(PARAM_RUNMODE, PARAM_RUNMODE_CLOCKED);
                        else
                            m_inst.SetParamExpr(PARAM_RUNMODE, PARAM_RUNMODE_APISINGLESTEP);
                        break;
                    case PARAM_TIMEMULTIPLEXING:
                        m_inst.SetParamExpr(PARAM_TIMEMULTIPLEXING, m_timeMultiplexing.ToString().ToLower());
                        break;
                    case PARAM_WAKEUPBEHAVIOUR:
                        m_inst.SetParamExpr(PARAM_WAKEUPBEHAVIOUR, m_wakeupBehaviour.ToString());
                        break;
                    default:
                        break;
                }
                CommitParams();
            }
        }

        public void CommitParams()
        {
            m_inst.CommitParamExprs();
        }

        #endregion Common functions
    }
}
