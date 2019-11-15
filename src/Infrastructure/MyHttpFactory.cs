using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Mjc.Templates.WebApi.Core.Exceptions;
using Mjc.Templates.WebApi.Core.Interfaces;

namespace Mjc.Templates.WebApi.Infrastructure
{
    public class MyHttpFactory : IMyHttpFactory
    {
        private readonly HttpClient _httpClient;

        public MyHttpFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Demo1Async(string keyword)
        {
            return await SendGetRequest($"https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={keyword}&utf8=&format=json&srsort=just_match&srlimit=1", null);
        }

        private async Task<string> SendDeleteRequest(string api)
        {
            var url = $"{api}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var error = JsonSerializer.Deserialize<ThirdPartyApiErrorMessage>(await response.Content.ReadAsStringAsync());
            throw new ThirdPartyException(error.Message);
        }

        private async Task<string> SendGetRequest(string api, string query)
        {
            var url = $"{api}";

            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var error = JsonSerializer.Deserialize<ThirdPartyApiErrorMessage>(await response.Content.ReadAsStringAsync());
            throw new ThirdPartyException(error.Message);
        }

        private async Task<string> SendPatchRequest(string api, string json)
        {
            var url = $"{api}";

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            var error = JsonSerializer.Deserialize<ThirdPartyApiErrorMessage>(await response.Content.ReadAsStringAsync());
            throw new ThirdPartyException(error.Message);
        }

        private async Task<string> SendPostRequest(string api, string json)
        {
            var url = $"{api}";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            var error = JsonSerializer.Deserialize<ThirdPartyApiErrorMessage>(await response.Content.ReadAsStringAsync());
            throw new ThirdPartyException(error.Message);
        }
    }
}
