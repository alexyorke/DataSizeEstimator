using System.Reflection;

namespace DataSizeEstimator;

public static class Extensions
{
    public static IList<CustomAttributeData> GetAttributesFrom(this object instance, string propertyName)
    {
        var property = instance.GetType().GetProperty(propertyName);
        if (property == null) return null;
        return property.GetCustomAttributesData();
    }

    public static Type GetPropertyType(this object instance, string propertyName)
    {
        var property = instance.GetType().GetProperty(propertyName);
        return property?.PropertyType ?? throw new NullReferenceException(nameof(propertyName));
    }
}