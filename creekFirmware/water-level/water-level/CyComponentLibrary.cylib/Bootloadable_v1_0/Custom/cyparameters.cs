/*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided. 
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.IO;

namespace Bootloadable_v1_0
{
    public class CyParameters
    {
        #region Constants
        public const string PARAM_APPVERSION = "appVersion";
        public const string PARAM_APPID = "appID";
        public const string PARAM_APPCUSTOMID = "appCustomID";
        public const string PARAM_AUTOPLACEMENT = "autoPlacement";
        public const string PARAM_PLACEMENTADDRESS = "placementAddress";
        public const string PARAM_HEXFILEPATH = "hexFilePath";
        public const string PARAM_ELFFILEPATH = "elfFilePath";
        #endregion Constants

        #region Properties, Fields
        public UInt16 AppVersion
        {
            get { return GetValue<UInt16>(PARAM_APPVERSION); }
            set { SetValue(PARAM_APPVERSION, value); }
        }
        public UInt16 AppID
        {
            get { return GetValue<UInt16>(PARAM_APPID); }
            set { SetValue(PARAM_APPID, value); }
        }
        public UInt32 AppCustomID
        {
            get { return GetValue<UInt32>(PARAM_APPCUSTOMID); }
            set { SetValue(PARAM_APPCUSTOMID, value); }
        }
        public bool AutoPlacement
        {
            get { return GetValue<bool>(PARAM_AUTOPLACEMENT); }
            set { SetValue(PARAM_AUTOPLACEMENT, value); }
        }
        public UInt32 PlacementAddress
        {
            get { return GetValue<UInt32>(PARAM_PLACEMENTADDRESS); }
            set { SetValue(PARAM_PLACEMENTADDRESS, value); }
        }

        public string HexFilePath
        {
            get { return GetValue<string>(PARAM_HEXFILEPATH); }
            set 
            {
                SetValue(PARAM_HEXFILEPATH, value);

                string elfFilePath = Path.ChangeExtension(value, ".elf");
                SetValue(PARAM_ELFFILEPATH, elfFilePath);
            }
        }

        public readonly ICyInstQuery_v1 m_inst;
        public bool m_globalEditMode = false;
        #endregion Properties, Fields

        #region Common
        public CyParameters()
        {
        }

        public CyParameters(ICyInstQuery_v1 inst)
            : this()
        {
            m_inst = inst;
        }

        private T GetValue<T>(string paramName)
        {
            return GetValue<T>(m_inst.GetCommittedParam(paramName));
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
                        valueToSet = Convert.ToInt32(value).ToString();
                    if ((value is byte) || (value is UInt16) || (value is UInt32) || (value is UInt64))
                        valueToSet = valueToSet + "u";
                    if (edit.SetParamExpr(paramName, valueToSet))
                    {
                        edit.CommitParamExprs();
                    }
                }
        }
        #endregion Common
    }
}
