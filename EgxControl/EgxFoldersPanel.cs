using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Egx.EgxControl
{
    public class EgxFoldersPanel :FlowLayoutPanel
    {
        private XmlDocument doc;
        public string XmlFile { get; set; }
        public string DefaultContent { get; set; }
        public Type ProjectType { get; set; }
        public EgxFoldersPanel() { }
        public EgxFoldersPanel(string xmlFilePath,Type type) 
        {
            this.XmlFile = xmlFilePath;
            this.ProjectType = type;
        }
        public EgxFoldersPanel(string xmlFilePath, string defaultContent,Type type) 
        {
            XmlFile = xmlFilePath;
            this.ProjectType = type;
            setContent(defaultContent);
        }


        public void setContent(string ContentName)
        {
             doc = new XmlDocument();
            // Environment.CurrentDirectory + "\\EgxSystemConfig.xml"
            string dir =XmlFile;
            doc.Load(dir);
            string xpath = "//Page[@name='" + ContentName + "']";
            foreach (XmlNode node in doc.DocumentElement.SelectNodes(xpath))
            {
                foreach (XmlNode folder in node.ChildNodes)
                {
                    CreateContent(folder);
                }
            }
        }

        private void CreateContent(XmlNode node)
        {
            Form f = null;
            Button b = new Button();
            b.Text = node.InnerText;
            b.Height = 90;
            b.Width = 90;
            b.FlatStyle = FlatStyle.Flat;
            b.TextImageRelation = TextImageRelation.ImageAboveText;
            ResourceManager rm = new ResourceManager(ProjectType);
            b.Image =(Bitmap)rm.GetObject(node.Attributes[1].Value);
            b.Click +=
                (p, i) =>
                {
                    f = TryGetFormByName(node.Attributes[0].Value);
                    f.Show();

                };
            this.Controls.Add(b);
        }

        public Form TryGetFormByName(string frmname)
        {
            var formType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.BaseType == typeof(Form) && a.Name == frmname)
                .FirstOrDefault();

            if (formType == null) // If there is no form with the given frmname
                return null;

            return (Form)Activator.CreateInstance(formType);
        }
    }
}
