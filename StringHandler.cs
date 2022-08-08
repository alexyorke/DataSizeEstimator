using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataSizeEstimator;

class StringHandler : ITypeAttributeHandler
{
    public IConcatenableType HandleTypeAttributes(IList<CustomAttributeData> attributes)
    {
        var min = 0;
        var max = 0;
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeType == typeof(MaxLengthAttribute))
            {
                max = (int)(attribute.ConstructorArguments.Select(x => x.Value).First() ?? throw new NullReferenceException());
            }
        }
        return new StringConcatenatable(min, max);
    }
}