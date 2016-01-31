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
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.ComponentModel;


namespace CapSense_v1_30
{
    #region System Messages
    public static class CyIntMessages
    {
        public const string STR_HA_FT_HY = "FingerThreshold + Hysteresis can't be more than 255!";
        public const string STR_CONFMODE = "Because you can't work in Parallel Synchronized Mode " +
            "with different CapSense Methods.";
        public const string STR_CLK_COMPARER = "The CapSense clock could not be greater than fastest " +
            "clock in your system.";
    }
    #endregion

    #region Enums
    public enum E_EL_SIDE { None = -1, Left = 0, Right = 1 }

    public enum E_ENE_DIS { Disabled = 0, Enabled = 1 };

    public enum E_MAIN_CONFIG { emSerial = 0, emParallelSynchron = 1, emParallelAsynchron = 2 }

    public enum E_CAPSENSE_MODE { CSD, CSA, None }
    public enum E_FO_FILTER
    {
        [Description("Disabled")]
        Disabled,
        [Description("0.5 previous + 0.5 current (Default)")]
        Prev0_5,
        [Description("0.75 previous + 0.25 current")]
        Prev0_75
    };
    public enum E_CSD_SUB_METHODS { IDACSourcing, IDACSinking, IDACDisable_RB }
    public enum E_PRESCALER { None, UDB, HW };
    public enum E_IDAC_OPTIONS { None, Source, Sink };
    public enum E_SENSOR_TYPE
    {
        Button = 0,
        Linear_Slider = 1,
        Radial_Slider = 2,
        Touchpad_Col = 4,
        Touchpad_Row = 3,
        Matrix_Buttons_Col = 6,
        Matrix_Buttons_Row = 5,
        Proximity = 7,
        Generic = 8
    }
    enum CyClockOptions { Clock_Bus, Clock_Auto };
    public enum E_COMPARER_TYPE { None, NameChange = 1, CountChange = 2, SideChange = 4, TypeChange = 8 };

    public enum E_SSDISABLESTATE { GND, Hi_Z, Shield }
    public enum E_PRS_OPTIONS
    {
        [Description("None")]
        None = 0,
        [Description("8 bits")]
        _8bits = 8,
        [Description("16 bits")]
        _16bits = 16
    };
    public enum E_VREF_OPTIONS
    {
        [Description("Ref_Vref")]
        Ref_Vref = 0,
        [Description("Ref_Vdac")]
        Ref_Vdac = 1
    };

    #region eCapSenseMode
    public static class CyStrCapSenseMode
    {
        public static string[] m_strCMode = new string[2] { "CSD", "CSA" };
        public static string GetStr(E_CAPSENSE_MODE en)
        {
            switch (en)
            {
                case E_CAPSENSE_MODE.CSD:
                    return "CSD";
                case E_CAPSENSE_MODE.CSA:
                    return "CSA";
                default:
                    break;
            }
            return "None";
        }
        public static E_CAPSENSE_MODE GetEnum(string str)
        {
            foreach (string item in m_strCMode)
                for (int i = 0; i < m_strCMode.Length; i++)
                {
                    if (m_strCMode[i] == str)
                        return (E_CAPSENSE_MODE)i;
                }
            return E_CAPSENSE_MODE.None;

        }
    }
    #endregion

    #region sensorType
    public static class CySensorType
    {
        public static string GetBaseType(E_SENSOR_TYPE type)
        {
            string res = type.ToString().ToUpper();

            if (GetBothParts(type).Count > 1)
            {
                return res.Substring(0, res.Length - 4);//Delete "_Col" or "_Row"
            }
            return res;
        }

