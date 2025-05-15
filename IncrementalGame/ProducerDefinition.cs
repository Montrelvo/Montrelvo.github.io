using System.Collections.Generic;

namespace IncrementalGame
{
    public abstract class ProducerDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public Dictionary<string, double> BaseCost { get; }
        public Dictionary<string, double> BaseProduction { get; }

        protected ProducerDefinition(string id, string name, Dictionary<string, double> baseCost, Dictionary<string, double> baseProduction)
        {
            Id = id;
            Name = name;
            BaseCost = baseCost ?? new Dictionary<string, double>();
            BaseProduction = baseProduction ?? new Dictionary<string, double>();
        }
    }
}