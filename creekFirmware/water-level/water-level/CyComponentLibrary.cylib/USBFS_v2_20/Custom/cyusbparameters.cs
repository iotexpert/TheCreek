/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using CyDesigner.Extensions.Common;
using CyDesigner.Extensions.Gde;

namespace USBFS_v2_20
{
    public partial class CyUSBFSParameters
    {
        public enum CyMemoryManagement
        {
            MANUAL,
            DMA_MANUAL_MEMORY,
            DMA_AUTOMATIC_MEMORY
        } ;

        public enum CyMemoryAllocation
        {
            STATIC,
            DYNAMIC
        } ;

        #region Const

        public const byte CLASS_NONE = 0x00;
        public const byte CLASS_AUDIO = 0x01;
        public const byte CLASS_CDC= 0x02;
        public const byte CLASS_HID = 0x03;
        public const byte CLASS_DATA = 0x0A;
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

        public const int SERIAL_STRING_INDEX = 0x80;
        public const int MSOS_STRING_INDEX = 0xEE;

        //public const int MEMORY_ALLOC_STATIC = 0;
        //public const int MEMORY_ALLOC_DYNAMIC = 1;

        public const string TYPE_TREE_NODE = "System.Windows.Forms.TreeNode";
        public const string MSOS_STRING = "MSFT100";

        public const string PARAM_DEVICEDESCRIPTORS = "DeviceDescriptors";
        public const string PARAM_STRINGDESCRIPTORS = "StringDescriptors";
        public const string PARAM_HIDREPORTDESCRIPTORS = "HIDReportDescriptors";
        public const string PARAM_AUDIODESCRIPTORS = "AudioDescriptors";
        public const string PARAM_MIDIDESCRIPTORS = "MidiDescriptors";
        public const string PARAM_CDCDESCRIPTORS = "CDCDescriptors";
        public const string PARAM_EXTERNCLS = "extern_cls";
        public const string PARAM_EXTERNVND = "extern_vnd";
        public const string PARAM_OUTSOF = "out_sof";
        public const string PARAM_MONVBUS = "mon_vbus";
        public const string PARAM_MAXINTERFACES = "max_interfaces_num";
        public const string PARAM_ENABLECDCAPI = "EnableCDCApi";
        public const string PARAM_ENABLEMIDIAPI = "EnableMidiApi";
        public const string PARAM_ENDPOINTMM = "endpointMM";
        public const string PARAM_ENDPOINTMA = "endpointMA";
        public const string PARAM_JACKCOUNT = "extJackCount";
        public const string PARAM_MODE = "Mode";
        public const string PARAM_VID = "Vid";
        public const string PARAM_PID = "Pid";

        public const string NODEKEY_DEVICE = "Device";
        public const string NODEKEY_STRING = "String";
        public const string NODEKEY_AUDIO = "Audio";
        public const string NODEKEY_MIDI = "Midi";
        public const string NODEKEY_CDC = "CDC";
        public const string NODEKEY_SPECIALSTRING = "SpecialString";
        public const string NODEKEY_HIDREPORT = "HIDReport";
        public const string NODEKEY_INTERFACE = "Interface";
        public const string NODEKEY_USBDESCRIPTOR = "USBDescriptor";
        public const string NODEKEY_MSOS_STRING = "EE";
        public const string NODEKEY_STRING_SERIAL = "Serial";
        public const string NODEKEY_STRING_LANGID = "LANGID";

        public const string RPTITEM_USAGE = "USAGE";
        public const string RPTITEM_USAGE_PAGE = "USAGE_PAGE";
        public const string RPTITEM_USAGE_MINIMUM = "USAGE_MINIMUM";
        public const string RPTITEM_USAGE_MAXIMUM = "USAGE_MAXIMUM";
        public const string RPTITEM_DESIGNATOR_INDEX = "DESIGNATOR_INDEX";
        public const string RPTITEM_DESIGNATOR_MINIMUM = "DESIGNATOR_MINIMUM";
        public const string RPTITEM_DESIGNATOR_MAXIMUM = "DESIGNATOR_MAXIMUM";
        public const string RPTITEM_STRING_INDEX = "STRING_INDEX";
        public const string RPTITEM_STRING_MINIMUM = "STRING_MINIMUM";
        public const string RPTITEM_STRING_MAXIMUM = "STRING_MAXIMUM";
        public const string RPTITEM_COLLECTION = "COLLECTION";
        public const string RPTITEM_END_COLLECTION = "END_COLLECTION";
        public const string RPTITEM_INPUT = "INPUT";
        public const string RPTITEM_OUTPUT = "OUTPUT";
        public const string RPTITEM_FEATURE = "FEATURE";
        public const string RPTITEM_LOGICAL_MINIMUM = "LOGICAL_MINIMUM";
        public const string RPTITEM_LOGICAL_MAXIMUM = "LOGICAL_MAXIMUM";
        public const string RPTITEM_PHYSICAL_MINIMUM = "PHYSICAL_MINIMUM";
        public const string RPTITEM_PHYSICAL_MAXIMUM = "PHYSICAL_MAXIMUM";
        public const string RPTITEM_UNIT_EXPONENT = "UNIT_EXPONENT";
        public const string RPTITEM_UNIT = "UNIT";
        public const string RPTITEM_REPORT_SIZE = "REPORT_SIZE";
        public const string RPTITEM_REPORT_ID = "REPORT_ID";
        public const string RPTITEM_REPORT_COUNT = "REPORT_COUNT";
        public const string RPTITEM_PUSH = "PUSH";
        public const string RPTITEM_POP = "POP";
        public const string RPTITEM_DELIMITER = "DELIMITER";
        public const string RPTITEM_CUSTOM = "CUSTOM";
        public const string RPTITEM_COMMENT = "COMMENT";

        public const string MSG_TITLE_ERROR = "Error";
        public const string MSG_TITLE_WARNING = "Warning";
        public const string MSG_TITLE_INFORMATION = "Information";
        public const string MSG_TITLE_QUESTION = "Question";

        #endregion Const

        #region Fields

        public readonly ICyInstEdit_v1 m_inst;

        #region Lang

        public static readonly UInt16[] LANG_ID_TABLE = {
                                             0x0436, 0x041c, 0x0401, 0x0801, 0x0c01, 0x1001, 0x1401, 0x1801,
                                             0x1c01, 0x2001, 0x2401, 0x2801, 0x2c01, 0x3001, 0x3401, 0x3801,
                                             0x3c01, 0x4001, 0x042b, 0x044d, 0x042c, 0x082c, 0x042d, 0x0423,
                                             0x0445, 0x0402, 0x0455, 0x0403, 0x0404, 0x0804, 0x0c04, 0x1004,
                                             0x1404, 0x041a, 0x0405, 0x0406, 0x0413, 0x0813, 0x0409, 0x0809,
                                             0x0c09, 0x1009, 0x1409, 0x1809, 0x1c09, 0x2009, 0x2409, 0x2809,
                                             0x2c09, 0x3009, 0x3409, 0x0425, 0x0438, 0x0429, 0x040b, 0x040c,
                                             0x080c, 0x0c0c, 0x100c, 0x140c, 0x180c, 0x0437, 0x0407, 0x0807,
                                             0x0c07, 0x1007, 0x1407, 0x0408, 0x0447, 0x040d, 0x0439, 0x040e,
                                             0x040f, 0x0421, 0x0410, 0x0810, 0x0411, 0x044b, 0x0860, 0x043f,
                                             0x0457, 0x0412, 0x0812, 0x0426, 0x0427, 0x0827, 0x042f, 0x043e,
                                             0x083e, 0x044c, 0x0458, 0x044e, 0x0861, 0x0414, 0x0814, 0x0448,
                                             0x0415, 0x0416, 0x0816, 0x0446, 0x0418, 0x0419, 0x044f, 0x0c1a,
                                             0x081a, 0x0459, 0x041b, 0x0424, 0x040a, 0x080a, 0x0c0a, 0x100a,
                                             0x140a, 0x180a, 0x1c0a, 0x200a, 0x240a, 0x280a, 0x2c0a, 0x300a,
                                             0x340a, 0x380a, 0x3c0a, 0x400a, 0x440a, 0x480a, 0x4c0a, 0x500a,
                                             0x0430, 0x0441, 0x041d, 0x081d, 0x0449, 0x0444, 0x044a, 0x041e,
                                             0x041f, 0x0422, 0x0420, 0x0820, 0x0443, 0x0843, 0x042a, 0x04ff
                                         };

        public static readonly string[] LangIDNames = {
                                                 "Afrikaans",               "Albanian",
                                                 "Arabic(Saudi Arabia)",    "Arabic(Iraq)",
                                                 "Arabic(Egypt)",           "Arabic(Libya)",
                                                 "Arabic(Algeria)",         "Arabic(Morocco)",
                                                 "Arabic(Tunisia)",         "Arabic(Oman)",
                                                 "Arabic(Yemen)",           "Arabic(Syria)",
                                                 "Arabic(Jordan)",          "Arabic(Lebanon)",
                                                 "Arabic(Kuwait)",          "Arabic(U.A.E.)",
                                                 "Arabic(Bahrain)",         "Arabic(Qatar)",
                                                 "Armenian.",               "Assamese.",
                                                 "Azeri(Latin)",            "Azeri(Cyrillic)",
                                                 "Basque",                  "Belarussian",
                                                 "Bengali.",                "Bulgarian",
                                                 "Burmese",                 "Catalan",
                                                 "Chinese(Taiwan)",         "Chinese(PRC)",
                                                 "Chinese(Hong Kong SAR, PRC)", "Chinese(Singapore)",
                                                 "Chinese(Macau SAR)",      "Croatian",
                                                 "Czech",                   "Danish",
                                                 "Dutch(Netherlands)",      "Dutch(Belgium)",
                                                 "English(United States)",  "English(United Kingdom)",
                                                 "English(Australian)",     "English(Canadian)",
                                                 "English(New Zealand)",    "English(Ireland)",
                                                 "English(South Africa)",   "English(Jamaica)",
                                                 "English(Caribbean)",      "English(Belize)",
                                                 "English(Trinidad)",       "English(Zimbabwe)",
                                                 "English(Philippines)",    "Estonian",
                                                 "Faeroese",                "Farsi",
                                                 "Finnish",                 "French(Standard)",
                                                 "French(Belgian)",         "French(Canadian)",
                                                 "French(Switzerland)",     "French(Luxembourg)",
                                                 "French(Monaco)",          "Georgian",
                                                 "German(Standard)",        "German(Switzerland)",
                                                 "German(Austria)",         "German(Luxembourg)",
                                                 "German(Liechtenstein)",   "Greek",
                                                 "Gujarati.",               "Hebrew",
                                                 "Hindi.",                  "Hungarian",
                                                 "Icelandic",               "Indonesian",
                                                 "Italian(Standard)",       "Italian(Switzerland)",
                                                 "Japanese",                "Kannada.",
                                                 "Kashmiri(India)",         "Kazakh",
                                                 "Konkani.",                "Korean",
                                                 "Korean(Johab)",           "Latvian",
                                                 "Lithuanian",              "Lithuanian(Classic)",
                                                 "Macedonian",              "Malay(Malaysian)",
                                                 "Malay(Brunei Darussalam)", "Malayalam.",
                                                 "Manipuri",                "Marathi.",
                                                 "Nepali(India).",          "Norwegian(Bokmal)",
                                                 "Norwegian(Nynorsk)",      "Oriya.",
                                                 "Polish",                  "Portuguese(Brazil)",
                                                 "Portuguese(Standard)",    "Punjabi.",
                                                 "Romanian",                "Russian",
                                                 "Sanskrit.",               "Serbian(Cyrillic)",
                                                 "Serbian(Latin)",          "Sindhi",
                                                 "Slovak",                  "Slovenian",
                                                 "Spanish(Traditional Sort)", "Spanish(Mexican)",
                                                 "Spanish(Modern Sort)",    "Spanish(Guatemala)",
                                                 "Spanish(Costa Rica)",     "Spanish(Panama)",
                                                 "Spanish(Dominican Republic)", "Spanish(Venezuela)", 
                                                 "Spanish(Colombia)",       "Spanish(Peru)",
                                                 "Spanish(Argentina)",      "Spanish(Ecuador)",
                                                 "Spanish(Chile)",          "Spanish(Uruguay)",
                                                 "Spanish(Paraguay)",       "Spanish(Bolivia)",
                                                 "Spanish(El Salvador)",    "Spanish(Honduras)",
                                                 "Spanish(Nicaragua)",      "Spanish(Puerto Rico)",
                                                 "Sutu",                    "Swahili(Kenya)",
                                                 "Swedish",                 "Swedish(Finland)",
                                                 "Tamil",                   "Tatar(Tatarstan)",
                                                 "Telugu.",                 "Thai",
                                                 "Turkish",                 "Ukrainian",
                                                 "Urdu(Pakistan)",          "Urdu(India)",
                                                 "Uzbek(Latin)",            "Uzbek(Cyrillic)",
                                                 "Vietnamese",              "HID(Usage Data Descriptor)"
                                             };

