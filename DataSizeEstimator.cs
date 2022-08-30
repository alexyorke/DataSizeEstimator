using System.Reflection;
using DataSizeEstimator.Handlers;

namespace DataSizeEstimator;

public class DataSizeEstimator
{

    public dynamic GenerateDataForProperty<T>(T @class, string propToGenerateDataFor,
        Dictionary<Type, ITypeAttributeHandler> handles)
    {
        if (@class != null)
        {
            var attrData = @class.GetAttributesFrom(propToGenerateDataFor);
            var items = HandleAttributesForProperty(handles, attrData, @class.GetPropertyType(propToGenerateDataFor));

            IConcatenableType target = items.First<IConcatenableType>();
            for (int i = 1; i < items.Count - 1; i++)
            {
                target = items[i].Concat(target);
            }

            return target.GetValue();
        }

        throw new InvalidDataException(nameof(@class) + " cannot be null");
    }

    private static List<IConcatenableType> HandleAttributesForProperty(IReadOnlyDictionary<Type, ITypeAttributeHandler> handles, IList<CustomAttributeData> attrData, Type t)
    {
        if (t.IsGenericType && !handles.ContainsKey(t))
        {
            Type wrapperType = t.GetGenericTypeDefinition();
            var innerTypes = t.GenericTypeArguments;

            List<IConcatenableType> results = new List<IConcatenableType>();

            foreach (var innerType in innerTypes)
            {
                results.AddRange(HandleAttributesForProperty(handles, attrData, innerType));
            }

            results.Add(handles[wrapperType].HandleTypeAttributes(attrData));
            return results;
        }

        return new List<IConcatenableType> { handles[t].HandleTypeAttributes(attrData) };
    }
}