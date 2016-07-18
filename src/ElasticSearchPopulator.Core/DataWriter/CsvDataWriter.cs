namespace ElasticSearchPopulator.Core.DataWriter
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Utilities;

    using Extensions;

    public class CsvDataWriter : IDataWriter
    {
        public void Initialise(string index, string type)
        {
        }

        public async Task ExecuteWrite<T>(TextWriter writer, Func<T> generateData, IProgress<double> progress, int numberOfRecords, int sizeOfBatch) where T : IHasId
        {
            ICalculatePercentage percentage = new CalculatePercentage();

            var streamWriter = (StreamWriter)writer;

            var headers = typeof(T)
                                .GetProperties()
                                .Select(x => x.Name)
                                .CommaSeparate();

            await streamWriter
                    .WriteLineAsync(headers)
                    .ConfigureAwait(false);

            var allBatch = numberOfRecords / sizeOfBatch;

            for (var i = 0; i < numberOfRecords; i++)
            {
                var document = generateData();
                
                var values = document
                                .GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Select(x =>
                                    Convert
                                        .ToString(x.GetValue(document, null))
                                        .Replace(Environment.NewLine, " "))
                                .CommaSeparate();

                await streamWriter
                            .WriteLineAsync(values)
                            .ConfigureAwait(false);

                progress.Report(percentage.Calculate(allBatch, i));
            }
        }
    }
}