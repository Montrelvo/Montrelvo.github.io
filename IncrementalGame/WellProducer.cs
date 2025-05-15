using System.Collections.Generic;

namespace IncrementalGame
{
    public class WellProducer : ProducerDefinition
    {
        public WellProducer() : base(
            "Well",
            "Well",
            new Dictionary<string, double>
            {
                { "Water", 15 } // Example cost: 15 Water
            },
            new Dictionary<string, double>
            {
                { "Water", 0.5 } // Example production: 0.5 Water per time unit
            })
        {
        }
    }
}