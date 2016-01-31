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
namespace CapSense_v0_5
{
    #region CsPropsFather
    [Serializable()]
    public class CsPropsFather : ICustomTypeDescriptor
    {
        [XmlIgnore]
        [NonSerialized]
        public List<object> listObjects = new List<object>();

        public CsPropsFather()
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
                foreach (object item in listObjects)
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
        /// attributes. this restriction is not implemented here.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(m_thing_with_properties, attributes, true);
            //return hack_property_display_names(properties);

            PropertyDescriptorCollection pds = GetProperties();
            return pds;
        }

        /// <summary>
        /// Called to get the properties of this type.
        /// </summary>
        /// <returns></returns>
        public PropertyDescriptorCollection GetProperties()
        {
            // Create a collection object to hold property descriptors
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            PropertyDescriptorCollection pb;
            foreach (object item in listObjects)
            {
                pb = TypeDescriptor.GetProperties(item);

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

    #region clPropsComparer
    //For  marking custom objects
    public interface IStepeble
    { }

    //For Execution post serialization functions
    public interface IMyPostSerialization
    {
        void ExecutePostSerialization();
    }

    [Serializable()]
    public class clPropsComparer
    {
        public static object Cmapare(object[] listProps)
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
                            if (val.GetType() == typeof(IntElement))
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
                    val = Cmapare(GetFieldsListWithName(listPD, listProps, listPD[0][i].Name).ToArray());
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
            Type ICustomType = val.GetType().GetInterface(Convert.ToString(typeof(IStepeble)), true);
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
    public enum ElClassType { BYTE, WORD };

    //This class is using for byte or int property in modules
    [Serializable()]
    [TypeConverter(typeof(IntElementConverter))]
    public class IntElement
    {
        [XmlIgnore()]
        [NonSerialized]
        public ElClassType ElType = ElClassType.WORD;

        [XmlIgnore()]
        [NonSerialized]
        public string descr = "";

        [XmlIgnore()]
        [NonSerialized]
        public string name = "Empty";

        [XmlAttribute("Val")]
        public UInt16 Value;
        [XmlIgnore()]
        [NonSerialized]
        //[XmlAttribute("Min")]
        public UInt16 Min;// = 0;
        [XmlIgnore()]
        [NonSerialized]
        //[XmlAttribute("Max")]
        public UInt16 Max;// = 255;

        public IntElement()
        {
            takeMinMax(0, 255);
        }
        //public IntElement(ElClassType ElType)
        //{
        //    this.ElType = ElType;
        //    takeMinMax(0, 255);
        //}

        public IntElement(UInt16 value)
        {
            takeMinMax(0, 255);
            this.Value = value;
        }

        public IntElement(UInt16 min, UInt16 max)
        {
            takeMinMax(min, max);
            Value = min;
        }
        public IntElement(UInt16 value, UInt16 min, UInt16 max)
        {
            takeMinMax(min, max);
            this.Value = value;
        }
        public void takeMinMax(UInt16 min, UInt16 max)
        {
            if (min > max) throw new Exception("Mininum > Maximum");
            if (ElType == ElClassType.BYTE)
            {
                if ((max > byte.MaxValue) || (max < byte.MinValue)) throw new Exception("Mininum out of range");
                if ((max > byte.MaxValue) || (max < byte.MinValue)) throw new Exception(" Maximum out of range");
            }
            this.Min = min;
            this.Max = max;

            if ((Value < min) || (Value > max)) Value = min;
        }
        public bool Validate(UInt16 val)
        {
            if ((Min <= val) && (Max >= val))
                Value = val;
            else
            {
                System.Windows.Forms.MessageBox.Show("Value is out of range!! Range [" + Min + ".." + Max + "]", "Out of range",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                //if (Value == val) Value = Min;
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
                System.Windows.Forms.MessageBox.Show("Value is Incorrect", "Error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }
        }
        public void Validate(TextBox tb)
        {
            if (!Validate(tb.Text))
            {
                tb.Text = Value.ToString();
            }
        }
        public override string ToString()
        {
            return Value.ToString();
        }

    }

    //This class is using for byte property in modules
    [Serializable()]
    public class ByteElement
    {
        public const ElClassType ElType = ElClassType.BYTE;
        public ByteElement(byte value)
        {
            this.Value = value;
        }
        public ByteElement(byte min, byte max)
        {
            this.Min = min;
            this.Max = max;
        }
        public ByteElement(IntElement el)
        {
            this.Min = (byte)el.Min;
            this.Max = (byte)el.Max;
            this.Value = (byte)el.Value;
        }
        public byte Value;
        public byte Min = 0;
        public byte Max = 255;
        public void Validate(int val)
        {
            if ((Min <= val) && (Max >= val))
                Value = (byte)val;
            else
                //new Exception();
                System.Windows.Forms.MessageBox.Show("Value is out of range!! Range [" + Min + ".." + Max + "]", "Out of range",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

        }
    }
    #endregion

    #region IntElementConverter
    internal class IntElementConverter : ExpandableObjectConverter
    {

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is IntElement)
            {
                // Cast the value to an IntElement type
                IntElement emp = (IntElement)value;

                // Return value
                return emp.Value.ToString();
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
                    return new IntElement(Convert.ToUInt16(value));
                }
                catch
                {
                    throw new ArgumentException(
                        "Can not convert '" + (string)value +
                                           "' to type ");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                  System.Type destinationType)
        {
            if (destinationType == typeof(IntElement))
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

    #region cyCBConverter
    public static class cyCBConverter
    {
        public static string cyGetValue(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return ff.Text;
            }
            return ff.Items[ff.SelectedIndex].ToString();
        }
        public static object cyGetObject(ComboBox ff)
        {
            if (ff.SelectedIndex == -1)
            {
                return null;
            }
            return ff.Items[ff.SelectedIndex];
        }
        public static void cySetValue(ComboBox ff, string str)
        {
            for (int i = 0; i < ff.Items.Count; i++)

                if (ff.Items[i].ToString() == str)
                {
                    ff.SelectedIndex = i;
                    return;
                }
            //return ff.Items[ff.SelectedIndex].ToString();
        }
    }
    #endregion

    #region clDefProps
    public static class clDefProps
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

    #region BaseClone


    /// <summary>
    /// <b>BaseObject</b> class is an abstract class for you to derive from. <br>
    /// Every class that will be dirived from this class will support the <b>Clone</b> method automaticly.<br>
    /// The class implements the interface <i>ICloneable</i> and there for every object that will be derived <br>
    /// from this object will support the <i>ICloneable</i> interface as well.
    /// </summary>
    public abstract class BaseClone : ICloneable
    {
        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <returns>Reference to the new cloned object.</returns>
        public object Clone()
        {
            //First we create an instance of this specific type.
            object newObject = Activator.CreateInstance(this.GetType());

            //We get the array of fields for the new type instance.
            FieldInfo[] fields = newObject.GetType().GetFields();

            int i = 0;

            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                //We query if the fiels support the ICloneable interface.
                Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);

                if (ICloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    ICloneable IClone = (ICloneable)fi.GetValue(this);

                    //We use the clone method to set the new value to the field.
                    fields[i].SetValue(newObject, IClone.Clone());
                }
                else
                {
                    //If the field doesn't support the ICloneable interface then just set it.
                    fields[i].SetValue(newObject, fi.GetValue(this));
                }

                //Now we check if the object support the IEnumerable interface, so if it does
                //we need to enumerate all its items and check if they support the ICloneable interface.
                Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
                if (IEnumerableType != null)
                {
                    //Get the IEnumerable interface from the field.
                    IEnumerable IEnum = (IEnumerable)fi.GetValue(this);

                    //This version support the IList and the IDictionary interfaces to iterate
                    //on collections.
                    Type IListType = fields[i].FieldType.GetInterface("IList", true);
                    Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

                    int j = 0;
                    if (IListType != null)
                    {
                        //Getting the IList interface.
                        IList list = (IList)fields[i].GetValue(newObject);

                        foreach (object obj in IEnum)
                        {
                            //Checking to see if the current item support the ICloneable interface.
                            ICloneType = obj.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                //If it does support the ICloneable interface, we use it to set the clone of
                                //the object in the list.
                                ICloneable clone = (ICloneable)obj;

                                list[j] = clone.Clone();
                            }

                            //NOTE: If the item in the list is not support the ICloneable interface then
                            // in the cloned list this item will be the same item as in the original list
                            //(as long as this type is a reference type).

                            j++;
                        }
                    }
                    else if (IDicType != null)
                    {
                        //Getting the dictionary interface.
                        IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
                        j = 0;
                        foreach (DictionaryEntry de in IEnum)
                        {
                            //Checking to see if the item support the ICloneable interface.
                            ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

                            if (ICloneType != null)
                            {
                                ICloneable clone = (ICloneable)de.Value;

                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
            return newObject;
        }
    }

    #endregion

    #region m_StringWriter
    public class m_StringWriter : StringWriter
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

}
