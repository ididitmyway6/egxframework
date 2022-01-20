using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using Egx.EgxBusiness;
namespace Egx.EgxControl
{
    public enum FormAction { Insert, Update, Delete, Other, Select, Navigation ,AddNew}

    public partial class EgxEntryForm : EgxForm
    {
        int intTest = 0;
        float floatTest = 0f;
        double doubleTest = 0;
        DateTime dateTest = DateTime.Now;
        private object _preActionObject;
      //  public bool AutoGenerateFormGridColumns { get { return this.dgv.AutoGenerateColumns; } set { this.dgv.AutoGenerateColumns = value; } }
        [DefaultValue(null)]
        public EgxBusinessField BusinessField { get; set; }
        public Type BusinessObject { get; set; }
        public FormAction FormMode { get; set; }
        public BindingSource BindSource { get; set; }
        public EgxNavigator Navigator { get; set; }
        public string BusinessObjectName { get; set; }
        public void NewMode(object sender)
        {
            addNew_Click(sender, null);
            save.Enabled = true;
        }
        private object Obj
        {
            get
            {
                try
                {
                    return Activator.CreateInstance(BusinessObject);
                }
                catch { return null; }
            }
            set { Obj = value; }
        }
        public BindingSource GridBindingSource { get; set; }
        public List<PropertyInfo> BusinessObjectProperties { get; set; }
        [DefaultValue(true)]
        public bool ShowFormGrid { get; set; }
        public delegate void PreAction(PreActionEventArgs args);
        public event PreAction PreActionEvent;
        public delegate void AfterAction(AfterActionEventArgs args);
        public event AfterAction AfterActionEvent;
        Thread t;
        public List<Error> Err = new List<Error>();
        public object CurrentObject { get; set; }
        public int CurrentPosition { get; set; }
        public void DisableBinding()
        {
            foreach (Control c in GetFormControles())
            {
                c.DataBindings.Clear();
            }
        }
        public void NewErrorList() { Err.Clear(); }
        public void SetErrorText(string errorText)
        {
            Error err = new Error();
            err.ErrorText = errorText;
            Err.Add(err);
        }
        public void ResetBinding()
        {
            foreach (Control c in GetFormControles())
            {
                c.DataBindings.Clear();
                switch (c.GetType().Name)
                {
                    case "EgxCheckBox":
                        c.DataBindings.Add("Checked", GridBindingSource, c.Name,true);
                        break;
                    case "EgxLookup":
                        c.DataBindings.Add("Text", GridBindingSource, c.Name, true);
                        break;
                    case "EgxComboBox":
                        c.DataBindings.Add("SelectedValue", GridBindingSource, c.Name, true);
                        
                        break;
                    default:
                        c.DataBindings.Add("Text", GridBindingSource, c.Name, true);
                        break;
                        //Add here for EgxTextLookup True Format ............................


                }
            }
        }
        public void SetupForm<T>(T BusinessObjType, string PKName,string FindBy, string LookupQuery, string LookupValueMember)
        {
            BusinessObject = typeof(T);
            egxNavigator1.PKName = PKName;
            egxNavigator1.FindBy = FindBy;
            egxNavigator1.SetupObject(Activator.CreateInstance(typeof(T)));
            egxNavigator1.LookupQuery = LookupQuery;
            egxNavigator1.LookupValueMember = LookupValueMember;
            Navigator = egxNavigator1;
            RefreshFormData();
        }

