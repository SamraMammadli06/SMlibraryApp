namespace SMlibraryApp.Core.Models;
public class Log
{
    public int userId { get; set; }
    public string url { get; set; }
    public string methodType { get; set; }
    public int statusCode { get; set; }
    public string requestBody { get; set; }
    public string responseBody { get; set; }
}
