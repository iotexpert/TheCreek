/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace  CapSense_v0_5
{
    #region CyWidgetsList
    [Serializable()]
    public class CyWidgetsList
    {
        #region Header
        [NonSerialized]
        [XmlIgnore()]
        public CyGeneralParams Base = null;

        [XmlArray("ListMainTerminal")]
        public List<ElTerminal> listTerminal = new List<ElTerminal>();

        [XmlArray("LiButtons")]
        public List<ElUnButton> listButtons = new List<ElUnButton>();
        [XmlArray("listMatrixButtons")]
        public List<ElUnMatrixButton> listMatrixButtons = new List<ElUnMatrixButton>();
        [XmlArray("listTouchPads")]
        public List<ElUnTouchPad> listTouchPads = new List<ElUnTouchPad>();
        [XmlArray("listSliders")]
        public List<ElUnSlider> listSliders = new List<ElUnSlider>();

        #region Functions for correct parameter access
        public void reMoveWidgetFromList(ElWidget wi)
        {
            if (wi != null) 
            switch (wi.type)
            {
                case sensorType.Button:
                case sensorType.Proximity:
                case sensorType.Generic:                    
                    listButtons.Remove((ElUnButton)wi);
                    break;
                case sensorType.Linear_Slider:
                case sensorType.Radial_Slider:
                     listSliders.Remove((ElUnSlider)wi);
                    break;
                case sensorType.Touchpad_Col:
                case sensorType.Touchpad_Row:                    
                  listTouchPads.Remove((ElUnTouchPad)wi);
                    break;
                case sensorType.Matrix_Buttons_Col:
                case sensorType.Matrix_Buttons_Row:                    
                    listMatrixButtons.Remove((ElUnMatrixButton)wi);
                    break;
                default:
                    break;
            }
        }
        public static IEnumerable<Type> GetListTypes()
        {
            yield return typeof(ElUnButton);
            yield return typeof(ElUnSlider);
            yield return typeof(ElUnTouchPad);
            yield return typeof(ElUnMatrixButton);
        }
                public List<ElWidget> GetListWidgets()
        {
            return new List<ElWidget>(getListWidgets());
        }
        public IEnumerable<ElWidget> getListWidgets()
        {
            foreach (ElWidget item in listMatrixButtons)
            {
                yield return item;
            }
            foreach (ElWidget item in listTouchPads)
            {
                yield return item;
            }
            foreach (ElWidget item in listSliders)
            {
                yield return item;
            }
            foreach (ElWidget item in listButtons)
            {
                yield return item;
            }
        }
        public void AddWidgetToList(ElWidget wi)
        {
            switch (wi.type)
            {
                case sensorType.Button:
                case sensorType.Proximity:
                case sensorType.Generic:
                    listButtons.Add((ElUnButton)wi);
                    break;
                case sensorType.Linear_Slider:
                case sensorType.Radial_Slider:
                    listSliders.Add((ElUnSlider)wi);
                    break;
                case sensorType.Touchpad_Col:
                case sensorType.Touchpad_Row:
                    listTouchPads.Add((ElUnTouchPad)wi);
                    break;
                case sensorType.Matrix_Buttons_Col:
                case sensorType.Matrix_Buttons_Row:
                    listMatrixButtons.Add((ElUnMatrixButton)wi);
                    break;
                default:
                    break;
            }
        }
        #endregion

        public CyWidgetsList(CyGeneralParams Base)
        {
            this.Base = Base;
        }
        public CyWidgetsList()
        {
        }

        #endregion

        #region Service work
        public ElWidget GetWidget(string Name, sensorType type)
        {
            foreach (ElWidget item in getListWidgets())
                if (item.Name == Name)
                {
                    if ((cySensorType.IsSlider(type) && (cySensorType.IsSlider(item.type))) || (item.type == type))//for sliders Names are unique
                        return item;
                }
            return null;
        }
        public ElWidget GetWidget(ElTerminal term)
        {
            return GetWidget(term.Name, term.type);
        }
        public ElWidget FindWidget(string str)
        {
            foreach (ElWidget item in getListWidgets())
                if (item.ToString() == str) return item;
            return null;
        }
        public ElWidget FindWidgetOldPattern(string str)
        {
            foreach (ElWidget item in getListWidgets())
                if ( item.type.ToString()+"_"+ item.Name== str) return item;
            return null;
        }
        public ElWidget FindWidget(sensorType stype, string sname)
        {
            ElWidget res = null;
            foreach (ElWidget itemIntern in getListWidgets())
                if ((itemIntern.type == stype) && (itemIntern.Name == sname))
                {
                    res = itemIntern;
                    break;
                }
            return res;
        }
       public HAProps GetWidgetsProperties(ElWidget wi)
       {
           sensorType type = cySensorType.GetBothParts(wi.type)[0];
           return GetWidget(wi.Name, type).GetProps();
       }

        public IEnumerable<ElTerminal> GetTerminals(ElWidget widg)
        {            
            foreach (ElTerminal item in listTerminal)
                if (item.Name == widg.Name)
                {
                    if (item.type == widg.type)
                    {
                        yield return item;
                    }
                }
        }
        public List<ElTerminal> GetTerminalsWithOutHeader(ElWidget widg)
        {
            List<ElTerminal> res = new List<ElTerminal>(GetTerminals(widg));
            if (cySensorType.IsCustomCase(widg.type))
            {
                res.RemoveAt(0);
            }
            return res;
        }


        ElWidget AppentToList(ElWidget wi)
        {
            foreach (ElWidget item in getListWidgets())
            {
                if (item.isSame(wi)) return item;
            }
            
            AddWidgetToList(wi);
            return null;
        }
        #endregion

        #region Function

        #region Add/Delete Widget
        public ElWidget AddWidget(object[] props, sensorType type)
        {
           return AddWidget(props, type, false);
        }
        public ElWidget AddWidget(object[] props, sensorType type, bool mVirtual)
        {
            string name = CyGeneralParams.GetString(props[0]);
            ElWidget WI = null;
            ElTerminal term;
            int Count = 0;
            switch (type)
            {
                case sensorType.Button:
                case sensorType.Proximity:
                case sensorType.Generic:
                    WI = new ElUnButton(props, type);
                    term = new ElTerminal(name, type);
                    if (!mVirtual)// Is Real Widget
                        AddTerminal(term, WI.side);
                    break;
                case sensorType.Linear_Slider:
                case sensorType.Radial_Slider:
                    type = CyGeneralParams.GetEnumSliderType(CyGeneralParams.GetString(props[1]));
                    WI = new ElUnSlider(type, props);
                    break;
                case sensorType.Touchpad_Col:
                case sensorType.Touchpad_Row:

                    WI = new ElUnTouchPad(props, type);
                    break;
                case sensorType.Matrix_Buttons_Col:
                case sensorType.Matrix_Buttons_Row:
                    WI = new ElUnMatrixButton(props, type);
                    break;

                default:
                    return null;
            }
            if (!mVirtual)// Is Real Widget
            {
                Count = WI.GetCount();
                if (AppentToList(WI) == null)
                {
                    //listWidgets.Add(WI);
                    if (Count > 0)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            term = new ElTerminal(name, type, j);
                            AddTerminal(term, WI.side);
                        }
                    }
                    Base.cyScanSlots.AddWidget(WI);
                }
                else return AppentToList(WI);
            }
            return WI;
        }
        public void DeleteWidget(ElWidget[] Wis)
        {
            foreach (ElWidget element in Wis)
            {
                switch (element.type)
                {
                    case sensorType.Button:
                    case sensorType.Proximity:
                    case sensorType.Generic:
                        DeleteTerminal(new ElTerminal(element.Name, element.type));
                        break;
                    case sensorType.Linear_Slider:
                    case sensorType.Radial_Slider:
                        DeleteTerminal(element.Name, element.type, element.GetCount());
                        break;
                    case sensorType.Touchpad_Col:
                    case sensorType.Touchpad_Row:
                        foreach (ElUnTouchPad item in Wis)
                            DeleteTerminal(item.Name, item.type, item.GetCount());
                        break;
                    case sensorType.Matrix_Buttons_Col:
                    case sensorType.Matrix_Buttons_Row:
                        foreach (ElUnMatrixButton item in Wis)
                            DeleteTerminal(item.Name, item.type, item.GetCount());
                        break;
                    default:
                        break;
                }
            }
            foreach (ElWidget item in Wis)
            {
                reMoveWidgetFromList(item);
            }
        }

        #endregion

        #region Validate
        enum intOperation
        {
            eioNew, eioReName, eioNone
        }
        bool IsColLocationChange(object[] NewRow, object[] LastRow, int colLocation)
        {
            return LastRow[colLocation].ToString() != NewRow[colLocation].ToString();
        }
        public ElWidget[] GetWidget(object[] NewRow, sensorType stype)
        {
            switch (stype)
            {
                case sensorType.Button:
                case sensorType.Linear_Slider:
                case sensorType.Radial_Slider:
                case sensorType.Proximity:                   
                case sensorType.Generic:
                    return new ElWidget[]{ (ElWidget)NewRow[NewRow.Length - 2]};                    
                case sensorType.Touchpad_Col:                    
                case sensorType.Touchpad_Row:                   
                case sensorType.Matrix_Buttons_Col:                    
                case sensorType.Matrix_Buttons_Row:
                    return new ElWidget[]{ (ElWidget)NewRow[NewRow.Length - 4],(ElWidget)NewRow[NewRow.Length - 3]};                    
                default:
                    break;
            }
            return null;
        }
        public IEnumerable<ElWidget> ValidateWidgets(sensorType[] type, object[] NewRow)
        {
            ElWidget newWidget = null;
            ElWidget[] oldWidget = GetWidget(NewRow, type[0]);

            for (int i = 0; i < type.Length; i++)
            {
                if (oldWidget[i] == null) { yield return AddWidget(NewRow, type[i]); continue; }

                    newWidget =  AddWidget(NewRow, type[i], true) ;

                    if (!(oldWidget[i].isAlmostSame(newWidget)))
                    {
                        DeleteWidget(new ElWidget[]{oldWidget[i]});
                        yield return AddWidget(NewRow, type[i]) ;
                        continue;
                    }
                    else
                    {
                        //Re-Name
                        ReNameTerminal(new ElTerminal(oldWidget[i].Name, oldWidget[i].type), newWidget.Name);
                        oldWidget[i].SetProps(NewRow);
                        yield return  null;
                    }                    
            }
        }
        #endregion


        #endregion

        #region Terminal Work

        bool AppendTerminal(ElTerminal term)
        {
            foreach (ElTerminal item in listTerminal)
            {
                if (item.IsSameFull(term)) return false;
            }
            return true;
        }
        public ElTerminal AddTerminal(ElTerminal term, eElSide side)
        {
            term.haveSide = side;
            if (AppendTerminal(term))
            {
                listTerminal.Add(term);
            }
            return term;
        }

        void ReNameTerminal(ElTerminal term, string Name)
        {
            //ReName Terminal in Main List
            foreach (ElTerminal item in listTerminal)
                if (item.IsSameW(term))
                {
                    item.Name = Name;
                    //break;
                }
        }
        public ElTerminal FindTerminal(string str)
        {
            foreach (ElTerminal  item in listTerminal)
                if (item.ToString() == str) return item;
            return null;
        }
        void DeleteTerminal(ElTerminal term)
        {
            //Delete Terminal in Main List
            int pos = 0;
            while (pos < listTerminal.Count)
            {
                if (listTerminal[pos].IsSameW(term))
                {
                    listTerminal.RemoveAt(pos);
                }
                else pos++;
            }

            //Delete Terminals in Scan Slots Lists
            Base.cyScanSlotsList.DeleteTerminal(term);

            Base.cyScanSlots.EraceFreeSS();
        }
        void DeleteTerminal(string name, sensorType type, int count)
        {
            ElTerminal term;
            for (int i = 0; i < count; i++)
            {
                term = new ElTerminal(name, type, i);
                DeleteTerminal(term);
            }
        }
        IEnumerable<ElTerminal> GetListTerminalsFromSideInt(eElSide side)
        {
            foreach (ElTerminal item in listTerminal)
                if (item.haveSide == side)
                {
                    if(!cySensorType.IsWidgetLabel(item))
                    yield return item;
                }
        }
        public List<ElTerminal> GetListTerminalsFromSide(eElSide side)
        {
            return new List<ElTerminal>(GetListTerminalsFromSideInt(side));
        }
        public List<ElTerminal> GetTerminalsSorted()
        {
            return new List<ElTerminal>(GetTerminalsSortedInt());
        }
        IEnumerable<ElTerminal> GetTerminalsSortedInt()
        {
            foreach (eElSide item in CyGeneralParams.sideMass)
            {
                foreach (ElTerminal s_item in GetListTerminalsFromSide(item))
                    yield return s_item;
            }
        }

        #endregion

        #region ListWidgets Functions

        #region Type Work
        public List<ElWidget> GetListWidgetsByType(Type type)
        {
            if(type==typeof(ElUnButton))   return new List<ElWidget>(listButtons.ToArray());
            if(type==typeof(ElUnSlider) )   return new List<ElWidget>(listSliders.ToArray());
            if(type==typeof(ElUnTouchPad) )   return new List<ElWidget>(listTouchPads.ToArray());
            if (type == typeof(ElUnMatrixButton)) return new List<ElWidget>(listMatrixButtons.ToArray());
            return new List<ElWidget>();
        }
        public List<ElWidget> GetListWidgetsByTypeHL(Type type)
        {
            if (type != typeof(ElUnButton)) return GetListWidgetsByType(type);
            else
            {
                //HL is  with out Generic type
                List<ElWidget> listBtn = new List<ElWidget>();
                foreach (ElWidget item in GetListWidgetsByType(type))
                    if (item.type != sensorType.Generic)
                    {
                        listBtn.Add(item);
                    }
                return listBtn;
            }
        }
        public int GetCountWidgetsSameType(sensorType stype)
        {
            int res = 0;
            foreach (ElWidget item in GetListWidgets())
                if (stype == item.type)
                {
                    res++;
                }
            return res;
        }
        public int GetCountWidgetsSameType(sensorType stype, eElSide _side)
        {
            int res = 0;
            foreach (ElWidget item in GetListWidgets())
                if (stype == item.type)
                {
                    if (item.side == _side)
                        res++;
                }
            return res;
        }
        public int GetCountFilterOnHL(Type type)
        {
            int res = 0;
            foreach (ElWidget item in GetListWidgetsByTypeHL(type))
                if (GetWidgetsProperties(item).GetFilterMask() != 0)
                {
                    res++;
                }
            return res;
        }
        public int GetIndexInTypedListHL(ElWidget wi)
        {
            return GetListWidgetsByTypeHL(wi.GetType()).IndexOf(wi);            
        }
        #endregion

        #region Full Widget Work
        public List<ElWidget> GetBothParts(ElWidget wi)
        {
            List<ElWidget> listRes = new List<ElWidget>();
            foreach (sensorType item in cySensorType.GetBothParts(wi.type))
            {
                listRes.Add(GetWidget(wi.Name, item));
            }
            return listRes;
        }
        public List<ElWidget> GetListWithFullWidgetsHL()
        {
            return new List<ElWidget>(GetListWithFullWidgetsHLInt());
        }
        IEnumerable<ElWidget> GetListWithFullWidgetsHLInt()
        {
            foreach (ElWidget item in listMatrixButtons)
                if (!cySensorType.IsRow(item.type))
                    yield return item;
            foreach (ElWidget item in listTouchPads)
                if (!cySensorType.IsRow(item.type))
                    yield return item;
            foreach (ElWidget item in listSliders)
                yield return item;
            foreach (ElWidget item in listButtons)
                if (item.type != sensorType.Generic)//HL is  with out Generic type
                yield return item;
        }
        public int GetIndexInListWithFullWidgetsHL(ElWidget wi)
        {
            if (wi.type == sensorType.Generic) return 0;//HL is  with out Generic type
            ElWidget top_wi = GetWidget(wi.Name, cySensorType.GetBothParts(wi.type)[0]);
            int res = GetListWithFullWidgetsHL().IndexOf(top_wi);

            //If Not Column Type Widget
            if ((cySensorType.IsRow(wi.type)) || (!cySensorType.IsArrayCase(wi.type)))
            {
                res += GetCountDoubleFullWidgets();
            }
            return res;
        }
        #endregion

        public int GetCountDoubleFullWidgets()
        {
            return (listMatrixButtons.Count + listTouchPads.Count) / 2;
        }
        public int GetCountWidgetsHL()
        {
            return GetListWidgets().Count - GetCountWidgetsSameType(sensorType.Generic);
        }
       #endregion

    }
    #endregion
}
