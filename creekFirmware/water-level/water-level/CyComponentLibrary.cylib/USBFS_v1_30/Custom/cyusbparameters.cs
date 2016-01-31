// ========================================
/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CyDesigner.Extensions.Gde;

namespace USBFS_v1_30
{
    public class CyUSBFSParameters
    {
        #region Const

        public const byte CLASS_NONE = 0x00;
        public const byte CLASS_HID = 0x03;
        public const byte CLASS_CDC = 0x02;
        public const byte CLASS_AUDIO = 0x01;
        public const byte CLASS_VENDORSPEC = 0xFF;

        public const int TRANSFERTYPE_CONTROL = 0x00;
        public const int TRANSFERTYPE_ISOCHRONOUS = 0x01;
        public const int TRANSFERTYPE_BULK = 0x02;
        public const int TRANSFERTYPE_INTERRUPT = 0x03;

        public const int USAGETYPE_DATAEP = 0x00;
        public const int USAGETYPE_FEEDBACKEP = 0x01;
        public const int USAGETYPE_IMPLICITEEP = 0x02;

        public const int SYNCHTYPE_NOSYNCH = 0x00;
        public const int SYNCHTYPE_ASYNCH = 0x01;
        public const int SYNCHTYPE_ADAPTIVE = 0x02;
        public const int SYNCHTYPE_SYNCH = 0x03;

        public const string MSOS_STRING = "MSFT100";

        #endregion Const

        #region Variables

        private readonly ICyInstEdit_v1 inst;

        #region Lang

        public static UInt16[] LangIDs = {
                                             0x0436,
                                             0x041c,
                                             0x0401,
                                             0x0801,
                                             0x0c01,
                                             0x1001,
                                             0x1401,
                                             0x1801,
                                             0x1c01,
                                             0x2001,
                                             0x2401,
                                             0x2801,
                                             0x2c01,
                                             0x3001,
                                             0x3401,
                                             0x3801,
                                             0x3c01,
                                             0x4001,
                                             0x042b,
                                             0x044d,
                                             0x042c,
                                             0x082c,
                                             0x042d,
                                             0x0423,
                                             0x0445,
                                             0x0402,
                                             0x0455,
                                             0x0403,
                                             0x0404,
                                             0x0804,
                                             0x0c04,
                                             0x1004,
                                             0x1404,
                                             0x041a,
                                             0x0405,
                                             0x0406,
                                             0x0413,
                                             0x0813,
                                             0x0409,
                                             0x0809,
                                             0x0c09,
                                             0x1009,
                                             0x1409,
                                             0x1809,
                                             0x1c09,
                                             0x2009,
                                             0x2409,
                                             0x2809,
                                             0x2c09,
                                             0x3009,
                                             0x3409,
                                             0x0425,
                                             0x0438,
                                             0x0429,
                                             0x040b,
                                             0x040c,
                                             0x080c,
                                             0x0c0c,
                                             0x100c,
                                             0x140c,
                                             0x180c,
                                             0x0437,
                                             0x0407,
                                             0x0807,
                                             0x0c07,
                                             0x1007,
                                             0x1407,
                                             0x0408,
                                             0x0447,
                                             0x040d,
                                             0x0439,
                                             0x040e,
                                             0x040f,
                                             0x0421,
                                             0x0410,
                                             0x0810,
                                             0x0411,
                                             0x044b,
                                             0x0860,
                                             0x043f,
                                             0x0457,
                                             0x0412,
                                             0x0812,
                                             0x0426,
                                             0x0427,
                                             0x0827,
                                             0x042f,
                                             0x043e,
                                             0x083e,
                                             0x044c,
                                             0x0458,
                                             0x044e,
                                             0x0861,
                                             0x0414,
                                             0x0814,
                                             0x0448,
                                             0x0415,
                                             0x0416,
                                             0x0816,
                                             0x0446,
                                             0x0418,
                                             0x0419,
                                             0x044f,
                                             0x0c1a,
                                             0x081a,
                                             0x0459,
                                             0x041b,
                                             0x0424,
                                             0x040a,
                                             0x080a,
                                             0x0c0a,
                                             0x100a,
                                             0x140a,
                                             0x180a,
                                             0x1c0a,
                                             0x200a,
                                             0x240a,
                                             0x280a,
                                             0x2c0a,
                                             0x300a,
                                             0x340a,
                                             0x380a,
                                             0x3c0a,
                                             0x400a,
                                             0x440a,
                                             0x480a,
                                             0x4c0a,
                                             0x500a,
                                             0x0430,
                                             0x0441,
                                             0x041d,
                                             0x081d,
                                             0x0449,
                                             0x0444,
                                             0x044a,
                                             0x041e,
                                             0x041f,
                                             0x0422,
                                             0x0420,
                                             0x0820,
                                             0x0443,
                                             0x0843,
                                             0x042a,
                                             0x04ff
                                         };

