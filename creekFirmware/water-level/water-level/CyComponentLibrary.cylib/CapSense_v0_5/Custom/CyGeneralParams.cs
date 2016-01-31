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


namespace  CapSense_v0_5
{
    #region Enums
    public enum eElSide { None=-1, Left=0, Right=1 }


    public enum TEneDis { Disabled = 0, Enabled = 1 };

    #region eCapSenseMode
    public enum eCapSenseMode
    {
        CSD, CSA, None
    }
    public static class strCapSenseMode
    {
        public  static string[]  strCMode = new string[3] { "CSD", "CSA", "None" };
        public static string GetStr(eCapSenseMode en)
        {
            switch (en)
            {
                case eCapSenseMode.CSD:
                    return "CSD";
                //break;
                case eCapSenseMode.CSA:
                    return "CSA";
                //break;
                default:
                    break;
            }
            return "None";
        }
        public static eCapSenseMode GetEnum(string str)
        {
            foreach (string item in strCMode)
                for (int i = 0; i < strCMode.Length; i++)
                {
                    if (strCMode[i] == str)
                        return (eCapSenseMode)i;
                }
            return eCapSenseMode.None;

        }
    }
    #endregion

    #region sensorType
    public enum sensorType
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

    public static class cySensorType
    {
        public static string GetBaseType(sensorType type)
        {
            string res=type.ToString().ToUpper();

            if (GetBothParts(type).Count > 1)
            {
                return res.Substring(0, res.Length - 4);//Delete "_Col" or "_Row"
            }
            return res;
        }

        public static bool IsCustomCase(sensorType type)
        {
            return (type == sensorType.Generic) || (type == sensorType.Proximity);
        }
        public static bool IsWidgetLabel(ElTerminal item)
        {
            return IsCustomCase(item.type) && (item.Nameindex == -1);
        }
        public static bool IsRow(sensorType type)
        {
            if ((type == sensorType.Matrix_Buttons_Row)) return true;
            if ((type == sensorType.Touchpad_Row)) return true;
            return false;
        }
        public static bool IsTouchPad(sensorType type)
        {
            if ((type == sensorType.Touchpad_Row)) return true;
            if ((type == sensorType.Touchpad_Col)) return true;
            return false;
        }
        public static bool IsSlider(sensorType type)
        {
            if ((type == sensorType.Linear_Slider)) return true;
            if ((type == sensorType.Radial_Slider)) return true;
            return false;
        }
        public static bool IsButtonsStrc(Type type)
        {
            if ((type == typeof(ElUnButton)) || ((type == typeof(ElUnMatrixButton)))) return true;
            return false;
        }
        public static bool IsArrayCase(sensorType type)
        {
            if ((type == sensorType.Matrix_Buttons_Row)) return true;
            if ((type == sensorType.Matrix_Buttons_Col)) return true;
            if ((type == sensorType.Touchpad_Col)) return true;
            if ((type == sensorType.Touchpad_Row)) return true;
            return false;
        }
        public static List<sensorType> GetBothParts(sensorType type)//!=MB, !=TB
        {
            List<sensorType> res = new List<sensorType>();
            res.Add(type);
            if ((type == sensorType.Matrix_Buttons_Row) || (type == sensorType.Matrix_Buttons_Col))
            {
                res.Clear();
                res.Add(sensorType.Matrix_Buttons_Col);
                res.Add(sensorType.Matrix_Buttons_Row);
            }
            if ((type == sensorType.Touchpad_Col) || (type == sensorType.Touchpad_Row))
            {
                res.Clear();
                res.Add(sensorType.Touchpad_Col);
                res.Add(sensorType.Touchpad_Row);
            }
            return res;
        }
    }

    #endregion

    #endregion

    #region CyGeneralParams

    [XmlRootAttribute("CyGeneralParams")]

    [Serializable()]
    public class CyGeneralParams //: ICyCustomData_v1
    {
        #region Head

