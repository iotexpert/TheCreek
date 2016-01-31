using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace Cypress.Components.System.cy_dma_v1_50
{
    public partial class CyDMAControl : UserControl, ICyParamEditingControl
    {
        ICyInstEdit_v1 m_edit;

        public CyDMAControl(ICyInstEdit_v1 edit)
            : this()
        {
            m_edit = edit;

            //Initialize the values.
            _update();
        }

        public CyDMAControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            m_errorProvider.SetIconAlignment(m_terminationComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.SetIconAlignment(m_requestComboBox, ErrorIconAlignment.MiddleLeft);
            m_errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            m_requestComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            m_terminationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            m_terminationComboBox.Items.Add(new CyHardwareTerminationItem(CyHardwareTerminationEnum.Disabled));
            m_terminationComboBox.Items.Add(new CyHardwareTerminationItem(CyHardwareTerminationEnum.Enabled));

            m_requestComboBox.Items.Add(new CyHardwareRequestItem(CyDMAParameters.CyHardwareRequestEnum.Disabled));
            m_requestComboBox.Items.Add(new CyHardwareRequestItem(CyDMAParameters.CyHardwareRequestEnum.Derived));
            m_requestComboBox.Items.Add(new CyHardwareRequestItem(CyDMAParameters.CyHardwareRequestEnum.RisingEdge));
            m_requestComboBox.Items.Add(new CyHardwareRequestItem(CyDMAParameters.CyHardwareRequestEnum.Level));

            m_terminationComboBox.SelectedIndexChanged += new EventHandler(m_terminationComboBox_SelectedIndexChanged);
            m_requestComboBox.SelectedIndexChanged += new EventHandler(m_requestComboBox_SelectedIndexChanged);
        }

        void m_requestComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CyDMAParameters.SetHardwareRequestValue(m_edit, HardwareRequest);
        }

        void m_terminationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CyDMAParameters.SetHardwareTerminationEnabledValue(m_edit, HardwareTerminationEnabled);
        }

        bool HardwareTerminationEnabled
        {
            get
            {
                CyHardwareTerminationItem term = m_terminationComboBox.SelectedItem as CyHardwareTerminationItem;
                Debug.Assert(term != null);
                switch (term.Value)
                {
                    case CyHardwareTerminationEnum.Disabled:
                        return false;

                    case CyHardwareTerminationEnum.Enabled:
                        return true;

                    default:
                        Debug.Fail("unhandled");
                        return false;
                }
            }

            set
            {
                CyHardwareTerminationEnum val;
                val = (value) ? CyHardwareTerminationEnum.Enabled : CyHardwareTerminationEnum.Disabled;
                foreach (CyHardwareTerminationItem term in m_terminationComboBox.Items)
                {
                    if (term.Value == val)
                    {
                        m_terminationComboBox.SelectedItem = term;
                        return;
                    }
                }
                m_terminationComboBox.SelectedItem = null;
            }
        }

        CyDMAParameters.CyHardwareRequestEnum HardwareRequest
        {
            get
            {
                CyHardwareRequestItem req = m_requestComboBox.SelectedItem as CyHardwareRequestItem;
                if (req == null)
                {
                    return CyDMAParameters.CyHardwareRequestEnum.Derived;
                }

                switch (req.Value)
                {
                    case CyDMAParameters.CyHardwareRequestEnum.Disabled:
                        return CyDMAParameters.CyHardwareRequestEnum.Disabled;

                    case CyDMAParameters.CyHardwareRequestEnum.Derived:
                        return CyDMAParameters.CyHardwareRequestEnum.Derived;

                    case CyDMAParameters.CyHardwareRequestEnum.RisingEdge:
                        return CyDMAParameters.CyHardwareRequestEnum.RisingEdge;

                    case CyDMAParameters.CyHardwareRequestEnum.Level:
                        return CyDMAParameters.CyHardwareRequestEnum.Level;

                    default:
                        Debug.Fail("unhandled");
                        return CyDMAParameters.CyHardwareRequestEnum.Derived;
                }
            }

            set
            {
                foreach (CyHardwareRequestItem req in m_requestComboBox.Items)
                {
                    if (req.Value == value)
                    {
                        m_requestComboBox.SelectedItem = req;
                        return;
                    }
                }
                m_requestComboBox.SelectedItem = null;
            }
        }

        public void Update(ICyParamEditor custEditor, CyCompDevParam param)
        {
            _update();
        }

        void _update()
        {
            m_terminationComboBox.SelectedIndexChanged -= m_terminationComboBox_SelectedIndexChanged;
            m_requestComboBox.SelectedIndexChanged -= m_requestComboBox_SelectedIndexChanged;

            CyCustErr err;

            CyDMAParameters.CyHardwareRequestEnum hwReq;
            err = CyDMAParameters.GetHardwareRequestValue(m_edit, out hwReq);
            HardwareRequest = hwReq;
            m_errorProvider.SetError(m_requestComboBox, (err.IsOk) ? string.Empty : err.Message);

            bool hwTerm;
            err = CyDMAParameters.GetHardwareTerminationEnabledValue(m_edit, out hwTerm);
            HardwareTerminationEnabled = hwTerm;
            m_errorProvider.SetError(m_terminationComboBox, (err.IsOk) ? string.Empty : err.Message);

            m_terminationComboBox.SelectedIndexChanged += new EventHandler(m_terminationComboBox_SelectedIndexChanged);
            m_requestComboBox.SelectedIndexChanged += new EventHandler(m_requestComboBox_SelectedIndexChanged);
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
                if (param.IsVisible && param.TabName == CyCustomizer.BasicTabName)
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
    }

    public enum CyHardwareTerminationEnum
    {
        Disabled,
        Enabled,
    }

    public class CyHardwareTerminationItem
    {
        CyHardwareTerminationEnum m_value;

        public CyHardwareTerminationEnum Value
        {
            get { return m_value; }
        }

        public CyHardwareTerminationItem(CyHardwareTerminationEnum value)
        {
            m_value = value;
        }

        public override string ToString()
        {
            switch (m_value)
            {
                case CyHardwareTerminationEnum.Disabled:
                    return "Disabled";

                case CyHardwareTerminationEnum.Enabled:
                    return "Enabled";

                default:
                    Debug.Fail("unhandled");
                    return base.ToString();
            }
        }
    }

    public class CyHardwareRequestItem
    {
        CyDMAParameters.CyHardwareRequestEnum m_value;

        public CyDMAParameters.CyHardwareRequestEnum Value
        {
            get { return m_value; }
        }

        public CyHardwareRequestItem(CyDMAParameters.CyHardwareRequestEnum value)
        {
            m_value = value;
        }

        public override string ToString()
        {
            switch (m_value)
            {
                case CyDMAParameters.CyHardwareRequestEnum.Disabled:
                    return "Disabled";

                case CyDMAParameters.CyHardwareRequestEnum.Derived:
                    return "Derived";

                case CyDMAParameters.CyHardwareRequestEnum.RisingEdge:
                    return "Rising Edge";

                case CyDMAParameters.CyHardwareRequestEnum.Level:
                    return "Level";

                default:
                    Debug.Fail("unhandled");
                    return base.ToString();
            }
        }
    }
}
