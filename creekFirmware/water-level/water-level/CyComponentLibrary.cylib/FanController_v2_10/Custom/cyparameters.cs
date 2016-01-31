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
using FanController_v2_10;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;


namespace FanController_v2_10
{
    public enum CyPWMResType
    {
        [Description("8 bit")]
        EIGHT_BIT = 0,
        [Description("10 bit")]
        TEN_BIT = 1
    }
    public enum CyFanModeType
    {
        FIRMWARE = 0,
        HARDWARE = 1
    }
    public enum CyPWMFreqType
    {
        [Description("25 kHz")]
        TWENTYFIVE_KHZ = 0,
        [Description("50 kHz")]
        FIFTY_KHZ = 1
    }
    public enum CyConnectinType
    {
        WIRED = 1,
        BUSSED = 0
    }

    public enum CyMotorType
    {
        FOUR_POLE = 0,
        SIX_POLE = 1
    }
    public class CyParameters
    {
        public const string P_NUMBER_OF_FANS = "NumberOfFans";
        public const string P_NUMBER_OF_BANKS = "NumberOfBanks";
        const string P_FAN_MODE = "FanMode";
        const string P_ALERT_ENABLE = "AlertEnable";
        const string P_CONNECTION = "Connection";
        const string P_FAN_PWM_FREQ = "FanPWMFreq";
        const string P_FAN_PWM_RES = "FanPWMRes";
        const string P_DAMPING_FACTOR = "DampingFactor";
        const string P_ACOUSTIC_NOISE_REDUCTION = "AcousticNoiseReduction";
        const string P_FAN_TOLERANCE = "FanTolerance";
        public const string P_MIN_RPM = "MinRPM";
        public const string P_MAX_RPM = "MaxRPM";
        public const string P_MIN_DUTY = "MinDuty";
        public const string P_MAX_DUTY = "MaxDuty";
        const string P_INITIAL_RPM = "InitialRPM";
        public const string P_INITIAL_DUTY_CYCLE = "InitialDutyCycle";

        const string P_EXTERNAL_CLOCK = "ExternalClock";
        const string P_MOTOR_TYPE = "MotorType";

        static string L_DEFAULT = " (default)";

        public const Byte MAX_FANS = 16;
        public const Byte MAX_FANS_CLOSED_8B = 12;
        public const Byte MAX_FANS_CLOSED_10B = 8;

        public const Byte ALERT_FAN_STALL = 1;
        public const Byte ALERT_SPEED_FAIL = 2;

        public const int VSPC_DEF = 25;

        public const int XPOS_NUD = 19;
        public const int YPOS_NUD = 4;
        public const int WIDTH_10K_NUD = 54;
        public const int HEIGHT_10K_NUD = 20;

        public const int XPOS_FAN_NUM_LBL = 35;
        public const int YPOS_FAN_NUM_LBL = 6;


        public const UInt16 MIN_RPM_NUD = 500;
        public const UInt16 MAX_RPM_NUD = 25000;

        public const UInt16 MIN_DUTY_NUD = 0;
        public const UInt16 MAX_DUTY_NUD = 100;


        public ICyInstQuery_v1 m_inst = null;
        public ICyInstValidate_v1 m_instVal = null;

        public CyFansTab m_controlfans = null;

        public bool m_globalEditMode = false;


        public CyParameters() { }

        public byte NumberOfFans
        {
            get { return GetValue<byte>(P_NUMBER_OF_FANS); }
            set { SetValue(P_NUMBER_OF_FANS, value); }
        }
        public byte NumberOfBanks
        {
            get { return GetValue<byte>(P_NUMBER_OF_BANKS); }
            set { SetValue(P_NUMBER_OF_BANKS, value); }
        }

