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

namespace USBFS_v2_20
{
    //================================================================================================================
    // Class-specific descriptors 2.0
    //================================================================================================================
    public class CyACHeaderDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private UInt16 m_bcdADC;
        private byte m_bCategory;
        private UInt16 m_wTotalLength;
        private byte m_bmControls;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Audio Device Class Specification Release Number in Binary-Coded Decimal.")]
        [DefaultValue(typeof(UInt16),"0")]
        public UInt16 bcdADC
        {
            get { return m_bcdADC; }
            set { m_bcdADC = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant, indicating the primary use of this audio function, as intended by the manufacturer.")]
        [DefaultValue((byte)0)]
        public byte bCategory
        {
            get { return m_bCategory; }
            set { m_bCategory = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [ReadOnly(true)]
        [Description("Total number of bytes returned for the class-specific AudioControl interface descriptor.")]
        public ushort wTotalLength
        {
            get { return m_wTotalLength; }
            set { m_wTotalLength = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Latency Control.")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACHeaderDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.HEADER;
            bLength = 9;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Header 2.0";
        }
    }

    //================================================================================================================
    public class CyACClockSourceDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bClockID;
        private byte m_bmAttributes;
        private byte m_bmControls;
        private byte m_bAssocTerminal;
        private byte m_iClockSource;

        private string m_sClockSource;
        public uint iwClockSource; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        [Description("")]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Clock Source Entity within the audio function. " + 
                     "This value is used in all requests to address this Entity.")]
        [DefaultValue((byte)0)]
        public byte bClockID
        {
            get { return m_bClockID; }
            set { m_bClockID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmAttributes
        {
            get { return m_bmAttributes; }
            set { m_bmAttributes = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Terminal ID of the Terminal that is associated with this Clock Source.")]
        [DefaultValue((byte)0)]
        public byte bAssocTerminal
        {
            get { return m_bAssocTerminal; }
            set { m_bAssocTerminal = value; }
        }

        [Browsable(false)]
        public byte iClockSource
        {
            get { return m_iClockSource; }
            set { m_iClockSource = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iClockSource"),
         Description("String descriptor that describes the Clock Source Entity.")]
        public string sClockSource
        {
            get { return m_sClockSource; }
            set { m_sClockSource = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACClockSourceDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.CLOCK_SOURCE;
            bLength = 8;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Clock Source 2.0";
        }
    }
    
    //================================================================================================================
    public class CyACClockSelectorDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bClockID;
        private byte m_bNrInPins;
        private List<byte> m_baCSourceID;
        private byte m_bmControls;
        private byte m_iClockSelector;

        private string m_sClockSelector;
        public uint iwClockSelector; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bClockID
        {
            get { return m_bClockID; }
            set { m_bClockID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNrInPins
        {
            get { return m_bNrInPins; }
            set
            {
                m_bNrInPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baCSourceID, m_bNrInPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Editor(typeof(System.ComponentModel.Design.ArrayEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public byte[] baCSourceID
        {
            get { return m_baCSourceID.ToArray(); }
            set
            {
                m_baCSourceID = new List<byte>(value);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iClockSelector
        {
            get { return m_iClockSelector; }
            set { m_iClockSelector = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iClockSelector")]
        public string sClockSelector
        {
            get { return m_sClockSelector; }
            set { m_sClockSelector = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACClockSelectorDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.CLOCK_SELECTOR;
            m_baCSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(7 + m_baCSourceID.Count);  // 7 + p
        }

        public override string ToString()
        {
            return "AC Clock Selector 2.0";
        }
    }

    //================================================================================================================
    public class CyACClockMultiplierDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bClockID;
        private byte m_bCSourceID;
        private byte m_bmControls;
        private byte m_iClockMultiplier;

        private string m_sClockMultiplier;
        public uint iwClockMultiplier; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        [Description("")]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Clock Source Entity within the audio function. " +
                     "This value is used in all requests to address this Entity.")]
        [DefaultValue((byte)0)]
        public byte bClockID
        {
            get { return m_bClockID; }
            set { m_bClockID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bCSourceID
        {
            get { return m_bCSourceID; }
            set { m_bCSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iClockMultiplier
        {
            get { return m_iClockMultiplier; }
            set { m_iClockMultiplier = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iClockMultiplier"),
         Description("String descriptor that describes the Clock Multiplier Entity.")]
        public string sClockMultiplier
        {
            get { return m_sClockMultiplier; }
            set { m_sClockMultiplier = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACClockMultiplierDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.CLOCK_MULTIPLIER;
            bLength = 7;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Clock Multiplier 2.0";
        }
    }

    //================================================================================================================
    public class CyACInputTerminalDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bTerminalID;
        private UInt16 m_wTerminalType;
        private byte m_bAssocTerminal;
        private byte m_bCSourceID;
        private byte m_bNrChannels;
        private UInt32 m_bmChannelConfig;
        private byte m_iChannelNames;
        private UInt16 m_bmControls;
        private byte m_iTerminal;

        private string m_sChannelNames;
        private string m_sTerminal;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwTerminal; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Terminal within the audio function.")]
        [DefaultValue((byte)0)]
        public byte bTerminalID
        {
            get { return m_bTerminalID; }
            set { m_bTerminalID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant characterizing the type of Terminal.")]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wTerminalType
        {
            get { return m_wTerminalType; }
            set { m_wTerminalType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Output Terminal to which this Input Terminal is associated.")]
        [DefaultValue((byte)0)]
        public byte bAssocTerminal
        {
            get { return m_bAssocTerminal; }
            set { m_bAssocTerminal = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Clock Entity to which this Input Terminal is connected.")]
        [DefaultValue((byte)0)]
        public byte bCSourceID
        {
            get { return m_bCSourceID; }
            set { m_bCSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of logical output channels in the Terminal's output audio channel cluster.")]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Describes the spatial location of the logical channels.")]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmChannelConfig
        {
            get { return m_bmChannelConfig; }
            set { m_bmChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iTerminal
        {
            get { return m_iTerminal; }
            set { m_iTerminal = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iChannelNames"),
         Description("String descriptor that describes the name of the first logical channel.")]
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iTerminal"),
         Description("String descriptor that describes the Input Terminal.")]
        public string sTerminal
        {
            get { return m_sTerminal; }
            set { m_sTerminal = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACInputTerminalDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.INPUT_TERMINAL;
            bLength = 17;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Input Terminal 2.0";
        }
    }

    //================================================================================================================
    public class CyACOutputTerminalDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bTerminalID;
        private UInt16 m_wTerminalType;
        private byte m_bAssocTerminal;
        private byte m_bSourceID;
        private byte m_bCSourceID;
        private UInt16 m_bmControls;
        private byte m_iTerminal;

        private string m_sTerminal;
        public uint iwTerminal; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        [Description("")]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Terminal within the audio function.")]
        [DefaultValue((byte)0)]
        public byte bTerminalID
        {
            get { return m_bTerminalID; }
            set { m_bTerminalID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant characterizing the type of Terminal.")]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wTerminalType
        {
            get { return m_wTerminalType; }
            set { m_wTerminalType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant, identifying the Input Terminal to which this Output Terminal is associated.")]
        [DefaultValue((byte)0)]
        public byte bAssocTerminal
        {
            get { return m_bAssocTerminal; }
            set { m_bAssocTerminal = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Unit or Terminal to which this Terminal is connected.")]
        [DefaultValue((byte)0)]
        public byte bSourceID
        {
            get { return m_bSourceID; }
            set { m_bSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Clock Entity to which this Output Terminal is connected.")]
        [DefaultValue((byte)0)]
        public byte bCSourceID
        {
            get { return m_bCSourceID; }
            set { m_bCSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iTerminal
        {
            get { return m_iTerminal; }
            set { m_iTerminal = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iTerminal"),
         Description("String descriptor that describes the Output Terminal.")]
        public string sTerminal
        {
            get { return m_sTerminal; }
            set { m_sTerminal = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACOutputTerminalDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.OUTPUT_TERMINAL;
            bLength = 12;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Output Terminal 2.0";
        }
    }

    //================================================================================================================
    public class CyACMixerUnitDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt32 m_bmChannelConfig;
        private byte m_iChannelNames;
        private List<byte> m_bmMixerControls;
        private byte m_bmControls;
        private byte m_iMixer;

        private string m_sChannelNames;
        private string m_sMixer;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwMixer; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNrInPins
        {
            get { return m_bNrInPins; }
            set
            {
                m_bNrInPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set
            {
                m_baSourceID = new List<byte>(value);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmChannelConfig
        {
            get { return m_bmChannelConfig; }
            set { m_bmChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Bit map indicating which Mixer Controls are programmable.")]
        public List<byte> bmMixerControls
        {
            get { return m_bmMixerControls; }
            set
            {
                m_bmMixerControls = value;
                UpdateLength();
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iMixer
        {
            get { return m_iMixer; }
            set { m_iMixer = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iChannelNames")]
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iMixer")]
        public string sMixer
        {
            get { return m_sMixer; }
            set { m_sMixer = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACMixerUnitDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.MIXER_UNIT;
            m_baSourceID = new List<byte>();
            m_bmMixerControls = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(13 + m_bmMixerControls.Count + m_baSourceID.Count);  // 13 + p + n
        }

        public override string ToString()
        {
            return "AC Mixer Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyACSelectorUnitDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bmControls;
        private byte m_iSelector;

        private string m_sSelector;
        public uint iwSelector; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNrInPins
        {
            get { return m_bNrInPins; }
            set
            {
                m_bNrInPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set { m_baSourceID = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iSelector
        {
            get { return m_iSelector; }
            set { m_iSelector = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iSelector")]
        public string sSelector
        {
            get { return m_sSelector; }
            set { m_sSelector = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACSelectorUnitDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.SELECTOR_UNIT;
            m_baSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(7 + m_baSourceID.Count);  // 7 + p
        }

        public override string ToString()
        {
            return "AC Selector Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyACFeatureUnitDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bSourceID;
        private List<uint> m_bmaControls;
        private byte m_iFeature;

        private string m_sFeature;
        public uint iwFeature; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bSourceID
        {
            get { return m_bSourceID; }
            set { m_bSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public uint[] bmaControls
        {
            get { return m_bmaControls.ToArray(); }
            set
            {
                m_bmaControls = new List<uint>(value);
                UpdateLength();
            }
        }

        [Browsable(false)]
        public byte iFeature
        {
            get { return m_iFeature; }
            set { m_iFeature = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iFeature")]
        public string sFeature
        {
            get { return m_sFeature; }
            set { m_sFeature = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACFeatureUnitDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.FEATURE_UNIT;
            m_bmaControls = new List<uint>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(6 + 4 * bmaControls.Length); // 6 + 4*p
        }

        public override string ToString()
        {
            return "AC Feature Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyACSamplingRateConverterDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bSourceID;
        private byte m_bCSourceInID;
        private byte m_bCSourceOutID;
        private byte m_iSRC;

        private string m_sSRC;
        public uint iwSRC; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Unit within the audio function. This value is used in all" +
                     " requests to address this Unit.")]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Unit or Terminal to which this SRC Unit is connected.")]
        [DefaultValue((byte)0)]
        public byte bSourceID
        {
            get { return m_bSourceID; }
            set { m_bSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Clock Entity to which this SRC Unit input section is connected.")]
        [DefaultValue((byte)0)]
        public byte bCSourceInID
        {
            get { return m_bCSourceInID; }
            set { m_bCSourceInID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("ID of the Clock Entity to which this SRC Unit output section is connected.")]
        [DefaultValue((byte)0)]
        public byte bCSourceOutID
        {
            get { return m_bCSourceOutID; }
            set { m_bCSourceOutID = value; }
        }

        [Browsable(false)]
        public byte iSRC
        {
            get { return m_iSRC; }
            set { m_iSRC = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iSRC")]
        [Description("Index of a string descriptor, describing the SRC Unit.")]
        public string sSRC
        {
            get { return m_sSRC; }
            set { m_sSRC = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACSamplingRateConverterDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.SAMPLE_RATE_CONVERTER;
            bLength = 8;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Sampling Rate Converter 2.0";
        }
    }

    //================================================================================================================
    public class CyACEffectUnitDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private CyUSBOtherTypes.CyEffectUnitEffectTypes m_wEffectType;
        private byte m_bSourceID;
        private List<UInt32> m_bmaControls;
        private byte m_iEffects;

        private string m_sEffects;
        public uint iwEffects; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [TypeConverter(typeof(CyEnumConverter))]
        [DefaultValue(typeof(CyUSBOtherTypes.CyEffectUnitEffectTypes), "EFFECT_UNDEFINED")]
        public CyUSBOtherTypes.CyEffectUnitEffectTypes wEffectType
        {
            get { return m_wEffectType; }
            set { m_wEffectType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bSourceID
        {
            get { return m_bSourceID; }
            set { m_bSourceID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public UInt32[] bmaControls
        {
            get { return m_bmaControls.ToArray(); }
            set 
            { 
                m_bmaControls = new List<UInt32>(value);
                UpdateLength();
            }
        }

        [Browsable(false)]
        public byte iEffects
        {
            get { return m_iEffects; }
            set { m_iEffects = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iEffects")]
        public string sEffects
        {
            get { return m_sEffects; }
            set { m_sEffects = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACEffectUnitDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.EFFECT_UNIT;
            m_bmaControls = new List<UInt32>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(8 + 4*bmaControls.Length); // 8 + 4*ch
        }

        public override string ToString()
        {
            return "AC Effect Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyACProcessingUnitDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private UInt16 m_wProcessType;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt32 m_bmChannelConfig;
        private byte m_iChannelNames;
        private UInt16 m_bmControls;
        private byte m_iProcessing;

        private string m_sChannelNames;
        private string m_sProcessing;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwProcessing; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Unit within the audio function. This value is used in " +
                     "all requests to address this Unit.")]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant identifying the type of processing this Unit is performing.")]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wProcessType
        {
            get { return m_wProcessType; }
            set { m_wProcessType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of Input Pins of this Unit.")]
        [DefaultValue((byte)0)]
        public byte bNrInPins
        {
            get { return m_bNrInPins; }
            set
            {
                m_bNrInPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("IDs of the Unit or Terminal to which Input Pins of this Processing Unit are connected.")]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set { m_baSourceID = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of logical output channels in the audio channel cluster of the Processing Unit.")]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Describes the spatial location of the logical channels in the audio channel cluster of the " +
                     "Processing Unit.")]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmChannelConfig
        {
            get { return m_bmChannelConfig; }
            set { m_bmChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iProcessing
        {
            get { return m_iProcessing; }
            set { m_iProcessing = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iChannelNames")]
        [Description("Index of a string descriptor that describes the name of the first logical channel in the " +
                     "audio channel cluster of the Processing Unit.")]
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iProcessing")]
        [Description("Index of a string descriptor, describing this Processing Unit.")]
        public string sProcessing
        {
            get { return m_sProcessing; }
            set { m_sProcessing = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACProcessingUnitDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.PROCESSING_UNIT;
            m_baSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(17 + bNrInPins); // 17 + p
        }

        public override string ToString()
        {
            return "AC Processing Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyACExtensionDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 m_bDescriptorSubtype;
        private byte m_bUnitID;
        private UInt16 m_wExtensionCode;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt32 m_bmChannelConfig;
        private byte m_iChannelNames;
        private byte m_bmControls;
        private byte m_iExtension;

        private string m_sChannelNames;
        private string m_sExtension;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwExtension; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2 bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant uniquely identifying the Unit within the audio function. This value is used in all " +
                     "requests to address this Unit.")]
        [DefaultValue((byte)0)]
        public byte bUnitID
        {
            get { return m_bUnitID; }
            set { m_bUnitID = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Vendor-specific code identifying the Extension Unit.")]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 wExtensionCode
        {
            get { return m_wExtensionCode; }
            set { m_wExtensionCode = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of Input Pins of this Unit.")]
        [DefaultValue((byte)0)]
        public byte bNrInPins
        {
            get { return m_bNrInPins; }
            set
            {
                m_bNrInPins = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baSourceID, m_bNrInPins);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("IDs of the Unit or Terminal to which the Input Pins of this Extension Unit are connected.")]
        public byte[] baSourceID
        {
            get { return m_baSourceID.ToArray(); }
            set { m_baSourceID = new List<byte>(value); }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of logical output channels in the audio channel cluster of the Extension Unit.")]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Describes the spatial location of the logical channels in the audio channel cluster of the " +
                     "Extension Unit.")]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmChannelConfig
        {
            get { return m_bmChannelConfig; }
            set { m_bmChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Browsable(false)]
        public byte iExtension
        {
            get { return m_iExtension; }
            set { m_iExtension = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iChannelNames")]
        [Description("Index of a string descriptor, describing the name of the first logical channel in the " +
                     "audio channel cluster of the Extension Unit.")]
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iExtension")]
        [Description("Index of a string descriptor, describing this Extension Unit.")]
        public string sExtension
        {
            get { return m_sExtension; }
            set { m_sExtension = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACExtensionDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes_v2.EXTENSION_UNIT;
            m_baSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(16 + bNrInPins); // 16 + p
        }

        public override string ToString()
        {
            return "AC Extension Unit 2.0";
        }
    }

    //================================================================================================================
    public class CyASGeneralDescriptor_v2 : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bTerminalLink;
        private byte m_bmControls;
        private byte m_bFormatType;
        private UInt32 m_bmFormats;
        private byte m_bNrChannels;
        private UInt32 m_bmChannelConfig;
        private byte m_iChannelNames;
        
        private string m_sChannelNames;
        public uint iwChannelNames; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("The Terminal ID of the Terminal to which this interface is connected.")]
        [DefaultValue((byte)0)]
        public byte bTerminalLink
        {
            get { return m_bTerminalLink; }
            set { m_bTerminalLink = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [DefaultValue((byte)0)]
        public byte bmControls
        {
            get { return m_bmControls; }
            set { m_bmControls = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Constant identifying the Format Type the AudioStreaming interface is using.")]
        [DefaultValue((byte)0)]
        public byte bFormatType
        {
            get { return m_bFormatType; }
            set { m_bFormatType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("The Audio Data Format(s) that can be used to communicate with this interface.")]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmFormats
        {
            get { return m_bmFormats; }
            set { m_bmFormats = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Number of physical channels in the AS Interface audio channel cluster.")]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Describes the spatial location of the physical channels.")]
        [DefaultValue(typeof(UInt32), "0")]
        public UInt32 bmChannelConfig
        {
            get { return m_bmChannelConfig; }
            set { m_bmChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iChannelNames")]
        [Description("Index of a string descriptor, describing the name of the first physical channel.")]
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyASGeneralDescriptor_v2()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes.AS_GENERAL;
            bLength = 16;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AS General 2.0";
        }
    }

    //================================================================================================================
}
