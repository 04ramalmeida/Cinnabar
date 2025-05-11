namespace Cinnabar.Models;

public class Cat
{
    
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class Dog
{
    public string Message  { get; set; }
    public string Status { get; set; }
}

public class Fox
{
    public string Image { get; set; }
    public string Link { get; set; }
}

public class Weather
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

public class DictionaryDef
{
    public string Word { get; set; }
    public string Phonetic { get; set; }
    public Phonetic[] Phonetics { get; set; }
    public string Origin { get; set; }
    public required Meaning[] Meanings { get; set; }
}

public class Phonetic
{
    public string? Text  { get; set; }
    public string? Audio { get; set; }
}

public class Meaning
{
    public required string PartOfSpeech  { get; set; }
    public required Definitions[] Definitions { get; set; }
}

public class Definitions
{
    public string Definition { get; set; }
    public string Example { get; set; }
    public string[] Synonyms { get; set; }
    public string[] Antonyms { get; set; }
}