using Cinnabar.Models;
using Cinnabar.Services;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;

namespace Cinnabar.Modules;

public class FunModule : InteractionModuleBase
{
    ApiService _apiService;
    IConfiguration _config;
    public FunModule(
        ApiService apiService, IConfiguration config)
    {
        _apiService = apiService;
        _config = config;
    }

    
    [SlashCommand("cat", "Get a picture of a cat.")]
    public async Task Cat()
    {
        var apiRequest = await _apiService.Get<List<Cat>>("https://api.thecatapi.com/v1/images/search");
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Uri url = new Uri(apiRequest.Object.FirstOrDefault()!.Url);
            await RespondAsync(url.AbsoluteUri);
        }
    }
    
    
    [SlashCommand("dog", "Get a picture of a dog.")]
    public async Task Dog()
    {
        var apiRequest = await _apiService.Get<Dog>("https://dog.ceo/api/breeds/image/random");
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Uri url = new Uri(apiRequest.Object.Url);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("fox", "Get a picture of a fox.")]
    public async Task Fox()
    {
        var apiRequest = await _apiService.Get<Fox>("https://randomfox.ca/floof/");
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Uri url = new Uri(apiRequest.Object.Url);
            await RespondAsync(url.AbsoluteUri);
        }
    }

    [SlashCommand("weather", "Get information about the weather in a certain city.")]
    public async Task Weather(string city)
    {
        var apiRequest = await _apiService.Get<Weather>($"https://goweather.xyz/weather/{city}");
        var currentDate = DateTime.Now;
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Weather response = apiRequest.Object;
            var temperatureField = new EmbedFieldBuilder
            {
                Name = "Temperature",
                Value = response.Temperature
            };
            var windField = new EmbedFieldBuilder
            {
                Name = "Wind",
                Value = response.Wind
            };
            var descriptionField = new EmbedFieldBuilder
            {
                Name = "Description",
                Value = response.Description
            };
            var forecastTmrwField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(1).ToShortDateString()}",
                Value = response.Forecast[0].Temperature,
                IsInline = true
            };
            var forecastTwoField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(2).ToShortDateString()}",
                Value = response.Forecast[1].Temperature,
                IsInline = true
            };
            var forecastThreeField = new EmbedFieldBuilder
            {
                Name = $"Forecast for {currentDate.AddDays(3).ToShortDateString()}",
                Value = response.Forecast[2].Temperature,
                IsInline = true
            };
            Embed embed = EmbedBase.CinnabarEmbed($"Weather for {city}", String.Empty, String.Empty,
                [temperatureField, windField, descriptionField, forecastTmrwField, forecastTwoField, forecastThreeField], Context.User);
            await RespondAsync(embed: embed);
        }
    }

    [SlashCommand("dictionary", "Get a definition of a word")]
    public async Task Dictionary(string word)
    {
        var apiRequest = await _apiService.Get<List<DictionaryDef>>($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            DictionaryDef response = apiRequest.Object.FirstOrDefault()!;
            var firstMeaning = GetMeaning(response);
            string description = "**Definition**\n" +
                                 $"{firstMeaning.Definitions[0].Definition}\n" +
                                 $"-# {firstMeaning.PartOfSpeech}";
            EmbedFieldBuilder phoneticsField = new EmbedFieldBuilder
            {
                Name = "Phonetics",
                Value = response.Phonetics[1].Text
            };
            
            Embed embed = EmbedBase.CinnabarEmbed($"Definition for {response.Word}", description, String.Empty, 
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
    public async Task Album(string artist, string album)
    {
        var apiKey = _config.GetValue<string>("FmApiKey");

        Uri uri = new Uri(
            $"https://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key={apiKey}&artist={artist}&album={album}&format=json");
        
        var apiRequest = await _apiService.Get<AlbumRootObject>(uri.ToString());
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Album response = apiRequest.Object.Album;

            var tagsField = new EmbedFieldBuilder
            {
                Name = "Tags",
                Value = String.Join(" ",response.TagsRoot.Tags.Select(t => t.Name))
            };

            Embed embed = EmbedBase.CinnabarEmbed($"About {response.Name} by {response.Artist}",
                response.Wiki.Summary,
                response.Images[2].ImageUrl,
                [tagsField], Context.User
            );
            await RespondAsync(embed: embed);
        }
    }

    [SlashCommand("artist", "Get information about an artist")]
    public async Task Artist(string artist)
    {
        var apiKey = _config.GetValue<string>("FmApiKey");

        Uri uri = new Uri(
            $"https://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist={artist}&api_key={apiKey}&format=json");
        
        var apiRequest = await _apiService.Get<ArtistRootObject>(uri.ToString());
        if (!apiRequest.IsSuccess || apiRequest.Object == null)
        {
            await RespondAsync("An error has occured.");
        }
        else
        {
            Artist response = apiRequest.Object.artist;

            var tagsField = new EmbedFieldBuilder
            {
                Name = "Tags",
                Value = String.Join(" ",response.TagsRoot.Tags.Select(t => t.Name))
            };
            
            var similarField = new EmbedFieldBuilder
            {
                Name = "Similar artists",
                Value = String.Join(", ",response.Similar.Artist.Select(s => s.Name)),
                
            };
            Embed embed = EmbedBase.CinnabarEmbed($"About {response.Name}",
                response.Bio.Summary, response.Image[2].ImageUrl,
                [tagsField, similarField], Context.User); 
            await RespondAsync(embed: embed);
        }
    }
}

