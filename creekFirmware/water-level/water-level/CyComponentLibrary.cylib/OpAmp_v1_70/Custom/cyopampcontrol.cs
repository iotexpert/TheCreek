using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
using OpAmp_v1_70;

namespace OpAmp_v1_70
{
    public partial class OPAMPcontrol : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        const int DEFAULT_MODE = 0;
        const string DEFAULT_POWER = "Low Power Over Compensated";

        public OPAMPcontrol(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            mMode.DropDownStyle = ComboBoxStyle.DropDownList;
            mPower.DropDownStyle = ComboBoxStyle.DropDownList;
            

            //Set the Opamp Mode Combo Box from Enums
            IEnumerable<string> OpampModeEnums = inst.GetPossibleEnumValues(OPAMPParameters.MODE);
            foreach (string str in OpampModeEnums)
            {
                mMode.Items.Add(str);
            }

            //Set the Opamp Power Combo Box from Enums
            IEnumerable<string> OpampPowerEnums = inst.GetPossibleEnumValues(OPAMPParameters.POWER);
            foreach (string str in OpampPowerEnums)
            {
                mPower.Items.Add(str);
            }
            mMode.SelectedItem = "OpAmp";
            mPower.SelectedItem = "Low Power Over Compensated";
            mPower.SelectedIndexChanged += new EventHandler(mPower_SelectedIndexChanged);
            mMode.SelectedIndexChanged += new EventHandler(mMode_SelectedIndexChanged);
            //UpdateFormFromParams(m_Component);
            
        }

        private void mMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(OPAMPParameters.MODE, mMode.Text);
            SetAParameter(OPAMPParameters.MODE, prm, true);
        }

        private void SetAParameter(string parameter, string value, bool CheckFocus)
        {
            if (this.ContainsFocus || !CheckFocus)
            {
                m_Component.SetParamExpr(parameter, value);
                m_Component.CommitParamExprs();
                if (m_Component.GetCommittedParam(parameter).ErrorCount != 0)
                {
                    string errors = null;
                    foreach (string err in m_Component.GetCommittedParam(parameter).Errors)
                    {
                        errors = errors + err + "\n";
                    }
                    MessageBox.Show(string.Format("Error Setting Parameter {0} with value {1}\n\nErrors:\n{2}", parameter, value, errors),
                        "Error Setting Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void UpdateFormFromParams(ICyInstEdit_v1 inst)
        {
            
            OPAMPParameters prms = new OPAMPParameters(inst);
            //Set the Opamp Mode Combo Box
            IEnumerable<string> OpampModeEnums = inst.GetPossibleEnumValues(OPAMPParameters.MODE);
            bool OpampModeFound = false;
            foreach (string str in OpampModeEnums)
            {
                if (!OpampModeFound)
                {
                    string paramcompare = inst.ResolveEnumIdToDisplay(OPAMPParameters.MODE, prms.opampMode.Expr);
                    if (paramcompare == str)
                    {
                        mMode.SelectedItem = paramcompare;
                    }
                }
            }

            OpampModeFound = true;
            if (!OpampModeFound)
            {
                mMode.Text = prms.opampMode.Expr;
            }

            //Set the Opamp Power Combo Box
            IEnumerable<string> OpampPowerEnums = inst.GetPossibleEnumValues(OPAMPParameters.POWER);
            bool OpampPowerFound = false;
            foreach (string str in OpampPowerEnums)
            {
                if (!OpampPowerFound)
                {
                    string paramcompare = inst.ResolveEnumIdToDisplay(OPAMPParameters.POWER, prms.opampPower.Expr);
                    if (paramcompare == str)
                    {
                        mPower.SelectedItem = paramcompare;
                    }
                }
            }

            OpampPowerFound = true;
            if (!OpampPowerFound)
            {
                mPower.Text = prms.opampPower.Expr;
            }
        }

        private void mPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(OPAMPParameters.POWER, mPower.Text);
            SetAParameter(OPAMPParameters.POWER, prm, true);           
            UpdateFormFromParams(m_Component);
        }
    }
}
