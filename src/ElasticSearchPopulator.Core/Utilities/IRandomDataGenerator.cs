namespace ElasticSearchPopulator.Core.Utilities
{
    public interface IRandomDataGenerator
    {
        string RandomAddress();

        string RandomName();

        string RandomNationalInsuranceNumber();

        string DateOfBirth();

        string Occupation();

        string RandomUsername();
    }
}