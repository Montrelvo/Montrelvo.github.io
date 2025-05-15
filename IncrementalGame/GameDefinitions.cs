using System.Collections.Generic;

namespace IncrementalGame
{
    public static class GameDefinitions
    {
        public static List<ResourceDefinition> ResourceDefinitions { get; } = new List<ResourceDefinition>
        {
            new FoodResource(),
            new WaterResource()
            // Add new resource definitions here
        };

        public static List<ProducerDefinition> ProducerDefinitions { get; } = new List<ProducerDefinition>
        {
            new FarmProducer(),
            new WellProducer()
            // Add new producer definitions here
        };

        // Helper method to get a resource definition by Id
        public static ResourceDefinition GetResourceDefinition(string id)
        {
            return ResourceDefinitions.Find(r => r.Id == id);
        }

        // Helper method to get a producer definition by Id
        public static ProducerDefinition GetProducerDefinition(string id)
        {
            return ProducerDefinitions.Find(p => p.Id == id);
        }
    }
}