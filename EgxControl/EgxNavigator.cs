using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxDataAccess;
namespace Egx.EgxControl
{
    public partial class EgxNavigator : FlowLayoutPanel
    {
        DataAccess da;
        public object CurrentObject { get; set; }
        private int cindex = 0;
        public int StartupID { get; set; }
        public int CurrentIndex
        {

            get { return cindex; }
            set
            {
                cindex = value;
                da.CurrentIndex = value;
            }

        }
        public bool IsEmpty { get { return da.IsEmpty(CurrentObject); } }
        public bool IsSingle { get { return da.IsSingle(CurrentObject); } }
        public object NavObject { get; set; }
        [DefaultValue("")]
        public string NavigationKey { get; set; }
        public string PKName { get; set; }
        [DefaultValue(50)]
        public int TextWidth { get; set; }
        public BindingSource DataSource { get; set; }
        private EgxClick btnNext, btnPrev, btnFirst, btnLast, btnSearch;
        public EgxTextBox txtFind;
        private EgxLookupForm luForm;
        [DefaultValue("")]
        public string LookupQuery { get; set; }
        public string LookupValueMember { get; set; }
        public string LookupID { get; set; }
        public string FindBy { get; set; }
        [DefaultValue("")]
        public string NavigationCondition { get; set; }
        public delegate void Navigate(EgxNavigatorEventArgs args);
        public event Navigate NavigateEvent;
        public void onNavigate(EgxNavigatorEventArgs args) 
        {
            if (NavigateEvent != null) 
            {
                NavigateEvent(args);
            }
        }
        public EgxNavigator()
        {
            InitializeComponent();
            StartupID = -1;
            da = new DataAccess();
            da.NavigationObject = NavObject;
         //   if (NavigationCondition.Trim().Length > 0) { da.NavigationCondition = this.NavigationCondition; }
            DataSource = new BindingSource();
            //Navigation Buttons Declaration
            btnNext = new EgxClick() { MouseMoveIcon = global::Egx.Properties.Resources.next_blue, MouseLeaveIcon = global::Egx.Properties.Resources.next_white };
            btnFirst = new EgxClick() { MouseMoveIcon = global::Egx.Properties.Resources.first_blue, MouseLeaveIcon = global::Egx.Properties.Resources.first_white };
            btnPrev = new EgxClick() { MouseMoveIcon = global::Egx.Properties.Resources.prev_blue, MouseLeaveIcon = global::Egx.Properties.Resources.prev_white };
            btnLast = new EgxClick() { MouseMoveIcon = global::Egx.Properties.Resources.last_blue, MouseLeaveIcon = global::Egx.Properties.Resources.last_white };
            btnSearch = new EgxClick() { MouseMoveIcon = global::Egx.Properties.Resources.find_orange, MouseLeaveIcon = global::Egx.Properties.Resources.find_black };
            txtFind = new EgxTextBox() { Width = 50, Multiline = true, Height = btnSearch.Height + 2, Visible = false };
            //Adding Navigation Buttons To Panel
            this.Controls.Add(btnFirst);
            this.Controls.Add(btnPrev);
            this.Controls.Add(btnNext);
            this.Controls.Add(btnLast);
            this.Controls.Add(txtFind);
            this.Controls.Add(btnSearch);
            //Declare Buttons Event Handlers
            btnNext.Click += btnNext_Click;
            btnPrev.Click += btnPrev_Click;
            btnFirst.Click += btnFirst_Click;
            btnLast.Click += btnLast_Click;
            btnSearch.Click += btnSearch_Click;
            txtFind.KeyDown += txtFind_KeyDown;
            txtFind.MouseDoubleClick += txtFind_MouseDoubleClick;

            this.AutoSize = true;
            LookupID = "";
            LookupValueMember = "";
            LookupQuery = "";
            NavigationCondition = "";
        }

        private void luForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (luForm.Value != null)
            {
                txtFind.Text = luForm.Value.ToString();
                if (txtFind.Text.Trim().Length > 0)
                {
                    txtFind_KeyDown(this, new KeyEventArgs(Keys.Enter));
                }
            }
        }



