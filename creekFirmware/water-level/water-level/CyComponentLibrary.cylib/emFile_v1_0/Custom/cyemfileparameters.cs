/*******************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Diagnostics;

namespace emFile_v1_0
{
    public class CyParamNames
    {
        public const string MAX_SPI_FREQUENCY = "Max_SPI_Frequency";
        public const string NUMBER_SD_CARDS = "NumberSDCards";
        public const string WP0_EN = "WP0_En";
        public const string WP1_EN = "WP1_En";
        public const string WP2_EN = "WP2_En";
        public const string WP3_EN = "WP3_En";
    }

    public class CyParamRanges
    {
        public const int SD_CARD_NUMBER_MIN = 1;
        public const int SD_CARD_NUMBER_MAX = 4;

        public const int MAX_SPI_FREQUENCY_MIN = 400;
        public const int MAX_SPI_FREQUENCY_MAX = 25000;
    }

    public class CyEmFileParameters
    {
        public ICyInstEdit_v1 m_inst;
        
        // During first getting of parameters this variable is false, what means that assigning
        // values to form controls will not immediatly overwrite parameters with the same values.
        public bool m_globalEditMode = false;

        // User control(s)
        public CyEmFileGeneralTab m_generalTab = null;

        #region Constructor(s)
        public CyEmFileParameters(ICyInstEdit_v1 inst)
        {
            m_inst = inst;
        }
        #endregion

        #region Class properties
        public int MaxSPIFrequency
        {
            get { return GetValue<int>(CyParamNames.MAX_SPI_FREQUENCY); }
            set { SetValue(CyParamNames.MAX_SPI_FREQUENCY, value); }
        }

        public int NumberSDCards
        {
            get { return GetValue<byte>(CyParamNames.NUMBER_SD_CARDS); }
            set { SetValue(CyParamNames.NUMBER_SD_CARDS, value); }
        }

        public bool WP0Enable
        {
            get { return GetValue<bool>(CyParamNames.WP0_EN); }
            set { SetValue(CyParamNames.WP0_EN, value); }
        }

        public bool WP1Enable
        {
            get { return GetValue<bool>(CyParamNames.WP1_EN); }
            set { SetValue(CyParamNames.WP1_EN, value); }
        }

        public bool WP2Enable
        {
            get { return GetValue<bool>(CyParamNames.WP2_EN); }
            set { SetValue(CyParamNames.WP2_EN, value); }
        }

        public bool WP3Enable
        {
            get { return GetValue<bool>(CyParamNames.WP3_EN); }
            set { SetValue(CyParamNames.WP3_EN, value); }
        }
        #endregion

        #region Getting Parameters
        public T GetValue<T>(string paramName)
        {
            T value;
            CyCompDevParam param = m_inst.GetCommittedParam(paramName);
            if (param != null)
            {
                param.TryGetValueAs<T>(out value);
                bool err = new List<string>(param.Errors).Count > 0;
                if (err == false)
                {
                    return value;
                }
            }
            else
            {
                Debug.Assert(false);
            }
            return default(T);
        }

        public void LoadParameters()
        {
            m_globalEditMode = false;
            m_generalTab.UpdateUI();
            m_globalEditMode = true;
        }
        #endregion

        #region Setting Parameters
        public void SetValue<T>(string paramName, T value)
        {
            if (m_globalEditMode)
            {
                if (value is bool)
                    m_inst.SetParamExpr(paramName, value.ToString().ToLower());
                else
                    try
                    {
                        m_inst.SetParamExpr(paramName, value.ToString());
                    }
                    catch
                    {
                        Debug.Assert(false);
                    }

                m_inst.CommitParamExprs();
            }
        }
        #endregion
    }
}
