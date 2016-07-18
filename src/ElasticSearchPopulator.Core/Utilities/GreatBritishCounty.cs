namespace ElasticSearchPopulator.Core.Utilities
{
    internal sealed class GreatBritishCounty
    {
        public GreatBritishCounty(string name, string postCodeArea)
        {
            this.Name = name;
            this.PostCodeArea = postCodeArea;
        }

        public string Name { get; }

        public string PostCodeArea { get; }
    }
}