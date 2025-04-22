using System.Text.Json;
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


}

public class CatHttpResponse
{
    
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}