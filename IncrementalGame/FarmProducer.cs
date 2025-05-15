using System.Collections.Generic;

namespace IncrementalGame
{
    public class FarmProducer : ProducerDefinition
    {
        public FarmProducer() : base(
            "Farm",
            "Farm",
            new Dictionary<string, double>
            {
                { "Food", 10 } // Example cost: 10 Food
            },
            new Dictionary<string, double>
            {
                { "Food", 1 } // Example production: 1 Food per time unit
            })
        {
        }
    }
}