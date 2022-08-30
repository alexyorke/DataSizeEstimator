using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataSizeEstimator.Handlers;

public class StringHandler : ITypeAttributeHandler
{
    public IConcatenableType HandleTypeAttributes(IList<CustomAttributeData> attributes)
    {
        var min = 0;
        var max = 0;
        foreach (var attribute in attributes)
        {
            if (attribute.AttributeType == typeof(MaxLengthAttribute))
            {
                max = (int)(attribute.ConstructorArguments.Select(x => x.Value).FirstOrDefault() ??
                            throw new NullReferenceException("MaxLengthAttribute must have an argument"));
                if (max == -1) max = int.MaxValue;
                return new StringConcatenatable(min, max);
            }

            if (attribute.AttributeType == typeof(CreditCardAttribute))
            {
                return new CreditCardConcatenable();
            }

            throw new NotImplementedException(attribute.AttributeType + " has no handler");
        }

        // default handler if no attributes are specified
        return new StringConcatenatable(min, max);
    }
}