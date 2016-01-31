/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;

namespace CapSense_CSD_v2_0
{
    #region CyWidgetsList
    [Serializable()]
    public class CyWidgetsList : ICyCSPostSerialization
    {
        #region Header
        [XmlArray("ListMainTerminal")]
        [XmlArrayItem("CyTerminal")]
        public List<CyTerminal> m_listTerminals = new List<CyTerminal>();

        [XmlArray("ListButtons")]
        [XmlArrayItem("CyButton")]
        public List<CyButton> m_listButtons = new List<CyButton>();
        [XmlArray("ListMatrixButtons")]
        [XmlArrayItem("CyMatrixButton")]
        public List<CyMatrixButton> m_listMatrixButtons = new List<CyMatrixButton>();
        [XmlArray("ListTouchPads")]
        [XmlArrayItem("CyTouchPad")]
        public List<CyTouchPad> m_listTouchPads = new List<CyTouchPad>();
        [XmlArray("ListSliders")]
        [XmlArrayItem("CySlider")]
        public List<CySlider> m_listSliders = new List<CySlider>();

        [XmlElement("GuardSensor")]
        public CyButton m_guardSensorWidget;
        [XmlElement("GuardSensorTerminal")]
        public CyTerminal m_guardSensorTerminal;

        [XmlElement("CyScanSlotsList")]
        public CyScanSlotsList m_scanSlots;

        [XmlIgnore]
        //This flag indicates that Widgets Data was changed
        public bool m_needUpdate = false;

        public CyWidgetsList()
        {
            m_scanSlots = new CyScanSlotsList(this);
        }
        public void ExecutePostSerialization()
        {
            m_scanSlots.m_widgets = this;
            try
            {
                if (m_guardSensorWidget == null)//Fiirst initialization of guard sensor
                {
                    m_guardSensorWidget = new CyButton(CyCsConst.P_GUARD_SENSOR, CySensorType.Button, null);
                    m_scanSlots.m_guardSensor = new CyScanSlot(m_guardSensorWidget);
                    m_guardSensorTerminal = new CyTerminal(m_guardSensorWidget, 0);
                }

                m_guardSensorTerminal.m_widget = m_guardSensorWidget;
                m_scanSlots.m_guardSensor.AddTerminal(m_guardSensorTerminal);
                m_scanSlots.m_guardSensor.m_widget = m_guardSensorWidget;

                //Assign base Widget for terminals
                List<CyTerminal> listTers = m_listTerminals;
                for (int i = 0; i < listTers.Count; i++)
                {
                    CyWidget wi = FindWidget(listTers[i].WidgetName);
                    if (wi != null)
                        listTers[i].m_widget = wi;
                    else
                        throw new Exception();
                }

                //Assign base Widget and connected terminals for ScanSlots                
                List<CyScanSlot> listSS = m_scanSlots.m_listScanSlots;
                for (int i = 0; i < listSS.Count; i++)
                {
                    CyScanSlot ss = listSS[i];
                    //Assign Widget
                    CyWidget wi = FindWidget(ss.WidgetName);
                    if (wi != null)
                        ss.m_widget = wi;
                    else
                        throw new Exception();

                    //Assign terminals list                
                    for (int j = 0; j < ss.GetListTerminalsDeserializedAlias().Count; j++)
                    {
                        CyTerminal term = FindTerminal(ss.GetListTerminalsDeserializedAlias()[j]);
                        if (term != null && (term.WidgetName == ss.WidgetName ||
                            CyCsConst.HasComplexScanSlot(ss.WidgetType)))
                            ss.AddTerminal(term);
                        else
                            throw new Exception();
                    }
                }
                //Assigne proper channel for Guard widget
                AssignGuardChannel();

                List<CyWidget> listWidgets = GetListWidgets();
                for (int i = 0; i < listWidgets.Count; i++)
                {
                    //Update WidgetSensorCountDelegat for all widgets
                    listWidgets[i].m_updateWidgetSensorCountDelegat = UpdateWidgetSensorCount;
                    //Synchronize complex widgets
                    if (CyCsConst.IsMainPartOfWidget(listWidgets[i].m_type) == false)
                    {
                        List<CyWidget> listParts = GetBothParts(listWidgets[i]);

                        if (listParts.Count != 2) throw new Exception(string.Empty);

                        if (listWidgets[i] is CyTouchPad)
                        {
                            (listWidgets[i] as CyTouchPad).m_propsCols = (listParts[0] as CyTouchPad).m_propsCols;
                            (listWidgets[i] as CyTouchPad).m_propsRows = (listParts[0] as CyTouchPad).m_propsRows;
                            (listWidgets[i] as CyTouchPad).m_positionFilter =
                                (listParts[0] as CyTouchPad).m_positionFilter;
                            (listWidgets[i] as CyTouchPad).m_colCount = (listParts[0] as CyTouchPad).m_colCount;
                            (listWidgets[i] as CyTouchPad).m_rowCount = (listParts[0] as CyTouchPad).m_rowCount;
                            (listWidgets[i] as CyTouchPad).m_colResolution =
                                (listParts[0] as CyTouchPad).m_colResolution;
                            (listWidgets[i] as CyTouchPad).m_rowResolution =
                                (listParts[0] as CyTouchPad).m_rowResolution;
                        }
                        if (listWidgets[i] is CyMatrixButton)
                        {
                            (listWidgets[i] as CyMatrixButton).m_propsCols =
                                (listParts[0] as CyMatrixButton).m_propsCols;
                            (listWidgets[i] as CyMatrixButton).m_propsRows =
                                (listParts[0] as CyMatrixButton).m_propsRows;
                            (listWidgets[i] as CyMatrixButton).m_colCount = (listParts[0] as CyMatrixButton).m_colCount;
                            (listWidgets[i] as CyMatrixButton).m_rowCount = (listParts[0] as CyMatrixButton).m_rowCount;
                        }
                    }
                }
                m_scanSlots.EraceFreeSS();

                //Calculate desired Scan Slot count base on widgets count
                int ss_count = listWidgets.Count;
                for (int i = 0; i < listWidgets.Count; i++)
                    if (listWidgets[i] is CyButton == false)
                    {
                        ss_count += listWidgets[i].GetCount() - 1;
                    }
                if (ss_count != m_scanSlots.m_listScanSlots.Count) throw new Exception();
            }
            catch
            {
                MessageBox.Show(CyCsResource.XmlDataIncorrect, CyCsResource.XmlDataIncorrectHeader);
            }
        }

