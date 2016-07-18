namespace ElasticSearchPopulator.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    public interface IDataContext
    {
        Task<string> BulkInsert<T>(string index, string type, Func<TextWriter, Task> writer) where T : IHasId;
        Task<string> DeleteIndex(string index);
        JObject ExtractSource(JObject outerObject);
        JArray GetResults(string response);
        IEnumerable<JToken> GetResults(string response, bool orderByScore);
        Task<string> Search(object query);
        Task<string> Search(string index, object query);
    }
}