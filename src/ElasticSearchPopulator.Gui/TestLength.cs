namespace ElasticSearchPopulator.Gui
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Core;
    using Core.DataWriter;

    public class TestLength : ITestLength
    {
        public int GetSizeInBytes<T>(int recordCount, Func<T> generate) where T : IHasId
        {
            var contentBuilder = new StringBuilder();
            var responseBuilder = new StringBuilder();
            
            var writer = new ElasticSearchBulkTextWriter(contentBuilder, responseBuilder, content => new Task<string>(() => string.Empty));

            var typeName = typeof(T).Name;

            Func<T, object> action = doc => new { create = new { _index = typeName + "s", _type = typeName, _id = doc.Id } };

            var docs = Enumerable.Range(0, recordCount).Select(x => generate());

            foreach (var document in docs)
            {
                writer.Write(action(document));
                writer.Write(document);
            }

            var bytes = Encoding.UTF8.GetBytes(contentBuilder.ToString());

            return bytes.Length;
        }
    }
}