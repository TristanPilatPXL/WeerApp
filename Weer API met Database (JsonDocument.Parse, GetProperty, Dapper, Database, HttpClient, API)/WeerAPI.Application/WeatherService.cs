using WeerAPI.Domain;
using WeerAPI.Infrastructure.ApiClients;
using WeerAPI.Infrastructure.Repositories;

namespace WeerAPI.Application;

public class WeatherService
{
    private readonly WeatherApiClient _apiClient;
    private readonly WeatherRepository _repository;

    public WeatherService(WeatherApiClient apiClient, WeatherRepository repository)
    {
        _apiClient = apiClient;
        _repository = repository;
    }

    public async Task<WeatherRecord> FetchWeatherAsync(string city)
        => await _apiClient.GetWeatherAsync(city);

    public async Task SaveWeatherAsync(WeatherRecord record)
        => await _repository.SaveAsync(record);

    public async Task<IEnumerable<WeatherRecord>> GetSavedRecordsAsync()
        => await _repository.GetAllAsync();

    public async Task DeleteRecordAsync(int id)
        => await _repository.DeleteAsync(id);
}