        public void SetupForm<T>(T BusinessObjType, string PKName,string FindBy, string LookupID)
        {
            BusinessObject = typeof(T);
            egxNavigator1.PKName = PKName;
            egxNavigator1.FindBy = FindBy;
            egxNavigator1.SetupObject(Activator.CreateInstance(typeof(T)));
            egxNavigator1.LookupID = LookupID;
            Navigator = egxNavigator1;
            RefreshFormData();
        }
        //public bool ValidInput(Control control ,PropertyInfo property) 
        //{
        //    string type = property.PropertyType.Name;
        //    if (property.PropertyType == typeof(int?)) 
        //    {
        //        if (int.TryParse(control.Text, out intTest)) { }
        //    }
        //    else if (property.PropertyType == typeof(float?)) { }
        //    else if (property.PropertyType == typeof(DateTime?)) { }
        //    else if (property.PropertyType == typeof(double?)) { }
        //    return true;
        //}
        private void SetBusinessObjectData(object o)
        {
            Control[] cn = null;
            foreach (PropertyInfo p in BusinessObjectProperties)
            {
                #region Get Values From Controls and then Set Business Object Instance
                cn = CntlPanel.Controls.Find(p.Name, true);
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

        public  void SetBusinessObjectData(object o,Form containerForm)
        {
            Control[] cn = null;
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                #region Get Values From Controls and then Set Business Object Instance
                List<Control> lst =   EgxValidations.GetAllControls(containerForm.Controls);
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
        public EgxBusiness.SystemMessage Message;
        public void ShowMessage()
        {
            panel2.Visible = true;
            Panel.CheckForIllegalCrossThreadCalls = false;
            t = new Thread(new ThreadStart(ShowMessageThread));

        }
        public void ShowMessageThread()
        {
            Thread.Sleep(1000);

            panel2.Visible = false;
        }
        public SystemMessage PrepareInsertCommand()
        {
            object o = Activator.CreateInstance(BusinessObject);
            BusinessField.Clear();
            SetBusinessObjectData(o);
            OnPreAction(new PreActionEventArgs() { ActionType = FormAction.Insert, CurrentObject = o });
            SetBusinessObjectData(o);
            //foreach (PropertyInfo p in BusinessObjectProperties)
            //{
            #region Get Values From Controls and then Set Business Object Instance
            //    cn = CntlPanel.Controls.Find(p.Name, true);
            //    if (cn.Length > 0)
            //    {
            //        string CntrlName = cn[0].GetType().Name;
            //        switch (CntrlName)
            //        {
            //            case "EgxComboBox":
            //                EgxControl.EgxComboBox cb = cn[0] as EgxControl.EgxComboBox;
            //                if (cb.SelectedValue != null)
            //                {
            //                    SetBusinessObjectProp(p, o, cb.SelectedValue);
            //                }
            //                break;

            //            case "EgxTextBox":
            //                EgxControl.EgxTextBox tb = cn[0] as EgxControl.EgxTextBox;
            //                if (tb.Text.Length != 0)
            //                {
            //                    SetBusinessObjectProp(p, o, tb.Text);
            //                }
            //                else { SetBusinessObjectProp(p, o, null); }
            //                break;
            //            case "EgxDateText":
            //                EgxControl.EgxDateText dtb = cn[0] as EgxControl.EgxDateText;
            //                if (dtb.Text != null)
            //                {
            //                    //  p.SetValue(ObjInstance, dtb.Value);
            //                    SetBusinessObjectProp(p, o, dtb.Value);
            //                }
            //                break;
            //            case "EgxLookup":
            //                EgxControl.EgxLookup lu = cn[0] as EgxControl.EgxLookup;
            //                                if (lu.Value != null)
            //                                {
            //                     p.SetValue(o, lu.Value);
            //                 }
            //                 break;
            //             case "EgxCheckBox":
            //                 EgxControl.EgxCheckBox ck = cn[0] as EgxControl.EgxCheckBox;
            //                 if (ck.Checked)
            //                 {
            //                     SetBusinessObjectProp(p, o, true);
            //                 }
            //                 else { SetBusinessObjectProp(p, o, false); }
            //                 break;
            //         }
            //    }
            #endregion

            #region Get Values From Business Fields and Then Set Business Object Instance
            //    if (BusinessField.Keys.Contains(p.Name))
            //    {
            //        if (BusinessField[p.Name] != null)
            //        {
            //            SetBusinessObjectProp(p, o, BusinessField[p.Name]);
            //        }
            //    }
            #endregion
            //}
            Message = new EgxBusiness.SystemMessage();
            Message = (EgxBusiness.SystemMessage)o.GetType().GetMethod("Insert").Invoke(o, null);
            if (Message.Type == EgxBusiness.MessageType.Pass)
            {
                ShowMessage();
                t.Start();
                if (ShowFormGrid) { ErrList.Hide(); } else { panel1.Hide(); ErrList.Hide(); }


            }
            else
            {
                NewErrorList();
                SetErrorText(Message.Message);
                ErrList.DataSource = Err;
                ErrList.Refresh();
                if (ShowFormGrid) { ErrList.Show(); } else { panel1.Show(); ErrList.Show(); }
            }
            return Message;
        }

        public SystemMessage PrepareUpdateCommand()
        {
            BusinessField.Clear();
            var o = GridBindingSource.Current;
            OnPreAction(new PreActionEventArgs() { ActionType = FormAction.Update, CurrentObject = _preActionObject });
            SetBusinessObjectData(o);
           return o.GetType().GetMethod("Update").Invoke(o, null).As<SystemMessage>();
        }

        public void PrepareDeleteCommand()
        {
            DialogResult res = MessageBox.Show("هل انت متأكد من اجراء عملية الحزف ؟", "رسالة تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                object o = GridBindingSource.Current;
                OnPreAction(new PreActionEventArgs() { ActionType= FormAction.Delete, CurrentObject=o });
                if (egxNavigator1.IsFirst())
                {
                    GridBindingSource.Clear();
                    GridBindingSource.DataSource = egxNavigator1.Next();
                    egxNavigator1.FirstAction();
                    o.GetType().GetMethod("Delete").Invoke(o, null);
                }
                else if (egxNavigator1.ISLast())
                {
                    GridBindingSource.Clear();
                    GridBindingSource.DataSource = egxNavigator1.Previous();
                    egxNavigator1.LastAction();
                    o.GetType().GetMethod("Delete").Invoke(o, null);
                }
                else
                {
                    GridBindingSource.Clear();
                    GridBindingSource.DataSource = egxNavigator1.Previous();
                    o.GetType().GetMethod("Delete").Invoke(o, null);
                }
                FormMode = FormAction.Delete;
                RefreshFormData();
            }
        }
        public EgxEntryForm()
        {
            InitializeComponent();

            BusinessField = new EgxBusinessField();
            CntlPanel.Paint += CntlPanel_Paint;
         //   dgv.CellEnter += dgv_CellEnter;
            CurrentObject = null;
            CurrentPosition = -1;
            this.save.EnabledChanged+=save_EnabledChanged;
            #region تجهيز دعم اختصارات لوحة المفاتيح
            KeyPreview = true;
            KeyDown += EgxEntryForm_KeyDown;
            #endregion
        }

        private void save_EnabledChanged(object sender, EventArgs e)
        {
            if (save.Enabled)
            {
                egxNavigator1.Visible = false;
            }
            else 
            {
                egxNavigator1.Visible = true;
            }
        }

        #region دعم اختصارات لوحة المفاتيح
        private void EgxEntryForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Save
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                save_Click(null, null);
            }//Delete
            else if ((e.Control && e.KeyCode == Keys.D) || (e.Shift && e.KeyCode == Keys.Delete) || (e.Control && e.KeyCode == Keys.Delete))
            {
                e.SuppressKeyPress = true;
                Delete_Click(null, null);
            }//New 
            else if (e.Control && e.KeyCode == Keys.N)
            {
                e.SuppressKeyPress = true;
                addNew_Click(null, null);
            }// Get Information Control 
            else if (e.Control && e.KeyCode == Keys.I)
            {
                e.SuppressKeyPress = true;
                MessageBox.Show("Form Name :{0} \nControl Name : {1} \nTag Value : {2} \nTable Name : {3}\nValue: {4}"
                    .Frmt(Name, ActiveControl.Name, ActiveControl.Tag, BusinessObject.Name,
                    ActiveControl is TextBox ? ActiveControl.Text : ActiveControl is CheckBox ? (ActiveControl as CheckBox).CheckState.Nz() :
                    ActiveControl is ComboBox ? (ActiveControl as ComboBox).SelectedValue.Nz() : ""),
                    EgxDataAccess.GProperties.ProjectName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }// RefreshFormData
            else if (e.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
                RefreshFormData();
            }// Search Data
            else if (e.Control && e.KeyCode == Keys.F)
            {
                e.SuppressKeyPress = true;
                if (egxNavigator1.LookupQuery == "" || egxNavigator1.LookupValueMember == "" && egxNavigator1.LookupID.Trim().Length>0) 
                {
                    var luForm = new EgxLookupForm();
                    luForm.FormClosing += luForm_FormClosing;
                    luForm.LookupID = egxNavigator1.LookupID;
                    egxNavigator1.txtFind.Visible = true;
                    luForm.ShowDialog();
                }
                else
                {
                    var luForm = new EgxLookupForm();
                    luForm.FormClosing += luForm_FormClosing;
                    luForm.SqlQuery = egxNavigator1.LookupQuery;
                    luForm.ValueMember = egxNavigator1.LookupValueMember;
                    luForm.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    egxNavigator1.txtFind.Visible = true;
                    luForm.ShowDialog();
                }
            }
        }

        void luForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((EgxLookupForm)sender).Value == null) return;
            save_Click(null, null);
            egxNavigator1.txtFind.Text = ((EgxLookupForm)sender).Value.ToString();
            egxNavigator1.DataSource.DataSource = egxNavigator1.Find(egxNavigator1.FindBy);
            egxNavigator1.onNavigate(new EgxNavigatorEventArgs() { CurrentObject=egxNavigator1.CurrentObject, CurrentIndex=egxNavigator1.CurrentIndex });
            egxNavigator1.txtFind.Visible = false;

        }

