using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

[DataContract]
[Serializable]
public class Rectangle : Figure
{
    [DataMember]
    public int xPosition, yPosition, horizon, vertical;

    public Rectangle() { }

    public Rectangle(int xPosition1, int yPosition1, int horizon, int vertical)
    {
        this.xPosition = xPosition1;
        this.yPosition = yPosition1;
        this.horizon = horizon;
        this.vertical = vertical;
    }

    public override GraphicsPath GetPath()
    {
        Point point1 = new Point(xPosition, yPosition);
        Point point2 = new Point(xPosition + horizon, yPosition);
        Point point3 = new Point(xPosition + horizon, yPosition + vertical);
        Point point4 = new Point(xPosition, yPosition + vertical);

        Point[] points = { point1, point2, point3, point4 };

        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(points);

        return path;
    }

    public override void SetManualParameters(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
        horizon = values[2] - values[0];
        vertical = values[3] - values[1];
    }
}
