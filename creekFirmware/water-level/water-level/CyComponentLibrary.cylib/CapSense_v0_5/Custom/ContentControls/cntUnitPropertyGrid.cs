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

namespace  CapSense_v0_5
{
    public partial class cntUnitPropertyGrid : UserControl
    {
        public bool IsTouchPadMode
        {
            get { return !sCMain.Panel2Collapsed; }
            set { sCMain.Panel2Collapsed = !value; }
        }
        public cntUnitPropertyGrid()
        {
            InitializeComponent();
            IsTouchPadMode = false;

        }
        public void GetProperties(ElWidget widget)
        {
            if (widget != null)
            {
                labelTop.Text = widget.Name;
                if (!IsTouchPadMode)
                {
                    propertyGrid.SelectedObject = widget.GetProps();
                }
                else
                {
                    propertyGrid.SelectedObject = ((ElUnTouchPad)widget).Properties;
                    pGRows.SelectedObject = ((ElUnTouchPad)widget).PropsRows;
                    pGCols.SelectedObject = ((ElUnTouchPad)widget).PropsCols;
                    pGRows.Invalidate();
                    pGCols.Invalidate();
                }
                ((HAProps)propertyGrid.SelectedObject).InitializeListObj();
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