        //Assigne proper channel for Guard widget
        public void AssignGuardChannel()
        {
            int ch0c = 0;
            int ch1c = 0;
            for (int i = 0; i < m_scanSlots.m_listScanSlots.Count; i++)
            {
                //Calculate scanslots on channels
                if (m_scanSlots.m_listScanSlots[i].Channel == CyChannelNumber.First) ch0c++;
                else ch1c++;
            }
            m_guardSensorWidget.m_channel = ch1c > ch0c ? CyChannelNumber.Second : CyChannelNumber.First;
        }
        #endregion

        #region Service work
        public List<CyWidget> GetListWidgets()
        {
            List<CyWidget> res = new List<CyWidget>();

            foreach (CyWidget item in m_listSliders)
                res.Add(item);
            foreach (CyWidget item in m_listTouchPads)
                res.Add(item);

            foreach (CyWidget item in m_listButtons)
                res.Add(item);

            foreach (CyWidget item in m_listMatrixButtons)
                res.Add(item);
            return res;
        }
        public List<CyWidget> GetListWidgetsByType(Type type)
        {
            if (type == typeof(CyButton)) return new List<CyWidget>(m_listButtons.ToArray());
            if (type == typeof(CySlider)) return new List<CyWidget>(m_listSliders.ToArray());
            if (type == typeof(CyTouchPad)) return new List<CyWidget>(m_listTouchPads.ToArray());
            if (type == typeof(CyMatrixButton)) return new List<CyWidget>(m_listMatrixButtons.ToArray());
            return new List<CyWidget>();
        }
        public CyWidget FindWidget(string name, CySensorType type)
        {
            foreach (CyWidget item in GetListWidgets())
                if (item.m_name == name)
                {
                    if (item.m_type == type)
                        return item;
                }
            if (type == m_guardSensorWidget.m_type && name == m_guardSensorWidget.m_name)
                return m_guardSensorWidget;
            return null;
        }
        public CyWidget FindWidget(string str)
        {
            foreach (CyWidget item in GetListWidgets())
                if (item.ToString() == str) return item;
            return null;
        }

        public CyTuningProperties GetWidgetsProperties(CyWidget wi)
        {
            List<CyWidget> list= GetBothParts(wi);          
            return (CyTuningProperties)list[0].GetAdditionalProperties()[list.IndexOf(wi)];
        }

        public IEnumerable<CyTerminal> GetTerminals(CyWidget wi)
        {
            foreach (CyTerminal item in m_listTerminals)
                if (item.m_widget == wi)
                {
                    yield return item;
                }
        }
        #endregion

