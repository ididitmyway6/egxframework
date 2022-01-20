using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public class CustomEntryForm
    {
        public static void SetDefaultIfNull(object value) 
        {
           
        }
        public static T DefineInsert<T>(List<PropertyInfo> objectProperties, List<Control> usedControls) 
        {
            Type castedType;
            PropertyInfo property;
            object o = Activator.CreateInstance(typeof(T));
            foreach (Control c in usedControls)
            {
                property = objectProperties.Find(pi => pi.Name == c.Name);
                castedType = property.PropertyType;
                if (castedType.IsGenericType &&
        castedType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    castedType = castedType.GetGenericArguments()[0];
                }
                if (c.GetType() == typeof(Egx.EgxControl.EgxTextBox))
                {
                    var value = Convert.ChangeType(c.Text, castedType);
                    property.SetValue(o, value, null);
                }
                else if (c.GetType() == typeof(Egx.EgxControl.EgxDateText))
                {
                    var value = Convert.ChangeType(c.As<Egx.EgxControl.EgxDateText>().Value.GetValueOrDefault(DateTime.Now), castedType);
                    property.SetValue(o, value, null);
                }
                else if (c.GetType() == typeof(Egx.EgxControl.EgxComboBox))
                {
                    if (c.As<Egx.EgxControl.EgxComboBox>().SelectedValue != null && c.As<Egx.EgxControl.EgxComboBox>().SelectedValue != DBNull.Value)
                    {
                        var value = Convert.ChangeType(c.As<Egx.EgxControl.EgxComboBox>().SelectedValue, castedType);
                        property.SetValue(o, value, null);
                    }
                }
            }
            return (T)o;
        }

        public EgxBusinessField BusinessField { get; set; }

        public List<Control> GetBusinessControls(Form containedForm,Object obj) 
        {
            var l = EgxValidations.GetAllControls(containedForm.Controls);
            foreach (PropertyInfo pinfo in obj.GetType().GetProperties()) 
            {

            }
            return l;
        }

        public void Bind(Form containedForm,BindingSource bindingSource) 
        {
            foreach (Control c in EgxValidations.GetAllControls(containedForm.Controls))
            {
                c.DataBindings.Clear();
                switch (c.GetType().Name)
                {
                    case "EgxCheckBox":
                        c.DataBindings.Add("Checked", bindingSource, c.Name, true);
                        break;
                    case "EgxLookup":
                        c.DataBindings.Add("Text", bindingSource, c.Name, true);
                        break;
                    case "EgxComboBox":
                        c.DataBindings.Add("SelectedValue", bindingSource, c.Name, true);

                        break;
                    default:
                        c.DataBindings.Add("Text", bindingSource, c.Name, true);
                        break;
                }
            }
        }


        public void DisableBinding(Form containedForm)
        {
            foreach (Control c in EgxValidations.GetAllControls(containedForm.Controls))
            {
                c.DataBindings.Clear();
            }
        }


        /// <summary>
        /// It used to set specific property in specific instance by specific value.
        /// It Converts Input Value to standard one to deal with DAL Properly
        /// </summary>
        /// <param name="property">The Property to set</param>
        /// <param name="instance">The Instance which contain the property to set</param>
        /// <param name="value">The value assigned to the property (Input value)</param>
        private void SetBusinessObjectProp(PropertyInfo property, object instance, object value)
        {
            try
            {
                string type = "";
                if (value == null)
                {
                    #region IfGenericType
                    if (property.PropertyType.IsGenericType)
                    {
                        type = Nullable.GetUnderlyingType(property.PropertyType).Name;
                        //string s = value.GetType().Name;
                        #region Switch Between Types
                        switch (type)
                        {
                            case "Int32":
                                property.SetValue(instance, 0, null);
                                break;
                            case "String":
                                property.SetValue(instance, "", null);
                                break;
                            case "Boolean":
                                property.SetValue(instance, false, null);
                                break;
                            case "Byte":
                                property.SetValue(instance, 0, null);
                                break;
                            case "DateTime":
                                property.SetValue(instance, null, null);
                                break;
                            case "Single":
                                property.SetValue(instance, 0f, null);
                                break;
                            case "Byte[]":
                                property.SetValue(instance, 0, null);
                                break;
                            case "Double":
                                property.SetValue(instance, 0, null);
                                break;
                            case "Decimal":
                                property.SetValue(instance, decimal.Zero, null);
                                break;

                        }
                        #endregion
                    }
                    else
                    {
                        property.SetValue(instance, "", null);
                    }
                    #endregion
                }
                else
                {

                    if (property.PropertyType.IsGenericType)
                    {

                        property.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType), null);

                    }
                    else
                    {
                        property.SetValue(instance, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }
            catch (Exception exc) { MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void SetBusinessObjectData(object o, Form containerForm)
        {
            Control[] cn = null;
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                #region Get Values From Controls and then Set Business Object Instance
                List<Control> lst = EgxValidations.GetAllControls(containerForm.Controls);
                cn = lst.FindAll(cnt => cnt.Name == p.Name).ToArray();
                if (cn.Length > 0)
                {
                    string CntrlName = cn[0].GetType().Name;
                    switch (CntrlName)
                    {
                        case "EgxComboBox":
                            EgxControl.EgxComboBox cb = cn[0] as EgxControl.EgxComboBox;
                            if (cb.SelectedValue != null && cb.SelectedValue != DBNull.Value)
                            {
                                SetBusinessObjectProp(p, o, cb.SelectedValue);
                            }
                            else { SetBusinessObjectProp(p, o, null); }
                            break;

                        case "EgxTextBox":
                            EgxControl.EgxTextBox tb = cn[0] as EgxControl.EgxTextBox;
                            if (tb.Text.Length != 0)
                            {
                                Type t = p.PropertyType;
                                //Test input method goes here ................................
                                SetBusinessObjectProp(p, o, tb.Text);
                            }
                            else { SetBusinessObjectProp(p, o, null); }
                            break;
                        case "EgxDateText":
                            EgxControl.EgxDateText dtb = cn[0] as EgxControl.EgxDateText;
                            if (dtb.Text != null)
                            {
                                //  p.SetValue(ObjInstance, dtb.Value);
                                SetBusinessObjectProp(p, o, dtb.Value);
                            }
                            break;
                        case "EgxLookup":
                            EgxControl.EgxLookup lu = cn[0] as EgxControl.EgxLookup;
                            if (lu.Value != null)
                            {
                                SetBusinessObjectProp(p, o, lu.ID);
                            }
                            break;
                        case "EgxCheckBox":
                            EgxControl.EgxCheckBox ck = cn[0] as EgxControl.EgxCheckBox;
                            if (ck.Checked)
                            {
                                SetBusinessObjectProp(p, o, true);
                            }
                            else { SetBusinessObjectProp(p, o, false); }
                            break;
                        case "EgxTextLookup":
                            EgxControl.EgxTextLookup tlu = cn[0] as EgxControl.EgxTextLookup;
                            if (tlu.Value != null)
                            {
                                SetBusinessObjectProp(p, o, tlu.Value);
                            }
                            break;
                        case "RichTextBox":
                            RichTextBox rtb = cn[0] as RichTextBox;
                            if (rtb.Text != null)
                            {
                                SetBusinessObjectProp(p, o, rtb.Text);
                            }
                            break;
                    }
                }
                #endregion

                #region Get Values From Business Fields and Then Set Business Object Instance
                if (BusinessField.Keys.Contains(p.Name))
                {
                    if (BusinessField[p.Name] != null)
                    {
                        SetBusinessObjectProp(p, o, BusinessField[p.Name]);
                    }
                }
                #endregion
            }
        }
    }
}
