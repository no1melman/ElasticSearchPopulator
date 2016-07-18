namespace ElasticSearchPopulator.Core
{
    public class Citizen : IHasId
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string NationalInsuranceNumber { get; set; }

        public string DateOfBirth { get; set; }

        public string Occupation { get; set; }

        public string CreatedBy { get; set; }

        public string Source { get; set; }
    }
}