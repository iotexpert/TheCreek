using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using TIA_v1_60;

namespace TIA_v1_60
{
    public partial class TIAcontrol : UserControl
    {
        public ICyInstEdit_v1 m_Component = null;
        const string DEFAULT_CAPFB = "4.6 pF";
        const string DEFAULT_POWER = "Medium Power";
        const string DEFAULT_MINVDDA = "2.7 V or greater";
        const string DEFAULT_RESFB = "20k ohms";

        public TIAcontrol(ICyInstEdit_v1 inst)
        {
            m_Component = inst;
            InitializeComponent();
            
            //Set the TIA capacitive feedback Combo Box from Enums
            IEnumerable<string> capFbEnums = inst.GetPossibleEnumValues(TIAParameters.CAP_FEEDBACK);
            foreach (string str in capFbEnums)
            {
                mCapFbk.Items.Add(str);
            }

            //Set the TIA power Combo Box from Enums
            IEnumerable<string> powerEnums = inst.GetPossibleEnumValues(TIAParameters.POWER);
            foreach (string str in powerEnums)
            {
                mPowr.Items.Add(str);
            }
            //Set the Resistive feedback Combo Box from Enums
            IEnumerable<string> resFbEnums = inst.GetPossibleEnumValues(TIAParameters.RES_FEEDBACK);
            foreach (string str in resFbEnums)
            {
                mResFbk.Items.Add(str);
            }
            

            if (m_Component != null)
            {
                UpdateFormFromParams(m_Component);
            }
        }
        // Save current parameter selection
        private void SaveParameters(string parameter, string value, bool CheckFocus)
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
            TIAParameters prms = new TIAParameters(inst);
            //Set the TIA capacitive feedback Combo Box
            IEnumerable<string> capfbEnums = inst.GetPossibleEnumValues(TIAParameters.CAP_FEEDBACK);
            bool capfbFound = false;
            foreach (string str in capfbEnums)
            {
                if (!capfbFound)
                {
                    string paramcompare = inst.ResolveEnumIdToDisplay(TIAParameters.CAP_FEEDBACK, prms.tiaCapFb.Expr);
                    if (paramcompare == str)
                    {
                        mCapFbk.SelectedItem = paramcompare;
                    }
                }
            }

            capfbFound = true;
            if (!capfbFound)
            {

                mCapFbk.Text = prms.tiaCapFb.Expr;
            }

            //Set the TIA Power Combo Box
            IEnumerable<string> powerEnums = inst.GetPossibleEnumValues(TIAParameters.POWER);
            bool powerFound = false;
            foreach (string str in powerEnums)
            {
                if (!powerFound)
                {
                    string paramcompare2 = inst.ResolveEnumIdToDisplay(TIAParameters.POWER, prms.tiaPower.Expr);
                    if (paramcompare2 == str)
                    {
                        mPowr.SelectedItem = paramcompare2;
                    }
                }
            }

            powerFound = true;
            if (!powerFound)
            {

                mPowr.Text = prms.tiaPower.Expr;
            }

            //Set the TIA Resistive feedback Combo Box
            IEnumerable<string> resfbEnums = inst.GetPossibleEnumValues(TIAParameters.RES_FEEDBACK);
            bool resfbFound = false;
            foreach (string str in resfbEnums)
            {
                if (!resfbFound)
                {
                    string paramcompare3 = inst.ResolveEnumIdToDisplay(TIAParameters.RES_FEEDBACK, prms.tiaResFb.Expr);
                    if (paramcompare3 == str)
                    {
                        mResFbk.SelectedItem = paramcompare3;
                    }
                }
            }

            resfbFound = true;
            if (!resfbFound)
            {

                mResFbk.Text = prms.tiaResFb.Expr;
            }
        }

        // Handling Capactive feedback selection
        private void mCapFbk_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(TIAParameters.CAP_FEEDBACK, mCapFbk.Text);
            SaveParameters(TIAParameters.CAP_FEEDBACK, prm, true);
            //if(mCapFbk.SelectedIndexChanged             
            CalculateDbFreq();
        }

        private void CalculateDbFreq()
        {
            const int MinPow = 0;
            const int LowPow = 1;
            const int MidPow = 2;
            const int HighPow = 3;
            double[] CfParam = { 0, 1.3, 3.3, 4.6 };
            double[] RbParam = { 20, 30, 40, 80, 120, 250, 500, 1000 };
            int cfIndex = Convert.ToInt32(m_Component.GetCommittedParam(TIAParameters.CAP_FEEDBACK).Value);
            int rbIndex = Convert.ToInt32(m_Component.GetCommittedParam(TIAParameters.RES_FEEDBACK).Value);
            string pwr = m_Component.GetCommittedParam(TIAParameters.POWER).Value;
            double term1, result5;
            double result6;
            int result7;
            double GBW = 0;
          
            if (pwr == MinPow.ToString())
            {
                GBW = 200000;
            }
            else if (pwr == LowPow.ToString())
            {
                GBW = 400000;
            }
            else if (pwr == MidPow.ToString())
            {
                GBW = 600000;
            }
            else if (pwr == HighPow.ToString())
            {
                GBW = 1000000;
            }

            term1 = 2 * Math.PI * (RbParam[rbIndex] * Math.Pow(10, 3)) * (CfParam[cfIndex] * Math.Pow(10, -12));
           
            result5 = (1 / Math.Sqrt(((term1 * term1)) + (1 / ((GBW) * (GBW)))));
            result6 = result5 / 1000;
            result7 = Convert.ToInt32(result6);
            DbFreq.Text = result7.ToString() +" kHz";  
            m_Component.SetParamExpr(TIAParameters.FCORNER, result7.ToString() + " kHz");
            m_Component.CommitParamExprs();
            
        }

        // Handling Power selection
        private void mPowr_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string prm = m_Component.ResolveEnumDisplayToId(TIAParameters.POWER, mPowr.Text);
            SaveParameters(TIAParameters.POWER, prm, true);
            CalculateDbFreq();
        }

        // Handling Resistive feedback selection
        private void mResFbk_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prm = m_Component.ResolveEnumDisplayToId(TIAParameters.RES_FEEDBACK, mResFbk.Text);
            SaveParameters(TIAParameters.RES_FEEDBACK, prm, true);
            CalculateDbFreq();
        }
    }
}
