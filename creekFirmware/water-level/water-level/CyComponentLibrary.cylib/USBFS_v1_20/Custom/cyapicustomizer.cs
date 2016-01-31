/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;


namespace USBFS_v1_20
{
    public partial class CyCustomizer : ICyAPICustomize_v1
    {
        private string CFile_Name = "USBFS.c";
        private string HFile_Name = "USBFS.h";
        private string instanceNameParam = "INSTANCE_NAME";


        private string ParamOut;

        private CyAPICustomizer CFile;
        private CyAPICustomizer HFile;

        #region ICyAPICustomize_v1 Members

        public IEnumerable<CyAPICustomizer> CustomizeAPIs(ICyInstQuery_v1 query, ICyTerminalQuery_v1 termQuery,
                                                          IEnumerable<CyAPICustomizer> apis)
        {
            //Debugger.Break();
            List<CyAPICustomizer> customizers = new List<CyAPICustomizer>(apis);
            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            string instanceName = "";

            // Fill the dictionary
            foreach (CyAPICustomizer api in customizers)
            {
                // Get dict from main file. 
                if (api.OriginalName.EndsWith(CFile_Name))
                {
                    CFile = api;
                    paramDict = api.MacroDictionary;
                    paramDict.TryGetValue(instanceNameParam, out instanceName);
                }
                else if (api.OriginalName.EndsWith(HFile_Name))
                {
                    HFile = api;
                }
            }

            //Extract parameters
            Parameters = new CyUSBFSParameters();
            paramDict.TryGetValue("DeviceDescriptors", out ParamOut);
            Parameters._SerializedDeviceDesc = ParamOut;
            paramDict.TryGetValue("StringDescriptors", out ParamOut);
            Parameters._SerializedStringDesc = ParamOut;
            paramDict.TryGetValue("HIDReportDescriptors", out ParamOut);
            Parameters._SerializedHIDReportDesc = ParamOut;
            Parameters.DeserializeDescriptors();

            //Update Parameters to include last changes 
            Parameters.RecalcDescriptors(Parameters.HIDReportTree.Nodes[0]);
            Parameters.RecalcDescriptors(Parameters.DeviceTree.Nodes[0]);

            //Prepare HIDReportTree for parsing (remove empty items)
            for (int i = 0; i < Parameters.HIDReportTree.Nodes[0].Nodes.Count; i++)
                for (int j = 0; j < Parameters.HIDReportTree.Nodes[0].Nodes[i].Nodes.Count; j++)
                {
                    if (((HIDReportItemDescriptor) Parameters.HIDReportTree.Nodes[0].Nodes[i].Nodes[j].Value).Item.Name 
                        == "")
                    {
                        Parameters.HIDReportTree.Nodes[0].Nodes[i].Nodes.RemoveAt(j--);
                    }
                }

            // Generate the descriptor, storage and other data structures
            USBCodeGenerator g = new USBCodeGenerator(instanceName, Parameters);

            foreach (KeyValuePair<string, string> p in g.CodeParameterDictionary)
            {
                paramDict.Add(p.Key, p.Value);
            }

            //Save result
            foreach (CyAPICustomizer api in customizers)
            {
                api.MacroDictionary = paramDict;
            }
            return customizers;
        }

        #endregion
    }

    // Start refactoring into a class that returns a set of dictionaries similar to the
    // CyAPICustomizer
    internal class USBCodeGenerator : UsbCodePrimitives
    {
        private const string APIGEN_DEFINES = "APIGEN_DEFINES";

        private const string APIGEN_DEVICE_DESCRIPTORS = "APIGEN_DEVICE_DESCRIPTORS";
        private const string APIGEN_DEVICE_TABLES = "APIGEN_DEVICE_TABLES";
        private const string DEF_NUM_DEVICES = "NUM_DEVICES";

        private const string APIGEN_STRING_DESCRIPTORS = "APIGEN_STRING_DESCRIPTORS";
        private const string APIGEN_EE_DESCRIPTOR = "APIGEN_EE_DESCRIPTOR";
        private const string APIGEN_SN_DESCRIPTOR = "APIGEN_SN_DESCRIPTOR";

        private const string APIGEN_DEFINE_ENABLE_STRINGS = "APIGEN_ENABLE_STRINGS";
        private const string DEF_DESCRIPTOR_STRING = "ENABLE_DESCRIPTOR_STRINGS";
        private const string APIGEN_DEFINE_EE_STRING = "APIGEN_ENABLE_EE_STRING";
        private const string APIGEN_DEFINE_SN_STRING = "APIGEN_ENABLE_SN_STRING";

        private const string DEF_ENABLE_STRINGS = "ENABLE_STRINGS";
        private const string DEF_MSOS_STRING = "ENABLE_MSOS_STRING";
        private const string DEF_SN_STRING = "ENABLE_SN_STRING";
        private const string DEF_FWSN_STRING = "ENABLE_FWSN_STRING";
        private const string DEF_IDSN_STRING = "ENABLE_IDSN_STRING";

        private const string APIGEN_HIDREPORT_DESCRIPTORS = "APIGEN_HIDREPORT_DESCRIPTORS";
        private const string APIGEN_HIDREPORT_TABLES = "APIGEN_HIDREPORT_TABLES";
        private const string DEF_ENABLE_HID_CLASS = "ENABLE_HID_CLASS";


        private const int SN_INDEX = 0;
        private const int EE_INDEX = 1;

        private string m_InstanceName;

        private CyUSBFSParameters m_Parameters;

        private Dictionary<string, string> m_CodeParameters = new Dictionary<string, string>();

        public USBCodeGenerator(string InstanceName, CyUSBFSParameters Parameters) : base(InstanceName)
        {
            m_InstanceName = InstanceName;
            m_Parameters = Parameters;
            GenerateCode();
        }

        private void GenerateCode()
        {
            GenerateDevices();
            GenerateStrings();
            GenerateHIDReports();
        }

        private void GenerateDevices()
        {
            //Generate the descriptors
            UsbDescriptorGenTree gtDescr = new UsbDescriptorGenTree();
            string deviceDescr = gtDescr.Generate(m_Parameters.DeviceTree.Nodes[0].Nodes, m_InstanceName, 
                                                  m_InstanceName);
            m_CodeParameters.Add(APIGEN_DEVICE_DESCRIPTORS, deviceDescr);

            UsbDeviceRootTableGen genTable = new UsbDeviceRootTableGen(m_InstanceName);
            string deviceTables = genTable.Generate(m_Parameters.DeviceTree.Nodes[0].Nodes, m_InstanceName,
                                                    m_InstanceName);
            m_CodeParameters.Add(APIGEN_DEVICE_TABLES, deviceTables);
            AddDefine(DEF_NUM_DEVICES, m_Parameters.DeviceTree.Nodes[0].Nodes.Count.ToString());
        }

