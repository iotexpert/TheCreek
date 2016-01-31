/*******************************************************************************
* Copyright 2010, Cypress Semiconductor Corporation.  All rights reserved.
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

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.Primitives.cy_analog_constraint_v1_50
{
    public partial class CyGlobalAnalogLocationControl :
		UserControl,
		ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;

        public CyGlobalAnalogLocationControl(
			ICyInstEdit_v1 edit,
			IEnumerable<CyGlobalAnalogLoc> gaLocs,
			string selectedGALocID)
        {
            InitializeComponent();

            m_edit = edit;
            GALocs = gaLocs;
            SelectedGALocId = selectedGALocID;

            m_gblAnaLocComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_gblAnaLocComboBox.SelectedIndexChanged += new EventHandler(m_gaLocComboBox_SelectedIndexChanged);
        }

        private void PerformDispose()
        {
            m_gblAnaLocComboBox.SelectedIndexChanged -= m_gaLocComboBox_SelectedIndexChanged;
        }

        IEnumerable<CyGlobalAnalogLoc> GALocs
        {
            get 
            {
                foreach (CyGlobalAnalogLoc obj in m_gblAnaLocComboBox.Items)
                {
                    yield return obj;
                }
            }

            set
            {
                m_gblAnaLocComboBox.Items.Clear();
                foreach (CyGlobalAnalogLoc gaLoc in value)
                {
                    m_gblAnaLocComboBox.Items.Add(gaLoc);
                }
            }
        }

        string SelectedGALocId
        {
            get 
            { 
                CyGlobalAnalogLoc gaLoc = m_gblAnaLocComboBox.SelectedItem as CyGlobalAnalogLoc;
                if (gaLoc != null)
                {
                    return gaLoc.ID;
                }
                return string.Empty;
            }

            set 
            {
                bool found = false;
                foreach (CyGlobalAnalogLoc gaLoc in GALocs)
                {
                    if (gaLoc.ID.Equals(value))
                    {
                        m_gblAnaLocComboBox.SelectedItem = gaLoc;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    m_gblAnaLocComboBox.SelectedItem = null;
                }
            }
        }

        CyGlobalAnalogLoc SelectedGALoc
        {
            get
            {
                CyGlobalAnalogLoc gaLoc = m_gblAnaLocComboBox.SelectedItem as CyGlobalAnalogLoc;
                return gaLoc;
            }
        }

        void m_gaLocComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CyGlobalAnalogLoc gaLoc = SelectedGALoc;
            if (gaLoc != null)
            {
                CyGlobalAnalogLocInfo.SetGblAnaLocIDExpr(m_edit, gaLoc.ID);
                CyGlobalAnalogLocInfo.SetGblAnaLocNameExpr(m_edit, gaLoc.Name);
                m_edit.CommitParamExprs();
            } 
        }

        public void Update(ICyParamEditor custEditor, CyCompDevParam param)
        {
            string id = CyGlobalAnalogLocInfo.GetGblAnaLocIDValue(m_edit);
            SelectedGALocId = id;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            // No errors are displayed in the control. 
            // All validation is done by DRCs.
            return new CyCustErr[] { };
        }

        #endregion
    }

    public class CyGlobalAnalogLoc : IComparable<CyGlobalAnalogLoc>
    {
        string m_id;
        string m_name;

        public string ID { get { return m_id; } }
        public string Name { get { return m_name; } }

        public CyGlobalAnalogLoc(string id, string name)
        {
            m_id = id;
            m_name = name;

            if (m_id == null || m_name == null)
            {
                throw new ArgumentNullException();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            CyGlobalAnalogLoc gaLoc = obj as CyGlobalAnalogLoc;
            if (gaLoc != null)
            {
                return (m_id.Equals(gaLoc.m_id) && m_name.Equals(gaLoc.m_name));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return m_id.GetHashCode() ^ m_name.GetHashCode();
        }

		public int CompareTo(CyGlobalAnalogLoc other)
		{
			return m_name.CompareTo(other.m_name);
		}
    }

}
//[]//
