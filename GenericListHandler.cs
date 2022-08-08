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
            else
            {
                throw new NotImplementedException(attribute.AttributeType + " has no handler");
            }
        }

        return new ListConcatenatable<string>(0, max);
    }
}