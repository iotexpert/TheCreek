/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Xml;

using ActiproSoftware.SyntaxEditor;

using CyDesigner.Common.Base;
using CyDesigner.Common.Base.Controls;
using CyDesigner.Common.Base.Dialogs;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;
using CyDesigner.PSoC.PSoC3.Fitter.DesignWideResources;

namespace Cypress.Components.System.Ports.CommonPortCode
{
    //------------------------------------

    internal class CyPortPinEditingControl : CyDataGridView, ICyParamEditingControl
    {
        //-----------------------------

        #region Member Variables

        ICyInstEdit_v1 m_edit;
        List<ICyPortsPinColumnInfo> m_columns;

        const string m_pinNumColName = "PinNum";
        bool m_updatingInternally = false;

        #endregion

        //-----------------------------

        #region Constructors

        public CyPortPinEditingControl(ICyInstEdit_v1 edit)
            : base()
        {
            this.AllowUserToAddRows = false;
            this.RowHeadersVisible = false;

            m_edit = edit;
        }

        #endregion

        //-----------------------------

        #region Functionality

        public void SetupColumns(IEnumerable<ICyPortsPinColumnInfo> columns, uint portWidth)
        {
            this.Rows.Clear();
            this.Columns.Clear();
            m_columns = new List<ICyPortsPinColumnInfo>(columns);

            DataGridViewTextBoxColumn pinNumCol = new DataGridViewTextBoxColumn();
            pinNumCol.HeaderText = "Pin Number";
            pinNumCol.ReadOnly = true;
            pinNumCol.Name = m_pinNumColName;
            pinNumCol.CellTemplate.Style.BackColor = Color.LightGray;
            pinNumCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            pinNumCol.SortMode = DataGridViewColumnSortMode.Automatic;
            this.Columns.Add(pinNumCol);

            foreach (ICyPortsPinColumnInfo colInfo in m_columns)
            {
                colInfo.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.Columns.Add(colInfo.Column);
            }

            DataGridViewColumn lastCol =
                    this.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            UpdatePins(portWidth);
        }

        protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
        {
            if (e.StateChanged == DataGridViewElementStates.Visible)
            {
                foreach (DataGridViewColumn col in Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                DataGridViewColumn lastCol =
                    this.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            base.OnColumnStateChanged(e);
        }

        public void UpdatePins(uint portWidth)
        {
            m_updatingInternally = true;

            //need to iterate over all rows to update visibility
            uint maxNumRows = Math.Max(portWidth, (uint)this.RowCount);
            for (int pinNum = 0; pinNum < maxNumRows; pinNum++)
            {
                DataGridViewRow row = GetAssocaitedRow(pinNum);
                int rowIndex = (row == null) ? this.Rows.Add() : row.Index;

                this[m_pinNumColName, rowIndex].Value = pinNum;
                foreach (ICyPortsPinColumnInfo colInfo in m_columns)
                {
                    Debug.Assert(colInfo.Column.Name == colInfo.ColumnName,
                        "These should be the same. ColumnName was only added to the interface to make " +
                        "it more explicit that the name of the column must be set.");

                    bool containsInfo;
                    if (colInfo.ContainsInfoFor(pinNum, out containsInfo).IsOk && containsInfo == false)
                    {
                        //if new row initialize if the value isn't empty
                        if (!string.IsNullOrEmpty(colInfo.DefaultValue))
                        {
                            colInfo.ValueChanged(colInfo.DefaultValue, pinNum);
                        }
                    }

                    this[colInfo.ColumnName, rowIndex].Value = colInfo.GetValueToDisplay(pinNum);
                    this[colInfo.ColumnName, rowIndex].ErrorText = colInfo.GetErrorMsg(pinNum);

                    bool isReadOnly = colInfo.GetShouldBeReadonly(pinNum);
                    this[colInfo.ColumnName, rowIndex].ReadOnly = isReadOnly;

                    this[colInfo.ColumnName, rowIndex].Style.BackColor =
                        (isReadOnly) ? Color.LightGray : this.DefaultCellStyle.BackColor;
                }
                this.Rows[rowIndex].Visible = (pinNum < portWidth);
            }

            m_updatingInternally = false;
        }

        DataGridViewRow GetAssocaitedRow(int pinNum)
        {
            int rowsPinNum;
            foreach (DataGridViewRow row in this.Rows)
            {
                rowsPinNum = GetPinNum(row.Index);
                if (rowsPinNum == pinNum)
                {
                    return row;
                }
            }

            return null;
        }

        int GetPinNum(int rowIndex)
        {
            return (int)this[m_pinNumColName, rowIndex].Value;
        }

        protected override void OnCellValueChangedByUser(DataGridViewCellEventArgs e)
        {
            if (!m_updatingInternally)
            {
                string colName = this.Columns[e.ColumnIndex].Name;
                bool handled = false;
                foreach (ICyPortsPinColumnInfo colInfo in m_columns)
                {
                    if (colName == colInfo.ColumnName)
                    {
                        int pinNum = GetPinNum(e.RowIndex);
                        colInfo.ValueChanged(this[e.ColumnIndex, e.RowIndex].Value, pinNum);

                        //update all errors for the column
                        foreach (DataGridViewRow row in this.Rows)
                        {
                            int tempPinNum = GetPinNum(row.Index);
                            this[e.ColumnIndex, tempPinNum].ErrorText = colInfo.GetErrorMsg(tempPinNum);
                        }
                        handled = true;
                        break;
                    }
                }
                Debug.Assert(handled, "unhandled editable column");
            }

            base.OnCellValueChangedByUser(e);
        }

        #endregion

        //-----------------------------

        #region ICyParamEditingControl Members

        public Control DisplayControl
        {
            get { return this; }
        }

        public IEnumerable<CyCustErr> GetErrors()
        {
            List<CyCustErr> errs = new List<CyCustErr>();
            string errMsg;
            int pinNum;

            foreach (DataGridViewRow row in this.Rows)
            {
                pinNum = GetPinNum(row.Index);
                foreach (ICyPortsPinColumnInfo colInfo in m_columns)
                {
                    errMsg = colInfo.GetErrorMsg(pinNum);
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        errs.Add(new CyCustErr(errMsg));
                    }
                }
            }

            return errs;
        }

        #endregion

        //-----------------------------
    }

    //------------------------------------

    #region Port Pin Info Columns

    internal interface ICyPortsPinColumnInfo
    {
        /// <summary>
        /// Gets the column to add to the control. Column.Name MUST be set.
        /// </summary>
        DataGridViewColumn Column { get; }

        /// <summary>
        /// Gets the name of the column. MUST == Column.Name.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets that value that should be displayed in the column for the specified pin.
        /// </summary>
        /// <param name="pinNum"></param>
        /// <returns></returns>
        object GetValueToDisplay(int pinNum);

        /// <summary>
        /// Gets whether or not the specified pin should be readonly for this column.
        /// </summary>
        /// <param name="pinNum"></param>
        /// <returns></returns>
        bool GetShouldBeReadonly(int pinNum);

        /// <summary>
        /// Called when the value changes in the table. Should update the value stored in the param.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="pinNum"></param>
        void ValueChanged(object newValue, int pinNum);

        /// <summary>
        /// Gets any error messages assocaited with the pin for this column. If no errors exist null should be returned.
        /// </summary>
        /// <param name="pinNum"></param>
        /// <returns></returns>
        string GetErrorMsg(int pinNum);

        /// <summary>
        /// Gets whether or not values should be wrapped as `=value` when set.
        /// </summary>
        bool AddExpressionWrapper { get; }

        /// <summary>
        /// Gets the value to use as the default value for newly added pins.
        /// </summary>
        string DefaultValue { get; }