        public static bool IsCustomCase(E_SENSOR_TYPE type)
        {
            return (type == E_SENSOR_TYPE.Generic) || (type == E_SENSOR_TYPE.Proximity);
        }
        public static bool IsWidgetLabel(CyElTerminal item)
        {
            return IsCustomCase(item.m_type) && (item.m_Nameindex == -1);
        }
        public static bool IsNotWidgetLabel(CyElTerminal item)
        {
            return IsWidgetLabel(item) == false;
        }
        public static bool IsRow(E_SENSOR_TYPE type)
        {
            if ((type == E_SENSOR_TYPE.Matrix_Buttons_Row)) return true;
            if ((type == E_SENSOR_TYPE.Touchpad_Row)) return true;
            return false;
        }
        public static bool IsTouchPad(E_SENSOR_TYPE type)
        {
            if ((type == E_SENSOR_TYPE.Touchpad_Row)) return true;
            if ((type == E_SENSOR_TYPE.Touchpad_Col)) return true;
            return false;
        }
        public static bool IsSlider(E_SENSOR_TYPE type)
        {
            if ((type == E_SENSOR_TYPE.Linear_Slider)) return true;
            if ((type == E_SENSOR_TYPE.Radial_Slider)) return true;
            return false;
        }
        public static bool IsButtonsStrc(Type type)
        {
            if ((type == typeof(CyElUnButton)) || ((type == typeof(CyElUnMatrixButton)))) return true;
            return false;
        }
        public static bool IsArrayCase(E_SENSOR_TYPE type)
        {
            if ((type == E_SENSOR_TYPE.Matrix_Buttons_Row)) return true;
            if ((type == E_SENSOR_TYPE.Matrix_Buttons_Col)) return true;
            if ((type == E_SENSOR_TYPE.Touchpad_Col)) return true;
            if ((type == E_SENSOR_TYPE.Touchpad_Row)) return true;
            return false;
        }
        public static List<E_SENSOR_TYPE> GetBothParts(E_SENSOR_TYPE type)//!=MB, !=TB
        {
            List<E_SENSOR_TYPE> res = new List<E_SENSOR_TYPE>();
            res.Add(type);
            if ((type == E_SENSOR_TYPE.Matrix_Buttons_Row) || (type == E_SENSOR_TYPE.Matrix_Buttons_Col))
            {
                res.Clear();
                res.Add(E_SENSOR_TYPE.Matrix_Buttons_Col);
                res.Add(E_SENSOR_TYPE.Matrix_Buttons_Row);
            }
            if ((type == E_SENSOR_TYPE.Touchpad_Col) || (type == E_SENSOR_TYPE.Touchpad_Row))
            {
                res.Clear();
                res.Add(E_SENSOR_TYPE.Touchpad_Col);
                res.Add(E_SENSOR_TYPE.Touchpad_Row);
            }
            return res;
        }
    }

    #endregion

    #endregion

    #region CyGeneralParams

    [XmlRootAttribute("CyGeneralParams")]
    [Serializable()]
    public class CyGeneralParams
    {
        #region Head

        #region Const Properties
        [NonSerialized()]
        [XmlIgnore()]
        public static E_EL_SIDE[] m_sideArray = new E_EL_SIDE[] { E_EL_SIDE.Left, E_EL_SIDE.Right };
        [NonSerialized()]
        [XmlIgnore()]
        public static string[] m_strWidgetTypes = new string[] {
            "Buttons",
            "Slider Linear",
            "Slider Radial",
            "Touch Pads Row",
            "Touch Pads Col",
            "Matrix Buttons Row",
            "Matrix Buttons Col",
            "Proximity Sensors",
            "Generic Sensors"};

        [NonSerialized()]
        [XmlIgnore()]
        public const string STR_AB_HEAD = "Half";

        [NonSerialized()]
        [XmlIgnore()]
        public const string STR_AB_HEAD_SS = "AMUX Bus Scan Slot Assignment";

