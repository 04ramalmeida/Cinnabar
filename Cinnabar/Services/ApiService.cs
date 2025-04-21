using System.Text.Json;

namespace Cinnabar.Services;

public class ApiService<T> where T : class
{
    private readonly HttpClient _client = new HttpClient();
    
    public async Task<ApiResponse> Get(string url)
    {
        _client.BaseAddress = new Uri(url);
        
        var response = await _client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse { 
                IsSuccess = false,
                StatusCode = response.StatusCode,
                Message = response.ReasonPhrase 
            };
        }
        else
        {
            T apiObject = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = response.StatusCode,
                Object = apiObject
            };
        }
    }
}