namespace ElasticSearchPopulator.Gui
{
    using System;
    using System.Threading.Tasks;

    public interface IDataGenerator<T>
    {
        Task<string> Generate(IProgress<double> progress, int noOfRecords, int batchSize);
    }
}