        [NonSerialized()]
        [XmlIgnore()]
        public static String[] STR_SLIDER_TYPE = new string[]    { "Linear",
            "Radial"};
        public static String GetStrSliderType(E_SENSOR_TYPE st)
        {
            String res = "";
            if (st == E_SENSOR_TYPE.Linear_Slider) res = STR_SLIDER_TYPE[0];
            if (st == E_SENSOR_TYPE.Radial_Slider) res = STR_SLIDER_TYPE[1];
            return res;

        }
        public static E_SENSOR_TYPE GetEnumSliderType(String st)
        {
            E_SENSOR_TYPE res = E_SENSOR_TYPE.Button;
            if (st == STR_SLIDER_TYPE[0]) res = E_SENSOR_TYPE.Linear_Slider;
            if (st == STR_SLIDER_TYPE[1]) res = E_SENSOR_TYPE.Radial_Slider;
            return res;
        }

        [NonSerialized()]
        [XmlIgnore()]
        public static Color COLOR_LEFT = Color.LightSteelBlue;

        [NonSerialized()]
        [XmlIgnore()]
        public static Color COLOR_RIGHT = Color.Khaki;

        #endregion

        #region Tabs

        [NonSerialized()]
        [XmlIgnore()]
        public CyGeneralTab m_cyGeneralTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyClockSwitchTab m_cyClockSourceTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyButtonsTab m_cyButtonsTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CySlidersTab m_cySlidersTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyGenericTab m_cyGenericTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyMatrixButtonsTab m_cyMatrixButtonsTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyProximityTab m_cyProximityTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyTouchPadsTab m_cyTouchPadsTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyScanSlotsTab m_cyScanSlotTab;
        #endregion

        [NonSerialized()]
        [XmlIgnore()]
        public ICyInstEdit_v1 m_edit;

        private bool m_GlobalEditMode = false;
        [XmlIgnore()]
        public bool GlobalEditMode { get { return m_GlobalEditMode; } set { m_GlobalEditMode = value; } }

        [XmlElement("localParams")]
        public CyLocalParams m_localParams = new CyLocalParams();

        [XmlIgnore()]
        public E_MAIN_CONFIG Configuration
        {
            get { return m_localParams.m_configuration; }
            set
            {
                m_localParams.m_configuration = value;
                if (m_cyScanSlotTab != null)
                    m_cyScanSlotTab.UpdateAllWidgetsInDataGrids();
            }
        }

        [XmlElement("cyScanSlotsList")]
        public CyScanSlotsList m_cyScanSlotsList;

        [XmlElement("cyWidgetsList")]
        public CyWidgetsList m_cyWidgetsList;

        public int GetRawDataOffset(CyElWidget wi)
        {
            int i = 0;
            foreach (CyElScanSlot item in m_cyScanSlotsList.GetScanSlotsSorted())
            {
                if (item.m_Widget == wi) return i;
                i++;
            }
            return 0;
        }

        #endregion

        #region Service static

        public static string GetString(object prop)
        {
            string res = "";
            if (prop == null)
                return res;
            res = prop.ToString();
            return res;

        }
        public static string GetDGString(object sender, int row, int col)
        {
            string res = "";
            if (((DataGridView)sender)[col, row].Value == null)
                return res;
            res = ((DataGridView)sender)[col, row].Value.ToString();
            return res;
        }
        public static bool IsSameArrays(object[] NewRow, object[] LastRow)
        {
            if (NewRow == null) return false;
            if (LastRow == null) return false;
            if (NewRow.Length != LastRow.Length) return false;
            for (int i = 0; i < LastRow.Length; i++)
                if (GetString(NewRow[i]) != GetString(LastRow[i]))
                {
                    return false;
                }
            return true;
        }

        #endregion

        #region Functions

        public CyGeneralParams()
        {
            m_cyScanSlotsList = new CyScanSlotsList(this);
            m_cyWidgetsList = new CyWidgetsList(this);
            m_localParams.FirstInitialszation();
        }
        public void Initialization()
        {
            m_cyScanSlotsList.m_Base = this;
            m_cyWidgetsList.m_Base = this;
            m_localParams.m_Base = this;
            m_localParams.m_actGlobalParamsChange += new EventHandler(SetCommitParams);
        }

