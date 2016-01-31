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

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.System.cy_gsref_v1_0
{
    public partial class CyGlobalSignalRefIDControl : UserControl, ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;

        public CyGlobalSignalRefIDControl(ICyInstEdit_v1 edit, IEnumerable<CyGlobalSignalRef> gsRefs, string selectedGSRefID)
        {
            InitializeComponent();

            m_edit = edit;
            GSRefs = gsRefs;
            SelectedGSRefId = selectedGSRefID;

            m_gsRefComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_gsRefComboBox.SelectedIndexChanged += new EventHandler(m_gsRefComboBox_SelectedIndexChanged);
        }

        private void PerformDispose()
        {
            m_gsRefComboBox.SelectedIndexChanged -= m_gsRefComboBox_SelectedIndexChanged;
        }

        IEnumerable<CyGlobalSignalRef> GSRefs
        {
            get 
            {
                foreach (CyGlobalSignalRef obj in m_gsRefComboBox.Items)
                {
                    yield return obj;
                }
            }

            set
            {
                m_gsRefComboBox.Items.Clear();
                foreach (CyGlobalSignalRef gsRef in value)
                {
                    m_gsRefComboBox.Items.Add(gsRef);
                }
            }
        }

        string SelectedGSRefId
        {
            get 
            { 
                CyGlobalSignalRef gsRef = m_gsRefComboBox.SelectedItem as CyGlobalSignalRef;
                if (gsRef != null)
                {
                    return gsRef.ID;
                }
                return string.Empty;
            }

            set 
            {
                bool found = false;
                foreach (CyGlobalSignalRef gsRef in GSRefs)
                {
                    if (gsRef.ID.Equals(value))
                    {
                        m_gsRefComboBox.SelectedItem = gsRef;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    m_gsRefComboBox.SelectedItem = null;
                }
            }
        }

        CyGlobalSignalRef SelectedGSRef
        {
            get
            {
                CyGlobalSignalRef gsRef = m_gsRefComboBox.SelectedItem as CyGlobalSignalRef;
                return gsRef;
            }
        }

        void m_gsRefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CyGlobalSignalRef gsRef = SelectedGSRef;
            if (gsRef != null)
            {
                CyGlobalSignalRefInfo.SetGSRefIDExpr(m_edit, gsRef.ID);
                CyGlobalSignalRefInfo.SetGSRefNameExpr(m_edit, gsRef.Name);
                m_edit.CommitParamExprs();
            } 
        }

        public void Update(ICyParamEditor custEditor, CyCompDevParam param)
        {
            string id = CyGlobalSignalRefInfo.GetGSRefIDValue(m_edit);
            SelectedGSRefId = id;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            //no errors are displayed in the control. All validation is done by DRCs.
            return new CyCustErr[] { };
        }

        #endregion
    }

    public class CyGlobalSignalRef
    {
        string m_id;
        string m_name;

        public string ID { get { return m_id; } }
        public string Name { get { return m_name; } }

        public CyGlobalSignalRef(string id, string name)
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
            CyGlobalSignalRef gsRef = obj as CyGlobalSignalRef;
            if (gsRef != null)
            {
                return (m_id.Equals(gsRef.m_id) && m_name.Equals(gsRef.m_name));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return m_id.GetHashCode() ^ m_name.GetHashCode();
        }
    }
}
