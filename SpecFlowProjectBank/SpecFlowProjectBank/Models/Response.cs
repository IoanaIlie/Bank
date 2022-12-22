using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace SpecFlowProjectBank.Models;

public class Response
{

    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("responseCode")]
    public string ResponseCode { get; set; }
    [JsonPropertyName("responseText")]
    public string ResponseText { get; set; }
    [JsonPropertyName("loop")]
    public int Loop { get; set; }
   
    public Response(string id, int loop, string responseCode, string responseText)
    {
        Id = id;
        Loop = loop;
        ResponseCode = responseCode;
        ResponseText = responseText;
    }
    
}