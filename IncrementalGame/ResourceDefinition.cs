namespace IncrementalGame
{
    public abstract class ResourceDefinition
    {
        public string Id { get; }
        public string Name { get; }

        protected ResourceDefinition(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}