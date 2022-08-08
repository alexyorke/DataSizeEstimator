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
                max = (int)(attribute.ConstructorArguments.Select(x => x.Value).FirstOrDefault() ?? throw new NullReferenceException("MaxLengthAttribute must have an argument"));
                if (max == -1) max = Int32.MaxValue;
            }
            else
            {
                throw new NotImplementedException(attribute.AttributeType + " has no handler");
            }
        }
        return new StringConcatenatable(min, max);
    }
}