        #region Add/Delete Widget
        void RemoveWidgetFromList(CyWidget wi)
        {
            if (wi != null)
                switch (wi.m_type)
                {
                    case CySensorType.Button:
                    case CySensorType.Proximity:
                    case CySensorType.Generic:
                        m_listButtons.Remove((CyButton)wi);
                        break;
                    case CySensorType.SliderLinear:
                    case CySensorType.SliderRadial:
                        m_listSliders.Remove((CySlider)wi);
                        break;
                    case CySensorType.TouchpadColumn:
                    case CySensorType.TouchpadRow:
                        m_listTouchPads.Remove((CyTouchPad)wi);
                        break;
                    case CySensorType.MatrixButtonsColumn:
                    case CySensorType.MatrixButtonsRow:
                        m_listMatrixButtons.Remove((CyMatrixButton)wi);
                        break;
                    default:
                        break;
                }
        }
        CyWidget AddWidgetToList(CyWidget wi)
        {
            foreach (CyWidget item in GetListWidgets())
            {
                if (item.IsSame(wi)) return item;
            }
            switch (wi.m_type)
            {
                case CySensorType.Button:
                case CySensorType.Proximity:
                case CySensorType.Generic:
                    m_listButtons.Add((CyButton)wi);
                    break;
                case CySensorType.SliderLinear:
                case CySensorType.SliderRadial:
                    m_listSliders.Add((CySlider)wi);
                    break;
                case CySensorType.TouchpadColumn:
                case CySensorType.TouchpadRow:
                    m_listTouchPads.Add((CyTouchPad)wi);
                    break;
                case CySensorType.MatrixButtonsColumn:
                case CySensorType.MatrixButtonsRow:
                    m_listMatrixButtons.Add((CyMatrixButton)wi);
                    break;
                default:
                    break;
            }
            return null;
        }

        public CyWidget AddWidget(string name, CySensorType type, CyChannelNumber channel, bool virtualWidget)
        {
            CyWidget wi = null;
            CyTerminal term;
            switch (type)
            {
                case CySensorType.Button:
                case CySensorType.Proximity:
                case CySensorType.Generic:
                    wi = new CyButton(name, type, UpdateWidgetSensorCount);
                    wi.m_channel = channel;
                    break;
                case CySensorType.SliderLinear:
                case CySensorType.SliderRadial:
                    wi = new CySlider(name, type, UpdateWidgetSensorCount);
                    wi.m_channel = channel;
                    break;
                case CySensorType.TouchpadColumn:
                case CySensorType.TouchpadRow:
                    wi = new CyTouchPad(name, type, UpdateWidgetSensorCount);
                    wi.m_channel = channel;
                    break;
                case CySensorType.MatrixButtonsColumn:
                case CySensorType.MatrixButtonsRow:
                    wi = new CyMatrixButton(name, type, UpdateWidgetSensorCount);
                    wi.m_channel = channel;
                    break;

                default:
                    return null;
            }
            if (wi != null && virtualWidget == false)// Is Real Widget
            {
                int count = wi.GetCount();
                if (AddWidgetToList(wi) == null)
                {
                    if (count > 0)
                        for (int j = 0; j < count; j++)
                        {
                            term = new CyTerminal(wi, j);
                            AddTerminal(term, wi);
                        }
                    m_scanSlots.AddScanSlotsRange(wi);
                }
                else return AddWidgetToList(wi);
            }
            m_needUpdate = true;
            return wi;
        }
        public void RemoveWidget(string widget_name)
        {
            CyWidget wi = FindWidget(widget_name);
            if (wi != null)
            {
                List<CyWidget> wiList = GetBothParts(wi);
                //Remove widget terminals
                foreach (CyWidget element in wiList)
                    if (element != null)
                    {
                        wi.m_isDeleted = true;
                        DeleteTerminals(element, 0, element.GetCount());//remove indexes terminals
                        //Remove Widget
                        RemoveWidgetFromList(element);
                    }
                m_scanSlots.EraceFreeSS();

                m_needUpdate = true;
            }
        }
        public void UpdateWidgetSensorCount(string name, CySensorType type, int oldCount, int newCount)
        {
            CyWidget wi = FindWidget(name, type);

            if (wi != null)
            {
                //Count change
                if (oldCount > newCount)
                //remove terminals
                {
                    DeleteTerminals(wi, newCount, oldCount);
                }
                if (oldCount < newCount)
                //append terminals
                {
                    for (int j = oldCount; j < newCount; j++)
                    {
                        CyTerminal term = new CyTerminal(wi, j);
                        AddTerminal(term, wi);
                        if (CyCsConst.HasComplexScanSlot(wi.m_type))
                        {
                            m_scanSlots.AppendTerminalForWidgetScanSlot(wi, term);
                        }
                        else
                        {
                            m_scanSlots.AddScanSlot(wi, term);
                        }
                    }
                    //Update terminal list order
                    SortListTerminals();
                }
                m_scanSlots.EraceFreeSS();

                m_needUpdate = true;
            }
        }

