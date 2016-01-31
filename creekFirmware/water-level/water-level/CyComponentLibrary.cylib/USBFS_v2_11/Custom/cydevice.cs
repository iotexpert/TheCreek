/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace USBFS_v2_11
{
    [XmlType("DeviceDescriptor")]
    public class CyDeviceDescriptor : CyUSBDescriptor
    {
        private byte m_bDeviceClass;
        private byte m_bDeviceSubClass;
        private byte m_bDeviceProtocol;
        private byte m_bMaxPacketSize0;
        private ushort m_idVendor = 0x04B4;
        private ushort m_idProduct = 0x8051;
        private byte m_iManufacturer;
        private byte m_iProduct;
        private byte m_iSerialNumber;
        private byte m_bNumConfigurations;
        private ushort m_bcdDevice = 0x0000;
        private byte m_bMemoryMgmt = 0;
        private byte m_bMemoryAlloc = 0;

        // For internal usage (absolute indexes)
        public uint iwManufacturer;
        public uint iwProduct;

        public string sManufacturer;
        public string sProduct;
        public string sSerialNumber;

        public CyDeviceDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.DEVICE;
            bLength = 18;
        }
        
        public override int GetLevel()
        {
            return 1;
        }

        public override string ToString()
        {
            return "Device Descriptor";
        }

        #region Properties

        public byte bDeviceClass
        {
            get
            {
                return m_bDeviceClass;
            }
            set
            {
                m_bDeviceClass = value;
            }
        }

        public byte bDeviceSubClass
        {
            get
            {
                return m_bDeviceSubClass;
            }
            set
            {
                m_bDeviceSubClass = value;
            }
        }
        public byte bDeviceProtocol
        {
            get
            {
                return m_bDeviceProtocol;
            }
            set
            {
                m_bDeviceProtocol = value;
            }
        }
        public byte bMaxPacketSize0
        {
            get
            {
                return m_bMaxPacketSize0;
            }
            set
            {
                m_bMaxPacketSize0 = value;
            }
        }
        public ushort idVendor
        {
            get
            {
                return m_idVendor;
            }
            set
            {
                m_idVendor = value;
            }
        }
        public ushort idProduct
        {
            get
            {
                return m_idProduct;
            }
            set
            {
                m_idProduct = value;
            }
        }

        /// <value>0x0200</value>
        public ushort bcdDevice
        {
            get
            {
                //return 0x0200;
                return m_bcdDevice;
            }
            set
            {
                m_bcdDevice = value;   
            }
        }
        
        public byte iManufacturer
        {
            get
            {
                return m_iManufacturer;
            }
            set
            {
                m_iManufacturer = value;
            }
        }

        public byte iProduct
        {
            get
            {
                return m_iProduct;
            }
            set
            {
                m_iProduct = value;
            }
        }

        public byte iSerialNumber
        {
            get
            {
                return m_iSerialNumber;
            }
            set
            {
                m_iSerialNumber = value;
            }
        }
        public byte bNumConfigurations
        {
            get
            {
                return m_bNumConfigurations;
            }
            set
            {
                m_bNumConfigurations = value;
            }
        }

        public byte bMemoryMgmt
        {
            get
            {
                return m_bMemoryMgmt;
            }
            set
            {
                m_bMemoryMgmt = value;
            }
        }

        public byte bMemoryAlloc
        {
            get
            {
                return m_bMemoryAlloc;
            }
            set
            {
                m_bMemoryAlloc = value;
            }
        }

        #endregion Properties
    }

    [XmlType("ConfigDescriptor")]
    public class CyConfigDescriptor : CyUSBDescriptor
    {
        private ushort m_wTotalLength;
        private byte m_bNumInterfaces;
        private byte m_bConfigurationValue;
        private byte m_iConfiguration; // index in array of strings
        public uint iwConfiguration; // absolute index (for internal usage)
        private byte m_bmAttributes;
        private byte m_bMaxPower;

        public string sConfiguration;

        public CyConfigDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CONFIGURATION;
            bLength = 9;
            bmAttributes = 1 << 6;
        }

        public override int GetLevel()
        {
            return 2;
        }

        public override string ToString()
        {
            return "Configuration Descriptor";
        }

        #region Properties
        public ushort wTotalLength
        {
            get
            {
                return m_wTotalLength;
            }
            set
            {
                m_wTotalLength = value;
            }
        }

        public byte bNumInterfaces
        {
            get
            {
                return m_bNumInterfaces;
            }
            set
            {
                m_bNumInterfaces = value;
            }
        }

        public byte bConfigurationValue
        {
            get
            {
                return m_bConfigurationValue;
            }
            set
            {
                m_bConfigurationValue = value;
            }
        }

        public byte iConfiguration
        {
            get
            {
                return m_iConfiguration;
            }
            set
            {
                m_iConfiguration = value;
            }
        }

        public byte bmAttributes
        {
            get
            {
                return m_bmAttributes;
            }
            set
            {
                m_bmAttributes = value;
            }
        }

        public byte bMaxPower
        {
            get
            {
                return m_bMaxPower;
            }
            set
            {
                m_bMaxPower = value;
            }
        }

        #endregion
    }

    [XmlType("EndpointDescriptor")]
    public class CyEndpointDescriptor : CyUSBDescriptor
    {
        private byte m_bInterval = 10;
        private byte m_bEndpointAddress;
        private byte m_bmAttributes = 2;
        private ushort m_wMaxPacketSize = 8;
        public bool DoubleBuffer;

        public CyEndpointDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.ENDPOINT;
            bLength = 7;
            bEndpointAddress = 1;
        }

        public override int GetLevel()
        {
            return 4;
        }

        public override string ToString()
        {
            return "Endpoint Descriptor";
        }


        #region Properties

        // Properties for PropertyGrid
        [XmlIgnore]
        [TypeConverter(typeof(CyEnumConverter))]
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public CyUSBOtherTypes.CyEndptNumbers EndpointNum
        {
            get
            {
                return (CyUSBOtherTypes.CyEndptNumbers)(bEndpointAddress & 0x0F);
            }
            set
            {
                bEndpointAddress = (byte)((byte)value | ((byte)Direction << 7));
            }
        }

        [XmlIgnore]
        [TypeConverter(typeof(CyEnumConverter))]
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public CyUSBOtherTypes.CyEndptDirections Direction
        {
            get
            {
                return (CyUSBOtherTypes.CyEndptDirections)(bEndpointAddress >> 7);
            }
            set
            {
                bEndpointAddress = (byte)((byte)EndpointNum | ((byte)value << 7));
            }
        }

        [XmlIgnore]
        [TypeConverter(typeof(CyEnumConverter))]
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public CyUSBOtherTypes.CyEndptTransferTypes TransferType
        {
            get
            {
                return (CyUSBOtherTypes.CyEndptTransferTypes)(bmAttributes & 0x03);
            }
            set
            {
                bmAttributes = (byte)value;
                if ((value != CyUSBOtherTypes.CyEndptTransferTypes.TRANSFERTYPE_ISOCHRONOUS) && (wMaxPacketSize > 64))
                    wMaxPacketSize = 64;
            }
        }

        // General properties
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DisplayName("Interval")]
        public byte bInterval
        {
            get
            {
                return m_bInterval;
            }
            set
            {
                m_bInterval = value;
            }
        }
        [Browsable(false)]
        [DisplayName("Endpoint Address")]
        public byte bEndpointAddress
        {
            get
            {
                return m_bEndpointAddress;
            }
            set
            {
                m_bEndpointAddress = value;
            }
        }
        [Browsable(false)]
        public byte bmAttributes
        {
            get
            {
                return m_bmAttributes;
            }
            set
            {
                m_bmAttributes = value;
            }
        }
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DisplayName("Max Packet Size")]
        public ushort wMaxPacketSize
        {
            get
            {
                return m_wMaxPacketSize;
            }
            set
            {
                if ((TransferType != CyUSBOtherTypes.CyEndptTransferTypes.TRANSFERTYPE_ISOCHRONOUS) && (value > 64))
                    throw new ArgumentOutOfRangeException("The value range for selected transfer type is [0, 64]");
                if ((TransferType == CyUSBOtherTypes.CyEndptTransferTypes.TRANSFERTYPE_ISOCHRONOUS) && (value > 1023))
                    throw new
                        ArgumentOutOfRangeException("The value range  for isochronous transfer type is [0, 1023]");
                m_wMaxPacketSize = value;
            }
        }

        #endregion Properties
    }

    [XmlType("InterfaceDescriptor")]
    public class CyInterfaceDescriptor : CyUSBDescriptor
    {
        private byte m_bInterfaceClass;
        private byte m_bInterfaceSubClass;
        private byte m_bAlternateSetting;
        private byte m_bInterfaceNumber;
        private byte m_bNumEndpoints;
        private byte m_bInterfaceProtocol;
        private byte m_iInterface; 
        public uint iwInterface; // absolute index (for internal usage)
        private string m_sInterface;

        private string m_interfaceDisplayName;

        public CyInterfaceDescriptor(byte interfaceNum, byte alternateSettingNum) : this()
        {
            m_bInterfaceNumber = interfaceNum;
            m_bAlternateSetting = alternateSettingNum;
        }
        public CyInterfaceDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.INTERFACE;
            bLength = 9;
        }

        public override int GetLevel()
        {
            return 3;
        }

        public override string ToString()
        {
            return "Alternate Setting " + bAlternateSetting;
        }


        #region Properties
        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON),
        TypeConverter(typeof(CyInterfaceClassConverter))]
        public byte bInterfaceClass
        {
            get
            {
                return m_bInterfaceClass;
            }
            set
            {
                if (m_bInterfaceClass != value)
                {
                    m_bInterfaceClass = value;
                    m_bInterfaceSubClass = 0;                    
                }
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte bAlternateSetting
        {
            get
            {
                return m_bAlternateSetting;
            }
            set
            {
                m_bAlternateSetting = value;
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte bInterfaceNumber
        {
            get
            {
                return m_bInterfaceNumber;
            }
            set
            {
                m_bInterfaceNumber = value;
            }
        }

        [Browsable(false)]
        public byte bNumEndpoints
        {
            get
            {
                return m_bNumEndpoints;
            }
            set
            {
                m_bNumEndpoints = value;
            }
        }

        [Browsable(false)]
        public byte bInterfaceSubClass
        {
            get
            {
               return m_bInterfaceSubClass;
            }
            set
            {
                m_bInterfaceSubClass = value;
            }
        }

        //Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Browsable(false)]
        public byte bInterfaceProtocol
        {
            get
            {
                return m_bInterfaceProtocol;
            }
            set
            {
                m_bInterfaceProtocol = value;
            }
        }

        [Browsable(false)]
        public byte iInterface
        {
            get
            {
                return m_iInterface;
            }
            set
            {
                m_iInterface = value;
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iInterface")]
        public string sInterface
        {
            get { return m_sInterface; }
            set { m_sInterface = value; }
        }

        [Browsable(false)]
        [XmlIgnore]
        public string InterfaceDisplayName
        {
            get { return m_interfaceDisplayName; }
            set { m_interfaceDisplayName = value; }
        }

        #endregion Properties
    }

    [XmlType("InterfaceGeneralDescriptor")]
    public class CyInterfaceGeneralDescriptor : CyUSBDescriptor
    {
        private string m_displayName;
        public string DisplayName
        {
            get { return m_displayName; }
            set { m_displayName = value; }
        }

        public CyInterfaceGeneralDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.ALTERNATE;
            bLength = 0;
            DisplayName = string.Empty;
        }

        public CyInterfaceGeneralDescriptor(string displayName) : this()
        {
            DisplayName = displayName;
        }

        public override int GetLevel()
        {
            return 0;
        }

        public override string ToString()
        {
            string res = String.IsNullOrEmpty(DisplayName) ? "Interface Descriptor" : DisplayName;
            return res;
        }
    }

    [XmlType("StringGenerationType")]
    public enum CyStringGenerationType { USER_ENTERED_TEXT, USER_CALL_BACK, SILICON_NUMBER } ;

    [XmlType("StringDescriptor")]
    public class CyStringDescriptor : CyUSBDescriptor
    {
        private string m_bString;
        private bool m_bUsed = false;
        public CyStringGenerationType snType; 

        public CyStringDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.STRING;
            bLength = 2; 
        }

        public override int GetLevel()
        {
            return 10;
        }

        #region Properties       
        public string bString
        {
            get
            {
                return m_bString;
            }
            set
            {
                m_bString = value;
                if (m_bString != null)
                    bLength = (byte)(2 + m_bString.Length * 2);
                else bLength = 2;
            }
        }

        public bool bUsed
        {
            get { return m_bUsed; } 
            set { m_bUsed = value; } 
        }

        public override string ToString()
        {
            return m_bString;
        }

        #endregion Properties
    }

    [XmlType("StringZeroDescriptor")]
    public class CyStringZeroDescriptor : CyUSBDescriptor
    {
        private UInt16 m_wLANGID = 0x0409;

        public CyStringZeroDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.STRING;
            bLength = 4; 
        }

        public override int GetLevel()
        {
            return 0;
        }

        #region Properties
        public UInt16 wLANGID
        {
            get
            {
                return m_wLANGID;
            }
            set
            {
                m_wLANGID = value;
            }
        }
        #endregion Properties
    }

    [XmlType("HIDDescriptor")]
    public class CyHIDDescriptor : CyUSBDescriptor
    {
        private ushort m_bcdHID;
        private byte m_bCountryCode;
        private byte m_bNumDescriptors;
        private byte m_bDescriptorType1 = 0x22;
        private ushort m_wDescriptorLength;
        public byte bReportIndex;
        public uint wReportIndex; // absolute index (for internal usage)

        public CyHIDDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.HID;
            bLength = 9; 
        }

        public override int GetLevel()
        {
            return 4;
        }

        public override string ToString()
        {
            return "HID Class Descriptor";
        }

        #region Properties
        public ushort bcdHID
        {
            get
            {
                return m_bcdHID;
            }
            set
            {
                m_bcdHID = value;
            }
        }

        public byte bCountryCode
        {
            get
            {
                return m_bCountryCode;
            }
            set
            {
                m_bCountryCode = value;
            }
        }

        public byte bNumDescriptors
        {
            get
            {
                return m_bNumDescriptors;
            }
            set
            {
                m_bNumDescriptors = value;
            }
        }

        public byte bDescriptorType1
        {
            get
            {
                return m_bDescriptorType1;
            }
            set
            {
                m_bDescriptorType1 = value;
            }
        }

        public ushort wDescriptorLength
        {
            get
            {
                return m_wDescriptorLength;
            }
            set
            {
                m_wDescriptorLength = value;
            }
        }
        #endregion Properties
    }

    [XmlType("HIDReportDescriptor")]
    public class CyHIDReportDescriptor : CyUSBDescriptor
    {
        public string Name;
        public ushort wLength = 0;

        public CyHIDReportDescriptor()
            : this("HID Report Descriptor")
        {
        }

        public CyHIDReportDescriptor(string name)
        {
            bDescriptorType = CyUSBDescriptorType.HID_REPORT;
            bLength = 2; 
            Name = name;
        }

        public override int GetLevel()
        {
            return 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [XmlType("HIDReportItemDescriptor")]
    public class CyHIDReportItemDescriptor : CyUSBDescriptor
    {

        public CyHidReportItem Item;

        public CyHIDReportItemDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.HID_REPORT_ITEM;
            bLength = 1; 
        }

        public override int GetLevel()
        {
            return 0;
        }

        public override string ToString()
        {
            string res;
            if (Item != null)
            {
                if (Item.Kind == CyHidReportItemKind.CUSTOM)
                {
                    res = Item.ToCustomString();
                }
                else if (CyUSBFSParameters.StringIsComment(Item.Name))
                    res = Item.Name;
                else
                    res = Item.ToString();
            }
            else
            {
                res = "Undefined";
            }
            return res;
        }
    }

    //---------------------------------------------------------------------------------------------------------------

    [XmlType("USBDescriptorType")]
    public enum CyUSBDescriptorType
    {
        DEVICE = 1,
        CONFIGURATION = 2,
        STRING = 3,
        INTERFACE = 4,
        ENDPOINT = 5,
        DEVICE_QUALIFIER = 6,
        OTHER_SPEED_CONFIGURATION = 7,
        INTERFACE_POWER = 8,
        OTG = 9,
        DEBUG = 10,
        INTERFACE_ASSOCIATION = 11,
        
        ALTERNATE = 0xE0,

        HID = 0x21,
        AUDIO = 0x24,
        CDC = 0x44, // Change to 0x24 (Duplicated value)
        CS_ENDPOINT = 0x25,
        HID_REPORT = 0xF0,
        HID_REPORT_ITEM = 0xF1,

        ALL = 0
    }

    [XmlType("USBOtherTypes")]
    public class CyUSBOtherTypes
    {
        public const string CATEGORY_COMMON = "Common";
        public const string CATEGORY_SPECIFIC = "Specific"; 

        [XmlType("CommunicationSubclassCodes")]
        public enum CyCommunicationSubclassCodes
        {
            [Description("No subclass")]
            SUBCLASS_UNDEFINED = 0x00,
            [Description("Direct Line Control Model")]
            DIRECT_LINE_CONTROL_MODEL = 0x01,
            [Description("Abstract Control Model")]
            ABSTRACT_CONTROL_MODEL = 0x02,
            [Description("Telephone Control Model")]
            TELEPHONE_CONTROL_MODEL = 0x03,
            [Description("Multi-Channel Control Model")]
            MULTI_CHANNEL_CONTROL_MODEL = 0x04,
            [Description("CAPI Control Model")]
            CAPI_CONTROL_MODEL = 0x05,
            [Description("Ethernet Networking Control Model")]
            ETHERNET_NETWORKING_CONTROL_MODEL = 0x06,
            [Description("ATM Networking Control Model")]
            ATM_NETWORKING_CONTROL_MODEL = 0x07,
            [Description("Wireless Handset Control Model")]
            WIRELESS_HANDSET_CONTROL_MODEL = 0x08,
            [Description("Device Management")]
            DEVICE_MANAGEMENT = 0x09,
            [Description("Mobile Direct Line Model")]
            MOBILE_DIRECT_LINE_MODEL = 0x0A,
            [Description("OBEX")]
            OBEX = 0x0B,
            [Description("Ethernet Emulation Model")]
            ETHERNET_EMULATION_MODEL = 0x0C,
            [Description("Network Control Model")]
            NETWORK_CONTROL_MODEL = 0x0D
        }

        [XmlType("CommunicationProtocolCodes")]
        public enum CyCommunicationProtocolCodes
        {
            [Description("No protocol")]
            NO_PROTOCOL = 0x00,
            [Description("V.250 etc")]
            V250 = 0x01,
            [Description("PCCA-101")]
            PCCA101 = 0x02,
            [Description("PCCA-101 & Annex O")]
            PCCA101_ANNEXO = 0x03,
            [Description("GSM 07.07")]
            GSM0707 = 0x04,
            [Description("3GPP 27.007")]
            GPP27007 = 0x05,
            [Description("TIA for CDMA")]
            TIA_CMDA = 0x06,
            [Description("Ethernet Emulation Model")]
            ETHERNET_EMULATION_MODEL = 0x07,
            [Description("External Protocol")]
            EXTERNAL = 0xFE,
            [Description("Vendor-specific")]
            VENDOR_SPEC = 0xFF
        }

        [XmlType("DataProtocolCodes")]
        public enum CyDataProtocolCodes
        {
            [Description("No protocol")]
            NO_PROTOCOL = 0x00,
            [Description("Network Transfer Block")]
            NETWORK_TRANSFER_BLOCK = 0x01,
            [Description("ISDN BRI")]
            ISDN_BRI = 0x30,
            [Description("HDLC")]
            HDLC = 0x31,
            [Description("Transparent")]
            TRANSPARENT = 0x32,
            [Description("Management protocol for Q.921")]
            MGMTQ921 = 0x50,
            [Description("Data link protocol for Q.931")]
            Q931 = 0x51,
            [Description("TEI-multiplexor for Q.921")]
            TEI_Q921 = 0x52,
            [Description("Data compression procedures")]
            DATA_COMPRESSION = 0x90,
            [Description("Euro-ISDN protocol control")]
            EURO_ISDN = 0x91,
            [Description("V.24 rate adaptation to ISDN")]
            V24_ISDN= 0x92,
            [Description("CAPI Commands")]
            CAPI_COMMANDS = 0x93,
            [Description("Host based driver")]
            HOST_BASED_DRIVER = 0xFD,
            [Description("Described using a Protocol Unit Functional Descriptors")]
            UNIT_DESC = 0xFE,
            [Description("Vendor-specific")]
            VENDOR_SPEC = 0xFF
        }


        [XmlType("CommunicationClassSubtypeCodes")]
        public enum CyCommunicationClassSubtypeCodes
        {
            HEADER = 0x00,
            CALL_MANAGEMENT = 0x01,
            ABSTRACT_CONTROL_MANAGEMENT = 0x02,
            DIRECT_LINE_MANAGEMENT = 0x03,
            TELEPHONE_RINGER = 0x04,
            TELEPHONE_CALL = 0x05,
            UNION = 0x06,
            COUNTRY_SELECTION = 0x07,
            TELEPHONE_OPERATIONAL_MODES = 0x08,
            USB_TERMINAL = 0x09,
            NETWORK_CHANNEL_Terminal = 0x0A,
            PROTOCOL_UNIT = 0x0B,
            EXTENSION_UNIT = 0x0C,
            MULTICHANNEL_MANAGEMENT = 0x0D,
            CAPI_CONTROL_MANAGEMENT = 0x0E,
            ETHERNET_NETWORKING = 0x0F,
            ATM_NETWORKING = 0x10,
            WIRELESS_HANDSET_CONTROL_MODEL = 0x11,
            MOBILE_DIRECT_LINE_MODEL = 0x12,
            MDLM_DETAIL = 0x13,
            DEVICE_MANAGEMENT_MODEL = 0x14,
            OBEX = 0x15,
            COMMAND_SET = 0x16,
            COMMAND_SET_DETAIL = 0x17,
            TELEPHONE_CONTROL_MODEL = 0x18,
            OBEX_SERVICE_IDENTIFIER = 0x19,
            NCM = 0x1A,
        }

        [XmlType("AudioSubclassCodes")]
        public enum CyAudioSubclassCodes
        {
            SUBCLASS_UNDEFINED = 0x00,
            AUDIOCONTROL = 0x01,
            AUDIOSTREAMING = 0x02,
            MIDISTREAMING = 0x03
        }

        [XmlType("AudioClassSpecDescriptorTypes")]
        public enum CyAudioClassSpecDescriptorTypes
        {
            CS_UNDEFINED = 0x20,
            CS_DEVICE = 0x21,
            CS_CONFIGURATION = 0x22,
            CS_STRING = 0x23,
            CS_INTERFACE = 0x24,
            CS_ENDPOINT = 0x25
        }

        [XmlType("AudioClassSpecEPDescriptorTypes")]
        public enum CyAudioClassSpecEPDescriptorTypes
        {
            DESCRIPTOR_UNDEFINED = 0x00,
            EP_GENERAL = 0x01,
        }

        [XmlType("ACInterfaceDescriptorSubtypes")]
        public enum CyACInterfaceDescriptorSubtypes
        {
            AC_DESCRIPTOR_UNDEFINED = 0x00,
            HEADER = 0x01,
            INPUT_TERMINAL = 0x02,
            OUTPUT_TERMINAL = 0x03,
            MIXER_UNIT = 0x04,
            SELECTOR_UNIT = 0x05,
            FEATURE_UNIT = 0x06,
            PROCESSING_UNIT = 0x07,
            EXTENSION_UNIT = 0x08
        }

        [XmlType("ACInterfaceDescriptorSubtypes_v2")]
        public enum CyACInterfaceDescriptorSubtypes_v2
        {
            AC_DESCRIPTOR_UNDEFINED = 0x00,
            HEADER = 0x01,
            INPUT_TERMINAL = 0x02,
            OUTPUT_TERMINAL = 0x03,
            MIXER_UNIT = 0x04,
            SELECTOR_UNIT = 0x05,
            FEATURE_UNIT = 0x06,
            EFFECT_UNIT = 0x07,
            PROCESSING_UNIT = 0x08,
            EXTENSION_UNIT = 0x09,
            CLOCK_SOURCE = 0x0A,
            CLOCK_SELECTOR = 0x0B,
            CLOCK_MULTIPLIER = 0x0C,
            SAMPLE_RATE_CONVERTER = 0x0D
        }

        [XmlType("ASInterfaceDescriptorSubtypes")]
        public enum CyASInterfaceDescriptorSubtypes
        {
            AS_DESCRIPTOR_UNDEFINED = 0x00,
            AS_GENERAL = 0x01,
            FORMAT_TYPE = 0x02,
            FORMAT_SPECIFIC = 0x03
        }

        [XmlType("FormatTypeCodes")]
        public enum CyFormatTypeCodes
        {
            FORMAT_TYPE_UNDEFINED = 0x00,
            FORMAT_TYPE_1 = 0x01,
            FORMAT_TYPE_2 = 0x02,
            FORMAT_TYPE_3 = 0x03
        }

        public enum CyEndptTransferTypes
        {
            [Description("CONT")]
            TRANSFERTYPE_CONTROL = 0x00,
            [Description("INT")]
            TRANSFERTYPE_INTERRUPT = 0x03,
            [Description("BULK")]
            TRANSFERTYPE_BULK = 0x02,
            [Description("ISOC")]
            TRANSFERTYPE_ISOCHRONOUS = 0x01
        }

        public enum CyEndptDirections
        {
            [Description("IN")]
            IN = 0x01,
            [Description("OUT")]
            OUT = 0x00
        }

        public enum CyEndptNumbers
        {
            [Description("EP1")]
            EP1 = 0x01,
            [Description("EP2")]
            EP2 = 0x02,
            [Description("EP3")]
            EP3 = 0x03,
            [Description("EP4")]
            EP4 = 0x04,
            [Description("EP5")]
            EP5 = 0x05,
            [Description("EP6")]
            EP6 = 0x06,
            [Description("EP7")]
            EP7 = 0x07,
            [Description("EP8")]
            EP8 = 0x08
        }

        public enum CyEffectUnitEffectTypes
        {
            [Description("EFFECT_UNDEFINED")]
            EFFECT_UNDEFINED = 0x00,
            [Description("PARAM_EQ_SECTION_EFFECT")]
            PARAM_EQ_SECTION_EFFECT = 0x01,
            [Description("REVERBERATION_EFFECT")]
            REVERBERATION_EFFECT = 0x02,
            [Description("MOD_DELAY_EFFECT")]
            MOD_DELAY_EFFECT = 0x03,
            [Description("DYN_RANGE_COMP_EFFECT")]
            DYN_RANGE_COMP_EFFECT = 0x04
        }
    }

    public static class CyHIDReportItemTables
    {
        #region Constructor
        
        static CyHIDReportItemTables()
        {
            const string VENDOR_STR = "Custom";
            const string VENDOR_VAL = "FF00";
            const int PAGE_LIST_LENGTH = 64;
            // Fill ValuesButtonPage Table
            ValuesButtonPage = new string[PAGE_LIST_LENGTH * 2 + 2];
            ValuesButtonPage[0] = "No button pressed";
            ValuesButtonPage[1] = "0";
            for (int i = 1; i < PAGE_LIST_LENGTH; i++)
            {
                ValuesButtonPage[i*2] = "Button " + i;
                ValuesButtonPage[i*2 + 1] = i.ToString("X");
            }
            ValuesButtonPage[PAGE_LIST_LENGTH * 2] = VENDOR_STR;
            ValuesButtonPage[PAGE_LIST_LENGTH * 2 + 1] = VENDOR_VAL;

            // Fill ValuesOrdinalPage Table
            ValuesOrdinalPage = new string[PAGE_LIST_LENGTH * 2 + 2];
            for (int i = 0; i < PAGE_LIST_LENGTH; i++)
            {
                ValuesOrdinalPage[i * 2] = "Instance " + (i+1);
                ValuesOrdinalPage[i * 2 + 1] = (i+1).ToString("X");
            }
            ValuesOrdinalPage[PAGE_LIST_LENGTH * 2] = VENDOR_STR;
            ValuesOrdinalPage[PAGE_LIST_LENGTH * 2 + 1] = VENDOR_VAL;

            // Fill ValuesUnicodePage Table
            ValuesUnicodePage = new string[PAGE_LIST_LENGTH * 2 + 2];
            for (int i = 0; i < PAGE_LIST_LENGTH; i++)
            {
                ValuesUnicodePage[i * 2] = "Unicode Char " + i;
                ValuesUnicodePage[i * 2 + 1] = i.ToString("X");
            }
            ValuesUnicodePage[PAGE_LIST_LENGTH * 2] = VENDOR_STR;
            ValuesUnicodePage[PAGE_LIST_LENGTH * 2 + 1] = VENDOR_VAL;

            // Fill ValuesMonitorEnumeratedValuesPage Table
            ValuesMonitorEnumeratedValuesPage = new string[PAGE_LIST_LENGTH * 2 + 2];
            for (int i = 0; i < PAGE_LIST_LENGTH; i++)
            {
                ValuesMonitorEnumeratedValuesPage[i * 2] = "ENUM " + i;
                ValuesMonitorEnumeratedValuesPage[i * 2 + 1] = i.ToString("X");
            }
            ValuesMonitorEnumeratedValuesPage[PAGE_LIST_LENGTH * 2] = VENDOR_STR;
            ValuesMonitorEnumeratedValuesPage[PAGE_LIST_LENGTH * 2 + 1] = VENDOR_VAL;
        }
        
        #endregion Constructor

        #region Constants

        public static readonly string[] ItemsName = {
                                        "USAGE",
                                        "USAGE_PAGE",
                                        "USAGE_MINIMUM",
                                        "USAGE_MAXIMUM",
                                        "DESIGNATOR_INDEX",
                                        "DESIGNATOR_MINIMUM",
                                        "DESIGNATOR_MAXIMUM",
                                        "STRING_INDEX",
                                        "STRING_MINIMUM",
                                        "STRING_MAXIMUM",
                                        "COLLECTION",
                                        "END_COLLECTION",
                                        "INPUT",
                                        "OUTPUT",
                                        "FEATURE",
                                        "LOGICAL_MINIMUM",
                                        "LOGICAL_MAXIMUM",
                                        "PHYSICAL_MINIMUM",
                                        "PHYSICAL_MAXIMUM",
                                        "UNIT_EXPONENT",
                                        "UNIT",
                                        "REPORT_SIZE",
                                        "REPORT_ID",
                                        "REPORT_COUNT",
                                        "PUSH",
                                        "POP",
                                        "DELIMITER"
                                    };

        public static readonly string[] ValuesInput = {
                                                 "Data", "0", "Constant", "1",
                                                 "Array", "2", "Variable", "3",
                                                 "Absolute", "4", "Relative", "5",
                                                 "No Wrap", "6", "Wrap", "7",
                                                 "Linear", "8", "Non Linear", "9",
                                                 "Preferred State", "A", "No Preferred", "B",
                                                 "No Null Position", "C", "Null State", "D",
                                                 "", "E", "", "F",
                                                 "Bit Field", "10", "Buffered Bytes", "11"
                                             };

        public static readonly string[] ValuesCollection = {
                                                      "Physical", "0",
                                                      "Application", "1",
                                                      "Logical", "2",
                                                      "Report", "3",
                                                      "Named Array", "4",
                                                      "Usage Switch", "5",
                                                      "Usage Modifier", "6",
                                                      "Vendor-defined", "80",
                                                  };

        public static readonly string[] ValuesDelimiter = {
                                                     "Open", "1",
                                                     "Close", "0"
                                                 };

        public static readonly string[] ValuesUnitExponent = {
                                                        "0", "0",
                                                        "1", "1",
                                                        "2", "2",
                                                        "3", "3",
                                                        "4", "4",
                                                        "5", "5",
                                                        "6", "6",
                                                        "7", "7",
                                                        "-8", "8",
                                                        "-7", "9",
                                                        "-6", "A",
                                                        "-5", "B",
                                                        "-4", "C",
                                                        "-3", "D",
                                                        "-2", "E",
                                                        "-1", "F"
                                                    };

        public static readonly string[] ValuesUsagePage = {
                                                     "Generic Desktop Controls", "01", //+-
                                                     "Simulation Controls", "02",
                                                     "VR Controls", "03",
                                                     "Sport Controls", "04",
                                                     "Game Controls", "05",
                                                     "Generic Device Controls", "06",
                                                     "Keyboard/Keypad", "07",
                                                     "LEDs", "08",
                                                     "Button", "09",
                                                     "Ordinal", "0A",
                                                     "Telephony Devices", "0B",
                                                     "Consumer Devices", "0C",
                                                     "Digitizer", "0D",
                                                     "Physical Input Device (PID)", "0F",
                                                     "Unicode", "10",
                                                     "Alphanumeric Display", "14",
                                                     "Medical Instruments", "40",
                                                     "Monitor Devices", "80",
                                                     "Monitor Enumerated Values", "81",
                                                     "VESA Virtual Controls", "82",
                                                     //"VESA Command", "83", //?
                                                     "Power Device", "84", 
                                                     "Battery System", "85",
                                                     "Vendor Defined Page", "FF00"
                                                 };

        public static readonly string[] ValuesGenericDesktopPage = {
                                                              "Undefined", "0",
                                                              "Pointer", "1",
                                                              "Mouse", "2",
                                                              "Joystick", "4",
                                                              "Game Pad", "5",
                                                              "Keyboard", "6",
                                                              "Keypad", "7",
                                                              "X", "30",
                                                              "Y", "31",
                                                              "Z", "32",
                                                              "Rx", "33",
                                                              "Ry", "34",
                                                              "Rz", "35",
                                                              "Slider", "36",
                                                              "Dial", "37",
                                                              "Wheel", "38",
                                                              "Hat switch", "39",
                                                              "Counted Buffer", "3A",
                                                              "Byte Count", "3B",
                                                              "Motion Wakeup", "3C",
                                                              "Start", "3D",
                                                              "Select", "3E",
                                                              "Vx", "40",
                                                              "Vy", "41",
                                                              "Vz", "42",
                                                              "Vbrx", "43",
                                                              "Vbry", "44",
                                                              "Vbrz", "45",
                                                              "Vno", "46",
                                                              "System Control", "80",
                                                              "System Power Down", "81",
                                                              "System Sleep", "82",
                                                              "System Wake Up", "83",
                                                              "System Context Menu", "84",
                                                              "System Main Menu", "85",
                                                              "System App Menu", "86",
                                                              "System Menu Help", "87",
                                                              "System Menu Exit", "88",
                                                              "System Menu Select", "89",
                                                              "System Menu Right", "8A",
                                                              "System Menu Left", "8B",
                                                              "System Menu Up", "8C",
                                                              "System Menu Down", "8D",
                                                              "Custom", "FF00"
                                                          };

        public static readonly string[] ValuesSimulationControlsPage = {
                                                                  "Undefined", "0",
                                                                  "Flight Simulation Device", "01",
                                                                  "Automobile Simulation Device", "02",
                                                                  "Tank Simulation Device", "03",
                                                                  "Spaceship Simulation Device", "04",
                                                                  "Submarine Simulation Device", "05",
                                                                  "Sailing Simulation Device", "06",
                                                                  "Motorcycle Simulation Device", "07",
                                                                  "Sports Simulation Device", "08",
                                                                  "Airplane Simulation Device", "09",
                                                                  "Helicopter Simulation Device", "0A",
                                                                  "Magic Carpet Simulation Device", "0B",
                                                                  "Bicycle Simulation Device", "0C",
                                                                  "Flight Control Stick", "20",
                                                                  "Flight Stick", "21",
                                                                  "Cyclic Control", "22",
                                                                  "Cyclic Trim", "23",
                                                                  "Flight Yoke", "24",
                                                                  "Track Control", "25",
                                                                  "Aileron", "B0",
                                                                  "Aileron Trim", "B1",
                                                                  "Anti-Torque Control", "B2",
                                                                  "Autopilot Enable", "B3",
                                                                  "Chaff Release", "B4",
                                                                  "Collective Control", "B5",
                                                                  "Dive Brake", "B6",
                                                                  "Electronic Countermeasures", "B7",
                                                                  "Elevator", "B8",
                                                                  "Elevator Trim", "B9",
                                                                  "Rudder", "BA",
                                                                  "Throttle", "BB",
                                                                  "Flight Communications", "BC",
                                                                  "Flare Release", "BD",
                                                                  "Landing Gear", "BE",
                                                                  "Toe Brake", "BF",
                                                                  "Trigger", "C0",
                                                                  "Weapons Arm", "C1",
                                                                  "Weapons Select", "C2",
                                                                  "Wing Flaps", "C3",
                                                                  "Accelerator", "C4",
                                                                  "Brake", "C5",
                                                                  "Clutch", "C6",
                                                                  "Shifter", "C7",
                                                                  "Steering", "C8",
                                                                  "Turret Direction", "C9",
                                                                  "Barrel Elevation", "CA",
                                                                  "Dive Plane", "CB",
                                                                  "Ballast", "CC",
                                                                  "Bicycle Crank", "CD",
                                                                  "Handle Bars", "CE",
                                                                  "Front Brake", "CF",
                                                                  "Rear Brake", "D0",
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesVRControlPage = {
                                                "Undefined","0",
                                                "Belt","1",
                                                "Body Suit","2",
                                                "Flexor","3",
                                                "Glove","4",
                                                "Head Tracker","5",
                                                "Head Mounted Display","6",
                                                "Hand Tracker","7",
                                                "Oculometer","8",
                                                "Vest","9",
                                                "Animatronic Device","A",
                                                "Stereo Enable","20",
                                                "Display Enable","21",
                                                "Custom", "FF00"
                                            };

        public static readonly string[] ValuesSportControlPage = {
                                                "Undefined","0",
                                                "Baseball Bat","1",
                                                "Golf Club","2",
                                                "Rowing Machine","3",
                                                "Treadmill","4",
                                                "Oar","30",
                                                "Slope","31",
                                                "Rate","32",
                                                "Stick Speed","33",
                                                "Stick Face Angle","34",
                                                "Stick Heel/Toe","35",
                                                "Stick Follow Through","36",
                                                "Stick Tempo","37",
                                                "Stick Type","38",
                                                "Stick Height","39",
                                                "Putter","50",
                                                "1 Iron","51",
                                                "2 Iron","52",
                                                "3 Iron","53",
                                                "4 Iron","54",
                                                "5 Iron","55",
                                                "6 Iron","56",
                                                "7 Iron","57",
                                                "8 Iron","58",
                                                "9 Iron","59",
                                                "10 Iron","5A",
                                                "11 Iron","5B",
                                                "Sand Wedge","5C",
                                                "Loft Wedge","5D",
                                                "Power Wedge","5E",
                                                "1 Wood","5F",
                                                "3 Wood","60",
                                                "5 Wood","61",
                                                "7 Wood","62",
                                                "9 Wood","63",
                                                "Custom", "FF00"
                                            };
        public static readonly string[] ValuesGameControlsPage = {
                                                                  "Undefined", "0",
                                                                  "3D Game Controller", "1",
                                                                  "Pinball Device", "2",
                                                                  "Gun Device", "3",
                                                                  "Point of View", "20",
                                                                  "Turn Right/Left", "21",
                                                                  "Pitch Forward/Backward", "22",
                                                                  "Roll Right/Left", "23",
                                                                  "Move Right/Left", "24",
                                                                  "Move Forward/Backward", "25",
                                                                  "Move Up/Down", "26",
                                                                  "Lean Right/Left", "27",
                                                                  "Lean Forward/Backward", "28",
                                                                  "Height of POV", "29",
                                                                  "Flipper", "2A",
                                                                  "Secondary Flipper", "2B",
                                                                  "Bump", "2C",
                                                                  "New Game", "2D",
                                                                  "Shoot Ball", "2E",
                                                                  "Player", "2F",
                                                                  "Gun Bolt", "30",
                                                                  "Gun Clip", "31",
                                                                  "Gun Selector", "32",
                                                                  "Gun Single Shot", "33",
                                                                  "Gun Burst", "34",
                                                                  "Gun Automatic", "35",
                                                                  "Gun Safety", "36",
                                                                  "Gamepad Fire/Jump", "37",
                                                                  "Gamepad Trigger", "39",
                                                                  "Custom", "FF00"
                                                              };
        public static readonly string[] ValuesGenericDeviceControlsPage = {
                                                                  "Undefined", "0",
                                                                  "Battery Strength", "20",
                                                                  "Wireless Channel", "21",
                                                                  "Wireless ID", "22",
                                                                  "Discover Wireless Control", "23",
                                                                  "Security Code Character Entered", "24",
                                                                  "Security Code Character Erased", "25",
                                                                  "Security Code Cleared", "26",
                                                                  "Custom", "FF00"
                                                              };
        public static readonly string[] ValuesKeyboardPage = {
                                                                "Undefined", "0",
                                                                "Keyboard ErrorRollOver",     "01", 
                                                                "Keyboard POSTFail",          "02",
                                                                "Keyboard ErrorUndefined",    "03",
                                                                "Keyboard a and A",          "04",
                                                                "Keyboard b and B",          "05",
                                                                "Keyboard c and C",          "06",
                                                                "Keyboard d and D",          "07",
                                                                "Keyboard e and E",          "08",
                                                                "Keyboard f and F",          "09",
                                                                "Keyboard g and G",          "0A",
                                                                "Keyboard h and H",          "0B",
                                                                "Keyboard i and I",          "0C",
                                                                "Keyboard j and J",          "0D",
                                                                "Keyboard k and K",          "0E",
                                                                "Keyboard l and L",          "0F",
                                                                "Keyboard m and M",          "10",
                                                                "Keyboard n and N",          "11",
                                                                "Keyboard o and O",          "12",
                                                                "Keyboard p and P",          "13",
                                                                "Keyboard q and Q",          "14",
                                                                "Keyboard r and R",          "15",
                                                                "Keyboard s and S",          "16",
                                                                "Keyboard t and T",          "17",
                                                                "Keyboard u and U",          "18",
                                                                "Keyboard v and V",          "19",
                                                                "Keyboard w and W",          "1A",
                                                                "Keyboard x and X",          "1B",
                                                                "Keyboard y and Y",          "1C",
                                                                "Keyboard z and Z",          "1D",
                                                                "Keyboard 1 and !",          "1E",
                                                                "Keyboard 2 and @",          "1F",
                                                                "Keyboard 3 and #",          "20",
                                                                "Keyboard 4 and $",          "21",
                                                                "Keyboard 5 and %",          "22",
                                                                "Keyboard 6 and ^",          "23",
                                                                "Keyboard 7 and &",          "24",
                                                                "Keyboard 8 and *",          "25",
                                                                "Keyboard 9 and (",          "26",
                                                                "Keyboard 0 and )",          "27",
                                                                "Keyboard Return (ENTER)",        "28",
                                                                "Keyboard ESCAPE",                "29",
                                                                "Keyboard DELETE (Backspace)",    "2A",
                                                                "Keyboard Tab",                   "2B",
                                                                "Keyboard Spacebar",              "2C",
                                                                "Keyboard - and (underscore)",    "2D",
                                                                "Keyboard = and +",               "2E",
                                                                "Keyboard [ and {",               "2F",
                                                                "Keyboard ] and }",               "30",
                                                                @"Keyboard \ and |",              "31",
                                                                "Keyboard Non-US # and ~",        "32",
                                                                "Keyboard ; and :",               "33",
                                                                "Keyboard ' and \"",              "34",
                                                                "Keyboard Grave Accent and Tilde","35",
                                                                "Keyboard, and < ",            "36",
                                                                "Keyboard . and >",               "37",
                                                                "Keyboard / and ?",               "38",
                                                                "Keyboard Caps Lock",             "39",
                                                                "Keyboard F1",                    "3A",
                                                                "Keyboard F2",                    "3B",
                                                                "Keyboard F3",                    "3C",
                                                                "Keyboard F4",                    "3D",
                                                                "Keyboard F5",                    "3E",
                                                                "Keyboard F6",                    "3F",
                                                                "Keyboard F7",                    "40",
                                                                "Keyboard F8",                    "41",
                                                                "Keyboard F9",                    "42",
                                                                "Keyboard F10",                   "43",
                                                                "Keyboard F11",                   "44",
                                                                "Keyboard F12",                   "45",
                                                                "Keyboard PrintScreen",           "46",
                                                                "Keyboard Scroll Lock",           "47",
                                                                "Keyboard Pause",                 "48",
                                                                "Keyboard Insert",                "49",
                                                                "Keyboard Home",                  "4A",
                                                                "Keyboard PageUp",                "4B",
                                                                "Keyboard Delete Forward",        "4C",
                                                                "Keyboard End",                   "4D",
                                                                "Keyboard PageDown",              "4E",
                                                                "Keyboard RightArrow",            "4F",
                                                                "Keyboard LeftArrow",             "50",
                                                                "Keyboard DownArrow",             "51",
                                                                "Keyboard UpArrow",               "52",
                                                                "Keypad Num Lock and Clear",      "53",
                                                                "Keypad /",                      "54",
                                                                "Keypad *",                       "55",
                                                                "Keypad -",                       "56",
                                                                "Keypad +",                       "57",
                                                                "Keypad ENTER",                   "58",
                                                                "Keypad 1 and End", "59",
                                                                "Keypad 2 and Down Arrow", "5A",
                                                                "Keypad 3 and PageDn", "5B",
                                                                "Keypad 4 and Left Arrow", "5C",
                                                                "Keypad 5", "5D",
                                                                "Keypad 6 and Right Arrow", "5E",
                                                                "Keypad 7 and Home", "5F",
                                                                "Keypad 8 and Up Arrow", "60",
                                                                "Keypad 9 and PageUp", "61",
                                                                "Keypad 0 and Insert", "62",
                                                                "Keypad . and Delete",  "63",
                                                                @"Keyboard Non-US \ and |", "64",
                                                                "Keyboard Application",           "65",
                                                                "Keyboard Power",                 "66",
                                                                "Keypad =",                       "67",
                                                                "Keyboard F13",                   "68",
                                                                "Keyboard F14",                   "69",
                                                                "Keyboard F15",                   "6A",
                                                                "Keyboard F16",                   "6B",
                                                                "Keyboard F17",                   "6C",
                                                                "Keyboard F18",                   "6D",
                                                                "Keyboard F19",                   "6E",
                                                                "Keyboard F20",                   "6F",
                                                                "Keyboard F21",                   "70",
                                                                "Keyboard F22",                   "71",
                                                                "Keyboard F23",                   "72",
                                                                "Keyboard F24",                   "73",
                                                                "Keyboard Execute",               "74",
                                                                "Keyboard Help",                  "75",
                                                                "Keyboard Menu",                  "76",
                                                                "Keyboard Select",                "77",
                                                                "Keyboard Stop",                  "78",
                                                                "Keyboard Again",                 "79",
                                                                "Keyboard Undo",                  "7A",
                                                                "Keyboard Cut",                   "7B",
                                                                "Keyboard Copy",                  "7C",
                                                                "Keyboard Paste",                 "7D",
                                                                "Keyboard Find",                  "7E",
                                                                "Keyboard Mute",                  "7F",
                                                                "Keyboard Volume Up",             "80",
                                                                "Keyboard Volume Down",           "81",
                                                                "Keyboard Locking Caps Lock",     "82",
                                                                "Keyboard Locking Num Lock",      "83",
                                                                "Keyboard Locking Scroll Lock",   "84",
                                                                "Keypad Comma",                   "85",
                                                                "Keypad Equal Sign",              "86",
                                                                "Keyboard International1",        "87",
                                                                "Keyboard International2",        "88",
                                                                "Keyboard International3",        "89",
                                                                "Keyboard International4",        "8A",
                                                                "Keyboard International5",        "8B",
                                                                "Keyboard International6",        "8C",
                                                                "Keyboard International7",        "8D",
                                                                "Keyboard International8",        "8E",
                                                                "Keyboard International9",        "8F",
                                                                "Keyboard LANG1",                 "90",
                                                                "Keyboard LANG2",                 "91",
                                                                "Keyboard LANG3",                 "92",
                                                                "Keyboard LANG4",                 "93",
                                                                "Keyboard LANG5",                 "94",
                                                                "Keyboard LANG6",                 "95",
                                                                "Keyboard LANG7",                 "96",
                                                                "Keyboard LANG8",                 "97",
                                                                "Keyboard LANG9",                 "98",
                                                                "Keyboard Alternate Erase",       "99",
                                                                "Keyboard SysReq/Attention",      "9A",
                                                                "Keyboard Cancel",                "9B",
                                                                "Keyboard Clear",                 "9C",
                                                                "Keyboard Prior",                 "9D",
                                                                "Keyboard Return",                "9E",
                                                                "Keyboard Separator",             "9F",
                                                                "Keyboard Out",                   "A0",
                                                                "Keyboard Oper",                  "A1",
                                                                "Keyboard Clear/Again",           "A2",
                                                                "Keyboard CrSel/Props",           "A3",
                                                                "Keyboard ExSel",                 "A4",
                                                                "Keypad 00",                      "B0",
                                                                "Keypad 000",                     "B1",
                                                                "Thousands Separator",            "B2",
                                                                "Decimal Separator",              "B3",
                                                                "Currency Unit",                  "B4",
                                                                "Currency Sub-unit",              "B5",
                                                                "Keypad (",                       "B6",
                                                                "Keypad )",                       "B7",
                                                                "Keypad {",                       "B8",
                                                                "Keypad }",                       "B9",
                                                                "Keypad Tab",                     "BA",
                                                                "Keypad Backspace",               "BB",
                                                                "Keypad A",                       "BC",
                                                                "Keypad B",                       "BD",
                                                                "Keypad C",                       "BE",
                                                                "Keypad D",                       "BF",
                                                                "Keypad E",                       "C0",
                                                                "Keypad F",                       "C1",
                                                                "Keypad XOR",                     "C2",
                                                                "Keypad ^",                       "C3",
                                                                "Keypad %",                       "C4",
                                                                "Keypad <",                       "C5",
                                                                "Keypad >",                       "C6",
                                                                "Keypad &",                       "C7",
                                                                "Keypad &&",                      "C8",
                                                                "Keypad |",                       "C9",
                                                                "Keypad ||",                      "CA",
                                                                "Keypad :",                       "CB",
                                                                "Keypad #",                       "CC",
                                                                "Keypad Space",                   "CD",
                                                                "Keypad @",              "CE",
                                                                "Keypad !",              "CF",
                                                                "Keypad Memory Store",   "D0",
                                                                "Keypad Memory Recall",  "D1",
                                                                "Keypad Memory Clear",   "D2",
                                                                "Keypad Memory Add",     "D3",
                                                                "Keypad Memory Subtract", "D4",
                                                                "Keypad Memory Multiply", "D5",
                                                                "Keypad Memory Divide",  "D6",
                                                                "Keypad +/-",  "D7",
                                                                "Keypad Clear", "D8",
                                                                "Keypad Clear", "D9",
                                                                "Keypad Binary", "DA",
                                                                "Keypad Octal", "DB",
                                                                "Keypad Decimal", "DC",
                                                                "Keypad Hexadecimal", "DD",
                                                                "Keyboard LeftControl", "E0",
                                                                "Keyboard LeftShift", "E1",
                                                                "Keyboard LeftAlt", "E2",
                                                                "Keyboard Left GUI", "E3",
                                                                "Keyboard RightControl", "E4",
                                                                "Keyboard RightShift", "E5",
                                                                "Keyboard RightAlt", "E6",
                                                                "Keyboard Right GUI", "E7",
                                                              };
        public static readonly string[] ValuesLEDPage = {
                                                                  "Undefined", "0",
                                                                  "Num Lock", "1",
                                                                  "Caps Lock", "2",
                                                                  "Scroll Lock", "3",
                                                                  "Compose", "4",
                                                                  "Kana", "5",
                                                                  "Power", "6",
                                                                  "Shift", "7",
                                                                  "Do Not Disturb", "8",
                                                                  "Mute", "9",
                                                                  "Tone Enable", "A",
                                                                  "High Cut Filter", "B",
                                                                  "Low Cut Filter", "C",
                                                                  "Equalizer Enable", "D",
                                                                  "Sound Field On", "E",
                                                                  "Surround On", "F",
                                                                  "Repeat", "10",
                                                                  "Stereo", "11",
                                                                  "Sampling Rate Detect", "12",
                                                                  "Spinning", "13",
                                                                  "CAV", "14",
                                                                  "CLV", "15",
                                                                  "Recording Format Detect", "16",
                                                                  "Off-Hook", "17",
                                                                  "Ring", "18",
                                                                  "Message Waiting", "19",
                                                                  "Data Mode", "1A",
                                                                  "Battery Operation", "1B",
                                                                  "Battery OK", "1C",
                                                                  "Battery Low", "1D",
                                                                  "Speaker", "1E",
                                                                  "Head Set", "1F",
                                                                  "Hold", "20",
                                                                  "Microphone", "21",
                                                                  "Coverage", "22",
                                                                  "Night Mode", "23",
                                                                  "Send Calls", "24",
                                                                  "Call Pickup", "25",
                                                                  "Conference", "26",
                                                                  "Stand-by", "27",
                                                                  "Camera On", "28",
                                                                  "Camera Off", "29",
                                                                  "On-Line", "2A",
                                                                  "Off-Line", "2B",
                                                                  "Busy", "2C",
                                                                  "Ready", "2D",
                                                                  "Paper-Out", "2E",
                                                                  "Paper-Jam", "2F",
                                                                  "Remote", "30",
                                                                  "Forward", "31",
                                                                  "Reverse", "32",
                                                                  "Stop", "33",
                                                                  "Rewind", "34",
                                                                  "Fast Forward", "35",
                                                                  "Play", "36",
                                                                  "Pause", "37",
                                                                  "Record", "38",
                                                                  "Error", "39",
                                                                  "Usage Selected Indicator", "3A",
                                                                  "Usage In Use Indicator", "3B",
                                                                  "Usage Multi Mode Indicator", "3C",
                                                                  "Indicator On", "3D",
                                                                  "Indicator Flash", "3E",
                                                                  "Indicator Slow Blink", "3F",
                                                                  "Indicator Fast Blink", "40",
                                                                  "Indicator Off", "41",
                                                                  "Flash On Time", "42",
                                                                  "Slow Blink On Time", "43",
                                                                  "Slow Blink Off Time", "44",
                                                                  "Fast Blink On Time", "45",
                                                                  "Fast Blink Off Time", "46",
                                                                  "Usage Indicator Color", "47",
                                                                  "Indicator Red", "48",
                                                                  "Indicator Green", "49",
                                                                  "Indicator Amber", "4A",
                                                                  "Generic Indicator", "4B",
                                                                  "System Suspend", "4C",
                                                                  "External Power Connected", "4D",
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesButtonPage;  // Filled in Constructor
        public static readonly string[] ValuesOrdinalPage;  // Filled in Constructor

        public static readonly string[] ValuesTelephonyDevicePage = {
                                                                  "Undefined", "0",
                                                                  "Phone", "01",
                                                                  "Answering Machine", "02",
                                                                  "Message Controls", "03",
                                                                  "Handset", "04",
                                                                  "Headset", "05",
                                                                  "Telephony Key Pad", "06",
                                                                  "Programmable Button", "07",
                                                                  "Hook Switch", "20",
                                                                  "Flash", "21",
                                                                  "Feature", "22",
                                                                  "Hold", "23",
                                                                  "Redial", "24",
                                                                  "Transfer", "25",
                                                                  "Drop", "26",
                                                                  "Park", "27",
                                                                  "Forward Calls", "28",
                                                                  "Alternate Function", "29",
                                                                  "Line", "2A",
                                                                  "Speaker Phone", "2B",
                                                                  "Conference", "2C",
                                                                  "Ring Enable", "2D",
                                                                  "Ring Select", "2E",
                                                                  "Phone Mute", "2F",
                                                                  "Caller ID", "30",
                                                                  "Send", "31",
                                                                  "Speed Dial", "50",
                                                                  "Store Number", "51",
                                                                  "Recall Number", "52",
                                                                  "Phone Directory", "53",
                                                                  "Voice Mail", "70",
                                                                  "Screen Calls", "71",
                                                                  "Do Not Disturb", "72",
                                                                  "Message", "73",
                                                                  "Answer On/Off", "74",
                                                                  "Inside Dial Tone", "90",
                                                                  "Outside Dial Tone", "91",
                                                                  "Inside Ring Tone", "92",
                                                                  "Outside Ring Tone", "93",
                                                                  "Priority Ring Tone", "94",
                                                                  "Inside Ringback", "95",
                                                                  "Priority Ringback", "96",
                                                                  "Line Busy Tone", "97",
                                                                  "Reorder Tone", "98",
                                                                  "Call Waiting Tone", "99",
                                                                  "Confirmation Tone 1", "9A",
                                                                  "Confirmation Tone 2", "9B",
                                                                  "Tones Off", "9C",
                                                                  "Outside Ringback", "9D",
                                                                  "Ringer", "9E",
                                                                  "Phone Key 0", "B0",
                                                                  "Phone Key 1", "B1",
                                                                  "Phone Key 2", "B2",
                                                                  "Phone Key 3", "B3",
                                                                  "Phone Key 4", "B4",
                                                                  "Phone Key 5", "B5",
                                                                  "Phone Key 6", "B6",
                                                                  "Phone Key 7", "B7",
                                                                  "Phone Key 8", "B8",
                                                                  "Phone Key 9", "B9",
                                                                  "Phone Key Star", "BA",
                                                                  "Phone Key Pound", "BB",
                                                                  "Phone Key A", "BC",
                                                                  "Phone Key B", "BD",
                                                                  "Phone Key C", "BE",
                                                                  "Phone Key D", "BF",
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesConsumerPage = {
                                                                  "Unassigned", "00",
                                                                  "Consumer Control", "01",
                                                                  "Numeric Key Pad", "02",
                                                                  "Programmable Buttons", "03",
                                                                  "Microphone", "04",
                                                                  "Headphone", "05",
                                                                  "Graphic Equalizer", "06",
                                                                  "Reserved", "07",
                                                                  "+10", "20",
                                                                  "+100", "21",
                                                                  "AM/PM", "22",
                                                                  "Power", "30",
                                                                  "Reset", "31",
                                                                  "Sleep", "32",
                                                                  "Sleep After", "33",
                                                                  "Sleep Mode", "34",
                                                                  "Illumination", "35",
                                                                  "Function Buttons", "36",
                                                                  "Menu", "40",
                                                                  "Menu Pick", "41",
                                                                  "Menu Up", "42",
                                                                  "Menu Down", "43",
                                                                  "Menu Left", "44",
                                                                  "Menu Right", "45",
                                                                  "Menu Escape", "46",
                                                                  "Menu Value Increase", "47",
                                                                  "Menu Value Decrease", "48",
                                                                  "Data On Screen", "60",
                                                                  "Closed Caption", "61",
                                                                  "Closed Caption Select", "62",
                                                                  "VCR/TV", "63",
                                                                  "Broadcast Mode", "64",
                                                                  "Snapshot", "65",
                                                                  "Still", "66",
                                                                  "Selection", "80",
                                                                  "Assign Selection", "81",
                                                                  "Mode Step", "82",
                                                                  "Recall Last", "83",
                                                                  "Enter Channel", "84",
                                                                  "Order Movie", "85",
                                                                  "Channel", "86",
                                                                  "Media Selection", "87",
                                                                  "Media Select Computer", "88",
                                                                  "Media Select TV", "89",
                                                                  "Media Select WWW", "8A",
                                                                  "Media Select DVD", "8B",
                                                                  "Media Select Telephone", "8C",
                                                                  "Media Select Program Guide", "8D",
                                                                  "Media Select Video Phone", "8E",
                                                                  "Media Select Games", "8F",
                                                                  "Media Select Messages", "90",
                                                                  "Media Select CD", "91",
                                                                  "Media Select VCR", "92",
                                                                  "Media Select Tuner", "93",
                                                                  "Quit", "94",
                                                                  "Help", "95",
                                                                  "Media Select Tape", "96",
                                                                  "Media Select Cable", "97",
                                                                  "Media Select Satellite", "98",
                                                                  "Media Select Security", "99",
                                                                  "Media Select Home", "9A",
                                                                  "Media Select Call", "9B",
                                                                  "Channel Increment", "9C",
                                                                  "Channel Decrement", "9D",
                                                                  "Media Select SAP", "9E",
                                                                  "VCR Plus", "A0",
                                                                  "Once", "A1",
                                                                  "Daily", "A2",
                                                                  "Weekly", "A3",
                                                                  "Monthly", "A4",
                                                                  "Play", "B0",
                                                                  "Pause", "B1",
                                                                  "Record", "B2",
                                                                  "Fast Forward", "B3",
                                                                  "Rewind", "B4",
                                                                  "Scan Next Track", "B5",
                                                                  "Scan Previous Track", "B6",
                                                                  "Stop", "B7",
                                                                  "Eject", "B8",
                                                                  "Random Play", "B9",
                                                                  "Select Disc", "BA",
                                                                  "Enter Disc", "BB",
                                                                  "Repeat", "BC",
                                                                  "Tracking", "BD",
                                                                  "Track Normal", "BE",
                                                                  "Slow Tracking", "BF",
                                                                  "Frame Forward", "C0",
                                                                  "Frame Back", "C1",
                                                                  "Mark", "C2",
                                                                  "Clear Mark", "C3",
                                                                  "Repeat From Mark", "C4",
                                                                  "Return To Mark", "C5",
                                                                  "Search Mark Forward", "C6",
                                                                  "Search Mark Backwards", "C7",
                                                                  "Counter Reset", "C8",
                                                                  "Show Counter", "C9",
                                                                  "Tracking Increment", "CA",
                                                                  "Tracking Decrement", "CB",
                                                                  "Stop/Eject", "CC",
                                                                  "Play/Pause", "CD",
                                                                  "Play/Skip", "CE",
                                                                  "Volume", "E0",
                                                                  "Balance", "E1",
                                                                  "Mute", "E2",
                                                                  "Bass", "E3",
                                                                  "Treble", "E4",
                                                                  "Bass Boost", "E5",
                                                                  "Surround Mode", "E6",
                                                                  "Loudness", "E7",
                                                                  "MPX", "E8",
                                                                  "Volume Increment", "E9", 
                                                                  "Volume Decrement", "EA",
                                                                  "Reserved", "EB",
                                                                  "Speed Select", "F0",
                                                                  "Playback Speed", "F1",
                                                                  "Standard Play", "F2",
                                                                  "Long Play", "F3",
                                                                  "Extended Play", "F4",
                                                                  "Slow", "F5",
                                                                  "Fan Enable", "100",
                                                                  "Fan Speed", "101",
                                                                  "Light Enable", "102",
                                                                  "Light Illumination Level", "103",
                                                                  "Climate Control Enable", "104",
                                                                  "Room Temperature", "105",
                                                                  "Security Enable", "106",
                                                                  "Fire Alarm", "107",
                                                                  "Police Alarm", "108", 
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesDigitizersPage = {
                                                                  "Undefined", "0",
                                                                  "Digitizer", "1",
                                                                  "Pen", "2",
                                                                  "Light Pen", "3",
                                                                  "Touch Screen", "4",
                                                                  "Touch Pad", "5",
                                                                  "White Board", "6",
                                                                  "Coordinate Measuring Machine", "7",
                                                                  "3D Digitizer", "8",
                                                                  "Stereo Plotter", "9",
                                                                  "Articulated Arm", "A",
                                                                  "Armature", "B",
                                                                  "Multiple Point Digitizer", "C",
                                                                  "Free Space Wand", "D",
                                                                  "Device Configuration", "E",
                                                                  "Stylus", "20",
                                                                  "Puck", "21",
                                                                  "Finger", "22",
                                                                  "Device Settings", "23",
                                                                  "Tip Pressure", "30",
                                                                  "Barrel Pressure", "31",
                                                                  "In Range", "32",
                                                                  "Touch", "33",
                                                                  "Untouch", "34",
                                                                  "Tap", "35",
                                                                  "Quality", "36",
                                                                  "Data Valid", "37",
                                                                  "Transducer Index", "38",
                                                                  "Tablet Function Keys", "39",
                                                                  "Program Change Keys", "3A",
                                                                  "Battery Strength", "3B",
                                                                  "Invert", "3C",
                                                                  "X Tilt", "3D",
                                                                  "Y Tilt", "3E",
                                                                  "Azimuth", "3F",
                                                                  "Altitude", "40",
                                                                  "Twist", "41",
                                                                  "Tip Switch", "42",
                                                                  "Secondary Tip Switch", "43",
                                                                  "Barrel Switch", "44",
                                                                  "Eraser", "45",
                                                                  "Tablet Pick", "46",
                                                                  "Confidence", "47",
                                                                  "Width", "48",
                                                                  "Height", "49",
                                                                  "Contact Identifier", "51",
                                                                  "Device Mode", "52",
                                                                  "Device Identifier", "53",
                                                                  "Contact Count", "54",
                                                                  "Contact Count Maximum", "55",
                                                                  "Custom", "FF00"
                                                              };
        public static readonly string[] ValuesPIDPage = {
                                                                  "Undefined", "0",
                                                                  "Physical Interface Device", "01",
                                                                  "1F Reserved", "02",
                                                                  "Normal", "20",
                                                                  "Set Effect Report", "21",
                                                                  "Effect Block Index", "22",
                                                                  "Parameter Block Offset", "23",
                                                                  "ROM Flag", "24",
                                                                  "Effect Type", "25",
                                                                  "ET Constant Force", "26",
                                                                  "ET Ramp", "27",
                                                                  "ET Custom Force Data", "28",
                                                                  "2F Reserved", "29",
                                                                  "ET Square", "30",
                                                                  "ET Sine", "31",
                                                                  "ET Triangle", "32",
                                                                  "ET Sawtooth Up", "33",
                                                                  "ET Sawtooth Down", "34",
                                                                  "ET Spring", "40",
                                                                  "ET Damper", "41",
                                                                  "ET Inertia", "42",
                                                                  "ET Friction", "43",
                                                                  "Duration", "50",
                                                                  "Sample Period", "51",
                                                                  "Gain", "52",
                                                                  "Trigger Button", "53",
                                                                  "Trigger Repeat Interval", "54",
                                                                  "Axes Enable", "55",
                                                                  "Direction Enable", "56",
                                                                  "Direction", "57",
                                                                  "Type Specific Block Offset", "58",
                                                                  "Block Type", "59",
                                                                  "Set Envelope Report", "5A",
                                                                  "Attack Level", "5B",
                                                                  "Attack Time", "5C",
                                                                  "Fade Level", "5D",
                                                                  "Fade Time", "5E",
                                                                  "Set Condition Report", "5F",
                                                                  "CP Offset", "60",
                                                                  "Positive Coefficient", "61",
                                                                  "Negative Coefficient", "62",
                                                                  "Positive Saturation", "63",
                                                                  "Negative Saturation", "64",
                                                                  "Dead Band", "65",
                                                                  "Download Force Sample", "66",
                                                                  "Isoch Custom Force Enable", "67",
                                                                  "Custom Force Data Report", "68",
                                                                  "Custom Force Data", "69",
                                                                  "Custom Force Vendor Defined Data", "6A",
                                                                  "Actuator Power", "A6",
                                                                  "Set Custom Force Report", "6B",
                                                                  "Custom Force Data Offset", "6C",
                                                                  "Sample Count", "6D",
                                                                  "Set Periodic Report", "6E",
                                                                  "Offset", "6F",
                                                                  "Magnitude", "70",
                                                                  "Phase", "71",
                                                                  "Period", "72",
                                                                  "Set Constant Force Report", "73",
                                                                  "Set Ramp Force Report", "74",
                                                                  "Ramp Start", "75",
                                                                  "Ramp End", "76",
                                                                  "Effect Operation Report", "77",
                                                                  "Effect Operation", "78",
                                                                  "Op Effect Start", "79",
                                                                  "Op Effect Start Solo", "7A",
                                                                  "Op Effect Stop", "7B",
                                                                  "Loop Count", "7C",
                                                                  "Device Gain Report", "7D",
                                                                  "Device Gain", "7E",
                                                                  "PID Pool Report", "7F",
                                                                  "RAM Pool Size", "80",
                                                                  "ROM Pool Size", "81",
                                                                  "ROM Effect Block Count", "82",
                                                                  "Simultaneous Effects Max", "83",
                                                                  "Pool Alignment", "84",
                                                                  "PID Pool Move Report", "85",
                                                                  "Move Source", "86",
                                                                  "Move Destination", "87",
                                                                  "Move Length", "88",
                                                                  "PID Block Load Report", "89",
                                                                  "Block Load Status", "8B",
                                                                  "Block Load Success", "8C",
                                                                  "Block Load Full", "8D",
                                                                  "Block Load Error", "8E",
                                                                  "Block Handle", "8F",
                                                                  "PID Block Free Report", "90",
                                                                  "Type Specific Block Handle", "91",
                                                                  "PID State Report", "92",
                                                                  "Effect Playing", "94",
                                                                  "PID Device Control Report", "95",
                                                                  "PID Device Control", "96",
                                                                  "DC Enable Actuators", "97",
                                                                  "DC Disable Actuators", "98",
                                                                  "DC Stop All Effects", "99",
                                                                  "DC Device Reset", "9A",
                                                                  "DC Device Pause", "9B",
                                                                  "DC Device Continue", "9C",
                                                                  "Device Paused", "9F",
                                                                  "Actuators Enabled", "A0",
                                                                  "Safety Switch", "A4",
                                                                  "Actuator Override Switch", "A5",
                                                                  "Start Delay", "A7",
                                                                  "Parameter Block Size", "A8",
                                                                  "Device Managed Pool", "A9",
                                                                  "Shared Parameter Blocks", "AA",
                                                                  "Create New Effect Report", "AB",
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesUnicodePage; // Filled in Constructor

        public static readonly string[] ValuesAlphanumericDisplayPage = {
                                                                  "Undefined", "0",
                                                                  "Alphanumeric Display", "01",
                                                                  "Bitmapped Display", "02",
                                                                  "Display Attributes Report", "20",
                                                                  "ASCII Character Set", "21",
                                                                  "Data Read Back", "22",
                                                                  "Font Read Back", "23",
                                                                  "Display Control Report", "24",
                                                                  "Clear Display", "25",
                                                                  "Display Enable", "26",
                                                                  "Screen Saver Delay", "27",
                                                                  "Screen Saver Enable", "28",
                                                                  "Vertical Scroll", "29",
                                                                  "Horizontal Scroll", "2A",
                                                                  "Character Report", "2B",
                                                                  "Display Data", "2C",
                                                                  "Display Status", "2D",
                                                                  "Stat Not Ready", "2E",
                                                                  "Stat Ready", "2F",
                                                                  "Err Not a loadable character", "30",
                                                                  "Err Font data cannot be read", "31",
                                                                  "Cursor Position Report", "32",
                                                                  "Row", "33",
                                                                  "Column", "34",
                                                                  "Rows", "35",
                                                                  "Columns", "36",
                                                                  "Cursor Pixel Positioning", "37",
                                                                  "Cursor Mode", "38",
                                                                  "Cursor Enable", "39",
                                                                  "Cursor Blink", "3A",
                                                                  "Font Report", "3B",
                                                                  "Font Data", "3C",
                                                                  "Character Width", "3D",
                                                                  "Character Height", "3E",
                                                                  "Character Spacing Horizontal", "3F",
                                                                  "Character Spacing Vertical", "40",
                                                                  "Unicode Character Set", "41",
                                                                  "Font 7-Segment", "42",
                                                                  "7-Segment Direct Map", "43",
                                                                  "Font 14-Segment", "44",
                                                                  "14-Segment Direct Map", "45",
                                                                  "Display Brightness", "46",
                                                                  "Display Contrast", "47",
                                                                  "Character Attribute", "48",
                                                                  "Attribute Readback", "49",
                                                                  "Attribute Data", "4A",
                                                                  "Char Attr Enhance", "4B",
                                                                  "Char Attr Underline", "4C",
                                                                  "Char Attr Blink", "4D",
                                                                  "Bitmap Size X", "80",
                                                                  "Bitmap Size Y", "81",
                                                                  "Bit Depth Format", "83",
                                                                  "Display Orientation", "84",
                                                                  "Palette Report", "85",
                                                                  "Palette Data Size", "86",
                                                                  "Palette Data Offset", "87",
                                                                  "Palette Data", "88",
                                                                  "Blit Report", "8A",
                                                                  "Blit Rectangle X1", "8B",
                                                                  "Blit Rectangle Y1", "8C",
                                                                  "Blit Rectangle X2", "8D",
                                                                  "Blit Rectangle Y2", "8E",
                                                                  "Blit Data", "8F",
                                                                  "Soft Button", "90",
                                                                  "Soft Button ID", "91",
                                                                  "Soft Button Side", "92",
                                                                  "Soft Button Offset 1", "93",
                                                                  "Soft Button Offset 2", "94",
                                                                  "Soft Button Report", "95", 
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesMedicalInstrumentPage = {
                                                                  "Undefined", "0",
                                                                  "Medical Ultrasound", "1",
                                                                  "VCR/Acquisition", "20",
                                                                  "Freeze/Thaw", "21",
                                                                  "Clip Store", "22",
                                                                  "Update", "23",
                                                                  "Next", "24",
                                                                  "Save", "25",
                                                                  "Print", "26",
                                                                  "Microphone Enable", "27",
                                                                  "Cine", "40",
                                                                  "Transmit Power", "41",
                                                                  "Volume", "42",
                                                                  "Focus", "43",
                                                                  "Depth", "44",
                                                                  "Soft Step - Primary", "60",
                                                                  "Soft Step - Secondary", "61",
                                                                  "Depth Gain Compensation", "70",
                                                                  "Zoom Select", "80",
                                                                  "Zoom Adjust", "81",
                                                                  "Spectral Doppler Mode Select", "82",
                                                                  "Spectral Doppler Adjust", "83",
                                                                  "Color Doppler Mode Select", "84",
                                                                  "Color Doppler Adjust", "85",
                                                                  "Motion Mode Select", "86",
                                                                  "Motion Mode Adjust", "87",
                                                                  "2-D Mode Select", "88",
                                                                  "2-D Mode Adjust", "89",
                                                                  "Soft Control Select", "A0",
                                                                  "Soft Control Adjust", "A1",
                                                                  "Custom", "FF00"
                                                              };

        public static readonly string[] ValuesMonitorDevicesPage = {
                                                                  "Monitor Control", "1",
                                                                  "EDID Information", "2",
                                                                  "VDIF Information", "3",
                                                                  "VESA Version", "4"
                                                              };

        public static readonly string[] ValuesMonitorEnumeratedValuesPage; // Filled in Constructor

        public static readonly string[] ValuesVESAVirtualControlsPage = {
                                                                  "Brightness", "10",
                                                                  "Contrast", "12",
                                                                  "Red Video Gain", "16",
                                                                  "Green Video Gain", "18",
                                                                  "Blue Video Gain", "1A",
                                                                  "Focus", "1C",
                                                                  "Horizontal Position", "20",
                                                                  "Horizontal Size", "22",
                                                                  "Horizontal Pincushion", "24",
                                                                  "Horizontal Pincushion Balance", "26",
                                                                  "Horizontal Misconvergence", "28",
                                                                  "Horizontal Linearity", "2A",
                                                                  "Horizontal Linearity Balance", "2C",
                                                                  "Vertical Position", "30",
                                                                  "Vertical Size", "32",
                                                                  "Vertical Pincushion", "34",
                                                                  "Vertical Pincushion Balance", "36",
                                                                  "Vertical Misconvergence", "38",
                                                                  "Vertical Linearity", "3A",
                                                                  "Vertical Linearity Balance", "3C",
                                                                  "Parallelogram Distortion (Key Balance)", "40",
                                                                  "Trapezoidal Distortion (Key)", "42",
                                                                  "Tilt (Rotation)", "44",
                                                                  "Top Corner Distortion Control", "46",
                                                                  "Top Corner Distortion Balance", "48",
                                                                  "Bottom Corner Distortion Control", "4A",
                                                                  "Bottom Corner Distortion Balance", "4C",
                                                                  "Horizontal Moire", "56",
                                                                  "Vertical Moire", "58",
                                                                  "Red Video Black Level", "6C",
                                                                  "Green Video Black Level", "6E",
                                                                  "Blue Video Black Level", "70",
                                                                  "Input Level Select", "5E",
                                                                  "Input Source Select", "60",
                                                                  "On Screen Display", "Ca",
                                                                  "StereoMode", "D4",
                                                                  "Polarity Horizontal Synchronization", "A4h",
                                                                  "Polarity Vertical Synchronization", "A6h",
                                                                  "Synchronization Type", "A8h",
                                                                  "Screen Orientation", "AAh",
                                                                  "Horizontal Frequency", "ACh",
                                                                  "Vertical Frequency", "AEh",
                                                                  "Degauss", "01h",
                                                                  "Settings", "B0h",
                                                                  "Custom", "FF00"
                                                              };


        public static readonly string[] ValuesPowerDevicePage = {
                                                                  "Undefined", "0",
                                                                  "iName", "1",
                                                                  "PresentStatus", "2",
                                                                  "ChangedStatus", "3",
                                                                  "UPS", "4",
                                                                  "PowerSupply", "5",
                                                                  "BatterySystem", "10",
                                                                  "BatterySystemID", "11",
                                                                  "Battery", "12",
                                                                  "BatteryID", "13",
                                                                  "Charger", "14",
                                                                  "ChargerID", "15",
                                                                  "PowerConverter", "16",
                                                                  "PowerConverterID", "17",
                                                                  "OutletSystem", "18",
                                                                  "OutletSystemID", "19",
                                                                  "Input", "1A",
                                                                  "InputID", "1B",
                                                                  "Output", "1C",
                                                                  "OutputID", "1D",
                                                                  "Flow", "1E",
                                                                  "FlowID", "1F",
                                                                  "Outlet", "20",
                                                                  "OutletID", "21",
                                                                  "Gang", "22",
                                                                  "GangID", "23",
                                                                  "PowerSummary", "24",
                                                                  "PowerSummaryID", "25",
                                                                  "Voltage", "30",
                                                                  "Current", "31",
                                                                  "Frequency", "32",
                                                                  "ApparentPower", "33",
                                                                  "ActivePower", "34",
                                                                  "PercentLoad", "35",
                                                                  "Temperature", "36",
                                                                  "Humidity", "37",
                                                                  "BadCount", "38",
                                                                  "ConfigVoltage", "40",
                                                                  "ConfigCurrent", "41",
                                                                  "ConfigFrequency", "42",
                                                                  "ConfigApparentPower", "43",
                                                                  "ConfigActivePower", "44",
                                                                  "ConfigPercentLoad", "45",
                                                                  "ConfigTemperature", "46",
                                                                  "ConfigHumidity", "47",
                                                                  "SwitchOnControl", "50",
                                                                  "SwitchOffControl", "51",
                                                                  "ToggleControl", "52",
                                                                  "LowVoltageTransfer", "53",
                                                                  "HighVoltageTransfer", "54",
                                                                  "DelayBeforeReboot", "55",
                                                                  "DelayBeforeStartup", "56",
                                                                  "DelayBeforeShutdown", "57",
                                                                  "Test", "58",
                                                                  "ModuleReset", "59",
                                                                  "AudibleAlarmControl", "5A",
                                                                  "Present", "60",
                                                                  "Good", "61",
                                                                  "InternalFailure", "62",
                                                                  "VoltageOutOfRange", "63",
                                                                  "FrequencyOutOfRange", "64",
                                                                  "Overload", "65",
                                                                  "OverCharged", "66",
                                                                  "OverTemperature", "67",
                                                                  "ShutdownRequested", "68",
                                                                  "ShutdownImminent", "69",
                                                                  "SwitchOn/Off", "6B",
                                                                  "Switchable", "6C",
                                                                  "Used", "6D",
                                                                  "Boost", "6E",
                                                                  "Buck", "6F",
                                                                  "Initialized", "70",
                                                                  "Tested", "71",
                                                                  "AwaitingPower", "72",
                                                                  "CommunicationLost", "73",
                                                                  "iManufacturer", "FD",
                                                                  "iProduct", "FE",
                                                                  "iserialNumber", "FF",
                                                                  "Custom", "FF00"
                                                              };
        public static readonly string[] ValuesBatterySystemPage = {

                                                                  "SMBBatteryMode", "01",
                                                                  "SMBBatteryStatus", "02",
                                                                  "SMBAlarmWarning", "03",
                                                                  "SMBChargerMode", "04",
                                                                  "SMBChargerStatus", "05",
                                                                  "SMBChargerSpecInfo", "06",
                                                                  "SMBSelectorState", "07",
                                                                  "SMBSelectorPresets", "08",
                                                                  "SMBSelectorInfo", "09",
                                                                  "OptionalMfgFunction1", "10",
                                                                  "OptionalMfgFunction2", "11",
                                                                  "OptionalMfgFunction3", "12",
                                                                  "OptionalMfgFunction4", "13",
                                                                  "OptionalMfgFunction5", "14",
                                                                  "ConnectionToSMBus", "15",
                                                                  "OutputConnection", "16",
                                                                  "ChargerConnection", "17",
                                                                  "BatteryInsertion", "18",
                                                                  "Usenext", "19",
                                                                  "OKToUse", "1A",
                                                                  "BatterySupported", "1B",
                                                                  "SelectorRevision", "1C", 
                                                                  "ChargingIndicator", "1D",
                                                                  "ManufacturerAccess", "28",
                                                                  "RemainingCapacityLimit", "29",
                                                                  "RemainingTimeLimit", "2A",
                                                                  "AtRate", "2B",
                                                                  "CapacityMode", "2C",
                                                                  "BroadcastToCharger", "2D",
                                                                  "PrimaryBattery", "2E",
                                                                  "ChargeController", "2F",
                                                                  "TerminateCharge", "40",
                                                                  "TerminateDischarge", "41",
                                                                  "BelowRemainingCapacityLimit", "42",
                                                                  "RemainingTimeLimitExpired", "43",
                                                                  "Charging", "44",
                                                                  "Discharging", "45",
                                                                  "FullyCharged", "46",
                                                                  "FullyDischarged", "47",
                                                                  "ConditioningFlag", "48",
                                                                  "AtRateOK", "49",
                                                                  "SMBErrorCode", "4A",
                                                                  "NeedReplacement", "4B",
                                                                  "AtRateTimeToFull", "60",
                                                                  "AtRateTimeToEmpty", "61",
                                                                  "AverageCurrent", "62",
                                                                  "Maxerror", "63",
                                                                  "RelativeStateOfCharge", "64",
                                                                  "AbsoluteStateOfCharge", "65",
                                                                  "RemainingCapacity", "66",
                                                                  "FullChargeCapacity", "67",
                                                                  "RunTimeToEmpty", "68",
                                                                  "AverageTimeToEmpty", "69",
                                                                  "AverageTimeToFull", "6A",
                                                                  "CycleCount", "6B",
                                                                  "BattPackModelLevel", "80",
                                                                  "InternalChargeController", "81",
                                                                  "PrimaryBatterySupport", "82",
                                                                  "DesignCapacity", "83",
                                                                  "SpecificationInfo", "84",
                                                                  "ManufacturerDate", "85",
                                                                  "SerialNumber", "86",
                                                                  "iManufacturerName", "87",
                                                                  "iDevicename", "88",
                                                                  "iDeviceChemistery", "89",
                                                                  "ManufacturerData", "8A",
                                                                  "Rechargable", "8B",
                                                                  "WarningCapacityLimit", "8C",
                                                                  "CapacityGranularity1", "8D",
                                                                  "CapacityGranularity2", "8E",
                                                                  "iOEMInformation", "8F",
                                                                  "InhibitCharge", "C0",
                                                                  "EnablePolling", "C1",
                                                                  "ResetToZero", "C2",
                                                                  "ACPresent", "D0",
                                                                  "BatteryPresent", "D1",
                                                                  "PowerFail", "D2",
                                                                  "AlarmInhibited", "D3",
                                                                  "ThermistorUnderRange", "D4",
                                                                  "ThermistorHot", "D5",
                                                                  "ThermistorCold", "D6",
                                                                  "ThermistorOverRange", "D7",
                                                                  "VoltageOutOfRange", "D8",
                                                                  "CurrentOutOfRange", "D9",
                                                                  "CurrentNotRegulated", "DA",
                                                                  "VoltageNotRegulated", "DB",
                                                                  "MasterMode", "DC",
                                                                  "ChargerSelectorSupport", "F0",
                                                                  "ChargerSpec", "F1",
                                                                  "Level2", "F2",
                                                                  "Level3", "F3",
                                                                  "Custom", "FF00"
                                                              };

        #endregion Constants
    }
}
