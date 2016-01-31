/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
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
using System.Diagnostics;

namespace CapSense_CSD_v2_0
{
    #region CsPropsFather
    [Serializable()]
    public class CyCsPropsFather : ICustomTypeDescriptor
    {
        public CyCsPropsFather()
        {
        }

        public virtual object[] GetAdditionalProperties()
        {
            return new object[] { };
        }
        public virtual PropertyDescriptorCollection ValidateProperties(PropertyDescriptorCollection pb)
        {
            return pb;
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
            object[] list = GetAdditionalProperties();
            if (pdi != null)
            {
                for (int i = -1; i < list.Length; i++)
                {
                    object item;
                    PropertyDescriptorCollection pb;
                    if (i == -1)
                    {
                        pb = ValidateProperties(TypeDescriptor.GetProperties(this.GetType()));
                        item = this;
                    }
                    else
                    {
                        item = list[i];
                        pb = TypeDescriptor.GetProperties(item);
                    }

                    for (int j = 0; j < pb.Count; j++)
                    {
                        if (pb[j].DisplayName == pdi.DisplayName)
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

        /// <summary>
        /// Called to get the properties of this type.
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties()
        {
            object[] list = GetAdditionalProperties();
            // Create a collection object to hold property descriptors
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            PropertyDescriptorCollection pb;

            for (int i = -1; i < list.Length; i++)
            {
                if (i == -1)
                {
                    //Add main instance properties
                    pb = TypeDescriptor.GetProperties(this.GetType());
                }
                else
                    pb = TypeDescriptor.GetProperties(list[i]);

                //Load to result list
                for (int j = 0; j < pb.Count; j++)
                {
                    pds.Add(pb[j]);
                }
            }
            pds = ValidateProperties(pds);
            return pds;
        }

        #endregion
    }
    #endregion

    //For Execution post serialization functions
    public interface ICyCSPostSerialization
    {
        void ExecutePostSerialization();
    }
    #region CyClPropsComparer

    public class CyCSCompareEnableAttribute : Attribute
    {
        public CyCSCompareEnableAttribute()
        {
        }
    }

    [Serializable()]
    public static class CyClassPropsComparer
    {
        #region Comapare
        public static object Comapare(object[] listProps)
        {
            if (listProps == null || listProps.Length == 0) return null;
            if (listProps.Length == 1) return listProps[0];

            object res = Activator.CreateInstance(listProps[0].GetType());

            FieldInfo[] listFieldsInfo = listProps[0].GetType().GetFields();
            PropertyInfo[] listPropertiesInfo = listProps[0].GetType().GetProperties();


            //Step by step comparing
            for (int i = 0; i < listFieldsInfo.Length; i++)
            {
                object[] obj = listFieldsInfo[i].GetCustomAttributes(typeof(CyCSCompareEnableAttribute), false);
                if (obj != null && obj.Length > 0)
                {
                    object val = listFieldsInfo[i].GetValue(listProps[0]);
                    for (int j = 0; j < listProps.Length; j++)
                        if (val.ToString() != listFieldsInfo[i].GetValue(listProps[j]).ToString())
                        {
                            if (val.GetType() == typeof(CyCsElement)) val = null;
                            else if (val.GetType() == typeof(bool)) val = true;
                            else if (val.GetType() == typeof(CheckState)) val = CheckState.Indeterminate;
                            else val = -1;
                            break;
                        }
                    listFieldsInfo[i].SetValue(res, val);
                }
            }
            for (int i = 0; i < listPropertiesInfo.Length; i++)
            {
                object[] obj = listPropertiesInfo[i].GetCustomAttributes(typeof(CyCSCompareEnableAttribute), false);
                if (obj != null && obj.Length > 0)
                {
                    //Compare sub branch
                    object val = Comapare(GetFieldsList(listPropertiesInfo[i], listProps));
                    listPropertiesInfo[i].SetValue(res, val, null);
                }
            }

            return res;
        }
        //Get List of objects with same name from array
        private static object[] GetFieldsList(PropertyInfo property, object[] listProps)
        {
            List<object> res = new List<object>();
            for (int i = 0; i < listProps.Length; i++)
            {
                res.Add(property.GetValue(listProps[i], null));
            }
            return res.ToArray();
        }
        #endregion

        #region PostSerializationIMyExecute
        public static void PostSerializationSearch(object obj)
        {
            if (obj != null && obj.GetType() != typeof(string))
                if (obj.GetType().IsClass)
                {
                    ExecutePostSerializationMethod(obj);

                    FieldInfo[] fields = obj.GetType().GetFields();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        FieldInfo field = fields[i];
                        object val = field.GetValue(obj);
                        if (field.IsNotSerialized == false)
                        {
                            //Check for next step
                            //Check for the IEnumerable interface
                            if ((new List<Type>(field.FieldType.GetInterfaces())).Contains(typeof(IEnumerable)))
                            {
                                //Get the IEnumerable interface from the field.
                                IEnumerable IEnum = (IEnumerable)val;
                                foreach (object item in IEnum)
                                {
                                    if (new List<Type>(item.GetType().GetInterfaces()).
                                        Contains(typeof(ICyCSPostSerialization)))
                                    {
                                        //Getting the interface.
                                        ICyCSPostSerialization typed_val = (ICyCSPostSerialization)item;
                                        typed_val.ExecutePostSerialization();
                                    }
                                    //Next step
                                    PostSerializationSearch(item);
                                }
                            }
                            else if (field.FieldType.IsSerializable)
                            {
                                //Next step
                                if (obj.GetType() != (field.FieldType))
                                    PostSerializationSearch(val);
                            }
                        }
                    }
                }
        }
        static void ExecutePostSerializationMethod(object obj)
        {
            if (obj == null) return;

            Type inter = obj.GetType().GetInterface(typeof(ICyCSPostSerialization).Name, true);
            if (inter != null)
            {
                //Getting the interface.
                ICyCSPostSerialization typed_val = (ICyCSPostSerialization)obj;
                typed_val.ExecutePostSerialization();
            }
        }
        #endregion

        #region SetDefaultProps
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
        #endregion
    }
    #endregion

    #region CyCsElement
    public enum CyElementType { Int, Double };
    //This class is used for holding default properties 
    public class CyCsElement
    {
        public double m_value;
        public double m_min = 0;
        public double m_max = 255;
        public CyElementType m_type = CyElementType.Int;

        public CyCsElement()
        {
        }
        public CyCsElement(double value)
        {
            this.m_value = value;
        }
        public CyCsElement(double value, double min, double max)
            : this(value)
        {
            TakeMinMax(min, max);
        }
        public CyCsElement(double value, double min, double max, CyElementType type)
            : this(value, min, max)
        {
            m_type = type;
            TakeMinMax(min, max);
        }


        public void TakeMinMax(double min, double max)
        {
            if (min > max)
            {
                System.Diagnostics.Debug.Assert(false);
                return;
            }
            this.m_min = min;
            this.m_max = max;
            if ((m_value < min) || (m_value > max))
            {
                System.Diagnostics.Debug.Assert(false);
                m_value = min;
            }
        }
        public bool CheckRange(object val)
        {
            return CheckRange(val, false);
        }
        public bool CheckRange(object val, bool showMessage)
        {
            try
            {
                double value = Double.Parse(val.ToString());
                if (m_type == CyElementType.Int)
                    value = int.Parse(val.ToString());
                if ((m_min > value) || (m_max < value)) throw new Exception();
            }
            catch
            {
                if (showMessage)
                    System.Windows.Forms.MessageBox.Show(String.Format(CyCsResource.ValueLitation, m_min, m_max)
                        , "Warning", System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        public override string ToString()
        {
            return m_value.ToString();
        }

    }
    #endregion

    #region EnumConverter
    class CyCsEnumConverter : EnumConverter
    {
        private Type m_enumType;
        /// Initializing instance
        public CyCsEnumConverter(Type type)
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
            return GetDescription(value);
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
            return GetValue(value, m_enumType);
        }
        /// <summary>
        /// Convert enum description string to enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValue(object value, Type _enumType)
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
        /// Convert enum value to enum description string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(object value)
        {
            Type _enumType = value.GetType();
            if (value is bool)
            {
                _enumType = typeof(CyEnDis);
                value = ((bool)value) ? CyEnDis.Enabled : CyEnDis.Disabled;
            }
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
        public static string[] GetDescriptionList(Type _enumType)
        {
            List<string> res = new List<string>();
            foreach (object item in Enum.GetValues(_enumType))
            {
                res.Add(GetDescription(item));
            }
            return res.ToArray();
        }
        public static void SetValue(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)

                if (ff.Items[i].ToString() == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
        }
        public static bool TryValue<T>(int value)
        {
            try
            {
                Array arr = Enum.GetValues(typeof(T));
                foreach (object item in arr)
                    if ((int)item == value)
                    {
                        return true;
                    }
            }
            catch 
            {
            }
            return false;
        }
    }
    #endregion

    /// <summary>
    /// Provide functions for bits operations
    /// </summary>
    public static class CyBitOperations
    {
        public static void AddBit(ref UInt32 val, int pos)
        {
            val |= (UInt32)(1 << pos);
        }

        public static void AddBit(ref UInt16 val, int pos)
        {
            val |= (UInt16)(1 << pos);
        }

        public static void AddBitRange(ref UInt32 val, UInt32 sourse, int pos)
        {
            val |= (sourse << pos);
        }

        public static bool IsTrue(UInt32 val, int pos)
        {
            return ((1 << pos) & val) > 0;
        }
    }
}