        #endregion

        #region Terminal Work
        public List<CyTerminal> GetListTerminals(CyChannelNumber channel)
        {
            List<CyTerminal> res = new List<CyTerminal>();
            SortListTerminals();
            for (int i = 0; i < m_listTerminals.Count; i++)
                if (m_listTerminals[i].Channel == channel)
                {
                    res.Add(m_listTerminals[i]);
                }
            return res;
        }
        public List<CyTerminal> GetListTerminalsSortedForAmux(CyCSSettings settings)
        {
            List<CyTerminal> res = new List<CyTerminal>();
            bool isComplexSS = m_scanSlots.IsComplexScanSlots();

            //Getting ScanSlot list
            List<CyScanSlot> listSS = null;
            if (settings.Configuration == CyChannelConfig.ONE_CHANNEL)
            {
                listSS = new List<CyScanSlot>(m_scanSlots.m_listScanSlots.ToArray());
                if (settings.m_guardSensorEnable)
                    listSS.Add(m_scanSlots.m_guardSensor);
            }
            else
            {
                listSS = m_scanSlots.GetSSList(CyChannelNumber.First);
                if (settings.m_guardSensorEnable && m_guardSensorWidget.m_channel == CyChannelNumber.First)
                    listSS.Add(m_scanSlots.m_guardSensor);
                listSS.AddRange(m_scanSlots.GetSSList(CyChannelNumber.Second));
                if (settings.m_guardSensorEnable && m_guardSensorWidget.m_channel == CyChannelNumber.Second)
                    listSS.Add(m_scanSlots.m_guardSensor);
            }

            //Sorting ScanSlot list in 
            for (int i = 0; i < listSS.Count; i++)
            {
                CyTerminal terminal = listSS[i].GetHeaderTerminal();
                System.Diagnostics.Debug.Assert(isComplexSS || terminal != null);
                if (terminal != null)
                    res.Add(terminal);
            }
            //Add missing terminals
            if (res.Count != (m_listTerminals.Count + Convert.ToInt16(settings.m_guardSensorEnable)))
            {
                System.Diagnostics.Debug.Assert(res.Count <= m_listTerminals.Count);
                for (int i = 0; i < m_listTerminals.Count; i++)
                    if (res.IndexOf(m_listTerminals[i]) == -1)
                    {
                        res.Add(m_listTerminals[i]);
                    }
            }
            return res;
        }
        CyTerminal AddTerminal(CyTerminal term, CyWidget wi)
        {
            term.m_widget = wi;
            if (TerminalInList(term))
            {
                m_listTerminals.Add(term);
            }
            return term;
        }
        bool TerminalInList(CyTerminal term)
        {
            foreach (CyTerminal item in m_listTerminals)
            {
                if (item.CompareTerminals(term)) return false;
            }
            return true;
        }
        CyTerminal FindTerminal(string str)
        {
            foreach (CyTerminal item in m_listTerminals)
                if (item.ToString() == str) return item;
            return null;
        }
        void DeleteTerminals(CyWidget wi, int startIndex, int count)
        {
            for (int nameIndex = startIndex; nameIndex < count; nameIndex++)
            {
                CyTerminal term = null;
                //Delete Terminal in ListTerminals
                int pos = 0;
                while (pos < m_listTerminals.Count)
                {
                    if (m_listTerminals[pos].m_widget == wi && m_listTerminals[pos].m_nameIndex == nameIndex)
                    {
                        term = m_listTerminals[pos];
                        m_listTerminals.RemoveAt(pos);
                        break;
                    }
                    else pos++;
                }
                //Delete Terminals in Scan Slots Lists
                m_scanSlots.DeleteTerminal(term);
            }
            m_scanSlots.EraceFreeSS();
        }