        #endregion

        //Set form data with current clickable Row.
        //private void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (ShowFormGrid)
        //    {
        //        if (dgv.SelectedCells.Count == 1)
        //        {
        //            int r = e.RowIndex;
        //            object o = dgv[e.ColumnIndex, e.RowIndex].OwningRow.DataBoundItem;
        //            if (o != null)
        //            {
        //                SetFormData(o);
        //                SetBusinessFieldValue(o);
        //            }
        //            else { o = Obj; }
        //        }
        //        int i = dgv.SelectedRows.Count;
        //    }
        //}

        //Paint ControlPanel Border
        private void CntlPanel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, 1, ButtonBorderStyle.Solid, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Solid);
        }

        //Paint ToolStrip Border
        private void toolStrip2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, 1, ButtonBorderStyle.Solid, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Solid);
        }

        public object GetBusinessField(string fieldName) 
        {
            object obj;
            if (GridBindingSource.Current != null)
            {
                 obj = GridBindingSource.Current.GetType().GetProperty(fieldName).GetValue(GridBindingSource.Current, null);
            }
            else { obj = null; }
            var current = obj;
            return current;
        }

        //Prepare Entry Form to receive data 
        //and initialize the important objects which system will be need in the future.
        protected override void OnShown(EventArgs e)
        {
           
            if (!DesignMode)
            {
                if (ShowFormGrid) { panel1.Visible = true; } else { panel1.Visible = false; }
                List<PropertyInfo> props = new List<PropertyInfo>();
                foreach (PropertyInfo p in BusinessObject.GetProperties())
                {
                    props.Add(p);
                }
                BusinessObjectProperties = props;
                base.OnShown(e);
                if (!DesignMode)
                {
                    foreach (Control c in GetFormControles())
                    {
                        if (c.GetType().Name == "EgxCheckBox")
                        {
                            c.Click += c_Click;

                        }
                        else if (c.GetType().Name == "EgxComboBox")
                        {
                            ((EgxComboBox)c).DropDown += EgxEntryForm_DropDown;
                        }
                        else if (c.GetType().Name == "EgxTextLookup") 
                        {
                            (c as EgxTextLookup).LookupClosingEvent+=EgxEntryForm_LookupClosingEvent;
                        }
                        else { c.KeyPress += c_KeyPress; }
                    }
                    save.Enabled = false;

                }

                this.egxNavigator1.DataSource = GridBindingSource;
                if (egxNavigator1.StartupID >= 0)
                {
                    GridBindingSource.Clear();
                    GridBindingSource.DataSource = egxNavigator1.Find(egxNavigator1.FindBy);
                }
            }
            base.OnShown(e);
        }

        private void EgxEntryForm_LookupClosingEvent(object sender)
        {
            BusinessField.Clear();
            var o = GridBindingSource.Current;
            _preActionObject = GridBindingSource.Current;
            OnPreAction(new PreActionEventArgs() { ActionType = FormAction.Update, CurrentObject = _preActionObject });

            ((EgxTextLookup)sender).DataBindings.Clear();
            if (FormMode != FormAction.Insert)
            {
                FormMode = FormAction.Update;
                save.Enabled = true;
            }
        }





        public void StartUpdate() 
        {
            _preActionObject = GridBindingSource.Current;
            FormMode = FormAction.Update;
            save.Enabled = true;
        }
        //Used to Clear binding from EgxComboBox and prepare it to Update Process.
        private void EgxEntryForm_DropDown(object sender, EventArgs e)
        {
            _preActionObject = GridBindingSource.Current;
            ((EgxComboBox)sender).DataBindings.Clear();
            if (FormMode != FormAction.Insert)
            {
                FormMode = FormAction.Update;
                save.Enabled = true;
            }
        }


        //Used to Clear binding from EgxCheckBox and prepare it to Update Process.
        private void c_Click(object sender, EventArgs e)
        {
            
            _preActionObject = GridBindingSource.Current;
            ((EgxCheckBox)sender).DataBindings.Clear();
            if (FormMode != FormAction.Insert)
            {
                FormMode = FormAction.Update;
                save.Enabled = true;
            }
        }


        //Used to Clear binding from controls and prepare them to Update Process.
        private void c_KeyPress(object sender, EventArgs e)
        {
            _preActionObject = GridBindingSource.Current;
            foreach (Control c in GetFormControles())
            {
                c.DataBindings.Clear();
            }

            if (FormMode != FormAction.Insert)
            {
                FormMode = FormAction.Update;
                save.Enabled = true;
            }
        }


        /// <summary>
        /// Get properties values from Object o to set Form Controls with it.
        /// </summary>
        /// <param name="o">The Object contains the data</param>
        //public void SetFormData(object o)
        //{
        //    string CntrlName = null;
        //    foreach (Control c in GetFormControles())
        //    {
        //        CntrlName = c.GetType().Name;

        //        switch (CntrlName)
        //        {

        //            //case "EgxComboBox":
        //            //    EgxControl.EgxComboBox cb = c as EgxControl.EgxComboBox;
        //            //    cb.Text = o.GetType().GetProperty(cb.Name).GetValue(o).ToString();
        //            //    break;

        //            case "EgxTextBox":
        //                EgxControl.EgxTextBox tb = c as EgxControl.EgxTextBox;
        //                if (o.GetType().GetProperty(tb.Name).GetValue(o,null) != null)
        //                {
        //                    tb.Text = o.GetType().GetProperty(tb.Name).GetValue(o, null).ToString();
        //                }
        //                else { tb.Text = ""; }
        //                break;
        //            case "EgxDateText":
        //                EgxControl.EgxDateText dtb = c as EgxControl.EgxDateText;
        //                if (o.GetType().GetProperty(dtb.Name).GetValue(o, null) != null)
        //                {
        //                    dtb.Text = o.GetType().GetProperty(dtb.Name).GetValue(o, null).ToString();
        //                }
        //                else { dtb.Text = ""; }
        //                break;
        //            case "EgxLookup":
        //                EgxControl.EgxLookup lu = c as EgxControl.EgxLookup;
        //                if (lu.Value != null)
        //                {
        //                    lu.Text = "";
        //                }
        //                lu.ID = (int)o.GetType().GetProperty(lu.Name).GetValue(o, null);

        //                break;
        //            case "EgxCheckBox":
        //                EgxControl.EgxCheckBox ck = c as EgxControl.EgxCheckBox;
        //                if (o.GetType().GetProperty(ck.Name).GetValue(o, null) != null)
        //                {
        //                    if (((bool)o.GetType().GetProperty(ck.Name).GetValue(o, null)))
        //                    {
        //                        ck.Checked = true;
        //                    }
        //                    else { ck.Checked = false; }
        //                }
        //                else { ck.Checked = false; }
        //                break;
        //            case "EgxTextLookup":
        //                EgxControl.EgxTextLookup tlu = c as EgxControl.EgxTextLookup;
        //                if (tlu.Value != null)
        //                {
        //                    tlu.Text = "";
        //                }
        //                string val = o.GetType().GetProperty(tlu.Name).GetValue(o, null).ToString();
        //                if (val != null && val.Trim().Length > 0)
        //                {
        //                    tlu.Text = EgxLookupForm.GetDisplayValue(tlu.LookupID, val);
        //                }
        //                else
        //                {
        //                    tlu.Text = "";
        //                }
                        
        //                break;
        //        }
        //    }
        //}


        /// <summary>
        /// It returns by List<Controls> used in Control Panel and Equivalent to Specific Property in BLL.
        /// </summary>
        /// <returns></returns>
        public List<Control> GetFormControles()
        {

            List<Control> ReqProps = new List<Control>();
            Control[] cn = null;
            foreach (PropertyInfo p in BusinessObject.GetProperties())
            {
                cn = CntlPanel.Controls.Find(p.Name, true);
                if (cn.Length != 0)
                {
                    ReqProps.Add(cn[0]);
                }
                // else if (cn.GetType().Name == "EgxCheckBox") { ReqProps.Add(cn[0]); }
            }
            return ReqProps;
        }

        /// <summary>
        /// It used to set BusinessField[Name] with specific object value.
        /// </summary>
        /// <param name="o">The source object used to set BusinessField</param>
        public void SetBusinessFieldValue(object o)
        {
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                BusinessField[p.Name] = p.GetValue(o, null);
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
        /// <summary>
        /// Refresh Controls Binding and Move to Specific Object According to FormMode
        /// </summary>
        public void RefreshFormData()
        {
            if (egxNavigator1.IsEmpty) { save.Enabled = false; Delete.Enabled = false; }
            else
            {
                if (GridBindingSource != null)
                {
                    if (egxNavigator1.StartupID < 0)
                    {
                        if (FormMode == FormAction.Insert)
                        {
                            GridBindingSource.Clear();
                            GridBindingSource.DataSource = egxNavigator1.Last();
                        }
                        else if (FormMode == FormAction.Delete)
                        {


                        }
                      
                        else
                        {
                            GridBindingSource.Clear();
                            GridBindingSource.DataSource = egxNavigator1.First();
                        }
                    }
                    FormMode = FormAction.Select;
                }
                ResetBinding();
            }
            if (egxNavigator1.IsSingle)
            {
                Delete.Enabled = true;
                egxNavigator1.EmptyAction();
            }
        }
        /// <summary>
        /// Set GridBindingSource to Curser in which Key Value is equivalent to one of BindingSource Items.
        /// It Used in case of GridBindingSource Contains List<object>.
        /// </summary>
        /// <param name="k">ID Key</param>
        public void search(string k)
        {
            if (GridBindingSource.DataSource != null)
            {
                var current_obj = GridBindingSource.Current;
                int cnt = GridBindingSource.Count;
                int pre_search_position = GridBindingSource.Position;
                GridBindingSource.MoveFirst();
                Type type = current_obj.GetType();
                string val = "";
                for (int i = 0; i < cnt; ++i)
                {
                    current_obj = GridBindingSource.Current;
                    val = type.GetProperty("ID").GetValue(current_obj, null).ToString();
                    if (val == k)
                    {
                        GridBindingSource.Position = i;
                        break;
                    }
                    GridBindingSource.MoveNext();
                }

                int result_position = GridBindingSource.Position;
                if (val != k) { GridBindingSource.Position = pre_search_position; }
            }
        }
        #region Custom Actions Before or After Data Manipulation process
        /// <summary>
        /// You can override this method and make any action you need before inserting , updating or deleting processess
        /// </summary>
        public virtual void OnPreAction(PreActionEventArgs args) 
        {
            
            if (PreActionEvent != null) 
            {
                PreActionEvent(args);
            }
        }


        /// <summary>
        /// You can override this method to make required validations
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }


        /// <summary>
        /// You can override this method and make any action you need After inserting , updating or deleting processess
        /// </summary>
        public virtual void OnActionFinished(AfterActionEventArgs args) 
        {
            if (AfterActionEvent != null) 
            {
                AfterActionEvent(args);
            }
        }


        #endregion

        #region Functions Buttons
        //Delete Button
        private void Delete_Click(object sender, System.EventArgs e)
        {
            if (egxNavigator1.IsSingle || egxNavigator1.IsEmpty)
            {
                MessageBox.Show("يجب ان يكون لديك على الاقل عنصر واحد", "تنبيه");
            }
            else
            {
                PrepareDeleteCommand();
            }

        }
        //Save Button
        private void save_Click(object sender, System.EventArgs e)
        {
            FormAction ac;
            SystemMessage sm ;
            try
            {
                if (Validate())
                {
                    if (ShowFormGrid) { ErrList.Hide(); } else { panel1.Hide(); ErrList.Hide(); }
                    if (FormMode == FormAction.Insert)
                    {
                        
                    sm=    PrepareInsertCommand();
                    ac = FormAction.Insert;
                        RefreshFormData();
                       // GridBindingSource.MoveLast();

                    }
                    else
                    {
                        ac = FormAction.Update;
                      sm=  PrepareUpdateCommand();
                        ResetBinding();
                    }

                    addNew.Enabled = true;
                    save.Enabled = false;
                    OnActionFinished(new AfterActionEventArgs() {  ActionType= ac, Message = "OK" , CurrentObject=GridBindingSource.Current, SystemMessage=sm });
                }
                else
                {
                    // panel1.Show();
                    ErrList.DataSource = Err;
                    ErrList.Refresh();
                    // ErrList.Show();
                    if (ShowFormGrid) { ErrList.Show(); } else { panel1.Show(); ErrList.Show(); }

                }
            }
            catch (Exception exc) { }
        }

        //Add New Button
        public void BeginAddNew(object sender) 
        {
            addNew_Click(sender, null);
        }
        private void addNew_Click(object sender, System.EventArgs e)
        {
            OnPreAction(new PreActionEventArgs() { ActionType= FormAction.AddNew });
            foreach (Control c in GetFormControles())
            {
                if (c is EgxDateText)
                    c.Text = DateTime.Now.ToShortDateString();
                else if (c is EgxCheckBox)
                    ((EgxCheckBox)c).Checked = false;
                else if (c is EgxComboBox)
                    ((EgxComboBox)c).SelectedItem = null;
                else
                    c.Text = "";
            }
            DisableBinding();
            FormMode = FormAction.Insert;
            
            save.Enabled = true;
        }


        //Refresh Button
        private void egxClick1_Click(object sender, EventArgs e)
        {   
            this.FormMode = FormAction.Other;
            ResetBinding();
            addNew.Enabled = true;
            save.Enabled = false;
            if (ShowFormGrid) { ErrList.Hide(); } else { panel1.Hide(); ErrList.Hide(); }
        }
        #endregion

    }

    public class Error
    {
        public string ErrorText { get; set; }
    }
}
