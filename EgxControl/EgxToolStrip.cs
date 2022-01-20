using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public class EgxToolStrip : ToolStrip
    {
        protected override void OnCreateControl()
        {
            Renderer = new EgxCustomToolStripProfessionalRender();
            base.OnCreateControl();
        }
    }
    public class EgxCustomToolStripProfessionalRender : ToolStripProfessionalRenderer 
    {
        public EgxCustomToolStripProfessionalRender() 
        {
            this.RoundedEdges = false;
         
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
        }
    }
}
