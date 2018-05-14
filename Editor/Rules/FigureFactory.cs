using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Xml.Serialization;

public abstract class FigureFactory
{
    abstract public GraphicsPath GetPathFactory();
    abstract public void SetManualParametersFactory(int[] values);
}