        /// <summary>
        /// Used when the width is changed to make sure that the default values have been set appropriately.
        /// </summary>
        /// <param name="pinNum"></param>
        /// <param name="containsInfo"></param>
        /// <returns></returns>
        CyCustErr ContainsInfoFor(int pinNum, out bool containsInfo);
    }

    internal abstract class CyPinColumnInfo : ICyPortsPinColumnInfo
    {
        //-----------------------------

        #region Member Variables

        readonly ICyInstEdit_v1 m_edit;
        readonly string m_paramName;
        readonly int m_version;
        string m_defaultValue;

        #endregion

        //-----------------------------

        #region Constructors

        public CyPinColumnInfo(ICyInstEdit_v1 edit, string paramName, int vesion, string defaultValue)
        {
            m_edit = edit;
            m_paramName = paramName;
            m_version = vesion;
            m_defaultValue = defaultValue;

            if (edit == null) throw new ArgumentNullException("edit");
        }

        #endregion

        //-----------------------------

        #region ICyPortsPinColumnInfo Members

        public CyCustErr ContainsInfoFor(int pinNum, out bool containsInfo)
        {
            CyCompDevParam param = m_edit.GetCommittedParam(m_paramName);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Expr, m_paramName, m_version, AddExpressionWrapper, out info);
            containsInfo = info.ContainsPinInfo(pinNum);
            return err;
        }

        public string DefaultValue
        {
            get { return m_defaultValue; }
        }

        public abstract DataGridViewColumn Column { get; }

        public abstract string ColumnName { get; }

        public virtual object GetValueToDisplay(int pinNum)
        {
            CyCompDevParam param = m_edit.GetCommittedParam(m_paramName);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Expr, m_paramName, m_version, AddExpressionWrapper, out info);
            string val = info.GetPinInfo(pinNum);
            return val;
        }

        public virtual bool GetShouldBeReadonly(int pinNum)
        {
            return false;
        }

        public void ValueChanged(object newValue, int pinNum)
        {
            CyCompDevParam param = m_edit.GetCommittedParam(m_paramName);
            string newString = (newValue != null) ? newValue.ToString() : string.Empty;

            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Expr, m_paramName, m_version, AddExpressionWrapper, out info);
            info.SetPinInfo(pinNum, newString);