        #region Const Properties
        public static eElSide[] sideMass=new eElSide[]{eElSide.Left,eElSide.Right};
        [NonSerialized()]
        [XmlIgnore()]
        public static string[] strWidgetTypes = new string[] {
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
        public static string strABHead = "Half";

        [NonSerialized()]
        [XmlIgnore()]
        public static string strABHeadSS = "AMUX Bus Scan Slot Assignment";

        [NonSerialized()]
        [XmlIgnore()]
        public static String[] strSliderType = new string[]    { "Linear",
            "Radial"};
        public static String GetStrSliderType(sensorType st)
        {
            String res = "";
            if (st == sensorType.Linear_Slider) res = strSliderType[0];
            if (st == sensorType.Radial_Slider) res = strSliderType[1];
            return res;

        }
        public static sensorType GetEnumSliderType(String st)
        {
            sensorType res = sensorType.Button;
            if (st ==strSliderType[0] ) res = sensorType.Linear_Slider;
            if (st == strSliderType[1]) res = sensorType.Radial_Slider;
            return res;

        }

        [NonSerialized()]
        [XmlIgnore()]
        public static Color ColorLeft = Color.LightSteelBlue;

        [NonSerialized()]
        [XmlIgnore()]
        public static Color ColorRight = Color.Khaki;

        #endregion

        
        #region Controls

        [NonSerialized()]
        [XmlIgnore()]        
        public CyGeneralTab cyGeneralTab;

        [NonSerialized()]
        [XmlIgnore()]
        public CyClockSwitch cyClockSource;

        [NonSerialized()]
        [XmlIgnore()]
        public CyButtons cyButtons;

        [NonSerialized()]
        [XmlIgnore()]
        public CySliders cySliders;

        [NonSerialized()]
        [XmlIgnore()]
        public CyGeneric cyGeneric;

        [NonSerialized()]
        [XmlIgnore()]
        public CyMatrixButtons cyMatrixButtons;

        [NonSerialized()]
        [XmlIgnore()]
        public CyProximity cyProximity;

        [NonSerialized()]
        [XmlIgnore()]
        public CyTouchPads cyTouchPads;

        [NonSerialized()]
        [XmlIgnore()]
        public CyScanSlots cyScanSlots;

      
        #endregion


        [NonSerialized()]
        [XmlIgnore()]
        public ICyInstEdit_v1 edit;


        public CyLocalParams localParams = new CyLocalParams();

        [XmlIgnore()]
        public eMConfiguration Configuration
        {
            get { return localParams.configuration; }
            set
            {
                localParams.configuration = value;
                if (cyScanSlots != null)
                cyScanSlots.UpdateAllWidgetsInDataGrids();
            }
        }

     

        public CyScanSlotsList cyScanSlotsList;


        public CyWidgetsList cyWidgetsList;

        public int getRawDataOffset(ElWidget wi)
        {
            int i = 0;
            foreach (ElScanSlot item in cyScanSlotsList.GetScanSlotsSorted())
            {
                if (item.Widget == wi) return i;
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
        public static bool isSameArrays(object[] NewRow, object[] LastRow)
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
            cyScanSlotsList = new CyScanSlotsList(this);
            cyWidgetsList = new CyWidgetsList(this);
            localParams.FirstInitialszation();
        }
       public void Initialization()
        {
            cyScanSlotsList.Base = this;
            cyWidgetsList.Base = this;
        }
        
        public IEnumerable<UserControl> GetAllTabsWidgets()
        {
            yield return cyButtons;
            yield return cyGeneric;
            yield return cyMatrixButtons;
            yield return cyProximity;
            yield return cySliders;
            yield return cyTouchPads;            
        }
        public IEnumerable<UserControl> GetAllTabsWithOutWidgets()
        {
            yield return cyScanSlots;
            yield return cyGeneralTab;
            yield return cyClockSource;
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
                ((M_ICyParamEditTemplate)item).actGlobalEditorGetErrors += new EventHandler(SetCommitParamsWidgetsTab);   
            }
            foreach (UserControl item in GetAllTabsWithOutWidgets())
            {
                item.Leave += new System.EventHandler(SetCommitParams);
                ((M_ICyParamEditTemplate)item).actGlobalEditorGetErrors += new EventHandler(SetCommitParams);   
            }
        }
            

        public CyScanSlots createScanSlots()
        {
            cyScanSlots = new CyScanSlots(this);
            return cyScanSlots;
        }
        #endregion

        #region Save/Load

        #region Binary Serialization

        public static byte[] getByteArrayWithObject(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }


        public static CyGeneralParams getObjectWithByteArray(byte[] theByteArray)
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

            res.cyScanSlotsList.Base = res;
            res.cyWidgetsList.Base = res;

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

        public static CyGeneralParams Deserialize(string serializedXml)
        {
            CyGeneralParams res = null;
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(CyGeneralParams));
                TextReader tr = new StringReader(serializedXml);
                res = (CyGeneralParams)x.Deserialize(tr);
            }
            catch
            {
                res = new CyGeneralParams();
            }
            // Post Serialization Events
            res.PostSerialization(res);

