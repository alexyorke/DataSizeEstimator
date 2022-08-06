using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DataSizeEstimator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            var obj = player.GetValidValueForProperty(nameof(player.Names));
            Console.WriteLine(JsonConvert.SerializeObject(obj));
        }
    }

    public class Player
    {
        [CreditCard]
        public string PlayerDescription { get; set; }

        [MaxLength(100)]
        [StringLength(2)]
        public List<List<string>> Names { get; set; }
    }

    public static class Extensions
    {
        public static dynamic GetValidValueForProperty<T>(this T prop, string type)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));

            // validate which ones can apply to this property

            // these are only applicable if the type is a string
            var maxLength = prop.GetAttributeFrom<MaxLengthAttribute>(type)?.Length;
            var minLength = prop.GetAttributeFrom<MinLengthAttribute>(type)?.Length;
            var stringMaxLength = prop.GetAttributeFrom<StringLengthAttribute>(type)?.MaximumLength;
            var isCreditCardAttribute = prop.GetAttributeFrom<CreditCardAttribute>(type) != null;

            // this one might have to be IComparable?
            var range = prop.GetAttributeFrom<RangeAttribute>(type);

            // can't be null or empty string potentially
            var required = prop.GetAttributeFrom<RequiredAttribute>(type)?.AllowEmptyStrings;

            var assemblyType = prop.GetPropertyType(type);
            return GenerateValueForProperty(assemblyType, minLength, maxLength, required, range, isCreditCardAttribute, stringMaxLength);
        }

        private static dynamic GenerateValueForProperty(Type? assemblyType, int? minLength, int? maxLength, bool? required, RangeAttribute? range, bool? isCreditCardAttribute, int? stringMaxLength)
        {
            if (assemblyType?.FullName == null) throw new InvalidDataException();

            if (assemblyType.FullName == "System.String")
            {
                return GenerateString(minLength, maxLength, required, isCreditCardAttribute);
            }
            else if (assemblyType.FullName.StartsWith("System.Collections.Generic.List`1"))
            {
                return GenerateList(minLength, maxLength, required, assemblyType.GetGenericType(), stringMaxLength);
            }
            else if (assemblyType.FullName == "System.Int32")
            {
                return GenerateInt((int?)range?.Minimum, (int?)range?.Maximum);
            }
            else
            {
                throw new InvalidOperationException("Not sure how to generate value for property for " +
                                                    assemblyType.FullName);
            }
        }

        private static int GenerateInt(int? min, int? max)
        {
            if (min != null && max != null)
            {
                return Random.Shared.Next((int)min, (int)max);
            }

            if (min == null && max != null)
            {
                return Random.Shared.Next(0, (int)max!);
            }

            return Random.Shared.Next();
        }

        private static List<dynamic> GenerateList(int? min, int? max, bool? required, Type? genericType, int? stringMaxLength)
        {
            const int DEFAULT_LENGTH = 10;
            var generatedListLength = 0;
            if (min == null && max == null && required == null)
                throw new InvalidDataException("No bounds on list; data could be infinite.");

            if (required != null && (bool)required && min == null) min = 1;

            if (min != null && max != null)
            {
                generatedListLength = Random.Shared.Next((int)min, (int)max);
            }
            else if (min == null && max != null)
            {
                generatedListLength = Random.Shared.Next(0, (int)max!);
            }
            else
            {
                generatedListLength = Random.Shared.Next((int)min!);
            }

            var toReturn = new List<dynamic>();
            for (int i = 0; i < generatedListLength; i++)
            {
                toReturn.Add(GenerateValueForProperty(genericType, 0, genericType == typeof(string) ? stringMaxLength : DEFAULT_LENGTH, false, null, false, stringMaxLength));
            }

            return toReturn;
        }

        private static string GenerateString(int? min, int? max, bool? required, bool? isCreditCardAttribute)
        {
            var generatedString = string.Empty;
            if (min == null && max == null && required == null && isCreditCardAttribute == null)
                throw new InvalidDataException("No bounds on string; data could be infinite.");

            if (required != null && (bool)required && min == null) min = 1;

            if (isCreditCardAttribute != null && (bool)isCreditCardAttribute)
            {
                generatedString = GenerateFakeCreditCardNumber();
            } else if (min != null && max != null)
            {
                generatedString = Random.Shared.NextStrings(((int)min, (int)max)).First();
            }
            else if (min == null && max != null)
            {
                generatedString = Random.Shared.NextStrings((0, (int)max)).First();
            }
            else
            {
                generatedString = Random.Shared.NextStrings(((int)min, Int32.MaxValue)).First();
            }

            return generatedString;
        }

        private static IEnumerable<string> NextStrings(
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

        private static string GenerateFakeCreditCardNumber()
        {
            return "1111222233334444";
        }

        public static T? GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            if (property == null) return null;
            return (T?)property?.GetCustomAttributes(attrType, false).FirstOrDefault();
        }

        public static Type? GetPropertyType(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName);
            return property?.PropertyType;
        }

        public static Type GetGenericType(this Type property)
        {
            return property.GenericTypeArguments.First();
        }
    }
}