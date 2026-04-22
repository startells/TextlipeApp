using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TextileApp.AvaloniaClient.Services;
using TextileApp.Contracts.DTO.Response;

namespace TextileApp.AvaloniaClient.API.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly SessionService _sessionService;
    
    public ApiService(HttpClient httpClient, SessionService sessionService)
    {
        _httpClient = httpClient;
        _sessionService = sessionService;
    }
    
    public async Task<TResponse?> GetAsync<TResponse>(string url)
    {
        await AddAuthHeader();
        var response = await _httpClient.GetAsync(url);
        return await HandleResponse<TResponse>(response);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        await AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync(url, data);
        return await HandleResponse<TResponse>(response);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        await AddAuthHeader();
        var response = await _httpClient.PutAsJsonAsync(url, data);
        return await HandleResponse<TResponse>(response);
    }

    public async Task<bool> DeleteAsync(string url)
    {
        await AddAuthHeader();
        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }

    private async Task AddAuthHeader()
    {
        if (!string.IsNullOrEmpty(_sessionService.Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sessionService.Token);
        }
    }

    private async Task<TResponse?> HandleResponse<TResponse>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            if (error != null)
                throw new Exception($"Error {error.Status}: {error.Detail}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
