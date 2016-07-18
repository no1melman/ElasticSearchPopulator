namespace ElasticSearchPopulator.Core.DataWriter
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IDataWriter
    {
        void Initialise(string index, string type);

        Task ExecuteWrite<T>(TextWriter writer, Func<T> generateData, IProgress<double> progress, int numberOfRecords, int sizeOfBatch) where T : IHasId;
    }
}