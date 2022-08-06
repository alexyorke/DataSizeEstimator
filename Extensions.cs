using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataSizeEstimator;

public static class Extensions
{
    public static IEnumerable<string> NextStrings(
        this Random rnd,
        (int Min, int Max) length,
        int count = 1)
    {
        var allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        ISet<string> usedRandomStrings = new HashSet<string>();
        (int min, int max) = length;
        char[] chars = new char[max];
        int setLength = allowedChars.Length;

        while (count-- > 0)
        {
            int stringLength = rnd.Next(min, max + 1);

            for (int i = 0; i < stringLength; ++i)
            {
                chars[i] = allowedChars[rnd.Next(setLength)];
            }

            string randomString = new string(chars, 0, stringLength);

            if (usedRandomStrings.Add(randomString))
            {
                yield return randomString;
            }
            else
            {
                count++;
            }
        }
    }

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