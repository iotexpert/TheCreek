using System;
using System.Collections.Generic;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using System.Windows.Forms;

namespace VectorCAN_v1_0
{
    [CyCompDevCustomizer()]
    public class CyCustomizer: ICyParamEditHook_v1
    {
        #region ICyParamEditHook_v1 Members
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            CyParameters.GLOBAL_EDIT_MODE = false;
            CyCSParamEditTemplate.RUN_MODE = true;
            CyParameters packParams = new CyParameters(edit, termQuery);
            CyGeneralTab m_generalTab = new CyGeneralTab(packParams);

            ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();

            CyParamExprDelegate dataChanged =
            delegate(ICyParamEditor custEditor, CyCompDevParam param)
            {
                CyParameters.GLOBAL_EDIT_MODE = false;
                m_generalTab.UpdateUI();
                CyParameters.GLOBAL_EDIT_MODE = true;
            };
            editor.AddCustomPage(cvresources.GeneralTab, m_generalTab, dataChanged, m_generalTab.TabName);
            editor.AddDefaultPage("Built-in", "Built-in");
            edit.NotifyWhenDesignUpdates(m_generalTab.UpdateClock);
            m_generalTab.UpdateUI();

            CyParameters.GLOBAL_EDIT_MODE = true;
            
            return editor.ShowDialog();
        }

        public bool EditParamsOnDrop
        {
            get
            {
                return false;
            }
        }

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE; ;
        }

        #endregion

    }
    #region CyMyICyParamEditTemplate
    public class CyCSParamEditTemplate : UserControl, ICyParamEditingControl
    {
        public static bool RUN_MODE = false;
        protected CyParameters m_packParams = null;

        public virtual string TabName
        {
            get { return "Empty"; }
        }

        public CyCSParamEditTemplate()
        {
            this.Load += new EventHandler(CyMyICyParamEditTemplate_Load);
        }

        void CyMyICyParamEditTemplate_Load(object sender, EventArgs e)
        {
            if (RUN_MODE)
                this.Dock = DockStyle.Fill;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }
        #region ICyParamEditingControl Members
        public Control DisplayControl
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets any errors that exist for parameters on the DisplayControl.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CyCustErr> GetErrors()
        {
            if (m_packParams != null && m_packParams.m_edit != null)
            {
                ICyInstQuery_v1 edit = m_packParams.m_edit;
                foreach (string paramName in edit.GetParamNames())
                {
                    CyCompDevParam param = edit.GetCommittedParam(paramName);
                    if (param.IsVisible && param.TabName == TabName)
                        if (param.ErrorCount > 0)
                        {
                            foreach (string errMsg in param.Errors)
                            {
                                yield return new CyCustErr(errMsg);
                            }
                        }
                }              
            }
        }

        #endregion
    }
    #endregion
}