        public IEnumerable<UserControl> GetAllTabsWidgets()
        {
            yield return m_cyButtonsTab;
            yield return m_cyGenericTab;
            yield return m_cyMatrixButtonsTab;
            yield return m_cyProximityTab;
            yield return m_cySlidersTab;
            yield return m_cyTouchPadsTab;
        }
        public IEnumerable<UserControl> GetAllTabsWithOutWidgets()
        {
            yield return m_cyScanSlotTab;
            yield return m_cyGeneralTab;
            yield return m_cyClockSourceTab;
        }

        public IEnumerable<UserControl> GetAllTabs()
        {
            foreach (UserControl item in GetAllTabsWidgets())
            {
                yield return item;
            }
            foreach (UserControl item in GetAllTabsWithOutWidgets())
            {
                yield return item;
            }
        }

        public void SetAllActions()
        {
            //Catch Enter event
            m_cyGeneralTab.m_actGlobalEditorGetErrors += new EventHandler(SetCommitParams);

            foreach (UserControl item in GetAllTabsWidgets())
            {
                item.Leave += new System.EventHandler(SetCommitParamsForWidgetsTabs);
            }
            GlobalEditMode = true;
        }

        public bool IsAmuxBusBlank(E_EL_SIDE side)
        {
            if (m_cyScanSlotsList.GetSSList(side).Count > 0)
                return false;
            return true;
        }
        #endregion

        #region Save/Load

        #region XML Serialization
        public string Serialize(object obj)
        {
            if (obj.GetType() == typeof(CyGeneralParams))
            {
                //Pre Serialization Events
                CyGeneralParams pp = (CyGeneralParams)obj;
                pp.PreSerialization();
            }
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            StringWriter tw = new StringWriter();
            x.Serialize(tw, obj);
            string res = tw.ToString();
            res = res.Replace("\r\n", " ");
            return res;
        }

