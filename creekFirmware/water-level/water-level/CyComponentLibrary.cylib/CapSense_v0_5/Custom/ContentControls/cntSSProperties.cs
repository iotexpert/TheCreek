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

namespace  CapSense_v0_5.ContentControls
{
    public partial class cntSSProperties : UserControl
    {

        public cntSSCSProps SSCSPropsSW = null;
        eCapSenseMode method = eCapSenseMode.CSD;

        public cntSSProperties()
        {
            InitializeComponent();
            SSCSAProps.Dock = DockStyle.Fill;
            SSCSDProps.Dock = DockStyle.Fill;
            Method = eCapSenseMode.CSA;
        }
        public eCapSenseMode Method
        {
            get { return method; }
            set 
            {
                method = value;
                SSCSAProps.Visible = false;
                SSCSDProps.Visible = false;

                if (method == eCapSenseMode.CSA){ SSCSAProps.Visible = true;SSCSPropsSW=SSCSAProps;}
                if (method == eCapSenseMode.CSD) { SSCSDProps.Visible = true; SSCSPropsSW = SSCSDProps; }
            }
        }
    }

    #region cntSSCSProps
    public  class cntSSCSProps : UserControl
    {
        public bool isIdac = true;
        public CyAmuxBParams currectCch = null;
        public clSSProperties lastObject;
        public List<clSSProperties> listLastObj = new List<clSSProperties>();
        protected bool isGeneration = false;

        public virtual void ShowInControl() { }
        public cntSSCSProps()
            : base()
        {
        }

        public virtual void ChangeState(CyAmuxBParams cch) { isGeneration = false; }
        public void SetObject()//Visualize objects from listLastObj array
        {
            isGeneration = true;
            lastObject = (clSSProperties)clPropsComparer.Cmapare(listLastObj.ToArray());//Compare objects
            ShowInControl();//Visualize data
            isGeneration = false;

        }        
        public void Clear()
        {
            isGeneration = true;
            listLastObj.Clear();
        }
        protected void cbCustomProcessing(CheckBox cbCustom)
        {
            if (!isGeneration)
            {
                //Updating properties in Control
                lastObject.Custom = cbCustom.Checked;
                isGeneration = true;
                ShowInControl();
                isGeneration = false;
            }
        }
        public void AddProps(ElScanSlot addObject)
        {
            isGeneration = true;
            listLastObj.Add(addObject.SSProperties);
        }

        #region Values Processing
        protected void SetValue(ref int main, int val)
        {
            if (val != -1)
            {
                main = val;
            }
        }
        protected void SetValue(ref IntElement main, TextBox val)
        {
            if (val.Text != "")
            {
                main.Validate(val.Text);
                val.Text = main.Value.ToString();
            }
        }
        protected string GetValue(IntElement val)
        {
            if (val != null)
            {
                return val.Value.ToString();
            }
            return "";
        }
        #endregion
    }
    #endregion
}
