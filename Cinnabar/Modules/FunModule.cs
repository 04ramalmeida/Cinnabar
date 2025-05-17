using System.Text.Json;
using System.Text.Json.Serialization;
using Cinnabar.Models;
using Cinnabar.Services;
using Discord;
using Discord.Interactions;
using Newtonsoft.Json;

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
        var apiRequest = await _apiService.Get<List<Cat>>("https://api.thecatapi.com/v1/images/search");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            List<Cat> response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.FirstOrDefault().Url);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("dog", "Get a picture of a dog.")]
    public async Task Dog()
    {
        var apiRequest = await _apiService.Get<Dog>("https://dog.ceo/api/breeds/image/random");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Dog response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.Message);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("fox", "Get a picture of a fox.")]
    public async Task Fox()
    {
        var apiRequest = await _apiService.Get<Fox>("https://randomfox.ca/floof/");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Fox response = apiRequest.Object;
            Uri url = new Uri(apiRequest.Object.Image);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("weather", "Get information about the weather in a certain city.")]
    public async Task Weather(string city)
    {
        var apiRequest = await _apiService.Get<Weather>($"http://goweather.xyz/weather/{city}");
        var currentDate = DateTime.Now;
        var tomorrow = DateTime.Today.AddDays(1);
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Weather response = apiRequest.Object;
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

    [SlashCommand("dictionary", "Get a definition of a word")]
    public async Task Dictionary(string word)
    {
        var apiRequest = await _apiService.Get<List<DictionaryDef>>($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            DictionaryDef response = apiRequest.Object.FirstOrDefault();
            var firstMeaning = GetMeaning(response);
            string description = "**Definition**\n" +
                                 $"{firstMeaning.Definitions[0].Definition}\n" +
                                 $"-# {firstMeaning.PartOfSpeech}";
            EmbedFieldBuilder phoneticsField = new EmbedFieldBuilder
            {
                Name = "Phonetics",
                Value = response.Phonetics[1].Text
            };
            /*EmbedFieldBuilder originField = new EmbedFieldBuilder
            {
                Name = "Origin",
                Value = response.Origin
            };*/
            
            
            var embed = _embed.CinnabarEmbed($"Definition for {response.Word}", description, String.Empty, 
                [phoneticsField], Context.User);
            await RespondAsync(embed: embed);
        }
        
    }

    private Meaning GetMeaning(DictionaryDef response)
    {
        List<Meaning> meanings = response.Meanings.ToList();
        // Sort list by the length of the first definition somehow
        meanings = meanings.OrderByDescending(m => m.Definitions.First().Definition.Length).ToList();
        // Return the first item
        return meanings.First();
    }

    [SlashCommand("album", "Get information about an album")]
    public async Task Album()
    {
        var apiKey = JsonConvert.DeserializeObject<Config>(File.ReadAllText("appsettings.json")).FmApiKey;

        Uri uri = new Uri(
            $"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key={apiKey}&artist=Cher&album=Believe&format=json");
        
        var apiRequest = await _apiService.Get<AlbumRootObject>(uri.ToString());
        if (!apiRequest.IsSuccess)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Album response = apiRequest.Object.Album;

            var TagsField = new EmbedFieldBuilder
            {
                Name = "Tags",
                Value = String.Join(" ",response.Tags.Tag.Select(t => t.Name))
            };

            var embed = _embed.CinnabarEmbed($"About {response.Name} by {response.Artist}",
                response.Wiki.Summary,
                response.Image[2].ImageUrl,
                [TagsField], Context.User
            );
            await RespondAsync(embed: embed);
        }
    }
}