        private void GenerateStrings()
        {
            // String Descriptors
            bool enableStrings = false;
            UsbStringDescriptorGenTree gtStrings = new UsbStringDescriptorGenTree();
            string stringDescr = gtStrings.Generate(m_Parameters.StringTree.Nodes[0].Nodes, m_InstanceName,
                                                    m_InstanceName);
            if (m_Parameters.StringTree.Nodes[0].Nodes.Count > 1)
            {
                enableStrings = true;
                AddDefine(DEF_DESCRIPTOR_STRING);
            }
            // Special String support MS OS String (EE)
            StringDescriptor s = (StringDescriptor) m_Parameters.StringTree.Nodes[1].Nodes[EE_INDEX].Value;
            if (s.bUsed)
            {
                enableStrings = true;
                AddDefine(DEF_MSOS_STRING);
            }
            // Special String support SN String
            s = (StringDescriptor) m_Parameters.StringTree.Nodes[1].Nodes[SN_INDEX].Value;
            string SN_Code;
            UsbStringSNDescriptorGen cg = new UsbStringSNDescriptorGen(m_InstanceName);
            SN_Code = cg.GenHeader(s, m_InstanceName, m_InstanceName);
            if (s.bUsed)
            {
                // "/* We gotta SN String */\n";
                enableStrings = true;
                SN_Code += cg.GenBody(s, m_InstanceName, m_InstanceName);
                SN_Code += cg.GenFooter(s, m_InstanceName, m_InstanceName);
                m_CodeParameters.Add(APIGEN_SN_DESCRIPTOR, SN_Code);

                AddDefine(DEF_SN_STRING);
                    
                if (s.snType == StringGenerationType.USER_CALL_BACK)
                {
                    AddDefine(DEF_FWSN_STRING);
                }
                else if (s.snType == StringGenerationType.SILICON_NUMBER)
                {
                    AddDefine(DEF_IDSN_STRING);
                }
                else
                {  /* not used*/
                }
            }
            m_CodeParameters.Add(APIGEN_STRING_DESCRIPTORS, stringDescr);
            if (enableStrings)
            {
                AddDefine(DEF_ENABLE_STRINGS);
            }
        }

        private void GenerateHIDReports()
        {
            UsbHIDReportDescriptorGenTree gtHIDReport = new UsbHIDReportDescriptorGenTree();
            string HIDReportDescr = gtHIDReport.Generate(m_Parameters.HIDReportTree.Nodes[0], m_InstanceName,
                                                         m_InstanceName);
            string HIDReportTables = GenReportTables(m_Parameters.DeviceTree.Nodes[0], m_InstanceName, m_InstanceName);

            if (HIDReportDescr.Length > 0)
            {
                AddDefine(DEF_ENABLE_HID_CLASS);
                m_CodeParameters.Add(APIGEN_HIDREPORT_DESCRIPTORS, HIDReportDescr);
                m_CodeParameters.Add(APIGEN_HIDREPORT_TABLES, HIDReportTables);
                for (int i = 0; i < m_Parameters.HIDReportTree.Nodes.Count; i++)
                {
                    int rptIndex = i + 1;
                    AddDefine("HID_RPT_" + rptIndex + "_SIZE_LSB",
                              String.Format("{0}", HexByteString((byte)
                                               (ReportDescriptorSize(m_Parameters.HIDReportTree.Nodes[i].Nodes[0].Nodes) 
                                                & 0xFF))));
                    AddDefine("HID_RPT_" + rptIndex + "_SIZE_MSB",
                              String.Format("{0}", HexByteString((byte)((ReportDescriptorSize(
                                                   m_Parameters.HIDReportTree.Nodes[i].Nodes[0].Nodes) >> 8) & 0xFF))));
                }
            }
        }

        private string GenReportTables(DescriptorNode node, string instName, string qualName)
        {
            string code = "";
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                string qual;
                if (node.Nodes[i].Value.bDescriptorType == USBDescriptorType.ALTERNATE)
                {
                    qual = qualName;
                }
                else
                {
                    qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                }
                code += GenReportTables(node.Nodes[i], instName, qual);
            }
            if (node.Value != null)
            {
                if (node.Value.bDescriptorType == USBDescriptorType.HID)
                {
                    HIDDescriptor h = (HIDDescriptor) node.Value;
                    code += GenReportTable(h.ReportIndex, instName, qualName);
                }
            }
            return code;
        }

        private string GenReportTable(int rptIndex, string instName, string qualName)
        {
            string code = "";
            if ((rptIndex > 0) && (rptIndex <= m_Parameters.HIDReportTree.Nodes.Count))
            {
                DescriptorNode ReportTop = m_Parameters.HIDReportTree.Nodes[rptIndex - 1];
                code += "#if !defined(USER_DEFINE_" + qualName + "_HID_RPT_STORAGE)\n";
                if (ReportCount(ReportTop, "INPUT") > 0)
                {
                    code += "/*********************************************************************\n";
                    code += " HID Input Report Storage\n";
                    code += " *********************************************************************/\n";
                    code += GenerateAllocateArray(qualName + "_IN_BUF", qualName + "_IN_BUF_SIZE") + NEWLINE;
                    AddDefineNoInstance(qualName + "_IN_BUF_SIZE", ReportSize(ReportTop).ToString());
                    code += "/*********************************************************************\n";
                    code += " HID Input Report TD Table\n";
                    code += " *********************************************************************/\n";
                    code += GenerateTDHeader(qualName + "_IN_RPT_TABLE[]");
                    code += GenerateTDEntry(qualName + "_NUM_IN_RPTS", qualName + "_IN_BUF[0]", NULL_PTR) + NEWLINE;
                    code += GenerateFooter() + ";\n";
                    AddDefineNoInstance(qualName + "_NUM_IN_RPTS", ReportCount(ReportTop, "INPUT").ToString());
                }

                if (ReportCount(ReportTop, "OUTPUT") > 0)
                {
                    code += "/*********************************************************************\n";
                    code += " HID Input Report Storage\n";
                    code += " *********************************************************************/\n";
                    code += GenerateAllocateArray(qualName + "_OUT_BUF", qualName + "_OUT_BUF_SIZE") + NEWLINE;
                    AddDefineNoInstance(qualName + "_OUT_BUF_SIZE", ReportSize(ReportTop).ToString());
                    code += "/*********************************************************************\n";
                    code += " HID Output Report TD Table\n";
                    code += " *********************************************************************/\n";
                    code += GenerateTDHeader(qualName + "_OUT_RPT_TABLE[]");
                    code += GenerateTDEntry(qualName + "_NUM_OUT_RPTS", qualName + "_OUT_BUF[0]", NULL_PTR) + NEWLINE;
                    code += GenerateFooter() + ";\n";
                    AddDefineNoInstance(qualName + "_NUM_OUT_RPTS", ReportCount(ReportTop, "OUTPUT").ToString());
                }

                if (ReportCount(ReportTop, "FEATURE") > 0)
                {
                    code += "/*********************************************************************\n";
                    code += " HID Feature Report Storage\n";
                    code += " *********************************************************************/\n";
                    code += GenerateAllocateArray(qualName + "_FEATURE_BUF", qualName + "_FEATURE_BUF_SIZE") + NEWLINE;
                    AddDefineNoInstance(qualName + "_FEATURE_BUF_SIZE", ReportSize(ReportTop).ToString());
                    code += "/*********************************************************************\n";
                    code += " HID Feature Report TD Table\n";
                    code += " *********************************************************************/\n";
                    code += GenerateTDHeader(qualName + "_FEATURE_RPT_TABLE[]");
                    code += GenerateTDEntry(qualName + "_NUM_FEATURE_RPTS", qualName + "_FEATURE_BUF[0]", NULL_PTR) +
                            NEWLINE;
                    code += GenerateFooter() + ";\n";
                    AddDefineNoInstance(qualName + "_NUM_FEATURE_RPTS", ReportCount(ReportTop, "FEATURE").ToString());
                }

                code += "/*********************************************************************\n";
                code += " HID Report Look Up Table          This table has three entries:\n";
                code += "                                         IN Report Table\n";
                code += "                                         OUT Report Table\n";
                code += "                                         Feature Report Table\n";
                code += " *********************************************************************/\n";
                code += GenerateLUTHeader(qualName + "_TABLE[]");
                AddDefineNoInstance(qualName + "_COUNT", "1"); // HID Class only has one LUT to link to the Interface

                if (ReportCount(ReportTop, "INPUT") > 0)
                {
                    code += GenerateLUTEntry(qualName + "_IN_RPT_TABLE", 1);
                }
                else
                {
                    code += GenerateNullLUTEntry();
                }
                code += COMMA_NEWLINE;
                if (ReportCount(ReportTop, "OUTPUT") > 0)
                {
                    code += GenerateLUTEntry(qualName + "_OUT_RPT_TABLE", 1);
                }
                else
                {
                    code += GenerateNullLUTEntry();
                }
                code += COMMA_NEWLINE;
                if (ReportCount(ReportTop, "FEATURE") > 0)
                {
                    code += GenerateLUTEntry(qualName + "_FEATURE_RPT_TABLE", 1);
                }
                else
                {
                    code += GenerateNullLUTEntry();
                }
                code += NEWLINE;
                code += GenerateFooter() + ";\n";
                code += "#endif /* USER_DEFINE_" + qualName + "_HID_RPT_STORAGE */\n";
            }
            return code;
        }

