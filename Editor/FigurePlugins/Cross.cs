using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

[Serializable]
public class Cross : Figure
{
    public int xPosition, yPosition;

    public Cross() { }

    public Cross(int xPosition1, int yPosition1)
    {
        this.xPosition = xPosition1;
        this.yPosition = yPosition1;
    }

    public override GraphicsPath GetPath()
    {
        Point point1 = new Point(xPosition, yPosition);
        Point point2 = new Point(xPosition - 10, yPosition);
        Point point3 = new Point(xPosition + 10, yPosition);
        Point point4 = new Point(xPosition, yPosition);
        Point point5 = new Point(xPosition, yPosition - 10);
        Point point6 = new Point(xPosition, yPosition + 10);

        Point[] points = { point1, point2, point3, point4, point5, point6 };

        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(points);

        return path;
    }

    public override void SetManualParameters(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
    }
}
