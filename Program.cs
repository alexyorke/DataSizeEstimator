using System.Reflection;
using Newtonsoft.Json;

namespace DataSizeEstimator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            var handles = new Dictionary<System.Type, ITypeAttributeHandler>
            {
                [typeof(string)] = new StringHandler(),
                [typeof(List<>)] = new GenericListHandler()
            };

            var attrData = player.GetAttributesFrom(nameof(player.Names));
            var items = HandleAttributesForProperty(handles, attrData, player.GetPropertyType(nameof(player.Names)));
            var output = items.Aggregate((prev, next) => next.Concat(prev));
            Console.WriteLine(JsonConvert.SerializeObject(output.GetValue()));
        }

        static List<IConcatenableType> HandleAttributesForProperty(IReadOnlyDictionary<Type, ITypeAttributeHandler> handles, IList<CustomAttributeData> attrData, Type t)
        {
            if (t.IsGenericType)
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
}