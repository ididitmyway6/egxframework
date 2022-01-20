using Egx.EgxControl;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public partial class frmReports : EgxForm
    {
        PrintDocument p = new PrintDocument();
        public void Print() 
        {
           
        }
        public LocalReport Report { get; set; }
        public string ReportName { get; set; }
        public void SetReport(string reportName, string dataSourceName,BindingSource bindingSource,Dictionary<string,string> parameters=null) 
        {

            this.reportViewer1.LocalReport.ReportEmbeddedResource = "EgxSmartInventory.Reports." + reportName + ".rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource(dataSourceName,bindingSource));
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter(kvp.Key, kvp.Value));
                }
            }
        }
        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
