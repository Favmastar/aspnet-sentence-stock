using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SentenceStock.Api.GraphQl;
using SentenceStock.Domain;

namespace SentenceStock.Api
{
    internal static class Program
    {
        private const string Prefix = "http://localhost:5000/graphql/";

        private static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            var repository = new InMemorySentenceRepository();
            var provider = new SimpleServiceProvider();

            provider.Add<ISentenceRepository>(repository);
            provider.Add(new SentenceType());
            provider.Add(new SentenceInputType());
            provider.Add(new SentenceQuery(repository));
            provider.Add(new SentenceMutation(repository));
            provider.Add(new SentenceStockSchema(provider));

            var executer = new DocumentExecuter();
            var writer = new GraphQLSerializer();

            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(Prefix);
                listener.Start();

                Console.WriteLine("SentenceStock GraphQL API listening on " + Prefix);
                Console.WriteLine("Press Ctrl+C to stop.");

                while (true)
                {
                    var context = await listener.GetContextAsync().ConfigureAwait(false);
                    _ = Task.Run(() => HandleRequestAsync(context, provider, executer, writer));
                }
            }
        }

        private static async Task HandleRequestAsync(
            HttpListenerContext http,
            SimpleServiceProvider provider,
            DocumentExecuter executer,
            GraphQLSerializer writer)
        {
            AddCorsHeaders(http.Response);

            if (http.Request.HttpMethod == "OPTIONS")
            {
                http.Response.StatusCode = 204;
                http.Response.Close();
                return;
            }

            if (http.Request.HttpMethod != "POST")
            {
                await WriteResponseAsync(http.Response, 405, "{\"error\":\"POST is required.\"}")
                    .ConfigureAwait(false);
                return;
            }

            try
            {
                var request = await ReadRequestAsync(http.Request).ConfigureAwait(false);
                var schema = (SentenceStockSchema)provider.GetService(typeof(SentenceStockSchema));

                var result = await executer.ExecuteAsync(options =>
                {
                    options.Schema = schema;
                    options.Query = request.Query;
                    options.OperationName = request.OperationName;
                    options.Variables = request.Variables == null
                        ? null
                        : new Inputs(ToDictionary(request.Variables));
                }).ConfigureAwait(false);

                var json = writer.Serialize(result);
                await WriteResponseAsync(http.Response, 200, json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var json = JsonConvert.SerializeObject(new { error = ex.Message });
                await WriteResponseAsync(http.Response, 500, json).ConfigureAwait(false);
            }
        }

        private static async Task<GraphQlRequest> ReadRequestAsync(HttpListenerRequest request)
        {
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<GraphQlRequest>(body);
            }
        }

        private static Dictionary<string, object> ToDictionary(JObject value)
        {
            return value.Properties()
                .ToDictionary(property => property.Name, property => ToPlainValue(property.Value));
        }

        private static object ToPlainValue(JToken token)
        {
            if (token is JObject obj)
            {
                return ToDictionary(obj);
            }

            if (token is JArray array)
            {
                return array.Select(ToPlainValue).ToList();
            }

            return ((JValue)token).Value;
        }

        private static async Task WriteResponseAsync(HttpListenerResponse response, int statusCode, string body)
        {
            var buffer = Encoding.UTF8.GetBytes(body);
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            response.Close();
        }

        private static void AddCorsHeaders(HttpListenerResponse response)
        {
            response.Headers["Access-Control-Allow-Origin"] = "*";
            response.Headers["Access-Control-Allow-Headers"] = "content-type";
            response.Headers["Access-Control-Allow-Methods"] = "POST, OPTIONS";
        }
    }
}
