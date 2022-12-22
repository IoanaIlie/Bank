using System.Text.Json.Serialization;

namespace SpecFlowProjectBank.Models;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }

    public User(string name)
    {
        Id = new Random().Next(1, 1000);
        Name = name; 
    }
    public User(int id, string name)
    {
        Id = id;
        Name = name; 
    }
    public string Print()
    {
        return string.Format(@"USER Id :{0}, Name:{1} ",Id, Name);
    }
}