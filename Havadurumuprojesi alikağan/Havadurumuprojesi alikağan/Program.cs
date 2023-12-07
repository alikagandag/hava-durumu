using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        await GetWeather("https://goweather.herokuapp.com/weather/istanbul");
        await GetWeather("https://goweather.herokuapp.com/weather/izmir");
        await GetWeather("https://goweather.herokuapp.com/weather/ankara");
    }

    static async Task GetWeather(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string jsonResult = await client.GetStringAsync(apiUrl);
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResult);

                // Şehir adını çıkartmak için Regex kullanımı
                string cityName = Regex.Match(apiUrl, @"\/weather\/(.+)$").Groups[1].Value;

                Console.WriteLine($"{cityName} Hava Durumu:");
                Console.WriteLine($"Sıcaklık: {weatherData.Temperature}");
                Console.WriteLine($"Rüzgar: {weatherData.Wind}");
                Console.WriteLine($"Durum: {weatherData.Description}");

                if (weatherData.Forecast != null && weatherData.Forecast.Count > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{cityName} 3 Günlük Tahmin:");
                    foreach (var forecast in weatherData.Forecast)
                    {
                        Console.WriteLine($"Tarih: {forecast.Day}");
                        Console.WriteLine($"Sıcaklık: {forecast.Temperature}");
                        Console.WriteLine($"Rüzgar: {forecast.Wind}");
                        Console.WriteLine();
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Hata: {e.Message}");
            }
        }
    }
}

class WeatherData
{
    public string Temperature { get; set; }
    public string Wind { get; set; }
    public string Description { get; set; }
    public List<ForecastData> Forecast { get; set; }
}

class ForecastData
{
    public string Day { get; set; }
    public string Temperature { get; set; }
    public string Wind { get; set; }
}