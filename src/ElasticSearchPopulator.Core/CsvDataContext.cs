namespace ElasticSearchPopulator.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Data;
    using Extensions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class CsvDataContext : IDataContext
    {
        private const string FileNameFormat = "{0}.csv";

        public async Task<string> BulkInsert<T>(string index, string type, Func<TextWriter, Task> writer) where T : IHasId
        {
            var fileName = FileNameFormat.InsertArgs(index);

            var ioErrors = new List<IOException>();
            var unknown = new List<Exception>();

            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 8192))
            {
                try
                {
                    await writer(streamWriter).ConfigureAwait(false);
                }
                catch (IOException ioex)
                {
                    ioErrors.Add(ioex);
                }
                catch (Exception ex)
                {
                    unknown.Add(ex);
                }
            }
            
            return JsonConvert
                .SerializeObject(new
                    {
                        // RecordsWritten = recordsWrote,
                        // TotalRecords = list.Count,
                        // SuccessPercentage = $"{decimal.Round((recordsWrote / (decimal)list.Count) * 100, 2)}%",
                        FilePath = new FileInfo(fileName).FullName,
                        IoErrorCount = ioErrors.Count,
                        UnknownErrorCount = unknown.Count,
                        IoErrors = ioErrors.Select(x => new { x.Message, HasInner = x.InnerException != null, Inner = new { x.InnerException?.Message } }),
                        UnknownErrors = unknown.Select(x => new { x.Message, HasInner = x.InnerException != null, Inner = new { x.InnerException?.Message } })
                }, Formatting.Indented);
        }

        public async Task<string> DeleteIndex(string index)
        {
            var fileResult = await Task.Run(
                () =>
                    {
                        var fileInfo = new FileInfo(FileNameFormat.InsertArgs(index));

                        if (!fileInfo.Exists)
                        {
                            return new { CsvFound = false, FileDelete = "-", Errors = "0", Message = "Nothing to delete" };
                        }

                        Exception error = null;
                        object result;
                        try
                        {
                            fileInfo.Delete();
                        }
                        catch (Exception ex)
                        {
                            error = ex;
                        }
                        finally
                        {
                            result = new { CsvFound = true, FileDeleted = error == null, Errors = error != null, error?.Message };
                        }

                        return result;
                    });

            return JsonConvert.SerializeObject(fileResult, Formatting.Indented);
        }

        public JObject ExtractSource(JObject outerObject)
        {
            throw new System.NotImplementedException();
        }

        public JArray GetResults(string response)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<JToken> GetResults(string response, bool orderByScore)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Search(object query)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> Search(string index, object query)
        {
            throw new System.NotImplementedException();
        }
    }
}