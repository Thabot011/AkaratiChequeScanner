using AkaratiCheckScanner;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(GlobalSetting.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalSetting.AuthToken);
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var request = new
        {
            username,
            password,
            identifier = "KS6YL+e8wNw3VFhRXx7ssQ=="
        };

        string jsonData = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/v1/token", content);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

