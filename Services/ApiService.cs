using AkaratiCheckScanner.Model;
using Newtonsoft.Json;
using SimpleScan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkaratiCheckScanner.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(GlobalSetting.BaseUrl);
            _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSetting.AuthToken);
        }

        public async Task<List<LookupItem>> GetLookupItemsAsync(string searchTerm, string lookupType)
        {
            try
            {
                var apiUrl = $"/v1/lookups/{lookupType}/{searchTerm}";
                var response = await _client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<LookupItem>>(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching {lookupType}: {ex.Message}");
                return null;
            }
        }  
        public async Task<List<LookupItem>> GetLookupItemsAsync(string lookupType)
        {
            try
            {
                var apiUrl = $"/v1/lookups/{lookupType}";
                var response = await _client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<LookupItem>>(content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching {lookupType}: {ex.Message}");
                return null;
            }
        }

        public async Task CreateChequeAsync(CreateChequesRequestDto chequeRequest)
        {
            try
            {
                var apiUrl = $"/v1/user/createCheques";
                var jsonData = JsonConvert.SerializeObject(chequeRequest);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Cheque request created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating cheque: {ex.Message}");
            }
        }
    }

}
