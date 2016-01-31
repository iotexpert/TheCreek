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
using System.Diagnostics;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using Cypress.Comps.PinsAndPorts.Common;

namespace Cypress.Comps.PinsAndPorts.cy_pins_v1_10
{
    public partial class CyPORControl : UserControl, ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;

        public CyPORControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            m_porComboBox.Items.Add(new CyPOR(CyPOR.CyPORMode.Unspecified));
            m_porComboBox.Items.Add(new CyPOR(CyPOR.CyPORMode.HiZAnalog));
            m_porComboBox.Items.Add(new CyPOR(CyPOR.CyPORMode.PulledUp));
            m_porComboBox.Items.Add(new CyPOR(CyPOR.CyPORMode.PulledDown));

            m_porComboBox.SelectedIndexChanged += new EventHandler(m_porComboBox_SelectedIndexChanged);
        }

        void m_porComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //User made a change, update.
            Edit.SetParamExpr(CyParamInfo.Formal_ParamName_PowerOnReset, ParamValue);
            Edit.CommitParamExprs();
        }

        public ICyInstEdit_v1 Edit
        {
            get { return m_edit; }
            set 
            { 
                m_edit = value;
                UpdateFromExprs();
            }
        }

        public string ParamValue
        {
            get
            {
                CyPOR por = m_porComboBox.SelectedItem as CyPOR;
                if (por == null)
                {
                    Debug.Fail("shouldn't happen");
                    return CyParamInfo.PORValue_Unspecified.ToString();
                }

                switch (por.PORMode)
                {
                    case CyPOR.CyPORMode.Unspecified:
                        return CyParamInfo.PORValue_Unspecified.ToString();

                    case CyPOR.CyPORMode.HiZAnalog:
                        return CyParamInfo.PORValue_HiZAnalog.ToString();

                    case CyPOR.CyPORMode.PulledUp:
                        return CyParamInfo.PORValue_PulledUp.ToString();

                    case CyPOR.CyPORMode.PulledDown:
                        return CyParamInfo.PORValue_PulledDown.ToString();

                    default:
                        Debug.Fail("unhandled");
                        return CyParamInfo.PORValue_Unspecified.ToString();
                }
            }
        }

        internal void UpdateFromExprs()
        {
            m_porComboBox.SelectedIndexChanged -= m_porComboBox_SelectedIndexChanged;

            CyParamInfo.CyPOR por;
            CyCustErr err = CyParamInfo.GetPORValue(Edit, out por);

            m_errorProvider.SetError(m_porComboBox, (err.IsNotOk) ? err.Message : string.Empty);

            switch (por)
            {
                case CyParamInfo.CyPOR.Unspecified:
                    Select(CyPOR.CyPORMode.Unspecified);
                    break;

                case CyParamInfo.CyPOR.HiZAnalog:
                    Select(CyPOR.CyPORMode.HiZAnalog);
                    break;

                case CyParamInfo.CyPOR.PulledUp:
                    Select(CyPOR.CyPORMode.PulledUp);
                    break;

                case CyParamInfo.CyPOR.PulledDown:
                    Select(CyPOR.CyPORMode.PulledDown);
                    break;

                default:
                    Debug.Fail("unhandled");
                    m_porComboBox.SelectedItem = null;
                    break;
            }

            m_porComboBox.SelectedIndexChanged += m_porComboBox_SelectedIndexChanged;
        }

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();

            foreach (string paramName in m_edit.GetParamNames())
            {
                CyCompDevParam param = m_edit.GetCommittedParam(paramName);
                if (param.TabName == CyCustomizer.ResetTabName)
                {
                    if (param.ErrorCount > 0)
                    {
                        foreach (string errMsg in param.Errors)
                        {
                            errs.Add(new CyCustErr(errMsg));
                        }
                    }
                }
            }

            return errs;
        }

        #endregion

        void Select(CyPOR.CyPORMode mode)
        {
            foreach (CyPOR porMode in m_porComboBox.Items)
            {
                if (porMode.PORMode == mode)
                {
                    m_porComboBox.SelectedItem = porMode;
                    break;
                }
            }
        }

        class CyPOR
        {
            public enum CyPORMode { Unspecified, HiZAnalog, PulledUp, PulledDown }

            CyPORMode m_mode;

            public CyPORMode PORMode { get { return m_mode; } }

            public CyPOR(CyPORMode mode)
            {
                m_mode = mode;
            }

            public override string ToString()
            {
                switch (PORMode)
                {
                    case CyPORMode.Unspecified:
                        return "Don't Care";

                    case CyPORMode.HiZAnalog:
                        return "High-Z Analog";

                    case CyPORMode.PulledUp:
                        return "Pulled-Up";

                    case CyPORMode.PulledDown:
                        return "Pulled-Down";

                    default:
                        Debug.Fail("unhandled");
                        return base.ToString();
                }
            }
        }
    }
}
