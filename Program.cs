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
                [typeof(List<>)] = new GenericListHandler(),
                //[typeof(List<string>)] = new GenericStringListHandler() // example: uncomment if you want to match List<string> as a top-level type
            };

            var propToGenerateDataFor = nameof(player.Names);
            var output = GenerateDataForProperty(player, propToGenerateDataFor, handles);
            player.Names = (List<List<string>>)output;
            Console.WriteLine(JsonConvert.SerializeObject(output));
        }

        private static object GenerateDataForProperty(Player player, string propToGenerateDataFor,
            Dictionary<Type, ITypeAttributeHandler> handles)
        {
            var attrData = player.GetAttributesFrom(propToGenerateDataFor);
            var items = HandleAttributesForProperty(handles, attrData, player.GetPropertyType(propToGenerateDataFor));

            IConcatenableType target = items.First();
            for (int i = 1; i < items.Count - 1; i++)
            {
                target = items[i].Concat(target);
            }

            return target.GetValue();
        }

        static List<IConcatenableType> HandleAttributesForProperty(IReadOnlyDictionary<Type, ITypeAttributeHandler> handles, IList<CustomAttributeData> attrData, Type t)
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
}