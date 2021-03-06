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
public class LineSegmentFactory: FigureFactory
{
    [DataMember]
    public int xPosition, yPosition, xPosition2, yPosition2;

    public LineSegmentFactory() { }

    public LineSegmentFactory(int xPosition1, int yPosition1, int xPosition2, int yPosition2)
    {
        this.xPosition = xPosition1;
        this.yPosition = yPosition1;
        this.xPosition2 = xPosition2;
        this.yPosition2 = yPosition2;
    }

    public override GraphicsPath GetPathFactory()
    {
        Point point1 = new Point(xPosition, yPosition);
        Point point2 = new Point(xPosition2, yPosition2);

        GraphicsPath path = new GraphicsPath();
        path.AddLine(point1, point2);

        return path;
    }

    public override void SetManualParametersFactory(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
        xPosition2 = values[2];
        yPosition2 = values[3];
    }
}
