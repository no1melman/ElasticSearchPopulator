namespace ElasticSearchPopulator.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Data;
    using DataWriter;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ElasticSearchHttpDataContext : IDataContext
    {
        private readonly string setIndex;

        private readonly HttpClient httpClient;

        public ElasticSearchHttpDataContext()
            : this(new Uri("http://localhost:9200/"))
        {
        }

        public ElasticSearchHttpDataContext(string index)
            : this(new Uri("http://localhost:9200/"), index)
        {
        }

        public ElasticSearchHttpDataContext(Uri baseAddress)
            : this(baseAddress, null)
        {
        }

        public ElasticSearchHttpDataContext(Uri baseAddress, string index)
        {
            this.setIndex = index;
            this.httpClient = new HttpClient
                                  {
                                      BaseAddress = baseAddress
                                  };
        }

        public async Task<string> Search(object query)
        {
            return await this.Search(this.setIndex, query).ConfigureAwait(false);
        }

        public async Task<string> Search(string index, object query)
        {
            var content = JsonConvert.SerializeObject(query);

            var response = await this
                                     .httpClient
                                     .PostAsync($"{index}/_search?pretty", this.CreateJsonContent(content))
                                     .ConfigureAwait(false);

            return await response
                             .Content
                             .ReadAsStringAsync()
                             .ConfigureAwait(false);
        }

        public async Task<string> BulkInsert<T>(string index, string type, Func<TextWriter, Task> writer) where T : IHasId
        {
            var sb = new StringBuilder();
            var responses= new StringBuilder();

            Func<string, Task<string>> flushTask = async content =>
                {
                    var response = await this
                                             .httpClient
                                             .PostAsync($"_bulk?pretty", this.CreateJsonContent(content))
                                             .ConfigureAwait(false);

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                };

            using (var textWriter = new ElasticSearchBulkTextWriter(sb, responses, flushTask))
            {
                await writer(textWriter);
            }

            return responses.ToString();
        }

        public async Task<string> DeleteIndex(string index)
        {
            var response = await this
                                     .httpClient
                                     .DeleteAsync($"{index}?pretty")
                                     .ConfigureAwait(false);

            return await response
                             .Content
                             .ReadAsStringAsync()
                             .ConfigureAwait(false);
        }

        public JArray GetResults(string response)
        {
            var results = DeserialiseResponse(response);

            var jToken = results[ElasticSearchResponseKeys.Hits];

            return (JArray)jToken[ElasticSearchResponseKeys.Hits];
        }

        public IEnumerable<JToken> GetResults(string response, bool orderByScore)
        {
            var results = DeserialiseResponse(response);

            var outerHits = results[ElasticSearchResponseKeys.Hits];

            var innerHits = (JArray)outerHits[ElasticSearchResponseKeys.Hits];

            var orderedResults = innerHits.OrderBy(x => ((JObject)x)[ElasticSearchResponseKeys.Score]);

            return orderedResults;
        }

        public JObject ExtractSource(JObject outerObject)
        {
            return outerObject[ElasticSearchResponseKeys.Score] != null
                       ? (JObject)outerObject[ElasticSearchResponseKeys.Source]
                       : null;
        }

        private static JObject DeserialiseResponse(string response)
        {
            return JsonConvert.DeserializeObject<JObject>(response);
        }

        private HttpContent CreateJsonContent(string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        internal static class ElasticSearchResponseKeys
        {
            public const string Hits = "hits";
            public const string Index = "_index";
            public const string Type = "_type";
            public const string Id = "_id";
            public const string Score = "_score";
            public const string Source = "_source";
            public const string Total = "total";
            public const string MaxScore = "max_score";
        }
    }
}