        public static readonly string[] CountryCodes = {
                                                  "Not Supported",      "Arabic",
                                                  "Belgian",            "Canadian-Bilingual",
                                                  "Canadian-French",    "Czech Republic",
                                                  "Danish",             "Finnish",
                                                  "French",             "German",
                                                  "Greek",              "Hebrew",
                                                  "Hungary",            "International (ISO)",
                                                  "Italian",            "Japan (Katakana)",
                                                  "Korean",             "Latin American",
                                                  "Netherlands/Dutch",  "Norwegian",
                                                  "Persian (Farsi)",    "Poland",
                                                  "Portuguese",         "Russia",
                                                  "Slovakia",           "Spanish",
                                                  "Swedish",            "Swiss/French",
                                                  "Swiss/German",       "Switzerland",
                                                  "Taiwan",             "Turkish-Q",
                                                  "UK",                 "US",
                                                  "Yugoslavia",         "Turkish-F"
                                              };

        #endregion Lang

        public List<string> m_emptyFields; // Used to store information about fields that user should fill 
                                           //before closing the customizer

        public CyDescriptorTree m_deviceTree;
        public CyDescriptorTree m_stringTree;
        public CyDescriptorTree m_hidReportTree;
        public CyDescriptorTree m_audioTree;
        public CyDescriptorTree m_midiTree;
        public CyDescriptorTree m_cdcTree;

        public string m_serializedDeviceDesc;
        public string m_serializedStringDesc;
        public string m_serializedHIDReportDesc;
        public string m_serializedAudioDesc;
        public string m_serializedMidiDesc;
        public string m_serializedCDCDesc;

        // Xml serialization parameters
        public XmlSerializer m_serializer;
        public XmlSerializerNamespaces m_customSerNamespace;

        private bool[] m_usedEp = new bool[9];
        private byte m_max_interfaces;
        private bool m_enableCDCApi;
        private bool m_enableMIDIApi;
        private byte m_extJackCount;
        private bool m_mode;
        private string m_vid;
        private string m_pid;

        // Endpoint memory management parameters
        private byte m_memoryMgmt = 0;
        private byte m_memoryAlloc = 0;

        // Advanced Tab parameters
        private bool m_mon_vbus;
        private bool m_extern_cls;
        private bool m_extern_vnd;
        private bool m_out_sof;

        public bool ParamDeviceTreeChanged
        {
            get { return false; }
            set
            {
                if (m_deviceTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_deviceTree.m_nodes[0]);
                SerializedDeviceDesc = CyDescriptorTree.SerializeDescriptors(m_deviceTree, m_serializer,
                                                                             m_customSerNamespace); 
            }
        }

        public bool ParamStringTreeChanged
        {
            get { return false; }
            set
            {
                if (m_deviceTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_deviceTree.m_nodes[0]);
                CyStringDescConverter.m_strDescList = GetStringsList();
                SerializedStringDesc = CyDescriptorTree.SerializeDescriptors(m_stringTree, m_serializer,
                                                                           m_customSerNamespace);
            }
        }

        public bool ParamHIDReportTreeChanged
        {
            get { return false; }
            set
            {
                if (m_hidReportTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_hidReportTree.m_nodes[0]);
                SerializedHIDReportDesc = CyDescriptorTree.SerializeDescriptors(m_hidReportTree, m_serializer,
                                                                           m_customSerNamespace);
            }
        }

        public bool ParamAudioTreeChanged
        {
            get { return false; }
            set
            {
                if (m_audioTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_audioTree.m_nodes[0]);
                SerializedAudioDesc = CyDescriptorTree.SerializeDescriptors(m_audioTree, m_serializer,
                                                                           m_customSerNamespace);
                ParamDeviceTreeChanged = true;
            }
        }

        public bool ParamMidiTreeChanged
        {
            get { return false; }
            set
            {
                if (m_midiTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_midiTree.m_nodes[0]);
                SerializedMidiDesc = CyDescriptorTree.SerializeDescriptors(m_midiTree, m_serializer,
                                                                           m_customSerNamespace);
                ParamDeviceTreeChanged = true;
            }
        }

        public bool ParamCDCTreeChanged
        {
            get { return false; }
            set
            {
                if (m_cdcTree.m_nodes.Count > 0)
                    RecalcDescriptors(m_cdcTree.m_nodes[0]);
                SerializedCDCDesc = CyDescriptorTree.SerializeDescriptors(m_cdcTree, m_serializer,
                                                                           m_customSerNamespace);
                ParamDeviceTreeChanged = true;
            }
        }

        public string SerializedDeviceDesc
        {
            get { return m_serializedDeviceDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedDeviceDesc)
                {
                    m_serializedDeviceDesc = value;
                    m_serializedDeviceDesc = m_serializedDeviceDesc.Replace("\r\n", " ");
                    SetParam(PARAM_DEVICEDESCRIPTORS);
                }
            }
        }

