using System;

[Serializable]
public class WeatherResponse
{
    public Coord coord;
    public WeatherInfo[] weather;
    public string @base;
    public MainInfo main;
    public int visibility;
    public WindInfo wind;
    public CloudsInfo clouds;
    public long dt;
    public SysInfo sys;
    public int timezone;
    public int id;
    public string name;
    public int cod;
}

[Serializable]
public class Coord
{
    public float lon;
    public float lat;
}

[Serializable]
public class WeatherInfo
{
    public int id;
    public string main;
    public string description;
    public string icon;
}

[Serializable]
public class MainInfo
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
    public int sea_level;
    public int grnd_level;
}

[Serializable]
public class WindInfo
{
    public float speed;
    public int deg;
}

[Serializable]
public class CloudsInfo
{
    public int all;
}

[Serializable]
public class SysInfo
{
    public int type;
    public int id;
    public string country;
    public long sunrise;
    public long sunset;
}
