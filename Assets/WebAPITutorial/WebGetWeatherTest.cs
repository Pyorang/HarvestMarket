using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetWeatherTest : MonoBehaviour
{
    private const string API_KEY = "0ddc4667490d12a799e9aa160a8c90a6";

    private async void Start()
    {
        float lat = 37.4049955f;
        float lon = 127.1060049f;

        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API_KEY}&units=metric&lang=kr";

        string result = await GetWebText(url);
        Debug.Log(result);

        WeatherResponse weather = JsonUtility.FromJson<WeatherResponse>(result);
        PrintWeather(weather);
    }

    private void PrintWeather(WeatherResponse weather)
    {
        Debug.Log("========== 🌤 날씨 정보 ==========");
        Debug.Log($"📍 도시: {weather.name} ({weather.sys.country})");
        Debug.Log($"📐 좌표: 위도 {weather.coord.lat}, 경도 {weather.coord.lon}");
        Debug.Log("-----------------------------------");
        Debug.Log($"🌡 현재 기온: {weather.main.temp}°C");
        Debug.Log($"🤔 체감 온도: {weather.main.feels_like}°C");
        Debug.Log($"⬇ 최저 기온: {weather.main.temp_min}°C");
        Debug.Log($"⬆ 최고 기온: {weather.main.temp_max}°C");
        Debug.Log("-----------------------------------");

        if (weather.weather != null && weather.weather.Length > 0)
        {
            Debug.Log($"☁ 날씨: {weather.weather[0].main} ({weather.weather[0].description})");
        }

        Debug.Log($"💧 습도: {weather.main.humidity}%");
        Debug.Log($"🔵 기압: {weather.main.pressure}hPa");
        Debug.Log($"👁 가시거리: {weather.visibility}m");
        Debug.Log("-----------------------------------");
        Debug.Log($"💨 풍속: {weather.wind.speed}m/s, 방향: {weather.wind.deg}°");
        Debug.Log($"☁ 구름량: {weather.clouds.all}%");
        Debug.Log("-----------------------------------");

        DateTime sunrise = DateTimeOffset.FromUnixTimeSeconds(weather.sys.sunrise + weather.timezone).DateTime;
        DateTime sunset = DateTimeOffset.FromUnixTimeSeconds(weather.sys.sunset + weather.timezone).DateTime;
        Debug.Log($"🌅 일출: {sunrise:HH:mm}");
        Debug.Log($"🌇 일몰: {sunset:HH:mm}");
        Debug.Log("===================================");
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }
}
