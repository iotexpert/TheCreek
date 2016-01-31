/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Windows.Forms;

namespace USBFS_v1_60
{
    /// <summary>
    /// CyReportBase class is used as a base class for all controls 
    /// that are used to configure hid report items  
    /// </summary>
    public partial class CyReportBase : UserControl
    {
        public CyHidReportItem m_item;
        protected bool m_editMode;
        public event EventHandler UpdatedItemEvent;
        protected bool m_internalChanges;

        public CyReportBase()
        {
            InitializeComponent();
        }

        public CyReportBase(CyHidReportItem item, bool edit)
        {
            InitializeComponent();
            m_item = item;

            if (edit)
            {
                m_editMode = true;
            }
        }

        protected void OnChanged()
        {
            if (UpdatedItemEvent != null)
                UpdatedItemEvent(this, EventArgs.Empty);
        }

        protected virtual void InitValues(){}
        protected virtual void InitControls() { }
        protected void Init()
        {
            InitControls();
            if (m_editMode)
                InitValues();
        }

        public virtual bool Apply()
        { 
            return true;
        }
    }
}
