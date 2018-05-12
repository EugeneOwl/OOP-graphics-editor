using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

[DataContract]
[Serializable]
public class CircleFactory: FigureFactory
{
    [DataMember]
    public int radius, xPosition, yPosition;

    public CircleFactory() { }

    public CircleFactory(int radius, int xPosition, int yPosition)
    {
        this.radius = radius;
        this.xPosition = xPosition;
        this.yPosition = yPosition;
    }

    public override GraphicsPath GetPathFactory()
    {
        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(xPosition, yPosition, radius, radius);

        return path;
    }

    public override void SetManualParametersFactory(int[] values)
    {
        xPosition = values[0];
        yPosition = values[1];
        int widthSqr = (values[2] - values[0]) * (values[2] - values[0]);
        int heightSqr = ((values[3] - values[1]) * (values[3] - values[1]));
        radius = (int)Math.Round(Math.Sqrt(widthSqr + heightSqr), 0);
        if (values[2] - values[0] < 0)
        {
            radius *= -1;
        }
    }
}
