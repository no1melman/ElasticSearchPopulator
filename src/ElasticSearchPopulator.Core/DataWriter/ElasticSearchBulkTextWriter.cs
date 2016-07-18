namespace ElasticSearchPopulator.Core.DataWriter
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ElasticSearchBulkTextWriter : TextWriter
    {
        private readonly StringBuilder content;

        private readonly StringBuilder responses;

        private readonly Func<string, Task<string>> flushTask;

        public ElasticSearchBulkTextWriter(StringBuilder content, StringBuilder responses, Func<string, Task<string>> flushTask)
        {
            this.content = content;
            this.responses = responses;
            this.flushTask = flushTask;
        }

        public override Encoding Encoding { get; }

        public override void Write(string value)
        {
            this.content.Append(value);
        }

        public override void Write(object value)
        {
            this.content.Append(this.CreateJson(value));
        }

        public override void WriteLine(string value)
        {
            this.content.AppendLine(value);
        }

        public override void WriteLine(object value)
        {
            this.content.AppendLine(this.CreateJson(value));
        }

        public override void Flush()
        {
        }

        public override async Task FlushAsync()
        {
            this.content.AppendLine();

            this.responses.AppendLine(await this.flushTask(this.content.ToString()));

            this.content.Clear();
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override void Close()
        {
        }

        private string CreateJson(object value)
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}