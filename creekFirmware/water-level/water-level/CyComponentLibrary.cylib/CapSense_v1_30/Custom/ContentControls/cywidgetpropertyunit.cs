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

namespace  CapSense_v1_30
{
    public partial class CyWidgetPropertyUnit : UserControl
    {
        public bool IsTouchPadMode
        {
            get { return !sCMain.Panel2Collapsed; }
            set { sCMain.Panel2Collapsed = !value; }
        }
        public CyWidgetPropertyUnit()
        {
            InitializeComponent();
            IsTouchPadMode = false;

        }
        public void GetProperties(CyElWidget widget)
        {
            if (widget != null)
            {
                labelTop.Text = widget.m_Name;
                if (!IsTouchPadMode)
                {
                    propertyGrid.SelectedObject = widget.GetProperties();
                }
                else
                {
                    propertyGrid.SelectedObject = ((CyElUnTouchPad)widget).m_Properties;
                    pGRows.SelectedObject = ((CyElUnTouchPad)widget).m_PropsRows;
                    pGCols.SelectedObject = ((CyElUnTouchPad)widget).m_PropsCols;
                    pGRows.Invalidate();
                    pGCols.Invalidate();
                }
                ((CyHAProps)propertyGrid.SelectedObject).InitializeListObj();
                propertyGrid.Invalidate();
            }
            else
            {
                labelTop.Text = "";
                propertyGrid.SelectedObject = null;
                propertyGrid.Invalidate();
                if (IsTouchPadMode)
                {
                    pGRows.SelectedObject = null;
                    pGCols.SelectedObject = null;
                    pGRows.Invalidate();
                    pGCols.Invalidate();
                }
            }
        }
    }
}