        public static string[] LangIDNames = {
                                                 "Afrikaans",
                                                 "Albanian",
                                                 "Arabic(Saudi Arabia)",
                                                 "Arabic(Iraq)",
                                                 "Arabic(Egypt)",
                                                 "Arabic(Libya)",
                                                 "Arabic(Algeria)",
                                                 "Arabic(Morocco)",
                                                 "Arabic(Tunisia)",
                                                 "Arabic(Oman)",
                                                 "Arabic(Yemen)",
                                                 "Arabic(Syria)",
                                                 "Arabic(Jordan)",
                                                 "Arabic(Lebanon)",
                                                 "Arabic(Kuwait)",
                                                 "Arabic(U.A.E.)",
                                                 "Arabic(Bahrain)",
                                                 "Arabic(Qatar)",
                                                 "Armenian.",
                                                 "Assamese.",
                                                 "Azeri(Latin)",
                                                 "Azeri(Cyrillic)",
                                                 "Basque",
                                                 "Belarussian",
                                                 "Bengali.",
                                                 "Bulgarian",
                                                 "Burmese",
                                                 "Catalan",
                                                 "Chinese(Taiwan)",
                                                 "Chinese(PRC)",
                                                 "Chinese(Hong Kong SAR, PRC)",
                                                 "Chinese(Singapore)",
                                                 "Chinese(Macau SAR)",
                                                 "Croatian",
                                                 "Czech",
                                                 "Danish",
                                                 "Dutch(Netherlands)",
                                                 "Dutch(Belgium)",
                                                 "English(United States)",
                                                 "English(United Kingdom)",
                                                 "English(Australian)",
                                                 "English(Canadian)",
                                                 "English(New Zealand)",
                                                 "English(Ireland)",
                                                 "English(South Africa)",
                                                 "English(Jamaica)",
                                                 "English(Caribbean)",
                                                 "English(Belize)",
                                                 "English(Trinidad)",
                                                 "English(Zimbabwe)",
                                                 "English(Philippines)",
                                                 "Estonian",
                                                 "Faeroese",
                                                 "Farsi",
                                                 "Finnish",
                                                 "French(Standard)",
                                                 "French(Belgian)",
                                                 "French(Canadian)",
                                                 "French(Switzerland)",
                                                 "French(Luxembourg)",
                                                 "French(Monaco)",
                                                 "Georgian",
                                                 "German(Standard)",
                                                 "German(Switzerland)",
                                                 "German(Austria)",
                                                 "German(Luxembourg)",
                                                 "German(Liechtenstein)",
                                                 "Greek",
                                                 "Gujarati.",
                                                 "Hebrew",
                                                 "Hindi.",
                                                 "Hungarian",
                                                 "Icelandic",
                                                 "Indonesian",
                                                 "Italian(Standard)",
                                                 "Italian(Switzerland)",
                                                 "Japanese",
                                                 "Kannada.",
                                                 "Kashmiri(India)",
                                                 "Kazakh",
                                                 "Konkani.",
                                                 "Korean",
                                                 "Korean(Johab)",
                                                 "Latvian",
                                                 "Lithuanian",
                                                 "Lithuanian(Classic)",
                                                 "Macedonian",
                                                 "Malay(Malaysian)",
                                                 "Malay(Brunei Darussalam)",
                                                 "Malayalam.",
                                                 "Manipuri",
                                                 "Marathi.",
                                                 "Nepali(India).",
                                                 "Norwegian(Bokmal)",
                                                 "Norwegian(Nynorsk)",
                                                 "Oriya.",
                                                 "Polish",
                                                 "Portuguese(Brazil)",
                                                 "Portuguese(Standard)",
                                                 "Punjabi.",
                                                 "Romanian",
                                                 "Russian",
                                                 "Sanskrit.",
                                                 "Serbian(Cyrillic)",
                                                 "Serbian(Latin)",
                                                 "Sindhi",
                                                 "Slovak",
                                                 "Slovenian",
                                                 "Spanish(Traditional Sort)",
                                                 "Spanish(Mexican)",
                                                 "Spanish(Modern Sort)",
                                                 "Spanish(Guatemala)",
                                                 "Spanish(Costa Rica)",
                                                 "Spanish(Panama)",
                                                 "Spanish(Dominican Republic)",
                                                 "Spanish(Venezuela)",
                                                 "Spanish(Colombia)",
                                                 "Spanish(Peru)",
                                                 "Spanish(Argentina)",
                                                 "Spanish(Ecuador)",
                                                 "Spanish(Chile)",
                                                 "Spanish(Uruguay)",
                                                 "Spanish(Paraguay)",
                                                 "Spanish(Bolivia)",
                                                 "Spanish(El Salvador)",
                                                 "Spanish(Honduras)",
                                                 "Spanish(Nicaragua)",
                                                 "Spanish(Puerto Rico)",
                                                 "Sutu",
                                                 "Swahili(Kenya)",
                                                 "Swedish",
                                                 "Swedish(Finland)",
                                                 "Tamil",
                                                 "Tatar(Tatarstan)",
                                                 "Telugu.",
                                                 "Thai",
                                                 "Turkish",
                                                 "Ukrainian",
                                                 "Urdu(Pakistan)",
                                                 "Urdu(India)",
                                                 "Uzbek(Latin)",
                                                 "Uzbek(Cyrillic)",
                                                 "Vietnamese",
                                                 "HID(Usage Data Descriptor)"
                                             };

        public static string[] CountryCodes = {
                                                  "Not Supported",
                                                  "Arabic",
                                                  "Belgian",
                                                  "Canadian-Bilingual",
                                                  "Canadian-French",
                                                  "Czech Republic",
                                                  "Danish",
                                                  "Finnish",
                                                  "French",
                                                  "German",
                                                  "Greek",
                                                  "Hebrew",
                                                  "Hungary",
                                                  "International (ISO)",
                                                  "Italian",
                                                  "Japan (Katakana)",
                                                  "Korean",
                                                  "Latin American",
                                                  "Netherlands/Dutch",
                                                  "Norwegian",
                                                  "Persian (Farsi)",
                                                  "Poland",
                                                  "Portuguese",
                                                  "Russia",
                                                  "Slovakia",
                                                  "Spanish",
                                                  "Swedish",
                                                  "Swiss/French",
                                                  "Swiss/German",
                                                  "Switzerland",
                                                  "Taiwan",
                                                  "Turkish-Q",
                                                  "UK",
                                                  "US",
                                                  "Yugoslavia",
                                                  "Turkish-F"
                                              };

        #endregion Lang

        public ArrayList EmptyFields;

        public DescriptorTree DeviceTree;
        public DescriptorTree StringTree;
        public DescriptorTree HIDReportTree;

        public string _SerializedDeviceDesc;
        public string _SerializedStringDesc;
        public string _SerializedHIDReportDesc;

        private bool[] m_UsedEp = new bool[9];
        private bool m_mon_vbus;

        private int _ParamsChanged = 0;

        public bool ParamsChanged
        {
            //get { return _ParamsChanged; }
            set
            {
                _ParamsChanged++;
                SetParam("DescChanged");
                CommitParams();
            }
        }

        public string SerializedDeviceDesc
        {
            get { return _SerializedDeviceDesc; }
            set
            {
                if (value != _SerializedDeviceDesc)
                {
                    _SerializedDeviceDesc = value;
                    _SerializedDeviceDesc = _SerializedDeviceDesc.Replace("\r\n", " ");
                    SetParam("DeviceDescriptors");
                    CommitParams();
                }
            }
        }

        public string SerializedStringDesc
        {
            get { return _SerializedStringDesc; }
            set
            {
                if (value != _SerializedStringDesc)
                {
                    _SerializedStringDesc = value;
                    _SerializedStringDesc = _SerializedStringDesc.Replace("\r\n", " ");
                    SetParam("StringDescriptors");
                    CommitParams();
                }
            }
        }

        public string SerializedHIDReportDesc
        {
            get { return _SerializedHIDReportDesc; }
            set
            {
                if (value != _SerializedHIDReportDesc)
                {
                    _SerializedHIDReportDesc = value;
                    _SerializedHIDReportDesc = _SerializedHIDReportDesc.Replace("\r\n", " ");
                    SetParam("HIDReportDescriptors");
                    CommitParams();
                }
            }
        }

        #endregion Variables

        #region Constructors

        public CyUSBFSParameters()
        {
            DeviceTree = new DescriptorTree();
            StringTree = new DescriptorTree();
            HIDReportTree = new DescriptorTree();
            EmptyFields = new ArrayList();
        }

        public CyUSBFSParameters(ICyInstEdit_v1 inst)
        {
            DeviceTree = new DescriptorTree();
            StringTree = new DescriptorTree();
            HIDReportTree = new DescriptorTree();
            EmptyFields = new ArrayList();
            this.inst = inst;
            GetParams();
        }

        #endregion Constructors

        #region Common

        private void GetParams()
        {
            if (inst != null)
            {
                _ParamsChanged = Convert.ToInt32(inst.GetCommittedParam("DescChanged").Value);
                if (_ParamsChanged > 1000) _ParamsChanged = 0;
                _SerializedDeviceDesc = Convert.ToString(inst.GetCommittedParam("DeviceDescriptors").Value);
                _SerializedStringDesc = Convert.ToString(inst.GetCommittedParam("StringDescriptors").Value);
                _SerializedHIDReportDesc = Convert.ToString(inst.GetCommittedParam("HIDReportDescriptors").Value);
                DeserializeDescriptors();
            }
        }

