/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace USBFS_v1_60
{
    public partial class CyUSBFSParameters
    {
        #region Save Templates

        public void SaveDescriptor(CyUSBDescriptor desc)
        {
            CyDescriptorTemplate descTemplate = null;
            if (desc is CyDeviceDescriptor)
                descTemplate = new CyDeviceDescriptorTemplate(desc);
            else if (desc is CyConfigDescriptor)
                descTemplate = new CyConfigurationDescriptorTemplate(desc);
            else if (desc is CyInterfaceDescriptor)
                descTemplate = new CyInterfaceDescriptorTemplate(desc);
            else if (desc is CyAudioEndpointDescriptor)
                descTemplate = new CyAudioEndpointDescriptorTemplate(desc);
            else if (desc is CyASEndpointDescriptor)
                descTemplate = new CyASEndpointDescriptorTemplate(desc);
            else if (desc is CyEndpointDescriptor)
                descTemplate = new CyEndpointDescriptorTemplate(desc);
            else if (desc is CyHIDDescriptor)
                descTemplate = new CyHIDDescriptorTemplate(desc);

            XmlSerializer s = new XmlSerializer(typeof(CyDescriptorTemplate));
            using (StringWriter sw = new StringWriter())
            {
                string serializedXml;
                s.Serialize(sw, descTemplate);
                serializedXml = sw.ToString();
                SaveTemplate(serializedXml, desc.bDescriptorType);
            }
        }

        public void SaveDescriptorList(CyDescriptorNode desc, CyUSBDescriptorType kind)
        {
            List<CyDescriptorTemplate> descTemplateList = new List<CyDescriptorTemplate>();
            BuildDescriptorTemplateList(desc, descTemplateList);
            XmlSerializer s = new XmlSerializer(typeof(List<CyDescriptorTemplate>));
            using (StringWriter sw = new StringWriter())
            {
                string serializedXml;
                s.Serialize(sw, descTemplateList);
                serializedXml = sw.ToString();
                SaveTemplate(serializedXml, kind);
            }
        }

        public void SaveReport(CyDescriptorNode node_report)
        {
            XmlSerializer s = new XmlSerializer(typeof(CyReportTemplate));
            using (StringWriter sw = new StringWriter())
            {
                string serializedXml;
                CyReportTemplate report = new CyReportTemplate(node_report);
                s.Serialize(sw, report);
                serializedXml = sw.ToString();
                SaveTemplate(serializedXml, CyUSBDescriptorType.HID_REPORT);
            }
        }

        private void BuildDescriptorTemplateList(CyDescriptorNode desc, List<CyDescriptorTemplate> lst)
        {
            if (desc.m_value != null)
            {
                if (desc.m_value is CyDeviceDescriptor)
                {
                    lst.Add(new CyDeviceDescriptorTemplate(desc.m_value));
                    AddSerialToTemplateList(lst);
                }
                else if (desc.m_value is CyConfigDescriptor)
                    lst.Add(new CyConfigurationDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyInterfaceDescriptor)
                    lst.Add(new CyInterfaceDescriptorTemplate(desc.m_value));
                else if ((desc.m_value is CyEndpointDescriptor) && !(desc.m_value is CyAudioEndpointDescriptor))
                    lst.Add(new CyEndpointDescriptorTemplate(desc.m_value));

                else if (desc.m_value is CyAudioEndpointDescriptor)
                    lst.Add(new CyAudioEndpointDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyASEndpointDescriptor)
                    lst.Add(new CyASEndpointDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACHeaderDescriptor)
                    lst.Add(new CyACHeaderDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACInputTerminalDescriptor)
                    lst.Add(new CyACInputTerminalDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACOutputTerminalDescriptor)
                    lst.Add(new CyACOutputTerminalDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACMixerUnitDescriptor)
                    lst.Add(new CyACMixerUnitDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACSelectorUnitDescriptor)
                    lst.Add(new CyACSelectorUnitDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACFeatureUnitDescriptor)
                    lst.Add(new CyACFeatureUnitDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACProcessingUnitDescriptor)
                    lst.Add(new CyACProcessingUnitDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyACExtensionDescriptor)
                    lst.Add(new CyACExtensionDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyASGeneralDescriptor)
                    lst.Add(new CyASGeneralDescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyASFormatType1Descriptor)
                    lst.Add(new CyASFormatType1DescriptorTemplate(desc.m_value));
                else if (desc.m_value is CyASFormatType2Descriptor)
                    lst.Add(new CyASFormatType2DescriptorTemplate(desc.m_value));

                else if (desc.m_value is CyHIDDescriptor)
                {
                    lst.Add(new CyHIDDescriptorTemplate(desc.m_value));
                    if (((CyHIDDescriptor)desc.m_value).wReportIndex != 0)
                    {
                        AddHidReportToTemplateList(((CyHIDDescriptor)desc.m_value).wReportIndex, lst);
                    }
                }
            }
            for (int i = 0; i < desc.m_nodes.Count; i++)
            {
                BuildDescriptorTemplateList(desc.m_nodes[i], lst);
            }
        }

        private void AddSerialToTemplateList(List<CyDescriptorTemplate> lst)
        {
            CyStringDescriptor serial = GetSerialDescriptor();
            if (serial.bUsed)
            {
                lst.Add(new CySerialStringDescriptorTemplate(serial));
            }
        }

        private void AddHidReportToTemplateList(uint wReportIndex, List<CyDescriptorTemplate> lst)
        {
            string reportKey = CyDescriptorNode.GetKeyByIndex(wReportIndex);
            CyDescriptorNode node_report = m_hidReportTree.GetNodeByKey(reportKey);
            lst.Add(new CyReportTemplate(node_report));
        }

        public static void SaveTemplate(string serializedXml, CyUSBDescriptorType kind)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                string ext = GetTemplateFileExt(kind);
                string dir = GetTemplateDir();
                string shortExt = ext.Substring(ext.IndexOf("(*.") + 3, 7);
                saveFileDialog1.Filter = String.Format("{0}XML Files (*.xml)|*.xml", ext);
                saveFileDialog1.InitialDirectory = dir;
                saveFileDialog1.DefaultExt = shortExt;
                saveFileDialog1.Title = "Save a Template File";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter strw = new StreamWriter(saveFileDialog1.FileName))
                        {
                            strw.WriteLine(serializedXml);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            String.Format("{0}\r\n{1}", Properties.Resources.MSG_UNABLE_SAVE_TEMPLATE, ex),
                            MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }    

        #endregion Save Templates

        //------------------------------------------------------------------------------------------------------------

        #region Import Templates

        public CyUSBDescriptor ImportDescriptor(CyUSBDescriptorType kind)
        {
            CyUSBDescriptor result = null;
            CyDescriptorTemplate tDesc = null;
            try
            {
                string serializedstr = LoadTemplateFromFile(kind);
                if (!String.IsNullOrEmpty(serializedstr))
                {
                    XmlSerializer s = new XmlSerializer(typeof(CyDescriptorTemplate));
                    tDesc = (CyDescriptorTemplate)s.Deserialize(new StringReader(serializedstr));
                }
                else
                {
                    return result;
                }
                result = tDesc.ConvertToUSBDesc();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\r\n{1}", Properties.Resources.MSG_UNABLE_LOAD_TEMPLATE, ex),
                    MSG_TITLE_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            CheckDescriptorForStrings(result);

            return result;
        }

        public bool ImportDescriptorList(CyUSBDescriptorType kind)
        {
            bool res = false;
            List<CyUSBDescriptor> lst = new List<CyUSBDescriptor>();
            List<CyDescriptorTemplate> tDescList = null;
            try
            {
                string serializedstr = LoadTemplateFromFile(kind);
                if (!String.IsNullOrEmpty(serializedstr))
                {
                    XmlSerializer s = new XmlSerializer(typeof(List<CyDescriptorTemplate>));
                    tDescList = (List<CyDescriptorTemplate>)s.Deserialize(new StringReader(serializedstr));
                }
                else
                {
                    return res;
                }
                for (int i = 0; i < tDescList.Count; i++)
                {
                    // Add report to the hid report tree, not include it to the lst
                    if (tDescList[i] is CyReportTemplate)
                    {
                        int wIndex = BuildReportTreeFromDesc(tDescList[i]);
                        // Set report index in the Hid descriptor
                        if (wIndex >= 0)
                        {
                            for (int j = lst.Count-1; j >= 0; j--)
                            {
                                if (lst[j].bDescriptorType == CyUSBDescriptorType.HID)
                                    ((CyHIDDescriptor) lst[j]).wReportIndex = (uint)wIndex;
                            }
                        }
                    }
                    else
                    {
                        lst.Add(tDescList[i].ConvertToUSBDesc());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\r\n{1}", Properties.Resources.MSG_UNABLE_LOAD_TEMPLATE, ex),
                    MSG_TITLE_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return res;
            }

            for (int i = 0; i < lst.Count; i++)
            {
                CheckDescriptorForStrings(lst[i]);
            }

            if (lst.Count > 0)
            {
                if (lst[0].GetLevel() == 1)
                {
                    CyDescriptorNode subTree = BuildDevTreeFromDesc(lst, 0, m_deviceTree.m_nodes[0].Key);
                    res = true;
                }
                else if (lst[0].GetLevel() == 3)
                {
                    CyDescriptorNode subTree = BuildAudioTreeFromDesc(lst, 0, m_audioTree.m_nodes[0].Key);
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Imports a HID Report from the file.
        /// </summary>
        /// <returns>ReportTemplate class</returns>
        public bool ImportReport()
        {
            bool res = false;
            CyReportTemplate rptTemplate;
            try
            {
                string serializedstr = LoadTemplateFromFile(CyUSBDescriptorType.HID_REPORT);
                if (!String.IsNullOrEmpty(serializedstr))
                {
                    XmlSerializer s = new XmlSerializer(typeof(CyReportTemplate));
                    rptTemplate = (CyReportTemplate)s.Deserialize(new StringReader(serializedstr));
                    res = BuildReportTreeFromDesc(rptTemplate) >= 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    String.Format("{0}\r\n{1}", Properties.Resources.MSG_UNABLE_LOAD_TEMPLATE, ex),
                    MSG_TITLE_WARNING, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return res;
        }

        /// <summary>
        /// Converts the array of descriptors to the device descriptor tree. Used in import operation. 
        /// Is recursive.
        /// </summary>
        /// <param name="lst">array of descriptors</param>
        /// <param name="index">index of the current descriptor in the lst</param>
        /// <param name="parentKey">key of the parent node in the device tree</param>
        /// <returns>newly created node</returns>
        private CyDescriptorNode BuildDevTreeFromDesc(List<CyUSBDescriptor> lst, int index, string parentKey)
        {
            CyDescriptorNode node = m_deviceTree.AddNode(parentKey);
            if (node.m_value.bDescriptorType == CyUSBDescriptorType.ALTERNATE)
            {
                // If node is audio descriptor, add it to the audio tree
                if (lst[index] is CyAudioInterfaceDescriptor)
                {
                    if (!m_audioTree.m_nodes[0].m_nodes.Contains(node))
                        m_audioTree.m_nodes[0].m_nodes.Add(node);
                }
                // If interface alternate number is 0, add new Alternate node, else attach it to previous
                if (((CyInterfaceDescriptor)lst[index]).bAlternateSetting == 0)
                {
                    node = node.m_nodes[0];
                }
                else
                {
                    byte ind = 0;
                    CyDescriptorNode configNode = m_deviceTree.GetNodeByKey(parentKey);
                    if (configNode.m_nodes.Count > 1)
                    {
                        configNode.m_nodes.Remove(node);
                        ind = (byte) (configNode.m_nodes.Count - 1);
                    }
                    m_deviceTree.AddNode(configNode.m_nodes[ind].Key, ind);
                    node = configNode.m_nodes[ind].m_nodes[configNode.m_nodes[ind].m_nodes.Count - 1];
                }
            }
            node.m_value = lst[index];
            node.m_nodes.Clear();

            int levelParent = lst[index].GetLevel();
            int childIndex = index + 1;
            // If found a new device descriptor, start a new branch
            if ((childIndex < lst.Count) && (lst[childIndex].GetLevel() == 1))
            {
                BuildDevTreeFromDesc(lst, childIndex, m_deviceTree.m_nodes[0].Key);
            }
            else
            {
                while ((childIndex < lst.Count) && (lst[childIndex].GetLevel() > levelParent))
                {
                    if (lst[childIndex].GetLevel() == levelParent + 1)
                    {
                        BuildDevTreeFromDesc(lst, childIndex, node.Key);
                    }
                    else if (lst[childIndex].GetLevel() == 10) // Serial String 
                    {
                        CyDescriptorNode serialNode = m_stringTree.GetNodeByKey(NODEKEY_STRING_SERIAL);
                        if (serialNode != null)
                            serialNode.m_value = lst[childIndex];
                    }
                    childIndex++;
                }
            }
            return node;
        }

        private CyDescriptorNode BuildAudioTreeFromDesc(List<CyUSBDescriptor> lst, int index, string parentKey)
        {
            CyDescriptorNode node = new CyDescriptorNode(lst[index]);
            CyDescriptorNode parentNode = m_audioTree.GetNodeByKey(parentKey);

            if (parentNode == null) return null;

            if (parentKey == NODEKEY_AUDIO) 
            {
                // Create general interface descriptor first or reassign to existing general descriptor
                if ((node.m_value.bDescriptorType == CyUSBDescriptorType.INTERFACE) &&
                    (((CyInterfaceDescriptor)lst[index]).bAlternateSetting > 0) &&
                    (parentNode.m_nodes.Count > 0))
                {
                    parentNode = parentNode.m_nodes[parentNode.m_nodes.Count - 1];
                }
                else
                {
                    // generate name for interface general descriptor
                    int newIndex = parentNode.m_nodes.Count + 1;
                    CyDescriptorNode generalInterfaceNode =
                        new CyDescriptorNode(new CyInterfaceGeneralDescriptor("Audio Interface " + newIndex));
                    generalInterfaceNode.Key = generalInterfaceNode.Key.Replace(NODEKEY_USBDESCRIPTOR, 
                                                                                NODEKEY_INTERFACE);
                    parentNode.m_nodes.Add(generalInterfaceNode);
                    parentNode = parentNode.m_nodes[parentNode.m_nodes.Count - 1];
                }
            }
            parentNode.m_nodes.Add(node);


            int levelParent = lst[index].GetLevel();
            int childIndex = index + 1;
            // If found a new interface descriptor, start a new branch
            if ((childIndex < lst.Count) && (lst[childIndex].GetLevel() == 3))
            {
                BuildAudioTreeFromDesc(lst, childIndex, m_audioTree.m_nodes[0].Key);
            }
            else
            {
                while ((childIndex < lst.Count) && (lst[childIndex].GetLevel() > levelParent))
                {
                    if (lst[childIndex].GetLevel() == levelParent + 1)
                    {
                        BuildAudioTreeFromDesc(lst, childIndex, node.Key);
                    }
                    childIndex++;
                }
            }

            return node;
        }

        private int BuildReportTreeFromDesc(CyDescriptorTemplate rptTemplate)
        {
            int wIndex = -1;
            CyDescriptorNode rptNode, newNode;
            CyReportTemplate rpt = (CyReportTemplate) rptTemplate;
            if (rptTemplate != null)
            {
                rptNode = m_hidReportTree.AddNode(m_hidReportTree.m_nodes[0].Key);
                //Report name
                ((CyHIDReportDescriptor)rptNode.m_value).Name = rpt.m_reportName;
                //Items
                for (int i = 0; i < rpt.m_items.Count; i++)
                {
                    CyHidReportItem item = rpt.m_items[i].ConvertToHidReportItem();
                    if (item != null)
                    {
                        newNode = m_hidReportTree.AddNode(rptNode.Key);
                        ((CyHIDReportItemDescriptor)newNode.m_value).Item = item;
                    }
                }
                // Get Index
                string strConfigKey = m_hidReportTree.GetKeyByNode(rptNode.m_value);
                if (strConfigKey != "")
                {
                    wIndex = (int)CyDescriptorNode.GetDescriptorIndex(strConfigKey);
                }
                ParamHIDReportTreeChanged = true;
            }
            return wIndex;
        }

        private void CheckDescriptorForStrings(CyUSBDescriptor desc)
        {
            if (desc == null) return;
            string newStr;
            if (desc is CyDeviceDescriptor)
            {
                newStr = ((CyDeviceDescriptor)desc).sManufacturer;
                ((CyDeviceDescriptor)desc).iwManufacturer = AddNewStrDescriptor(newStr);

                newStr = ((CyDeviceDescriptor)desc).sProduct;
                ((CyDeviceDescriptor)desc).iwProduct = AddNewStrDescriptor(newStr);
            }
            if (desc is CyConfigDescriptor)
            {
                newStr = ((CyConfigDescriptor)desc).sConfiguration;
                ((CyConfigDescriptor)desc).iwConfiguration = AddNewStrDescriptor(newStr);
            }
            if (desc is CyInterfaceDescriptor)
            {
                newStr = ((CyInterfaceDescriptor)desc).sInterface;
                ((CyInterfaceDescriptor)desc).iwInterface = AddNewStrDescriptor(newStr);
            }
            if (desc.bDescriptorType == CyUSBDescriptorType.AUDIO)
            {
                try
                {
                    Type descType = desc.GetType();
                    FieldInfo[] fi = descType.GetFields();
                    for (int j = 0; j < fi.Length; j++)
                    {
                        if (fi[j].Name.StartsWith("iw"))
                        {
                            newStr = "";
                            string propName = fi[j].Name.Remove(0, 2).Insert(0, "s");
                            PropertyInfo pi = descType.GetProperty(propName);
                            if ((pi != null) && (pi.GetValue(desc, null) != null))
                                newStr = pi.GetValue(desc, null).ToString();
                            fi[j].SetValue(desc, AddNewStrDescriptor(newStr));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.ToString());
                }
            }
        }

        private uint AddNewStrDescriptor(string str)
        {
            uint strIndex = 0;
            if (!String.IsNullOrEmpty(str))
            {
                // If string exists, return its index
                for (int i = 1; i < m_stringTree.m_nodes[0].m_nodes.Count; i++)
                {
                    CyStringDescriptor strDesc = (CyStringDescriptor) m_stringTree.m_nodes[0].m_nodes[i].m_value;
                    if (strDesc.bString == str)
                        strIndex = GetStringDescriptorIndex(strDesc, this);
                }
                // If not exists, create new
                if (strIndex == 0)
                {
                    // Add string to the string descriptors list
                    CyStringDescriptor newDesc = CreateNewStringDescriptor(str, this);
                    // Get the index of the new String Descriptor
                    strIndex = GetStringDescriptorIndex(newDesc, this);
                }
            }
            return strIndex;
        }

        /// <summary>
        /// Imports a HID Report from the file.
        /// </summary>
        /// <returns>ReportTemplate class</returns>
        public static string LoadTemplateFromFile(CyUSBDescriptorType kind)
        {
            StringBuilder serializedstr = new StringBuilder();
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                string ext = GetTemplateFileExt(kind);
                string dir = GetTemplateDir();
                openFileDialog1.Filter = String.Format("{0}XML Files (*.xml)|*.xml", ext);
                openFileDialog1.InitialDirectory = dir;
                openFileDialog1.Title = "Open a Template File";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                serializedstr.Append(line);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            String.Format("{0}\r\n{1}", Properties.Resources.MSG_UNABLE_LOAD_TEMPLATE, ex),
                            MSG_TITLE_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return serializedstr.ToString();
        }

        #endregion Import Templates
    }

    //============================================================================================================

    #region Descriptor Template

    /// <summary>
    /// These classes are used for xml serialization of the templates
    /// </summary>

    [Serializable()]
    [XmlInclude(typeof(CyDeviceDescriptorTemplate))]
    [XmlInclude(typeof(CyConfigurationDescriptorTemplate))]
    [XmlInclude(typeof(CyInterfaceDescriptorTemplate))]
    [XmlInclude(typeof(CyEndpointDescriptorTemplate))]
    [XmlInclude(typeof(CyHIDDescriptorTemplate))]
    [XmlInclude(typeof(CySerialStringDescriptorTemplate))]
    [XmlInclude(typeof(CyReportTemplate))]
    [XmlInclude(typeof(CyACHeaderDescriptorTemplate))]
    [XmlInclude(typeof(CyACInputTerminalDescriptorTemplate))]
    [XmlInclude(typeof(CyACOutputTerminalDescriptorTemplate))]
    [XmlInclude(typeof(CyACMixerUnitDescriptorTemplate))]
    [XmlInclude(typeof(CyACSelectorUnitDescriptorTemplate))]
    [XmlInclude(typeof(CyACFeatureUnitDescriptorTemplate))]
    [XmlInclude(typeof(CyACProcessingUnitDescriptorTemplate))]
    [XmlInclude(typeof(CyACExtensionDescriptorTemplate))]
    [XmlInclude(typeof(CyASGeneralDescriptorTemplate))]
    [XmlInclude(typeof(CyASFormatType1DescriptorTemplate))]
    [XmlInclude(typeof(CyASFormatType2DescriptorTemplate))]
    [XmlInclude(typeof(CyAudioEndpointDescriptorTemplate))]
    [XmlInclude(typeof(CyASEndpointDescriptorTemplate))]

    [XmlType("Descriptor")]
    public abstract class CyDescriptorTemplate
    {
        public abstract CyUSBDescriptor ConvertToUSBDesc();

        protected static string ByteToStr(byte n)
        {
            return "0x" + n.ToString("X2");
        }
        protected static string UshortToStr(ushort n)
        {
            return "0x" + n.ToString("X4");
        }
        protected static string Byte3ToStr(uint n)
        {
            return "0x" + n.ToString("X6");
        }
        protected static string ByteArrayToStr(byte[] n)
        {
            StringBuilder res = new StringBuilder();
            if (n.Length > 0)
            {
                for (int i = 0; i < n.Length - 1; i++)
                {
                    res.Append("0x" + n[i].ToString("X2") + ", ");
                }
                res.Append("0x" + n[n.Length - 1].ToString("X2"));
            }
            return res.ToString();
        }
        protected static string Byte3ArrayToStr(uint[] n)
        {
            StringBuilder res = new StringBuilder();
            if (n.Length > 0)
            {
                for (int i = 0; i < n.Length - 1; i++)
                {
                    res.Append("0x" + n[i].ToString("X6") + ", ");
                }
                res.Append("0x" + n[n.Length - 1].ToString("X6"));
            }
            return res.ToString();
        }


        protected static byte StrToHex1B(string s)
        {
            byte res = 0;
            try
            {
                res = Convert.ToByte(s.Replace("0x", "").Trim(), 16);
            }
            catch
            {
                Debug.Assert(false);
            }
            return res;
        }
        protected static ushort StrToHex2B(string s)
        {
            ushort res = 0;
            try
            {
                res = Convert.ToUInt16(s.Replace("0x", "").Trim(), 16);
            }
            catch
            {
                Debug.Assert(false);
            }
            return res;
        }
        protected static uint StrToHex3B(string s)
        {
            uint res = 0;
            try
            {
                res = Convert.ToUInt32(s.Replace("0x", "").Trim(), 16);
            }
            catch
            {
                Debug.Assert(false);
            }
            return res;
        }
        protected static byte[] StrToHex1BArray(string s)
        {
            List<byte> reslst = new List<byte>();
            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    string[] sres = s.Split(',');
                    for (int i = 0; i < sres.Length; i++)
                    {
                        reslst.Add(StrToHex1B(sres[i]));
                    }
                }
                catch
                {
                    Debug.Assert(false);
                }
            }
            return reslst.ToArray();
        }
        protected static uint[] StrToHex3BArray(string s)
        {
            List<uint> reslst = new List<uint>();
            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    string[] sres = s.Split(',');
                    for (int i = 0; i < sres.Length; i++)
                    {
                        reslst.Add(StrToHex3B(sres[i]));
                    }
                }
                catch
                {
                    Debug.Assert(false);
                }
            }
            return reslst.ToArray();
        }
    }
    [XmlType("Device_Descriptor")]
    public class CyDeviceDescriptorTemplate : CyDescriptorTemplate
    {
        public string bDeviceClass;
        public string bDeviceSubClass;
        public string bDeviceProtocol;
        public string bMaxPacketSize0;
        public string idVendor;
        public string idProduct;
        public string iManufacturer;
        public string iProduct;
        public string iSerialNumber;
        public string bNumConfigurations;
        public string bcdDevice;
        public string bMemoryMgmt;

        public CyDeviceDescriptorTemplate()
        {
        }

        public CyDeviceDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bDeviceClass = ByteToStr(((CyDeviceDescriptor)usbDesc).bDeviceClass);
            bDeviceSubClass = ByteToStr(((CyDeviceDescriptor)usbDesc).bDeviceSubClass);
            bDeviceProtocol = ByteToStr(((CyDeviceDescriptor)usbDesc).bDeviceProtocol);
            bMaxPacketSize0 = ByteToStr(((CyDeviceDescriptor)usbDesc).bMaxPacketSize0);
            idVendor = UshortToStr(((CyDeviceDescriptor)usbDesc).idVendor);
            idProduct = UshortToStr(((CyDeviceDescriptor)usbDesc).idProduct);
            iManufacturer = ((CyDeviceDescriptor)usbDesc).sManufacturer;
            iProduct = ((CyDeviceDescriptor)usbDesc).sProduct;
            iSerialNumber = ((CyDeviceDescriptor)usbDesc).sSerialNumber;
            bNumConfigurations = ByteToStr(((CyDeviceDescriptor)usbDesc).bNumConfigurations);
            bcdDevice = UshortToStr(((CyDeviceDescriptor)usbDesc).bcdDevice);
            bMemoryMgmt = ByteToStr(((CyDeviceDescriptor)usbDesc).bMemoryMgmt);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyDeviceDescriptor usbDesc = new CyDeviceDescriptor();

            usbDesc.bDeviceClass = StrToHex1B(bDeviceClass);
            usbDesc.bDeviceSubClass = StrToHex1B(bDeviceSubClass);
            usbDesc.bDeviceProtocol = StrToHex1B(bDeviceProtocol);
            usbDesc.bMaxPacketSize0 = StrToHex1B(bMaxPacketSize0);
            usbDesc.idVendor = StrToHex2B(idVendor);
            usbDesc.idProduct = StrToHex2B(idProduct);
            usbDesc.sManufacturer = iManufacturer;
            usbDesc.sProduct = iProduct;
            usbDesc.sSerialNumber = iSerialNumber;
            usbDesc.bNumConfigurations = StrToHex1B(bNumConfigurations);
            usbDesc.bcdDevice = StrToHex2B(bcdDevice);
            usbDesc.bMemoryMgmt = StrToHex1B(bMemoryMgmt);

            return usbDesc;
        }
    }
    [XmlType("Configuration_Descriptor")]
    public class CyConfigurationDescriptorTemplate : CyDescriptorTemplate
    {
        public string wTotalLength;
        public string bNumInterfaces;
        public string bConfigurationValue;
        public string iConfiguration;
        public string bmAttributes;
        public string bMaxPower;

        public CyConfigurationDescriptorTemplate()
        {
        }

        public CyConfigurationDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            wTotalLength = UshortToStr(((CyConfigDescriptor)usbDesc).wTotalLength);
            bNumInterfaces = ByteToStr(((CyConfigDescriptor)usbDesc).bNumInterfaces);
            bConfigurationValue = ByteToStr(((CyConfigDescriptor)usbDesc).bConfigurationValue);
            iConfiguration = ((CyConfigDescriptor)usbDesc).sConfiguration;
            bmAttributes = ByteToStr(((CyConfigDescriptor)usbDesc).bmAttributes);
            bMaxPower = ByteToStr(((CyConfigDescriptor)usbDesc).bMaxPower);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyConfigDescriptor usbDesc = new CyConfigDescriptor();

            usbDesc.wTotalLength = StrToHex2B(wTotalLength);
            usbDesc.bNumInterfaces = StrToHex1B(bNumInterfaces);
            usbDesc.bConfigurationValue = StrToHex1B(bConfigurationValue);
            usbDesc.sConfiguration = iConfiguration;
            usbDesc.bmAttributes = StrToHex1B(bmAttributes);
            usbDesc.bMaxPower = StrToHex1B(bMaxPower);
            return usbDesc;
        }
    }
    [XmlType("Interface_Descriptor")]
    public class CyInterfaceDescriptorTemplate : CyDescriptorTemplate
    {
        public string bInterfaceClass;
        public string bInterfaceSubClass;
        public string bAlternateSetting;
        public string bInterfaceNumber;
        public string bNumEndpoints;
        public string bInterfaceProtocol;
        public string iInterface;

        public CyInterfaceDescriptorTemplate()
        {
        }

        public CyInterfaceDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bInterfaceClass = ByteToStr(((CyInterfaceDescriptor)usbDesc).bInterfaceClass);
            bInterfaceSubClass = ByteToStr(((CyInterfaceDescriptor)usbDesc).bInterfaceSubClass);
            bAlternateSetting = ByteToStr(((CyInterfaceDescriptor)usbDesc).bAlternateSetting);
            bInterfaceNumber = ByteToStr(((CyInterfaceDescriptor)usbDesc).bInterfaceNumber);
            bNumEndpoints = ByteToStr(((CyInterfaceDescriptor)usbDesc).bNumEndpoints);
            bInterfaceProtocol = ByteToStr(((CyInterfaceDescriptor)usbDesc).bInterfaceProtocol);
            iInterface = ((CyInterfaceDescriptor)usbDesc).sInterface;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyInterfaceDescriptor usbDesc;
            if (StrToHex1B(bInterfaceClass) == CyUSBFSParameters.CLASS_AUDIO)
                usbDesc  = new CyAudioInterfaceDescriptor();
            else
                usbDesc = new CyInterfaceDescriptor();

            usbDesc.bInterfaceClass = StrToHex1B(bInterfaceClass);
            usbDesc.bInterfaceSubClass = StrToHex1B(bInterfaceSubClass);
            usbDesc.bAlternateSetting = StrToHex1B(bAlternateSetting);
            usbDesc.bInterfaceNumber = StrToHex1B(bInterfaceNumber);
            usbDesc.bNumEndpoints = StrToHex1B(bNumEndpoints);
            usbDesc.bInterfaceProtocol = StrToHex1B(bInterfaceProtocol);
            usbDesc.sInterface = iInterface;

            return usbDesc;
        }
    }

    [XmlType("Endpoint_Descriptor")]
    public class CyEndpointDescriptorTemplate : CyDescriptorTemplate
    {
        public string bInterval;
        public string bEndpointAddress;
        public string bmAttributes;
        public string wMaxPacketSize;

        public CyEndpointDescriptorTemplate()
        {
        }

        public CyEndpointDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bInterval = ByteToStr(((CyEndpointDescriptor)usbDesc).bInterval);
            bEndpointAddress = ByteToStr(((CyEndpointDescriptor)usbDesc).bEndpointAddress);
            bmAttributes = ByteToStr(((CyEndpointDescriptor)usbDesc).bmAttributes);
            wMaxPacketSize = UshortToStr(((CyEndpointDescriptor)usbDesc).wMaxPacketSize);

        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyEndpointDescriptor usbDesc = new CyEndpointDescriptor();

            usbDesc.bInterval = StrToHex1B(bInterval);
            usbDesc.bEndpointAddress = StrToHex1B(bEndpointAddress);
            usbDesc.bmAttributes = StrToHex1B(bmAttributes);
            usbDesc.wMaxPacketSize = StrToHex2B(wMaxPacketSize);

            return usbDesc;
        }
    }
    [XmlType("HID_Descriptor")]
    public class CyHIDDescriptorTemplate : CyDescriptorTemplate
    {
        public string bcdHID;
        public string bCountryCode;
        public string bNumDescriptors;
        public string bDescriptorType;
        public string wDescriptorLength;

        public CyHIDDescriptorTemplate()
        {
        }

        public CyHIDDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bcdHID = UshortToStr(((CyHIDDescriptor)usbDesc).bcdHID);
            bCountryCode = ByteToStr(((CyHIDDescriptor)usbDesc).bCountryCode);
            bNumDescriptors = ByteToStr(((CyHIDDescriptor)usbDesc).bNumDescriptors);
            bDescriptorType = ByteToStr(((CyHIDDescriptor)usbDesc).bDescriptorType1);
            wDescriptorLength = UshortToStr(((CyHIDDescriptor)usbDesc).wDescriptorLength);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyHIDDescriptor usbDesc = new CyHIDDescriptor();

            usbDesc.bcdHID = StrToHex2B(bcdHID);
            usbDesc.bCountryCode = StrToHex1B(bCountryCode);
            usbDesc.bNumDescriptors = StrToHex1B(bNumDescriptors);
            usbDesc.bDescriptorType1 = StrToHex1B(bDescriptorType);
            usbDesc.wDescriptorLength = StrToHex2B(wDescriptorLength);

            return usbDesc;
        }
    }

    [XmlType("Serial_Descriptor")]
    public class CySerialStringDescriptorTemplate : CyDescriptorTemplate
    {
        public string bString;
        public bool bUsed;
        public CyStringGenerationType snType;

        public CySerialStringDescriptorTemplate()
        {
        }

        public CySerialStringDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bString = ((CyStringDescriptor)usbDesc).bString;
            bUsed = ((CyStringDescriptor)usbDesc).bUsed;
            snType = ((CyStringDescriptor)usbDesc).snType;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyStringDescriptor usbDesc = new CyStringDescriptor();

            usbDesc.bString = bString;
            usbDesc.bUsed = bUsed;
            usbDesc.snType = snType;

            return usbDesc;
        }
    }

    #endregion Descriptor Template

    //============================================================================================================

    #region Report Template

    /// <summary>
    /// Used for saving and importing HID reports.
    /// </summary>
    [XmlRoot("HID_Report")]
    [XmlType("ReportTemplate")]
    public class CyReportTemplate : CyDescriptorTemplate
    {
        [XmlAttribute("Report_Name")]
        public string m_reportName;
        [XmlArray("Items")]
        [XmlArrayItem("HID_Item")]
        public List<CyReportTemplateItem> m_items;

        public CyReportTemplate()
        {
            m_items = new List<CyReportTemplateItem>();
        }

        public CyReportTemplate(CyDescriptorNode node_report)
        {
            m_reportName = ((CyHIDReportDescriptor)node_report.m_value).Name;
            m_items = new List<CyReportTemplateItem>();
            for (int j = 0; j < node_report.m_nodes.Count; j++)
            {
                CyHidReportItem item =
                    ((CyHIDReportItemDescriptor)node_report.m_nodes[j].m_value).Item;
                List<byte> byteList = new List<byte>(item.m_value);
                if (byteList.Count > 0)
                    byteList.RemoveAt(0);
                long val = 0;
                if ((item.Kind == CyHidReportItemKind.INT) || (item.Kind == CyHidReportItemKind.CUSTOM))
                {
                    val = CyUSBFSParameters.ConvertByteArrayToInt(byteList);
                }
                else
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        for (int k = 1; k < item.m_value.Count; k++)
                        {
                            val += item.m_value[k] << (k - 1) * 8;
                        }
                    }
                    else
                    {
                        for (int k = 1; k < item.m_value.Count; k++)
                        {
                            val += item.m_value[k] << (item.m_value.Count - k - 1) * 8;
                        }
                    }
                }
                m_items.Add(new CyReportTemplateItem(item.Name, item.m_prefix, item.m_size, val, item.m_description));
            }
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyHIDReportDescriptor usbDesc = new CyHIDReportDescriptor();
            //Report name
            usbDesc.Name = m_reportName;
            //Items
            for (int i = 0; i < m_items.Count; i++)
            {
                CyHidReportItem item = m_items[i].ConvertToHidReportItem();
            }
            return usbDesc;
        }
    }

    /// <summary>
    /// Used for saving and importing HID report items.
    /// </summary>
    [XmlType("ReportTemplateItem")]
    public class CyReportTemplateItem 
    {
        [XmlAttribute("Type")]
        public string m_type;
        [XmlAttribute("Code")]
        public int m_code;
        [XmlAttribute("Size")]
        public int m_size;
        [XmlAttribute("Value")]
        public long m_value;
        [XmlAttribute("Desc")]
        public string m_description;

        public CyReportTemplateItem()
        {
        }

        public CyReportTemplateItem(string type, int code, int size, long value, string description)
        {
            m_type = type;
            m_code = code;
            m_size = size;
            m_value = value;
            m_description = description;
        }

        public CyHidReportItem ConvertToHidReportItem()
        {
            CyHidReportItem item = new CyHidReportItem(m_type, (byte)m_code, m_size, m_value, m_description);
            return item;
        }
    }

    #endregion Report Template

    //============================================================================================================

    #region Audio Template

    [XmlType("ACHeader_Descriptor")]
    public class CyACHeaderDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bcdADC;
        public string wTotalLength;
        public string bInCollection;
        public string baInterfaceNr;

        public CyACHeaderDescriptorTemplate()
        {
        }

        public CyACHeaderDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACHeaderDescriptor desc = (CyACHeaderDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bcdADC = UshortToStr(desc.bcdADC);
            wTotalLength = UshortToStr(desc.wTotalLength);
            bInCollection = ByteToStr(desc.bInCollection);
            baInterfaceNr = ByteArrayToStr(desc.baInterfaceNr);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACHeaderDescriptor usbDesc = new CyACHeaderDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bcdADC = StrToHex2B(bcdADC);
            usbDesc.wTotalLength = StrToHex2B(wTotalLength);
            usbDesc.bInCollection = StrToHex1B(bInCollection);
            usbDesc.baInterfaceNr = StrToHex1BArray(baInterfaceNr);
            return usbDesc;
        }
    }

    [XmlType("ACInputTerminal_Descriptor")]
    public class CyACInputTerminalDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bTerminalID;
        public string wTerminalType;
        public string bAssocTerminal;
        public string bNrChannels;
        public string wChannelConfig;
        public string iChannelNames;
        public string iTerminal;

        public CyACInputTerminalDescriptorTemplate()
        {
        }

        public CyACInputTerminalDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACInputTerminalDescriptor desc = (CyACInputTerminalDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bTerminalID = ByteToStr(desc.bTerminalID);
            wTerminalType = UshortToStr(desc.wTerminalType);
            bAssocTerminal = ByteToStr(desc.bAssocTerminal);
            bNrChannels = ByteToStr(desc.bNrChannels);
            wChannelConfig = UshortToStr(desc.wChannelConfig);
            iChannelNames = desc.sChannelNames;
            iTerminal = desc.sTerminal;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACInputTerminalDescriptor usbDesc = new CyACInputTerminalDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bTerminalID = StrToHex1B(bTerminalID);
            usbDesc.wTerminalType = StrToHex2B(wTerminalType);
            usbDesc.bAssocTerminal = StrToHex1B(bAssocTerminal);
            usbDesc.bNrChannels = StrToHex1B(bNrChannels);
            usbDesc.wChannelConfig = StrToHex2B(wChannelConfig);
            usbDesc.sChannelNames = iChannelNames;
            usbDesc.sTerminal = iTerminal;
            return usbDesc;
        }
    }

    [XmlType("ACOutputTerminalDescriptor_Descriptor")]
    public class CyACOutputTerminalDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bTerminalID;
        public string wTerminalType;
        public string bAssocTerminal;
        public string bSourceID;
        public string iTerminal;

        public CyACOutputTerminalDescriptorTemplate()
        {
        }

        public CyACOutputTerminalDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACOutputTerminalDescriptor desc = (CyACOutputTerminalDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bTerminalID = ByteToStr(desc.bTerminalID);
            wTerminalType = UshortToStr(desc.wTerminalType);
            bAssocTerminal = ByteToStr(desc.bAssocTerminal);
            bSourceID = ByteToStr(desc.bSourceID);
            iTerminal = desc.sTerminal;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACOutputTerminalDescriptor usbDesc = new CyACOutputTerminalDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bTerminalID = StrToHex1B(bTerminalID);
            usbDesc.wTerminalType = StrToHex2B(wTerminalType);
            usbDesc.bAssocTerminal = StrToHex1B(bAssocTerminal);
            usbDesc.bSourceID = StrToHex1B(bSourceID);
            usbDesc.sTerminal = iTerminal;           
            return usbDesc;
        }
    }

    [XmlType("ACMixerUnitDescriptor_Descriptor")]
    public class CyACMixerUnitDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bUnitID;
        public string bNrInPins;
        public string baSourceID;
        public string bNrChannels;
        public string wChannelConfig;
        public string iChannelNames;
        public string bmControls;
        public string iMixer;

        public CyACMixerUnitDescriptorTemplate()
        {
        }

        public CyACMixerUnitDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACMixerUnitDescriptor desc = (CyACMixerUnitDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bUnitID = ByteToStr(desc.bUnitID);
            bNrInPins = ByteToStr(desc.bNrInPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            bNrChannels = ByteToStr(desc.bNrChannels);
            wChannelConfig = UshortToStr(desc.wChannelConfig);
            iChannelNames = desc.sChannelNames;
            bmControls = ByteArrayToStr(desc.bmControls.ToArray());
            iMixer = desc.sMixer;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACMixerUnitDescriptor usbDesc = new CyACMixerUnitDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bUnitID = StrToHex1B(bUnitID);
            usbDesc.bNrInPins = StrToHex1B(bNrInPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.bNrChannels = StrToHex1B(bNrChannels);
            usbDesc.wChannelConfig = StrToHex2B(wChannelConfig);
            usbDesc.sChannelNames = iChannelNames;
            usbDesc.bmControls = new List<byte>(StrToHex1BArray(bmControls));
            usbDesc.sMixer = iMixer;
            return usbDesc;
        }
    }

    [XmlType("ACSelectorUnit_Descriptor")]
    public class CyACSelectorUnitDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bUnitID;
        public string bNrInPins;
        public string baSourceID;
        public string iSelector;

        public CyACSelectorUnitDescriptorTemplate()
        {
        }

        public CyACSelectorUnitDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACSelectorUnitDescriptor desc = (CyACSelectorUnitDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bUnitID = ByteToStr(desc.bUnitID);
            bNrInPins = ByteToStr(desc.bNrInPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            iSelector = desc.sSelector;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACSelectorUnitDescriptor usbDesc = new CyACSelectorUnitDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bUnitID = StrToHex1B(bUnitID);
            usbDesc.bNrInPins = StrToHex1B(bNrInPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.sSelector = iSelector;
            return usbDesc;
        }
    }

    [XmlType("ACFeatureUnit_Descriptor")]
    public class CyACFeatureUnitDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bUnitID;
        public string bSourceID;
        public string bControlSize;
        public string bmaControls;
        public string iFeature;

        public CyACFeatureUnitDescriptorTemplate()
        {
        }

        public CyACFeatureUnitDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACFeatureUnitDescriptor desc = (CyACFeatureUnitDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bUnitID = ByteToStr(desc.bUnitID);
            bSourceID = ByteToStr(desc.bSourceID);
            bControlSize = ByteToStr(desc.bControlSize);
            bmaControls = ByteArrayToStr(desc.bmaControls.ToArray());
            iFeature = desc.sFeature;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACFeatureUnitDescriptor usbDesc = new CyACFeatureUnitDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bUnitID = StrToHex1B(bUnitID);
            usbDesc.bSourceID = StrToHex1B(bSourceID);
            usbDesc.bControlSize = StrToHex1B(bControlSize);
            usbDesc.bmaControls = new List<byte>(StrToHex1BArray(bmaControls));
            usbDesc.sFeature = iFeature;
            return usbDesc;
        }
    }

    [XmlType("ACProcessingUnit_Descriptor")]
    public class CyACProcessingUnitDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bUnitID;
        public string wProcessType;
        public string bNrInPins;
        public string baSourceID;
        public string bNrChannels;
        public string wChannelConfig;
        public string iChannelNames;
        public string bControlSize;
        public string bmControls;
        public string iProcessing;

        public CyACProcessingUnitDescriptorTemplate()
        {
        }

        public CyACProcessingUnitDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACProcessingUnitDescriptor desc = (CyACProcessingUnitDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bUnitID = ByteToStr(desc.bUnitID);
            wProcessType = UshortToStr(desc.wProcessType);
            bNrInPins = ByteToStr(desc.bNrInPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            bNrChannels = ByteToStr(desc.bNrChannels);
            wChannelConfig = UshortToStr(desc.wChannelConfig);
            iChannelNames = desc.sChannelNames;
            bControlSize = ByteToStr(desc.bControlSize);
            bmControls = ByteArrayToStr(desc.bmControls);
            iProcessing = desc.sProcessing;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACProcessingUnitDescriptor usbDesc = new CyACProcessingUnitDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bUnitID = StrToHex1B(bUnitID);
            usbDesc.wProcessType = StrToHex2B(wProcessType);
            usbDesc.bNrInPins = StrToHex1B(bNrInPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.bNrChannels = StrToHex1B(bNrChannels);
            usbDesc.wChannelConfig = StrToHex2B(wChannelConfig);
            usbDesc.sChannelNames = iChannelNames;
            usbDesc.bControlSize = StrToHex1B(bControlSize);
            usbDesc.bmControls = StrToHex1BArray(bmControls);
            usbDesc.sProcessing = iProcessing;
            return usbDesc;
        }
    }

    [XmlType("ACExtension_Descriptor")]
    public class CyACExtensionDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyACInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bUnitID;
        public string wExtensionCode;
        public string bNrInPins;
        public string baSourceID;
        public string bNrChannels;
        public string wChannelConfig;
        public string iChannelNames;
        public string bControlSize;
        public string bmControls;
        public string iExtension;


        public CyACExtensionDescriptorTemplate()
        {
        }

        public CyACExtensionDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyACExtensionDescriptor desc = (CyACExtensionDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bUnitID = ByteToStr(desc.bUnitID);
            wExtensionCode = UshortToStr(desc.wExtensionCode);
            bNrInPins = ByteToStr(desc.bNrInPins);
            baSourceID = ByteArrayToStr(desc.baSourceID);
            bNrChannels = ByteToStr(desc.bNrChannels);
            wChannelConfig = UshortToStr(desc.wChannelConfig);
            iChannelNames = desc.sChannelNames;
            bControlSize = ByteToStr(desc.bControlSize);
            bmControls = ByteArrayToStr(desc.bmControls);
            iExtension = desc.sExtension;
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyACExtensionDescriptor usbDesc = new CyACExtensionDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bUnitID = StrToHex1B(bUnitID);
            usbDesc.wExtensionCode = StrToHex2B(wExtensionCode);
            usbDesc.bNrInPins = StrToHex1B(bNrInPins);
            usbDesc.baSourceID = StrToHex1BArray(baSourceID);
            usbDesc.bNrChannels = StrToHex1B(bNrChannels);
            usbDesc.wChannelConfig = StrToHex2B(wChannelConfig);
            usbDesc.sChannelNames = iChannelNames;
            usbDesc.bControlSize = StrToHex1B(bControlSize);
            usbDesc.bmControls = StrToHex1BArray(bmControls);
            usbDesc.sExtension = iExtension;

            return usbDesc;
        }
    }

    [XmlType("ASGeneral_Descriptor")]
    public class CyASGeneralDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype;
        public string bTerminalLink;
        public string bDelay;
        public string wFormatTag;

        public CyASGeneralDescriptorTemplate()
        {
        }

        public CyASGeneralDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyASGeneralDescriptor desc = (CyASGeneralDescriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bTerminalLink = ByteToStr(desc.bTerminalLink);
            bDelay = ByteToStr(desc.bDelay);
            wFormatTag = UshortToStr(desc.wFormatTag);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyASGeneralDescriptor usbDesc = new CyASGeneralDescriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bTerminalLink = StrToHex1B(bTerminalLink);
            usbDesc.bDelay = StrToHex1B(bDelay);
            usbDesc.wFormatTag = StrToHex2B(wFormatTag);
            return usbDesc;
        }
    }

    [XmlType("CyASFormatType1_Descriptor")]
    public class CyASFormatType1DescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype;
        public CyUSBOtherTypes.CyFormatTypeCodes bFormatType;
        public string bNrChannels;
        public string bSubframeSize;
        public string bBitResolution;
        public string bSamFreqType;
        public string tLowerSamFreq;
        public string tUpperSamFreq;
        public string tSamFreq;

        public CyASFormatType1DescriptorTemplate()
        {
        }

        public CyASFormatType1DescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyASFormatType1Descriptor desc = (CyASFormatType1Descriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            bNrChannels = ByteToStr(desc.bNrChannels);
            bSubframeSize = ByteToStr(desc.bSubframeSize);
            bBitResolution = ByteToStr(desc.bBitResolution);
            bSamFreqType = ByteToStr(desc.bSamFreqType);
            tLowerSamFreq = Byte3ToStr(desc.tLowerSamFreq); 
            tUpperSamFreq = Byte3ToStr(desc.tUpperSamFreq); 
            tSamFreq = Byte3ArrayToStr(desc.tSamFreq);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyASFormatType1Descriptor usbDesc = new CyASFormatType1Descriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bNrChannels = StrToHex1B(bNrChannels);
            usbDesc.bSubframeSize = StrToHex1B(bSubframeSize);
            usbDesc.bBitResolution = StrToHex1B(bBitResolution);
            usbDesc.bSamFreqType = StrToHex1B(bSamFreqType);
            usbDesc.tLowerSamFreq = StrToHex3B(tLowerSamFreq);
            usbDesc.tUpperSamFreq = StrToHex3B(tUpperSamFreq);
            usbDesc.tSamFreq = StrToHex3BArray(tSamFreq);
            return usbDesc;
        }
    }

    [XmlType("CyASFormatType2_Descriptor")]
    public class CyASFormatType2DescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyASInterfaceDescriptorSubtypes bDescriptorSubtype;
        public CyUSBOtherTypes.CyFormatTypeCodes bFormatType;
        public string wMaxBitRate;
        public string wSamplesPerFrame;
        public string bSamFreqType;
        public string tLowerSamFreq;
        public string tUpperSamFreq;
        public string tSamFreq;

        public CyASFormatType2DescriptorTemplate()
        {
        }

        public CyASFormatType2DescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            CyASFormatType2Descriptor desc = (CyASFormatType2Descriptor)usbDesc;
            bDescriptorSubtype = desc.bDescriptorSubtype;
            wMaxBitRate = UshortToStr(desc.wMaxBitRate);
            wSamplesPerFrame = UshortToStr(desc.wSamplesPerFrame);
            bSamFreqType = ByteToStr(desc.bSamFreqType);
            tLowerSamFreq = Byte3ToStr(desc.tLowerSamFreq);
            tUpperSamFreq = Byte3ToStr(desc.tUpperSamFreq); 
            tSamFreq = Byte3ArrayToStr(desc.tSamFreq);

        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyASFormatType2Descriptor usbDesc = new CyASFormatType2Descriptor();
            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.wMaxBitRate = StrToHex2B(wMaxBitRate);
            usbDesc.wSamplesPerFrame = StrToHex2B(wSamplesPerFrame);
            usbDesc.bSamFreqType = StrToHex1B(bSamFreqType);
            usbDesc.tLowerSamFreq = StrToHex3B(tLowerSamFreq);
            usbDesc.tUpperSamFreq = StrToHex3B(tUpperSamFreq);
            usbDesc.tSamFreq = StrToHex3BArray(tSamFreq);
            return usbDesc;
        }
    }

    [XmlType("AudioEndpoint_Descriptor")]
    public class CyAudioEndpointDescriptorTemplate : CyDescriptorTemplate
    {
        public string bInterval;
        public string bEndpointAddress;
        public string bmAttributes;
        public string wMaxPacketSize;
        public string bRefresh;
        public string bSynchAddress;


        public CyAudioEndpointDescriptorTemplate()
        {
        }

        public CyAudioEndpointDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bInterval = ByteToStr(((CyAudioEndpointDescriptor)usbDesc).bInterval);
            bEndpointAddress = ByteToStr(((CyAudioEndpointDescriptor)usbDesc).bEndpointAddress);
            bmAttributes = ByteToStr(((CyAudioEndpointDescriptor)usbDesc).bmAttributes);
            wMaxPacketSize = UshortToStr(((CyAudioEndpointDescriptor)usbDesc).wMaxPacketSize);
            bRefresh = ByteToStr(((CyAudioEndpointDescriptor)usbDesc).bRefresh);
            bSynchAddress = ByteToStr(((CyAudioEndpointDescriptor)usbDesc).bSynchAddress);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyAudioEndpointDescriptor usbDesc = new CyAudioEndpointDescriptor();

            usbDesc.bInterval = StrToHex1B(bInterval);
            usbDesc.bEndpointAddress = StrToHex1B(bEndpointAddress);
            usbDesc.bmAttributes = StrToHex1B(bmAttributes);
            usbDesc.wMaxPacketSize = StrToHex2B(wMaxPacketSize);
            usbDesc.bInterval = StrToHex1B(bInterval);
            usbDesc.bRefresh = StrToHex1B(bRefresh);
            usbDesc.bSynchAddress = StrToHex1B(bSynchAddress);

            return usbDesc;
        }
    }

    [XmlType("ASEndpoint_Descriptor")]
    public class CyASEndpointDescriptorTemplate : CyDescriptorTemplate
    {
        public CyUSBOtherTypes.CyAudioClassSpecEPDescriptorTypes bDescriptorSubtype;
        public string bmAttributes;
        public string bLockDelayUnits;
        public string wLockDelay;


        public CyASEndpointDescriptorTemplate()
        {
        }

        public CyASEndpointDescriptorTemplate(CyUSBDescriptor usbDesc)
        {
            bDescriptorSubtype = ((CyASEndpointDescriptor) usbDesc).bDescriptorSubtype;
            bmAttributes = ByteToStr(((CyASEndpointDescriptor)usbDesc).bmAttributes);
            bLockDelayUnits = ByteToStr(((CyASEndpointDescriptor)usbDesc).bLockDelayUnits);
            wLockDelay = UshortToStr(((CyASEndpointDescriptor)usbDesc).wLockDelay);
        }

        public override CyUSBDescriptor ConvertToUSBDesc()
        {
            CyASEndpointDescriptor usbDesc = new CyASEndpointDescriptor();

            usbDesc.bDescriptorSubtype = bDescriptorSubtype;
            usbDesc.bmAttributes = StrToHex1B(bmAttributes);
            usbDesc.bLockDelayUnits = StrToHex1B(bLockDelayUnits);
            usbDesc.wLockDelay = StrToHex2B(wLockDelay);

            return usbDesc;
        }
    }

    #endregion Audio Template
    //============================================================================================================
}
