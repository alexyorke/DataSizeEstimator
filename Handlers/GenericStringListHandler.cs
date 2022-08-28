using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataSizeEstimator.Handlers;

internal class GenericStringListHandler : ITypeAttributeHandler
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

        return new ListConcatenatable<string>(0, max);
    }
}