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
public class ExternalRectangleFactory: ExternalFigureFactory
{
    [DataMember]
    public int x, y, horizon, vertical;

    public ExternalRectangleFactory() { }

    public ExternalRectangleFactory(int x, int y, int horizon, int vertical)
    {
        this.x = x;
        this.y = y;
        this.horizon = horizon;
        this.vertical = vertical;
    }

    public override GraphicsPath GetPathFactoryExternalWay()
    {
        Point point1 = new Point(x, y);
        Point point2 = new Point(x + horizon, y);
        Point point3 = new Point(x + horizon, y + vertical);
        Point point4 = new Point(x, y + vertical);

        Point[] points = { point1, point2, point3, point4 };

        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(points);

        return path;
    }

    public override void SetManualParametersFactoryExternalWay(int[] values)
    {
        x = values[0];
        y = values[1];
        horizon = values[2] - values[0];
        vertical = values[3] - values[1];
    }

    public override int[] GetParameters()
    {
        int[] parameters = { x, y, horizon, vertical};
        return parameters;
    }
}
