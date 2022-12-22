using Newtonsoft.Json;
using SpecFlowProjectBank.Models;

namespace SpecFlowProjectBank.Utils;

public class JsonUtils
{
    public static Response GetResponse(string responseId, int loop)
    {

        var filePath ="JsonData/Responses.json";
        byte[] file = File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, filePath));
        string jsonData = System.Text.Encoding.UTF8.GetString(file, 0, file.Length);

        var responseList = JsonConvert.DeserializeObject<List<Response>>(jsonData) 
                       ?? new List<Response>();
        var response = responseList.FirstOrDefault(response1 => response1.Id == responseId && response1.Loop == loop);
        return response;
    }
}