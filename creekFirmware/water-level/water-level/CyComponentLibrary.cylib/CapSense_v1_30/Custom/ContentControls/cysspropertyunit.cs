/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace  CapSense_v1_30.ContentControls
{
    public partial class CySSPropertyUnit : UserControl
    {
        CySSBaseProperties m_SSCSPropsSW = null;
        E_CAPSENSE_MODE m_method = E_CAPSENSE_MODE.CSD;
        List<CySSBaseProperties> m_listSSProperties = new List<CySSBaseProperties>();

        public CySSPropertyUnit()
        {
            InitializeComponent();
            m_listSSProperties.Add(new CySSCSDProperties());
            m_listSSProperties.Add(new CySSCSAProperties());
            panelConteiner.Enabled = false;
            foreach (CySSBaseProperties item in m_listSSProperties)
            {
                item.Dock = DockStyle.Fill;                
                panelConteiner.Controls.Add(item);
            }
            Method = E_CAPSENSE_MODE.CSA;
            cbCustom.CheckedChanged+=new EventHandler(cbCustom_CheckedChanged);

        }
        public void SetSavedHandler(EventHandler saveHandler)
        {
            foreach (CySSBaseProperties item in m_listSSProperties)
                item.m_actSaveChanges += saveHandler;
        }
        public E_CAPSENSE_MODE Method
        {
            get { return m_method; }
            set
            {
                m_method = value;
                m_SSCSPropsSW=null;
                foreach (CySSBaseProperties item in m_listSSProperties)
                    if (item.m_Method == m_method)
                    {
                        item.Visible = true;
                        m_SSCSPropsSW = item;
                    }
                    else
                        item.Visible = false;
                System.Diagnostics.Debug.Assert(m_SSCSPropsSW != null);
                lCSLeft.Text = "CapSense Method: " + m_method.ToString();

            }
        }
        private void cbCustom_CheckedChanged(object sender, EventArgs e)
        {
            panelConteiner.Enabled = cbCustom.Checked;
            if (m_SSCSPropsSW != null)
                m_SSCSPropsSW.CustomCheckBoxChanged(cbCustom);
        }
        public void GetProperties(CyAmuxBParams cch)
        {
            m_SSCSPropsSW.GetProperties(cch);
        }
        public void Clear()
        {
            m_SSCSPropsSW.Clear();
        }
        public void AddProperties(CyElScanSlot addObject)
        {
            m_SSCSPropsSW.AddProperties(addObject);
        }
        public void CombineProperties()
        {
            cbCustom.CheckState =m_SSCSPropsSW.CombineProperties();
        }
   }

    #region cntSSCSProps
    public  class CySSBaseProperties : UserControl
    {
        public E_CAPSENSE_MODE m_Method= E_CAPSENSE_MODE.None;
        public bool m_isIdac = true;
        public CyAmuxBParams m_amuxparam = null;
        public CySSProperties m_CombinedProperty;
        public List<CySSProperties> m_listProperties = new List<CySSProperties>();
        protected bool m_Locked = false;
        public EventHandler m_actSaveChanges;

        public virtual void UpdateForm() { }
        public CySSBaseProperties()
            : base()
        {
            this.m_actSaveChanges += new EventHandler(SaveChanges);
        }
        void SaveChanges(object sender, EventArgs e) { }
        public virtual void GetProperties(CyAmuxBParams cch) { }

        public void Clear()
        {
            m_Locked = true;
            m_listProperties.Clear();
        }

        public void CustomCheckBoxChanged(CheckBox cbCustom)
        {
            if (!m_Locked)
            {
                //Updating properties in Control
                m_CombinedProperty.Custom = cbCustom.Checked;
                m_Locked = true;
                UpdateForm();
                m_Locked = false;
                //Save Data
                m_actSaveChanges(null, null);
            }
        }

        public void AddProperties(CyElScanSlot addObject)
        {
            m_Locked = true;
            m_listProperties.Add(addObject.m_SSProperties);
        }

        public CheckState CombineProperties()
        {
            m_Locked = true;
            //Compare objects
            m_CombinedProperty = (CySSProperties)CyClPropsComparer.Comapare(m_listProperties.ToArray());
            UpdateForm();
            m_Locked = false;

            #region Convert bool to CheckState
            CheckState res = CheckState.Indeterminate;
            switch (m_CombinedProperty.Custom)
            {
                case true:
                    res = CheckState.Checked;
                    break;
                case false:
                    res = CheckState.Unchecked;
                    break;
                default:
                    break;
            }
            #endregion
            return res;
        }
    }
    #endregion
}
