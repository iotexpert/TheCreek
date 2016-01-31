/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace USBFS_v2_12
{
    [XmlType("MSHeader_Descriptor")]
    public class CyMSHeaderDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bcdMSC;
        public string wTotalLength;

        public CyMSHeaderDescriptorTemplate()
        {
        }

        public CyMSHeaderDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyMSHeaderDescriptor desc = (CyMSHeaderDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bcdMSC = UshortToStr(desc.bcdMSC);
            wTotalLength = UshortToStr(desc.wTotalLength);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyMSHeaderDescriptor usbDesc = new CyMSHeaderDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bcdMSC = StrToHex2B(bcdMSC);
            usbDesc.wTotalLength = StrToHex2B(wTotalLength);
            return usbDesc;
        }
    }

    [XmlType("MIDIInJack_Descriptor")]
    public class CyMSInJackDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bJackType;
        public string bJackID;
        public string iJack;

        public CyMSInJackDescriptorTemplate()
        {
        }

        public CyMSInJackDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyMSInJackDescriptor desc = (CyMSInJackDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bJackType = ByteToStr((byte)desc.bJackType);
            bJackID = ByteToStr(desc.bJackID);
            iJack = desc.sJack;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyMSInJackDescriptor usbDesc = new CyMSInJackDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bJackType = (CyUSBOtherTypes.CyJackTypes)StrToHex1B(bJackType);
            usbDesc.bJackID = StrToHex1B(bJackID);
            usbDesc.sJack = iJack;
            return usbDesc;
        }
    }

    [XmlType("MIDIOutJack_Descriptor")]
    public class CyMSOutJackDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bJackType;
        public string bJackID;
        public string iJack;
        public string bNrInputPins;
        public string baSourceID;
        public string baSourcePin;

        public CyMSOutJackDescriptorTemplate()
        {
        }

        public CyMSOutJackDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyMSOutJackDescriptor desc = (CyMSOutJackDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bJackType = ByteToStr((byte)desc.bJackType);
            bJackID = ByteToStr(desc.bJackID);
            bNrInputPins = ByteToStr(desc.bNrInputPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            baSourcePin = ByteArrayToStr(desc.baSourcePin);
            iJack = desc.sJack;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyMSOutJackDescriptor usbDesc = new CyMSOutJackDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bJackType = (CyUSBOtherTypes.CyJackTypes)StrToHex1B(bJackType);
            usbDesc.bJackID = StrToHex1B(bJackID);
            usbDesc.bNrInputPins = StrToHex1B(bNrInputPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.baSourcePin = StrToHex1BArray(baSourcePin);
            usbDesc.sJack = iJack;
            return usbDesc;
        }
    }

    [XmlType("MIDIElement_Descriptor")]
    public class CyMSElementDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyMSInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bElementID;
        public string bNrInputPins;
        public string baSourceID;
        public string baSourcePin;

        public string bNrOutPins;
        public string bInTerminalLink;
        public string bOutTerminalLink;
        public string bElCapsSize;
        public string bmElementCaps;

        public string iElement;

        public CyMSElementDescriptorTemplate()
        {
        }

        public CyMSElementDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyMSElementDescriptor desc = (CyMSElementDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bElementID = ByteToStr(desc.bElementID);
            bNrInputPins = ByteToStr(desc.bNrInputPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            baSourcePin = ByteArrayToStr(desc.baSourcePin);

            bNrOutPins = ByteToStr(desc.bNrOutPins);
            bInTerminalLink = ByteToStr(desc.bInTerminalLink);
            bOutTerminalLink = ByteToStr(desc.bOutTerminalLink);
            bElCapsSize = ByteToStr(desc.bElCapsSize);
            bmElementCaps = ByteArrayToStr(desc.bmElementCaps);

            iElement = desc.sElement;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyMSElementDescriptor usbDesc = new CyMSElementDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bElementID = StrToHex1B(bElementID);
            usbDesc.bNrInputPins = StrToHex1B(bNrInputPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.baSourcePin = StrToHex1BArray(baSourcePin);

            usbDesc.bNrOutPins = StrToHex1B(bElementID);
            usbDesc.bInTerminalLink = StrToHex1B(bInTerminalLink);
            usbDesc.bOutTerminalLink = StrToHex1B(bOutTerminalLink);
            usbDesc.bElCapsSize = StrToHex1B(bElCapsSize);
            usbDesc.bmElementCaps = StrToHex1BArray(bmElementCaps);

            usbDesc.sElement = iElement;
            return usbDesc;
        }
    }

    [XmlType("MSBulkDataEndpoint_Descriptor")]
    public class CyMSEndpointDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyMSClassSpecEPDescriptorTypes bDescriptorSubtype;
        public string bNumEmbMIDIJack;
        public string baAssocJackID;

        public CyMSEndpointDescriptorTemplate()
        {
        }

        public CyMSEndpointDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyMSEndpointDescriptor desc = (CyMSEndpointDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bNumEmbMIDIJack = ByteToStr(desc.bNumEmbMIDIJack);
            baAssocJackID = ByteArrayToStr(desc.baAssocJackID);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyMSEndpointDescriptor usbDesc = new CyMSEndpointDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bNumEmbMIDIJack = StrToHex1B(bNumEmbMIDIJack);
            usbDesc.baAssocJackID = StrToHex1BArray(baAssocJackID);
            return usbDesc;
        }
    }

}
