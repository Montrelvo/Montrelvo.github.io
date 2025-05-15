using System.Collections.Generic;
using System.Linq;

namespace IncrementalGame
{
    public class GameState
    {
        public Dictionary<string, double> Resources { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, int> Producers { get; set; } = new Dictionary<string, int>();

        public GameState()
        {
            // Initialize resources based on definitions
            foreach (var resourceDef in GameDefinitions.ResourceDefinitions)
            {
                Resources.Add(resourceDef.Id, 0);
            }

            // Initialize producers based on definitions
            foreach (var producerDef in GameDefinitions.ProducerDefinitions)
            {
                Producers.Add(producerDef.Id, 0);
            }
        }

        // Method to add resources
        public void AddResource(string resourceId, double amount)
        {
            if (Resources.ContainsKey(resourceId))
            {
                Resources[resourceId] += amount;
            }
        }

        // Method to buy a producer
        public bool BuyProducer(string producerId)
        {
            var producerDef = GameDefinitions.GetProducerDefinition(producerId);
            if (producerDef == null || !Producers.ContainsKey(producerId))
            {
                return false;
            }

            // Check if player has enough resources
            foreach (var cost in producerDef.BaseCost)
            {
                if (!Resources.ContainsKey(cost.Key) || Resources[cost.Key] < cost.Value)
                {
                    return false; // Not enough resources
                }
            }

            // Deduct resources
            foreach (var cost in producerDef.BaseCost)
            {
                Resources[cost.Key] -= cost.Value;
            }

            // Increase producer count
            Producers[producerId]++;
            return true;
        }

        // Method to calculate total production per resource per time unit
        public Dictionary<string, double> CalculateProductionPerSecond()
        {
            var production = new Dictionary<string, double>();

            foreach (var producerEntry in Producers)
            {
                string producerId = producerEntry.Key;
                int count = producerEntry.Value;
                var producerDef = GameDefinitions.GetProducerDefinition(producerId);

                if (producerDef != null)
                {
                    foreach (var productionEntry in producerDef.BaseProduction)
                    {
                        string resourceId = productionEntry.Key;
                        double amount = productionEntry.Value;

                        if (!production.ContainsKey(resourceId))
                        {
                            production.Add(resourceId, 0);
                        }
                        production[resourceId] += amount * count;
                    }
                }
            }

            return production;
        }
    }
}