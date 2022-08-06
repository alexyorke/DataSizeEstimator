using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataSizeEstimator;

internal class GenericListHandler : ITypeAttributeHandler
{
    public IConcatenableType HandleTypeAttributes(IList<CustomAttributeData> attributes)
    {
        int max = 0;
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeType == typeof(MaxLengthAttribute))
            {
                max = (int)(attribute.ConstructorArguments.Select(x => x.Value).First() ?? throw new NullReferenceException());
            }
        }

        return new ListConcatenatable<string> { val = new List<string>(), max = max };
    }
}