        public CyFanModeType FanMode
        {
            get { return GetValue<CyFanModeType>(P_FAN_MODE); }
            set { SetValue(P_FAN_MODE, value); }
        }
        public byte AlertEnable
        {
            get { return GetValue<byte>(P_ALERT_ENABLE); }
            set { SetValue(P_ALERT_ENABLE, value); }
        }
        public CyPWMFreqType FanPWMFreq
        {
            get { return GetValue<CyPWMFreqType>(P_FAN_PWM_FREQ); }
            set { SetValue(P_FAN_PWM_FREQ, value); }
        }
        public CyPWMResType FanPWMRes
        {
            get { return GetValue<CyPWMResType>(P_FAN_PWM_RES); }
            set { SetValue(P_FAN_PWM_RES, value); }
        }
        public double DampingFactor
        {
            get { return (double)GetValue<byte>(P_DAMPING_FACTOR) / 100; }
            set
            {
                int val = (int)(value * 100);
                SetValue(P_DAMPING_FACTOR, val);
            }
        }
        public byte AcousticNoiseReduction
        {
            get { return GetValue<byte>(P_ACOUSTIC_NOISE_REDUCTION); }
            set { SetValue(P_ACOUSTIC_NOISE_REDUCTION, value); }
        }
        public byte FanTolerance
        {
            get { return GetValue<byte>(P_FAN_TOLERANCE); }
            set { SetValue(P_FAN_TOLERANCE, value); }
        }
        public CyConnectinType Connection
        {
            get { return GetValue<CyConnectinType>(P_CONNECTION); }
            set { SetValue(P_CONNECTION, value); }
        }
        public byte ExternalClock
        {
            get { return GetValue<byte>(P_EXTERNAL_CLOCK); }
            set { SetValue(P_EXTERNAL_CLOCK, value); }
        }

        public CyMotorType MotorType
        {
            get { return GetValue<CyMotorType>(P_MOTOR_TYPE); }
            set { SetValue(P_MOTOR_TYPE, value); }
        }

        public int GetMinRPM(int i)
        {
            return GetValue<UInt16>(GetName(P_MIN_RPM, i));
        }
        public void SetMinRPM(int i, UInt16 value)
        {
            SetValue(GetName(P_MIN_RPM, i), value);
        }
        public int GetMaxRPM(int i)
        {
            return GetValue<UInt16>(GetName(P_MAX_RPM, i));
        }
        public void SetMaxRPM(int i, UInt16 value)
        {
            SetValue(GetName(P_MAX_RPM, i), value);
        }
        public int GetMinDuty(int i)
        {
            return GetValue<UInt16>(GetName(P_MIN_DUTY, i));
        }
        public void SetMinDuty(int i, UInt16 value)
        {
            SetValue(GetName(P_MIN_DUTY, i), value);
        }
        public int GetMaxDuty(int i)
        {
            return GetValue<UInt16>(GetName(P_MAX_DUTY, i));
        }
        public void SetMaxDuty(int i, UInt16 value)
        {
            SetValue(GetName(P_MAX_DUTY, i), value);
        }
        public UInt16 GetInitialRPM(int i)
        {
            return GetValue<UInt16>(GetName(P_INITIAL_RPM, i));
        }
        public void SetInitialRPM(int i, UInt16 value)
        {
            SetValue(GetName(P_INITIAL_RPM, i), value);
        }

        public static string GetName(string name, int index)
        {
            return name + (index+1).ToString().PadLeft(2,'0');
        }

        private T GetValue<T>(string paramName)
        {
            if (m_inst != null)
                return GetValue<T>(m_inst.GetCommittedParam(paramName));
            else if (m_instVal != null)
                return GetValue<T>(m_instVal.GetCommittedParam(paramName));
            else return default(T);
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
                if (m_inst != null && m_inst is ICyInstEdit_v1)
                {
                    ICyInstEdit_v1 edit = m_inst as ICyInstEdit_v1;
                    string valueToSet = value.ToString();
                    if (value is bool)
                        valueToSet = valueToSet.ToLower();
                    if (value.GetType().BaseType == typeof(Enum))
                    {

                        valueToSet = value.ToString();
                    }
                    if (edit.SetParamExpr(paramName, valueToSet.ToString()))
                    {
                        edit.CommitParamExprs();
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
                    return Enum.Parse(_enumType, fi.Name);
            }

            if (value != null)
            {
                if (value.IndexOf(L_DEFAULT) > 0)
                    value = value.Substring(0, value.Length - L_DEFAULT.Length);
            }

            return Enum.Parse(_enumType, value);
        }
        public static void FillEnum(ComboBox cb, Type enumType)
        {
            cb.Items.Clear();
            cb.Items.AddRange(GetDescriptionList(enumType));
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
                if ((item == str) || (item.IndexOf(L_DEFAULT) > 0 &&
                    (str == item.Substring(0, item.Length - L_DEFAULT.Length))))
                {
                    ff.SelectedIndex = i;
                    return;
                }
            }
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
            FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

            if (dna != null)
                return dna.Description;
            else
                return value.ToString();
        }

        public static int GetNumericUpDownText(object numericUpDown, out bool error)
        {
            error = false;
            int val = 0;
            try
            {
                val = Convert.ToInt32(((NumericUpDown)numericUpDown).Text);
            }
            catch
            { error = true; }
            return val;
        }
    }
}
