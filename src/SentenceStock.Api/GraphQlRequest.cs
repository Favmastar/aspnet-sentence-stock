using Newtonsoft.Json.Linq;

namespace SentenceStock.Api
{
    public sealed class GraphQlRequest
    {
        public string Query { get; set; }

        public string OperationName { get; set; }

        public JObject Variables { get; set; }
    }
}
