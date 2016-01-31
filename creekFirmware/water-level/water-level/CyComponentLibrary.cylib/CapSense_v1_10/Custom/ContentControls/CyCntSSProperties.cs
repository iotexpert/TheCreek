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

namespace  CapSense_v1_10.ContentControls
{
    public partial class CyCntSSProperties : UserControl
    {
        CyCntSSCSProps m_SSCSPropsSW = null;
        E_CAPSENSE_MODE m_method = E_CAPSENSE_MODE.CSD;

        public CyCntSSProperties()
        {
            InitializeComponent();
            panelConteiner.Enabled = false;
            SSCSAProps.Dock = DockStyle.Fill;
            SSCSDProps.Dock = DockStyle.Fill;
            Method = E_CAPSENSE_MODE.CSA;
            cbCustom.CheckedChanged+=new EventHandler(cbCustom_CheckedChanged);

        }
        public void SetSavedHandler(EventHandler saveHandler)
        {
            SSCSAProps.m_actSaveChanges += saveHandler;
            SSCSDProps.m_actSaveChanges += saveHandler;
        }
        public E_CAPSENSE_MODE Method
        {
            get { return m_method; }
            set 
            {
                m_method = value;
                SSCSAProps.Visible = false;
                SSCSDProps.Visible = false;

                if (m_method == E_CAPSENSE_MODE.CSA){ SSCSAProps.Visible = true;m_SSCSPropsSW=SSCSAProps;}
                if (m_method == E_CAPSENSE_MODE.CSD) { SSCSDProps.Visible = true; m_SSCSPropsSW = SSCSDProps; }
                lCSLeft.Text = "CapSense Method: " + m_method.ToString();
            }
        }
        private void cbCustom_CheckedChanged(object sender, EventArgs e)
        {
            panelConteiner.Enabled = cbCustom.Checked;
            if (m_SSCSPropsSW != null)
                m_SSCSPropsSW.CbCustomProcessing(cbCustom);
        }
        public void GetProperties(CyAmuxBParams cch)
        {
            m_SSCSPropsSW.GetProperties(cch);
        }
        public void Clear()
        {
            m_SSCSPropsSW.Clear();
        }
        public void AddProps(CyElScanSlot addObject)
        {
            m_SSCSPropsSW.AddProps(addObject);
        }
        public void SetObject()
        {
            cbCustom.Checked =m_SSCSPropsSW.SetObject();            
        }
    }

    #region cntSSCSProps
    public  class CyCntSSCSProps : UserControl
    {
        public bool m_isIdac = true;
        public CyAmuxBParams m_currectCch = null;
        public CySSProperties m_lastObject;
        public List<CySSProperties> m_listLastObj = new List<CySSProperties>();
        protected bool m_isGeneration = false;
        public EventHandler m_actSaveChanges;

        public virtual void ShowInControl() { }
        public CyCntSSCSProps()
            : base()
        {
            this.m_actSaveChanges += new EventHandler(SaveChanges);
        }
        void SaveChanges(object sender, EventArgs e) { }
        public virtual void GetProperties(CyAmuxBParams cch) { m_isGeneration = false; }
        public bool SetObject()//Visualize objects from listLastObj array
        {
            m_isGeneration = true;
            m_lastObject = (CySSProperties)CyClPropsComparer.Comapare(m_listLastObj.ToArray());//Compare objects
            ShowInControl();//Visualize data
            m_isGeneration = false;
            return m_lastObject.Custom;
        }    
        public void Clear()
        {
            m_isGeneration = true;
            m_listLastObj.Clear();
        }
        public void CbCustomProcessing(CheckBox cbCustom)
        {
            if (!m_isGeneration)
            {
                //Updating properties in Control
                m_lastObject.Custom = cbCustom.Checked;
                m_isGeneration = true;
                ShowInControl();
                m_isGeneration = false;
                //Save Data
                m_actSaveChanges(null, null);
            }
        }
        public void AddProps(CyElScanSlot addObject)
        {
            m_isGeneration = true;
            m_listLastObj.Add(addObject.m_SSProperties);
        }

        #region Values Processing
        protected void SetValue(ref int main, int val)
        {
            if (val != -1)
            {
                main = val;
            }
        }
        protected void SetValue(ref CyIntElement main, TextBox val)
        {
            if (val.Text != "")
            {
                main.Validate(val.Text);
                val.Text = main.m_Value.ToString();
            }
        }
        protected string GetValue(CyIntElement val)
        {
            if (val != null)
            {
                return val.m_Value.ToString();
            }
            return "";
        }
        #endregion
    }
    #endregion
}
