using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopyCat
{
    public class AzureDevOpsApiClient : IAzureDevOpsApiClient
    {
        public async Task<IList<Repository>> GetRepositoriesAsync(string requestUri, string project, string pat)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}")));

                var response = await client.GetAsync($"{requestUri}/{project}/_apis/git/repositories?api-version=5.0");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var body = JsonConvert.DeserializeObject<dynamic>(content);

                return body.value.ToObject<List<Repository>>();
            }
        }
    }

    internal interface IAzureDevOpsApiClient
    {
        Task<IList<Repository>> GetRepositoriesAsync(string requestUri, string project, string pat);
    }
}
