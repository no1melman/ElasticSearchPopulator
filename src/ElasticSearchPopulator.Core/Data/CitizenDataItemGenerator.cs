namespace ElasticSearchPopulator.Core.Data
{
    using System;

    using Utilities;

    public class CitizenDataItemGenerator : IDataItemGenerator<Citizen>
    {
        private readonly IRandomDataGenerator randomDataGenerator;

        public CitizenDataItemGenerator(
            IRandomDataGenerator randomDataGenerator)
        {
            this.randomDataGenerator = randomDataGenerator;
        }

        public Citizen Generate()
        {
            return new Citizen
                       {
                           Id = Guid.NewGuid().ToString("N"),
                           FullName = this.randomDataGenerator.RandomName(),
                           Address = this.randomDataGenerator.RandomAddress(),
                           NationalInsuranceNumber = this.randomDataGenerator.RandomNationalInsuranceNumber(),
                           DateOfBirth = this.randomDataGenerator.DateOfBirth(),
                           Occupation = this.randomDataGenerator.Occupation(),
                           CreatedBy = this.randomDataGenerator.RandomUsername(),
                           Source = "RandomData"
                       };
        }
    }
}