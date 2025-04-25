using System.Text.Json;
using System.Text.Json.Serialization;
using Cinnabar.Services;
using Discord.Interactions;

namespace Cinnabar.Modules;

public class FunModule : InteractionModuleBase
{
    ApiService _apiService;
    public FunModule(ApiService apiService)
    {
        _apiService = apiService;
    }
    
    [SlashCommand("cat", "Get a picture of a cat.")]
    public async Task Cat()
    {
        var apiRequest = await _apiService.Get<List<CatHttpResponse>>("https://api.thecatapi.com/v1/images/search");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            List<CatHttpResponse> response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.FirstOrDefault().Url);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("dog", "Get a picture of a dog.")]
    public async Task Dog()
    {
        var apiRequest = await _apiService.Get<DogHttpResponse>("https://dog.ceo/api/breeds/image/random");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            DogHttpResponse response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.Message);
            await RespondAsync(url.AbsoluteUri);
        }
    }
}

public class CatHttpResponse
{
    
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class DogHttpResponse
{
    public string Message  { get; set; }
    public string Status { get; set; }
}