            m_edit.SetParamExpr(m_paramName, info.Text);
            m_edit.CommitParamExprs();
        }

        public string GetErrorMsg(int pinNum)
        {
            CyCompDevParam param = m_edit.GetCommittedParam(m_paramName);

            if (param.ErrorCount > 0)
            {
                string msg = string.Empty;
                foreach (string errMsg in param.Errors)
                {
                    if (errMsg.StartsWith("pin" + pinNum.ToString()))
                    {
                        msg += errMsg + Environment.NewLine;
                    }
                }
                msg = msg.Trim();
                return msg;
            }
            return null;
        }

        public virtual bool AddExpressionWrapper
        {
            get { return true; }
        }

        #endregion

        //-----------------------------

        #region Data For Inheriting Classes

        protected ICyInstEdit_v1 EditObj
        {
            get { return m_edit; }
        }

        protected string ParamName
        {
            get { return m_paramName; }
        }

        protected int Version
        {
            get { return m_version; }
        }

        #endregion
    }

    internal class CyPinAliasColumnInfo : CyPinColumnInfo
    {
        readonly CyDGVEnhancedTextBoxColumn m_column;

        public CyPinAliasColumnInfo(ICyInstEdit_v1 edit)
            : base(edit, CyPortParamInfo.ParamNamePinAliases, 1, string.Empty)
        {
            m_column = new CyDGVEnhancedTextBoxColumn();
            m_column.HeaderText = "Alias";
            m_column.Name = "PinAliasColumn";
            m_column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        public override DataGridViewColumn Column
        {
            get { return m_column; }
        }

        public override string ColumnName
        {
            get { return m_column.Name; }
        }

        public override bool AddExpressionWrapper
        {
            get { return false; }
        }
    }

    internal class CyPinModeColumnInfo : CyPinColumnInfo
    {
        readonly CyDGVEnhancedTextBoxColumn m_column;

        public CyPinModeColumnInfo(ICyInstEdit_v1 edit)
            : this(edit, null) { }

        public CyPinModeColumnInfo(ICyInstEdit_v1 edit,
            EventHandler<CyMemberListEventArgs> memberListDisplayingHandler)
            : base(edit, CyPortParamInfoWithPinModeParams.ParamNamePinModes, 1,
            CyPortParamInfoWithPinModeParams.DefaultPinMode)
        {
            m_column = new CyDGVEnhancedTextBoxColumn();
            m_column.HeaderText = "Pin Mode";
            m_column.ReadOnly = false;
            m_column.Name = "PinModeColumn";
            m_column.SortMode = DataGridViewColumnSortMode.Automatic;

            if (memberListDisplayingHandler == null)
            {
                m_column.MemberListDisplaying +=
                    new EventHandler<CyMemberListEventArgs>(pinModeCol_MemberListDisplaying);
            }
            else
            {
                m_column.MemberListDisplaying += memberListDisplayingHandler;
            }
        }

        void pinModeCol_MemberListDisplaying(object sender, CyMemberListEventArgs e)
        {
            List<IntelliPromptMemberListItem> items = new List<IntelliPromptMemberListItem>();

            foreach (string enumName in EditObj.GetPossibleEnumValuesFromType(
                CyPortParamInfoWithPinModeParams.EnumTypeNamePinMode))
            {
                items.Add(new IntelliPromptMemberListItem(enumName,
                    (int)ActiproSoftware.Products.SyntaxEditor.IconResource.PublicEnumeration));
            }

            e.MemberListData = items.ToArray();
            e.Handled = true;
        }

        public override DataGridViewColumn Column
        {
            get { return m_column; }
        }

        public override string ColumnName
        {
            get { return m_column.Name; }
        }
    }

    #endregion

    //------------------------------------

    #region Param Info

    internal abstract class CyPortParamInfo
    {
        //------------------------------------

        #region Built-in Params

        const string ParamNameCyRemove = "CY_REMOVE";
        const string ParamNameShortInstanceName = "CY_INSTANCE_SHORT_NAME";
        const string ParamNameSupressAPIGen = "CY_SUPPRESS_API_GEN";
        const string ParamNameComponentName = "CY_COMPONENT_NAME";

        public string GetShortInstanceName(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameShortInstanceName);
            return param.Value;
        }

        public string GetComponentName(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameComponentName);
            return param.Value;
        }

        public bool TryGetCyRemove(ICyInstQuery_v1 query, out bool remove)
        {
            return TryGetBoolParamValue(query, ParamNameCyRemove, out remove);
        }

        public bool TryGetSuppressAPIGen(ICyInstQuery_v1 query, out bool supressAPIGen)
        {
            return TryGetBoolParamValue(query, ParamNameSupressAPIGen, out supressAPIGen);
        }

        #endregion

        //------------------------------------

        #region PinMode Params

        //Verilog values
        protected const string VerilogPinMode_CMOSOut = "110";
        protected const string VerilogPinMode_DigitalHiZ = "001";
        protected const string VerilogPinMode_AnalogHiZ = "000";
        protected const string VerilogPinMode_OpenDrainHi = "101";
        protected const string VerilogPinMode_OpenDrainLo = "100";
        protected const string VerilogPinMode_ResPullDown = "011";
        protected const string VerilogPinMode_ResPullUp = "010";
        protected const string VerilogPinMode_ResPullUpDown = "111";

        public string GetVerilogPinModeBitArray(ICyInstQuery_v1 query, int width)
        {
            string bits = string.Empty;
            for (int i = 0; i < width; i++)
            {
                bits += VerilogPinMode(query, i);
            }
            bits = bits.Trim();
            Debug.Assert(!string.IsNullOrEmpty(bits));
            string intr_mode = string.Format("{0}'b{1}", bits.Length, bits);
            return intr_mode;
        }

        protected abstract string VerilogPinMode(ICyInstQuery_v1 query, int pinNum);

        #endregion

        //------------------------------------

        #region Width Param

        public const string ParamNameWidth = "Width";

        public byte GetWidth(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameWidth);
            return GetWidth(param);
        }

        public byte GetWidth(ICyInstValidate_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNameWidth);
            return GetWidth(param);
        }

        byte GetWidth(CyCompDevParam widthParam)
        {
            if (widthParam.ErrorCount == 0)
            {
                byte width;
                CyCustErr err = widthParam.TryGetValueAs<byte>(out width);
                Debug.Assert(err.IsOk, err.Message);
                if (err.IsNotOK)
                {
                    throw new InvalidCastException(err.Message);
                }

                return width;
            }
            return 0;
        }

        #endregion

        //------------------------------------

        #region Power On Reset Param

        //PortPORState enum values
        const string EnumValuePortPORState_InDisabledOutHiZ = "0";
        const string EnumValuePortPORState_InEnabledOutHiZ = "1";
        const string EnumValuePortPORState_InEnabledOut1 = "2";
        const string EnumValuePortPORState_InEnabledOut0 = "3";

        ////PortPORState enum names
        //const string EnumNamePortPinMode_InDisabledOutHiZ = "PortPORState_InDisabledOutHiZ";
        //const string EnumNamePortPinMode_InEnabledOutHiZ = "PortPORState_InEnabledOutHiZ";
        //const string EnumNamePortPinMode_InEnabledOut1 = "PortPORStatee_InEnabledOut1";
        //const string EnumNamePortPinMode_InEnabledOut0 = "PortPORState_InEnabledOut0";

        //PortPORState param
        const string ParamNamePowerOnResetState = "PowerOnResetState";

        public enum CyPORState
        {
            InDisabledOutHiZ = 0,
            InEnabledOutHiZ = 1,
            InEnabledOut1 = 2,
            InEnabledOut0 = 3,

            Unknown = 4,
        }

        public CyPORState GetPORState(ICyInstQuery_v1 query)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePowerOnResetState);
            CyPORState result = GetPORStateFromString(param.Value);
            return result;
        }

        CyPORState GetPORStateFromString(string state)
        {
            if (state == EnumValuePortPORState_InDisabledOutHiZ)
            {
                return CyPORState.InDisabledOutHiZ;
            }
            if (state == EnumValuePortPORState_InEnabledOutHiZ)
            {
                return CyPORState.InEnabledOutHiZ;
            }
            if (state == EnumValuePortPORState_InEnabledOut1)
            {
                return CyPORState.InEnabledOut1;
            }
            if (state == EnumValuePortPORState_InEnabledOut0)
            {
                return CyPORState.InEnabledOut0;
            }
            Debug.Fail("unhandled state");
            return CyPORState.Unknown;
        }

        public virtual string GetVerilogPORState(ICyInstQuery_v1 query)
        {
            CyPORState state = GetPORState(query);
            return GetVerilogPORState(state);
        }

        protected string GetVerilogPORState(CyPORState state)
        {
            switch (state)
            {
                case CyPORState.InDisabledOutHiZ:
                    return "0";

                case CyPORState.InEnabledOutHiZ:
                    return "1";

                case CyPORState.InEnabledOut1:
                    return "2";

                case CyPORState.InEnabledOut0:
                    return "3";

                case CyPORState.Unknown:
                default:
                    Debug.Fail("unhandled mode");
                    return string.Empty;
            }
        }

        #endregion

        //------------------------------------

        #region Pin Alias Params

        public const string ParamNamePinAliases = "PinAliases";
        public const int Version_PinAliases = 1;

        public string GetPinAliasValue(ICyInstQuery_v1 instVal, int pinNum)
        {
            string paramName = ParamNamePinAliases;
            CyCompDevParam param = instVal.GetCommittedParam(paramName);
            string xmlText = param.Value;

            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(xmlText, ParamNamePinAliases, Version_PinAliases, false, out info);
            return info.GetPinInfo(pinNum);
        }

        public string GetPinAliasValue(ICyInstValidate_v1 instVal, int pinNum)
        {
            string paramName = ParamNamePinAliases;
            CyCompDevParam param = instVal.GetCommittedParam(paramName);
            string xmlText = param.Value;

            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(xmlText, ParamNamePinAliases, Version_PinAliases, false, out info);
            return info.GetPinInfo(pinNum);
        }

        internal static void ValidatePinAliases(CyPortParamInfo info, ICyInstValidate_v1 instVal)
        {
            //validate pin aliases
            uint width = info.GetWidth(instVal);

            List<string> aliases = new List<string>();
            for (int i = 0; i < width; i++)
            {
                string alias = info.GetPinAliasValue(instVal, i);
                if (!string.IsNullOrEmpty(alias))
                {
                    string errMsg;
                    if (!CyBasic.IsValidCCppIdentifierName(alias, out errMsg))
                    {
                        //not a valid alias
                        instVal.AddError(ParamNamePinAliases, new CyCustErr(string.Format(
                            "pin{0}: Invalid alias '{1}'. Pin aliases must be valid C identifier names. {2}",
                            i, alias, errMsg)));
                    }
                    else if (aliases.Contains(alias))
                    {
                        //not a valid alias (duplicate)
                        instVal.AddError(ParamNamePinAliases, new CyCustErr(string.Format(
                            "pin{0}: Invalid alias '{1}'. This alias has already been defined for another pin.",
                            i, alias)));
                    }
                    else
                    {
                        aliases.Add(alias);
                    }
                }
            }
        }

        public string GetPinAliasVerilog(ICyInstQuery_v1 instVal, int width)
        {
            string[] pin_aliases = new string[width];
            for (int i = 0; i < width; i++)
            {
                string alias = GetPinAliasValue(instVal, i);
                pin_aliases[i] = alias != null ? alias : string.Empty;
            }

            return GetAliasVerilog(pin_aliases);
        }

        public static string GetAliasVerilog(string[] pin_aliases)
        {
            string aliases = string.Join(",", pin_aliases);
            string verilogString = CyBasic.MakeWarpSafeVerilogString(aliases);
            return verilogString;
        }

        #endregion

        //------------------------------------

        protected bool TryGetBoolParamValue(ICyInstQuery_v1 query, string paramName, out bool boolValue)
        {
            CyCompDevParam param = query.GetCommittedParam(paramName);
            CyCustErr err = param.TryGetValueAs<bool>(out boolValue);
            return err.IsOk;
        }

        //------------------------------------

        #region Access Mode

        protected const string VerilogAccessMode_SW_ONLY = "SW_ONLY";
        protected const string VerilogAccessMode_HW_ONLY = "HW_ONLY";
        protected const string VerilogAccessMode_HW_SW = "HW_SW";
        protected const string VerilogAccessMode_LCD = "LCD";
        protected const string VerilogAccessMode_EMIF = "EMIF";

        public abstract string GetVerilogAccessMode(ICyInstQuery_v1 query);

        #endregion

        //------------------------------------

        #region Layout Mode

        public const string ParamNameContiguous = "Contiguous";

        protected const string LayoutMode_CONTIGUOUS = "CONTIGUOUS";
        protected const string LayoutMode_NONCONTIGUOUS = "NONCONTIGUOUS";

        public bool TryGetContiguous(ICyInstQuery_v1 query, out bool contiguous)
        {
            return TryGetBoolParamValue(query, ParamNameContiguous, out contiguous);
        }

        public string GetVerilogLayoutMode(ICyInstQuery_v1 query)
        {
            bool contiguous;
            bool contiguousValid = TryGetContiguous(query, out contiguous);
            Debug.Assert(contiguousValid, "Verilog shouldn't be generated if the params are not valid");

            if (contiguous)
            {
                return LayoutMode_CONTIGUOUS;
            }
            else
            {
                return LayoutMode_NONCONTIGUOUS;
            }
        }

        #endregion

        //------------------------------------

        #region Direction

        public const string EnumNamePortDirection_BIDIRECTIONAL = "PortDirection_Bidirectional";

        //Verilog values
        protected const string VerilogDirection_INPUT = "INPUT";
        protected const string VerilogDirection_OUTPUT = "OUTPUT";
        protected const string VerilogDirection_INOUT = "INOUT";
        protected const string VerilogDirection_ANALOG = "ANALOG";
        protected const string VerilogDirection_BIDIRECTIONAL = "BIDIRECTIONAL";

        public abstract string GetVerilogDirection(ICyInstQuery_v1 query);

        #endregion

        //------------------------------------

        #region Interrupt Mode

        //PortInterruptMode enum values
        protected const string EnumValuePortInterruptMode_None = "1";
        protected const string EnumValuePortInterruptMode_RisingEdge = "2";
        protected const string EnumValuePortInterruptMode_FallingEdge = "3";
        protected const string EnumValuePortInterruptMode_OnChange = "4";

        ////PortInterruptMode enum names
        //protected const string EnumNamePortInterruptMode_None = "PortInterruptMode_None";
        //protected const string EnumNamePortInterruptMode_RisingEdge = "PortInterruptMode_RisingEdge";
        //protected const string EnumNamePortInterruptMode_FallingEdge = "PortInterruptMode_FallingEdge";
        //protected const string EnumNamePortInterruptMode_OnChange = "PortInterruptMode_OnChange";

        //Verilog values
        protected const string VerilogInterruptMode_None = "00";
        protected const string VerilogInterruptMode_RisingEdge = "01";
        protected const string VerilogInterruptMode_FallingEdge = "10";
        protected const string VerilogInterruptMode_OnChange = "11";

        protected abstract string VerilogInterruptMode(ICyInstQuery_v1 query, int pinNum);

        public string GetVerilogInterruptModeBitArray(ICyInstQuery_v1 query, int width)
        {
            string bits = string.Empty;
            for (int i = 0; i < width; i++)
            {
                bits += VerilogInterruptMode(query, i);
            }
            bits = bits.Trim();
            string intr_mode = string.Format("{0}'b{1}", bits.Length, bits);
            return intr_mode;
        }

        #endregion

        //------------------------------------
    }

    internal abstract class CyPortParamInfoWithPinModeParams : CyPortParamInfo
    {
        #region PinMode Params

        //PortPinMode enum values
        internal const string EnumValuePortPinMode_CMOSOut = "1";
        const string EnumValuePortPinMode_HiZ = "2";
        const string EnumValuePortPinMode_ResPullUp = "3";
        const string EnumValuePortPinMode_ResPullDown = "4";
        const string EnumValuePortPinMode_ResPullUpDown = "5";
        const string EnumValuePortPinMode_OpenDrainLo = "6";
        const string EnumValuePortPinMode_OpenDrainHi = "7";

        //PortPinMode enum names

        /// <summary>
        /// DO NOT USE FOR COMPARING WITH EVALUATED VALUES. Used for intelliprompt filtering only.
        /// </summary>
        internal const string EnumNamePortPinMode_CMOSOut = "CMOS_Out";
        //internal const string EnumNamePortPinMode_Hi_Z = "Hi_Z";
        //internal const string EnumNamePortPinMode_ResPull_Up = "ResPull_Up";
        //internal const string EnumNamePortPinMode_ResPull_Down = "ResPull_Down";
        //internal const string EnumNamePortPinMode_ResPull_UpDown = "ResPull_UpDown";
        //internal const string EnumNamePortPinMode_OpenDrain_Lo = "OpenDrain_Lo";
        //internal const string EnumNamePortPinMode_OpenDrain_Hi = "OpenDrain_Hi";

        //PinMode param names
        public const string ParamNamePinModes = "PinModes";
        public const string ParamNamePinModeValidator = "PinModeValidator";
        public const string EnumTypeNamePinMode = "PortPinMode";

        public const string DefaultPinMode = "Hi_Z";

        public string GetPinModeValue(ICyInstQuery_v1 query, int pinNum)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinModes);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinModes, 1, true, out info);
            return info.GetPinInfo(pinNum);
        }

        public CyCustErr GetPinModeValue(ICyInstValidate_v1 query, int pinNum, out string value)
        {
            CyCompDevParam param = query.GetCommittedParam(ParamNamePinModes);
            CyPinInfo info;
            CyCustErr err = CyPinInfo.LoadInfo(param.Value, ParamNamePinModes, 1, true, out info);
            value = info.GetPinInfo(pinNum);
            return err;
        }

        protected override string VerilogPinMode(ICyInstQuery_v1 query, int pinNum)
        {
            string paramValue = GetPinModeValue(query, pinNum);

            if (paramValue == EnumValuePortPinMode_CMOSOut) return VerilogPinMode_CMOSOut;
            if (paramValue == EnumValuePortPinMode_HiZ) return HiZVerilogValue;
            if (paramValue == EnumValuePortPinMode_OpenDrainHi) return VerilogPinMode_OpenDrainHi;
            if (paramValue == EnumValuePortPinMode_OpenDrainLo) return VerilogPinMode_OpenDrainLo;
            if (paramValue == EnumValuePortPinMode_ResPullDown) return VerilogPinMode_ResPullDown;
            if (paramValue == EnumValuePortPinMode_ResPullUp) return VerilogPinMode_ResPullUp;
            if (paramValue == EnumValuePortPinMode_ResPullUpDown) return VerilogPinMode_ResPullUpDown;

            Debug.Fail(string.Format("unhandled pin mode: '{0}'", paramValue));
            return string.Empty;

        }

        protected abstract string HiZVerilogValue { get; }

        #endregion

        internal static void ValidatePinModes(CyPortParamInfoWithPinModeParams info, ICyInstValidate_v1 instVal)
        {
            //validate pin modes
            uint width = info.GetWidth(instVal);

            for (int i = 0; i < width; i++)
            {
                string mode;
                CyCustErr err = info.GetPinModeValue(instVal, i, out mode);
                int modeValue;

                if (err.IsNotOk)
                {
                    instVal.AddError(CyPortParamInfoWithPinModeParams.ParamNamePinModes,
                            new CyCustErr(string.Format("pin{0}: {1}", i, err.Message)));
                }
                else if (int.TryParse(mode, out modeValue))
                {
                    if (!(instVal.EnumValueWithinRange(
                        CyPortParamInfoWithPinModeParams.EnumTypeNamePinMode, modeValue)))
                    {
                        instVal.AddError(CyPortParamInfoWithPinModeParams.ParamNamePinModes,
                            new CyCustErr(string.Format("pin{0}: Value out of range for enumeration type '{1}'", i,
                                CyPortParamInfoWithPinModeParams.EnumTypeNamePinMode)));
                    }
                }
                else
                {
                    //the expression must have been replaced with an error msg
                    instVal.AddError(CyPortParamInfoWithPinModeParams.ParamNamePinModes,
                            new CyCustErr(string.Format("pin{0}: {1}", i, mode)));
                }
            }
        }
    }

    #endregion

    //------------------------------------
}

