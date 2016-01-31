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

namespace USBFS_v2_30
{
    //================================================================================================================
    // Class-specific descriptors
    //================================================================================================================

    public class CyMSHeaderDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private UInt16 m_bcdMSC = (UInt16)0x0100;
        private UInt16 m_wTotalLength;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("MIDIStreaming SubClass Specification Release Number in Binary-Coded Decimal")]
        [DefaultValue(typeof(UInt16), "256")]
        public UInt16 bcdMSC
        {
            get { return m_bcdMSC; }
            set { m_bcdMSC = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Browsable(false)]
        [Description("Total number of bytes returned for the class-specific MIDIStreaming interface descriptor.")]
        [DefaultValue(typeof(UInt16), "256")]
        public ushort wTotalLength
        {
            get { return m_wTotalLength; }
            set { m_wTotalLength = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyMSHeaderDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes.MS_HEADER;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = 7;
        }

        public override string ToString()
        {
            return "MS Header";
        }

    }
    //================================================================================================================

    public class CyMSInJackDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bJackType;
        private byte m_bJackID;
        private byte m_iJack;

        private string m_sJack;
        public uint iwJack; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [TypeConverter(typeof(CyEnumConverter))]
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("EMBEDDED or EXTERNAL")]
        [DefaultValue(typeof(CyUSBOtherTypes.CyJackTypes), "JACK_TYPE_UNDEFINED")]
        public CyUSBOtherTypes.CyJackTypes bJackType
        {
            get { return (CyUSBOtherTypes.CyJackTypes)m_bJackType; }
            set { m_bJackType = (byte)value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the MIDI IN Jack within the USB-MIDI function.")]
        [DefaultValue((byte) 0)]
        public byte bJackID
        {
            get { return m_bJackID; }
            set { m_bJackID = value; }
        }

        [Browsable(false)]
        public byte iJack
        {
            get { return m_iJack; }
            set { m_iJack = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iJack"),
         Description("String descriptor that describes the MIDI IN Jack.")]
        public string sJack
        {
            get { return m_sJack; }
            set { m_sJack = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyMSInJackDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes.MIDI_IN_JACK;
            bLength = 6;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "MIDI IN Jack";
        }

    }
    //================================================================================================================

    public class CyMSOutJackDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bJackType;
        private byte m_bJackID;
        private byte m_bNrInputPins;
        private List<byte> m_baSourceID;
        private List<byte> m_baSourcePin;
        private byte m_iJack;

        private string m_sJack;
        public uint iwJack; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [TypeConverter(typeof(CyEnumConverter))]
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("EMBEDDED or EXTERNAL")]
        [DefaultValue(typeof(CyUSBOtherTypes.CyJackTypes), "JACK_TYPE_UNDEFINED")]
        public CyUSBOtherTypes.CyJackTypes bJackType
        {
            get { return (CyUSBOtherTypes.CyJackTypes)m_bJackType; }
            set { m_bJackType = (byte)value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the MIDI OUT Jack within the USB-MIDI function.")]
        [DefaultValue((byte)0)]
        public byte bJackID
        {
            get { return m_bJackID; }
            set { m_bJackID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of Input Pins of this MIDI OUT Jack")]
        [DefaultValue((byte)0)]
        public byte bNrInputPins
        {
            get { return m_bNrInputPins; }
            set
            {
                m_bNrInputPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInputPins);
                CyUSBFSParameters.ChangeArrayLength(m_baSourcePin, m_bNrInputPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Entity to which the first Input Pin of this MIDI OUT Jack is connected")]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set { m_baSourceID = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description(
            "Output Pin number of the Entity to which the first Input Pin of this MIDI OUT Jack is connected")]
        public byte[] baSourcePin
        {
            get { return m_baSourcePin.ToArray(); }
            set { m_baSourcePin = new List<byte>(value); }
        }

        [Browsable(false)]
        public byte iJack
        {
            get { return m_iJack; }
            set { m_iJack = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iJack"),
         Description("String descriptor that describes the MIDI OUT Jack.")]
        public string sJack
        {
            get { return m_sJack; }
            set { m_sJack = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyMSOutJackDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes.MIDI_OUT_JACK;
            m_baSourceID = new List<byte>();
            m_baSourcePin = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(7 + 2 * m_bNrInputPins); // 7 + 2*p
        }

        public override string ToString()
        {
            return "MIDI OUT Jack";
        }

    }
    //================================================================================================================

    public class CyMSElementDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bElementID;
        private byte m_bNrInputPins;
        private List<byte> m_baSourceID;
        private List<byte> m_baSourcePin;
        private byte m_bNrOutPins;
        private byte m_bInTerminalLink;
        private byte m_bOutTerminalLink;
        private byte m_bElCapsSize;
        private List<byte> m_bmElementCaps;
        private byte m_iElement;

        private string m_sElement;
        public uint iwElement; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Element within the USB-MIDI function.")]
        [DefaultValue((byte)0)]
        public byte bElementID
        {
            get { return m_bElementID; }
            set { m_bElementID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of Input Pins of this Element")]
        [DefaultValue((byte)0)]
        public byte bNrInputPins
        {
            get { return m_bNrInputPins; }
            set
            {
                m_bNrInputPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInputPins);
                CyUSBFSParameters.ChangeArrayLength(m_baSourcePin, m_bNrInputPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Entities to which Input Pins of this Element are connected")]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set { m_baSourceID = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Output Pin number of the Entities to which Input Pins of this Element are connected")]
        public byte[] baSourcePin
        {
            get { return m_baSourcePin.ToArray(); }
            set { m_baSourcePin = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of Output Pins of this Element")]
        [DefaultValue((byte)0)]
        public byte bNrOutPins
        {
            get { return m_bNrOutPins; }
            set { m_bNrOutPins = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("The Terminal ID of the Input Terminal to which this Element is connected")]
        [DefaultValue((byte)0)]
        public byte bInTerminalLink
        {
            get { return m_bInTerminalLink; }
            set { m_bInTerminalLink = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("The Terminal ID of the Output Terminal to which this Element is connected")]
        [DefaultValue((byte)0)]
        public byte bOutTerminalLink
        {
            get { return m_bOutTerminalLink; }
            set { m_bOutTerminalLink = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Size, in bytes, of the bmElementCaps field")]
        [DefaultValue((byte)0)]
        public byte bElCapsSize
        {
            get { return m_bElCapsSize; }
            set
            {
                m_bElCapsSize = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_bmElementCaps, m_bElCapsSize);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public byte[] bmElementCaps
        {
            get { return m_bmElementCaps.ToArray(); }
            set { m_bmElementCaps = new List<byte>(value); }
        }

        [Browsable(false)]
        public byte iElement
        {
            get { return m_iElement; }
            set { m_iElement = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iElement"),
         Description("String descriptor that describes the Element")]
        public string sElement
        {
            get { return m_sElement; }
            set { m_sElement = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyMSElementDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes.ELEMENT;
            m_baSourceID = new List<byte>();
            m_baSourcePin = new List<byte>();
            m_bmElementCaps = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(10 + 2 * m_bNrInputPins + m_bElCapsSize); // 10 + 2*p + n
        }

        public override string ToString()
        {
            return "Element";
        }

    }
    //================================================================================================================

    public class CyMSEndpointDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyMSClassSpecEPDescriptorTypes m_bDescriptorSubtype;
        private byte m_bNumEmbMIDIJack;
        private List<byte> m_baAssocJackID;

        public CyMSEndpointDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CS_ENDPOINT;
            m_bDescriptorSubtype = CyUSBOtherTypes.CyMSClassSpecEPDescriptorTypes.MS_GENERAL;
            m_baAssocJackID = new List<byte>();
        }

        public override int GetLevel()
        {
            return 5;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(4 + m_bNumEmbMIDIJack); // 4 + n
        }

        public override string ToString()
        {
            return "MS Bulk Data Endpoint Descriptor";
        }


        #region Properties

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyMSClassSpecEPDescriptorTypes bDescriptorSubtype
        {
            get
            {
                return m_bDescriptorSubtype;
            }
            set
            {
                m_bDescriptorSubtype = value;
            }
        }
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNumEmbMIDIJack
        {
            get { return m_bNumEmbMIDIJack; }
            set
            {
                m_bNumEmbMIDIJack = value; 
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baAssocJackID, m_bNumEmbMIDIJack);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte[] baAssocJackID
        {
            get { return m_baAssocJackID.ToArray(); }
            set { m_baAssocJackID = new List<byte>(value); }
        }
       
        #endregion Properties
    }
}
