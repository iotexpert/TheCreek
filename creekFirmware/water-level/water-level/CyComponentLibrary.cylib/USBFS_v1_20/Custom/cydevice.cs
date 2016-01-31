/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;

namespace USBFS_v1_20
{
    public class DeviceDescriptor : USBDescriptor
    {
        private byte _bDeviceClass;
        private byte _bDeviceSubClass;
        private byte _bDeviceProtocol;
        private byte _bMaxPacketSize0;
        private ushort _idVendor = 0x04B4;
        private ushort _idProduct = 0x8051;
        private byte _iManufacturer;
        private byte _iProduct;
        private byte _iSerialNumber;
        private byte _bNumConfigurations;
        private ushort _bcdDevice = 0x0000;
		private byte _bMemoryMgmt = 0;

        public uint iwManufacturer;
        public uint iwProduct;

        public DeviceDescriptor()
        {
            bDescriptorType = USBDescriptorType.DEVICE;
            bLength = 18;
        }
        
        #region Properties
        
        public byte bDeviceClass
        {
            get
            {
                return _bDeviceClass;
            }
            set
            {
                _bDeviceClass = value;
            }
        }

        public byte bDeviceSubClass
        {
            get
            {
                return _bDeviceSubClass;
            }
            set
            {
                _bDeviceSubClass = value;
            }
        }

        public byte bDeviceProtocol
        {
            get
            {
                return _bDeviceProtocol;
            }
            set
            {
                _bDeviceProtocol = value;
            }
        }

        public byte bMaxPacketSize0
        {
            get
            {
                return _bMaxPacketSize0;
            }
            set
            {
                _bMaxPacketSize0 = value;
            }
        }

        public ushort idVendor
        {
            get
            {
                return _idVendor;
            }
            set
            {
                _idVendor = value;
            }
        }

        public ushort idProduct
        {
            get
            {
                return _idProduct;
            }
            set
            {
                _idProduct = value;
            }
        }

        /// <value>0x0200</value>
        public ushort bcdDevice
        {
            get
            {
                //return 0x0200;
                return _bcdDevice;
            }
            set
            {
                _bcdDevice = value;   
            }
        }

        public byte iManufacturer
        {
            get
            {
                return _iManufacturer;
            }
            set
            {
                _iManufacturer = value;
            }
        }

        public byte iProduct
        {
            get
            {
                return _iProduct;
            }
            set
            {
                _iProduct = value;
            }
        }

        public byte iSerialNumber
        {
            get
            {
                return _iSerialNumber;
            }
            set
            {
                _iSerialNumber = value;
            }
        }

        public byte bNumConfigurations
        {
            get
            {
                return _bNumConfigurations;
            }
            set
            {
                _bNumConfigurations = value;
            }
        }
        public byte bMemoryMgmt
        {
            get
            {
                return _bMemoryMgmt;
            }
            set
            {
                _bMemoryMgmt = value;
            }
        }
        

        #endregion Properties
    }

    public class ConfigDescriptor : USBDescriptor
    {
        private ushort _wTotalLength;
        private byte _bNumInterfaces;
        private byte _bConfigurationValue;
        private byte _iConfiguration; // index in array of strings
        public uint iwConfiguration; // absolute index

        private byte _bmAttributes;
        private byte _bMaxPower;

        public ConfigDescriptor()
        {
            bDescriptorType = USBDescriptorType.CONFIGURATION;
            bLength = 9;
        }

        #region Properties
        public ushort wTotalLength
        {
            get
            {
                return _wTotalLength;
            }
            set
            {
                _wTotalLength = value;
            }
        }

        public byte bNumInterfaces
        {
            get
            {
                return _bNumInterfaces;
            }
            set
            {
                _bNumInterfaces = value;
            }
        }

        public byte bConfigurationValue
        {
            get
            {
                return _bConfigurationValue;
            }
            set
            {
                _bConfigurationValue = value;
            }
        }

        public byte iConfiguration
        {
            get
            {
                return _iConfiguration;
            }
            set
            {
                _iConfiguration = value;
            }
        }

        #endregion

        public byte bmAttributes
        {
            get
            {
                return _bmAttributes;
            }
            set
            {
                _bmAttributes = value;
            }
        }

        public byte bMaxPower
        {
            get
            {
                return _bMaxPower;
            }
            set
            {
                _bMaxPower = value;
            }
        }
    }

    public class EndpointDescriptor : USBDescriptor
    {
        private byte _bInterval = 10;
        private byte _bEndpointAddress;
        private byte _bmAttributes = 2;
        private ushort _wMaxPacketSize = 8;
        public bool DoubleBuffer;

        public EndpointDescriptor()
        {
            bDescriptorType = USBDescriptorType.ENDPOINT;
            bLength = 7;
            bEndpointAddress = 1;
        }

        #region Properties

