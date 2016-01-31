/*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/



using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using CyDesigner.Extensions.Gde;
using CyDesigner.Extensions.Common;
namespace CapSense_v1_30
{
    #region CsPropsFather
    /// <summary>
    /// Used for sorting properties
    /// </summary>
    public class CyIntOrder : Attribute
    {
        int m_order = 0;
        public CyIntOrder(int order)
        {
            m_order = order;
        }
        public override string ToString()
        {
            return m_order.ToString();
        }
    }
    public class CyIntOrderComparer : System.Collections.IComparer
    {
        public CyIntOrderComparer()
        {
        }

        public int Compare(object x, object y)
        {
            PropertyDescriptor pd1 = (PropertyDescriptor)x;
            PropertyDescriptor pd2 = (PropertyDescriptor)y;

            int CompareResult = 0;
            if (pd1.Attributes[typeof(CyIntOrder)] == null) return 0;
            if (pd2.Attributes[typeof(CyIntOrder)] == null) return 0;

            int f1 = Convert.ToInt32(pd1.Attributes[typeof(CyIntOrder)].ToString());
            int f2 = Convert.ToInt32(pd2.Attributes[typeof(CyIntOrder)].ToString());
            if (f1 > f2) CompareResult = 1;
            if (f1 < f2) CompareResult = -1;
            return CompareResult;
        }
    }
    [Serializable()]
    public class CyCsPropsFather : ICustomTypeDescriptor
    {
        [XmlIgnore]
        [NonSerialized]
        public List<object> m_listObjects = new List<object>();

        public CyCsPropsFather()
        {
        }

        #region ICustomTypeDescriptor impl

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pdi)
        {
            if (pdi != null)
            {
                foreach (object item in m_listObjects)
                {
                    PropertyDescriptorCollection pb = TypeDescriptor.GetProperties(item);

                    for (int i = 0; i < pb.Count; i++)
                    {
                        if (pb[i].Name == pdi.Name)
                            return item;
                    }
                }
            }
            return this;
        }


        /// <summary>
        /// Called to get the properties of this type. Returns properties with certain
        /// attributes.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pds = GetProperties();
            return pds;
        }

        CyIntOrderComparer m_propsOrder = new CyIntOrderComparer();
        /// <summary>
        /// Called to get the properties of this type.
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties()
        {
            // Create a collection object to hold property descriptors
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection pb;
            foreach (object item in m_listObjects)
            {
                pb = TypeDescriptor.GetProperties(item);
                pb=pb.Sort(m_propsOrder);

                for (int i = 0; i < pb.Count; i++)
                {
                    pds.Add(pb[i]);
                }

            }
            return pds;
        }

        #endregion
    }
    #endregion

    //For marking custom objects
    public interface ICyStepeble
    { }

    //For Execution post serialization functions
    public interface ICyMyPostSerialization
    {
        void ExecutePostSerialization();
    }
    #region CyClPropsComparer
    [Serializable()]
    public class CyClPropsComparer
    {
        public static object Comapare(object[] listProps)
        {
            List<object> ListProps = new List<object>(listProps);
            object res = null;

            if (ListProps.Count == 0) return res;
            if (ListProps.Count == 1) return ListProps[0];

            res = Activator.CreateInstance(ListProps[0].GetType());

            List<List<FieldInfo>> listPD = new List<List<FieldInfo>>();

            //Get fields info
            for (int i = 0; i < ListProps.Count; i++)
            {
                listPD.Add(new List<FieldInfo>(ListProps[i].GetType().GetFields()));
            }

            //Step by step comparing
            for (int i = 0; i < listPD[0].Count; i++)
            {
                object val = listPD[0][i].GetValue(ListProps[0]);
                if (!isCustomObject(val))
                {
                    for (int j = 0; j < listPD.Count; j++)
                        if (val.ToString() != listPD[0][i].GetValue(ListProps[j]).ToString())
                        {
                            if (val.GetType() == typeof(CyIntElement))
                            {
                                val = null;
                            }
                            else if (val.GetType() == typeof(bool))
                            {
                                val = true;
                            }
                            else
                                val = -1;
                            break;
                        }
                }
                else
                {
                    //Compare sub branch
                    val = Comapare(GetFieldsListWithName(listPD, listProps, listPD[0][i].Name).ToArray());
                }
                listPD[0][i].SetValue(res, val);
            }

            return res;
        }

        #region Functions
        //Get List of objects with same name from array
        public static List<object> GetFieldsListWithName(List<List<FieldInfo>> listPD, object[] ListProps, string name)
        {
            List<object> res = new List<object>();
            for (int i = 0; i < listPD.Count; i++)
            {

                for (int j = 0; j < listPD[i].Count; j++)
                    if (listPD[i][j].Name == name)
                    {
                        res.Add(listPD[i][j].GetValue(ListProps[i]));
                        break;
                    }
            }
            return res;
        }

        //If is it custom object
        public static bool isCustomObject(object val)
        {
            Type ICustomType = val.GetType().GetInterface(Convert.ToString(typeof(ICyStepeble)), true);
            if (ICustomType != null) return true;
            else return false;
        }

        public static IEnumerable<object> GetIEnum(object[] objm)
        {
            foreach (object item in objm)
            {
                yield return item;
            }
        }
        #endregion
    }
    #endregion

    #region Elements definition
    public enum E_EL_CLASS_TYPE { BYTE, WORD };

    //This class is used for holding byte or int properties
    [Serializable()]
    [TypeConverter(typeof(CyIntElementConverter))]
    public class CyIntElement
    {
        [XmlIgnore()]
        [NonSerialized]
        public E_EL_CLASS_TYPE m_ElType = E_EL_CLASS_TYPE.WORD;

        [XmlIgnore()]
        [NonSerialized]
        public string m_descr = "";

        [XmlIgnore()]
        [NonSerialized]
        public string m_name = "Empty";

        [XmlAttribute("Val")]
        public UInt16 m_Value;
        [XmlIgnore()]
        [NonSerialized]
        //[XmlAttribute("Min")]
        public UInt16 m_Min;// = 0;
        [XmlIgnore()]
        [NonSerialized]
        //[XmlAttribute("Max")]
        UInt16 m_Max;// = 255;

        public CyIntElement()
        {
            TakeMinMax(0, 255);
        }
        //public IntElement(ElClassType ElType)
        //{
        //    this.ElType = ElType;
        //    takeMinMax(0, 255);
        //}

        public CyIntElement(UInt16 value)
        {
            TakeMinMax(0, 255);
            this.m_Value = value;
        }

        public CyIntElement(UInt16 min, UInt16 max)
        {
            TakeMinMax(min, max);
            m_Value = min;
        }
        public CyIntElement(UInt16 value, UInt16 min, UInt16 max)
        {
            TakeMinMax(min, max);
            this.m_Value = value;
        }
        public void TakeMinMax(UInt16 min, UInt16 max)
        {
            if (min > max) throw new Exception("Mininum > Maximum");
            if (m_ElType == E_EL_CLASS_TYPE.BYTE)
            {
                if ((max > byte.MaxValue) || (max < byte.MinValue)) throw new Exception("Mininum out of range");
                if ((max > byte.MaxValue) || (max < byte.MinValue)) throw new Exception(" Maximum out of range");
            }
            this.m_Min = min;
            this.m_Max = max;

            if ((m_Value < min) || (m_Value > max)) m_Value = min;
        }
        public bool Validate(UInt16 val)
        {
            if ((m_Min <= val) && (m_Max >= val))
                m_Value = val;
            else
            {
                System.Windows.Forms.MessageBox.Show("Value is out of range! Range [" + m_Min + ".." + m_Max + "]"
                    , "Warning", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public bool Validate(string str)
        {
            try
            {
                ushort val = Convert.ToUInt16(str);
                return Validate(val);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Value is incorrect", "Warning",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
        }
        public void Validate(TextBox tb)
        {
            if (!Validate(tb.Text))
            {
                tb.Text = m_Value.ToString();
            }
        }
        public override string ToString()
        {
            return m_Value.ToString();
        }

    }
    #endregion

    #region IntElementConverter
    internal class CyIntElementConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is CyIntElement)
            {
                // Cast the value to an IntElement type
                CyIntElement emp = (CyIntElement)value;

                // Return value
                return emp.m_Value.ToString();
            }
            return base.ConvertTo(context, culture, value, destType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context,
                              CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return new CyIntElement(Convert.ToUInt16(value));
                }
                catch
                {
                    throw new ArgumentException("Could not convert '" + (string)value + "' to type");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                  System.Type destinationType)
        {
            if (destinationType == typeof(CyIntElement))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context,
                      System.Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
    }
    #endregion

    #region CyConverter
    public static class CyIntConverter
    {
        public static string CyGetValue(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return ff.Text;
            }
            return ff.Items[ff.SelectedIndex].ToString();
        }
        public static object CyGetObject(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return null;
            }
            return ff.Items[ff.SelectedIndex];
        }
        public static void CySetValue(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)

                if (ff.Items[i].ToString() == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
        }
        public static void SetValue(ref int main, int val)
        {
            if (val != -1)
            {
                main = val;
            }
        }
        public static void SetValue(ref CyIntElement main, TextBox val)
        {
            if (val.Text != "")
            {
                main.Validate(val.Text);
                val.Text = main.m_Value.ToString();
            }
        }
        public static string GetValue(CyIntElement val)
        {
            if (val != null)
            {
                return val.m_Value.ToString();
            }
            return "";
        }
    }
    #endregion

    #region clDefProps
    public static class CyClDefProps
    {
        public static void SetDefaultProps(object obj)
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);
            for (int i = 0; i < pdc.Count; i++)
            {
                AttributeCollection attributes = pdc[i].Attributes;
                DefaultValueAttribute myAttribute =
               (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
                pdc[i].SetValue(obj, myAttribute.Value);
            }

        }
    }
    #endregion

    #region m_StringWriter
    public class CyMyStringWriter : StringWriter
    {
        string lastStr = "";
        public override void Write(string value)
        {
            lastStr += value;
        }
        public override void WriteLine(string value)
        {
            base.WriteLine(lastStr + value);
            lastStr = "";
        }
        public void Remove(int StartIndex)
        {
            lastStr.TrimEnd();
            lastStr = lastStr.Remove(StartIndex);
        }
        public int Get0Length()
        {
            return lastStr.Length;
        }
        public override string ToString()
        {
            return base.ToString() + lastStr;
        }

    }
    #endregion

    #region EnumConverters
    class CyCustomBooleanConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture,
        object value,
        Type destType)
        {
            return (bool)value ?
            "Enabled" : "Disabled";
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
        CultureInfo culture,
        object value)
        {
            return (string)value == "Enable";
        }
    }
    class CyIntEnumConverter : EnumConverter
    {
        private Type m_enumType;
        /// Initializing instance
        public CyIntEnumConverter(Type type)
            : base(type)
        {
            m_enumType = type;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
            Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture,
            object value, Type destType)
        {
            return GetEnumString(value );
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context,
            Type srcType)
        {
            return srcType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            return GetEnumValue(value,m_enumType);
        }
        /// <summary>
        /// Convert enum value to enum description string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetEnumValue(object value, Type _enumType)
        {
            foreach (FieldInfo fi in _enumType.GetFields())
            {
                DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

                if ((dna != null) && ((string)value == dna.Description))
                    return Enum.Parse(_enumType, fi.Name);
            }
            return Enum.Parse(_enumType, (string)value);
        }
        /// <summary>
        /// Convert enum description string to enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumString(object value )
        {
            Type _enumType = value.GetType();
            FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, value));
            DescriptionAttribute dna =
                (DescriptionAttribute)Attribute.GetCustomAttribute(
                fi, typeof(DescriptionAttribute));

            if (dna != null)
                return dna.Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Get all enum descriptions
        /// </summary>
        /// <param name="_enumType"></param>
        /// <returns></returns>
        public static string[] GetEnumStringList(Type _enumType)
        {
            List<string> res = new  List<string>();
            foreach (object item in Enum.GetValues(_enumType))
            {
                res.Add(GetEnumString(item));
            }
            return res.ToArray();
        }
    }
    #endregion
}
