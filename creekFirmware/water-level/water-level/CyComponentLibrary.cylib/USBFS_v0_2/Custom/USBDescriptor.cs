/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;

namespace USBFS_v0_2
{
    [Serializable()]
    [XmlInclude(typeof(DeviceDescriptor))]
    [XmlInclude(typeof(ConfigDescriptor))]
    [XmlInclude(typeof(EndpointDescriptor))]
    [XmlInclude(typeof(InterfaceDescriptor))]
    //[XmlInclude(typeof(InterfaceAssociationDescriptor))]
    [XmlInclude(typeof(StringsDescriptor))]
    [XmlInclude(typeof(StringDescriptor))]
    [XmlInclude(typeof(StringZeroDescriptor))]
    [XmlInclude(typeof(HIDDescriptor))]
    [XmlInclude(typeof(HIDReportDescriptor))]
    [XmlInclude(typeof(HIDReportItemDescriptor))]
    [XmlInclude(typeof(AudioDescriptor))]
    public abstract class USBDescriptor
    {
        private USBDescriptorType _bDescriptorType;
        private byte _bLength;
        
        public USBDescriptorType bDescriptorType
        {
            get
            {
                return _bDescriptorType;
            }
            set
            {
                _bDescriptorType = value;
            }
        }

        public byte bLength
        {
            get
            {
                return _bLength;
            }
            set
            {
                _bLength = value;
            }
        }
    }
}
