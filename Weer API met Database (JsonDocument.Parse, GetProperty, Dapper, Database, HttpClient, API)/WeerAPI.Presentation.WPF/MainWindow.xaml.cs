using System.Windows;
using WeerAPI.Application;
using WeerAPI.Domain;

namespace WeerAPI.Presentation.WPF;

public partial class MainWindow : Window
{
    private readonly WeatherService _service;
    private WeatherRecord? _currentRecord;

    public MainWindow(WeatherService service)
    {
        InitializeComponent();
        _service = service;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await RefreshGridAsync();
    }

    private async void ZoekButton_Click(object sender, RoutedEventArgs e)
    {
        var city = cityTextBox.Text.Trim();
        if (string.IsNullOrEmpty(city)) return;

        try
        {
            _currentRecord = await _service.FetchWeatherAsync(city);
            cityLabel.Text = $"Stad: {_currentRecord.City}";
            tempLabel.Text = $"Temperatuur: {_currentRecord.Temperature:F1} °C";
            feelsLikeLabel.Text = $"Voelt als: {_currentRecord.FeelsLike:F1} °C";
            descLabel.Text = $"Beschrijving: {_currentRecord.Description}";
            humidityLabel.Text = $"Luchtvochtigheid: {_currentRecord.Humidity}%";
            windLabel.Text = $"Windsnelheid: {_currentRecord.WindSpeed} m/s";
            saveButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fout: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_currentRecord == null) return;
        await _service.SaveWeatherAsync(_currentRecord);
        await RefreshGridAsync();
        saveButton.IsEnabled = false;
        _currentRecord = null;
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (recordsGrid.SelectedItem is WeatherRecord record)
        {
            await _service.DeleteRecordAsync(record.Id);
            await RefreshGridAsync();
        }
    }

    private async Task RefreshGridAsync()
    {
        var records = await _service.GetSavedRecordsAsync();
        recordsGrid.ItemsSource = null;
        recordsGrid.ItemsSource = records;
    }
}