        public byte bInterval
        {
            get
            {
                return _bInterval;
            }
            set
            {
                _bInterval = value;
            }
        }

        public byte bEndpointAddress
        {
            get
            {
                return _bEndpointAddress;
            }
            set
            {
                _bEndpointAddress = value;
            }
        }

        public byte bmAttributes
        {
            get
            {
                return _bmAttributes;
            }
            set
            {
                _bmAttributes = value;
            }
        }

        public ushort wMaxPacketSize
        {
            get
            {
                return _wMaxPacketSize;
            }
            set
            {
                _wMaxPacketSize = value;
            }
        }

        #endregion Properties
    }

    public class InterfaceDescriptor : USBDescriptor
    {
        private byte _bInterfaceClass;
        private byte _bInterfaceSubClass;
        private byte _bAlternateSetting;
        private byte _bInterfaceNumber;
        private byte _bNumEndpoints;
        private byte _bInterfaceProtocol;
        private byte _iInterface;
        public uint iwInterface;

        public InterfaceDescriptor(byte interfaceNum, byte alternateSettingNum)
        {
            bDescriptorType = USBDescriptorType.INTERFACE;
            bLength = 9;
            _bInterfaceNumber = interfaceNum;
            _bAlternateSetting = alternateSettingNum;
        }
        public InterfaceDescriptor()
        {
            bDescriptorType = USBDescriptorType.INTERFACE;
            bLength = 9;
        }

        #region Properties

        public byte bInterfaceClass
        {
            get
            {
                return _bInterfaceClass;
            }
            set
            {
                if (_bInterfaceClass != value)
                {
                    _bInterfaceClass = value;
                    _bInterfaceSubClass = 0;                    
                }
            }
        }

        public byte bAlternateSetting
        {
            get
            {
                return _bAlternateSetting;
            }
            set
            {
                _bAlternateSetting = value;
            }
        }

        public byte bInterfaceNumber
        {
            get
            {
                return _bInterfaceNumber;
            }
            set
            {
                _bInterfaceNumber = value;
            }
        }

        public byte bNumEndpoints
        {
            get
            {
                return _bNumEndpoints;
            }
            set
            {
                _bNumEndpoints = value;
            }
        }

        public byte bInterfaceSubClass
        {
            get
            {
               return _bInterfaceSubClass;
            }
            set
            {
                _bInterfaceSubClass = value;
            }
        }

        public byte bInterfaceProtocol
        {
            get
            {
                return _bInterfaceProtocol;
            }
            set
            {
                _bInterfaceProtocol = value;
            }
        }

        public byte iInterface
        {
            get
            {
                return _iInterface;
            }
            set
            {
                _iInterface = value;
            }
        }

        #endregion Properties
    }

    public class InterfaceGeneralDescriptor : USBDescriptor
    {
         public InterfaceGeneralDescriptor()
        {
            bDescriptorType = USBDescriptorType.ALTERNATE;
            bLength = 0;
        }
    }

    public class InterfaceAssociationDescriptor : USBDescriptor
    {

        #region Properties
        public byte bInterfaceCount
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public byte bFirstInterface
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public byte bFunctionClass
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public byte iFunction
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        #endregion

        public byte bFunctionProtocol
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public byte bFunctionSubClass
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }

    public class StringsDescriptor : USBDescriptor
    {
        private UInt16 _wLANGID;
        private StringDescriptor _eeString;
        private List<StringDescriptor> _bStrings;
        public bool ShowSerial = true;
        public bool ShowEE = true;

        public StringsDescriptor()
        {
            bDescriptorType = USBDescriptorType.STRING;
            bLength = 18; //??
        }

        #region Properties
        public UInt16 wLANGID
        {
            get
            {
                return _wLANGID;
            }
            set
            {
                _wLANGID = value;
            }
        }

        public StringDescriptor eeString
        {
            get
            {
                return _eeString;
            }
            set
            {
                _eeString = value;
            }
        }


        public List<StringDescriptor> bStrings
        {
            get
            {
                return _bStrings;
            }
            set
            {
                _bStrings = value;
            }
        }
        #endregion Properties
    }

    public enum StringGenerationType { USER_ENTERED_TEXT, USER_CALL_BACK, SILICON_NUMBER } ;

    public class StringDescriptor : USBDescriptor
    {
        private string _bString;
        private bool _bUsed = true;
        public StringGenerationType snType; 

        public StringDescriptor()
        {
            bDescriptorType = USBDescriptorType.STRING;
            bLength = 2; 
        }

        #region Properties       
        public string bString
        {
            get
            {
                return _bString;
            }
            set
            {
                _bString = value;
                bLength = (byte)(2 + _bString.Length*2);
            }
        }

        public bool bUsed
        {
            get { return _bUsed; } 
            set { _bUsed = value; } 
        }

        public override string ToString()
        {
            return _bString;
        }

        #endregion Properties
    }

