using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxControl;
using Egx.EgxDataAccess;
using System.Xml;
namespace Egx.EgxControl
{
    public partial class DBConnection : EgxForm
    {
        public DBConnection()
        {
            InitializeComponent();
            egxComboBox1.DataSource = Enum.GetValues(typeof(EgxDataType));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DataAccess da = new DataAccess();
            DataAccess.ExecData("INSERT INTO [cmp](ServerName,DBName,Username,pwd,DBType,CompanyName,CompanyAddress,Phone) VALUES('" + egxTextBox1.Text + "','" + egxComboBox2.Text + "','" + Username.Text + "','" + pwd.Text + "','" + egxComboBox1.SelectedValue + "','" + cmpName.Text + "','" + cmpAddr.Text + "','" + cmpPhone.Text + "')", EgxDataType.Temp);
            DBConfig.ServerName = egxTextBox1.Text;
            DBConfig.DbName = egxComboBox2.Text;
            DBConfig.UserName = Username.Text;
            DBConfig.Password = pwd.Text;
            DBConfig.Datatype = (EgxDataType)egxComboBox1.SelectedValue;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Successful Connection", "Database Configurations Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Database Configurations Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {

            }
        }

        private void DBConnection_Load(object sender, EventArgs e)
        {

        }

        private void egxTextBox1_Leave(object sender, EventArgs e)
        {
          
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private void egxTextBox1_Leave_1(object sender, EventArgs e)
        {
            try
            {
                if (egxTextBox1.Text.Trim().Length > 0)
                {
                    DataTable dt = (DataTable)new DataAccess().ExecuteCustomConnection("server=" + egxTextBox1.Text + ";Integrated Security = sspi", "SELECT Name FROM master.sys.databases", true);
                    egxComboBox2.DataSource = dt;
                    egxComboBox2.DisplayMember = "Name";
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Database Configurations Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
    }
}
