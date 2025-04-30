using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AuthService()
    {
        _httpClient = new HttpClient();
        _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
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

        var response = await _httpClient.PostAsync($"{_baseUrl}/v1/token", content);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}