    public class StringZeroDescriptor : USBDescriptor
    {
        private UInt16 _wLANGID = 0x0409;

        public StringZeroDescriptor()
        {
            bDescriptorType = USBDescriptorType.STRING;
            bLength = 4; //??
        }

        #region Properties
        public UInt16 wLANGID
        {
            get
            {
                return _wLANGID;
            }
            set
            {
                _wLANGID = value;
            }
        }
        #endregion Properties
    }

    public class HIDDescriptor : USBDescriptor
    {
        private ushort _bcdHID;
        private byte _bCountryCode;
        private byte _bNumDescriptors;
        private byte _bDescriptorType1 = 0x22;
        private ushort _wDescriptorLength;

        public byte ReportIndex;
        public uint wReportIndex;

        public HIDDescriptor()
        {
            bDescriptorType = USBDescriptorType.HID;
            bLength = 9; 
        }

        #region Properties
        public ushort bcdHID
        {
            get
            {
                return _bcdHID;
            }
            set
            {
                _bcdHID = value;
            }
        }

        public byte bCountryCode
        {
            get
            {
                return _bCountryCode;
            }
            set
            {
                _bCountryCode = value;
            }
        }

        public byte bNumDescriptors
        {
            get
            {
                return _bNumDescriptors;
            }
            set
            {
                _bNumDescriptors = value;
            }
        }

        public byte bDescriptorType1
        {
            get
            {
                return _bDescriptorType1;
            }
            set
            {
                _bDescriptorType1 = value;
            }
        }

        public ushort wDescriptorLength
        {
            get
            {
                return _wDescriptorLength;
            }
            set
            {
                _wDescriptorLength = value;
            }
        }
        #endregion Properties
    }

    public class HIDReportDescriptor : USBDescriptor
    {
        public string Name;
        public ushort wLength = 0;

        public HIDReportDescriptor()
        {
            bDescriptorType = USBDescriptorType.HID_REPORT;
            bLength = 2; //??
            Name = "HID Report Descriptor";
        }

        public HIDReportDescriptor(string name)
        {
            bDescriptorType = USBDescriptorType.HID_REPORT;
            bLength = 2; //??
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class HIDReportItemDescriptor : USBDescriptor
    {

        public HIDReportItem Item;

        public HIDReportItemDescriptor()
        {
            bDescriptorType = USBDescriptorType.HID_REPORT_ITEM;
            bLength = 1; //??
        }
    }

    public class AudioDescriptor : USBDescriptor
    {
        public byte bAudioDescriptorSubClass; // subclass of interface descriptor
        public byte bAudioDescriptorSubType;
        public List<byte> bValues;

        public AudioDescriptor()
        {
            bDescriptorType = USBDescriptorType.AUDIO;
            bLength = 2; //??
            bValues = new List<byte>();
        }
    }

    public class CDCDescriptor : USBDescriptor
    {
        public byte bCDCDescriptorSubType;
        public List<byte> bValues;

        public CDCDescriptor()
        {
            bDescriptorType = USBDescriptorType.AUDIO;
            bLength = 2; //??
            bValues = new List<byte>();
        }
    }

    /// <remarks>usb 2.0 Ch. 9 Table 9-5</remarks>
    public enum USBDescriptorType
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
        HID_REPORT = 0xF0,
        HID_REPORT_ITEM = 0xF1
    }

    public class USBOtherTypes
    {
        public enum CommunicationSubclassCodes
        {
            SUBCLASS_UNDEFINED = 0x00,
            DirectLineControlModel = 0x01,
            AbstractControlModel = 0x02,
            TelephoneControlModel = 0x03,
            MultiChannelControlModel = 0x04,
            CAPIControlModel = 0x05,
            EthernetNetworkingControlModel = 0x06,
            ATMNetworkingControlModel = 0x07,
            WirelessHandsetControlModel = 0x08,
            DeviceManagement = 0x09,
            MobileDirectLineModel = 0x0A,
            OBEX = 0x0B,
            EthernetEmulationModel = 0x0C
        }

        public enum AudioSubclassCodes
        {
            SUBCLASS_UNDEFINED = 0x00,
            AUDIOCONTROL = 0x01,
            AUDIOSTREAMING = 0x02,
            MIDISTREAMING = 0x03
        }

        public enum AudioClassSpecDescriptorTypes
        {
            CS_UNDEFINED = 0x20,
            CS_DEVICE = 0x21,
            CS_CONFIGURATION = 0x22,
            CS_STRING = 0x23,
            CS_INTERFACE = 0x24,
            CS_ENDPOINT = 0x25
        }

        public enum ACInterfaceDescriptorSubtypes
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

        public enum ASInterfaceDescriptorSubtypes
        {
            AS_DESCRIPTOR_UNDEFINED = 0x00,
            AS_GENERAL = 0x01,
            FORMAT_TYPE = 0x02,
            FORMAT_SPECIFIC = 0x03
        }
    }
}