        public void SetParam(string paramName)
        {
            if (inst != null)
            {
                switch (paramName)
                {
                    case "DeviceDescriptors":
                        inst.SetParamExpr("DeviceDescriptors", SerializedDeviceDesc);
                        break;
                    case "StringDescriptors":
                        inst.SetParamExpr("StringDescriptors", SerializedStringDesc);
                        break;
                    case "HIDReportDescriptors":
                        inst.SetParamExpr("HIDReportDescriptors", SerializedHIDReportDesc);
                        break;
                    case "DescChanged":
                        inst.SetParamExpr("DescChanged", _ParamsChanged.ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void SetParams()
        {
            if (inst != null)
            {
                inst.SetParamExpr("DeviceDescriptors", SerializedDeviceDesc);
                inst.SetParamExpr("StringDescriptors", SerializedStringDesc);
                inst.SetParamExpr("HIDReportDescriptors", SerializedHIDReportDesc);
                inst.SetParamExpr("DescChanged", _ParamsChanged.ToString());
            }
        }

        public void SetParam_rm_ep_isr()
        {
            if (inst != null)
            {
                for (int i = 1; i < m_UsedEp.Length; i++)
                {
                    string param = "rm_ep_isr_" + i;
                    if(m_UsedEp[i] == true)
                    {
                        inst.SetParamExpr(param, false.ToString());
                    }
                    else
                    {
                        inst.SetParamExpr(param, true.ToString());
                    }
                }
                CommitParams();
            }
        }

        public void SetParam_mon_vbus()
        {
            if (inst != null)
            {
                string param = "mon_vbus";
                    if (m_mon_vbus == true)
                    {
                        inst.SetParamExpr(param, true.ToString());
                    }
                    else
                    {
                        inst.SetParamExpr(param, false.ToString());
                    }
                CommitParams();
            }
        }

        public void CommitParams()
        {
            if (inst != null)
            {
                if (!inst.CommitParamExprs())
                {
                    /*if (inst.GetCommittedParam("").ErrorCount > 0)
                        MessageBox.Show("");
                   */
                }
            }
        }

        public void DeserializeDescriptors()
        {
            //try
            {
                if (!string.IsNullOrEmpty(_SerializedDeviceDesc))
                {
                    DeviceTree = DescriptorTree.DeserializeDescriptors(_SerializedDeviceDesc);
                    CheckOldFormatCompability(DeviceTree);
                }
                else
                {
                    InitDescriptors(true, false, false);
                }
                if (!string.IsNullOrEmpty(_SerializedStringDesc))
                {
                    StringTree = DescriptorTree.DeserializeDescriptors(_SerializedStringDesc);
                    CheckStringOldFormatCompability(StringTree);
                }
                else
                {
                    InitDescriptors(false, true, false);
                }
                if (!string.IsNullOrEmpty(_SerializedHIDReportDesc))
                    HIDReportTree = DescriptorTree.DeserializeDescriptors(_SerializedHIDReportDesc);
                else
                {
                    InitDescriptors(false, false, true);
                }
            }
            //catch
            //{
            //    MessageBox.Show("Can't load parameters. Some parameter has incorrect format.", 
            //                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        #endregion Common

        #region Private functions

        /// <summary>
        /// Fills Device, String and HIDReport trees with the default nodes.
        /// </summary>
        /// <param name="device">Apply to the DeviceTree or no</param>
        /// <param name="str">Apply to the StringTree or no</param>
        /// <param name="hidReport">Apply to the HIDReportTree or no</param>
        public void InitDescriptors(bool device, bool str, bool hidReport)
        {
            if (device)
                DeviceTree.AddNode("Device");
            if (str)
            {
                StringTree.AddNode("String");
                StringTree.AddNode("SpecialString");
            }
            if (hidReport)
                HIDReportTree.AddNode("HIDReport");
        }

        /// <summary>
        /// Finds the index of the string that was selected in the descriptor.
        /// </summary>
        /// <param name="comboBox">Combobox where user selects the string descriptor</param>
        /// <param name="parameters">Reference to the Parameters</param>
        /// <returns>Unique index of the string</returns>
        public static uint SaveStringDescriptor(ComboBox comboBox, CyUSBFSParameters parameters)
        {
            StringDescriptor strDesc = null;
            string strConfigKey;
            uint strIndex = 0;

            if (comboBox.Text == "") return strIndex;

            if ((comboBox.SelectedIndex < 0) && (comboBox.Text != ""))
            {
                //Search for string in pre-defined array
                bool predefined = false;
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Text == comboBox.Items[i].ToString())
                    {
                        predefined = true;
                        strDesc = (StringDescriptor) comboBox.Items[i];
                        break;
                    }
                }
                if (!predefined)
                {
                    DescriptorNode newNode = parameters.StringTree.AddNode("String");
                    ((StringDescriptor) newNode.Value).bString = comboBox.Text;
                    strDesc = (StringDescriptor) newNode.Value;
                }
            }
            else
            {
                strDesc = (StringDescriptor) comboBox.Items[comboBox.SelectedIndex];
            }

            //New string index-based index
            //strIndex = (byte)(comboBox.SelectedIndex + 1);

            //General USBDescriptor-based index
            strConfigKey = parameters.StringTree.GetKeyByNode(strDesc);
            if (strConfigKey != "")
            {
                strIndex = DescriptorNode.GetDescriptorIndex(strConfigKey);
            }
            return strIndex;
        }

        /// <summary>
        /// Finds the index of the report that was selected in the descriptor.
        /// </summary>
        /// <param name="comboBox">Combobox where user selects the report</param>
        /// <param name="parameters">Reference to the Parameters</param>
        /// <returns>Unique index of the report</returns>
        public static uint SaveReportDescriptor(ComboBox comboBox, CyUSBFSParameters parameters)
        {
            HIDReportDescriptor reportDesc = null;
            string strConfigKey;
            uint strIndex = 0;

            if (comboBox.Text == "") return strIndex;


            reportDesc = (HIDReportDescriptor) comboBox.Items[comboBox.SelectedIndex];

            strConfigKey = parameters.HIDReportTree.GetKeyByNode(reportDesc);
            if (strConfigKey != "")
            {
                strIndex = DescriptorNode.GetDescriptorIndex(strConfigKey);
            }
            return strIndex;
        }

        /// <summary>
        /// Finds the index of the special string that was selected in the descriptor.
        /// </summary>
        /// <param name="comboBox">Combobox where user selects the special string descriptor</param>
        /// <param name="type">Reference to the Parameters</param>
        /// <param name="parameters"></param>
        /// <returns>Unique index of the string</returns>
        public static byte SaveSpecialStringDescriptor(ComboBox comboBox, string type, CyUSBFSParameters parameters)
        {
            string strConfigKey = type;
            byte strIndex = 0;
            if ((comboBox.SelectedIndex < 0) && (comboBox.Text != ""))
            {
                DescriptorNode newNode;
                if (type == "Serial")
                {
                    newNode = parameters.StringTree.GetNodeByKey(strConfigKey);
                    ((StringDescriptor) newNode.Value).bString = comboBox.Text;
                }
                strIndex = Convert.ToByte(DescriptorNode.GetDescriptorIndex(strConfigKey));
            }
            else if (comboBox.SelectedIndex >= 0)
            {
                strIndex = Convert.ToByte(DescriptorNode.GetDescriptorIndex(strConfigKey));
            }
            return strIndex;
        }

        /// <summary>
        /// Determines if the string is a comment (empty sting or starts with "//").
        /// Used in HID reports.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringIsComment(string str)
        {
            bool res = false;
            if ((str.TrimStart(' ') == "") || str.TrimStart(' ').StartsWith("//"))
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// Performs neccessary calculations in the descriptors when editing of desriptors is finished. 
        /// </summary>
        /// <param name="node"></param>
        public void RecalcDescriptors(DescriptorNode node)
        {
            //Clean EmptyFields array
            if (node.Key == "Device")
            {
                EmptyFields.Clear();

                // Reset m_UsedEp array 
                for (int i = 0; i < m_UsedEp.Length; i++)
                {
                    m_UsedEp[i] = false;
                }
                // Reset m_mon_vbus
                m_mon_vbus = false;
            }

            foreach (DescriptorNode node_child in node.Nodes)
            {
                if (node_child.Value != null)
                {
                    switch (node_child.Value.bDescriptorType)
                    {
                        case USBDescriptorType.DEVICE:
                            DeviceDescriptor deviceDescriptor = (DeviceDescriptor) node_child.Value;
                            deviceDescriptor.bNumConfigurations = (byte) node_child.Nodes.Count;
                            deviceDescriptor.iManufacturer = GetStringLocalIndex(deviceDescriptor.iwManufacturer);
                            deviceDescriptor.iProduct = GetStringLocalIndex(deviceDescriptor.iwProduct);
                            if (((StringDescriptor) StringTree.Nodes[1].Nodes[0].Value).bString == "")
                            {
                                deviceDescriptor.iSerialNumber = 0;
                            }
                            break;
                        case USBDescriptorType.CONFIGURATION:
                            ConfigDescriptor configDescriptor = (ConfigDescriptor) node_child.Value;
                            configDescriptor.bNumInterfaces = (byte) node_child.Nodes.Count;
                            // Calculate bConfigurationValue
                            int configIndex = node.Nodes.IndexOf(node_child);
                            if (configIndex >= 0)
                                configDescriptor.bConfigurationValue = (byte) configIndex;
                            else
                                configDescriptor.bConfigurationValue = 0;

                            configDescriptor.iConfiguration = GetStringLocalIndex(configDescriptor.iwConfiguration);
                            // Calculate the total length
                            ushort totalLength = configDescriptor.bLength;
                            foreach (DescriptorNode node_in_config in node_child.Nodes)
                                foreach (DescriptorNode node_in_interfaces in node_in_config.Nodes)
                                {
                                    totalLength += node_in_interfaces.Value.bLength;
                                    foreach (DescriptorNode node_in_interface in node_in_interfaces.Nodes)
                                    {
                                        totalLength += node_in_interface.Value.bLength;
                                    }
                                }
                            configDescriptor.wTotalLength = totalLength;
                            ushort totalSize = GetSumPacketSize(node_child);
                            // Set m_mon_vbus value
                            if ((configDescriptor.bmAttributes & (1 << 6)) > 0)
                            {
                                m_mon_vbus = true;
                            }
                            break;
                        case USBDescriptorType.INTERFACE:
                            InterfaceDescriptor interfaceDescriptor = (InterfaceDescriptor) node_child.Value;
                            //Calculate the number of Endpoints
                            byte numEndpt = 0;
                            foreach (DescriptorNode node_endpt in node_child.Nodes)
                            {
                                if (node_endpt.Value.bDescriptorType == USBDescriptorType.ENDPOINT)
                                    numEndpt++;
                            }
                            interfaceDescriptor.bNumEndpoints = numEndpt;
                            interfaceDescriptor.iInterface = GetStringLocalIndex(interfaceDescriptor.iwInterface);
                            break;
                        case USBDescriptorType.HID:
                            HIDDescriptor hidDescriptor = (HIDDescriptor) node_child.Value;
                            hidDescriptor.bNumDescriptors = 1;
                            hidDescriptor.ReportIndex = GetHIDReportLocalIndex(hidDescriptor.wReportIndex);

                            // Calculate wDescriptorLength
                            string reportKey = DescriptorNode.GetKeyByIndex(hidDescriptor.wReportIndex);
                            DescriptorNode nodeHIDReport = HIDReportTree.GetNodeByKey(reportKey);
                            if (nodeHIDReport != null)
                            {
                                hidDescriptor.wDescriptorLength = ((HIDReportDescriptor) nodeHIDReport.Value).wLength;
                            }
                            else
                            {
                                hidDescriptor.wReportIndex = 0;
                            }

                            if ((hidDescriptor.wReportIndex == 0) && (!EmptyFields.Contains(node_child.Key)))
                                EmptyFields.Add(node_child.Key);
                            break;
                        case USBDescriptorType.ENDPOINT:
                            EndpointDescriptor endpointDescriptor = (EndpointDescriptor)node_child.Value;
                            byte endpointNum = (byte)(endpointDescriptor.bEndpointAddress & 0x0F);
                            if (endpointNum < m_UsedEp.Length)
                                m_UsedEp[endpointNum] = true;
                            break;
                        case USBDescriptorType.HID_REPORT:
                            HIDReportDescriptor hidRptDescriptor = (HIDReportDescriptor) node_child.Value;
                            ushort rptSize = 0;
                            foreach (DescriptorNode node_in_report in node_child.Nodes)
                            {
                                rptSize += (ushort) (((HIDReportItemDescriptor) node_in_report.Value).Item.Size + 1);
                                // +1 - prefix
                            }
                            hidRptDescriptor.wLength = rptSize;
                            break;
                    }
                }
                RecalcDescriptors(node_child);
            }
        }

        /// <summary>
        /// Calculates the total packet size of the Configuration Descriptor.
        /// </summary>
        /// <param name="configNode">The Configuration Descriptor</param>
        /// <returns>Total packet size</returns>
        public ushort GetSumPacketSize(DescriptorNode configNode)
        {
            ushort totalSize = 0;
            foreach (DescriptorNode node_in_config in configNode.Nodes)
            {
                foreach (DescriptorNode node_in_interface in node_in_config.Nodes)
                {
                    if (node_in_interface.Value is EndpointDescriptor)
                    {
                        totalSize += ((EndpointDescriptor) node_in_interface.Value).wMaxPacketSize;
                    }
                }
            }
            return totalSize;
        }

        /// <summary>
        /// Returns the index of the String Descriptor in the array of String Descriptors. 
        /// This index is used in API generation.
        /// </summary>
        /// <param name="keyIndex">Unique internal index</param>
        /// <returns>Index that is used in API generation</returns>
        private byte GetStringLocalIndex(uint keyIndex)
        {
            byte localIndex = 0;
            if (keyIndex > 0)
            {
                for (byte i = 0; i < StringTree.Nodes[0].Nodes.Count; i++)
                {
                    if (DescriptorNode.GetDescriptorIndex(StringTree.Nodes[0].Nodes[i].Key) == keyIndex)
                    {
                        localIndex = i;
                        break;
                    }
                }
            }
            return localIndex;
        }

        /// <summary>
        /// Returns the index of the HID Report Descriptor in the array of HID Report Descriptors. 
        /// This index is used in API generation.
        /// </summary>
        /// <param name="keyIndex">Unique internal index</param>
        /// <returns>Index that is used in API generation</returns>
        private byte GetHIDReportLocalIndex(uint keyIndex)
        {
            byte localIndex = 0;
            if (keyIndex > 0)
            {
                for (byte i = 0; i < HIDReportTree.Nodes[0].Nodes.Count; i++)
                {
                    if (DescriptorNode.GetDescriptorIndex(HIDReportTree.Nodes[0].Nodes[i].Key) == keyIndex)
                    {
                        localIndex = (byte) (i + 1);
                        break;
                    }
                }
            }
            return localIndex;
        }

        /// <summary>
        /// For compability with the previous versions, checks 
        /// the presence of the ALTERNATE node in the Device Descriptor tree.
        /// </summary>
        /// <param name="dTree">Device Descriptor tree</param>
        private void CheckOldFormatCompability(DescriptorTree dTree)
        {
            bool oldFormat = false;
            for (int i = 0; i < dTree.Nodes.Count; i++) // Level 1
                for (int j = 0; j < dTree.Nodes[i].Nodes.Count; j++) // Level 2 - Device
                    for (int k = 0; k < dTree.Nodes[i].Nodes[j].Nodes.Count; k++) // Level 3 - Configuration
                        for (int m = 0; m < dTree.Nodes[i].Nodes[j].Nodes[k].Nodes.Count; m++) // Level 4 - Interface
                        {
                            DescriptorNode node = dTree.Nodes[i].Nodes[j].Nodes[k].Nodes[m];
                            if (node.Value.bDescriptorType != USBDescriptorType.ALTERNATE)
                            {
                                oldFormat = true;
                            }
                        }
            //If the tree is in old format, add a node layer
            if (oldFormat)
            {
                for (int i = 0; i < dTree.Nodes.Count; i++) // Level 1
                    for (int j = 0; j < dTree.Nodes[i].Nodes.Count; j++) // Level 2 - Device
                        for (int k = 0; k < dTree.Nodes[i].Nodes[j].Nodes.Count; k++) // Level 3 - Configuration
                        {
                            // Search for interface numbers
                            List<byte> interfaceNums = new List<byte>();
                            for (int m = 0; m < dTree.Nodes[i].Nodes[j].Nodes[k].Nodes.Count; m++)
                                // Level 4 - Interface
                            {
                                DescriptorNode node = dTree.Nodes[i].Nodes[j].Nodes[k].Nodes[m];
                                byte interfaceNum = ((InterfaceDescriptor) node.Value).bInterfaceNumber;
                                if (!interfaceNums.Contains(interfaceNum))
                                    interfaceNums.Add(interfaceNum);
                            }
                            interfaceNums.Sort();

                            // Add interfaces to tree and attach alternate to them
                            for (int m = 0; m < interfaceNums.Count; m++)
                            {
                                DescriptorNode newNode = dTree.AddNode(dTree.Nodes[i].Nodes[j].Nodes[k].Key);
                                newNode.Nodes.Clear();
                                for (int n = 0; n < dTree.Nodes[i].Nodes[j].Nodes[k].Nodes.Count; n++)
                                {
                                    DescriptorNode interfaceNode = dTree.Nodes[i].Nodes[j].Nodes[k].Nodes[n];
                                    if ((interfaceNode.Value.bDescriptorType == USBDescriptorType.INTERFACE) &&
                                        (((InterfaceDescriptor) interfaceNode.Value).bInterfaceNumber ==
                                         interfaceNums[m]))
                                    {
                                        dTree.Nodes[i].Nodes[j].Nodes[k].Nodes.Remove(interfaceNode);
                                        newNode.Nodes.Add(interfaceNode);
                                        n--;
                                    }
                                }
                            }
                        }
            }
        }

        /// <summary>
        /// For compability with the previous versions, checks
        /// if MS OS String Descriptor has the constant value. 
        /// </summary>
        /// <param name="sTree">String Descriptor tree</param>
        private void CheckStringOldFormatCompability(DescriptorTree sTree)
        {
            // Check if MS OS String Descriptor has its default value
            ((StringDescriptor) sTree.Nodes[1].Nodes[1].Value).bString = CyUSBFSParameters.MSOS_STRING;
        }

        public static Int64 ConvertByteArrayToInt(List<byte> byteList)
        {
            Int64 val = 0;
            bool isNegative = false;
            if (byteList.Count > 0)
            {
                if (byteList[byteList.Count - 1] > Math.Pow(2, 7))
                    isNegative = true;
                for (int i = byteList.Count; i < 8; i++)
                {
                    if (!isNegative) byteList.Add(0);
                    else byteList.Add(0xFF);
                }
                val = BitConverter.ToInt64(byteList.ToArray(), 0);
            }
            return val;
        }

        public static List<byte> ConvertIntToByteArray(Int64 val)
        {
            List<byte> byteList = new List<byte>(BitConverter.GetBytes(val));
            //Remove extra bytes
            if (val >= 0)
            {
                while ((byteList[byteList.Count - 1] == 0) && (byteList.Count > 1))
                    byteList.RemoveAt(byteList.Count - 1);
            }
            else
            {
                while ((byteList[byteList.Count - 1] == 0xFF) && (byteList.Count > 1))
                    byteList.RemoveAt(byteList.Count - 1);
            }
            //For numbers such as 128(0x80) (with a significant high byte) add one more byte.
            if (Math.Abs(val) >= Math.Pow(2, (byteList.Count * 8 - 1)))
            {
                if (val >= 0) byteList.Add(0);
                else byteList.Add(0xFF);
            }
            // If value has 3 bytes (without prefix), add 4th byte
            if (byteList.Count == 3 + 1)
            {
                if (val >= 0) byteList.Add(0);
                else byteList.Add(0xFF);
            }
            return byteList;
        }

        #endregion Private functions
    }

    //=================================================================================================================

    [XmlRootAttribute("Descriptor")]
    public class DescriptorNode
    {
        public USBDescriptor Value;
        private string _Key;
        private static uint KeyIndex = 1;
        [XmlArray("Nodes")] public List<DescriptorNode> Nodes;

        [XmlAttribute]
        public string Key
        {
            get { return _Key; }
            set
            {
                _Key = value;
                if ((GetDescriptorIndex(value) >= KeyIndex) && (value != "Serial") && (value != "EE"))
                {
                    KeyIndex = GetDescriptorIndex(value) + 1;
                    if (KeyIndex == 0x80 || (KeyIndex == 0xEE)) KeyIndex++;
                }
            }
        }

        public DescriptorNode()
        {
            Nodes = new List<DescriptorNode>();
            //_Key = "USBDescriptor" + KeyIndex++;
            //if ((KeyIndex == 0x80) || (KeyIndex == 0xEE))
            //    KeyIndex++;
        }

        public DescriptorNode(USBDescriptor value)
        {
            Nodes = new List<DescriptorNode>();
            _Key = "USBDescriptor" + KeyIndex++;
            if ((KeyIndex == 0x80) || (KeyIndex == 0xEE))
                KeyIndex++;
            Value = value;
        }

        public DescriptorNode(string key, USBDescriptor value)
        {
            Nodes = new List<DescriptorNode>();
            _Key = key;
            Value = value;
        }

        public static uint GetDescriptorIndex(string key)
        {
            uint res = 0;
            if (key.IndexOf("USBDescriptor") >= 0)
                res = Convert.ToUInt32(key.Replace("USBDescriptor", ""));
            else if (key.IndexOf("Interface") >= 0)
                res = Convert.ToUInt32(key.Replace("Interface", ""));
            else
            {
                switch (key)
                {
                    case "LANGID":
                        res = 0;
                        break;
                    case "Serial":
                        res = 0x80;
                        break;
                    case "EE":
                        res = 0xEE;
                        break;
                }
            }
            return res;
        }

        public static void SetKeyIndex(uint index)
        {
            KeyIndex = index;
        }

        public static string GetKeyByIndex(uint index)
        {
            return "USBDescriptor" + index;
        }

        public static string GetDescriptorLabel(DescriptorNode descriptor)
        {
            string nodeLabel = "";
            if (descriptor.Value == null)
            {
                switch (descriptor.Key)
                {
                    case "Device":
                        nodeLabel = "Descriptor Root";
                        break;
                    case "String":
                        nodeLabel = "String Descriptors";
                        break;
                    case "SpecialString":
                        nodeLabel = "Special Strings";
                        break;
                    case "HIDReport":
                        nodeLabel = "HID Report Descriptors";
                        break;
                    default:
                        if (descriptor.Key.Contains("Interface"))
                        {
                            nodeLabel = "Interface Descriptor";
                        }
                        else
                        {
                            nodeLabel = "Unknown Descriptor";
                        }
                        break;
                }
            }
            else
            {
                switch (descriptor.Value.bDescriptorType)
                {
                    case USBDescriptorType.DEVICE:
                        nodeLabel = "Device Descriptor";
                        break;
                    case USBDescriptorType.CONFIGURATION:
                        nodeLabel = "Configuration Descriptor";
                        break;
                    case USBDescriptorType.ALTERNATE:
                        nodeLabel = "Interface Descriptor";
                        break;
                    case USBDescriptorType.INTERFACE:
                        int altNum = ((InterfaceDescriptor) descriptor.Value).bAlternateSetting;
                        nodeLabel = "Alternate Setting " + altNum;
                        break;
                    case USBDescriptorType.ENDPOINT:
                        nodeLabel = "Endpoint Descriptor";
                        break;
                    case USBDescriptorType.HID:
                        nodeLabel = "HID Class Descriptor";
                        break;
                    case USBDescriptorType.AUDIO:
                        switch (((AudioDescriptor) descriptor.Value).bAudioDescriptorSubClass)
                        {
                            case (byte) USBOtherTypes.AudioSubclassCodes.AUDIOCONTROL:
                                switch (((AudioDescriptor) descriptor.Value).bAudioDescriptorSubType)
                                {
                                    case (byte) USBOtherTypes.ACInterfaceDescriptorSubtypes.HEADER:
                                        nodeLabel = "Audio Control Descriptor";
                                        break;
                                    default:
                                        nodeLabel = "Audio Control Descriptor";
                                        break;
                                }
                                break;
                            case (byte) USBOtherTypes.AudioSubclassCodes.AUDIOSTREAMING:
                                switch (((AudioDescriptor) descriptor.Value).bAudioDescriptorSubType)
                                {
                                    case (byte) USBOtherTypes.ASInterfaceDescriptorSubtypes.AS_GENERAL:
                                        nodeLabel = "Audio Streaming Interface Descriptor";
                                        break;
                                    case (byte) USBOtherTypes.ASInterfaceDescriptorSubtypes.FORMAT_TYPE:
                                        nodeLabel = "Audio Streaming Format Type Descriptor";
                                        break;
                                    case (byte) USBOtherTypes.ASInterfaceDescriptorSubtypes.FORMAT_SPECIFIC:
                                        nodeLabel = "Audio Streaming Format Specific Descriptor";
                                        break;
                                    default:
                                        nodeLabel = "Audio Streaming Descriptor";
                                        break;
                                }
                                break;
                            case (byte) USBOtherTypes.AudioSubclassCodes.MIDISTREAMING:
                                switch (((AudioDescriptor) descriptor.Value).bAudioDescriptorSubType)
                                {
                                    default:
                                        nodeLabel = "Audio Descriptor";
                                        break;
                                }
                                break;
                            case 0xFF: //CDC
                                nodeLabel = "Functional Descriptor";
                                break;
                            default:
                                nodeLabel = "Audio Descriptor";
                                break;
                        }
                        break;
                    case USBDescriptorType.STRING:
                        if (descriptor.Key == "LANGID")
                            nodeLabel = "LANGID";
                        else if (descriptor.Key == "Serial")
                            nodeLabel = "Serial Number String";
                        else if (descriptor.Key == "EE")
                            nodeLabel = "MS OS String Descriptor";
                        else
                        {
                            nodeLabel = "String             \"" + ((StringDescriptor) descriptor.Value).bString + "\"";
                        }
                        break;
                    case USBDescriptorType.HID_REPORT:
                        nodeLabel = ((HIDReportDescriptor) descriptor.Value).Name;
                        break;
                    case USBDescriptorType.HID_REPORT_ITEM:
                        HIDReportItem item = ((HIDReportItemDescriptor) descriptor.Value).Item;
                        if (item != null)
                        {
                            if (item.Kind == HIDReportItemKind.Custom)
                            {
                                nodeLabel = item.ToCustomString();
                            }
                            else if (CyUSBFSParameters.StringIsComment(item.Name))
                                nodeLabel = item.Name;
                            else
                                nodeLabel = item.ToString();
                        }
                        else
                        {
                            nodeLabel = "Undefined";
                        }
                        break;
                    default:
                        nodeLabel = "Unknown Descriptor";
                        break;
                }
            }
            return nodeLabel;
        }
    }

    //=================================================================================================================

    [XmlRootAttribute("Tree")]
    public class DescriptorTree
    {
        [XmlArray("Tree Descriptors")] public List<DescriptorNode> Nodes;

        public DescriptorTree()
        {
            Nodes = new List<DescriptorNode>();
        }

        public DescriptorNode GetNodeByKey(string key)
        {
            DescriptorNode resultNode = null;
            Queue<DescriptorNode> nodeList = new Queue<DescriptorNode>(Nodes);
            while (nodeList.Count > 0)
            {
                DescriptorNode node = nodeList.Dequeue();
                if (node.Key == key)
                {
                    resultNode = node;
                    break;
                }
                else
                {
                    foreach (DescriptorNode child_node in node.Nodes)
                    {
                        nodeList.Enqueue(child_node);
                    }
                }
            }
            return resultNode;
        }

        public string GetKeyByNode(USBDescriptor node)
        {
            string resultKey = "";
            Queue<DescriptorNode> nodeList = new Queue<DescriptorNode>(Nodes);
            while (nodeList.Count > 0)
            {
                DescriptorNode node_tmp = nodeList.Dequeue();
                if ((node_tmp.Value != null) && (node_tmp.Value.Equals(node)))
                {
                    resultKey = node_tmp.Key;
                    break;
                }
                else
                {
                    foreach (DescriptorNode child_node in node_tmp.Nodes)
                    {
                        nodeList.Enqueue(child_node);
                    }
                }
            }
            return resultKey;
        }

        public DescriptorNode AddNode(string parentKey)
        {
            DescriptorNode parentNode = GetNodeByKey(parentKey);
            DescriptorNode newNode = null;
            if (parentNode != null)
            {
                if (parentNode.Value == null)
                {
                    switch (parentNode.Key)
                    {
                        case "Device":
                            newNode = new DescriptorNode(new DeviceDescriptor());
                            parentNode.Nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case "String":
                            if (parentNode.Nodes.Count == 0)
                            {
                                newNode = new DescriptorNode("LANGID", new StringZeroDescriptor());
                                parentNode.Nodes.Add(newNode);
                            }
                            //if (parentNode.Nodes.Count == 1)
                            //{
                            //    newNode = new DescriptorNode("Serial", new StringDescriptor());
                            //    parentNode.Nodes.Add(newNode);
                            //}
                            //if (parentNode.Nodes.Count == 2)
                            //{
                            //    newNode = new DescriptorNode("EE", new StringDescriptor());
                            //    parentNode.Nodes.Add(newNode);
                            //}
                            newNode = new DescriptorNode(new StringDescriptor());
                            parentNode.Nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case "SpecialString":
                            if (parentNode.Nodes.Count == 0)
                            {
                                newNode = new DescriptorNode("Serial", new StringDescriptor());
                                parentNode.Nodes.Add(newNode);
                            }
                            if (parentNode.Nodes.Count == 1)
                            {
                                newNode = new DescriptorNode("EE", new StringDescriptor());
                                ((StringDescriptor) newNode.Value).bString = CyUSBFSParameters.MSOS_STRING;
                                parentNode.Nodes.Add(newNode);
                            }
                            break;
                        case "HIDReport":
                            //Name of Descriptor
                            int index = 1;
                            while (true)
                            {
                                int i;
                                for (i = 0; i < parentNode.Nodes.Count; i++)
                                {
                                    if (((HIDReportDescriptor) parentNode.Nodes[i].Value).Name ==
                                        "HID Report Descriptor " + index)
                                    {
                                        index++;
                                        break;
                                    }
                                }
                                if (i == parentNode.Nodes.Count) break;
                            }
                            newNode = new DescriptorNode(new HIDReportDescriptor("HID Report Descriptor " + index));
                            parentNode.Nodes.Add(newNode);
                            break;
                        default:

                            break;
                    }
                }
                else
                {
                    switch (parentNode.Value.bDescriptorType)
                    {
                        case USBDescriptorType.DEVICE:
                            newNode = new DescriptorNode(new ConfigDescriptor());
                            parentNode.Nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case USBDescriptorType.CONFIGURATION:
                            uint lastInterfaceKeyIndex = 0;
                            if (parentNode.Nodes.Count > 0)
                            {
                                lastInterfaceKeyIndex =
                                    DescriptorNode.GetDescriptorIndex(parentNode.Nodes[parentNode.Nodes.Count - 1].Key);
                                lastInterfaceKeyIndex++;
                            }
                            newNode = new DescriptorNode(new InterfaceGeneralDescriptor());
                            newNode.Key = newNode.Key.Replace("USBDescriptor", "Interface");
                            parentNode.Nodes.Add(newNode);
                            AddNode(newNode.Key, (byte) (parentNode.Nodes.Count - 1));
                            break;
                        case USBDescriptorType.INTERFACE:
                            newNode = new DescriptorNode(new EndpointDescriptor());
                            parentNode.Nodes.Add(newNode);
                            break;
                        case USBDescriptorType.HID_REPORT:
                            newNode = new DescriptorNode(new HIDReportItemDescriptor());
                            parentNode.Nodes.Add(newNode);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                newNode = new DescriptorNode(parentKey, null);
                Nodes.Add(newNode);
                if ((newNode.Key != "String") && (newNode.Key != "HIDReport"))
                    AddNode(newNode.Key);
            }
            return newNode;
        }

        public void AddNode(string parentKey, USBDescriptorType type)
        {
            DescriptorNode parentNode = GetNodeByKey(parentKey);
            DescriptorNode newNode = null;
            if (parentNode != null)
            {
                switch (parentNode.Value.bDescriptorType)
                {
                    case USBDescriptorType.INTERFACE:
                        if (type == USBDescriptorType.HID)
                        {
                            newNode = new DescriptorNode(new HIDDescriptor());
                            parentNode.Nodes.Add(newNode);
                        }
                        else if (type == USBDescriptorType.AUDIO)
                        {
                            newNode = new DescriptorNode(new AudioDescriptor());
                            if (((InterfaceDescriptor) parentNode.Value).bInterfaceClass ==
                                CyUSBFSParameters.CLASS_AUDIO)
                            {
                                ((AudioDescriptor) newNode.Value).bAudioDescriptorSubClass =
                                    ((InterfaceDescriptor) parentNode.Value).bInterfaceSubClass;
                            }
                            else
                            {
                                ((AudioDescriptor) newNode.Value).bAudioDescriptorSubClass = 0xFF;
                            }
                            parentNode.Nodes.Add(newNode);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                newNode = new DescriptorNode(parentKey, null);
                Nodes.Add(newNode);
                AddNode(newNode.Key);
            }
        }

        public void AddNode(string parentKey, byte index) // For alternate interfaces
        {
            DescriptorNode parentNode = GetNodeByKey(parentKey);
            DescriptorNode newNode = null;
            if ((parentNode != null) && parentNode.Key.Contains("Interface"))
            {
                newNode = new DescriptorNode(new InterfaceDescriptor(index, (byte) parentNode.Nodes.Count));
                parentNode.Nodes.Add(newNode);
                AddNode(newNode.Key);
            }
        }

        public void RemoveNode(string key, string parentKey)
        {
            DescriptorNode node = GetNodeByKey(key);
            DescriptorNode parentNode = GetNodeByKey(parentKey);
            if (node != null)
            {
                if (parentNode != null)
                {
                    parentNode.Nodes.Remove(node);
                }
                else
                {
                    this.Nodes.Remove(node);
                }
            }
        }

        public static string SerializeDescriptors(DescriptorTree tree)
        {
            XmlSerializer s = new XmlSerializer(typeof (DescriptorTree));
            StringWriter sw = new StringWriter();
            s.Serialize(sw, tree);
            string serializedXml = sw.ToString();
            return serializedXml;
        }

        public static DescriptorTree DeserializeDescriptors(string serializedXml)
        {
            XmlSerializer s = new XmlSerializer(typeof (DescriptorTree));
            DescriptorTree tree = (DescriptorTree) s.Deserialize(new StringReader(serializedXml));
            return tree;
        }
    }

    //=================================================================================================================
    /// <summary>
    /// Describes a kind of Settings Panel for the HID Report Item.
    /// </summary>
    public enum HIDReportItemKind
    {
        Bits,
        List,
        Int,
        Unit,
        None,
        Custom
    } ;

    /// <summary>
    /// HID Report Item class
    /// </summary>
    [XmlRootAttribute("HID_ITEM")]
    public class HIDReportItem : ICloneable
    {
        private string _Name;
        public string Description;
        [XmlAttribute("Code")] public byte Prefix;
        [XmlAttribute("Size")] public int Size;
        [XmlArray("Value")] public List<byte> Value;
        [XmlIgnore] public Dictionary<ushort, string> ValuesList;
        private HIDReportItemKind _Kind;

        [XmlAttribute("Type")]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                FillDictionary();
            }
        }

        public HIDReportItemKind Kind
        {
            get { return _Kind; }
            set
            {
                _Kind = value;
                FillDictionary();
            }
        }

        public HIDReportItem()
        {
            ValuesList = new Dictionary<ushort, string>();
            Value = new List<byte>();
        }

        public HIDReportItem(string name, byte prefix, int size, HIDReportItemKind kind)
        {
            _Name = name;
            Description = "";
            Prefix = prefix;
            Size = size;
            _Kind = kind;
            ValuesList = new Dictionary<ushort, string>();
            Value = new List<byte>();
        }

        /// <summary>
        /// Used during importing the report.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefix"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public HIDReportItem(string name, byte prefix, int size, long value, string desc)
        {
            _Name = name;
            Description = "";
            Prefix = prefix;
            Size = size;
            Description = desc;

            ValuesList = new Dictionary<ushort, string>();

            //Kind
            switch (_Name)
            {
                case "COLLECTION":
                case "USAGE":
                case "USAGE_PAGE":
                case "UNIT_EXPONENT":
                case "DELIMITER":
                    _Kind = HIDReportItemKind.List;
                    break;
                case "USAGE_MINIMUM":
                case "USAGE_MAXIMUM":
                case "DESIGNATOR_INDEX":
                case "DESIGNATOR_MINIMUM":
                case "DESIGNATOR_MAXIMUM":
                case "STRING_INDEX":
                case "STRING_MINIMUM":
                case "STRING_MAXIMUM":
                case "LOGICAL_MINIMUM":
                case "LOGICAL_MAXIMUM":
                case "PHYSICAL_MINIMUM":
                case "PHYSICAL_MAXIMUM":
                case "REPORT_SIZE":
                case "REPORT_ID":
                case "REPORT_COUNT":
                    _Kind = HIDReportItemKind.Int;
                    break;
                case "INPUT":
                case "OUTPUT":
                case "FEATURE":
                    _Kind = HIDReportItemKind.Bits;
                    break;
                case "UNIT":
                    _Kind = HIDReportItemKind.Unit;
                    break;
                case "PUSH":
                case "POP":
                case "END_COLLECTION":
                    _Kind = HIDReportItemKind.None;
                    break;
                case "CUSTOM":
                    _Kind = HIDReportItemKind.Custom;
                    break;
                default:
                    _Kind = HIDReportItemKind.None;
                    break;
            }
            FillDictionary();

            Value = new List<byte>();
            Value.Add((byte) (Prefix | (byte) size));
            List<byte> byteList;
            if ((_Kind == HIDReportItemKind.Int) || (_Kind == HIDReportItemKind.Custom))
            {
                byteList = CyUSBFSParameters.ConvertIntToByteArray(value);
            }
            else
            {
                byteList = new List<byte>(BitConverter.GetBytes(value));
                while ((byteList.Count > 1) && (byteList[byteList.Count - 1] == 0))
                    byteList.RemoveAt(byteList.Count - 1);
            }
            Value.AddRange(byteList.ToArray());
        }

        /// <summary>
        /// This string representation is used to display items (except CUSTOM and COMMENT) in the Report tree.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string output = Name + " " + Description + "     ";

            for (int i = 0; i < Value.Count; i++)
            {
                output += Value[i].ToString("X2") + " ";
            }
            return output;
        }

        /// <summary>
        /// This string representation is used to display CUSTOM and COMMENT items in the Report tree.
        /// </summary>
        /// <returns></returns>
        public string ToCustomString()
        {
            string output = "";

            if (Name == "CUSTOM")
            {
                output = "0x" + Value[0].ToString("X2") + " " + Description + "     ";

                for (int i = 0; i < Value.Count; i++)
                {
                    output += Value[i].ToString("X2") + " ";
                }
            }
            else if (Name == "COMMENT")
            {
                output = Name;
            }

            return output;
        }

        /// <summary>
        /// Fills the item's ValuesList depending on its name.
        /// </summary>
        public void FillDictionary()
        {
            if ((Kind == HIDReportItemKind.List) || (Kind == HIDReportItemKind.Bits))
            {
                ValuesList.Clear();
                switch (Name)
                {
                    case "USAGE":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesGenericDesktopPage.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesGenericDesktopPage[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesGenericDesktopPage[i * 2]);
                        }
                        break;
                    case "USAGE_PAGE":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesUsagePage.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToUInt16(CyHIDReportItemTables.ValuesUsagePage[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesUsagePage[i * 2]);
                        }
                        break;
                    case "COLLECTION":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesCollection.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesCollection[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesCollection[i * 2]);
                        }
                        break;
                    case "INPUT":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        break;
                    case "OUTPUT":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        ValuesList[14] = "Non Volatile";
                        ValuesList[15] = "Volatile";
                        break;
                    case "FEATURE":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        ValuesList[14] = "Non Volatile";
                        ValuesList[15] = "Volatile";
                        break;
                    case "UNIT_EXPONENT":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesUnitExponent.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesUnitExponent[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesUnitExponent[i * 2]);
                        }
                        break;
                    case "DELIMITER":
                        for (int i = 0; i < CyHIDReportItemTables.ValuesDelimiter.Length / 2; i++)
                        {
                            ValuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesDelimiter[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesDelimiter[i * 2]);
                        }
                        break;
                }
            }
        }

        #region ICloneable Members

        public object Clone()
        {
            HIDReportItem item = new HIDReportItem(Name, Prefix, Size, Kind);
            item.Description = Description;
            item.ValuesList = new Dictionary<ushort, string>(ValuesList);
            item.Value.AddRange(Value);
            return item;
        }

        #endregion
    }

    //=================================================================================================================

}