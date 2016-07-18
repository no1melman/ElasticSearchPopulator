namespace ElasticSearchPopulator.Core.DataWriter
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Utilities;

    public class ElasticDataWriter : IDataWriter
    {
        private string index;

        private string type;

        public void Initialise(string index, string type)
        {
            this.index = index;
            this.type = type;
        }

        public async Task ExecuteWrite<T>(TextWriter writer, Func<T> generateData, IProgress<double> progress, int numberOfRecords, int sizeOfBatch) where T : IHasId
        {
            Func<T, object> action = doc => new { create = new { _index = this.index, _type = this.type, _id = doc.Id } };

            ICalculatePercentage percentage = new CalculatePercentage();

            for (var i = 0; i < numberOfRecords; i++)
            {
                var docs = Enumerable.Range(0, sizeOfBatch).Select(x => generateData());

                foreach (var document in docs)
                {
                    writer.Write(action(document));
                    writer.Write(document);
                }

                await writer.FlushAsync();

                progress.Report(percentage.Calculate(numberOfRecords, i));

                i += docs.Count();
            }
        }
    }


}