using DataSizeEstimator.Handlers;
using Newtonsoft.Json;

namespace DataSizeEstimator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var player = new Player();
            var handles = new Dictionary<Type, ITypeAttributeHandler>
            {
                [typeof(string)] = new StringHandler(),
                [typeof(List<>)] = new GenericListHandler(),
                //[typeof(List<string>)] = new GenericStringListHandler() // example: uncomment if you want to match List<string> as a top-level type
            };

            var propToGenerateDataFor = nameof(player.PlayerDescription);
            var estimator = new DataSizeEstimator();
            var output = estimator.GenerateDataForProperty(player, propToGenerateDataFor, handles);
            player.PlayerDescription = output;
            Console.WriteLine(JsonConvert.SerializeObject(output));
        }
    }
}