using AkaratiCheckScanner.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AkaratiCheckScanner.Service
{
    public class HttpclientService
    {
        private readonly HttpClient _httpClient;
        public HttpclientService()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }


        public async Task<T> Get<T>(string url)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GlobalSetting.AuthToken}");
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();  // Throws an exception if the status code is not successful

            // Read the API response
            string responseContent = await response.Content.ReadAsStringAsync();

            // Parse the API response (Assuming JSON in this case)
            var result = JsonConvert.DeserializeObject<T>(responseContent);
            return result;
        }


        public async Task<string> Post(string url, object body)
        {
            string jsonData = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GlobalSetting.AuthToken}");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();  // Throws an exception if the status code is not successful

            // Read the API response
            string responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

    }
}
