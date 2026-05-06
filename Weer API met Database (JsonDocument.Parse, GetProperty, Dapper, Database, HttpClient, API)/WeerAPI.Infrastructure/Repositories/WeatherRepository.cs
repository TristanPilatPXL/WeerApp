using Dapper;
using Microsoft.Data.SqlClient;
using WeerAPI.Domain;

namespace WeerAPI.Infrastructure.Repositories;

public class WeatherRepository
{
    private readonly string _connectionString;

    public WeatherRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveAsync(WeatherRecord record)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"INSERT INTO WeatherRecords (City, Temperature, FeelsLike, Humidity, WindSpeed, Description, SavedAt)
                    VALUES (@City, @Temperature, @FeelsLike, @Humidity, @WindSpeed, @Description, @SavedAt)";
        await connection.ExecuteAsync(sql, record);
    }

    public async Task<IEnumerable<WeatherRecord>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<WeatherRecord>("SELECT * FROM WeatherRecords ORDER BY SavedAt DESC");
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM WeatherRecords WHERE Id = @Id", new { Id = id });
    }
}
