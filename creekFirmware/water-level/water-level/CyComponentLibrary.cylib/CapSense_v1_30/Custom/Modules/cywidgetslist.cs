/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace CapSense_v1_30
{
    #region CyWidgetsList
    [Serializable()]
    public class CyWidgetsList
    {
        #region Header
        [NonSerialized]
        [XmlIgnore()]
        public CyGeneralParams m_Base = null;

        [XmlArray("ListMainTerminal")]
        [XmlArrayItem("ElTerminal")]
        public List<CyElTerminal> m_listTerminal = new List<CyElTerminal>();

        [XmlArray("ListButtons")]
        [XmlArrayItem("ElUnButton")]
        public List<CyElUnButton> m_listButtons = new List<CyElUnButton>();
        [XmlArray("ListMatrixButtons")]
        [XmlArrayItem("ElUnMatrixButton")]
        public List<CyElUnMatrixButton> m_listMatrixButtons = new List<CyElUnMatrixButton>();
        [XmlArray("ListTouchPads")]
        [XmlArrayItem("ElUnTouchPad")]
        public List<CyElUnTouchPad> m_listTouchPads = new List<CyElUnTouchPad>();
        [XmlArray("ListSliders")]
        [XmlArrayItem("ElUnSlider")]
        public List<CyElUnSlider> m_listSliders = new List<CyElUnSlider>();

        #region Functions for correct parameter access
        public void ReMoveWidgetFromList(CyElWidget wi)
        {
            if (wi != null)
                switch (wi.m_type)
                {
                    case E_SENSOR_TYPE.Button:
                    case E_SENSOR_TYPE.Proximity:
                    case E_SENSOR_TYPE.Generic:
                        m_listButtons.Remove((CyElUnButton)wi);
                        break;
                    case E_SENSOR_TYPE.Linear_Slider:
                    case E_SENSOR_TYPE.Radial_Slider:
                        m_listSliders.Remove((CyElUnSlider)wi);
                        break;
                    case E_SENSOR_TYPE.Touchpad_Col:
                    case E_SENSOR_TYPE.Touchpad_Row:
                        m_listTouchPads.Remove((CyElUnTouchPad)wi);
                        break;
                    case E_SENSOR_TYPE.Matrix_Buttons_Col:
                    case E_SENSOR_TYPE.Matrix_Buttons_Row:
                        m_listMatrixButtons.Remove((CyElUnMatrixButton)wi);
                        break;
                    default:
                        break;
                }
        }
        public static IEnumerable<Type> GetListTypes()
        {
            yield return typeof(CyElUnButton);
            yield return typeof(CyElUnSlider);
            yield return typeof(CyElUnTouchPad);
            yield return typeof(CyElUnMatrixButton);
        }
        public List<CyElWidget> GetListWidgets()
        {
            return new List<CyElWidget>(GetListWidgets_Int());
        }
         IEnumerable<CyElWidget> GetListWidgets_Int()
        {
            foreach (CyElWidget item in m_listMatrixButtons)
            {
                yield return item;
            }
            foreach (CyElWidget item in m_listTouchPads)
            {
                yield return item;
            }
            foreach (CyElWidget item in m_listSliders)
            {
                yield return item;
            }
            foreach (CyElWidget item in m_listButtons)
            {
                yield return item;
            }
        }
        CyElWidget AddWidgetToList(CyElWidget wi)
        {
            foreach (CyElWidget item in GetListWidgets_Int())
            {
                if (item.IsSameFull(wi)) return item;
            }
            switch (wi.m_type)
            {
                case E_SENSOR_TYPE.Button:
                case E_SENSOR_TYPE.Proximity:
                case E_SENSOR_TYPE.Generic:
                    m_listButtons.Add((CyElUnButton)wi);
                    break;
                case E_SENSOR_TYPE.Linear_Slider:
                case E_SENSOR_TYPE.Radial_Slider:
                    m_listSliders.Add((CyElUnSlider)wi);
                    break;
                case E_SENSOR_TYPE.Touchpad_Col:
                case E_SENSOR_TYPE.Touchpad_Row:
                    m_listTouchPads.Add((CyElUnTouchPad)wi);
                    break;
                case E_SENSOR_TYPE.Matrix_Buttons_Col:
                case E_SENSOR_TYPE.Matrix_Buttons_Row:
                    m_listMatrixButtons.Add((CyElUnMatrixButton)wi);
                    break;
                default:
                    break;
            }
            return null;
        }
        #endregion

        public CyWidgetsList(CyGeneralParams Base)
        {
            this.m_Base = Base;
        }
        public CyWidgetsList()
        {
        }

        #endregion

        #region Service work
        public CyElWidget FindWidget(string name, E_SENSOR_TYPE type)
        {
            foreach (CyElWidget item in GetListWidgets_Int())
                if (item.m_Name == name)
                {
                    //for sliders Names are unique
                    if ((CySensorType.IsSlider(type) && (CySensorType.IsSlider(item.m_type))) || (item.m_type == type))
                        return item;
                }
            return null;
        }
        public CyElWidget FindWidget(CyElTerminal term)
        {
            return FindWidget(term.m_Name, term.m_type);
        }
        public CyElWidget FindWidget(string str)
        {
            foreach (CyElWidget item in GetListWidgets_Int())
                if (item.ToString() == str) return item;
            return null;
        }
        public CyElWidget FindWidgetOldPattern(string str)
        {
            foreach (CyElWidget item in GetListWidgets_Int())
                if (item.m_type.ToString() + "_" + item.m_Name == str) return item;
            return null;
        }

        public CyHAProps GetWidgetsProperties(CyElWidget wi)
        {
            E_SENSOR_TYPE type = CySensorType.GetBothParts(wi.m_type)[0];
            return FindWidget(wi.m_Name, type).GetProperties();
        }

        public IEnumerable<CyElTerminal> GetTerminals(CyElWidget wi)
        {
            foreach (CyElTerminal item in m_listTerminal)
                if (item.m_Name == wi.m_Name)
                {
                    if (item.m_type == wi.m_type)
                    {
                        yield return item;
                    }
                }
        }
        public List<CyElTerminal> GetTerminalsWithOutHeader(CyElWidget wi)
        {
            List<CyElTerminal> res = new List<CyElTerminal>(GetTerminals(wi));
            if (CySensorType.IsCustomCase(wi.m_type))
            {
                res.RemoveAt(0);
            }
            return res;
        }
        #endregion

        #region Function

        #region Add/Delete Widget
        public CyElWidget AddWidget(object[] props, E_SENSOR_TYPE type)
        {
            return AddWidget(props, type, false);
        }
        public CyElWidget AddWidget(object[] props, E_SENSOR_TYPE type, bool mVirtual)
        {
            string name = CyGeneralParams.GetString(props[0]);
            CyElWidget WI = null;
            CyElTerminal term;
            int Count = 0;
            switch (type)
            {
                case E_SENSOR_TYPE.Button:
                case E_SENSOR_TYPE.Proximity:
                case E_SENSOR_TYPE.Generic:
                    WI = new CyElUnButton(props, type);
                    term = new CyElTerminal(name, type);
                    if (mVirtual==false)// Is Real Widget
                        AddTerminal(term, WI.m_side);
                    break;
                case E_SENSOR_TYPE.Linear_Slider:
                case E_SENSOR_TYPE.Radial_Slider:
                    type = CyGeneralParams.GetEnumSliderType(CyGeneralParams.GetString(props[1]));
                    WI = new CyElUnSlider(type, props);
                    break;
                case E_SENSOR_TYPE.Touchpad_Col:
                case E_SENSOR_TYPE.Touchpad_Row:
                    WI = new CyElUnTouchPad(props, type);
                    break;
                case E_SENSOR_TYPE.Matrix_Buttons_Col:
                case E_SENSOR_TYPE.Matrix_Buttons_Row:
                    WI = new CyElUnMatrixButton(props, type);
                    break;

                default:
                    return null;
            }
            if (mVirtual==false)// Is Real Widget
            {
                Count = WI.GetCount();
                if (AddWidgetToList(WI) == null)
                {
                    if (Count > 0)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            term = new CyElTerminal(name, type, j);
                            AddTerminal(term, WI.m_side);
                        }
                    }
                    m_Base.m_cyScanSlotTab.InsertNewScanSlots(WI, WI.m_side);
                }
                else return AddWidgetToList(WI);
            }
            return WI;
        }
        public void DeleteWidgets(CyElWidget[] wi)
        {
            //Remove widget terminals
            foreach (CyElWidget element in wi)
            {
                DeleteTerminal(new CyElTerminal(element.m_Name, element.m_type));//remove header
                DeleteTerminal(element.m_Name, element.m_type, element.GetCount());//remove indexes terminals
            }
            //Remove Widget
            foreach (CyElWidget item in wi)
            {
                ReMoveWidgetFromList(item);
            }
        }

        #endregion

        #region Validate
        bool IsColLocationChanged(object[] NewRow, object[] LastRow, int colLocation)
        {
            return LastRow[colLocation].ToString() != NewRow[colLocation].ToString();
        }
        public CyElWidget[] GetWidgets(object[] NewRow, E_SENSOR_TYPE stype)
        {
            switch (stype)
            {
                case E_SENSOR_TYPE.Button:
                case E_SENSOR_TYPE.Linear_Slider:
                case E_SENSOR_TYPE.Radial_Slider:
                case E_SENSOR_TYPE.Proximity:
                case E_SENSOR_TYPE.Generic:
                    return new CyElWidget[] { (CyElWidget)NewRow[NewRow.Length - 2] };
                case E_SENSOR_TYPE.Touchpad_Col:
                case E_SENSOR_TYPE.Touchpad_Row:
                case E_SENSOR_TYPE.Matrix_Buttons_Col:
                case E_SENSOR_TYPE.Matrix_Buttons_Row:
                    return new CyElWidget[]{ (CyElWidget)NewRow[NewRow.Length - 4],
                                                                (CyElWidget)NewRow[NewRow.Length - 3]};
                default:
                    break;
            }
            return null;
        }
        public IEnumerable<CyElWidget> ValidateWidgets(E_SENSOR_TYPE[] type, object[] newRow)
        {
            CyElWidget newWidget = null;
            CyElWidget[] oldWidget = GetWidgets(newRow, type[0]);

            for (int i = 0; i < type.Length; i++)
            {
                if (oldWidget[i] == null) { yield return AddWidget(newRow, type[i]); continue; }

                newWidget = AddWidget(newRow, type[i], true);
                E_COMPARER_TYPE c_res = oldWidget[i].ChangesSearch(newWidget);
                if (((c_res & E_COMPARER_TYPE.NameChange) != 0) ||
                    ((c_res & E_COMPARER_TYPE.TypeChange) != 0))
                {
                    //Re-Name
                    ReNameTerminal(new CyElTerminal(oldWidget[i].m_Name, oldWidget[i].m_type), newWidget.m_Name, 
                        newWidget.m_type);
                    oldWidget[i].m_type = newWidget.m_type;
                }
                if ((c_res & E_COMPARER_TYPE.CountChange) != 0)
                {
                    //Count change
                    if (oldWidget[i].GetCount() > newWidget.GetCount())
                    //remove terminals
                    {
                        DeleteTerminal(oldWidget[i].m_Name, oldWidget[i].m_type, newWidget.GetCount(), 
                        oldWidget[i].GetCount());
                    }
                    if (oldWidget[i].GetCount() < newWidget.GetCount())
                    //append terminals
                    {
                        CyElWidget wi = oldWidget[i];
                        for (int j = oldWidget[i].GetCount(); j < newWidget.GetCount(); j++)
                        {
                            CyElTerminal term = new CyElTerminal(wi.m_Name, wi.m_type, j);
                            AddTerminal(term, wi.m_side);
                            if (CySensorType.IsCustomCase(wi.m_type))
                            {
                                m_Base.m_cyScanSlotsList.AppendTerminalInSS(wi, term);
                            }
                            else
                            {
                                m_Base.m_cyScanSlotTab.InsertNewScanSlotWithTerm(term, wi.m_side);
                            }
                        }
                        //Update terminal list order
                        SortListTerminals();
                    }
                }
                if ((c_res & E_COMPARER_TYPE.SideChange) != 0)
                {
                    for (int id = 0; id < m_listTerminal.Count; id++)
                        if (FindWidget(m_listTerminal[id]).IsSameFull(oldWidget[i]))
                        {
                            m_listTerminal[id].m_haveSide = newWidget.m_side;
                        }
                    m_Base.m_cyScanSlotTab.ChangeWidgetSide(oldWidget[i], newWidget);
                    oldWidget[i].m_side = newWidget.m_side;
                }
                oldWidget[i].SetObjData(newRow);
                yield return null;
            }
        }
        #endregion


        #endregion

        #region Terminal Work

        bool AppendTerminal(CyElTerminal term)
        {
            foreach (CyElTerminal item in m_listTerminal)
            {
                if (item.CompareTerminals(term)) return false;
            }
            return true;
        }
        public CyElTerminal AddTerminal(CyElTerminal term, E_EL_SIDE side)
        {
            term.m_haveSide = side;
            if (AppendTerminal(term))
            {
                m_listTerminal.Add(term);
            }
            return term;
        }

        void ReNameTerminal(CyElTerminal term, string name, E_SENSOR_TYPE _type)
        {
            //ReName Terminal in Main List
            foreach (CyElTerminal item in m_listTerminal)
                if (item.CompareBaseWidget(term))
                {
                    item.m_Name = name;
                    item.m_type = _type;
                }
        }
        public CyElTerminal FindTerminal(string str)
        {
            foreach (CyElTerminal item in m_listTerminal)
                if (item.ToString() == str) return item;
            return null;
        }
        void DeleteTerminal(CyElTerminal term)
        {
            //Delete Terminal in Main List
            int pos = 0;
            while (pos < m_listTerminal.Count)
            {
                if (m_listTerminal[pos].CompareTerminals(term))
                {
                    m_listTerminal.RemoveAt(pos);
                }
                else pos++;
            }

            //Delete Terminals in Scan Slots Lists
            m_Base.m_cyScanSlotsList.DeleteTerminal(term);

            m_Base.m_cyScanSlotTab.EraceFreeSS();
        }
        void DeleteTerminal(string name, E_SENSOR_TYPE type, int count)
        {
            DeleteTerminal(name, type, 0, count);
        }
        void DeleteTerminal(string name, E_SENSOR_TYPE type, int start, int count)
        {
            CyElTerminal term;
            for (int i = start; i < count; i++)
            {
                term = new CyElTerminal(name, type, i);
                DeleteTerminal(term);
            }
        }

        IEnumerable<CyElTerminal> GetTerminalsListInt(E_EL_SIDE side)
        {
            SortListTerminals();
            foreach (CyElTerminal item in m_listTerminal)
                if (item.m_haveSide == side)
                {
                    if (CySensorType.IsNotWidgetLabel(item))
                        yield return item;
                }
        }
        public List<CyElTerminal> GetListTerminals(E_EL_SIDE side)
        {
            return new List<CyElTerminal>(GetTerminalsListInt(side));
        }
        public List<CyElTerminal> GetTerminalsSorted()
        {
            return new List<CyElTerminal>(GetTerminalsSortedInt());
        }
        IEnumerable<CyElTerminal> GetTerminalsSortedInt()
        {
            foreach (E_EL_SIDE item in CyGeneralParams.m_sideArray)
            {
                foreach (CyElTerminal s_item in GetListTerminals(item))
                    yield return s_item;
            }
        }
        //Update terminal list order
        public void SortListTerminals()
        {
            int i = 0;
            int j = 0;
            while (i < m_listTerminal.Count)
            {
                //Search for group end
                j = i;
                while (j < m_listTerminal.Count)
                    if (m_listTerminal[i].CompareBaseWidget(m_listTerminal[j]))
                    {
                        i = j;
                        j++;
                    }
                    else
                        break;

                //Collect group together
                while (j < m_listTerminal.Count)
                    if (m_listTerminal[i].CompareBaseWidget(m_listTerminal[j]))
                    {
                        CyElTerminal temp = m_listTerminal[j];
                        m_listTerminal.RemoveAt(j);
                        m_listTerminal.Insert(i + 1, temp);
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

        #region Type Work
        public List<CyElWidget> GetListWidgetsByType(Type type)
        {
            if (type == typeof(CyElUnButton)) return new List<CyElWidget>(m_listButtons.ToArray());
            if (type == typeof(CyElUnSlider)) return new List<CyElWidget>(m_listSliders.ToArray());
            if (type == typeof(CyElUnTouchPad)) return new List<CyElWidget>(m_listTouchPads.ToArray());
            if (type == typeof(CyElUnMatrixButton)) return new List<CyElWidget>(m_listMatrixButtons.ToArray());
            return new List<CyElWidget>();
        }
        public List<CyElWidget> GetListWidgetsByType(E_SENSOR_TYPE type)
        {
            List<CyElWidget> res = new List<CyElWidget>();
            foreach (CyElWidget item in GetListWidgets())
                if (item.m_type == type)
                {
                    res.Add(item);
                }
            return res;
        }
        public List<CyElWidget> GetListWidgetsByTypeHL(Type type)
        {
            if (type != typeof(CyElUnButton)) return GetListWidgetsByType(type);
            else
            {
                //HL is  with out Generic type
                List<CyElWidget> listBtn = new List<CyElWidget>();
                foreach (CyElWidget item in GetListWidgetsByType(type))
                    if (item.m_type != E_SENSOR_TYPE.Generic)
                    {
                        listBtn.Add(item);
                    }
                return listBtn;
            }
        }
        public int GetWidgetsCount(E_SENSOR_TYPE stype)
        {
            int res = 0;
            foreach (CyElWidget item in GetListWidgets())
                if (stype == item.m_type)
                {
                    res++;
                }
            return res;
        }
        public int GetWidgetsCount(E_SENSOR_TYPE stype, E_EL_SIDE _side)
        {
            int res = 0;
            foreach (CyElWidget item in GetListWidgets())
                if (stype == item.m_type)
                {
                    if (item.m_side == _side)
                        res++;
                }
            return res;
        }
        public int GetCountFilterOnHL(Type type)
        {
            int res = 0;
            foreach (CyElWidget item in GetListWidgetsByTypeHL(type))
                if (GetWidgetsProperties(item).GetFilterMask() != 0)
                {
                    res++;
                }
            return res;
        }
        public int GetIndexInTypedListHL(CyElWidget wi)
        {
            return GetListWidgetsByTypeHL(wi.GetType()).IndexOf(wi);
        }
        #endregion

        #region Full Widget Work
        public List<CyElWidget> GetBothParts(CyElWidget wi)
        {
            List<CyElWidget> listRes = new List<CyElWidget>();
            foreach (E_SENSOR_TYPE item in CySensorType.GetBothParts(wi.m_type))
            {
                listRes.Add(FindWidget(wi.m_Name, item));
            }
            return listRes;
        }
        public List<CyElWidget> GetListWithFullWidgetsHL()
        {
            return new List<CyElWidget>(GetListWithFullWidgetsHLInt());
        }
        IEnumerable<CyElWidget> GetListWithFullWidgetsHLInt()
        {
            foreach (CyElWidget item in m_listMatrixButtons)
                if (CySensorType.IsRow(item.m_type)==false)
                    yield return item;
            foreach (CyElWidget item in m_listTouchPads)
                if (CySensorType.IsRow(item.m_type)==false)
                    yield return item;
            foreach (CyElWidget item in m_listSliders)
                yield return item;
            foreach (CyElWidget item in m_listButtons)
                if (item.m_type != E_SENSOR_TYPE.Generic)//HL is  with out Generic type
                    yield return item;
        }
        public int GetIndexInListWithFullWidgetsHL(CyElWidget wi)
        {
            if (wi.m_type == E_SENSOR_TYPE.Generic) return 0;//HL is  with out Generic type
            CyElWidget top_wi = FindWidget(wi.m_Name, CySensorType.GetBothParts(wi.m_type)[0]);
            int res = GetListWithFullWidgetsHL().IndexOf(top_wi);

            //If Not Column Type Widget
            if ((CySensorType.IsRow(wi.m_type)) || (!CySensorType.IsArrayCase(wi.m_type)))
            {
                res += GetDoubleFullWidgetsCount();
            }
            return res;
        }
        #endregion

        public int GetDoubleFullWidgetsCount()
        {
            return (m_listMatrixButtons.Count + m_listTouchPads.Count) / 2;
        }
        public int GetCountWidgetsHL()
        {
            return GetListWidgets().Count - GetWidgetsCount(E_SENSOR_TYPE.Generic);
        }
        #endregion

    }
    #endregion
}
