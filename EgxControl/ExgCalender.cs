using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Egx.EgxControl
{
    public partial class EgxCalender : Form
    {
        private MonthCalendar _calendar;
        private TextBox _textBox;
        public EgxCalender(TextBox text)
        {
            InitializeComponent();
            _textBox = text;
            DateTime d;
            if (_textBox.Text != "" && DateTime.TryParse(_textBox.Text, out d))
            {
                Calendar.SelectionStart = DateTime.Parse(_textBox.Text);
                Calendar.SelectionEnd = DateTime.Parse(_textBox.Text);
            }
        }
        
        public MonthCalendar Calendar
        {
            get { return monthCalendar1; }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            _textBox.Text= e.End.ToShortDateString();
            this.Close();
        }
    }
}
