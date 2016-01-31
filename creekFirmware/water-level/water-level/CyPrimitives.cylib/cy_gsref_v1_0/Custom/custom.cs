/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.Common.Base.Controls;

namespace Cypress.Components.System.cy_gsref_v1_0
{
    public class CyCustomizer : ICyParamEditHook_v1, ICyVerilogCustomize_v1, 
        ICyShapeCustomize_v1, ICyToolTipCustomize_v1
    {
        //-----------------------------

        const string m_builtinTabName = "Built-in";
        const string m_basicTabName = "Basic";

        //-----------------------------

        #region ICyParamEditHook_v1 Members

        public CyCompDevParamEditorMode GetEditorMode()
        {
            return CyCompDevParamEditorMode.COMPLETE;
        }

        /// <summary>
        /// Displays a custom parameter editor to the user to allow them to edit the params.
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            if (edit.DesignQuery != null && edit.DesignQuery.DesignInfoAvailable)
            {
                List<CyGlobalSignalRef> gsRefs = new List<CyGlobalSignalRef>();
                List<string> gsIDs = new List<string>(edit.DesignQuery.GSRefIDs);
                gsIDs.Sort();

                //If the id has not been set it will be the empty string, in which case the default ref will be used.
                string defaultName = string.Format("Default <{0}>", 
                    edit.DesignQuery.GetGSRefName(edit.DesignQuery.GSRefDefaultID));
                gsRefs.Add(new CyGlobalSignalRef(string.Empty, defaultName));

                foreach (string id in gsIDs)
                {
                    string name = edit.DesignQuery.GetGSRefName(id);
                    string abbrName = edit.DesignQuery.GetGSRefAbbr(id);
                    gsRefs.Add(new CyGlobalSignalRef(id, string.Format("{0} ({1})", name, abbrName)));
                }

                string selectedID = CyGlobalSignalRefInfo.GetGSRefIDValue(edit);
                CyGlobalSignalRefIDControl control = new CyGlobalSignalRefIDControl(edit, gsRefs, selectedID);
                control.Dock = DockStyle.Fill;

                ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
                editor.AddCustomPage(m_basicTabName, control, control.Update, m_basicTabName);
                editor.AddDefaultPage(m_builtinTabName, m_builtinTabName);
                return editor.ShowDialog();
            }
            else
            {
                //Show error message. This component cannot be configured without a DesigneQuery available.
                CyMsgControl msgControl = new CyMsgControl();
                msgControl.Text = "Unable to configure this Global Signal Ref component. This component requires " +
                                  "family specific information. It cannot be added at the Generic level in a library " +
                                  "project. To use this component add it to a family or device specific " +
                                  "implementation.";

                ICyParamEditor editor = edit.CreateParamEditor(msgControl);
                return editor.ShowDialog();
            }
        }

        /// <summary>
        /// Gets whether or not EditParams should be called when initailly
        /// dropped onto a canvas.
        /// </summary>
        public bool EditParamsOnDrop
        {
            get { return false; }
        }

        #endregion

        //-----------------------------

        #region ICyVerilogCustomize_v1 Members

