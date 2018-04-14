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
public class Triangle : Figure
{
    [DataMember]
    public int xPosition, yPosition, xPosition2, yPosition2, xPosition3, yPosition3;

    public Triangle() { }

    public Triangle(int xPosition1, int yPosition1, int xPosition2, int yPosition2, int xPosition3, int yPosition3)
    {
        this.xPosition = xPosition1;
        this.yPosition = yPosition1;
        this.xPosition2 = xPosition2;
        this.yPosition2 = yPosition2;
        this.xPosition3 = xPosition3;
        this.yPosition3 = yPosition3;
    }

    public override GraphicsPath GetPath()
    {
        Point point1 = new Point(xPosition, yPosition);
        Point point2 = new Point(xPosition2, yPosition2);
        Point point3 = new Point(xPosition3, yPosition3);

        Point[] points = { point1, point2, point3 };

        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(points);

        return path;
    }

    public override void SetManualParameters(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
        xPosition2 = values[2];
        yPosition2 = values[3];
        xPosition3 = values[4];
        yPosition3 = values[5];
    }
}
