using System.Text.Json;
using WeerAPI.Domain;

namespace WeerAPI.Infrastructure.ApiClients;

public class WeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public WeatherApiClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<WeatherRecord> GetWeatherAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
        var json = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        // Nested navigation with JsonDocument
        var main = root.GetProperty("main");
        var wind = root.GetProperty("wind");
        var weatherArray = root.GetProperty("weather");
        var description = weatherArray.EnumerateArray().First().GetProperty("description").GetString()!;

        return new WeatherRecord
        {
            City = root.GetProperty("name").GetString()!,
            Temperature = main.GetProperty("temp").GetDouble(),
            FeelsLike = main.GetProperty("feels_like").GetDouble(),
            Humidity = main.GetProperty("humidity").GetInt32(),
            WindSpeed = wind.GetProperty("speed").GetDouble(),
            Description = description,
            SavedAt = DateTime.Now
        };
    }
}
