using System.Text.Json;
using System.Text.Json.Serialization;
using Cinnabar.Services;
using Discord;
using Discord.Interactions;

namespace Cinnabar.Modules;

public class FunModule : InteractionModuleBase
{
    ApiService _apiService;
    EmbedBase _embed;
    public FunModule(
        ApiService apiService,
        EmbedBase embed)
    {
        _apiService = apiService;
        _embed = embed;
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

    [SlashCommand("fox", "Get a picture of a fox.")]
    public async Task Fox()
    {
        var apiRequest = await _apiService.Get<FoxHttpResponse>("https://randomfox.ca/floof/");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            FoxHttpResponse response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.Image);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("weather", "Get information about the weather in a certain city.")]
    public async Task Weather(string city)
    {
        var apiRequest = await _apiService.Get<WeatherHttpResponse>($"http://goweather.xyz/weather/{city}");
        var currentDate = DateTime.Now;
        var tomorrow = DateTime.Today.AddDays(1);
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            WeatherHttpResponse response = apiRequest.Object;
            var TemperatureField = new EmbedFieldBuilder
            {
                Name = "Temperature",
                Value = response.Temperature
            };
            var WindField = new EmbedFieldBuilder
            {
                Name = "Wind",
                Value = response.Wind
            };
            var DescriptionField = new EmbedFieldBuilder
            {
                Name = "Description",
                Value = response.Description
            };
            var ForecastTmrwField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(1).ToShortDateString()}",
                Value = response.Forecast[0].Temperature,
                IsInline = true
            };
            var ForecastTwoField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(2).ToShortDateString()}",
                Value = response.Forecast[1].Temperature,
                IsInline = true
            };
            var ForecastThreeField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(3).ToShortDateString()}",
                Value = response.Forecast[2].Temperature,
                IsInline = true
            };
            var embed = _embed.CinnabarEmbed($"Weather for {city}", String.Empty, String.Empty,
                [TemperatureField, WindField, DescriptionField, ForecastTmrwField, ForecastTwoField, ForecastThreeField], Context.User);
            await RespondAsync(embed: embed);
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

public class FoxHttpResponse
{
    public string Image { get; set; }
    public string Link { get; set; }
}

public class WeatherHttpResponse
{
    public string Temperature { get; set; }
    public string Wind  { get; set; }
    public string Description { get; set; }
    public Forecast[] Forecast { get; set; }
}

public class Forecast
{
    public string Day { get; set; }
    public string Temperature { get; set; }
    public string Wind { get; set; }
}