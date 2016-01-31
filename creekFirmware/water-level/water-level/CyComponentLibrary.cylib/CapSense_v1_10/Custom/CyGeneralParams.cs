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


namespace  CapSense_v1_10
{
    #region Enums
    public enum E_EL_SIDE { None=-1, Left=0, Right=1 }


    public enum E_ENE_DIS { Disabled = 0, Enabled = 1 };

    #region eCapSenseMode
    public enum E_CAPSENSE_MODE
    {
        CSD, CSA, None
    }
    public static class CyStrCapSenseMode
    {
        public  static string[]  m_strCMode = new string[2] { "CSD", "CSA"};
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

    public static class CySensorType
    {
        public static string GetBaseType(E_SENSOR_TYPE type)
        {
            string res=type.ToString().ToUpper();

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
            return !IsWidgetLabel(item);
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
        public static E_EL_SIDE[] m_sideMass=new E_EL_SIDE[]{E_EL_SIDE.Left,E_EL_SIDE.Right};
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
        public static string m_strABHead = "Half";

        [NonSerialized()]
        [XmlIgnore()]
        public static string m_strABHeadSS = "AMUX Bus Scan Slot Assignment";

        [NonSerialized()]
        [XmlIgnore()]
        public static String[] m_strSliderType = new string[]    { "Linear",
            "Radial"};
        public static String GetStrSliderType(E_SENSOR_TYPE st)
        {
            String res = "";
            if (st == E_SENSOR_TYPE.Linear_Slider) res = m_strSliderType[0];
            if (st == E_SENSOR_TYPE.Radial_Slider) res = m_strSliderType[1];
            return res;

        }
        public static E_SENSOR_TYPE GetEnumSliderType(String st)
        {
            E_SENSOR_TYPE res = E_SENSOR_TYPE.Button;
            if (st ==m_strSliderType[0] ) res = E_SENSOR_TYPE.Linear_Slider;
            if (st == m_strSliderType[1]) res = E_SENSOR_TYPE.Radial_Slider;
            return res;

        }

        [NonSerialized()]
        [XmlIgnore()]
        public static Color m_ColorLeft = Color.LightSteelBlue;

        [NonSerialized()]
        [XmlIgnore()]
        public static Color m_ColorRight = Color.Khaki;

        #endregion

        
        #region Controls

        [NonSerialized()]
        [XmlIgnore()]        
        public CyGeneralTab m_cyGeneralTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyClockSwitch m_cyClockSource;

        [NonSerialized()]
        [XmlIgnore()]
        public CyButtons m_cyButtons;

        [NonSerialized()]
        [XmlIgnore()]
        public CySliders m_cySliders;

        [NonSerialized()]
        [XmlIgnore()]
        public CyGeneric m_cyGeneric;

        [NonSerialized()]
        [XmlIgnore()]
        public CyMatrixButtons m_cyMatrixButtons;

        [NonSerialized()]
        [XmlIgnore()]
        public CyProximity m_cyProximity;

        [NonSerialized()]
        [XmlIgnore()]
        public CyTouchPads m_cyTouchPads;

        [NonSerialized()]
        [XmlIgnore()]
        public CyScanSlots m_cyScanSlots;
        #endregion


        [NonSerialized()]
        [XmlIgnore()]
        public ICyInstEdit_v1 m_edit;

        [NonSerialized()]
        [XmlIgnore()]
        public bool m_bGlobalEditMode = false;

        [XmlElement("localParams")]
        public CyLocalParams m_localParams = new CyLocalParams();

        [XmlIgnore()]
        public E_MAIN_CONFIG Configuration
        {
            get { return m_localParams.m_configuration; }
            set
            {
                m_localParams.m_configuration = value;
                if (m_cyScanSlots != null)
                m_cyScanSlots.UpdateAllWidgetsInDataGrids();
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
            //if ((row < 0) || (col < 0)) return "";
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
        }
        
        public IEnumerable<UserControl> GetAllTabsWidgets()
        {
            yield return m_cyButtons;
            yield return m_cyGeneric;
            yield return m_cyMatrixButtons;
            yield return m_cyProximity;
            yield return m_cySliders;
            yield return m_cyTouchPads;            
        }
        public IEnumerable<UserControl> GetAllTabsWithOutWidgets()
        {
            yield return m_cyScanSlots;
            yield return m_cyGeneralTab;
            yield return m_cyClockSource;
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
            foreach (UserControl item in GetAllTabsWidgets())
            {
                item.Leave += new System.EventHandler(SetCommitParamsWidgetsTab);
                ((CyMyICyParamEditTemplate)item).m_actGlobalEditorGetErrors += 
                    new EventHandler(SetCommitParamsWidgetsTab);   
            }
            foreach (UserControl item in GetAllTabsWithOutWidgets())
            {
                item.Leave += new System.EventHandler(SetCommitParams);
                ((CyMyICyParamEditTemplate)item).m_actGlobalEditorGetErrors += new EventHandler(SetCommitParams);   
            }
            m_bGlobalEditMode = true;
        }
            

        public CyScanSlots CreateScanSlots()
        {
            m_cyScanSlots = new CyScanSlots(this);
            return m_cyScanSlots;
        }

        public bool IsAmuxBusBlank(E_EL_SIDE side)
        {
            if (m_cyScanSlotsList.GetListFromSide(side).Count > 0)
                return false;
            return true;
        }
        #endregion

        #region Save/Load

        #region Binary Serialization

        public static byte[] GetByteArrayWithObject(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }


        public static CyGeneralParams GetObjectWithByteArray(byte[] theByteArray)
        {
            CyGeneralParams res;

            try
            {
                if (theByteArray == null) throw new Exception();

                MemoryStream ms = new MemoryStream(theByteArray);
                BinaryFormatter bf1 = new BinaryFormatter();
                ms.Position = 0;

                res = (CyGeneralParams)bf1.Deserialize(ms);
            }
            catch { res = new CyGeneralParams(); }

            res.m_cyScanSlotsList.m_Base = res;
            res.m_cyWidgetsList.m_Base = res;

            return res;
        }

        #endregion

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

        public static CyGeneralParams Deserialize(string serializedXml,bool _editparams)
        {
            bool _showmessage=false;
            CyGeneralParams res = null;
            try
            {
                System.Xml.Serialization.XmlSerializer x =
                    new System.Xml.Serialization.XmlSerializer(typeof(CyGeneralParams));
                TextReader tr = new StringReader(serializedXml);
                res = (CyGeneralParams)x.Deserialize(tr);

                _showmessage = _editparams;
                // Post Serialization Events
                res.PostSerializationIMyExecute(res);
                res.PostSerializationCs();
            }
            catch
            {
                if (_showmessage)
                    MessageBox.Show("CapSense settings have incorrect values! " +
                        "Please redefine it for proper CapSense operation."
                        , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                res = new CyGeneralParams();
            }
            res.Initialization();

            return res;
        }
        public void PostSerializationIMyExecute(object obj)
        {
            if (obj != null)
                if(obj.GetType()!=typeof(string))
                    if (obj.GetType().IsClass)
            {
                Type IMyPostSerializationType;
                //We get the array of fields for the new type instance.
                FieldInfo[] fields = obj.GetType().GetFields();

                int i = 0;

                foreach (FieldInfo fi in fields)
                    if(!fi.IsNotSerialized)
                {
                    ExecutePostSerialization(fi, obj);
                    //Now we check if the object support the IEnumerable interface, so if it does
                    Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
                    if (IEnumerableType != null)
                    {
                        //Get the IEnumerable interface from the field.
                        IEnumerable IEnum = (IEnumerable)fi.GetValue(obj);

                        //This version support the IList and the IDictionary interfaces to iterate
                        //on collections.
                        Type IListType = fields[i].FieldType.GetInterface("IList", true);
                        Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

                        if (IListType != null)
                        {
                            //Getting the IList interface.
                            IList list = (IList)fields[i].GetValue(obj);

                            foreach (object item in IEnum)
                            {
                                //Checking to see if the current item support the ICloneable interface.
                                IMyPostSerializationType = item.GetType().GetInterface("ICyMyPostSerialization", true);
                                if (IMyPostSerializationType != null)
                                {
                                    //Getting the IMyPostSerialization interface from the object.
                                    ICyMyPostSerialization IField = (ICyMyPostSerialization)item;

                                    IField.ExecutePostSerialization();
                                }
                                PostSerializationIMyExecute(item);
                            }
                        }
                    }
                    else if (fi.FieldType.IsSerializable)
                    {
                        if (obj.GetType() != (fi.FieldType))
                            PostSerializationIMyExecute(fi.GetValue(obj));
                    }

                    i++;
                }
            }
        }
        void ExecutePostSerialization(FieldInfo fi,object obj)
        {
            Type IMyPostSerializationType = fi.FieldType.GetInterface("ICyMyPostSerialization", true);

            if (IMyPostSerializationType != null)
            {
                //Getting the IMyPostSerialization interface from the object.
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
        enum CyClockOptions { Clock_Bus, Clock_Auto };
        //Local parameters
        public const string p_instanceNameParam = "INSTANCE_NAME";
        public const string p_XMLMainData = "XMLMainData";
        public const string p_Clk = "Clk";
        public const string p_ClockSource = "ClockSource";
        public const string p_SyncMode = "SyncMode";
        public const string p_ScanMode = "ScanMode";
        public const string p_PrescalerOptions = "PrescalerOptions";
        public const string p_IdacOptions = "IdacOptions";
        public const string p_PrsOptions = "PrsOptions";
        public const string p_RbNumber = "RbNumber";
        public const string p_SensorNumber = "SensorNumber";
        public const string p_ShieldNumber = "ShieldNumber";
        public const string p_AliasSns = "AliasSns";
        public const string p_AliasRb = "AliasRb";
        public const string p_AliasCmod = "AliasCmod";

        //Parameter values
        public const string m_str_AliasShield = "Shield";        
        public const string m_str_AliasRb = "Rb";
        public const string m_str_AliasCmod = "Cmod";

        public string GetAliasShieldByIndex(CyAmuxBParams item,  int i)
        {
            return GetPrefixForSchematic(item) + m_str_AliasShield + i;
        }
        public string GetAliasRbByIndex(CyAmuxBParams item, int i)
        {
            return GetPrefixForSchematic(item) + m_str_AliasRb + i;
        }

        #endregion

        public void GetParams(ICyInstEdit_v1 inst)
        {
            m_cyClockSource.m_cntClkL.GetProperties(m_localParams.m_listCsHalfs[0], this);
            m_cyClockSource.m_cntClkR.GetProperties(m_localParams.m_listCsHalfs[1], this);
        }

        public void SetParams(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                //Insert Global Parameters      
                string strXML = Serialize(this);
                inst.SetParamExpr(p_XMLMainData, strXML);

                inst.SetParamExpr(p_SyncMode, m_localParams.GetSyncModeOption());

                inst.SetParamExpr(p_ScanMode, m_localParams.GetScanModeOption());

                foreach (CyAmuxBParams item in m_localParams.m_listCsHalfs)
                    if (m_localParams.BCsHalfIsEnable(item))
                    {
                        //Set Rb
                        if (item.m_csdSubMethod == E_CSD_SUB_METHODS.IDACDisable_RB)
                        {
                            InternalSetParam(inst, item, p_RbNumber, item.CountRb.ToString());
                        }
                        else
                        {
                            InternalSetParam(inst, item, p_RbNumber, "0");
                        }

                        //Set ShieldElectrode
                        if (item.IsShieldElectrode())
                            InternalSetParam(inst, item, p_ShieldNumber, item.CountShieldElectrode.ToString());
                        else
                            InternalSetParam(inst, item, p_ShieldNumber, "0");

                        //Set PRSResolution
                        InternalSetParam(inst, item, p_PrsOptions, SetEnum(item.GetPRSResolutionOption(), inst));

                        //Set Terminal Count
                        int termCount = m_cyWidgetsList.GetListTerminalsFromSide(item.m_side).Count;
                        //if(termCount>0)
                        InternalSetParam(inst, item, p_SensorNumber, termCount.ToString());

                        //Set Clk                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                        InternalSetParam(inst, item, p_Clk, item.m_currectClk.m_ActualFreq.ToString());
                        if (item.m_currectClk.IsDirectClock())
                            InternalSetParam(inst, item, p_ClockSource, CyClockOptions.Clock_Bus.ToString());
                        else
                            InternalSetParam(inst, item, p_ClockSource, CyClockOptions.Clock_Auto.ToString());
                        //InternalSetParam(inst, item, "DirectClock",SetBool(item.m_currectClk.IsDirectClock()));                        
                        //InternalSetParam(inst, item, "ClockId", item.m_currectClk.m_ClockID);
                        //InternalSetParam(inst, item, "ClockName", item.m_currectClk.m_ClockName);
                        //InternalSetParam(inst, item, "ClockIsFr", SetBool(item.m_currectClk.IsDirectClock()==false));

                        //Set Prescaler
                        InternalSetParam(inst, item, p_PrescalerOptions, item.GetPrescalerOptions());

                        //Set IdacOptions
                        InternalSetParam(inst, item, p_IdacOptions, (item.GetIdacOptions()).ToString());

                        //Set Aliases
                        int alias_count = m_cyWidgetsList.GetListTerminalsFromSide(item.m_side).Count;
                        for (int i = 0; i < alias_count; i++)
                        {
                            InternalSetParam(inst, item, p_AliasSns + i,
                                m_cyWidgetsList.GetListTerminalsFromSide(item.m_side)[i].ToString());
                        }
                        if (item.IsShieldElectrode())
                            for (int i = alias_count; i < alias_count + item.CountShieldElectrode; i++)
                            {
                                InternalSetParam(inst, item, p_AliasSns + i, GetAliasShieldByIndex(item, i - alias_count));
                            }

                        //Set Rb
                        for (int i = 0; i < item.CountRb; i++)
                        {
                            InternalSetParam(inst, item, p_AliasRb + i, GetAliasRbByIndex(item, i));
                        }
                        InternalSetParam(inst, item, p_AliasCmod, GetPrefixForSchematic(item) + m_str_AliasCmod);
                    }

            }
        }
        public void SetCommitParams(object sender, EventArgs e)
        {
            if (m_bGlobalEditMode)
            {
                SetParams(m_edit);
                CommitParams(m_edit);
            }
        }
        //Occurs for input widget tabs
        public void SetCommitParamsWidgetsTab(object sender, EventArgs e)
        {
            if (m_bGlobalEditMode)
            {
                m_cyScanSlots.UpdateWidgetsDataGrids(sender);
                SetParams(m_edit);
                CommitParams(m_edit);
            }
        }
        public void CommitParams(ICyInstEdit_v1 inst)
        {
            if ((inst != null) && (!inst.CommitParamExprs()))
            {                
                MessageBox.Show("Error in Committing Parameters");
            }
        }

        #endregion

        #region Service Functions
        string SetBool(bool bl)
        {
            return Convert.ToInt16(bl).ToString();
        }
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
        public string GetPrefixForParams(CyAmuxBParams item)
        {            
            if (item.m_side == E_EL_SIDE.Right) return "r";
            if (item.m_side == E_EL_SIDE.Left) return "l";
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

        Dictionary<string, string> m_dict1 = new Dictionary<string, string>();
        void InternalSetParam(ICyInstEdit_v1 inst, CyAmuxBParams item, string name, string value)
        {
            string Prefix = GetPrefixForParams(item);
            inst.SetParamExpr(Prefix + name, value);
        }
        public string GetLeftLabel()
        {
            if (Configuration == E_MAIN_CONFIG.emSerial)
            {
                return "";
            }
            else
                return "Left " + m_strABHead;
        }
 
        #endregion
    }
    #endregion
}