        private byte ReportCount(DescriptorNode node, string rptType)
        {
            byte count = 0;
            foreach (DescriptorNode n in node.Nodes)
            {
                count += ReportCount(n, rptType);
            }
            if (node.Value != null)
            {
                if (node.Value.bDescriptorType == USBDescriptorType.HID_REPORT_ITEM)
                {
                    HIDReportItemDescriptor h = (HIDReportItemDescriptor) node.Value;
                    if (h.Item.Name == rptType)
                    {
                        count += 1;
                    }
                }
            }
            // For now, only returns one or zero.  Update when we support storage for multiple reports
            if (count > 0)
                count = 1;
            return count;
        }

        private ushort ReportDescriptorSize(List<DescriptorNode> nodes)
        {
            ushort size = 0;
            HIDReportItemDescriptor h;
            for (int i = 0; i < nodes.Count; i++)
            {
                h = (HIDReportItemDescriptor) nodes[i].Value;
                size += CalcHIDItemLength(h);
            }
            return size;
        }

        private byte ReportSize(DescriptorNode n)
        {
            return 8;
        }

        private void AddDefine(string d)
        {
            string defs;

            m_CodeParameters.TryGetValue(APIGEN_DEFINES, out defs);
            m_CodeParameters.Remove(APIGEN_DEFINES);
            defs += "#define " + m_InstanceName + "_" + d + "\n";
            m_CodeParameters.Add(APIGEN_DEFINES, defs);
        }

        private void AddDefine(string d, string v)
        {
            string defs;

            m_CodeParameters.TryGetValue(APIGEN_DEFINES, out defs);
            m_CodeParameters.Remove(APIGEN_DEFINES);
            defs += "#define " + m_InstanceName + "_" + d + "   " + v + "\n";
            m_CodeParameters.Add(APIGEN_DEFINES, defs);
        }

        private void AddDefineNoInstance(string d, string v)
        {
            string defs;

            m_CodeParameters.TryGetValue(APIGEN_DEFINES, out defs);
            m_CodeParameters.Remove(APIGEN_DEFINES);
            defs += "#define " + d + "   " + v + "\n";
            m_CodeParameters.Add(APIGEN_DEFINES, defs);
        }

