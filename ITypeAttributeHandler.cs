using System.Reflection;

namespace DataSizeEstimator;

interface ITypeAttributeHandler
{
    IConcatenableType HandleTypeAttributes(IList<CustomAttributeData> attributes);
}