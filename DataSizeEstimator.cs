using System.Reflection;
using System.Runtime.CompilerServices;
using DataSizeEstimator.Handlers;

namespace DataSizeEstimator;

public class DataSizeEstimator
{
    private Random rnd;

    public void GenerateDataForClass<T>(T @class, Dictionary<Type, ITypeAttributeHandler> handlers)
    {
        if (@class != null)
        {
            foreach (var prop in @class.GetType().GetProperties())
            {
                SetObjectProperty(prop.Name, GenerateDataForProperty<T>(@class, prop.Name, handlers), @class);
            }
        }
        else
        {
            throw new InvalidDataException(nameof(@class) + " cannot be null");
        }
    }

    private void SetObjectProperty(string propertyName, dynamic value, object obj)
    {
        PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
        // make sure object has the property we are after
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(obj, value, null);
        }
    }

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

    private List<IConcatenableType> HandleAttributesForProperty(IReadOnlyDictionary<Type, ITypeAttributeHandler> handles, IList<CustomAttributeData> attrData, Type t)
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

            results.Add(handles[wrapperType].HandleTypeAttributes(attrData, this.rnd));
            return results;
        }

        return new List<IConcatenableType> { handles[t].HandleTypeAttributes(attrData, this.rnd)};
    }

    public void SetRandomInstance(Random rnd)
    {
        this.rnd = rnd ?? Random.Shared;
    }
}