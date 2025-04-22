using System.Text.Json;
using Cinnabar.Modules;

namespace Cinnabar.Services;

public class ApiService
{
    
    
    public async Task<ApiResponse<T>> Get<T>(string url)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(url);
        
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<T> { 
                IsSuccess = false,
                StatusCode = response.StatusCode,
                Message = response.ReasonPhrase 
            };
        }
        else
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiObject = JsonSerializer.Deserialize<T>(responseContent, options);
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = response.StatusCode,
                Object = apiObject
            };
        }
    }
}