        public Dictionary<string, string> CodeParameterDictionary
        {
            get { return m_CodeParameters; }
        }
    }

    public class UsbCodePrimitives
    {
        private const int commentWidth = 40;
        private const string m_COMMA_NEWLINE = ",\n";
        private const string m_NEWLINE = "\n";
        private string m_Code = "CODE";
        private string m_InstanceName;

        public string GenerateAllocateArray(string n, int s)
        {
            string code = "";
            code += "uint8 " + n + "[" + s + "];\n";
            ;
            return code;
        }

        public string GenerateAllocateArray(string n, string s)
        {
            string code = "";
            code += "uint8 " + n + "[" + s + "];\n";
            ;
            return code;
        }

        public UsbCodePrimitives(string InstanceName)
        {
            m_InstanceName = InstanceName;
        }

        public byte MSB(ushort i)
        {
            return (byte) (0xff & (i >> 8));
        }

        public byte LSB(ushort i)
        {
            return (byte) (0xff & i);
        }

        public string HexByteString(byte b)
        {
            return string.Format("0x{0:X2}u", b);
        }

        public string HexWordString(ushort w)
        {
            return string.Format("0x{0:X4}u", w);
        }

        public string GenerateDescrByte(string c, byte b)
        {
            string s;
            s = string.Format("/* {0}*/ ", c.PadRight(commentWidth, ' '));
            s += HexByteString(b);
            return s;
        }

        public string GenerateDescrDWLS(string c, ushort u)
        {
            string s;
            s = string.Format("/* {0}*/ ", c.PadRight(commentWidth, ' '));
            s += HexByteString(LSB(u));
            s += ", ";
            s += HexByteString(MSB(u));
            return s;
        }

        public string GeneratePtr(string d)
        {
            string s;
            s = string.Format("    &{0}", d);
            return s;
        }

        public string GenerateUsbUnicode(string d)
        {
            string s = "";
            string comma = "";
            int lineLength = 0;
            
            if (String.IsNullOrEmpty(d)==false)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    if (lineLength > 72)
                    {
                        s += NEWLINE;
                        lineLength = 0;
                    }
                    s += comma;
                    s += string.Format("'{0}', 0", d.Substring(i, 1));
                    comma = ",";
                    lineLength += 8;
                }
            }
            return s;
        }

        public string GenerateVoidPtr(string d)
        {
            string s;
            s = string.Format("    (void *)({0})", d);
            return s;
        }

        public string GenerateReportItem(HIDReportItemDescriptor d)
        {
            string s;

            s = string.Format("/* {0}*/ ", d.Item.Name.PadRight(commentWidth, ' '));
            for (byte i = 0; i < CalcHIDItemLength(d); i++)
            {
                s += HexByteString(d.Item.Value[i]) + ", ";
            }
            s += NEWLINE;
            return s;
        }

        public ushort CalcHIDItemLength(HIDReportItemDescriptor d)
        {
            //Per HID 1.1 Secion 6.2.2.2
            ushort r = 0;
            switch (d.Item.Size)
            {
                case 0:
                    r = 1;
                    break;
                case 1:
                    r = 2;
                    break;
                case 2:
                    r = 3;
                    break;
                case 3:
                    r = 5;
                    break;
            }
            return r;
        }

        public string GenerateSymbolicByte(string c, string sym)
        {
            string s;
            s = string.Format("/* {0}*/ ", c.PadRight(commentWidth, ' '));
            s += sym;
            return s;
        }

        public string GenerateWordCount(string c, ushort u)
        {
            string s;
            s = string.Format("/* {0}*/ ", c.PadRight(commentWidth, ' '));
            s += HexByteString(LSB(u));
            s += ", ";
            s += HexByteString(MSB(u));
            return s;
        }

        public string GenerateByte(byte b)
        {
            string s = "";
            s += HexByteString(b);
            return s;
        }

        public string GenerateWord(ushort w)
        {
            string s = "";
            s += HexWordString(w);
            return s;
        }

        public string GenerateLUTHeader(string n)
        {
            string s = "";
            s += "T_" + INSTANCE_NAME + "_LUT " + CODE + " " + n + " = {\n";
            return s;
        }

        public string GenerateLUTEntry(string n, byte c)
        {
            string s = "";
            s += "    {";
            s += HexByteString(c) + ", " + GeneratePtr(n);
            s += "}";
            return s;
        }

        public string GenerateLUTEntry(string n, string c)
        {
            string s = "";
            s += "    {";
            s += c + ", " + GeneratePtr(n);
            s += "}";
            return s;
        }

        public string GenerateNullLUTEntry()
        {
            string s = "";
            s += "    {";
            s += HexByteString(0) + ",    " + NULL_PTR;
            s += "}";
            return s;
        }

        public string GenerateTDHeader(string n)
        {
            string s = "";
            s += "T_" + INSTANCE_NAME + "_TD " + CODE + " " + n + " = {\n";
            return s;
        }

        public string GenerateTDEntry(byte c, string p, string d)
        {
            string s = "";
            s += "    {";
            s += HexByteString(c) + ", ";
            s += (p == NULL_PTR ? NULL_PTR : GeneratePtr(p)) + ", ";
            s += (d == NULL_PTR ? NULL_PTR : GeneratePtr(d));
            s += "}";
            return s;
        }

        public string GenerateTDEntry(string c, string p, string d)
        {
            string s = "";
            s += "    {";
            s += c + ", ";
            s += (p == NULL_PTR ? NULL_PTR : GeneratePtr(p)) + ", ";
            s += (d == NULL_PTR ? NULL_PTR : GeneratePtr(d));
            s += "}";
            return s;
        }

        public string GenerateNullTDEntry()
        {
            string s = "";
            s += "    {";
            s += HexByteString(0) + ", " + HexByteString(0) + ", " + HexByteString(0);
            s += "}";
            return s;
        }

        public string GenerateFooter()
        {
            string s = "";
            s += "}";
            return s;
        }

        public string COMMA_NEWLINE
        {
            get { return m_COMMA_NEWLINE; }
        }

        public string NEWLINE
        {
            get { return m_NEWLINE; }
        }

        public string CODE
        {
            get { return m_InstanceName + "_" + m_Code; }
            set { m_Code = value; }
        }

        public string INSTANCE_NAME
        {
            get { return m_InstanceName; }
            set { m_InstanceName = value; }
        }

        public string NULL_PTR
        {
            get { return m_InstanceName + "_NULL"; }
        }
    }

    #region Descriptor Code Generators

    public abstract class UsbCodeGen : UsbCodePrimitives
    {
        protected string m_nl;

        public UsbCodeGen(string InstanceName) : base(InstanceName)
        {
            m_nl = "";
        }

        public virtual string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";

            UsbCodeGenMaker f = new UsbCodeGenMaker();

            UsbDescriptorGenTree cgt = new UsbDescriptorGenTree();

            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = qualName + "_" + nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                UsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].Value, INSTANCE_NAME);
                code += cg.GenHeader(nodes[i].Value, instName, qual);
                code += cg.GenBody(nodes[i].Value, instName, qual);
                code += cgt.Generate(nodes[i].Nodes, instName, qual);
                code += cg.GenFooter(nodes[i].Value, instName, qual);
            }
            return code;
        }

        public virtual string Generate(DescriptorNode node, string instName, string qualName)
        {
            throw new NotImplementedException();
        }

        public abstract string GenHeader(USBDescriptor ud, string instanceName, string qualName);
        public abstract string GenBody(USBDescriptor ud, string instanceName, string qualName);
        public abstract string GenFooter(USBDescriptor ud, string instanceName, string qualName);

        protected string GenLUTEntry(string instanceName, string tableSuffix, byte c)
        {
            string code = "";
            code += m_nl;
            m_nl = COMMA_NEWLINE;
            code += GenerateLUTEntry(instanceName + tableSuffix, c);
            return code;
        }

        protected string GenNullLUTEntry()
        {
            string code = "";
            code += m_nl;
            m_nl = COMMA_NEWLINE;
            code += GenerateNullLUTEntry();
            return code;
        }
    }

    internal class UsbDeviceDescriptorGen : UsbCodeGen
    {
        public UsbDeviceDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            DeviceDescriptor d = (DeviceDescriptor) ud;
            // Header Comment
            //       0123456789012345678901234567890123456789012345678901234567890123456789
            code += "/*********************************************************************\n";
            code += " Device Descriptors\n";
            code += " *********************************************************************/\n";
            code += "uint8 " + CODE + " " + qualName + "_DESCR[] = {" + "\n";
            code += GenerateDescrByte("Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte("DescriptorType: DEVICE", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrDWLS("bcdUSB (ver 2.0)", 0x0200) + COMMA_NEWLINE;
            code += GenerateDescrByte("bDeviceClass", d.bDeviceClass) + COMMA_NEWLINE;
            code += GenerateDescrByte("bDeviceSubClass", d.bDeviceSubClass) + COMMA_NEWLINE;
            code += GenerateDescrByte("bDeviceProtocol", d.bDeviceProtocol) + COMMA_NEWLINE;
            //            code += GenerateDescrByte("bMaxPacketSize0", d.bMaxPacketSize0) + COMMA_NEWLINE;
            code += GenerateDescrByte("bMaxPacketSize0", 8) + COMMA_NEWLINE;
            code += GenerateDescrDWLS("idVendor", d.idVendor) + COMMA_NEWLINE;
            code += GenerateDescrDWLS("idProduct", d.idProduct) + COMMA_NEWLINE;
            code += GenerateDescrDWLS("bcdDevice", d.bcdDevice) + COMMA_NEWLINE;
            code += GenerateDescrByte("iManufacturer", d.iManufacturer) + COMMA_NEWLINE;
            code += GenerateDescrByte("iProduct", d.iProduct) + COMMA_NEWLINE;
//TODO: FORCE SERIAL NUMBER STRING TO ZERO FOR NOW
//            code += GenerateDescrByte("iSerialNumber", d.iSerialNumber) + COMMA_NEWLINE;
            code += GenerateDescrByte("iSerialNumber", 0) + COMMA_NEWLINE;
            code += GenerateDescrByte("bNumConfigurations", d.bNumConfigurations) + NEWLINE;
            code += "};\n";

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbConfigDescriptorGen : UsbCodeGen
    {
        public UsbConfigDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            ConfigDescriptor d = (ConfigDescriptor) ud;
            // Header Comment
            code += "/*********************************************************************\n";
            code += " Config Descriptor  \n";
            code += " *********************************************************************/\n";
            code += "uint8 " + CODE + " " + qualName + "_DESCR[] = {" + "\n";

            code += GenerateDescrByte(" Config Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" DescriptorType: CONFIG", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrDWLS(" wTotalLength", d.wTotalLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bNumInterfaces", d.bNumInterfaces) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bConfigurationValue", (byte) (d.bConfigurationValue + 1)) + COMMA_NEWLINE;
            code += GenerateDescrByte(" iConfiguration", d.iConfiguration) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bmAttributes", d.bmAttributes) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bMaxPower", d.bMaxPower);

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\n};\n";

            return code;
        }
    }

    internal class UsbInterfaceGeneralDescriptorGen : UsbCodeGen
    {
        public UsbInterfaceGeneralDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
#if false
		            InterfaceDescriptor d = (InterfaceDescriptor)ud;
            // Header Comment
            code += ",\n";
            code += "/*********************************************************************\n";
            code += " Interface Descriptor\n";
            code += " *********************************************************************/\n";
            code += GenerateDescrByte(" Interface Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" DescriptorType: INTERFACE", (byte)d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceNumber", d.bInterfaceNumber) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bAlternateSetting", d.bAlternateSetting) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bNumEndpoints", d.bNumEndpoints) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceClass", d.bInterfaceClass) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceSubClass", d.bInterfaceSubClass) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceProtocol", d.bInterfaceProtocol) + COMMA_NEWLINE;
            code += GenerateDescrByte(" iInterface", d.iInterface);


