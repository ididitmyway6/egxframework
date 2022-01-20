using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Egx.EgxControl
{
    public class EgxValidations
    {
        static ErrorProvider ep = new ErrorProvider();
        public static List<Control> GetAllControls(IList ctrls)
        {
            List<Control> RetCtrls = new List<Control>();
            foreach (Control ctl in ctrls)
            {
                RetCtrls.Add(ctl);
                List<Control> SubCtrls = GetAllControls(ctl.Controls);
                RetCtrls.AddRange(SubCtrls);
            }
            return RetCtrls;
        }
        public static bool CheckMandatory(Form containerForm) 
        {
           
            List<Control> cntrls = GetAllControls(containerForm.Controls);
            foreach (Control c in cntrls) 
            {
                if (c.GetType().Name == "EgxTextBox") 
                {
                    if (c.As<EgxTextBox>().IsMandatory) 
                    {
                        if (c.As<EgxTextBox>().Text.Trim().Length == 0)
                        {
                            ep.SetError(c, c.As<EgxTextBox>().RequiredErrorText);
                            return false;
                        }
                        else { ep.SetError(c, ""); ep.Clear(); }
                    }
                }
                else if (c.GetType().Name == "EgxComboBox") 
                {
                    if (c.As<EgxComboBox>().IsMandatory)
                    {
                        if (c.As<EgxComboBox>().Text.Trim().Length == 0)
                        {
                            ep.SetError(c, c.As<EgxComboBox>().RequiredErrorText);
                            return false;
                        }
                        else { ep.SetError(c, ""); ep.Clear(); }
                    }
                }
                else if (c.GetType().Name == "EgxDateText")
                {
                    if (c.As<EgxDateText>().IsMandatory)
                    {
                        if (c.As<EgxDateText>().Text == "Invalid")
                        {
                            ep.SetError(c, c.As<EgxDateText>().RequiredErrorText);
                            return false;
                        }
                        else { ep.SetError(c, ""); ep.Clear(); }
                    }
                }
            }
            return true;
        }
    }
}