        public static CyGeneralParams Deserialize(string serializedXml, bool _editparams)
        {
            bool _showmessage = false;
            CyGeneralParams res = null;
            try
            {
                System.Xml.Serialization.XmlSerializer x =
                    new System.Xml.Serialization.XmlSerializer(typeof(CyGeneralParams));
                TextReader tr = new StringReader(serializedXml);
                res = (CyGeneralParams)x.Deserialize(tr);

                _showmessage = _editparams;
                // Post Serialization Events
                res.PostSerializationCs();
                res.PostSerializationIMyExecute(res);
            }
            catch// (Exception ex)
            {
                if (_showmessage)
                    MessageBox.Show("CapSense settings have incorrect values! " +
                        "Please redefine it for proper CapSense operation."
                        , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = new CyGeneralParams();
                res.PostSerializationIMyExecute(res);
            }
            res.Initialization();

            return res;
        }
        public void PostSerializationIMyExecute(object obj)
        {
            if (obj != null)
                if (obj.GetType() != typeof(string))
                    if (obj.GetType().IsClass)
                    {
                        FieldInfo[] fields = obj.GetType().GetFields();

                        int inc = 0;

                        foreach (FieldInfo field in fields)
                            if (field.IsNotSerialized == false)
                            {
                                ExecutePostSerialization(field, obj);

                                //Check for next step
                                //Check for the IEnumerable interface
                                if (new List<Type>(field.FieldType.GetInterfaces()).Contains(typeof(IEnumerable)))
                                {
                                    //Get the IEnumerable interface from the field.
                                    IEnumerable IEnum = (IEnumerable)field.GetValue(obj);

                                    if (new List<Type>(fields[inc].FieldType.GetInterfaces()).Contains(typeof(IList)))
                                    {
                                        IList list = (IList)fields[inc].GetValue(obj);

                                        foreach (object item in IEnum)
                                        {
                                            if (new List<Type>(item.GetType().GetInterfaces()).
                                                Contains(typeof(ICyMyPostSerialization)))
                                            {
                                                //Getting the interface.
                                                ICyMyPostSerialization inter = (ICyMyPostSerialization)item;

                                                inter.ExecutePostSerialization();
                                            }
                                            //Next step
                                            PostSerializationIMyExecute(item);
                                        }
                                    }
                                }
                                else if (field.FieldType.IsSerializable)
                                {
                                    //Next step
                                    if (obj.GetType() != (field.FieldType))
                                        PostSerializationIMyExecute(field.GetValue(obj));
                                }

                                inc++;
                            }
                    }
        }
        void ExecutePostSerialization(FieldInfo fi, object obj)
        {
            Type IMyPostSerializationType = fi.FieldType.GetInterface(typeof(ICyMyPostSerialization).Name, true);

            if (IMyPostSerializationType != null)
            {
                //Getting the interface.
                ICyMyPostSerialization IField = (ICyMyPostSerialization)fi.GetValue(obj);

                IField.ExecutePostSerialization();
            }
        }
        #endregion

        #region Pre/Post Serialization Events
        public void PreSerialization()
        {
            foreach (CyElScanSlot item in m_cyScanSlotsList.m_listScanSlotsL)
            {
                item.PreSerialization();
            }
            foreach (CyElScanSlot item in m_cyScanSlotsList.m_listScanSlotsR)
            {
                item.PreSerialization();
            }
        }
        public void PostSerializationCs()
        {
            PsRestoreSS(E_EL_SIDE.Left, m_cyScanSlotsList.m_listScanSlotsL);
            PsRestoreSS(E_EL_SIDE.Right, m_cyScanSlotsList.m_listScanSlotsR);
        }
        public void PsRestoreSS(E_EL_SIDE side, List<CyElScanSlot> listSS)
        {
            foreach (CyElScanSlot item in listSS)
            {
                //Initial Index
                item.m_Index.m_index = m_cyScanSlotsList.NextIndex(side);
                //Add terminal list to scanslot
                foreach (string itemInt in item.m_srlistTerminalsNames)
                {
                    item.m_listTerminals.Add(m_cyWidgetsList.FindTerminal(itemInt));
                }

                //Add Widget
                item.m_Widget = m_cyWidgetsList.FindWidget(item.m_srWidgetName);
                if (item.m_Widget == null)
                {
                    item.m_Widget = m_cyWidgetsList.FindWidget(item.m_srWidgetName);
                    item.m_Widget = m_cyWidgetsList.FindWidgetOldPattern(item.m_srWidgetName);
                }

                //Add ssprops from widget
                item.m_SSProperties.m_baseProps = item.m_Widget.m_baseSSProps;
            }
        }

        #endregion

        #endregion

        #region Get/Set params

        #region Parameters names
        //Local parameters
        public const string C_INSTANCE_NAME = "INSTANCE_NAME";
        public const string C_XML_MAIN_DATA = "XMLMainData";
        public const string C_CLK = "Clk";
        public const string C_CLOCK_SOURCE = "ClockSource";
        public const string C_SYNC_MODE = "SyncMode";
        public const string C_SCAN_MODE = "ScanMode";
        public const string C_METHOD = "Method";
        public const string C_PRESCALER_OPTIONS = "PrescalerOptions";
        public const string C_IDAC_OPTIONS = "IdacOptions";
        public const string C_PRS_OPTIONS = "PrsOptions";
        public const string C_RB_NUMBER = "RbNumber";
        public const string C_SENSOR_NUMBER = "SensorNumber";
        public const string C_SHIELD_NUMBER = "ShieldNumber";
        public const string C_ALIAS_SNS = "AliasSns";
        public const string C_ALIAS_RB = "AliasRb";
        public const string C_ALIAS_CMOD = "AliasCmod";
        public const string C_VREF_OPTIONS = "VrefOptions";

        //Parameter values
        public const string C_ALIAS_SHIELD_VAL = "Shield";
        public const string C_ALIAS_RB_VAL = "Rb";
        public const string C_ALIAS_CMOD_VAL = "Cmod";

        public string GetAliasShieldByIndex(CyAmuxBParams item, int i)
        {
            return GetPrefixForSchematic(item) + C_ALIAS_SHIELD_VAL + i;
        }
        public string GetAliasRbByIndex(CyAmuxBParams item, int i)
        {
            return GetPrefixForSchematic(item) + C_ALIAS_RB_VAL + i;
        }

        #endregion

        public void GetParams(ICyInstQuery_v1 inst, CyCompDevParam param)
        {
            if (inst != null)
            {
                GetGeneralParameters(inst, param);
                m_cyScanSlotTab.LoadFormGeneralParams();
            }
        }
        public void GetGeneralParameters(ICyInstQuery_v1 inst, CyCompDevParam param)
        {
            if ((inst != null) && ((GlobalEditMode) || (param == null)))
            {
                this.GlobalEditMode = false;

                if ((ParameterChanged(param, C_SCAN_MODE, "")) || (ParameterChanged(param, C_SYNC_MODE, "")))
                    m_localParams.SetSyncScanModeOption(inst.GetCommittedParam(C_SCAN_MODE).Expr,
                      inst.GetCommittedParam(C_SYNC_MODE).Expr);

                foreach (CyAmuxBParams item in m_localParams.m_listCsHalfs)
                {
                    //Get Method
                    if (ParameterChanged(param, C_METHOD, GetSidePrefix(item)))
                        item.m_Method = (E_CAPSENSE_MODE)InternalGetParam(inst, item, C_METHOD,
                            typeof(E_CAPSENSE_MODE));

                    //Get ShieldElectrode
                    if (ParameterChanged(param, C_SHIELD_NUMBER, GetSidePrefix(item)))
                        if (item.IsShieldElectrode())
                            item.m_cShieldElectrode.Validate(InternalGetParam(inst, item, C_SHIELD_NUMBER));

                    //Get PRSResolution
                    if (ParameterChanged(param, C_PRS_OPTIONS, GetSidePrefix(item)))
                        item.SetPRSResolutionOption(InternalGetEnumParam(inst, item, C_PRS_OPTIONS));

                    //Get Prescaler
                    if (ParameterChanged(param, C_PRESCALER_OPTIONS, GetSidePrefix(item)))
                        item.SetPrescalerOptions(InternalGetEnumParam(inst, item, C_PRESCALER_OPTIONS));

                    //Get IdacOptions
                    if (ParameterChanged(param, C_IDAC_OPTIONS, GetSidePrefix(item)))
                        item.SetIdacOptions(InternalGetEnumParam(inst, item, C_IDAC_OPTIONS));

                    //Get Rb
                    if (ParameterChanged(param, C_RB_NUMBER, GetSidePrefix(item)))
                        if (item.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                            item.m_cRb.Validate(InternalGetParam(inst, item, C_RB_NUMBER));

                    //Get VrefOptions
                    if (ParameterChanged(param, C_VREF_OPTIONS, GetSidePrefix(item)))
                        item.m_VrefOptions = (E_VREF_OPTIONS)InternalGetParam(inst, item, C_VREF_OPTIONS,
                        typeof(E_VREF_OPTIONS));
                }

                //Block CSA Mode
                foreach (CyAmuxBParams item in m_localParams.m_listCsHalfs)
                {
                    if (item.m_Method == E_CAPSENSE_MODE.CSA)
                    {
                        item.m_Method = E_CAPSENSE_MODE.CSD;
                    }
                }

                if ((m_cyClockSourceTab != null) && (m_cyGeneralTab != null))
                {
                    //Loading Action
                    m_cyClockSourceTab.LoadFormGeneralParams();
                    m_cyGeneralTab.LoadFormGeneralParams();
                }
                this.GlobalEditMode = true;
            }
        }
        bool ParameterChanged(CyCompDevParam param, string param_name, string prefix)
        {
            if (param != null)
                if (param.Name != prefix + param_name) return false;
            return true;
        }

        public void ValidateParameters(CyAmuxBParams param)
        {
            if (GlobalEditMode)
            {
                GlobalEditMode = false;
                if (m_localParams.m_configuration == E_MAIN_CONFIG.emParallelSynchron)
                //Synchron mode Limits
                {
                    if (param.m_Method == E_CAPSENSE_MODE.CSD)
                    {
                        //Clone sub method
                        switch (param.m_side)
                        {
                            case E_EL_SIDE.Left:
                                m_localParams.m_listCsHalfs[1].m_csdSubMethod = param.m_csdSubMethod;
                                break;
                            case E_EL_SIDE.Right:
                                m_localParams.m_listCsHalfs[0].m_csdSubMethod = param.m_csdSubMethod;
                                break;
                            default:
                                break;
                        }
                    }
                }
                m_cyGeneralTab.LoadFormGeneralParams();
                GlobalEditMode = true;
                SetCommitParams(null, null);
            }
        }
        void SetParams(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                //Insert Global Parameters
                string strXML = Serialize(this);
                inst.SetParamExpr(C_XML_MAIN_DATA, strXML);

                inst.SetParamExpr(C_SYNC_MODE, m_localParams.GetSyncModeOption());

                inst.SetParamExpr(C_SCAN_MODE, m_localParams.GetScanModeOption());

                foreach (CyAmuxBParams item in m_localParams.m_listCsHalfs)
                    if (m_localParams.IsAmuxBusEnable(item))
                    {
                        //Set Rb
                        if (item.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            InternalSetParam(inst, item, C_RB_NUMBER, item.CountRb.ToString());
                        }
                        else
                        {
                            InternalSetParam(inst, item, C_RB_NUMBER, "0");
                        }
                        //Set Method
                        InternalSetParam(inst, item, C_METHOD, item.m_Method.ToString());

                        //Set ShieldElectrode
                        InternalSetParam(inst, item, C_SHIELD_NUMBER, item.CountShieldElectrode.ToString());

                        //Set PRSResolution
                        InternalSetParam(inst, item, C_PRS_OPTIONS, SetEnum(item.GetPRSResolutionOption(), inst));

                        //Set Terminal Count
                        int termCount = m_cyWidgetsList.GetListTerminals(item.m_side).Count;
                        InternalSetParam(inst, item, C_SENSOR_NUMBER, termCount.ToString());

                        //Set Clk
                        if (item.m_currectClk != null)
                            InternalSetParam(inst, item, C_CLK, item.m_currectClk.m_ActualFreq.ToString());
                        if ((item.m_currectClk == null) || (item.m_currectClk.IsDirectClock()))
                            InternalSetParam(inst, item, C_CLOCK_SOURCE, CyClockOptions.Clock_Bus.ToString());
                        else
                            InternalSetParam(inst, item, C_CLOCK_SOURCE, CyClockOptions.Clock_Auto.ToString());

                        //Set Prescaler
                        InternalSetParam(inst, item, C_PRESCALER_OPTIONS, item.GetPrescalerOptions());

                        //Set IdacOptions
                        InternalSetParam(inst, item, C_IDAC_OPTIONS, (item.GetIdacOptions()).ToString());

                        //Set Aliases
                        int alias_count = m_cyWidgetsList.GetListTerminals(item.m_side).Count;
                        for (int i = 0; i < alias_count; i++)
                        {
                            InternalSetParam(inst, item, C_ALIAS_SNS + i,
                                m_cyWidgetsList.GetListTerminals(item.m_side)[i].ToString());
                        }
                        if (item.IsShieldElectrode())
                            for (int i = alias_count; i < alias_count + item.CountShieldElectrode; i++)
                            {
                                InternalSetParam(inst, item, C_ALIAS_SNS + i, GetAliasShieldByIndex(item, 
                                    i - alias_count));
                            }

                        //Set Rb
                        for (int i = 0; i < item.CountRb; i++)
                        {
                            InternalSetParam(inst, item, C_ALIAS_RB + i, GetAliasRbByIndex(item, i));
                        }
                        InternalSetParam(inst, item, C_ALIAS_CMOD, GetPrefixForSchematic(item) + C_ALIAS_CMOD_VAL);

                        //Set VrefOptions
                        InternalSetParam(inst, item, C_VREF_OPTIONS, item.m_VrefOptions.ToString());
                    }
            }
        }
        public void SetCommitParams(object sender, EventArgs e)
        {
            if (GlobalEditMode)
            {
                GlobalEditMode = false;
                SetParams(m_edit);
                if (m_edit != null)
                    if (m_edit.CommitParamExprs() == false)
                    {
                        MessageBox.Show("Error in Committing Parameters");
                    }
                GlobalEditMode = true;
            }
        }
        //Occurs for input widget tabs
        public void SetCommitParamsForWidgetsTabs(object sender, EventArgs e)
        {
            if (GlobalEditMode)
            {
                GlobalEditMode = false;
                m_cyScanSlotTab.UpdateWidgetsDataGrids(sender);
                SetCommitParams(sender, e);
                GlobalEditMode = true;
            }
        }
        #endregion

        #region Service Functions
        string SetEnum(string str, ICyInstEdit_v1 inst)
        {
            return str;
        }
        public string GetPrefixForSchematic(CyAmuxBParams item)
        {
            if (Configuration == E_MAIN_CONFIG.emSerial) return "s";
            if (item.m_side == E_EL_SIDE.Right) return "r";
            if (item.m_side == E_EL_SIDE.Left) return "l";
            return "";
        }
        public string GetSidePrefix(CyAmuxBParams item)
        {
            if (item.m_side == E_EL_SIDE.Right) return "r";
            if (item.m_side == E_EL_SIDE.Left) return "l";
            return "";
        }
        public string GetMethodPrefixForImage(CyAmuxBParams item)
        {
            if (item.m_Method == E_CAPSENSE_MODE.CSA) return "_csa";
            if (item.m_Method == E_CAPSENSE_MODE.CSD) return "_csd";
            return "";
        }
        public string GetPrefixForClock(CyAmuxBParams item)
        {
            if (Configuration == E_MAIN_CONFIG.emSerial) return "s";
            if (Configuration == E_MAIN_CONFIG.emParallelSynchron) return "l";
            if (item.m_side == E_EL_SIDE.Right) return "r";
            if (item.m_side == E_EL_SIDE.Left) return "l";
            return "";
        }

        void InternalSetParam(ICyInstEdit_v1 inst, CyAmuxBParams item, string name, string value)
        {
            string Prefix = GetSidePrefix(item);
            inst.SetParamExpr(Prefix + name, value);
        }
        string InternalGetEnumParam(ICyInstQuery_v1 inst, CyAmuxBParams item, string name)
        {
            string Prefix = GetSidePrefix(item);
            string res = inst.GetCommittedParam(Prefix + name).Expr.ToString();
            CyLocalParams.RemovePrefix(ref res, CyLocalParams.m_Conponent_prefix);
            return res;
        }
        string InternalGetParam(ICyInstQuery_v1 inst, CyAmuxBParams item, string name)
        {
            string Prefix = GetSidePrefix(item);
            return inst.GetCommittedParam(Prefix + name).Value.ToString();
        }
        object InternalGetParam(ICyInstQuery_v1 inst, CyAmuxBParams item, string name, Type _type)
        {
            return Enum.Parse(_type, InternalGetEnumParam(inst, item, name));
        }
        public string GetLeftLabel()
        {
            if (Configuration == E_MAIN_CONFIG.emSerial)
            {
                return "";
            }
            else
                return "Left " + STR_AB_HEAD;
        }
        public static void AssingActions(Control cnt, EventHandler handler)
        {
            foreach (Control item in cnt.Controls)
            {
                if ((item.GetType() == typeof(TextBox)))
                {
                    item.Validated += handler;
                }
                if (item.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)item).SelectedIndexChanged += handler;
                }
                if (item.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)item).CheckedChanged += handler;
                }
                if (item.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)item).CheckedChanged += handler;
                }
            }
        }
        #endregion
    }
    #endregion
}


