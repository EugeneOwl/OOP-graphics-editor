using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

public class Square : Figure
{
    private int xPosition, yPosition, sideSize;

    public Square() { }

    public Square(int xPosition, int yPosition, int sideSize)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
        this.sideSize = sideSize;
    }

    public override GraphicsPath GetPath()
    {
        Point point1 = new Point(xPosition, yPosition);
        Point point2 = new Point(xPosition + sideSize, yPosition);
        Point point3 = new Point(xPosition + sideSize, yPosition + sideSize);
        Point point4 = new Point(xPosition, yPosition + sideSize);

        Point[] points = { point1, point2, point3, point4 };

        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(points);

        return path;
    }

    public override void SetManualParameters(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
        int widthSqr = (values[2] - values[0]) * (values[2] - values[0]);
        int heightSqr = ((values[3] - values[1]) * (values[3] - values[1]));
        sideSize = (int)Math.Round(Math.Sqrt((widthSqr + heightSqr) / 2), 0);
        if (values[2] - values[0] < 0)
        {
            sideSize *= -1;
        }
    }
}

