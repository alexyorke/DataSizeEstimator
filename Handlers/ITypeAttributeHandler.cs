using System.Reflection;

namespace DataSizeEstimator.Handlers;

public interface ITypeAttributeHandler
{
    public IConcatenableType HandleTypeAttributes(IList<CustomAttributeData> attributes, Random rnd = null);
}