        //Update terminal list order
        void SortListTerminals()
        {
            int i = 0;
            int j = 0;
            while (i < m_listTerminals.Count)
            {
                //Search for group end
                j = i;
                while (j < m_listTerminals.Count)
                    if (m_listTerminals[i].m_widget == m_listTerminals[j].m_widget)
                    {
                        i = j;
                        j++;
                    }
                    else
                        break;

                //Collect group together
                while (j < m_listTerminals.Count)
                    if (m_listTerminals[i].m_widget == m_listTerminals[j].m_widget)
                    {
                        CyTerminal temp = m_listTerminals[j];
                        m_listTerminals.RemoveAt(j);
                        m_listTerminals.Insert(i + 1, temp);
                        i++;
                        j++;
                    }
                    else
                        j++;
                i++;
            }
        }

        #endregion

        #region ListWidgets Functions
        public List<CyWidget> GetBothParts(CyWidget wi)
        {
            List<CyWidget> listRes = new List<CyWidget>();
            foreach (CySensorType item in CyCsConst.GetBothParts(wi.m_type))
            {
                listRes.Add(FindWidget(wi.m_name, item));
            }
            return listRes;
        }

        public void PrepareWidgetsForAPIGenerator(CyChannelConfig config)
        {
            List<CySlider> sliders = new List<CySlider>(m_listSliders.Count);
            //Add Diplexed Linear Sliders
            for (int i = 0; i < m_listSliders.Count; i++)
                if (m_listSliders[i].Diplexing == CyDiplexed.Diplexed)
                {
                    sliders.Add(m_listSliders[i]);
                }
            //Add Not Diplexed Linear Sliders
            for (int i = 0; i < m_listSliders.Count; i++)
                if (m_listSliders[i].m_type == CySensorType.SliderLinear && sliders.Contains(m_listSliders[i]) == false)
                    sliders.Add(m_listSliders[i]);

            //Add Radial SLiders
            for (int i = 0; i < m_listSliders.Count; i++)
                if (m_listSliders[i].m_type == CySensorType.SliderRadial)
                    sliders.Add(m_listSliders[i]);
            m_listSliders = sliders;

            //Sort TouchPads
            List<CyTouchPad> touchPads = new List<CyTouchPad>(m_listTouchPads.Count);
            CySensorType mainType = CyCsConst.GetBothParts(CySensorType.TouchpadColumn)[0];
            for (int i = 0; i < m_listTouchPads.Count; i++)
                if (m_listTouchPads[i].m_type == mainType)
                {
                    List<CyWidget> listBoth = GetBothParts(m_listTouchPads[i]);
                    for (int j = 0; j < listBoth.Count; j++)
                    {
                        touchPads.Add((CyTouchPad)listBoth[j]);
                    }
                }
            m_listTouchPads = touchPads;

            //Sort MatrixButtons
            List<CyMatrixButton> matrixButtons = new List<CyMatrixButton>(m_listMatrixButtons.Count);
            mainType = CyCsConst.GetBothParts(CySensorType.MatrixButtonsColumn)[0];
            for (int i = 0; i < m_listMatrixButtons.Count; i++)
                if (m_listMatrixButtons[i].m_type == mainType)
                {
                    List<CyWidget> listBoth = GetBothParts(m_listMatrixButtons[i]);
                    for (int j = 0; j < listBoth.Count; j++)
                    {
                        matrixButtons.Add((CyMatrixButton)listBoth[j]);
                    }
                }
            m_listMatrixButtons = matrixButtons;

            //Fix widgets channel  
            if (config == CyChannelConfig.ONE_CHANNEL)
            {
                List<CyWidget> list = GetListWidgets();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].m_channel = CyChannelNumber.First;
                }
            }
        }
        #endregion

        public void RenameWidget(CyWidget wi, string newName)
        {
            List<CyWidget> list = GetBothParts(wi);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].m_name = newName;
            }
            m_needUpdate = true;
        }
        int TestNameIdentity(string str)
        {
            int res = 0;
            foreach (CyWidget item in GetListWidgets())
                if (item.m_name.ToLower() == str.ToLower())
                    res++;
            return res;
        }
        public bool NameValidating(string name)
        {
            if (name == CyCsConst.P_GUARD_SENSOR) return false;

            return NameTest(name) && TestNameIdentity(name) == 0;
        }
        public static bool NameTest(string name)
        {
            if (String.IsNullOrEmpty(name) == false)
            {
                //Test Characters
                for (int i = 0; i < name.Length; i++)
                {
                    char ch = name[i];
                    if ((ch >= 'A') && (ch <= 'Z')) continue;
                    if ((ch >= 'a') && (ch <= 'z')) continue;
                    //First character is not letter
                    if (i == 0) return false;
                    if ((ch >= '0') && (ch <= '9')) continue;
                    if (ch == '_') continue;
                    return false;
                }
            }
            return true;
        }
    }
    #endregion
}
