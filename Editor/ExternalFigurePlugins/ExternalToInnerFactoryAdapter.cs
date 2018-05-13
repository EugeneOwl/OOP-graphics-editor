using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

[DataContract]
[Serializable]
public class ExternalToInnerFactoryAdapter: FigureFactory
{
    [DataMember]
    public ExternalFigureFactory externalFigureFactory;

    public ExternalToInnerFactoryAdapter() { }

    public ExternalToInnerFactoryAdapter(ExternalFigureFactory externalFigureFactory)
    {
        this.externalFigureFactory = externalFigureFactory;
    }

    public override GraphicsPath GetPathFactory()
    {
        return externalFigureFactory.GetPathFactoryExternalWay();
    }

    public override void SetManualParametersFactory(int[] values)
    {
        externalFigureFactory.SetManualParametersFactoryExternalWay(values);
    }
}
