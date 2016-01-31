/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace USBFS_v1_50
{
    [Serializable()]
    [XmlInclude(typeof(CyDeviceDescriptor))]
    [XmlInclude(typeof(CyConfigDescriptor))]
    [XmlInclude(typeof(CyEndpointDescriptor))]
    [XmlInclude(typeof(CyInterfaceDescriptor))]
    [XmlInclude(typeof(CyInterfaceGeneralDescriptor))]
    [XmlInclude(typeof(CyStringDescriptor))]
    [XmlInclude(typeof(CyStringZeroDescriptor))]
    [XmlInclude(typeof(CyHIDDescriptor))]
    [XmlInclude(typeof(CyHIDReportDescriptor))]
    [XmlInclude(typeof(CyHIDReportItemDescriptor))]

    [XmlInclude(typeof(CyAudioInterfaceDescriptor))]
    [XmlInclude(typeof(CyACInputTerminalDescriptor))]
    [XmlInclude(typeof(CyACOutputTerminalDescriptor))]
    [XmlInclude(typeof(CyACHeaderDescriptor))]
    [XmlInclude(typeof(CyACMixerUnitDescriptor))]
    [XmlInclude(typeof(CyACSelectorUnitDescriptor))]
    [XmlInclude(typeof(CyACFeatureUnitDescriptor))]
    [XmlInclude(typeof(CyACProcessingUnitDescriptor))]
    [XmlInclude(typeof(CyACExtensionDescriptor))]
    [XmlInclude(typeof(CyASGeneralDescriptor))]
    [XmlInclude(typeof(CyASFormatTypeBaseDescriptor))]
    [XmlInclude(typeof(CyASFormatType1Descriptor))]
    [XmlInclude(typeof(CyASFormatType2Descriptor))]

    [XmlType("USBDescriptor")]
    public abstract class CyUSBDescriptor : ICloneable
    {
        protected const int AUDIO_LEVEL = 4;
        private CyUSBDescriptorType m_bDescriptorType;
        private byte m_bLength;

        [ReadOnly(true)]
        [Category(CyUSBOtherTypes.CATEGORY_COMMON)]
        public CyUSBDescriptorType bDescriptorType
        {
            get
            {
                return m_bDescriptorType;
            }
            set
            {
                m_bDescriptorType = value;
            }
        }

        [Browsable(false)]
        public byte bLength
        {
            get
            {
                return m_bLength;
            }
            set
            {
                m_bLength = value;
            }
        }

        public abstract int GetLevel();
        public virtual void UpdateLength()
        {
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