        public string SerializedStringDesc
        {
            get { return m_serializedStringDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedStringDesc)
                {
                    m_serializedStringDesc = value;
                    m_serializedStringDesc = m_serializedStringDesc.Replace("\r\n", " ");
                    SetParam(PARAM_STRINGDESCRIPTORS);
                }
            }
        }

        public string SerializedHIDReportDesc
        {
            get { return m_serializedHIDReportDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedHIDReportDesc)
                {
                    m_serializedHIDReportDesc = value;
                    m_serializedHIDReportDesc = m_serializedHIDReportDesc.Replace("\r\n", " ");
                    SetParam(PARAM_HIDREPORTDESCRIPTORS);
                }
            }
        }

        public string SerializedAudioDesc
        {
            get { return m_serializedAudioDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedAudioDesc)
                {
                    m_serializedAudioDesc = value;
                    m_serializedAudioDesc = m_serializedAudioDesc.Replace("\r\n", " ");
                    SetParam(PARAM_AUDIODESCRIPTORS);
                }
            }
        }

        public string SerializedMidiDesc
        {
            get { return m_serializedMidiDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedMidiDesc)
                {
                    m_serializedMidiDesc = value;
                    m_serializedMidiDesc = m_serializedMidiDesc.Replace("\r\n", " ");
                    SetParam(PARAM_MIDIDESCRIPTORS);
                }
            }
        }

        public string SerializedCDCDesc
        {
            get { return m_serializedCDCDesc; }
            set
            {
                if (value.Replace("\r\n", " ") != m_serializedCDCDesc)
                {
                    m_serializedCDCDesc = value;
                    m_serializedCDCDesc = m_serializedCDCDesc.Replace("\r\n", " ");
                    SetParam(PARAM_CDCDESCRIPTORS);
                }
            }
        }

        public byte MaxInterfaces
        {
            get { return m_max_interfaces; }
            set
            {
                if (value != m_max_interfaces)
                {
                    m_max_interfaces = value;
                    SetParam(PARAM_MAXINTERFACES);
                }
            }
        }

        public byte ExtJackCount
        {
            get { return m_extJackCount; }
            set
            {
                if (value != m_extJackCount)
                {
                    m_extJackCount = value;
                    SetParam(PARAM_JACKCOUNT);
                }
            }
        }

        public bool Mode
        {
            get { return m_mode; }
            set
            {
                if (value != m_mode)
                {
                    m_mode = value;
                    SetParam(PARAM_MODE);
                }
            }
        }

        public bool EnableCDCApi
        {
            get { return m_enableCDCApi; }
            set
            {
                if (value != m_enableCDCApi)
                {
                    m_enableCDCApi = value;
                    SetParam(PARAM_ENABLECDCAPI);
                }
            }
        }

        public bool EnableMIDIApi
        {
            get { return m_enableMIDIApi; }
            set
            {
                if (value != m_enableMIDIApi)
                {
                    m_enableMIDIApi = value;
                    SetParam(PARAM_ENABLEMIDIAPI);
                }
            }
        }

        public bool Extern_cls
        {
            get { return m_extern_cls; }
            set
            {
                if (value != m_extern_cls)
                {
                    m_extern_cls = value;
                    SetParam(PARAM_EXTERNCLS);
                }
            }
        }

        public bool Extern_vnd
        {
            get { return m_extern_vnd; }
            set
            {
                if (value != m_extern_vnd)
                {
                    m_extern_vnd = value;
                    SetParam(PARAM_EXTERNVND);
                }
            }
        }

        public bool Out_sof
        {
            get { return m_out_sof; }
            set 
            {
                if (value != m_out_sof)
                {
                    m_out_sof = value;
                    SetParam(PARAM_OUTSOF);
                }
            }
        }

        public bool Mon_vbus
        {
            get { return m_mon_vbus; }
            set 
            {
                if (value != m_mon_vbus)
                {
                    m_mon_vbus = value;
                    SetParam(PARAM_MONVBUS);
                }
            }
        }

        public byte EPMemoryMgmt
        {
            get { return m_memoryMgmt; }
            set
            {
                if (value != m_memoryMgmt)
                {
                    m_memoryMgmt = value;
                    SetParam(PARAM_ENDPOINTMM);
                }
            }
        }

        public byte EPMemoryAlloc
        {
            get { return m_memoryAlloc; }
            set
            {
                if (value != m_memoryAlloc)
                {
                    m_memoryAlloc = value;
                    SetParam(PARAM_ENDPOINTMA);
                }
            }
        }

        public string Vid
        {
            get { return m_vid; }
            set
            {
                if (value != m_vid)
                {
                    m_vid = value;
                    SetParam(PARAM_VID);
                }
            }
        }

        public string Pid
        {
            get { return m_pid; }
            set
            {
                if (value != m_pid)
                {
                    m_pid = value;
                    SetParam(PARAM_PID);
                }
            }
        }

        #endregion Fields

        #region Constructors

        public CyUSBFSParameters()
        {
            // Create XML Serializer
            m_serializer = new XmlSerializer(typeof(CyDescriptorTree));
            m_customSerNamespace = new XmlSerializerNamespaces();
            Type classType = typeof(CyUSBFSParameters);
            string curNamespace = classType.Namespace;
            string version = curNamespace.Substring(curNamespace.LastIndexOf("_v") + 2);
            m_customSerNamespace.Add("CustomizerVersion", version);

            m_deviceTree = new CyDescriptorTree();
            m_stringTree = new CyDescriptorTree();
            m_hidReportTree = new CyDescriptorTree();
            m_audioTree = new CyDescriptorTree();
            m_midiTree = new CyDescriptorTree();
            m_cdcTree = new CyDescriptorTree();
            m_emptyFields = new List<string>();
        }

        public CyUSBFSParameters(ICyInstEdit_v1 inst) : this()
        {
            m_inst = inst;
            GetParams(m_inst);
        }

        #endregion Constructors

        #region Common

        public void GetParams(ICyInstQuery_v1 inst)
        {
            if (inst != null)
            {
                m_serializedDeviceDesc = inst.GetCommittedParam(PARAM_DEVICEDESCRIPTORS).Value;
                m_serializedStringDesc = inst.GetCommittedParam(PARAM_STRINGDESCRIPTORS).Value;
                m_serializedHIDReportDesc = inst.GetCommittedParam(PARAM_HIDREPORTDESCRIPTORS).Value;
                m_serializedAudioDesc = inst.GetCommittedParam(PARAM_AUDIODESCRIPTORS).Value;
                m_serializedMidiDesc = inst.GetCommittedParam(PARAM_MIDIDESCRIPTORS).Value;
                m_serializedCDCDesc = inst.GetCommittedParam(PARAM_CDCDESCRIPTORS).Value;
                GetAdvancedParams(inst);

                DeserializeDescriptors();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void GetAdvancedParams(ICyInstQuery_v1 inst)
        {
            if (inst != null)
            {
                inst.GetCommittedParam(PARAM_MONVBUS).TryGetValueAs(out m_mon_vbus);
                inst.GetCommittedParam(PARAM_EXTERNCLS).TryGetValueAs(out m_extern_cls);
                inst.GetCommittedParam(PARAM_EXTERNVND).TryGetValueAs(out m_extern_vnd);
                inst.GetCommittedParam(PARAM_OUTSOF).TryGetValueAs(out m_out_sof);

                inst.GetCommittedParam(PARAM_ENABLECDCAPI).TryGetValueAs(out m_enableCDCApi);
                inst.GetCommittedParam(PARAM_ENABLEMIDIAPI).TryGetValueAs(out m_enableMIDIApi);
                inst.GetCommittedParam(PARAM_MODE).TryGetValueAs(out m_mode);
                
                int tmp;
                inst.GetCommittedParam(PARAM_ENDPOINTMM).TryGetValueAs<int>(out tmp);
                m_memoryMgmt = (byte)tmp;

                inst.GetCommittedParam(PARAM_ENDPOINTMA).TryGetValueAs<int>(out tmp);
                m_memoryAlloc = (byte)tmp;
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void SetParam(string paramName)
        {
            if (m_inst != null)
            {
                switch (paramName)
                {
                    case PARAM_DEVICEDESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_DEVICEDESCRIPTORS, SerializedDeviceDesc);
                        break;
                    case PARAM_STRINGDESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_STRINGDESCRIPTORS, SerializedStringDesc);
                        break;
                    case PARAM_HIDREPORTDESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_HIDREPORTDESCRIPTORS, SerializedHIDReportDesc);
                        break;
                    case PARAM_AUDIODESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_AUDIODESCRIPTORS, SerializedAudioDesc);
                        break;
                    case PARAM_MIDIDESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_MIDIDESCRIPTORS, SerializedMidiDesc);
                        break;
                    case PARAM_CDCDESCRIPTORS:
                        m_inst.SetParamExpr(PARAM_CDCDESCRIPTORS, SerializedCDCDesc);
                        break;
                    case PARAM_MONVBUS:
                        m_inst.SetParamExpr(PARAM_MONVBUS, Mon_vbus.ToString().ToLower());
                        break;
                    case PARAM_EXTERNCLS:
                        m_inst.SetParamExpr(PARAM_EXTERNCLS, Extern_cls.ToString().ToLower());
                        break;
                    case PARAM_EXTERNVND:
                        m_inst.SetParamExpr(PARAM_EXTERNVND, Extern_vnd.ToString().ToLower());
                        break;
                    case PARAM_OUTSOF:
                        m_inst.SetParamExpr(PARAM_OUTSOF, Out_sof.ToString().ToLower());
                        break;
                    case PARAM_MAXINTERFACES:
                        m_inst.SetParamExpr(PARAM_MAXINTERFACES, m_max_interfaces.ToString());
                        break;
                    case PARAM_ENABLECDCAPI:
                        m_inst.SetParamExpr(PARAM_ENABLECDCAPI, m_enableCDCApi.ToString());
                        break;
                    case PARAM_ENABLEMIDIAPI:
                        m_inst.SetParamExpr(PARAM_ENABLEMIDIAPI, m_enableMIDIApi.ToString());
                        break;
                    case PARAM_ENDPOINTMM:
                        m_inst.SetParamExpr(PARAM_ENDPOINTMM, m_memoryMgmt.ToString());
                        break;
                    case PARAM_ENDPOINTMA:
                        m_inst.SetParamExpr(PARAM_ENDPOINTMA, m_memoryAlloc.ToString());
                        break;
                    case PARAM_MODE:
                        m_inst.SetParamExpr(PARAM_MODE, m_mode.ToString().ToLower());
                        SetParam(PARAM_JACKCOUNT);
                        break;
                    case PARAM_VID:
                        m_inst.SetParamExpr(PARAM_VID, m_vid);
                        break;
                    case PARAM_PID:
                        m_inst.SetParamExpr(PARAM_PID, m_pid);
                        break;
                    case PARAM_JACKCOUNT:
                        if (!m_mode)
                            m_inst.SetParamExpr(PARAM_JACKCOUNT,"0");
                        else
                            m_inst.SetParamExpr(PARAM_JACKCOUNT, m_extJackCount.ToString());
                        break;
                    default:
                        break;
                }
                CommitParams();
            }
            else
            {
                // Do nothing, internal calculations
            }
        }

        public void SetParam_rm_ep_isr()
        {
            if (m_inst != null)
            {
                bool alwaysTrue = true; // for rm_dma params
                
                    if (EPMemoryMgmt != (byte)CyMemoryManagement.MANUAL)
                        alwaysTrue = false;
                
                for (int i = 1; i < m_usedEp.Length; i++)
                {
                    string param_rm_ep_isr = "rm_ep_isr_" + i;
                    string param_rm_dma = "rm_dma_" + i;
                    if (m_usedEp[i] == true)
                    {
                        m_inst.SetParamExpr(param_rm_ep_isr, false.ToString());
                        m_inst.SetParamExpr(param_rm_dma, alwaysTrue.ToString()); // if alwaysTrue=false then False
                    }
                    else
                    {
                        m_inst.SetParamExpr(param_rm_ep_isr, true.ToString());
                        m_inst.SetParamExpr(param_rm_dma, true.ToString());
                    }
                }
                CommitParams();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        public void SetCdcVidPid(CyDescriptorNode node, string vid, string pid)
        {
            if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE))
            {
                CyInterfaceDescriptor interfaceDesc = (CyInterfaceDescriptor)node.m_value;
                if (interfaceDesc.bInterfaceClass == CyUSBFSParameters.CLASS_CDC)
                {
                    Vid = vid;
                    Pid = pid;
                }
            }
            else
            {
                if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.DEVICE))
                {
                    CyDeviceDescriptor deviceDesc = (CyDeviceDescriptor)node.m_value;
                    vid = deviceDesc.idVendor.ToString("X4");
                    pid = deviceDesc.idProduct.ToString("X4");
                }

                for (int i = node.m_nodes.Count-1; i >= 0; i--)
                {
                    SetCdcVidPid(node.m_nodes[i], vid, pid);
                }
            }
        }

        public void CommitParams()
        {
            if (m_inst != null)
            {
                if (!m_inst.CommitParamExprs())
                    ShowAllErrors();
            }
            else
            {
                // This method is never called when m_inst is not specified
                Debug.Assert(false);
            }
        }

        private void ShowAllErrors()
        {
            foreach (string paramName in m_inst.GetParamNames())
            {
                CyCompDevParam curParam = m_inst.GetCommittedParam(paramName);
                if (curParam.ErrorCount > 0) ShowParamErrors(curParam);
            }
        }

        private void ShowParamErrors(CyCompDevParam param)
        {
            MessageBox.Show(
                param.ErrorMsgs,
                param.Name + " CommitParams errors",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public void DeserializeDescriptors()
        {
            // Deserialize Device Tree
            if (!string.IsNullOrEmpty(m_serializedDeviceDesc))
            {
                try
                {
                    m_deviceTree = CyDescriptorTree.DeserializeDescriptors(m_serializedDeviceDesc, m_serializer);
                    CheckOldFormatCompability(m_deviceTree);
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_DEVICEDESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(true, false, false, false, false, false);
                }
            }
            else
            {
                InitDescriptors(true, false, false, false, false, false);
            }
            // Deserialize String Tree
            if (!string.IsNullOrEmpty(m_serializedStringDesc))
            {
                try
                {
                    m_stringTree = CyDescriptorTree.DeserializeDescriptors(m_serializedStringDesc, m_serializer);
                    CheckStringOldFormatCompability(m_stringTree);
                    CyStringDescConverter.m_strDescList = GetStringsList();
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_STRINGDESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(false, true, false, false, false, false);
                }
            }
            else
            {
                InitDescriptors(false, true, false, false, false, false);
            }
            // Deserialize HID Report Tree
            if (!string.IsNullOrEmpty(m_serializedHIDReportDesc))
            {
                try
                {
                    m_hidReportTree = CyDescriptorTree.DeserializeDescriptors(m_serializedHIDReportDesc, m_serializer);
                    RestoreHIDReportUsageList();
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_HIDREPORTDESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(false, false, true, false, false, false);
                }
            }
            else
            {
                InitDescriptors(false, false, true, false, false, false);
            }
            // Deserialize Audio Tree
            if (!string.IsNullOrEmpty(m_serializedAudioDesc))
            {
                try
                {
                    m_audioTree = CyDescriptorTree.DeserializeDescriptors(m_serializedAudioDesc, m_serializer);
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_AUDIODESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(false, false, false, true, false, false);
                }
            }
            else
            {
                InitDescriptors(false, false, false, true, false, false);
            }

            // Deserialize Midi Tree
            if (!string.IsNullOrEmpty(m_serializedMidiDesc))
            {
                try
                {
                    m_midiTree = CyDescriptorTree.DeserializeDescriptors(m_serializedMidiDesc, m_serializer);
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_MIDIDESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(false, false, false, false, false, true);
                }
            }
            else
            {
                InitDescriptors(false, false, false, false, false, true);
            }

            if (!string.IsNullOrEmpty(m_serializedCDCDesc))
            {
                try
                {
                    m_cdcTree = CyDescriptorTree.DeserializeDescriptors(m_serializedCDCDesc, m_serializer);
                }
                catch
                {
                    MessageBox.Show(String.Format(Properties.Resources.MSG_WRONG_PARAMETER, PARAM_CDCDESCRIPTORS),
                                    MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    InitDescriptors(false, false, false, false, true, false);
                }
            }
            else
            {
                InitDescriptors(false, false, false, false, true, false);
            }

            // Check if Device tree has audio, midi or cdc interfaces and if so, merge them with audio or cdc interfaces
            // to make one object
            for (int i = 0; i < m_deviceTree.m_nodes[0].m_nodes.Count; i++) //device level
                for (int j = 0; j < m_deviceTree.m_nodes[0].m_nodes[i].m_nodes.Count; j++) // configuration level
                    // alternate level
                    for (int k = 0; k < m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes.Count; k++)
                        // interface level
                        for (int m = 0; m < Math.Min(1, m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].
                                                                            m_nodes[k].m_nodes.Count); m++)
                        {
                            // audio / midi
                            if (m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[m].m_value is
                                CyAudioInterfaceDescriptor)
                            {
                                // audio tree
                                for (int l = 0; l < m_audioTree.m_nodes[0].m_nodes.Count; l++)
                                {
                                    // If node keys are equal, merge interface nodes
                                    if (m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].Key ==
                                       m_audioTree.m_nodes[0].m_nodes[l].Key)
                                    {
                                        m_audioTree.m_nodes[0].m_nodes.RemoveAt(l);
                                        m_audioTree.m_nodes[0].m_nodes.Insert(l,
                                                                              m_deviceTree.m_nodes[0].m_nodes[i].
                                                                                  m_nodes[j].m_nodes[k]);
                                        break;
                                    }
                                }

                                // midi tree
                                for (int l = 0; l < m_midiTree.m_nodes[0].m_nodes.Count; l++)
                                {
                                    // If node keys are equal, merge interface nodes
                                    if (m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].Key ==
                                       m_midiTree.m_nodes[0].m_nodes[l].Key)
                                    {
                                        m_midiTree.m_nodes[0].m_nodes.RemoveAt(l);
                                        m_midiTree.m_nodes[0].m_nodes.Insert(l,
                                                                              m_deviceTree.m_nodes[0].m_nodes[i].
                                                                                  m_nodes[j].m_nodes[k]);
                                        break;
                                    }
                                }
                            }

                            // cdc
                            else if (m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[m].m_value is
                                     CyCDCInterfaceDescriptor)
                            {
                                for (int l = 0; l < m_cdcTree.m_nodes[0].m_nodes.Count; l++)
                                {
                                    // If node keys are equal, merge interface nodes
                                    if (m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].Key ==
                                       m_cdcTree.m_nodes[0].m_nodes[l].Key)
                                    {
                                        m_cdcTree.m_nodes[0].m_nodes.RemoveAt(l);
                                        m_cdcTree.m_nodes[0].m_nodes.Insert(l,
                                                                              m_deviceTree.m_nodes[0].m_nodes[i].
                                                                                  m_nodes[j].m_nodes[k]);
                                        break;
                                    }
                                }
                            }
                        }

            // Reassing Endpoint memory allocation value for compatibility with previous versions
            if (m_deviceTree.m_nodes[0].m_nodes.Count > 0)
            {
                CyDeviceDescriptor node = (CyDeviceDescriptor)m_deviceTree.m_nodes[0].m_nodes[0].m_value;
                if (node.bMemoryMgmt != 0)
                {
                    node.bMemoryMgmt = 0;
                }
                if (node.bMemoryAlloc != 0)
                {
                    EPMemoryAlloc = node.bMemoryAlloc;
                    node.bMemoryAlloc = 0;
                }
            }
        }

        #endregion Common

        #region Manipulations with String descriptors

        /// <summary>
        /// Finds the index of the string that was selected in the descriptor.
        /// </summary>
        /// <param name="comboBox">Combobox where user selects the string descriptor</param>
        /// <param name="parameters">Reference to the Parameters</param>
        /// <returns>Unique index of the string</returns>
        public static uint SaveStringDescriptor(ComboBox comboBox, CyUSBFSParameters parameters)
        {
            CyStringDescriptor strDesc = null;
            uint strIndex = 0;

            if (comboBox.Text == "") return strIndex;

            if ((comboBox.SelectedIndex < 0) && (comboBox.Text != ""))
            {
                //Search for string in pre-defined array
                bool predefined = false;
                List<CyStringDescriptor> strList = parameters.GetStringDescList();
                for (int i = 0; i < strList.Count; i++)
                {
                    if (comboBox.Text == strList[i].ToString())
                    {
                        predefined = true;
                        strDesc = strList[i];
                        break;
                    }
                }
                if (!predefined)
                {
                    strDesc = CreateNewStringDescriptor(comboBox.Text, parameters);
                    parameters.ParamStringTreeChanged = true;
                }
            }
            else
            {
                strDesc = (CyStringDescriptor)comboBox.Items[comboBox.SelectedIndex];
            }

            //General USBDescriptor-based index
            strIndex = GetStringDescriptorIndex(strDesc, parameters);

            return strIndex;
        }

        public static CyStringDescriptor CreateNewStringDescriptor(string strValue, CyUSBFSParameters parameters)
        {
            CyDescriptorNode newNode = parameters.m_stringTree.AddNode(NODEKEY_STRING);
            ((CyStringDescriptor)newNode.m_value).bString = strValue;
            return (CyStringDescriptor)newNode.m_value;
        }

        /// <summary>
        /// This function returns a general USBDescriptor-based index of the String Descriptor.
        /// </summary>
        /// <param name="strDesc"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static uint GetStringDescriptorIndex(CyStringDescriptor strDesc, CyUSBFSParameters parameters)
        {
            uint strIndex = 0;
            string strConfigKey = parameters.m_stringTree.GetKeyByNode(strDesc);
            if (strConfigKey != "")
            {
                strIndex = CyDescriptorNode.GetDescriptorIndex(strConfigKey);
            }
            return strIndex;
        }

        /// <summary>
        /// Gets the Serial String descriptor node from the m_stringTree
        /// </summary>
        /// <returns></returns>
        public CyStringDescriptor GetSerialDescriptor()
        {
            CyStringDescriptor res = null;
            if ((m_stringTree != null) &&
                (m_stringTree.m_nodes.Count > 1) &&
                (m_stringTree.m_nodes[1].m_nodes.Count > 0))
            {
                res = (CyStringDescriptor)m_stringTree.m_nodes[1].m_nodes[0].m_value;
            }
            return res;
        }

        public List<CyStringDescriptor> GetStringDescList()
        {
            List<CyStringDescriptor> strList = new List<CyStringDescriptor>();
            for (int i = 1; i < m_stringTree.m_nodes[0].m_nodes.Count; i++)
            {
                CyStringDescriptor strDesc =
                                        (CyStringDescriptor)m_stringTree.m_nodes[0].m_nodes[i].m_value;
                if (strDesc.bString != null)
                {
                    strList.Add(strDesc);
                }
            }
            return strList;
        }

        public List<string> GetStringsList()
        {
            List<CyStringDescriptor> strDescList = GetStringDescList();
            List<string> strList = new List<string>();
            for (int i = 0; i < strDescList.Count; i++)
            {
                strList.Add(strDescList[i].bString);
            }
            return strList;
        }

        public uint SaveStringDescriptor(string value)
        {
            CyStringDescriptor strDesc = null;
            uint strIndex = 0;

            if (value == "") return strIndex;

            //Search for string in predefined array
            bool predefined = false;
            List<CyStringDescriptor> strDescList = GetStringDescList();
            for (int i = 0; i < strDescList.Count; i++)
            {
                if (strDescList[i].bString == value)
                {
                    predefined = true;
                    strDesc = strDescList[i];
                    break;
                }
            }

            if (!predefined)
            {
                strDesc = CreateNewStringDescriptor(value, this);
                ParamStringTreeChanged = true;
            }

            //General USBDescriptor-based index
            strIndex = GetStringDescriptorIndex(strDesc, this);
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
                CyDescriptorNode newNode;
                if (type == NODEKEY_STRING_SERIAL)
                {
                    newNode = parameters.m_stringTree.GetNodeByKey(strConfigKey);
                    ((CyStringDescriptor)newNode.m_value).bString = comboBox.Text;
                }
                strIndex = Convert.ToByte(CyDescriptorNode.GetDescriptorIndex(strConfigKey));
            }
            else if (comboBox.SelectedIndex >= 0)
            {
                strIndex = Convert.ToByte(CyDescriptorNode.GetDescriptorIndex(strConfigKey));
            }
            return strIndex;
        }


        #endregion Manipulations with String descriptors

        #region Public Static functions

        /// <summary>
        /// Finds the index of the report that was selected in the descriptor.
        /// </summary>
        /// <param name="comboBox">Combobox where user selects the report</param>
        /// <param name="parameters">Reference to the Parameters</param>
        /// <returns>Unique index of the report</returns>
        public static uint SaveReportDescriptor(ComboBox comboBox, CyUSBFSParameters parameters)
        {
            CyHIDReportDescriptor reportDesc = null;
            string strConfigKey;
            uint strIndex = 0;

            if (comboBox.Text == "") return strIndex;


            reportDesc = (CyHIDReportDescriptor)comboBox.Items[comboBox.SelectedIndex];

            strConfigKey = parameters.m_hidReportTree.GetKeyByNode(reportDesc);
            if (strConfigKey != "")
            {
                strIndex = CyDescriptorNode.GetDescriptorIndex(strConfigKey);
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
            // If value has 3 bytes, add 4th byte
            if (byteList.Count == 3)
            {
                if (val >= 0) byteList.Add(0);
                else byteList.Add(0xFF);
            }
            return byteList;
        }

        public static List<byte> ConvertInt3ToByteArray(Int64 val)
        {
            List<byte> byteList = ConvertIntToByteArray(val);
            while (byteList.Count < 3)
            {
                byteList.Add(0);
            }
            if (byteList.Count > 3)
                byteList.RemoveRange(3, byteList.Count - 3);
            return byteList;
        }

        public static bool CheckIntValue(string str, bool hex)
        {
            bool res;
            Int64 val;
            if (!hex)
            {
                res = Int64.TryParse(str, out val);
            }
            else
            {
                if (str.StartsWith("0x"))
                    str = str.Remove(0, 2);
                res = Int64.TryParse(str, NumberStyles.HexNumber, Application.CurrentCulture.NumberFormat, out val);
            }
            return res;
        }

        public static bool CheckIntRange(int val, int min, int max)
        {
            bool res = ((val >= min) && (val <= max)) ? true : false;
            return res;
        }

        public static void ChangeArrayLength(List<byte> arr, int len)
        {
            if (arr.Count < len)
            {
                int count = arr.Count;
                for (int i = 0; i < len - count; i++)
                    arr.Add(0);
            }
            else if (arr.Count > len)
                arr.RemoveRange(len, arr.Count - len);
        }

        #endregion Public Static functions

        #region Public functions

        /// <summary>
        /// Fills Device, String and HIDReport trees with the default nodes.
        /// </summary>
        /// <param name="device">Apply to the m_deviceTree or no</param>
        /// <param name="str">Apply to the m_stringTree or no</param>
        /// <param name="hidReport">Apply to the m_hidReportTree or no</param>
        public void InitDescriptors(bool device, bool str, bool hidReport, bool audio, bool cdc, bool midi)
        {
            if (device)
            {
                if (m_deviceTree.m_nodes.Count > 0)
                    ParamDeviceTreeChanged = true;
                m_deviceTree.m_nodes.Clear();
                m_deviceTree.AddNode(NODEKEY_DEVICE);
            }
            if (str)
            {
                if (m_stringTree.m_nodes.Count > 0)
                    ParamStringTreeChanged = true;
                m_stringTree.m_nodes.Clear();
                m_stringTree.m_nodes.Clear();
                m_stringTree.AddNode(NODEKEY_STRING);
                m_stringTree.AddNode(NODEKEY_SPECIALSTRING);
            }
            if (hidReport)
            {
                if (m_hidReportTree.m_nodes.Count > 0)
                    ParamHIDReportTreeChanged = true;
                m_hidReportTree.m_nodes.Clear();
                m_hidReportTree.AddNode(NODEKEY_HIDREPORT);
            }
            if (audio)
            {
                if (m_audioTree.m_nodes.Count > 0)
                    ParamAudioTreeChanged = true;
                m_audioTree.m_nodes.Clear();
                m_audioTree.AddNode(NODEKEY_AUDIO);
            }
            if (cdc)
            {
                if (m_cdcTree.m_nodes.Count > 0)
                    ParamCDCTreeChanged = true;
                m_cdcTree.m_nodes.Clear();
                m_cdcTree.AddNode(NODEKEY_CDC);
            }
            if (midi)
            {
                if (m_midiTree.m_nodes.Count > 0)
                    ParamMidiTreeChanged = true;
                m_midiTree.m_nodes.Clear();
                m_midiTree.AddNode(NODEKEY_MIDI);
            }
        }

        /// <summary>
        /// Performs neccessary calculations in the descriptors when editing of desriptors is finished: 
        /// Removes unused controls;
        /// Sets string indexes;
        /// Updates interface numbers;
        /// Other.
        /// </summary>
        /// <param name="node"></param>
        public void RecalcDescriptors(CyDescriptorNode node)
        {
            if (node.Key == NODEKEY_DEVICE)
            {
                //Clean m_emptyFields array
                m_emptyFields.Clear();

                // Reset m_usedEp array 
                for (int i = 0; i < m_usedEp.Length; i++)
                {
                    m_usedEp[i] = false;
                }

                //Reset m_max_interfaces
                m_max_interfaces = 0;
                m_extJackCount = 0;
            }

            for (int i = 0; i < node.m_nodes.Count; i++)
            {
                CyDescriptorNode node_child = node.m_nodes[i];
                if (node_child.m_value != null)
                {
                    switch (node_child.m_value.bDescriptorType)
                    {
                        case CyUSBDescriptorType.DEVICE:
                            CyDeviceDescriptor deviceDescriptor = (CyDeviceDescriptor) node_child.m_value;
                            deviceDescriptor.bNumConfigurations = (byte) node_child.m_nodes.Count;
                            deviceDescriptor.iManufacturer = GetStringLocalIndex(deviceDescriptor.iwManufacturer);
                            deviceDescriptor.iProduct = GetStringLocalIndex(deviceDescriptor.iwProduct);
                            deviceDescriptor.sManufacturer = GetStringDescTextByIndex(deviceDescriptor.iwManufacturer);
                            deviceDescriptor.sProduct = GetStringDescTextByIndex(deviceDescriptor.iwProduct);
                            deviceDescriptor.sSerialNumber = (GetSerialDescriptor()).bString;
                            break;
                        case CyUSBDescriptorType.CONFIGURATION:
                            CyConfigDescriptor configDescriptor = (CyConfigDescriptor) node_child.m_value;
                            configDescriptor.bNumInterfaces = (byte)node_child.m_nodes.Count;
                            // Calculate max_interfaces 
                            if (configDescriptor.bNumInterfaces > MaxInterfaces)
                            {
                                MaxInterfaces = configDescriptor.bNumInterfaces;
                            }
                            // Calculate bConfigurationValue
                            int configIndex = node.m_nodes.IndexOf(node_child);
                            if (configIndex >= 0)
                                configDescriptor.bConfigurationValue = (byte) configIndex;
                            else
                                configDescriptor.bConfigurationValue = 0;

                            configDescriptor.iConfiguration = GetStringLocalIndex(configDescriptor.iwConfiguration);
                            configDescriptor.sConfiguration = 
                                GetStringDescTextByIndex(configDescriptor.iwConfiguration);
                            // Calculate the total length
                            ushort totalLength = configDescriptor.bLength;
                            for (int j = 0; j < node_child.m_nodes.Count; j++)
                            {
                                CyDescriptorNode node_in_config = node_child.m_nodes[j];
                                for (int k = 0; k < node_in_config.m_nodes.Count; k++)
                                {
                                    CyDescriptorNode node_in_interfaces = node_in_config.m_nodes[k];
                                    totalLength += node_in_interfaces.m_value.bLength;
                                    for (int l = 0; l < node_in_interfaces.m_nodes.Count; l++)
                                    {
                                        CyDescriptorNode node_in_interface = node_in_interfaces.m_nodes[l];
                                        totalLength += node_in_interface.m_value.bLength;
                                        for (int m = 0; m < node_in_interface.m_nodes.Count; m++)
                                        {
                                            CyDescriptorNode node_in_endpoint = node_in_interface.m_nodes[m];
                                            totalLength += node_in_endpoint.m_value.bLength;
                                        }
                                    }
                                }
                            }
                            configDescriptor.wTotalLength = totalLength;
                            break;
                        case CyUSBDescriptorType.INTERFACE:
                            CyInterfaceDescriptor interfaceDescriptor = (CyInterfaceDescriptor)node_child.m_value;
                            //Calculate the number of Endpoints
                            byte numEndpt = 0;
                            for (int j = 0; j < node_child.m_nodes.Count; j++)
                            {
                                CyDescriptorNode node_endpt = node_child.m_nodes[j];
                                if (node_endpt.m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT)
                                    numEndpt++;
                            }
                            //Calculate the number of External MIDI IN Jack, OUT Jack
                            if ((interfaceDescriptor.bInterfaceClass == 1) &&
                                (interfaceDescriptor.bInterfaceSubClass == 
                                 (byte)CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING))
                            {
                                byte numExtMidiIN = 0;
                                byte numExtMidiOUT = 0;
                                for (int j = 0; j < node_child.m_nodes.Count; j++)
                                {
                                    CyDescriptorNode node_jack = node_child.m_nodes[j];
                                    if (node_jack.m_value is CyMSInJackDescriptor)
                                    {
                                        if (((CyMSInJackDescriptor)node_jack.m_value).bJackType == 
                                              CyUSBOtherTypes.CyJackTypes.EXTERNAL)
                                            numExtMidiIN++;
                                    }
                                    else if (node_jack.m_value is CyMSOutJackDescriptor)
                                    {
                                        if (((CyMSOutJackDescriptor)node_jack.m_value).bJackType == 
                                              CyUSBOtherTypes.CyJackTypes.EXTERNAL)
                                            numExtMidiOUT++;
                                    }
                                }
                                ExtJackCount = Math.Max(m_extJackCount, Math.Max(numExtMidiIN, numExtMidiOUT));
                            }
                            interfaceDescriptor.bNumEndpoints = numEndpt;
                            interfaceDescriptor.iInterface = GetStringLocalIndex(interfaceDescriptor.iwInterface);
                            interfaceDescriptor.sInterface = GetStringDescTextByIndex(interfaceDescriptor.iwInterface);
                            break;
                        case CyUSBDescriptorType.HID:
                            CyHIDDescriptor hidDescriptor = (CyHIDDescriptor)node_child.m_value;
                            hidDescriptor.bNumDescriptors = 1;
                            hidDescriptor.bReportIndex = GetHIDReportLocalIndex(hidDescriptor.wReportIndex);

                            // Calculate wDescriptorLength
                            string reportKey = CyDescriptorNode.GetKeyByIndex(hidDescriptor.wReportIndex);
                            CyDescriptorNode nodeHIDReport = m_hidReportTree.GetNodeByKey(reportKey);
                            if (nodeHIDReport != null)
                            {
                                hidDescriptor.wDescriptorLength = 
                                                                ((CyHIDReportDescriptor)nodeHIDReport.m_value).wLength;
                            }
                            else
                            {
                                hidDescriptor.wReportIndex = 0;
                            }

                            if ((hidDescriptor.wReportIndex == 0) && (!m_emptyFields.Contains(node_child.Key)))
                                m_emptyFields.Add(node_child.Key);
                            break;
                        case CyUSBDescriptorType.ENDPOINT:
                            CyEndpointDescriptor endpointDescriptor = (CyEndpointDescriptor)node_child.m_value;
                            byte endpointNum = (byte)(endpointDescriptor.bEndpointAddress & 0x0F);
                            if (endpointNum < m_usedEp.Length)
                                m_usedEp[endpointNum] = true;
                            break;
                        case CyUSBDescriptorType.HID_REPORT:
                            CyHIDReportDescriptor hidRptDescriptor = (CyHIDReportDescriptor)node_child.m_value;
                            ushort rptSize = 0;
                            for (int j = 0; j < node_child.m_nodes.Count; j++)
                            {
                                CyDescriptorNode node_in_report = node_child.m_nodes[j];
                                rptSize += CyUsbCodePrimitives.CalcHIDItemLength(
                                                                    (CyHIDReportItemDescriptor)node_in_report.m_value);
                            }
                            hidRptDescriptor.wLength = rptSize;
                            break;
                        case CyUSBDescriptorType.AUDIO:
                            // String indexes
                            try
                            {
                                Type nodeType = node_child.m_value.GetType();
                                FieldInfo[] fi = nodeType.GetFields();
                                for (int j = 0; j < fi.Length; j++)
                                {
                                    if (fi[j].Name.StartsWith("iw"))
                                    {
                                        uint val = (uint) fi[j].GetValue(node_child.m_value);
                                        string propName = fi[j].Name.Remove(0, 2).Insert(0, "i");
                                        PropertyInfo pi = nodeType.GetProperty(propName);
                                        if (pi != null)
                                            pi.SetValue(node_child.m_value, GetStringLocalIndex(val), null);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.Assert(false, ex.ToString());
                            }
                            node_child.m_value.UpdateLength();

                            // Calculate the total length of CyACHeaderDescriptor
                            if ((node_child.m_value is CyACHeaderDescriptor) || 
                                (node_child.m_value is CyACHeaderDescriptor_v2) ||
                                (node_child.m_value is CyMSHeaderDescriptor))
                            {
                                ushort totalLengthAudio = 0;
                                for (int j = 0; j < node.m_nodes.Count; j++)
                                {
                                    CyDescriptorNode node_audio = node.m_nodes[j];
                                    if (node_audio.m_value.bDescriptorType == CyUSBDescriptorType.AUDIO)
                                    {
                                        node_audio.m_value.UpdateLength();
                                        totalLengthAudio += node_audio.m_value.bLength;
                                    }
                                }
                                if (node_child.m_value is CyACHeaderDescriptor)
                                {
                                    CyACHeaderDescriptor acHeaderDesc = (CyACHeaderDescriptor) node_child.m_value;
                                    acHeaderDesc.wTotalLength = totalLengthAudio;
                                }
                                else if (node_child.m_value is CyACHeaderDescriptor_v2)
                                {
                                    CyACHeaderDescriptor_v2 acHeaderDesc = (CyACHeaderDescriptor_v2)node_child.m_value;
                                    acHeaderDesc.wTotalLength = totalLengthAudio;
                                }
                                else if (node_child.m_value is CyMSHeaderDescriptor)
                                {
                                    CyMSHeaderDescriptor msHeaderDesc = (CyMSHeaderDescriptor)node_child.m_value;
                                    msHeaderDesc.wTotalLength = totalLengthAudio;
                                }
                            }

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
        public ushort GetSumPacketSize(CyDescriptorNode configNode)
        {
            ushort totalSize = 0;
            for (int i = 0; i < configNode.m_nodes.Count; i++)
                for (int j = 0; j < configNode.m_nodes[i].m_nodes.Count; j++)
                {
                    CyDescriptorNode node_in_interface = configNode.m_nodes[i].m_nodes[j];
                
                    if (node_in_interface.m_value is CyEndpointDescriptor)
                    {
                        totalSize += ((CyEndpointDescriptor)node_in_interface.m_value).wMaxPacketSize;
                    }
                }
            return totalSize;
        }

        public bool CheckBootloaderReady()
        {
            bool res = false;
            // Parse device tree for all Interface descriptors and check if exists any one that has 
            // at least two endpoints: one In(EP2, MaxPktSize=64) and one Out(EP1, MaxPktSize=64).
            for (int i = 0; i < m_deviceTree.m_nodes[0].m_nodes.Count; i++)//device level
            {
                for (int j = 0; j < m_deviceTree.m_nodes[0].m_nodes[i].m_nodes.Count; j++) // configuration level
                {
                    // alternate level
                    for (int k = 0; k < m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes.Count; k++) 
                    {
                        // interface level
                        for (int m = 0; m < m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].m_nodes.Count; m++)
                        {
                            CyDescriptorNode node = m_deviceTree.m_nodes[0].m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[m];
                            CyInterfaceDescriptor desc = (CyInterfaceDescriptor) node.m_value;
                            
                            bool foundIN = false;
                            bool foundOUT = false;
                            for (int l = 0; l < node.m_nodes.Count; l++)
                            {
                                if (node.m_nodes[l].m_value is CyEndpointDescriptor)
                                {
                                    CyEndpointDescriptor endptDesc = (CyEndpointDescriptor) node.m_nodes[l].m_value;
                                    byte endpointNum = (byte) (endptDesc.bEndpointAddress & 0x0F);
                                    byte direction = (byte) (endptDesc.bEndpointAddress >> 7);
                                    byte transferType = (byte)(endptDesc.bmAttributes & 0x03);
                                    if ((endpointNum == 1) && (direction == 0) && 
                                        (transferType == TRANSFERTYPE_INTERRUPT) && 
                                        (endptDesc.wMaxPacketSize == 64))
                                        foundOUT = true;
                                    else if ((endpointNum == 2) && (direction == 1) && 
                                             (transferType == TRANSFERTYPE_INTERRUPT) &&
                                             (endptDesc.wMaxPacketSize == 64))
                                        foundIN = true;
                                }
                            }
                            if (foundIN && foundOUT)
                            {
                                res = true;
                                break;
                            }
                        }
                        if (res) break;
                    }
                    if (res) break;
                }
                if (res) break;
            }
            return res;
        }

        public bool CheckSiliconRevisionForDMA(ICyDeviceQuery_v1 deviceQuery, out string selOption)
        {
            bool res = true;
            bool supportDMA = true;
            if (deviceQuery.ArchFamilyMember == "PSoC5A")
            {
                supportDMA = false;
            }
            selOption = "";

            if (!supportDMA)
            {
                if (EPMemoryMgmt != (byte)CyMemoryManagement.MANUAL) // if DMA Manual or DMA Automatic
                {
                    res = false;
                    if (EPMemoryMgmt == (byte)CyMemoryManagement.DMA_MANUAL_MEMORY) 
                        selOption = "DMA w/Manual Memory Mgmt";
                    else if (EPMemoryMgmt == (byte)CyMemoryManagement.DMA_AUTOMATIC_MEMORY) 
                        selOption = "DMA w/Automatic Memory Mgmt";
                }
            }
            return res;
        }

        public void SetNodesISerialNumber()
        {
            bool m_showSerial = (GetSerialDescriptor()).bUsed;
            if (!m_showSerial)
            {
                // Set Serial Number in DeviceDescriptor to zero
                try
                {
                    for (int i = 0; i < m_deviceTree.m_nodes[0].m_nodes.Count; i++)
                    {
                        ((CyDeviceDescriptor)m_deviceTree.m_nodes[0].m_nodes[i].m_value).iSerialNumber
                            = 0;
                    }
                }
                catch (Exception)
                {
                    Debug.Assert(false);
                }
            }
            else
            {
                for (int i = 0; i < m_deviceTree.m_nodes[0].m_nodes.Count; i++)
                {
                    ((CyDeviceDescriptor)m_deviceTree.m_nodes[0].m_nodes[i].m_value).iSerialNumber =
                       Convert.ToByte(CyDescriptorNode.GetDescriptorIndex(CyUSBFSParameters.NODEKEY_STRING_SERIAL));
                }
            }
        }

        /// <summary>
        /// Generate error when Endpoint MaxPacketSize is greater then 512 and EMM is not "DMA w/AutomaticMM".
        /// </summary>
        public bool CheckEPMaxPacketSize(CyDescriptorNode node)
        {
            bool res = true;

            if (EPMemoryMgmt == (byte)CyMemoryManagement.DMA_AUTOMATIC_MEMORY) 
                return true;
            
            if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT))
            {
                CyEndpointDescriptor desc = (CyEndpointDescriptor)node.m_value;
                if (desc.wMaxPacketSize > 512)
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    res &= CheckEPMaxPacketSize(node.m_nodes[i]);
                }
            }
            return res;
        }

        /// <summary>
        /// Generate error when  total sum of the MaxPacketSize for all EPs in one Device is greater then 512 
        /// when EMM is not "DMA w/AutomaticMM".
        /// </summary>
        public bool CheckSumEPMaxPacketSize(CyDescriptorNode node)
        {
            bool res = true;

            if (EPMemoryMgmt == (byte)CyMemoryManagement.DMA_AUTOMATIC_MEMORY)
                return true;

            if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE))
            {
                int sum = 0;
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    if (node.m_nodes[i].m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT)
                    {
                        CyEndpointDescriptor desc = (CyEndpointDescriptor)node.m_nodes[i].m_value;
                        sum += desc.wMaxPacketSize;
                    }
                }
                if (sum > 512)
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    res &= CheckSumEPMaxPacketSize(node.m_nodes[i]);
                }
            }
            return res;
        }

        /// <summary>
        /// Generate error when  total sum of the MaxPacketSize for all EPs in one Device is greater then 1100. 
        /// </summary>
        public bool CheckSumEPMaxPacketSizeAll(CyDescriptorNode node)
        {
            bool res = true;

            if ((node.m_value != null) && (node.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE))
            {
                int sum = 0;
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    if (node.m_nodes[i].m_value.bDescriptorType == CyUSBDescriptorType.ENDPOINT)
                    {
                        CyEndpointDescriptor desc = (CyEndpointDescriptor)node.m_nodes[i].m_value;
                        sum += desc.wMaxPacketSize;
                    }
                }
                if (sum > 1100)
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < node.m_nodes.Count; i++)
                {
                    res &= CheckSumEPMaxPacketSizeAll(node.m_nodes[i]);
                }
            }
            return res;
        }


        #endregion Public functions

        #region Private functions

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
                for (byte i = 0; i < m_stringTree.m_nodes[0].m_nodes.Count; i++)
                {
                    if (CyDescriptorNode.GetDescriptorIndex(m_stringTree.m_nodes[0].m_nodes[i].Key) == keyIndex)
                    {
                        localIndex = i;
                        break;
                    }
                }
            }
            return localIndex;
        }

        private string GetStringDescTextByIndex(uint keyIndex)
        {
            string res = null;
            if (keyIndex > 0)
            {
                string configStrKey = CyDescriptorNode.GetKeyByIndex(keyIndex);
                CyDescriptorNode node = m_stringTree.GetNodeByKey(configStrKey);
                if (node != null)
                {
                    res = ((CyStringDescriptor)node.m_value).bString;
                }
            }
            return res;
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
                for (byte i = 0; i < m_hidReportTree.m_nodes[0].m_nodes.Count; i++)
                {
                    if (CyDescriptorNode.GetDescriptorIndex(m_hidReportTree.m_nodes[0].m_nodes[i].Key) == keyIndex)
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
        private void CheckOldFormatCompability(CyDescriptorTree dTree)
        {
            bool oldFormat = false;
            for (int i = 0; i < dTree.m_nodes.Count; i++) // Level 1
                for (int j = 0; j < dTree.m_nodes[i].m_nodes.Count; j++) // Level 2 - Device
                    for (int k = 0; k < dTree.m_nodes[i].m_nodes[j].m_nodes.Count; k++) // Level 3 - Configuration
                        for (int m = 0; m < dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes.Count; m++) // Level 4 - Int.
                        {
                            CyDescriptorNode node = dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[m];
                            if (node.m_value.bDescriptorType != CyUSBDescriptorType.ALTERNATE)
                            {
                                oldFormat = true;
                            }
                        }
            //If the tree is in old format, add a node layer
            if (oldFormat)
            {
                for (int i = 0; i < dTree.m_nodes.Count; i++) // Level 1
                    for (int j = 0; j < dTree.m_nodes[i].m_nodes.Count; j++) // Level 2 - Device
                        for (int k = 0; k < dTree.m_nodes[i].m_nodes[j].m_nodes.Count; k++) // Level 3 - Configuration
                        {
                            // Search for interface numbers
                            List<byte> interfaceNums = new List<byte>();
                            for (int m = 0; m < dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes.Count; m++)
                                // Level 4 - Interface
                            {
                                CyDescriptorNode node = dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[m];
                                byte interfaceNum = ((CyInterfaceDescriptor) node.m_value).bInterfaceNumber;
                                if (!interfaceNums.Contains(interfaceNum))
                                    interfaceNums.Add(interfaceNum);
                            }
                            interfaceNums.Sort();

                            // Add interfaces to tree and attach alternate to them
                            for (int m = 0; m < interfaceNums.Count; m++)
                            {
                                CyDescriptorNode newNode = dTree.AddNode(dTree.m_nodes[i].m_nodes[j].m_nodes[k].Key);
                                newNode.m_nodes.Clear();
                                for (int n = 0; n < dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes.Count; n++)
                                {
                                    CyDescriptorNode interfaceNode = dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes[n];
                                    if ((interfaceNode.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE) &&
                                        (((CyInterfaceDescriptor) interfaceNode.m_value).bInterfaceNumber ==
                                         interfaceNums[m]))
                                    {
                                        dTree.m_nodes[i].m_nodes[j].m_nodes[k].m_nodes.Remove(interfaceNode);
                                        newNode.m_nodes.Add(interfaceNode);
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
        private void CheckStringOldFormatCompability(CyDescriptorTree sTree)
        {
            // Check if MS OS String Descriptor has its default value
            ((CyStringDescriptor) sTree.m_nodes[1].m_nodes[1].m_value).bString = CyUSBFSParameters.MSOS_STRING;
        }

        /// <summary>
        /// For compatibility with previous versions, assigns correct 
        /// usage values list to USAGE HID report items.
        /// </summary>
        private void RestoreHIDReportUsageList()
        {
            for (int i = 0; i < m_hidReportTree.m_nodes[0].m_nodes.Count; i++)
			{
                UInt16 lastUsagePage = 0;
                for (int j = 0; j < m_hidReportTree.m_nodes[0].m_nodes[i].m_nodes.Count; j++)
                {
                    try
                    {
                        CyHidReportItem item =
                            ((CyHIDReportItemDescriptor)m_hidReportTree.m_nodes[0].m_nodes[i].m_nodes[j].m_value).Item;
                        if ((item.Name == RPTITEM_USAGE_PAGE) && (item.m_value.Count == 2))
                        {
                            lastUsagePage = item.m_value[1];
                        }
                        if ((item.Name == RPTITEM_USAGE) && 
                            (item.m_valuesList.Count == CyHIDReportItemTables.ValuesGenericDesktopPage.Length/2))
                        {
                            CyHidDescriptorPage.SetListForItem(item, lastUsagePage);
                        }
                    }
                    catch
                    {
                        Debug.Assert(false);
                    }
                }
			}
        }

        private static string GetTemplateFileExt(CyUSBDescriptorType kind)
        {
            string ext = String.Empty;
            switch (kind)
            {
                case CyUSBDescriptorType.DEVICE:
                    ext = "Device Descriptor Template Files (*.dev.xml)|*.dev.xml|";
                    break;
                case CyUSBDescriptorType.CONFIGURATION:
                    ext = "Configuration Descriptor Template Files (*.cfg.xml)|*.cfg.xml|";
                    break;
                case CyUSBDescriptorType.INTERFACE:
                    ext = "Interface Descriptor Template Files (*.inf.xml)|*.inf.xml|";
                    break;
                case CyUSBDescriptorType.HID_REPORT:
                    ext = "HID Template Files (*.hid.xml)|*.hid.xml|";
                    break;
                case CyUSBDescriptorType.AUDIO:
                    ext = "Audio Interface Descriptor Template Files (*.audio.xml)|*.audio.xml|";
                    break;
                case CyUSBDescriptorType.MIDI:
                    ext = "MIDI Interface Descriptor Template Files (*.midi.xml)|*.midi.xml|";
                    break;
                case CyUSBDescriptorType.CDC:
                    ext = "CDC Interface Descriptor Template Files (*.cdc.xml)|*.cdc.xml|";
                    break;
                case CyUSBDescriptorType.ALL:
                    ext = "Descriptor Tree Template Files (*.root.xml)|*.root.xml|";
                    break;
            }
            return ext;
        }
  
        private static string GetTemplateDir()
        {
            // "USBFS_v2_20" should be replaced with other version after upgrade 
            DirectoryInfo di = new DirectoryInfo(
                    "..\\psoc\\content\\cycomponentlibrary\\CyComponentLibrary.cylib\\USBFS_v2_20\\Custom\\template");
            return di.FullName;
        }

        #endregion Private functions
    }

    //=================================================================================================================

    [XmlType("DescriptorNode")]
    public class CyDescriptorNode
    {
        #region Fields, Properties

        [XmlElement("Value")]
        public CyUSBDescriptor m_value;
        private string m_key;
        private static uint m_keyIndex = 1;
        [XmlArray("Nodes")] public List<CyDescriptorNode> m_nodes;

        [XmlAttribute]
        public string Key
        {
            get { return m_key; }
            set
            {
                m_key = value;
                if ((GetDescriptorIndex(value) >= m_keyIndex) && (value != CyUSBFSParameters.NODEKEY_STRING_SERIAL) && 
                    (value != CyUSBFSParameters.NODEKEY_MSOS_STRING))
                {
                    m_keyIndex = GetDescriptorIndex(value) + 1;
                    if (m_keyIndex == CyUSBFSParameters.SERIAL_STRING_INDEX || 
                       (m_keyIndex == CyUSBFSParameters.MSOS_STRING_INDEX))
                        m_keyIndex++;
                }
            }
        }

        #endregion Fields, Properties

        #region Constructors

        public CyDescriptorNode()
        {
            m_nodes = new List<CyDescriptorNode>();
        }

        public CyDescriptorNode(CyUSBDescriptor value) : this()
        {
            m_key = CyUSBFSParameters.NODEKEY_USBDESCRIPTOR + m_keyIndex++;
            if ((m_keyIndex == CyUSBFSParameters.SERIAL_STRING_INDEX) || 
                (m_keyIndex == CyUSBFSParameters.MSOS_STRING_INDEX))
                m_keyIndex++;
            m_value = value;
        }

        public CyDescriptorNode(string key, CyUSBDescriptor value) : this()
        {
            m_key = key;
            m_value = value;
        }

        #endregion Constructors

        #region Public functions

        public static uint GetDescriptorIndex(string key)
        {
            uint res = 0;
            if (key.IndexOf(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR) >= 0)
                res = Convert.ToUInt32(key.Replace(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR, ""));
            else if (key.IndexOf(CyUSBFSParameters.NODEKEY_INTERFACE) >= 0)
                res = Convert.ToUInt32(key.Replace(CyUSBFSParameters.NODEKEY_INTERFACE, ""));
            else
            {
                switch (key)
                {
                    case CyUSBFSParameters.NODEKEY_STRING_LANGID:
                        res = 0;
                        break;
                    case CyUSBFSParameters.NODEKEY_STRING_SERIAL:
                        res = CyUSBFSParameters.SERIAL_STRING_INDEX;
                        break;
                    case CyUSBFSParameters.NODEKEY_MSOS_STRING:
                        res = CyUSBFSParameters.MSOS_STRING_INDEX;
                        break;
                }
            }
            return res;
        }

        public static void SetKeyIndex(uint index)
        {
            m_keyIndex = index;
        }

        public static string GetKeyByIndex(uint index)
        {
            return CyUSBFSParameters.NODEKEY_USBDESCRIPTOR + index;
        }

        public static string GetDescriptorLabel(CyDescriptorNode descriptor)
        {
            string nodeLabel = "";
            if (descriptor.m_value == null)
            {
                switch (descriptor.Key)
                {
                    case CyUSBFSParameters.NODEKEY_DEVICE:
                        nodeLabel = "Descriptor Root";
                        break;
                    case CyUSBFSParameters.NODEKEY_STRING:
                        nodeLabel = "String Descriptors";
                        break;
                    case CyUSBFSParameters.NODEKEY_SPECIALSTRING:
                        nodeLabel = "Special Strings";
                        break;
                    case CyUSBFSParameters.NODEKEY_HIDREPORT:
                        nodeLabel = "HID Report Descriptors";
                        break;
                    case CyUSBFSParameters.NODEKEY_AUDIO:
                        nodeLabel = "Audio Descriptors";
                        break;
                    case CyUSBFSParameters.NODEKEY_MIDI:
                        nodeLabel = "MIDI Descriptors";
                        break;
                    case CyUSBFSParameters.NODEKEY_CDC:
                        nodeLabel = "CDC Descriptors";
                        break;
                    default:
                        nodeLabel = "Unknown Descriptor";
                        break;
                }
            }
            else
            {
                switch (descriptor.m_value.bDescriptorType)
                {
                    case CyUSBDescriptorType.STRING:
                        if (descriptor.Key == CyUSBFSParameters.NODEKEY_STRING_LANGID)
                            nodeLabel = "LANGID";
                        else if (descriptor.Key == CyUSBFSParameters.NODEKEY_STRING_SERIAL)
                            nodeLabel = "Serial Number String";
                        else if (descriptor.Key == CyUSBFSParameters.NODEKEY_MSOS_STRING)
                            nodeLabel = "MS OS String Descriptor";
                        else
                        {
                            nodeLabel = string.Format("String             \"{0}\"",
                                                      ((CyStringDescriptor) descriptor.m_value).bString);
                        }
                        break;

                    default:
                        nodeLabel = descriptor.m_value.ToString();
                        break;
                }
            }
            return nodeLabel;
        }

        #endregion Public functions
    }

    //=================================================================================================================

    [XmlRootAttribute("Tree")]
    public class CyDescriptorTree
    {
        private const string HIDREPORT_DEF_NAME = "HID Report Descriptor ";

        [XmlArray("Tree Descriptors")] public List<CyDescriptorNode> m_nodes;

        public CyDescriptorTree()
        {
            m_nodes = new List<CyDescriptorNode>();
        }

        #region Public functions

        public CyDescriptorNode GetNodeByKey(string key)
        {
            CyDescriptorNode resultNode = null;
            Queue<CyDescriptorNode> nodeList = new Queue<CyDescriptorNode>(m_nodes);
            while (nodeList.Count > 0)
            {
                CyDescriptorNode node = nodeList.Dequeue();
                if (node.Key == key)
                {
                    resultNode = node;
                    break;
                }
                else
                {
                    for (int i = 0; i < node.m_nodes.Count; i++)
                    {
                        nodeList.Enqueue(node.m_nodes[i]);
                    }
                }
            }
            return resultNode;
        }

        public string GetKeyByNode(CyUSBDescriptor node)
        {
            string resultKey = "";
            Queue<CyDescriptorNode> nodeList = new Queue<CyDescriptorNode>(m_nodes);
            while (nodeList.Count > 0)
            {
                CyDescriptorNode node_tmp = nodeList.Dequeue();
                if ((node_tmp.m_value != null) && (node_tmp.m_value.Equals(node)))
                {
                    resultKey = node_tmp.Key;
                    break;
                }
                else
                {
                    for (int i = 0; i < node_tmp.m_nodes.Count; i++)
                    {
                        nodeList.Enqueue(node_tmp.m_nodes[i]);
                    }
                }
            }
            return resultKey;
        }

        public CyDescriptorNode AddNode(string parentKey)
        {
            CyDescriptorNode parentNode = GetNodeByKey(parentKey);
            CyDescriptorNode newNode = null;
            if (parentNode != null)
            {
                if (parentNode.m_value == null )
                {
                    switch (parentNode.Key)
                    {
                        case CyUSBFSParameters.NODEKEY_DEVICE:
                            newNode = new CyDescriptorNode(new CyDeviceDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case CyUSBFSParameters.NODEKEY_STRING:
                            if (parentNode.m_nodes.Count == 0)
                            {
                                newNode = new CyDescriptorNode(CyUSBFSParameters.NODEKEY_STRING_LANGID,
                                                               new CyStringZeroDescriptor());
                                parentNode.m_nodes.Add(newNode);
                            }
                            newNode = new CyDescriptorNode(new CyStringDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case CyUSBFSParameters.NODEKEY_SPECIALSTRING:
                            if (parentNode.m_nodes.Count == 0)
                            {
                                newNode = new CyDescriptorNode(CyUSBFSParameters.NODEKEY_STRING_SERIAL,
                                                             new CyStringDescriptor());
                                parentNode.m_nodes.Add(newNode);
                            }
                            if (parentNode.m_nodes.Count == 1)
                            {
                                newNode = new CyDescriptorNode(CyUSBFSParameters.NODEKEY_MSOS_STRING,
                                                             new CyStringDescriptor());
                                ((CyStringDescriptor) newNode.m_value).bString = CyUSBFSParameters.MSOS_STRING;
                                parentNode.m_nodes.Add(newNode);
                            }
                            break;
                        case CyUSBFSParameters.NODEKEY_HIDREPORT:
                            //Name of Descriptor
                            int index = 1;
                            while (true)
                            {
                                int i;
                                for (i = 0; i < parentNode.m_nodes.Count; i++)
                                {
                                    if (((CyHIDReportDescriptor) parentNode.m_nodes[i].m_value).Name ==
                                        HIDREPORT_DEF_NAME + index)
                                    {
                                        index++;
                                        break;
                                    }
                                }
                                if (i == parentNode.m_nodes.Count) break;
                            }
                            newNode = new CyDescriptorNode(new CyHIDReportDescriptor(HIDREPORT_DEF_NAME + index));
                            parentNode.m_nodes.Add(newNode);
                            break;
                        default:

                            break;
                    }
                }
                else
                {
                    switch (parentNode.m_value.bDescriptorType)
                    {
                        case CyUSBDescriptorType.DEVICE:
                            newNode = new CyDescriptorNode(new CyConfigDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            AddNode(newNode.Key);
                            break;
                        case CyUSBDescriptorType.CONFIGURATION:
                            uint lastInterfaceKeyIndex = 0;
                            if (parentNode.m_nodes.Count > 0)
                            {
                                lastInterfaceKeyIndex =
                                    CyDescriptorNode.GetDescriptorIndex(
                                        parentNode.m_nodes[parentNode.m_nodes.Count - 1].Key);
                                lastInterfaceKeyIndex++;
                            }
                            newNode = new CyDescriptorNode(new CyInterfaceGeneralDescriptor());
                            newNode.Key = newNode.Key.Replace(CyUSBFSParameters.NODEKEY_USBDESCRIPTOR,
                                                              CyUSBFSParameters.NODEKEY_INTERFACE);
                            parentNode.m_nodes.Add(newNode);
                            AddNode(newNode.Key, (byte) (parentNode.m_nodes.Count - 1));
                            break;
                        case CyUSBDescriptorType.INTERFACE:
                            newNode = new CyDescriptorNode(new CyEndpointDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            break;
                        case CyUSBDescriptorType.ENDPOINT:
                            newNode = new CyDescriptorNode(new CyASEndpointDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            break;
                        case CyUSBDescriptorType.HID_REPORT:
                            newNode = new CyDescriptorNode(new CyHIDReportItemDescriptor());
                            parentNode.m_nodes.Add(newNode);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                newNode = new CyDescriptorNode(parentKey, null);
                m_nodes.Add(newNode);
                if ((newNode.Key != CyUSBFSParameters.NODEKEY_STRING) && 
                    (newNode.Key != CyUSBFSParameters.NODEKEY_HIDREPORT) &&
                    (newNode.Key != CyUSBFSParameters.NODEKEY_AUDIO))
                    AddNode(newNode.Key);
            }
            return newNode;
        }

        public void AddNode(string parentKey, CyUSBDescriptorType type)
        {
            CyDescriptorNode parentNode = GetNodeByKey(parentKey);
            CyDescriptorNode newNode = null;
            if (parentNode != null)
            {
                switch (parentNode.m_value.bDescriptorType)
                {
                    case CyUSBDescriptorType.INTERFACE:
                        if (type == CyUSBDescriptorType.HID)
                        {
                            newNode = new CyDescriptorNode(new CyHIDDescriptor());
                            parentNode.m_nodes.Add(newNode);
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                newNode = new CyDescriptorNode(parentKey, null);
                m_nodes.Add(newNode);
                AddNode(newNode.Key);
            }
        }

        /// <summary>
        /// This function is used for alternate interfaces.
        /// </summary>
        /// <param name="parentKey"></param>
        /// <param name="index"> Interface number </param>
        public void AddNode(string parentKey, byte index)
        {
            CyDescriptorNode parentNode = GetNodeByKey(parentKey);
            CyDescriptorNode newNode = null;
            if ((parentNode != null) && parentNode.Key.Contains(CyUSBFSParameters.NODEKEY_INTERFACE))
            {
                newNode = new CyDescriptorNode(new CyInterfaceDescriptor(index, (byte) parentNode.m_nodes.Count));
                parentNode.m_nodes.Add(newNode);
                AddNode(newNode.Key);
            }
        }

        public void RemoveNode(string key, string parentKey)
        {
            CyDescriptorNode node = GetNodeByKey(key);
            CyDescriptorNode parentNode = GetNodeByKey(parentKey);
            if (node != null)
            {
                if (parentNode != null)
                {
                    parentNode.m_nodes.Remove(node);
                }
                else
                {
                    this.m_nodes.Remove(node);
                }
            }
        }

        public static string SerializeDescriptors(CyDescriptorTree tree, XmlSerializer s,
                                              XmlSerializerNamespaces customSerNamespace)
        {
            string serializedXml = "";
            using (StringWriter sw = new StringWriter())
            {
                s.Serialize(sw, tree, customSerNamespace);
                serializedXml = sw.ToString();
            }
            return serializedXml;
        }

        public static CyDescriptorTree DeserializeDescriptors(string serializedXml, XmlSerializer s)
        {
            CyDescriptorTree tree = (CyDescriptorTree) s.Deserialize(new StringReader(serializedXml));
            return tree;
        }

        #endregion Public functions
    }

    //=================================================================================================================
    /// <summary>
    /// Describes a kind of Settings Panel for the HID Report Item.
    /// </summary>
    [XmlType("HIDReportItemKind")]
    public enum CyHidReportItemKind
    {
        [XmlEnum("Bits")]
        BITS,
        [XmlEnum("List")]
        LIST,
        [XmlEnum("Int")]
        INT,
        [XmlEnum("Unit")]
        UNIT,
        [XmlEnum("None")]
        NONE,
        [XmlEnum("Custom")]
        CUSTOM
    } ;

    //=================================================================================================================

    /// <summary>
    /// HID Report Item class
    /// </summary>
    [XmlRootAttribute("HID_ITEM")]
    public class CyHidReportItem
    {
        #region Fields, Properties

        private string m_name;
        [XmlElement("Description")] public string m_description; 
        [XmlAttribute("Code")] public byte m_prefix;
        [XmlAttribute("Size")] public int m_size;
        [XmlArray("Value")] public List<byte> m_value;
        [XmlIgnore] public Dictionary<ushort, string> m_valuesList;
        private CyHidReportItemKind m_kind;

        [XmlAttribute("Type")]
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                FillDictionary();
            }
        }

        public CyHidReportItemKind Kind
        {
            get { return m_kind; }
            set
            {
                m_kind = value;
                FillDictionary();
            }
        }

        #endregion Fields, Properties

        #region Constructors

        public CyHidReportItem()
        {
            m_valuesList = new Dictionary<ushort, string>();
            m_value = new List<byte>();
        }

        public CyHidReportItem(string name, byte prefix, int size, CyHidReportItemKind kind) : this ()
        {
            m_name = name;
            m_description = "";
            m_prefix = prefix;
            m_size = size;
            m_kind = kind;
        }

        /// <summary>
        /// Used during importing the report.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefix"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public CyHidReportItem(string name, byte prefix, int size, long value, string desc) 
            : this (name, prefix, size, CyHidReportItemKind.NONE)
        {
            m_description = desc;

            //Kind
            switch (m_name)
            {
                case CyUSBFSParameters.RPTITEM_COLLECTION:
                case CyUSBFSParameters.RPTITEM_USAGE:
                case CyUSBFSParameters.RPTITEM_USAGE_PAGE:
                case CyUSBFSParameters.RPTITEM_UNIT_EXPONENT:
                case CyUSBFSParameters.RPTITEM_DELIMITER:
                    m_kind = CyHidReportItemKind.LIST;
                    break;
                case CyUSBFSParameters.RPTITEM_USAGE_MINIMUM:
                case CyUSBFSParameters.RPTITEM_USAGE_MAXIMUM:
                case CyUSBFSParameters.RPTITEM_DESIGNATOR_INDEX:
                case CyUSBFSParameters.RPTITEM_DESIGNATOR_MINIMUM:
                case CyUSBFSParameters.RPTITEM_DESIGNATOR_MAXIMUM:
                case CyUSBFSParameters.RPTITEM_STRING_INDEX:
                case CyUSBFSParameters.RPTITEM_STRING_MINIMUM:
                case CyUSBFSParameters.RPTITEM_STRING_MAXIMUM:
                case CyUSBFSParameters.RPTITEM_LOGICAL_MINIMUM:
                case CyUSBFSParameters.RPTITEM_LOGICAL_MAXIMUM:
                case CyUSBFSParameters.RPTITEM_PHYSICAL_MINIMUM:
                case CyUSBFSParameters.RPTITEM_PHYSICAL_MAXIMUM:
                case CyUSBFSParameters.RPTITEM_REPORT_SIZE:
                case CyUSBFSParameters.RPTITEM_REPORT_ID:
                case CyUSBFSParameters.RPTITEM_REPORT_COUNT:
                    m_kind = CyHidReportItemKind.INT;
                    break;
                case CyUSBFSParameters.RPTITEM_INPUT:
                case CyUSBFSParameters.RPTITEM_OUTPUT:
                case CyUSBFSParameters.RPTITEM_FEATURE:
                    m_kind = CyHidReportItemKind.BITS;
                    break;
                case CyUSBFSParameters.RPTITEM_UNIT:
                    m_kind = CyHidReportItemKind.UNIT;
                    break;
                case CyUSBFSParameters.RPTITEM_PUSH:
                case CyUSBFSParameters.RPTITEM_POP:
                case CyUSBFSParameters.RPTITEM_END_COLLECTION:
                    m_kind = CyHidReportItemKind.NONE;
                    break;
                case CyUSBFSParameters.RPTITEM_CUSTOM:
                    m_kind = CyHidReportItemKind.CUSTOM;
                    break;
                default:
                    m_kind = CyHidReportItemKind.NONE;
                    break;
            }
            FillDictionary();

            m_value.Add((byte) (m_prefix | (byte) size));
            List<byte> byteList;
            if ((m_kind == CyHidReportItemKind.INT) || (m_kind == CyHidReportItemKind.CUSTOM))
            {
                byteList = CyUSBFSParameters.ConvertIntToByteArray(value);
            }
            else
            {
                byteList = new List<byte>(BitConverter.GetBytes(value));
                while ((byteList.Count > 1) && (byteList[byteList.Count - 1] == 0))
                    byteList.RemoveAt(byteList.Count - 1);
            }
            m_value.AddRange(byteList.ToArray());
        }

        public CyHidReportItem(CyHidReportItem item)
        {
            m_name = item.Name;
            m_prefix = item.m_prefix;
            m_size = item.m_size;
            m_description = item.m_description;
            m_kind = item.m_kind;
            m_valuesList = new Dictionary<ushort, string>(item.m_valuesList);
            m_value = new List<byte>(item.m_value);
        }

        #endregion Constructors

        /// <summary>
        /// This string representation is used to display items (except CUSTOM and COMMENT) in the Report tree.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder(string.Format("{0} {1}     ", Name, m_description));

            for (int i = 0; i < m_value.Count; i++)
            {
                output.AppendFormat("{0} ", m_value[i].ToString("X2"));
            }
            return output.ToString();
        }

        #region Public functions

        /// <summary>
        /// This string representation is used to display CUSTOM and COMMENT items in the Report tree.
        /// </summary>
        /// <returns></returns>
        public string ToCustomString()
        {
            StringBuilder output = new StringBuilder();

            if (Name == CyUSBFSParameters.RPTITEM_CUSTOM)
            {
                output.AppendFormat("0x{0} {1}     ", m_value[0].ToString("X2"), m_description);

                for (int i = 0; i < m_value.Count; i++)
                {
                    output.AppendFormat("{0} ", m_value[i].ToString("X2"));
                }
            }
            else if (Name == CyUSBFSParameters.RPTITEM_COMMENT)
            {
                output.Append(Name);
            }

            return output.ToString();
        }

        /// <summary>
        /// Fills the item's m_valuesList depending on its name.
        /// </summary>
        public void FillDictionary()
        {
            const string RPTITEM_NONVOLATILE = "Non Volatile";
            const string RPTITEM_VOLATILE = "Volatile";

            if ((Kind == CyHidReportItemKind.LIST) || (Kind == CyHidReportItemKind.BITS))
            {
                m_valuesList.Clear();
                switch (Name)
                {
                    case CyUSBFSParameters.RPTITEM_USAGE:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesGenericDesktopPage.Length / 2; i++)
                        {
                            m_valuesList.Add(
                                Convert.ToUInt16(CyHIDReportItemTables.ValuesGenericDesktopPage[i * 2 + 1], 16),
                                CyHIDReportItemTables.ValuesGenericDesktopPage[i*2]);
                        }
                        break;
                    case CyUSBFSParameters.RPTITEM_USAGE_PAGE:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesUsagePage.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToUInt16(CyHIDReportItemTables.ValuesUsagePage[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesUsagePage[i * 2]);
                        }
                        break;
                    case CyUSBFSParameters.RPTITEM_COLLECTION:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesCollection.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesCollection[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesCollection[i * 2]);
                        }
                        break;
                    case CyUSBFSParameters.RPTITEM_INPUT:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        break;
                    case CyUSBFSParameters.RPTITEM_OUTPUT:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        m_valuesList[14] = RPTITEM_NONVOLATILE;
                        m_valuesList[15] = RPTITEM_VOLATILE;
                        break;
                    case CyUSBFSParameters.RPTITEM_FEATURE:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesInput.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesInput[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesInput[i * 2]);
                        }
                        m_valuesList[14] = RPTITEM_NONVOLATILE;
                        m_valuesList[15] = RPTITEM_VOLATILE;
                        break;
                    case CyUSBFSParameters.RPTITEM_UNIT_EXPONENT:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesUnitExponent.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesUnitExponent[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesUnitExponent[i * 2]);
                        }
                        break;
                    case CyUSBFSParameters.RPTITEM_DELIMITER:
                        for (int i = 0; i < CyHIDReportItemTables.ValuesDelimiter.Length / 2; i++)
                        {
                            m_valuesList.Add(Convert.ToByte(CyHIDReportItemTables.ValuesDelimiter[i * 2 + 1], 16),
                                           CyHIDReportItemTables.ValuesDelimiter[i * 2]);
                        }
                        break;
                }
            }
        }

        #endregion Public functions
    }

    //=================================================================================================================

    #region CyCustomToolStripColors class
    /// <summary>
    /// CyCustomToolStripColors class is used to define colors for the toolStripMenu 
    /// </summary>
    internal class CyCustomToolStripColors : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin
        {
            get { return SystemColors.Control; }
        }

        public override Color ToolStripGradientMiddle
        {
            get { return SystemColors.Control; }
        }

        public override Color ToolStripGradientEnd
        {
            get { return SystemColors.ControlDark; }
        }

        public override Color ToolStripBorder
        {
            get { return SystemColors.ControlDark; }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return SystemColors.ControlDark; }
        }
    }

    #endregion CyCustomToolStripColors class

    //=================================================================================================================

}
