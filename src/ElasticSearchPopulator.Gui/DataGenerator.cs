namespace ElasticSearchPopulator.Gui
{
    using System;
    using System.Threading.Tasks;

    using Core;
    using Core.Data;

    using Core.DataWriter;

    public class CitizenDataGenerator : IDataGenerator<Citizen>
    {
        private readonly IDataContext context;

        private readonly IDataWriter dataWriter;

        private readonly IDataItemGenerator<Citizen> dataItemGenerator;

        public CitizenDataGenerator(IDataContext context, IDataWriter dataWriter, IDataItemGenerator<Citizen> dataItemGenerator)
        {
            this.context = context;
            this.dataWriter = dataWriter;
            this.dataItemGenerator = dataItemGenerator;
        }

        public async Task<string> Generate(IProgress<double> progress, int noOfRecords, int batchSize)
        {
            var typeName = typeof(Citizen).Name.ToLower();
            var index = typeName + "s";
            
            this.dataWriter.Initialise(index, typeName);

            return await Task.Run(async () => await this.context.BulkInsert<Citizen>(index, typeName,
                async writer =>
                    {
                        await this.dataWriter.ExecuteWrite(writer, () => this.dataItemGenerator.Generate(), progress, noOfRecords, batchSize);
                    }));
        }
    }
}