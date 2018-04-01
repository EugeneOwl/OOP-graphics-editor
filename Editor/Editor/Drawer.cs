using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Editor
{
    class Drawer
    {
        public void DrawFigureByPath(PaintEventArgs e, GraphicsPath path)
        {
            Pen myPen = new Pen(Color.Green, 1);
            e.Graphics.DrawPath(myPen, path);
        }
    }
}
