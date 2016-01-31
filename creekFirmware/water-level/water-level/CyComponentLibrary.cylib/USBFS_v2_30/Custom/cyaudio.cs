/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace USBFS_v2_30
{
    public class CyAudioInterfaceDescriptor : CyInterfaceDescriptor
    {
        /// <summary>
        /// This property is used in property grid to display enum values list
        /// </summary>
        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        [DisplayName("bInterfaceSubClass")]
        [TypeConverter(typeof(CyInterfaceSubClassConverter))]
        [XmlIgnore]
        public byte bInterfaceSubClassWrapper
        {
            get { return bInterfaceSubClass; }
            set { bInterfaceSubClass = value; }
        }

        public override string ToString()
        {
            StringBuilder title = new StringBuilder( string.Format("Alternate Settings {0}", bAlternateSetting));
            switch (bInterfaceSubClass)
            {
                case (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOCONTROL:
                    title.Insert(0, "AC ");
                    break;
                case (byte)CyUSBOtherTypes.CyAudioSubclassCodes.AUDIOSTREAMING:
                    title.Insert(0, "AS ");
                    break;
                case (byte)CyUSBOtherTypes.CyAudioSubclassCodes.MIDISTREAMING:
                    title.Insert(0, "MS ");
                    break;
                default:
                    break;
            }
            return title.ToString();
        }
    }

    //================================================================================================================
    // Class-specific descriptors 1.0
    //================================================================================================================

    public class CyACHeaderDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private UInt16 m_bcdADC;
        private UInt16 m_wTotalLength;
        private byte m_bInCollection;
        private List<byte> m_baInterfaceNr;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Audio Device Class Specification Release Number in Binary-Coded Decimal.")]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 bcdADC
        {
            get { return m_bcdADC; }
            set { m_bcdADC = value; }
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
        [Description("The number of AudioStreaming and MIDIStreaming interfaces in the Audio Interface Collection " +
                     "to which this AudioControl interface belongs")]
        [DefaultValue((byte)0)]
        public byte bInCollection
        {
            get { return m_bInCollection; }
            set 
            {
                m_bInCollection = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_baInterfaceNr, m_bInCollection);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Interface numbers of the AudioStreaming or MIDIStreaming interfaces in the Collection.")]
        public byte[] baInterfaceNr
        {
            get { return m_baInterfaceNr.ToArray(); }
            set { m_baInterfaceNr = new List<byte>(value); }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACHeaderDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.HEADER;
            m_baInterfaceNr = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(8 + m_bInCollection); // 8 + n
        }

        public override string ToString()
        {
            return "AC Header";
        }
    }

    //================================================================================================================

    public class CyACInputTerminalDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bTerminalID;
        private UInt16 m_wTerminalType;
        private byte m_bAssocTerminal;
        private byte m_bNrChannels;
        private UInt16 m_wChannelConfig;
        private byte m_iChannelNames;
        private byte m_iTerminal;

        private string m_sChannelNames;
        private string m_sTerminal;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwTerminal; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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
        [Description("Number of logical output channels in the Terminal's output audio channel cluster.")]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("Describes the spatial location of the logical channels.")]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wChannelConfig
        {
            get { return m_wChannelConfig; }
            set { m_wChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
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

        public CyACInputTerminalDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.INPUT_TERMINAL;
            bLength = 12;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Input Terminal";
        }
    }

    //================================================================================================================

    public class CyACOutputTerminalDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bTerminalID;
        private UInt16 m_wTerminalType;
        private byte m_bAssocTerminal;
        private byte m_bSourceID;
        private byte m_iTerminal;

        private string m_sTerminal;
        public uint iwTerminal; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        [Description("")]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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

        public CyACOutputTerminalDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.OUTPUT_TERMINAL;
            bLength = 9;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AC Output Terminal";
        }
    }

    //================================================================================================================
    public class CyACMixerUnitDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt16 m_wChannelConfig;
        private byte m_iChannelNames;
        private List<byte> m_bmControls;
        private byte m_iMixer;

        private string m_sChannelNames;
        private string m_sMixer;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwMixer; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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
        [DefaultValue(typeof(ushort), "0")]
        public ushort wChannelConfig
        {
            get { return m_wChannelConfig; }
            set { m_wChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public List<byte> bmControls
        {
            get { return m_bmControls; }
            set
            {
                m_bmControls = value;
                UpdateLength();
            }
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

        public CyACMixerUnitDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.MIXER_UNIT;
            m_baSourceID = new List<byte>();
            m_bmControls = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(10 + m_bmControls.Count + m_baSourceID.Count);  // 10 + p + n
        }

        public override string ToString()
        {
            return "AC Mixer Unit";
        }
    }

    //================================================================================================================
    public class CyACSelectorUnitDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_iSelector;

        private string m_sSelector;
        public uint iwSelector; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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

        public CyACSelectorUnitDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.SELECTOR_UNIT;
            m_baSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(6 + m_baSourceID.Count);  // 6 + p
        }

        public override string ToString()
        {
            return "AC Selector Unit";
        }
    }

    //================================================================================================================
    public class CyACFeatureUnitDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bUnitID;
        private byte m_bSourceID;
        private byte m_bControlSize = 1;
        private List<uint> m_bmaControls;
        private byte m_iFeature;

        private string m_sFeature;
        public uint iwFeature; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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
        [DefaultValue((byte)1)]
        public byte bControlSize
        {
            get { return m_bControlSize; }
            set
            {
                m_bControlSize = value;
                UpdateLength();
            }
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

        public CyACFeatureUnitDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.FEATURE_UNIT;
            m_bmaControls = new List<uint>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(7 + m_bControlSize * bmaControls.Length); // 7 + p*n
        }

        public override string ToString()
        {
            return "AC Feature Unit";
        }
    }

    //================================================================================================================
    public class CyACProcessingUnitDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bUnitID;
        private UInt16 m_wProcessType;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt16 m_wChannelConfig;
        private byte m_iChannelNames;
        private byte m_bControlSize = 1;
        private List<byte> m_bmControls;
        private byte m_iProcessing;

        private string m_sChannelNames;
        private string m_sProcessing;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwProcessing; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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
        [DefaultValue(typeof(ushort), "0")]
        public ushort wProcessType
        {
            get { return m_wProcessType; }
            set { m_wProcessType = value; }
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
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wChannelConfig
        {
            get { return m_wChannelConfig; }
            set { m_wChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)1)]
        public byte bControlSize
        {
            get { return m_bControlSize; }
            set
            {
                m_bControlSize = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_bmControls, m_bControlSize);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte[] bmControls
        {
            get { return m_bmControls.ToArray(); }
            set { m_bmControls = new List<byte>(value); }
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
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iProcessing")]
        public string sProcessing
        {
            get { return m_sProcessing; }
            set { m_sProcessing = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyACProcessingUnitDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.PROCESSING_UNIT;
            m_baSourceID = new List<byte>();
            m_bmControls = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(13 + bControlSize + bNrInPins); // 13 + p + n
        }

        public override string ToString()
        {
            return "AC Processing Unit";
        }
    }

    //================================================================================================================
    public class CyACExtensionDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bUnitID;
        private UInt16 m_wExtensionCode;
        private byte m_bNrInPins;
        private List<byte> m_baSourceID;
        private byte m_bNrChannels;
        private UInt16 m_wChannelConfig;
        private byte m_iChannelNames;
        private byte m_bControlSize = 1;
        private List<byte> m_bmControls;
        private byte m_iExtension;

        private string m_sChannelNames;
        private string m_sExtension;
        public uint iwChannelNames; // absolute index (for internal usage)
        public uint iwExtension; // absolute index (for internal usage)

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype
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
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 wExtensionCode
        {
            get { return m_wExtensionCode; }
            set { m_wExtensionCode = value; }
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
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(UInt16), "0")]
        public UInt16 wChannelConfig
        {
            get { return m_wChannelConfig; }
            set { m_wChannelConfig = value; }
        }

        [Browsable(false)]
        public byte iChannelNames
        {
            get { return m_iChannelNames; }
            set { m_iChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)1)]
        public byte bControlSize
        {
            get { return m_bControlSize; }
            set
            {
                m_bControlSize = value;
                UpdateLength();
                CyUSBFSParameters.ChangeArrayLength(m_bmControls, m_bControlSize);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        public byte[] bmControls
        {
            get { return m_bmControls.ToArray(); }
            set { m_bmControls = new List<byte>(value); }
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
        public string sChannelNames
        {
            get { return m_sChannelNames; }
            set { m_sChannelNames = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC),
         TypeConverter(typeof(CyStringDescConverter)),
         DisplayName("iExtension")]
        public string sExtension
        {
            get { return m_sExtension; }
            set { m_sExtension = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyACExtensionDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes.EXTENSION_UNIT;
            m_bmControls = new List<byte>();
            m_baSourceID = new List<byte>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override void UpdateLength()
        {
            bLength = (byte)(13 + m_bControlSize + bNrChannels); // 13 + p + n
        }

        public override string ToString()
        {
            return "AC Extension Unit";
        }
    }

    //================================================================================================================

    public class CyASGeneralDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private byte m_bTerminalLink;
        private byte m_bDelay;
        private UInt16 m_wFormatTag;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bTerminalLink
        {
            get { return m_bTerminalLink; }
            set { m_bTerminalLink = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bDelay
        {
            get { return m_bDelay; }
            set { m_bDelay = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wFormatTag
        {
            get { return m_wFormatTag; }
            set { m_wFormatTag = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyASGeneralDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes.AS_GENERAL;
            bLength = 7;
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AS General";
        }
    }

    //================================================================================================================

    public abstract class CyASFormatTypeBaseDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes m_bDescriptorSubtype;
        private CyUSBOtherTypes.CyFormatTypeCodes m_bFormatType;
        private byte m_bSamFreqType;
        private uint m_tLowerSamFreq;
        private uint m_tUpperSamFreq;
        private List<uint> m_tSamFreq;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyFormatTypeCodes bFormatType
        {
            get { return m_bFormatType; }
            set { m_bFormatType = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bSamFreqType
        {
            get { return m_bSamFreqType; }
            set
            {
                m_bSamFreqType = value;
                UpdateLength();
                if (m_tSamFreq.Count < m_bSamFreqType)
                {
                    int count = m_tSamFreq.Count;
                    for (int i = 0; i < m_bSamFreqType - count; i++)
                        m_tSamFreq.Add(0);
                }
                else if (m_tSamFreq.Count > m_bSamFreqType)
                    m_tSamFreq.RemoveRange(m_bSamFreqType, m_tSamFreq.Count - m_bSamFreqType);
            }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(uint), "0")]
        public uint tLowerSamFreq
        {
            get { return m_tLowerSamFreq; }
            set { m_tLowerSamFreq = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(uint), "0")]
        public uint tUpperSamFreq
        {
            get { return m_tUpperSamFreq; }
            set { m_tUpperSamFreq = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(uint), "0")]
        public uint[] tSamFreq
        {
            get { return m_tSamFreq.ToArray(); }
            set { m_tSamFreq = new List<uint>(value); }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyASFormatTypeBaseDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.AUDIO;
            bDescriptorSubtype = CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes.FORMAT_TYPE;
            m_tSamFreq = new List<uint>();
        }

        public override int GetLevel()
        {
            return AUDIO_LEVEL;
        }

        public override string ToString()
        {
            return "AS Format Type";
        }

        #region ICustomTypeDescriptor members
        public override PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this, true);
            PropertyDescriptorCollection filteredpdc = new PropertyDescriptorCollection(new PropertyDescriptor[] {});
            for (int i = 0; i < pdc.Count; i++)
            {
                bool show = true;
                if ((bSamFreqType > 0) && ((pdc[i].Name == "tLowerSamFreq") || (pdc[i].Name == "tUpperSamFreq")))
                {
                    show = false;
                }
                else if ((bSamFreqType == 0) && (pdc[i].Name == "tSamFreq"))
                {
                    show = false;
                }
                if (show)
                    filteredpdc.Add(pdc[i]);
            }
            return filteredpdc;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this,attributes, true);
            PropertyDescriptorCollection filteredpdc = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
            for (int i = 0; i < pdc.Count; i++)
            {
                bool show = true;
                if ((bSamFreqType > 0) && ((pdc[i].Name == "tLowerSamFreq") || (pdc[i].Name == "tUpperSamFreq")))
                {
                    show = false;
                }
                else if ((bSamFreqType == 0) && (pdc[i].Name == "tSamFreq"))
                {
                    show = false;
                }
                if (show)
                    filteredpdc.Add(pdc[i]);
            }
            return filteredpdc;
        }
        #endregion ICustomTypeDescriptor members
    }

    //================================================================================================================

    public class CyASFormatType1Descriptor : CyASFormatTypeBaseDescriptor
    {
        private byte m_bNrChannels;
        private byte m_bSubframeSize;
        private byte m_bBitResolution;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bNrChannels
        {
            get { return m_bNrChannels; }
            set { m_bNrChannels = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bSubframeSize
        {
            get { return m_bSubframeSize; }
            set { m_bSubframeSize = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bBitResolution
        {
            get { return m_bBitResolution; }
            set { m_bBitResolution = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyASFormatType1Descriptor()
        {
            bLength = 8;
            bFormatType = CyUSBOtherTypes.CyFormatTypeCodes.FORMAT_TYPE_1;
        }

        public override void UpdateLength()
        {
            int freqSize = bSamFreqType == 0 ? 6 : bSamFreqType*3;
            bLength = (byte)(8 + freqSize); // 8 + (ns*3)
        }

        public override string ToString()
        {
            return "AS Format Type I";
        }
    }

    //================================================================================================================

    public class CyASFormatType2Descriptor : CyASFormatTypeBaseDescriptor
    {
        private UInt16 m_wMaxBitRate;
        private UInt16 m_wSamplesPerFrame;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wMaxBitRate
        {
            get { return m_wMaxBitRate; }
            set { m_wMaxBitRate = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wSamplesPerFrame
        {
            get { return m_wSamplesPerFrame; }
            set { m_wSamplesPerFrame = value; }
        }


        //------------------------------------------------------------------------------------------------------------

        public CyASFormatType2Descriptor()
        {
            bLength = 9;
            bFormatType = CyUSBOtherTypes.CyFormatTypeCodes.FORMAT_TYPE_2;
        }

        public override void UpdateLength()
        {
            int freqSize = bSamFreqType == 0 ? 6 : bSamFreqType * 3;
            bLength = (byte)(9 + freqSize); // 9 + (ns*3)
        }

        public override string ToString()
        {
            return "AS Format Type II";
        }
    }

    //================================================================================================================

    public class CyAudioEndpointDescriptor : CyEndpointDescriptor
    {
        private byte m_bRefresh;
        private byte m_bSynchAddress;

        public CyAudioEndpointDescriptor()
        {
            bLength = 9;
        }

        #region Properties
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bRefresh
        {
            get
            {
                return m_bRefresh;
            }
            set
            {
                m_bRefresh = value;
            }
        }
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue((byte)0)]
        public byte bSynchAddress
        {
            get
            {
                return m_bSynchAddress;
            }
            set
            {
                m_bSynchAddress = value;
            }
        }
        #endregion Properties
    }
    
    //================================================================================================================

    public class CyASEndpointDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyAudioClassSpecEPDescriptorTypes m_bDescriptorSubtype;
        private byte m_bmAttributes;
        private byte m_bLockDelayUnits;
        private ushort m_wLockDelay;

        public CyASEndpointDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CS_ENDPOINT;
            m_bDescriptorSubtype = CyUSBOtherTypes.CyAudioClassSpecEPDescriptorTypes.EP_GENERAL;
            bLength = 7;
        }

        public override int GetLevel()
        {
            return 5;
        }

        public override string ToString()
        {
            return "AS Endpoint Descriptor";
        }


        #region Properties

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyAudioClassSpecEPDescriptorTypes bDescriptorSubtype
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
        [DefaultValue((byte)0)]
        public byte bLockDelayUnits
        {
            get
            {
                return m_bLockDelayUnits;
            }
            set
            {
                m_bLockDelayUnits = value;
            }
        }
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DefaultValue(typeof(ushort), "0")]
        public ushort wLockDelay
        {
            get
            {
                return m_wLockDelay;
            }
            set
            {
                m_wLockDelay = value;
            }
        }

        #endregion Properties
    }
    
    //================================================================================================================


    #region Property Converters
    internal class CyStringDescConverter : StringConverter
    {
        public static List<string> m_strDescList;

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(m_strDescList);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    internal class CyInterfaceClassConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, 
                                         Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string res = value.ToString();
                switch ((byte)value)
                {
                    case CyUSBFSParameters.CLASS_AUDIO:
                        res = "AUDIO";
                        break;
                    case CyUSBFSParameters.CLASS_CDC:
                        res = "COMMUNICATIONS";
                        break;
                    case CyUSBFSParameters.CLASS_DATA:
                        res = "DATA";
                        break;
                    default:
                        break;
                }
                return res;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    internal class CyInterfaceSubClassConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return Enum.GetName(typeof(CyUSBOtherTypes.CyAudioSubclassCodes), value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }


    #endregion Property Converters

    //================================================================================================================
}

