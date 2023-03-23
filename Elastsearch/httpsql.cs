using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Elastsearch
{
    public class QueryParam
    {
        public string query { get; set; }
    }

    public class HttpHelper
    {
        private static HttpClient _httpClient = new HttpClient();

        public static string Post(QueryParam param, string url)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(param));
            content.Headers.ContentType= new MediaTypeHeaderValue("application/json");
            string result = _httpClient.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            content.Dispose();
            return result;
        }
    }
}