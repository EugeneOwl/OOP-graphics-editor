using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Reflection;

public abstract class ExternalFigureFactory
{
    abstract public GraphicsPath GetPathFactoryExternalWay();
    abstract public void SetManualParametersFactoryExternalWay(int[] values);
    abstract public int[] GetParameters();
}