        private void txtFind_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            luForm = new EgxLookupForm();
            luForm.FormClosing += luForm_FormClosing;
            if (LookupQuery.Length > 0 && LookupValueMember.Length>0)
            {
                luForm.SqlQuery = LookupQuery;
                luForm.ValueMember = LookupValueMember;
                luForm.ShowDialog();
            }
            else if (LookupID.Trim().Length > 0) 
            {
                luForm.LookupID = LookupID;
              
                luForm.ShowDialog();

            }
            
        }

        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtFind.Text.Length > 0 )
                {
                    DataSource.Clear();
                    DataSource.DataSource = Find(FindBy);
                    onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

                }
                else { MessageBox.Show("تاكد من المدخلات"); txtFind.Text = ""; }
            }
        }
        public void GoTo() 
        {
                DataSource.Clear();
                DataSource.DataSource = Find(FindBy);
                onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtFind.Visible) { txtFind.Visible = false; } else { txtFind.Visible = true; }
            txtFind.Text = "";
        }
        /// <summary>
        /// It initialize the required object to navigate over it 
        /// </summary>
        /// <param name="navigationObject">Represent object to navigate over it </param>
        public void SetupObject(object navigationObject)
        {
            DataSource = new BindingSource();
            CurrentObject = navigationObject;
            if (IsEmpty || IsSingle)
            { EmptyAction(); }
            else
            {
                if (DataSource != null)
                {
                    DataSource.Clear();
                }

                CurrentObject = First();
                DataSource.DataSource = CurrentObject;
                CurrentIndex = da.GetFirstIndex(PKName);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            DataSource.Clear();
            DataSource.DataSource = Last();
            onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            DataSource.Clear();
            DataSource.DataSource = First();
            onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            DataSource.Clear();
            DataSource.DataSource = Previous();
            onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DataSource.Clear();
            DataSource.DataSource = Next();
            onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public bool IsBeforeLast()
        {
            da.NavigationObject = CurrentObject;
            if (da.IsBeforeLast(PKName)) { return true; } else { return false; }
        }

        public bool ISLast()
        {
            da.NavigationObject = CurrentObject;
            if (CurrentIndex == da.GetLastIndex(PKName)) { return true; } else { return false; }
        }

        public bool IsFirst()
        {
            da.NavigationObject = CurrentObject;
            if (CurrentIndex == da.GetFirstIndex(PKName)) { return true; } else { return false; }
        }

        public bool IsAfterFirst()
        {
            da.NavigationObject = CurrentObject;
            if (da.IsAfterFirst(PKName)) { return true; } else { return false; }
        }
        public object Next()
        {
            if (IsSingle || IsEmpty) { EmptyAction(); }
            else
            {
                da.NavigationObject = CurrentObject;
                if (da.IsBeforeLast(PKName))
                {
                    LastAction();
                }
                else { NormalAction(); }
             //   if (NavigationCondition.Trim().Length > 0) { da.NavigationCondition = this.NavigationCondition; }
                CurrentObject = da.Move(EgxMoveType.Next, PKName);
                CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject, null);
            }
            return CurrentObject;
        }
        public object Find(string FindBy)
        {
            object tmp = CurrentObject;
            CurrentObject=da.GetByColumn(this.FindBy, StartupID >= 0 ? StartupID.ToString() : txtFind.Text, CurrentObject);
          /////////////////////// // CurrentObject = da.GetByID(StartupID>=0?StartupID:Int32.Parse(txtFind.Text), CurrentObject);
            if (CurrentObject != null)
            {
                da.NavigationObject = CurrentObject;
                da.CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject, null);

            }
            else
            {
                CurrentObject = tmp;
                da.NavigationObject = CurrentObject;
                da.CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject, null);
            }

            if (da.IsLast(PKName))
            {
                LastAction();
            }
            else { NormalAction(); }
            if (IsSingle || IsEmpty) { EmptyAction(); }
            return CurrentObject;
        }

        public object Find(string FindBy,object value)
        {
            object tmp = CurrentObject;
            CurrentObject = da.GetByColumn(this.FindBy, value, CurrentObject);
            /////////////////////// // CurrentObject = da.GetByID(StartupID>=0?StartupID:Int32.Parse(txtFind.Text), CurrentObject);
            if (CurrentObject != null)
            {
                da.NavigationObject = CurrentObject;
                da.CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject, null);

            }
            else
            {
                CurrentObject = tmp;
                da.NavigationObject = CurrentObject;
                da.CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject, null);
            }

            if (da.IsLast(PKName))
            {
                LastAction();
            }
            else { NormalAction(); }
            if (IsSingle || IsEmpty) { EmptyAction(); }
            return CurrentObject;
        }
       
        public object Previous()
        {
            if (IsSingle || IsEmpty) { EmptyAction(); }
            else
            {
                da.NavigationObject = CurrentObject;
                if (da.IsAfterFirst(PKName))
                {
                    FirstAction();
                }
                else { NormalAction(); }
             //   if (NavigationCondition.Trim().Length > 0) { da.NavigationCondition = this.NavigationCondition; }
                CurrentObject = da.Move(EgxMoveType.Prev, PKName);
                CurrentIndex = (int)CurrentObject.GetType().GetProperty(PKName).GetValue(CurrentObject,null);
            }
            return CurrentObject;
        }
        public object First()
        {
            if (IsSingle || IsEmpty) { EmptyAction(); }
            else
            {
                da.NavigationObject = CurrentObject;
              //  if (NavigationCondition.Trim().Length > 0) { da.NavigationCondition = this.NavigationCondition; }
                CurrentObject = da.Move(EgxMoveType.First, PKName);
                CurrentIndex = da.GetFirstIndex(PKName);
                FirstAction();
                onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

            }
            return CurrentObject;
        }
        public object Last()
        {
            //if (IsSingle || IsEmpty) { EmptyAction(); }
            //else
            //{
            da.NavigationObject = CurrentObject;
           // if (NavigationCondition.Trim().Length > 0) { da.NavigationCondition = this.NavigationCondition; }

            CurrentObject = da.Move(EgxMoveType.Last, PKName);
            CurrentIndex = da.GetLastIndex(PKName);
            onNavigate(new EgxNavigatorEventArgs() { CurrentIndex = this.CurrentIndex, CurrentObject = this.CurrentObject });

            LastAction();
            //}
            return CurrentObject;
        }

        public void LastAction()
        {
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
        }
        public void FirstAction()
        {
            btnFirst.Enabled = false;
            btnPrev.Enabled = false;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
        }
        public void NormalAction()
        {
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
        }
        public void EmptyAction()
        {
            btnFirst.Enabled = false;
            btnPrev.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
    }
}
