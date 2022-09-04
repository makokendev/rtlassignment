using System.Collections.Generic;

namespace CodingChallenge.Domain.Entities;
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class TVMazeCastDataResponse
{
    public List<TVMazeCastItem> CastList { get; set; }
    public bool RateLimited { get; set; }
    public bool IsSuccessful { get; set; }
    public bool AlreadyStored { get; set; }
    public bool NotFound { get; set; }
}

public class Character
{
    public int id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    public Image image { get; set; }
    public Links _links { get; set; }
}

public class Country
{
    public string name { get; set; }
    public string code { get; set; }
    public string timezone { get; set; }
}

public class Image
{
    public string medium { get; set; }
    public string original { get; set; }
}

public class Links
{
    public Self self { get; set; }
}

public class Person
{
    public int id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    public Country country { get; set; }
    public string birthday { get; set; }
    public string deathday { get; set; }
    public string gender { get; set; }
    public Image image { get; set; }
    public int updated { get; set; }
    public Links _links { get; set; }
}

public class TVMazeCastItem
{
    public Person person { get; set; }
    public Character character { get; set; }
    public bool self { get; set; }
    public bool voice { get; set; }
}

public class Self
{
    public string href { get; set; }
}