            res.CsPostSerialization();
            return res;
        }
        public void PostSerialization(object obj)
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
                                IMyPostSerializationType = item.GetType().GetInterface("IMyPostSerialization", true);
                                if (IMyPostSerializationType != null)
                                {
                                    //Getting the IMyPostSerialization interface from the object.
                                    IMyPostSerialization IField = (IMyPostSerialization)item;

                                    IField.ExecutePostSerialization();
                                }
                                PostSerialization(item);
                            }
                        }
                        //else if (IDicType != null)
                        //{
                        //    //Getting the dictionary interface.
                        //    IDictionary dic = (IDictionary)fields[i].GetValue(obj);
                        //    j = 0;
                        //    foreach (DictionaryEntry de in IEnum)
                        //    {
                        //        //Checking to see if the item support the ICloneable interface.
                        //        ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

                        //        if (ICloneType != null)
                        //        {
                        //            ICloneable clone = (ICloneable)de.Value;

                        //            dic[de.Key] = clone.Clone();
                        //        }
                        //        j++;
                        //    }
                        //}
                    }
                    else if (fi.FieldType.IsSerializable)
                    {
                        if (obj.GetType() != (fi.FieldType))
                            PostSerialization(fi.GetValue(obj));
                    }

                    i++;
                }
            }
        }
        void ExecutePostSerialization(FieldInfo fi,object obj)
        {
            Type IMyPostSerializationType = fi.FieldType.GetInterface("IMyPostSerialization", true);

            if (IMyPostSerializationType != null)
            {
                //Getting the IMyPostSerialization interface from the object.
                IMyPostSerialization IField = (IMyPostSerialization)fi.GetValue(obj);

                IField.ExecutePostSerialization();
            }
        }
        #endregion

        #region Pre/Post Serialization Events
        public void PreSerialization()
        {
            foreach (ElScanSlot item in cyScanSlotsList.listScanSlotsL)
            {
                item.PreSerialization();
            }
            foreach (ElScanSlot item in cyScanSlotsList.listScanSlotsR)
            {
                item.PreSerialization();
            }
        }
        public void CsPostSerialization()
        {
            psRestoreSS(eElSide.Left, cyScanSlotsList.listScanSlotsL);
            psRestoreSS(eElSide.Right, cyScanSlotsList.listScanSlotsR);
            Initialization();
        }
        public void psRestoreSS(eElSide side, List<ElScanSlot> listSS)
        {
            foreach (ElScanSlot item in listSS)
            {
                //Initial Index
                item.Index.index = cyScanSlotsList.NextIndex(side);
                //Add terminal list to scanslot
                foreach (string itemInt in item.srlistTerminalsNames)
                {
                    item.listTerminals.Add(cyWidgetsList.FindTerminal(itemInt));
                }

                //Add Widget
                item.Widget = cyWidgetsList.FindWidget(item.srWidgetName);
                if (item.Widget == null)
                {
                    item.Widget = cyWidgetsList.FindWidget(item.srWidgetName);
                    item.Widget = cyWidgetsList.FindWidgetOldPattern(item.srWidgetName);
                }
                //Add ssprops from widget
                item.SSProperties.baseProps = item.Widget.baseSSProps;
            }
        }

        #endregion

        #endregion

        #region Get/Set params

        #region Parameters names

        //Local parameters
        public const string p_instanceNameParam = "INSTANCE_NAME";
        public const string p_XMLMainData = "XMLMainData";
        public const string p_Clk = "Clk";
        public const string p_HalfEnable = "HalfEnable";
        public const string p_IDACEnable = "IDACEnable";
        public const string p_Sync = "Sync";
        public const string p_PrescalerEnable = "PrescalerEnable";
        public const string p_PrescalerUDB = "PrescalerUDB";
        public const string p_IDACMode = "IDACMode";
        public const string p_PRSResolution = "PRSResolution";
        public const string p_Rb0Enable= "Rb0Enable";
        public const string p_Rb1Enable = "Rb1Enable";
        public const string p_Rb2Enable = "Rb2Enable";
        public const string p_SensorNumber = "SensorNumber";
        public const string p_ShieldNumber = "ShieldNumber";
        public const string p_SerialModeEnable = "SerialModeEnable";
        public const string p_ShieldEnable = "ShieldEnable";
        public const string p_AliasSns = "AliasSns";
        public const string p_AliasShield = "AliasShield";
        public const string p_AliasRb = "AliasRb";
        public const string p_AliasCmod= "AliasCmod";

        //public const string p_ = "";

        //Parameter values
        public const string str_AliasShield = "Shield";        
        public const string str_AliasRb = "Rb";
        public const string str_AliasCmod = "Cmod";

        public string GetAliasShieldByIndex(CyAmuxBParams item,  int i)
        {
            return GetPrefixForSchematic(item) + str_AliasShield + i;
        }
        public string GetAliasRbByIndex(CyAmuxBParams item, int i)
        {
            return GetPrefixForSchematic(item) + str_AliasRb + i;
        }

        #endregion

        public void GetParams(ICyInstEdit_v1 inst)
        {
            cyClockSource.cntClkL.GetProperties(localParams.listCsHalfs[0], this);
            cyClockSource.cntClkR.GetProperties(localParams.listCsHalfs[1], this);
        }

        public void SetParams(ICyInstEdit_v1 inst)
        {
            if (inst != null)
            {
                //dict1.Clear();
                //Finish All DataGridsInput

                //Insert Global Parameters      
                string strXML = Serialize(this);
                inst.SetParamExpr(p_XMLMainData, strXML);

                inst.SetParamExpr(p_Sync, SetBool(Configuration == eMConfiguration.emParallelSynchron));

                inst.SetParamExpr(p_SerialModeEnable, SetBool(Configuration == eMConfiguration.emSerial));

                //Set Visibility
                if (Configuration == eMConfiguration.emSerial)
                {
                    inst.SetParamExpr("l" + p_HalfEnable, SetBool(true));//Left==Serial
                    inst.SetParamExpr("r" + p_HalfEnable, SetBool(false));
                }
                else
                {
                    inst.SetParamExpr("l" + p_HalfEnable, SetBool(localParams.bCsHalfIsEnable(localParams.listCsHalfs[0])));
                    inst.SetParamExpr("r" + p_HalfEnable, SetBool(localParams.bCsHalfIsEnable(localParams.listCsHalfs[1])));
                }


                foreach (CyAmuxBParams item in localParams.listCsHalfs)
                    if (localParams.bCsHalfIsEnable(item))
                    {
                        internalSetParam(inst, item, p_IDACEnable, SetBool(item.isIdac));
                        internalSetParam(inst, item, p_PRSResolution, item.GetPRSResolution());

                        //Set Rb
                        if (item.csdSubMethod == CSDSubMethods.IDACDisable_RB)
                        {
                            internalSetParam(inst, item, p_Rb0Enable, SetBool((item.countRb > 0) && (item.isRbEnable)));
                            internalSetParam(inst, item, p_Rb1Enable, SetBool(item.countRb > 1));
                            internalSetParam(inst, item, p_Rb2Enable, SetBool(item.countRb > 2));
                        }
                        else
                        {
                            internalSetParam(inst, item, p_Rb0Enable, SetBool(false));
                            internalSetParam(inst, item, p_Rb1Enable, SetBool(false));
                            internalSetParam(inst, item, p_Rb2Enable, SetBool(false));
                        }

                        //Set ShieldElectrode
                        internalSetParam(inst, item, p_ShieldEnable, SetBool(item.IsShieldElectrode()));
                        if (item.IsShieldElectrode())
                            internalSetParam(inst, item, p_ShieldNumber, item.countShieldElectrode.ToString());

                        //Set Terminal Count
                        int termCount = cyWidgetsList.GetListTerminalsFromSide(item.side).Count;
                        //if(termCount>0)
                        internalSetParam(inst, item, p_SensorNumber, termCount.ToString());

                        //Set Clk                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                        internalSetParam(inst, item, p_Clk, item.currectClk.ActualFreq.ToString());

                        //Set Prescaler
                        internalSetParam(inst, item, p_PrescalerEnable, SetBool(item.IsPrescaler()));
                        internalSetParam(inst, item, p_PrescalerUDB, SetBool(item.Prescaler== enPrescaler.UDB));

                        //Set IDACMode
                        internalSetParam(inst, item, p_IDACMode, SetBool(item.csdSubMethod == CSDSubMethods.IDACSinking));

                        //Set Aliases
                        for (int i = 0; i < cyWidgetsList.GetListTerminalsFromSide(item.side).Count; i++)
                        {
                            internalSetParam(inst, item, p_AliasSns + i, cyWidgetsList.GetListTerminalsFromSide(item.side)[i].ToString());
                        }

                        //Set Shield
                        for (int i = 0; i < item.countShieldElectrode; i++)
                        {
                            internalSetParam(inst, item, p_AliasShield + i, GetAliasShieldByIndex(item,i));
                        }

                        //Set Rb
                        for (int i = 0; i <item.countRb; i++)
                        {
                            internalSetParam(inst, item, p_AliasRb + i, GetAliasRbByIndex(item, i));
                        }
                        internalSetParam(inst, item, p_AliasCmod, GetPrefixForSchematic(item) + str_AliasCmod);
                    }

            }
        }
        public void SetCommitParams(object sender, EventArgs e)
        {
            SetParams(edit);
            CommitParams(edit);
        }
        //Occurs for input widget tabs
        public void SetCommitParamsWidgetsTab(object sender, EventArgs e)
        {
            cyScanSlots.UpdateWidgetsDataGrids(sender);
            SetParams(edit);
            CommitParams(edit);
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
        public string GetPrefixForSchematic(CyAmuxBParams item)
        {
            if (Configuration == eMConfiguration.emSerial) return "s";
            if (item.side == eElSide.Right) return "r";
            if (item.side == eElSide.Left) return "l";
            return "";
        }
        public string GetPrefixForParams(CyAmuxBParams item)
        {            
            if (item.side == eElSide.Right) return "r";
            if (item.side == eElSide.Left) return "l";
            return "";
        }
        public string GetPrefixForClock(CyAmuxBParams item)
        {
            if (Configuration == eMConfiguration.emSerial) return "s";
            if (Configuration == eMConfiguration.emParallelSynchron) return "l";
            if (item.side == eElSide.Right) return "r";
            if (item.side == eElSide.Left) return "l";            
            return "";
        }

        Dictionary<string, string> dict1 = new Dictionary<string, string>();
        void internalSetParam(ICyInstEdit_v1 inst, CyAmuxBParams item, string name, string value)
        {
            string Prefix = GetPrefixForParams(item);
            inst.SetParamExpr(Prefix + name, value);
            //dict1.Add(Prefix + name, value);
        }
        public string GetLeftLabel()
        {
            if (Configuration == eMConfiguration.emSerial)
            {
                return "";//strABHead;
            }
            else
                return "Left " + strABHead;
        }
 
        #endregion

    }
    #endregion




}