        /// <summary>
        /// Needed to convert the default empty string to the default ID.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="termQuery"></param>
        /// <param name="codeSnippet"></param>
        /// <returns></returns>
        public CyCustErr CustomizeVerilog(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_gsref_v1_0", query.InstanceName);

            //Add Generics.
            foreach (string paramName in query.GetParamNames())
            {
                CyCompDevParam param = query.GetCommittedParam(paramName);
                if (param.IsHardware)
                {
                    string value;
                    value = (CyGlobalSignalRefInfo.IsGUIDParam(param)) ? "\"" + GetGSRefID(query) + "\"" : param.Value;
                    vw.AddGeneric(param.Name, value);
                }
            }

            //Add Ports.
            foreach (string termCanonicalName in termQuery.GetTerminalNames())
            {
                string value = termQuery.GetTermSigSegName(termCanonicalName);

                CyCompDevTermDir dir = termQuery.GetTermDirection(termCanonicalName);
                bool hasNoDrivers = termQuery.GetHasNoDrivers(termCanonicalName);
                if (dir != CyCompDevTermDir.OUTPUT && hasNoDrivers)
                {
                    value = termQuery.GetTermDefaultVlogExpr(termCanonicalName);
                }

                string baseTermName = termQuery.GetTermBaseName(termCanonicalName);
                vw.AddPort(baseTermName, value);
            }

            codeSnippet = vw.ToString();
            return err;
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        const string InstNameTag = "name";
        const string BodyNameTag = "body";

        /// <summary>
        /// Add the instance name. Use the abbreviated version of the name for this purpose.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="shapeEdit"></param>
        /// <param name="termEdit"></param>
        /// <returns></returns>
        public CyCustErr CustomizeShapes(ICyInstQuery_v1 query, ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            string dspName = GetGSRefAbbrName(query);

            RectangleF bounds = shapeEdit.GetShapeBounds(BodyNameTag);
            StringFormat fmt = new StringFormat();
            fmt.Alignment = StringAlignment.Far;
            fmt.LineAlignment = StringAlignment.Center;

            CyCustErr err = shapeEdit.CreateAnnotation(new string[] { InstNameTag },
                dspName,
                new PointF(bounds.Left, bounds.Top + (bounds.Height / 2.0f)),
                new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                fmt);
            if (err.IsNotOk) return err;

            err = shapeEdit.SetFillColor(InstNameTag, Color.Black);
            if (err.IsNotOk) return err;

            err = shapeEdit.ClearOutline(InstNameTag);
            if (err.IsNotOk) return err;

            return err;
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1

        /// <summary>
        /// Looks up the human readable name for the signal.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            string id = CyGlobalSignalRefInfo.GetGSRefIDValue(query);
            bool isDefault = string.IsNullOrEmpty(id);
            string refName = GetGSRefName(query);

            string newToolTip = query.DefaultToolTipText;

            newToolTip += Environment.NewLine;
            newToolTip += "GSRefName = " + ((isDefault) ? "Default <" + refName + ">" : refName);

            return newToolTip;
        }

        #endregion

        //-----------------------------

        /// <summary>
        /// Gets the ID substituting the Default ID for the empty string if needed.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGSRefID(ICyInstQuery_v1 query)
        {
            string id = CyGlobalSignalRefInfo.GetGSRefIDValue(query);

            //If the guid is not set, use the default Global Signal Reference.
            if(string.IsNullOrEmpty(id) && query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                id = query.DesignQuery.GSRefDefaultID;
            }

            return id;
        }

        /// <summary>
        /// Gets the human readable name of the signal. If the ID is empty the Default ID is used.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGSRefName(ICyInstQuery_v1 query)
        {
            if (query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                string id = GetGSRefID(query);
                string name = query.DesignQuery.GetGSRefName(id);
                if (string.IsNullOrEmpty(name) == false)
                {
                    return name;
                }
            }
            return CyGlobalSignalRefInfo.GetGSRefNameValue(query);
        }

        /// <summary>
        /// Gets the abbreviated name for the signal. If the ID is empty the Default ID is used.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGSRefAbbrName(ICyInstQuery_v1 query)
        {
            if (query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                string id = GetGSRefID(query);
                string name = query.DesignQuery.GetGSRefAbbr(id);
                if (string.IsNullOrEmpty(name) == false)
                {
                    return name;
                }
            }
            return CyGlobalSignalRefInfo.GetGSRefNameValue(query);
        }

        //-----------------------------

        class CyMsgControl : CyCenteredTextControl, ICyParamEditingControl
        {
            #region ICyParamEditingControl Members

            public Control DisplayControl
            {
                get { return this; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                return new CyCustErr[] { };
            }

            #endregion
        }
    }

    public static class CyGlobalSignalRefInfo
    {
        const string ParamNameGSRefID = "guid";
        const string ParamNameGSRefName = "GSRefName";

        public static string GetGSRefIDValue(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameGSRefID);
            return param.Value;
        }

        public static void SetGSRefIDExpr(ICyInstEdit_v1 edit, string expr)
        {
            edit.SetParamExpr(ParamNameGSRefID, expr);
        }

        public static string GetGSRefNameValue(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameGSRefName);
            return param.Value;
        }

        public static void SetGSRefNameExpr(ICyInstEdit_v1 edit, string expr)
        {
            edit.SetParamExpr(ParamNameGSRefName, expr);
        }

        public static bool IsGUIDParam(CyCompDevParam param)
        {
            return param.Name == ParamNameGSRefID;
        }
    }
}
