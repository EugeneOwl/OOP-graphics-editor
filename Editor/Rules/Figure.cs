using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.Serialization;

[assembly: AssemblyKeyFileAttribute("newOrigin.snk")]
[DataContract]
[Serializable]
public class Figure
{
    [DataMember]
    public FigureFactory undermaskFactory;

    public Figure() { }

    public Figure(FigureFactory factory)
    {
        undermaskFactory = factory;
    }

    public GraphicsPath GetPath()
    {
        return undermaskFactory.GetPathFactory();
    }

    public void SetManualParameters(int[] values)
    {
        undermaskFactory.SetManualParametersFactory(values);
    }
}

