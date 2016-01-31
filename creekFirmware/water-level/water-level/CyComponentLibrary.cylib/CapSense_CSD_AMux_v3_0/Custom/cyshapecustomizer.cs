/*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CyDesigner.Extensions.Common;
using CapSense_CSD_AMux_v3_0.LogicGateCustomizer;
using System.Diagnostics;
using System.Drawing;
using CyDesigner.Extensions.Gde;

namespace CyCustomizer.CapSense_CSD_AMux_v3_0
{
    public partial class CyCustomizer : ICyShapeCustomize_v1
    {
        const string m_iBaseTerminalName = "Sensors";
        private List<string> GetTermNames(ICyInstQuery_v1 instQuery, ICyTerminalQuery_v1 termQuery,
            ref uint numTerminals)
        {
            // Get parameters
            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam("Channels");
            uint numSns = uint.Parse(numTerminals_param.Value);
            CyCompDevParam b_IDACVis_param = instQuery.GetCommittedParam("IdacEnable");
            bool b_IDACVis = Convert.ToBoolean(b_IDACVis_param.Value.ToString());
            
            CyCompDevParam b_VrefVis_param = instQuery.GetCommittedParam("VrefEnable");
            bool b_VrefVis = Convert.ToBoolean(b_VrefVis_param.Value.ToString());

            List<string> listNames = new List<string>();
            List<string> inASigSegNames = new List<string>();

            string sigSegName = termQuery.GetTermSigSegScalarName(termQuery.GetTermName(m_iBaseTerminalName));
            if (numSns > 1)
            {
                sigSegName = sigSegName.Substring(0, sigSegName.IndexOf("["));
                for (int i = 0; i < numSns; i++)
                {
                    inASigSegNames.Add(sigSegName + "[" + i + "]");
                }
            }
            else inASigSegNames.Add(sigSegName);


            listNames.Add("Cmod");
            listNames.Add("vpCmp");
            if (b_IDACVis)
            {
                listNames.Add("Idac");
            }
            if (b_VrefVis)
            {
                listNames.Add("Vref");
            }
            for (int i = 0; i < listNames.Count; i++)
            {
                sigSegName = termQuery.GetTermSigSegScalarName(listNames[i]);
                inASigSegNames.Add(sigSegName);
            }

            numTerminals = (uint)inASigSegNames.Count;
            return inASigSegNames;
        }

        #region ICyShapeCustomize_v1 Members

        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 instQuery,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            //Debugger.Launch();
            CyCustErr err;
            // We leave the symbol as it is for symbol preview
            if (instQuery.IsPreviewCanvas)
                return CyCustErr.OK;

            // Read Parameters
            CyCompDevParam numTerminals_param = instQuery.GetCommittedParam("Channels");
            uint numTerminals = uint.Parse(numTerminals_param.Value);

            string termName_i = termEdit.GetTermName(m_iBaseTerminalName);

            uint maxBitIndex = numTerminals - 1;
            err = termEdit.TerminalRename(
                termName_i, string.Format("{0}[{1}:0]", m_iBaseTerminalName, maxBitIndex.ToString()));
            if (err.IsNotOK) return err;

            return CyCustErr.OK;
        }
        #endregion
    }
}
