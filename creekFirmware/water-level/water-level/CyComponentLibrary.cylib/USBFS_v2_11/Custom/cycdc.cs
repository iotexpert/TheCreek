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
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace USBFS_v2_11
{
    #region Interface descriptors
    public class CyCDCInterfaceDescriptor : CyInterfaceDescriptor
    {
        public override string ToString()
        {
            StringBuilder title = new StringBuilder(string.Format("Alternate Settings {0}", bAlternateSetting));
            switch (bInterfaceClass)
            {
                case CyUSBFSParameters.CLASS_CDC:
                    title.Insert(0, "Communications ");
                    break;
                case CyUSBFSParameters.CLASS_DATA:
                    title.Insert(0, "Data ");
                    break;
                default:
                    break;
            }
            return title.ToString();
        }
    }

    public class CyCommunicationsInterfaceDescriptor : CyCDCInterfaceDescriptor
    {
        /// <summary>
        /// This property is used in property grid to display enum values list
        /// </summary>
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DisplayName("bInterfaceSubClass")]
        [TypeConverter(typeof(CyEnumConverter))]
        [XmlIgnore]
        public CyUSBOtherTypes.CyCommunicationSubclassCodes bInterfaceSubClassWrapper
        {
            get { return (CyUSBOtherTypes.CyCommunicationSubclassCodes)bInterfaceSubClass; }
            set { bInterfaceSubClass = (byte)value; }
        }

        /// <summary>
        /// This property is used in property grid to display enum values list
        /// </summary>
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DisplayName("bInterfaceProtocol")]
        [TypeConverter(typeof(CyEnumConverter))]
        [XmlIgnore]
        public CyUSBOtherTypes.CyCommunicationProtocolCodes bInterfaceProtocolWrapper
        {
            get { return (CyUSBOtherTypes.CyCommunicationProtocolCodes)this.bInterfaceProtocol; }
            set { this.bInterfaceProtocol = (byte)value; }
        }
    }

    public class CyDataInterfaceDescriptor : CyCDCInterfaceDescriptor
    {
        /// <summary>
        /// This property is used in property grid to display enum values list
        /// </summary>
        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [DisplayName("bInterfaceProtocol")]
        [TypeConverter(typeof(CyEnumConverter))]
        [XmlIgnore]
        public CyUSBOtherTypes.CyDataProtocolCodes bInterfaceProtocolWrapper
        {
            get { return (CyUSBOtherTypes.CyDataProtocolCodes)this.bInterfaceProtocol; }
            set { this.bInterfaceProtocol = (byte)value; }
        }
    }
    #endregion Interface descriptors

    //================================================================================================================

    #region CDC Class Descriptors

    public class CyCDCClassDescriptor : CyUSBDescriptor
    {
        private CyUSBOtherTypes.CyCommunicationClassSubtypeCodes m_bDescriptorSubtype;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBOtherTypes.CyCommunicationClassSubtypeCodes bDescriptorSubtype
        {
            get { return m_bDescriptorSubtype; }
            set { m_bDescriptorSubtype = value; }
        }

        public override int GetLevel()
        {
            return CDC_LEVEL;
        }
    }

    //================================================================================================================

    public class CyCDCHeaderDescriptor : CyCDCClassDescriptor
    {
        private UInt16 m_bcdADC;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description(
         "USB Class Definitions for Communications Devices Specification release number in binary-coded decimal.")]
        public UInt16 bcdADC
        {
            get { return m_bcdADC; }
            set { m_bcdADC = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyCDCHeaderDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CDC;
            bDescriptorSubtype = CyUSBOtherTypes.CyCommunicationClassSubtypeCodes.HEADER;
            bLength = 5;
        }

        public override string ToString()
        {
            return "Header";
        }
    }

    //================================================================================================================\
    
    public class CyCDCUnionDescriptor : CyCDCClassDescriptor
    {
        private byte m_bControlInterface;
        private List<byte> m_bSubordinateInterface;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("The interface number of the Communications or Data Class interface," + 
                     " designated as the controlling interface for the union.")]
        public byte bControlInterface
        {
            get { return m_bControlInterface; }
            set { m_bControlInterface = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        [Editor(typeof(System.ComponentModel.Design.ArrayEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public byte[] bSubordinateInterface
        {
            get { return m_bSubordinateInterface.ToArray(); }
            set
            {
                m_bSubordinateInterface = new List<byte>(value);
                UpdateLength();
            }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyCDCUnionDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CDC;
            bDescriptorSubtype = CyUSBOtherTypes.CyCommunicationClassSubtypeCodes.UNION;
            m_bSubordinateInterface = new List<byte>();
        }

        public override void UpdateLength()
        {
            bLength = (byte)(4 + m_bSubordinateInterface.Count);
        }

        public override string ToString()
        {
            return "Union";
        }
    }

    //================================================================================================================

    public class CyCDCCountrySelectionDescriptor : CyCDCClassDescriptor
    {
        private byte m_iCountryCodeRelDate;
        private List<UInt16> m_wCountryCode;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public byte iCountryCodeRelDate
        {
            get { return m_iCountryCodeRelDate; }
            set { m_iCountryCodeRelDate = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public UInt16[] wCountryCode
        {
            get { return m_wCountryCode.ToArray(); }
            set 
            { 
                m_wCountryCode = new List<UInt16>(value);
                UpdateLength();
            }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyCDCCountrySelectionDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CDC;
            bDescriptorSubtype = CyUSBOtherTypes.CyCommunicationClassSubtypeCodes.COUNTRY_SELECTION;
            m_wCountryCode = new List<UInt16>();
        }

        public override void UpdateLength()
        {
            bLength = (byte)(4 + m_wCountryCode.Count * 2);
        }

        public override string ToString()
        {
            return "Country Selection";
        }
    }

    //================================================================================================================

    public class CyCDCCallManagementDescriptor : CyCDCClassDescriptor
    {
        private byte m_bmCapabilities;
        private byte m_bDataInterface;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public byte bmCapabilities
        {
            get { return m_bmCapabilities; }
            set { m_bmCapabilities = value; }
        }

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public byte bDataInterface
        {
            get { return m_bDataInterface; }
            set { m_bDataInterface = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyCDCCallManagementDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CDC;
            bDescriptorSubtype = CyUSBOtherTypes.CyCommunicationClassSubtypeCodes.CALL_MANAGEMENT;
            bLength = 5;
        }

        public override string ToString()
        {
            return "Call Management";
        }
    }

    //================================================================================================================

    public class CyCDCAbstractControlMgmtDescriptor : CyCDCClassDescriptor
    {
        private byte m_bmCapabilities;

        [Category(CyUSBOtherTypes.CATEGORY_SPECIFIC)]
        [Description("")]
        public byte bmCapabilities
        {
            get { return m_bmCapabilities; }
            set { m_bmCapabilities = value; }
        }

        //------------------------------------------------------------------------------------------------------------

        public CyCDCAbstractControlMgmtDescriptor()
        {
            bDescriptorType = CyUSBDescriptorType.CDC;
            bDescriptorSubtype = CyUSBOtherTypes.CyCommunicationClassSubtypeCodes.ABSTRACT_CONTROL_MANAGEMENT;
            bLength = 4;
        }

        public override string ToString()
        {
            return "Abstract Control Management";
        }
    }
    #endregion CDC Class Descriptors

    //================================================================================================================

    #region Type Converters

    class CyEnumConverter : EnumConverter
    {
        private Type m_enumType;

        public CyEnumConverter(Type type)
            : base(type)
        {
            m_enumType = type;
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, 
                                         Type destType)
        {
            FieldInfo fi = m_enumType.GetField(Enum.GetName(m_enumType, value));
            DescriptionAttribute dna =
                (DescriptionAttribute) Attribute.GetCustomAttribute(fi, typeof (DescriptionAttribute));
            if (dna != null)
                return dna.Description;
            else
                return value.ToString();
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
        {
            return srcType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (FieldInfo fi in m_enumType.GetFields())
            {
                DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                if ((dna != null) && ((string)value == dna.Description))
                    return Enum.Parse(m_enumType, fi.Name);
            }
            return Enum.Parse(m_enumType, (string)value);
        }
    }
    #endregion
}
