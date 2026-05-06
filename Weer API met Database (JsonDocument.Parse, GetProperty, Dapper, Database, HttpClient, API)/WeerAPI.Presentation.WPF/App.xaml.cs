using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using WeerAPI.Application;
using WeerAPI.Infrastructure.ApiClients;
using WeerAPI.Infrastructure.Repositories;

namespace WeerAPI.Presentation.WPF;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var doc = JsonDocument.Parse(File.ReadAllText("appsettings.json"));
        var root = doc.RootElement;
        var apiKey = root.GetProperty("ApiKey").GetString()!;
        var connectionString = root.GetProperty("ConnectionString").GetString()!;

        /*
         * api keys enz prive maken
         * 1: Microsoft.Extension.Configuration.Json
         * 2: Microsoft.Extension.Configuration
         * 
         * const string apiKey = "je api key";
         * const string connectionString = "Server=.\\SQLEXPRESS;Database=data base naam;Integrated Security=True;TrustServerCertificate=True;";
         */

        var httpClient = new HttpClient();
        var apiClient = new WeatherApiClient(httpClient, apiKey);
        var repository = new WeatherRepository(connectionString);
        var service = new WeatherService(apiClient, repository);

        var mainWindow = new MainWindow(service);
        mainWindow.Show();
    }
}