#endif
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbInterfaceDescriptorGen : UsbCodeGen
    {
        public UsbInterfaceDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            InterfaceDescriptor d = (InterfaceDescriptor) ud;
            // Header Comment
            code += ",\n";
            code += "/*********************************************************************\n";
            code += " Interface Descriptor\n";
            code += " *********************************************************************/\n";
            code += GenerateDescrByte(" Interface Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" DescriptorType: INTERFACE", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceNumber", d.bInterfaceNumber) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bAlternateSetting", d.bAlternateSetting) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bNumEndpoints", d.bNumEndpoints) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceClass", d.bInterfaceClass) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceSubClass", d.bInterfaceSubClass) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterfaceProtocol", d.bInterfaceProtocol) + COMMA_NEWLINE;
            code += GenerateDescrByte(" iInterface", d.iInterface);

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbEndpointDescriptorGen : UsbCodeGen
    {
        public UsbEndpointDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";
            code += GenHeader(node.Value, instName, qualName);
            code += GenBody(node.Value, instName, qualName);
            code += GenFooter(node.Value, instName, qualName);
            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            EndpointDescriptor d = (EndpointDescriptor) ud;
            // Fix EndpointAddress issue
            if ((d.bEndpointAddress & 0x0F) == 0)
            {
                d.bEndpointAddress |= 1;
            }
            // Header Comment
            code += ",\n";
            code += "/*********************************************************************\n";
            code += " Endpoint Descriptor\n";
            code += " *********************************************************************/\n";
            code += GenerateDescrByte(" Endpoint Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" DescriptorType: ENDPOINT", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bEndpointAddress", d.bEndpointAddress) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bmAttributes", d.bmAttributes) + COMMA_NEWLINE;
            code += GenerateDescrDWLS(" wMaxPacketSize", d.wMaxPacketSize) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bInterval", d.bInterval);

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbHIDClassDescriptorGen : UsbCodeGen
    {
        public UsbHIDClassDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";
            code += GenHeader(node.Value, instName, qualName);
            code += GenBody(node.Value, instName, qualName);
            code += GenFooter(node.Value, instName, qualName);
            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            HIDDescriptor d = (HIDDescriptor) ud;
            // Header Comment
            code += ",\n";
            code += "/*********************************************************************\n";
            code += " HID Class Descriptor\n";
            code += " *********************************************************************/\n";
            code += GenerateDescrByte(" HID Class Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte(" DescriptorType: HID_CLASS", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrDWLS(" bcdHID", 0x0111) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bCountryCode", d.bCountryCode) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bNumDescriptors", d.bNumDescriptors) + COMMA_NEWLINE;
            code += GenerateDescrByte(" bDescriptorType", d.bDescriptorType1) + COMMA_NEWLINE;
            code +=
                GenerateSymbolicByte(" wDescriptorLength (LSB)",
                                     instanceName + "_HID_RPT_" + d.ReportIndex + "_SIZE_LSB") + COMMA_NEWLINE;
            code += GenerateSymbolicByte(" wDescriptorLength (MSB)",
                                         instanceName + "_HID_RPT_" + d.ReportIndex + "_SIZE_MSB");

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbStringDescriptorGen : UsbCodeGen
    {
        public UsbStringDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            StringDescriptor d = (StringDescriptor) ud;
            string code = "";
            // Header Comment
            code += COMMA_NEWLINE;
            code += "/*********************************************************************\n";
            code += string.Format(" String Descriptor: \"{0}\"\n", d.bString);
            code += " *********************************************************************/\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = " ";
            StringDescriptor d = (StringDescriptor) ud;
            
            if (d.bString != null)
            {
                code += GenerateDescrByte("Descriptor Length", (byte) (2 + (d.bString.Length*2))) + COMMA_NEWLINE;
                code += GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType) + COMMA_NEWLINE + " ";
                code += GenerateUsbUnicode(d.bString);
            }
            else
            {
                code += GenerateDescrByte("Descriptor Length", (byte) (2)) + COMMA_NEWLINE;
                code += GenerateDescrByte("DescriptorType: STRING",  (byte) d.bDescriptorType);
            }

            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    internal class UsbStringZeroDescriptorGen : UsbCodeGen
    {
        public UsbStringZeroDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Language ID Descriptor has to be the 0th descriptor, so it will anchor
            // all of the string descriptors
            code += "/*********************************************************************\n";
            code += " String Descriptor Table\n";
            code += " *********************************************************************/\n";
            code += "uint8 " + CODE + " " + qualName + "_STRING_DESCRIPTORS[] = {\n";
            code += "/*********************************************************************\n";
            code += " Language ID Descriptor\n";
            code += " *********************************************************************/\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            StringZeroDescriptor d = (StringZeroDescriptor) ud;
            string code = "";
            code += GenerateDescrByte("Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            code += GenerateDescrDWLS("Language Id", d.wLANGID);
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Language ID Descriptor has to be the 0th descriptor, so it will close
            // all of the string descriptors
            code += COMMA_NEWLINE;
            code += "/*********************************************************************/\n";
            code += GenerateDescrByte("Marks the end of the list.", 0) + "};\n";
            code += "/*********************************************************************/\n";
            return code;
        }
    }

    internal class UsbStringSNDescriptorGen : UsbCodeGen
    {
        public UsbStringSNDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Serial Number String Descriptor
            code += "/*********************************************************************\n";
            code += " Serial Number String Descriptor\n";
            code += " *********************************************************************/\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            StringDescriptor d = (StringDescriptor) ud;
            string code = "";
            code += "uint8 " + CODE + " " + qualName + "_SN_STRING_DESCRIPTOR[] = {\n";
            if (d.bString != null)
            {
                code += GenerateDescrByte("Descriptor Length", d.bLength) + COMMA_NEWLINE;
                code += GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType) + COMMA_NEWLINE;
                code += GenerateUsbUnicode(d.bString);
            }
            else
            {
                code += GenerateDescrByte("Descriptor Length", (byte) (2)) + COMMA_NEWLINE;
                code += GenerateDescrByte("DescriptorType: STRING",  (byte) d.bDescriptorType);
            }            
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Microsoft Operating System Descriptor
            code += NEWLINE;
            code += "};\n";
            return code;
        }
    }

    internal class UsbStringMSOSDescriptorGen : UsbCodeGen
    {
        public UsbStringMSOSDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Microsoft Operating System Descriptors
            code += "/*********************************************************************\n";
            code += " Microsoft Operating System Descriptor\n";
            code += " *********************************************************************/\n";
            code += "uint8 " + CODE + " " + qualName + "_MSOS_DESCRIPTOR[] = {\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            StringDescriptor d = (StringDescriptor) ud;
            string code = "";
            code += GenerateDescrByte("Descriptor Length", d.bLength) + COMMA_NEWLINE;
            code += GenerateDescrByte("DescriptorType: STRING", (byte) d.bDescriptorType) + COMMA_NEWLINE;
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Microsoft Operating System Descriptor
            code += NEWLINE;
            code += "};\n";
            return code;
        }
    }

    internal class UsbHIDReportDescriptorGen : UsbCodeGen
    {
        public UsbHIDReportDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Header Comment
            code += "/*********************************************************************\n";
            code += " HID Report Descriptors\n";
            code += " *********************************************************************/\n";
            code += "uint8 " + CODE + " " + qualName + "_HIDREPORT_DESCRIPTORS[] = {\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            HIDReportDescriptor d = (HIDReportDescriptor) ud;
            string code = "";
            code += "/*********************************************************************\n";
            code += " HID Report Descriptor: " + d.Name + "\n";
            code += " *********************************************************************/\n";
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            code += "/*********************************************************************/\n";
            code += GenerateDescrDWLS("End of the HID Report Descriptors", 0) + "};\n";
            code += "/*********************************************************************/\n";
            return code;
        }
    }

    internal class UsbHIDReportItemGen : UsbCodeGen
    {
        public UsbHIDReportItemGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            HIDReportItemDescriptor d = (HIDReportItemDescriptor) ud;
            return GenerateReportItem(d);
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }
    }

    internal class UsbUnknownDescriptorGen : UsbCodeGen
    {
        public UsbUnknownDescriptorGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Header Comment
            code += "/*********************************************************************\n";
            code += " Unknown Descriptor " + ud.bDescriptorType + ":::" + ud.bLength + "\n";
            code += " *********************************************************************/\n";
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";

            return code;
        }
    }

    #endregion Descriptor Code Generators

    #region Table Generators

    internal class UsbDeviceRootTableGen : UsbCodeGen
    {
        public UsbDeviceRootTableGen(string InstanceName) : base(InstanceName)
        {
            m_nl = "";
        }

        public override string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";
            // Generate the children
            for (int i = 0; i < nodes.Count; i++)
            {
                UsbDeviceTableGen cgt = new UsbDeviceTableGen(INSTANCE_NAME);
                string qual = qualName + "_" + nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                code += cgt.Generate(nodes[i], instName, qual);
            }

            code += GenHeader(nodes[0].Value, instName, instName);
            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = qualName + "_" + nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                code += this.GenLUTEntry(qual, "_TABLE", (byte) nodes[i].Nodes.Count);
            }
            code += GenFooter(nodes[0].Value, instName, instName);
            return code;
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            throw new NotImplementedException();
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Device Root Table Header
            code += "/*********************************************************************\n";
            code += " Device Table -- Indexed by the device number.\n";
            code += " *********************************************************************/\n";
            code += GenerateLUTHeader(qualName + "_TABLE[]");
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            throw new NotImplementedException();
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = NEWLINE;
            code += GenerateFooter() + ";" + NEWLINE;
            return code;
        }
    }

    internal class UsbDeviceTableGen : UsbCodeGen
    {
        public UsbDeviceTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";
            // Generate the children
            if (node.Nodes.Count > 0)
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    UsbConfigTableGen cgt = new UsbConfigTableGen(INSTANCE_NAME);
                    string qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                    code += cgt.Generate(node.Nodes[i], instName, qual);
                }
            }

            code += GenHeader(node.Value, instName, qualName);
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                string qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                code += this.GenLUTEntry(qual, "_TABLE", (byte) node.Nodes.Count);
            }
            code += GenFooter(node.Value, instName, qualName);

            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            DeviceDescriptor d = (DeviceDescriptor) ud;
            // Device  Table Header
            code += "/*********************************************************************\n";
            code += " Device Dispatch Table -- Points to the Device Descriptor and each of\n";
            code += "                          and Configuration Tables for this Device \n";
            code += " *********************************************************************/\n";
            code += GenerateLUTHeader(qualName + "_TABLE[]");
            code += GenLUTEntry(qualName, "_DESCR", d.bNumConfigurations);
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            throw new NotImplementedException();
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\n};\n";
            return code;
        }
    }

    internal class UsbConfigTableGen : UsbCodeGen
    {
        public UsbConfigTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";

            // Generate any child tables
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                UsbInterfaceTableGen cgt = new UsbInterfaceTableGen(INSTANCE_NAME);

                for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
                {
                    string qual = qualName + "_" + node.Nodes[i].Nodes[j].Value.bDescriptorType + j; //+ i.ToString;
                    code += cgt.Generate(node.Nodes[i].Nodes[j], instName, qual);
                }
            }
            // Generate the Endpoint Table
            UsbEndportTableGen gtEP = new UsbEndportTableGen(INSTANCE_NAME);
            code += gtEP.Generate(node, instName, qualName);

            code += GenHeader(node.Value, instName, qualName);
            code += GenLUTEntry(qualName, "_EP_SETTINGS_TABLE", gtEP.GetNumEPDescriptors(node));

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
                {
                    string qual = qualName + "_" + node.Nodes[i].Nodes[j].Value.bDescriptorType + j; //+ i.ToString;
                    code += GenInterfaceTblPtr(node.Nodes[i].Nodes[j], instName, qual);
                }
            }
            code += GenFooter(node.Value, instName, qualName);

            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            ConfigDescriptor d = (ConfigDescriptor) ud;
            // Device  Table Header
            code += "/*********************************************************************\n";
            code += " Config Dispatch Table -- Points to the Config Descriptor and each of\n";
            code += "                          and endpoint setup table and to each interface \n";
            code += "                          table if it specifies a USB Class \n";
            code += " *********************************************************************/\n";
            code += GenerateLUTHeader(qualName + "_TABLE[]");
            code += GenLUTEntry(qualName, "_DESCR", 1);
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\n};\n";
            return code;
        }

        private string GenInterfaceTblPtr(DescriptorNode node, string instanceName, string qualName)
        {
            string code = "";

            InterfaceDescriptor d = (InterfaceDescriptor) node.Value;
            // for (int i = 0; i < node.Nodes.Count; i++)
            {
                if ((d.bInterfaceClass == CyUSBFSParameters.CLASS_NONE) ||
                    (d.bInterfaceClass == CyUSBFSParameters.CLASS_VENDORSPEC))
                {
                    code += GenNullLUTEntry();
                }
                else
                {
                    code += GenLUTEntry(qualName, "_TABLE", GetClassDescriptorCount(node));
                }
            }
            return code;
        }

        private byte GetClassDescriptorCount(DescriptorNode node)
        {
            byte count = 0;

            foreach (DescriptorNode node_i in node.Nodes)
            {
                if (node_i.Value.bDescriptorType != USBDescriptorType.ENDPOINT)
                {
                    count++;
                }
            }
            return count;
        }
    }

    internal class UsbInterfaceTableGen : UsbCodeGen
    {
        public UsbInterfaceTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";

            if (FindClassDescriptor(node))
            {
// Generate the children
                if (node.Nodes.Count > 0)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        string qual;
                        if (node.Nodes[i].Value.bDescriptorType == USBDescriptorType.ALTERNATE)
                        {
                            qual = qualName;
                        }
                        else
                        {
                            qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                        }
                    }
                }
                code += GenHeader(node.Value, instName, qualName);

                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    string qual;
                    if (node.Nodes[i].Value.bDescriptorType == USBDescriptorType.ALTERNATE)
                    {
                        qual = qualName;
                    }
                    else
                    {
                        qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                    }
                    code += GenBody(node.Nodes[i].Value, qual, qual);
                }
                code += GenFooter(node.Value, instName, qualName);
            }
            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            // Device  Table Header
            code += "/*********************************************************************\n";
            code += " Interface Dispatch Table -- Points to the Class Dispatch Tables\n";
            code += " *********************************************************************/\n";
            code += GenerateLUTHeader(qualName + "_TABLE[]");
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            if (ud.bDescriptorType != USBDescriptorType.ENDPOINT)
                code += GenerateLUTEntry(qualName + "_TABLE", qualName + "_COUNT");
            return code;
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "\n};\n";
            return code;
        }

        private bool FindClassDescriptor(DescriptorNode node)
        {
            bool found = false;

            foreach (DescriptorNode n in node.Nodes)
            {
                if (n.Nodes.Count > 0)
                {
                    found = FindClassDescriptor(n);
                }
                if (n.Value.bDescriptorType == USBDescriptorType.HID)
                {
                    found = true;
                }
            }
            return found;
        }
    }

    internal class UsbEndportTableGen : UsbCodeGen
    {
        public UsbEndportTableGen(string InstanceName)
            : base(InstanceName)
        {
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";

            if (node.Value.bDescriptorType == USBDescriptorType.CONFIGURATION)
            {
                code += GenHeader(node.Value, instName, qualName);
                foreach (DescriptorNode igd in node.Nodes)
                {
                    foreach (DescriptorNode id in igd.Nodes)
                    {
                        InterfaceDescriptor d = (InterfaceDescriptor) id.Value;
                        byte CurInterface = d.bInterfaceNumber;
                        byte CurAltSetting = d.bAlternateSetting;
                        foreach (DescriptorNode en in id.Nodes)
                        {
                            if (en.Value.bDescriptorType == USBDescriptorType.ENDPOINT)
                            {
                                code += FormatEndpointSetting(CurInterface, CurAltSetting, 
                                    (EndpointDescriptor) en.Value);
                            }
                        }
                    }
                }
                code += GenFooter(node.Value, instName, qualName);
            }

#if false
		    if (node.Value.bDescriptorType == USBDescriptorType.CONFIGURATION)
            {
                // Scan the tree for all of the interfaces and record the endpoint information

                byte CurInterface = 0;
                byte CurAltSetting = 0;

                code += GenHeader(node.Value, instName, qualName);
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    string qual = qualName + "_" + node.Nodes[i].Value.bDescriptorType + i;//+ i.ToString;
                    InterfaceDescriptor d = (InterfaceDescriptor)node.Nodes[i].Value;
                    CurInterface = d.bInterfaceNumber;
                    CurAltSetting = d.bAlternateSetting;
                    for (int j = 0; j < node.Nodes[i].Nodes.Count; j++)
                    {
                        if (node.Nodes[i].Nodes[j].Value.bDescriptorType == USBDescriptorType.ENDPOINT)
                        {
                            code += FormatEndpointSetting(CurInterface, CurAltSetting, 
                                    (EndpointDescriptor)node.Nodes[i].Nodes[j].Value);
                        }
                    }
                }
  
                code += GenFooter(node.Value, instName, qualName);
            }
#endif
            return code;
        }

        public override string GenHeader(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            ConfigDescriptor d = (ConfigDescriptor) ud;
            // Table Header
            code += "/*********************************************************************\n";
            code += "  Endpoint Setting Table -- This table contain the endpoint setting\n";
            code += "                            for each endpoint in the configuration.  It\n";
            code += "                            contains the necessary information to\n";
            code += "                            configure the endpoint hardware for each\n";
            code += "                            interface and alternate setting.\n";
            code += "  *********************************************************************/\n";
            code += "T_" + INSTANCE_NAME + "_EP_SETTINGS_BLOCK " + CODE + " " + qualName + "_EP_SETTINGS_TABLE[] = {\n";
            code += "/* IFC  ALT    EPAddr bmAttr MaxPktSize BufferType ********************/\n";
            return code;
        }

        public override string GenBody(USBDescriptor ud, string instanceName, string qualName)
        {
            throw new NotImplementedException();
        }

        public override string GenFooter(USBDescriptor ud, string instanceName, string qualName)
        {
            string code = "";
            code += "\n};\n";
            return code;
        }

        public byte GetNumEPDescriptors(DescriptorNode node)
        {
            byte count = 0;
            if (node.Nodes.Count == 0)
            {
                if (node.Value.bDescriptorType == USBDescriptorType.ENDPOINT)
                {
                    count = 1;
                }
            }
            else
            {
                foreach (DescriptorNode node_i in node.Nodes)
                {
                    count += GetNumEPDescriptors(node_i);
                }
            }
            return count;
        }

        private string FormatEndpointSetting(byte ifc, byte alt, EndpointDescriptor d)
        {
            string code = "";
            code += m_nl;
            code += "{";
            code += GenerateByte(ifc) + ", ";
            code += GenerateByte(alt) + ", ";
            code += GenerateByte(d.bEndpointAddress) + ", ";
            code += GenerateByte(d.bmAttributes) + ", ";
            code += GenerateWord(d.wMaxPacketSize) + ",   ";
            code += GenerateByte(d.DoubleBuffer ? (byte) 1 : (byte) 0);
            code += "}";
            m_nl = COMMA_NEWLINE;
            return code;
        }
    }

    #endregion

    #region Tree Generators

    internal abstract class GenTree
    {
        public abstract string Generate(List<DescriptorNode> nodes, string instName, string qualName);
        public abstract string Generate(DescriptorNode node, string instName, string qualName);
    }

    internal class UsbDescriptorGenTree : GenTree
    {
        public UsbDescriptorGenTree()
        {
        }

        public override string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";

            UsbCodeGenMaker f = new UsbCodeGenMaker();

            UsbDescriptorGenTree cgt = new UsbDescriptorGenTree();

            for (int i = 0; i < nodes.Count; i++)
            {
                string qual = qualName + "_" + nodes[i].Value.bDescriptorType + i; //+ i.ToString;
                UsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].Value, instName);
                code += cg.GenHeader(nodes[i].Value, instName, qual);
                code += cg.GenBody(nodes[i].Value, instName, qual);
                if (nodes[i].Value.bDescriptorType == USBDescriptorType.INTERFACE)
                {
                    DescriptorNode n = nodes[i];

                    // Class Descriptors first
                    for (int j = 0; j < n.Nodes.Count; j++)
                    {
                        if (n.Nodes[j].Value.bDescriptorType != USBDescriptorType.ENDPOINT)
                        {
                            code += cgt.Generate(n.Nodes[j], instName, qual);
                        }
                    }
                    // Now, the endpoints
                    for (int j = 0; j < n.Nodes.Count; j++)
                    {
                        if (n.Nodes[j].Value.bDescriptorType == USBDescriptorType.ENDPOINT)
                        {
                            code += cgt.Generate(n.Nodes[j], instName, qual);
                        }
                    }
                }
                else
                {
                    code += cgt.Generate(nodes[i].Nodes, instName, qual);
                }
                code += cg.GenFooter(nodes[i].Value, instName, qual);
            }
            return code;
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            string code = "";
            UsbCodeGenMaker f = new UsbCodeGenMaker();
            UsbCodeGen cg = f.MakeDescriptorGenerator(node.Value, instName);
            code += cg.Generate(node, instName, qualName);

            return code;
        }
    }

    internal class UsbStringDescriptorGenTree : GenTree
    {
        public UsbStringDescriptorGenTree()
        {
        }

        public override string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";

            if (nodes.Count > 1)
            {
                UsbCodeGenMaker f = new UsbCodeGenMaker();

                //The zeroth string should be the LangID/Zero code generator
                UsbCodeGen zcg = f.MakeDescriptorGenerator(nodes[0].Value, instName);

                code += zcg.GenHeader(nodes[0].Value, instName, qualName);
                code += zcg.GenBody(nodes[0].Value, instName, qualName);

                for (int i = 1; i < nodes.Count; i++)
                {
                    UsbCodeGen cg = f.MakeDescriptorGenerator(nodes[i].Value, instName);
                    code += cg.GenHeader(nodes[i].Value, instName, qualName);
                    code += cg.GenBody(nodes[i].Value, instName, qualName);
                    code += cg.GenFooter(nodes[i].Value, instName, qualName);
                }
                code += zcg.GenFooter(nodes[0].Value, instName, qualName);
            }
            return code;
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            throw new NotImplementedException();
        }
    }

    internal class UsbHIDReportDescriptorGenTree : GenTree
    {
        public UsbHIDReportDescriptorGenTree()
        {
        }

        public override string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";

            if (nodes.Count > 0)
            {
                UsbCodeGenMaker f = new UsbCodeGenMaker();

                UsbCodeGen cg = f.MakeDescriptorGenerator(nodes[0].Value, instName);

                UsbHIDReportItemGenTree hgt = new UsbHIDReportItemGenTree();

                UsbCodePrimitives prim = new UsbCodePrimitives(instName);

                code += cg.GenHeader(nodes[0].Value, instName, qualName);
                for (int i = 0; i < nodes.Count; i++)
                {
                    code += cg.GenBody(nodes[i].Value, instName, qualName);
                    code +=
                        prim.GenerateDescrDWLS("Descriptor Size (Not part of descriptor)",
                                               ReportSize(nodes[0].Nodes, prim)) + prim.COMMA_NEWLINE;
                    code += hgt.Generate(nodes[i], instName, qualName); // Generate the HID Report Items
                }
                code += cg.GenFooter(nodes[0].Value, instName, qualName);
            }
            return code;
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            return Generate(node.Nodes, instName, qualName);
        }

        private ushort ReportSize(List<DescriptorNode> nodes, UsbCodePrimitives p)
        {
            ushort size = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                HIDReportItemDescriptor h = (HIDReportItemDescriptor) nodes[i].Value;
                size += p.CalcHIDItemLength(h);
            }
            return size;
        }
    }

    internal class UsbHIDReportItemGenTree : GenTree
    {
        public UsbHIDReportItemGenTree()
        {
        }

        public override string Generate(List<DescriptorNode> nodes, string instName, string qualName)
        {
            string code = "";

            if (nodes.Count > 0)
            {
                UsbCodeGenMaker f = new UsbCodeGenMaker();

                UsbCodeGen cg = f.MakeDescriptorGenerator(nodes[0].Value, instName);

                for (int i = 0; i < nodes.Count; i++)
                {
                    code += cg.GenBody(nodes[i].Value, instName, qualName);
                }
                code += cg.GenFooter(nodes[0].Value, instName, qualName);
            }
            return code;
        }

        public override string Generate(DescriptorNode node, string instName, string qualName)
        {
            return Generate(node.Nodes, instName, qualName);
        }
    }

    internal class UsbCodeGenMaker
    {
        public UsbCodeGenMaker()
        {
        }

        public UsbCodeGen MakeDescriptorGenerator(USBDescriptor d, string instName)
        {
            UsbCodeGen c;

            Type d_t = d.GetType();

            if (d_t == typeof (DeviceDescriptor))
            {
                c = new UsbDeviceDescriptorGen(instName);
            }
            else if (d_t == typeof (ConfigDescriptor))
            {
                c = new UsbConfigDescriptorGen(instName);
            }
            else if (d_t == typeof (InterfaceGeneralDescriptor))
            {
                c = new UsbInterfaceGeneralDescriptorGen(instName);
            }
            else if (d_t == typeof (InterfaceDescriptor))
            {
                c = new UsbInterfaceDescriptorGen(instName);
            }
            else if (d_t == typeof (EndpointDescriptor))
            {
                c = new UsbEndpointDescriptorGen(instName);
            }
            else if (d_t == typeof (HIDDescriptor))
            {
                c = new UsbHIDClassDescriptorGen(instName);
            }
            else if (d_t == typeof (StringDescriptor))
            {
                c = new UsbStringDescriptorGen(instName);
            }
            else if (d_t == typeof (StringZeroDescriptor))
            {
                c = new UsbStringZeroDescriptorGen(instName);
            }
            else if (d_t == typeof (HIDReportDescriptor))
            {
                c = new UsbHIDReportDescriptorGen(instName);
            }
            else if (d_t == typeof (HIDReportItemDescriptor))
            {
                c = new UsbHIDReportItemGen(instName);
            }
            else
            {
                throw new NotImplementedException();
            }

            return c;
        }

#if false
        public UsbCodeGen MakeStorageGenerator(USBDescriptor d)
        {
            UsbCodeGen c;

            Type d_t = d.GetType();

            if (d_t == typeof(DeviceDescriptor))
            {
                c = new UsbUnknownDescriptorGen();
            }
            else if (d_t == typeof(ConfigDescriptor))
            {
                c = new UsbUnknownDescriptorGen();
            }
            else if (d_t == typeof(InterfaceDescriptor))
            {
                c = new UsbUnknownDescriptorGen();
            }
            else if (d_t == typeof(EndpointDescriptor))
            {
                c = new UsbUnknownDescriptorGen();
            }
            else if (d_t == typeof(HIDDescriptor))
            {
                c = new UsbUnknownDescriptorGen();
            }
            else
            {
                throw new NotImplementedException();
            }

            return c;
        }
#endif
//HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
#if false
        public UsbCodeGen MakeTableGenerator(USBDescriptor d, string InstanceName)
        {
            UsbCodeGen c = null;

            Type d_t;

            if (d == null)  // Assume this is the root
            {
                c = new UsbDeviceRootTableGen(InstanceName);
            }
            else
            {
                d_t = d.GetType();
                if (d_t == typeof(DeviceDescriptor))
                {
                    c = new UsbDeviceTableGen();
                }
#endif
#if false
                else if (d_t == typeof(ConfigDescriptor))
                {
                    c = new UsbConfigTableGen();
                }
                else if (d_t == typeof(InterfaceDescriptor))
                {
                    c = new UsbInterfaceTableGen();
                }
                else if (d_t == typeof(EndpointDescriptor))
                {
                    c = new UsbEndpointTableGen();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return c;
        }
#endif
    }

    #endregion Tree Generators
}