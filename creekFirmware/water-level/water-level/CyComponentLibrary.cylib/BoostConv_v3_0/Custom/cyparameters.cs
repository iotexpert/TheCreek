/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;

namespace BoostConv_v3_0
{
    public enum CyFrequency
    {
        [Description("400 KHz")]
        SWITCH_FREQ_400KHZ = 1,
        [Description("32 KHz")]
        SWITCH_FREQ_32KHZ = 3
    }
    public enum CyClockFr
    {
        [Description("None")]
        EXTCLK_NONE = 0,
        [Description("ECO 32kHz")]
        EXTCLK_ECO = 1,
        [Description("ILO 32kHz")]
        EXTCLK_ILO = 2
    }
    public class CyParameters
    {
        public ICyInstQuery_v1 m_edit;

        public bool m_globalEditMode = false;

        private double m_inputVoltage;
        private byte m_outputCurrent;
        private byte m_outputVoltage;
        private bool m_disableAutoBattery;

        public const string P_CONFIG_PARAMETERS_TAB_NAME = "Config";

        public const string INPUT_VOLTAGE = "InputVoltage";
        public const string OUTPUT_CURRENT = "OutCurrent";
        public const string OUTPUT_VOLTAGE = "OutVoltage";
        public const string FREQUENCY = "Frequency";
        public const string EXTERNAL_CLOCK_SRC = "ExtClk_Source";
        public const string DISABLE_AUTO_BATTERY = "DisableAutoBattery";

        public const double MIN_PSOC5LP_INPUT_VOLTAGE = 0.75;
        public const double MIN_INPUT_VOLTAGE = 0.5;
        public const double MAX_INPUT_VOLTAGE = 3.6;

        public static object[] m_outputValuesRange = new object[]{
            0,1.8,1.9,2,2.1,2.2,2.3,2.4,2.5,2.6,2.7,2.8,2.9,3,3.1,3.2,3.3,3.4,3.5,3.6,4,4.25,4.5,4.75,5,5.25};
            
 
        public CyParameters(ICyInstQuery_v1 edit)
        {
            m_edit = edit;
            LoadParams();
        }

        public double InputVoltage
        {
            get { return m_inputVoltage; }
            set
            {
                if (m_inputVoltage != value)
                {
                    m_inputVoltage = value;
                    SaveParam(INPUT_VOLTAGE, m_inputVoltage.ToString());
                }
            }
        }

        public byte OutCurrent
        {
            get { return m_outputCurrent; }
            set
            {
                if (m_outputCurrent != value)
                {
                    m_outputCurrent = value;
                    SaveParam(OUTPUT_CURRENT, m_outputCurrent.ToString());
                }
            }
        }

        public byte OutVoltage
        {
            get { return m_outputVoltage; }
            set
            {
                if (m_outputVoltage != value)
                {
                    m_outputVoltage = value;
                    SaveParam(OUTPUT_VOLTAGE, m_outputVoltage.ToString());
                }
            }
        }

        public CyFrequency Frequency
        {
            get { return GetValue<CyFrequency>(FREQUENCY); }
            set
            {
                SetValue(FREQUENCY, value);
            }
        }

        public CyClockFr ExternalClockSrc
        {
            get { return GetValue<CyClockFr>(EXTERNAL_CLOCK_SRC); }
            set { SetValue(EXTERNAL_CLOCK_SRC, value); }
        }

        public bool DisableAutoBattery
        {
            get { return m_disableAutoBattery; }
            set 
            {
                if (m_disableAutoBattery != value)
                {
                    m_disableAutoBattery = value;
                    SaveParam(DISABLE_AUTO_BATTERY, m_disableAutoBattery.ToString());
                }
            }
        }

        public void LoadParams()
        {
            if (m_edit != null)
            {
                CyCustErr err;
                CyCompDevParam param = m_edit.GetCommittedParam(INPUT_VOLTAGE);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<double>(out m_inputVoltage);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(OUTPUT_CURRENT);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<byte>(out m_outputCurrent);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(OUTPUT_VOLTAGE);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<byte>(out m_outputVoltage);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);

                param = m_edit.GetCommittedParam(DISABLE_AUTO_BATTERY);
                if (param != null && param.ErrorCount == 0)
                {
                    err = param.TryGetValueAs<bool>(out m_disableAutoBattery);
                    Debug.Assert(err.IsOk);
                }
                else Debug.Assert(param != null);
            }
        }


        private void SaveParam(String paramName, String paramValue)
        {
            if (m_edit != null && m_edit is ICyInstEdit_v1)
            {
                (m_edit as ICyInstEdit_v1).SetParamExpr(paramName, paramValue);
                (m_edit as ICyInstEdit_v1).CommitParamExprs();
            }
        }

        private T GetValue<T>(string paramName)
        {
            return GetValue<T>(m_edit.GetCommittedParam(paramName));
        }

        public static T GetValue<T>(CyCompDevParam prm)
        {
            T value = default(T);
            CyCustErr err;
            if (typeof(T).BaseType == typeof(Enum))
            {
                int res;
                err = prm.TryGetValueAs<int>(out res);
                try
                {
                    value = (T)Enum.ToObject(value.GetType(), res);
                }
                catch { value = default(T); }
            }
            else
                err = prm.TryGetValueAs<T>(out value);
            if (err != null && err.IsOK)
            {
                return value;
            }
            else
            {
                return default(T);
            }
        }
        private void SetValue<T>(string paramName, T value)
        {
            if (m_globalEditMode)
                if (m_edit != null && m_edit is ICyInstEdit_v1)
                {
                    ICyInstEdit_v1 edit = m_edit as ICyInstEdit_v1;
                    string valueToSet = value.ToString();
                    if (value is bool)
                        valueToSet = valueToSet.ToLower();
                    if (value.GetType().BaseType == typeof(Enum))
                        valueToSet = value.ToString();
                    if (value is UInt16 || value is UInt32 || value is UInt64)
                        valueToSet = value.ToString() + "u";
                    if (edit.SetParamExpr(paramName, valueToSet.ToString()))
                    {
                        edit.CommitParamExprs();
                    }
                }
        }
        public static void FillEnum(ComboBox cb, Type enumType)
        {
            cb.Items.Clear();
            cb.Items.AddRange(GetDescriptionList(enumType));
        }

        static string[] GetDescriptionList(Type _enumType)
        {
            List<string> res = new List<string>();
            foreach (object item in Enum.GetValues(_enumType))
            {
                res.Add(GetDescription(item));
            }
            return res.ToArray();
        }
        static string GetDescription(object value)
        {
            Type _enumType = value.GetType();
            try
            {
                FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
                DescriptionAttribute dna =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(
                    fi, typeof(DescriptionAttribute));

                if (dna != null)
                    return dna.Description;
                else
                    return value.ToString();
            }
            catch { return string.Empty; }
        }
        public static void SetValue(ComboBox ff, object value)
        {
            string descr = GetDescription(value);
            SetString(ff, descr);
        }
        static void SetString(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)
            {
                string item = ff.Items[i].ToString();
                if (item == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
            }
        }
        public static T GetEnum<T>(string value)
        {
            return (T)GetEnum(value, typeof(T));
        }
        /// <summary>
        /// Convert enum description string to enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static object GetEnum(string value, Type _enumType)
        {
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

                if ((dna != null) && (value == dna.Description))
                {
                    object res = Enum.Parse(_enumType, fi.Name);
                    return res;
                }

            }

            return Enum.Parse(_enumType, value);
        }

        public bool IsPSoC5LP()
        {
            return (m_edit.DeviceQuery.ArchFamilyMember == "PSoC5LP");
        }
    }
}
