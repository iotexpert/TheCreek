/*******************************************************************************
* Copyright 2010, Cypress Semiconductor Corporation.  All rights reserved.
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

namespace Cypress.Components.Primitives.cy_analog_constraint_v1_50
{
    public class CyCustomizer :
        ICyParamEditHook_v1,
        ICyVerilogCustomize_v1,
        ICyShapeCustomize_v1,
        ICyToolTipCustomize_v1
    {
        CyGlobalAnalogLocCustomizerHelper m_helper = new CyGlobalAnalogLocCustomizerHelper();

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
            return m_helper.EditParams(edit, termQuery, mgr);
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
            return m_helper.CustomizeVerilog(CyGlobalAnalogLocType.Constraint, query, termQuery, out codeSnippet);
        }

        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Members

        /// <summary>
        /// Add the short location name inside the box.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="shapeEdit"></param>
        /// <param name="termEdit"></param>
        /// <returns></returns>
        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            return m_helper.CustomizeShapes(query, shapeEdit, termEdit);
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1

        /// <summary>
        /// Looks up the human readable name for the location.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            return m_helper.CustomizeToolTip(query, termQuery);
        }

        #endregion
    }

    public static class CyGlobalAnalogLocInfo
    {
        const string ParamNameGblAnaLocID = "guid";
        const string ParamNameGblAnaLocName = "name";

        public static string GetGblAnaLocIDValue(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameGblAnaLocID);
            return param.Value;
        }

        public static void SetGblAnaLocIDExpr(ICyInstEdit_v1 edit, string expr)
        {
            edit.SetParamExpr(ParamNameGblAnaLocID, expr);
        }

        public static string GetGblAnaLocNameValue(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameGblAnaLocName);
			return param.Value;
        }

        public static void SetGblAnaLocNameExpr(ICyInstEdit_v1 edit, string expr)
        {
            edit.SetParamExpr(ParamNameGblAnaLocName, expr);
        }

        public static bool IsGUIDParam(CyCompDevParam param)
        {
            return param.Name == ParamNameGblAnaLocID;
        }
    }

    public class CyGlobalAnalogLocCustomizerHelper
    {
        // Note: Both cy_analog_constraint and cy_analog_reserve
        // component belong to "system components" catagory. They 
        // use design queries. 

        //-----------------------------

        const string m_builtinTabName = "Built-in";
        const string m_basicTabName = "Basic";

        //-----------------------------

        #region ICyParamEditHook_v1 Helper Routine
        /// <summary>
        /// Helper routine to build a custom parameter editor to the user to allow them to edit the params.
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="termQuery"></param>
        /// <param name="mgr"></param>
        /// <returns></returns>
        public DialogResult EditParams(ICyInstEdit_v1 edit, ICyTerminalQuery_v1 termQuery, ICyExpressMgr_v1 mgr)
        {
            if (edit.DesignQuery != null && edit.DesignQuery.DesignInfoAvailable)
            {
                List<CyGlobalAnalogLoc> gaLocs = new List<CyGlobalAnalogLoc>();
                List<string> gaIDs = new List<string>(edit.DesignQuery.GblAnaLocIDs);

                foreach (string id in gaIDs)
                {
                    string name = edit.DesignQuery.GetGblAnaLocName(id);
                    string abbrName = edit.DesignQuery.GetGblAnaLocAbbr(id);
                    gaLocs.Add(new CyGlobalAnalogLoc(id, string.Format("{0} ({1})", name, abbrName)));
                }
                gaLocs.Sort();

                //If the id has not been set it will be the empty string, in which case the default ref will be used.
                string defaultName = string.Format("Default <{0}>",
                    edit.DesignQuery.GetGblAnaLocName(edit.DesignQuery.GblAnaLocDefaultID));
                gaLocs.Insert(0, new CyGlobalAnalogLoc(string.Empty, defaultName));

                string selectedID = CyGlobalAnalogLocInfo.GetGblAnaLocIDValue(edit);
                CyGlobalAnalogLocationControl control = new CyGlobalAnalogLocationControl(edit, gaLocs, selectedID);
                control.Dock = DockStyle.Fill;

                ICyTabbedParamEditor editor = edit.CreateTabbedParamEditor();
                editor.AddCustomPage(m_basicTabName, control, control.Update, m_basicTabName);
                editor.AddDefaultPage(m_builtinTabName, m_builtinTabName);
                return editor.ShowDialog();
            }
            else
            {
                //Show error message. This component cannot be configured without a DesignQuery available.
                CyMsgControl msgControl = new CyMsgControl();
                msgControl.Text = "Unable to configure this Global Resource Constraint component. "
                    + "This component requires "
                    + "family specific information. It cannot be added at the Generic level in a library "
                    + "project. To use this component add it to a family or device specific "
                    + "implementation.";

                ICyParamEditor editor = edit.CreateParamEditor(msgControl);
                return editor.ShowDialog();
            }
        }

        class CyMsgControl : CyCenteredTextControl, ICyParamEditingControl
        {
            #region ICyParamEditingControl Members

            public Control DisplayControl
            {
                get { return this; }
            }

            public IEnumerable<CyCustErr> GetErrors()
            {
                // This component's dialog doesn't generate any error.
                // Otherwise, this method should return error generated
                // from this component customizer dialog.
                return new CyCustErr[] { };
            }

            #endregion
        }

        #endregion

        //-----------------------------

        #region ICyVerilogCustomize_v1 Helper Routine
        /// <summary>
        /// Helper routine to build Customizer Verilog
        /// </summary>
        /// <param name="query"></param>
        /// <param name="termQuery"></param>
        /// <param name="codeSnippet"></param>
        /// <returns></returns>
        public CyCustErr CustomizeVerilog(CyGlobalAnalogLocType type, ICyInstQuery_v1 query, 
            ICyTerminalQuery_v1 termQuery, out string codeSnippet)
        {
            CyCustErr err = CyCustErr.Ok;
            codeSnippet = string.Empty;
            CyVerilogWriter vw = new CyVerilogWriter("cy_analog_location_v1_0", query.InstanceName);

            //Add Generics.
            foreach (string paramName in query.GetParamNames())
            {
                CyCompDevParam param = query.GetCommittedParam(paramName);
                if (param.IsHardware)
                {
                    string value;
                    value = (CyGlobalAnalogLocInfo.IsGUIDParam(param))
                        ? GetGblAnaLocID(query)
                        : param.Value;
                    value = String.Format("\"{0}\"", value);
                    vw.AddGeneric(param.Name, value);
                }
            }

            //Add Ports.
            switch (type)
            {
                case CyGlobalAnalogLocType.Constraint:
                    AddPortsDefine(termQuery, vw);
                    break;
                case CyGlobalAnalogLocType.Reserve:
                    vw.AddPort("connect", string.Empty);
                    break;
                default:
                    break;
            }

            codeSnippet = vw.ToString();
            return err;
        }

        void AddPortsDefine(ICyTerminalQuery_v1 termQuery, CyVerilogWriter vw)
        {
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
        }
        #endregion

        //-----------------------------

        #region ICyShapeCustomize_v1 Helper Routine

        const string InstNameTag = "name";
        const string BodyNameTag = "body";

        /// <summary>
        /// Add the short location name inside the box.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="shapeEdit"></param>
        /// <param name="termEdit"></param>
        /// <returns></returns>
        public CyCustErr CustomizeShapes(
            ICyInstQuery_v1 query,
            ICySymbolShapeEdit_v1 shapeEdit,
            ICyTerminalEdit_v1 termEdit)
        {
            string dspName = GetGblAnaLocAbbrName(query);

            RectangleF bounds = shapeEdit.GetShapeBounds(BodyNameTag);
            StringFormat fmt = new StringFormat();
            fmt.Alignment = StringAlignment.Center;
            fmt.LineAlignment = StringAlignment.Center;

            CyCustErr err = shapeEdit.CreateAnnotation(new string[] { InstNameTag },
                dspName,
                new PointF(bounds.Left + (bounds.Width / 2.0f), bounds.Top + (bounds.Height / 2.0f)),
                new Font("Microsoft Sans Serif" , 10, FontStyle.Regular),
                fmt);
            if (err.IsNotOk) return err;

            err = shapeEdit.SetFillColor(InstNameTag, Color.White);
            if (err.IsNotOk) return err;

            err = shapeEdit.ClearOutline(InstNameTag);
            if (err.IsNotOk) return err;

            return err;
        }

        #endregion

        //-----------------------------

        #region ICyToolTipCustomize_v1 Helper Routine

        /// <summary>
        /// Helper routine to customizer componnet tool tip 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="termQuery"></param>
        /// <returns></returns>
        public string CustomizeToolTip(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery)
        {
            string id = CyGlobalAnalogLocInfo.GetGblAnaLocIDValue(query);
            bool isDefault = string.IsNullOrEmpty(id);
            string refName = GetGblAnaLocName(query);

            string newToolTip = query.DefaultToolTipText;

            newToolTip += Environment.NewLine;
            newToolTip += "GblAnaLocName = " + ((isDefault) ? "Default <" + refName + ">" : refName);

            return newToolTip;
        }

        #endregion

        //-----------------------------

        /// <summary>
        /// Gets the ID substituting the Default ID for the empty string if needed.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGblAnaLocID(ICyInstQuery_v1 query)
        {
            string id = CyGlobalAnalogLocInfo.GetGblAnaLocIDValue(query);

            //If the guid is not set, use the default.
            if (string.IsNullOrEmpty(id) && query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                id = query.DesignQuery.GblAnaLocDefaultID;
            }

            return id;
        }

        /// <summary>
        /// Gets the human readable name of the location.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGblAnaLocName(ICyInstQuery_v1 query)
        {
            if (query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                string id = GetGblAnaLocID(query);
                string name = query.DesignQuery.GetGblAnaLocName(id);
                if (string.IsNullOrEmpty(name) == false)
                {
                    return name;
                }
            }
            return CyGlobalAnalogLocInfo.GetGblAnaLocNameValue(query);
        }

        /// <summary>
        /// Gets the abbreviated human readable name of the location.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string GetGblAnaLocAbbrName(ICyInstQuery_v1 query)
        {
            if (query.DesignQuery != null && query.DesignQuery.DesignInfoAvailable)
            {
                string id = GetGblAnaLocID(query);
                string name = query.DesignQuery.GetGblAnaLocAbbr(id);
                if (string.IsNullOrEmpty(name) == false)
                {
                    return name;
                }
            }
            return CyGlobalAnalogLocInfo.GetGblAnaLocNameValue(query);
        }

        //-----------------------------

    }

    public enum CyGlobalAnalogLocType
    {
        Constraint, 
        Reserve
    }
}
//[]//
