using System.Text.Json.Serialization;

namespace SpecFlowProjectBank.Models;

public class Account
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("value")]
    public double Value { get; set; }

    public Account(int userId, string type, double value)
    {
        Id = new Random().Next(1, 100000);
        UserId = userId;
        Type = type;
        Value = value;
    }
    public string Print()
    {
        return string.Format(@"ACCOUNT Id :{0}, UserId:{1}, Type:{2}, Value:{3} ",Id, UserId,Type,Value);
    }
}