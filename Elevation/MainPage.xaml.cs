using Android.Content.PM;
using Elevation.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.MauiMTAdmob;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Elevation;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        SetLocalization();
        LoadCoordinates();

        CrossMauiMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
    }

    private async void SetLocalization()
    {
        string localization = await SecureStorage.Default.GetAsync("localization");
        if (localization != null)
        {
            switch (localization)
            {
                case "English":
                    Coordinates.Text = English.coordinates;
                    Elevation.Text = English.elevation;
                    Btn.Text = English.refresh;
                    break;
                case "Polish":
                    Coordinates.Text = Polish.coordinates;
                    Elevation.Text = Polish.elevation;
                    Btn.Text = Polish.refresh;
                    break;
                case "Slovakian":
                    Coordinates.Text = Slovakian.coordinates;
                    Elevation.Text = Slovakian.elevation;
                    Btn.Text = Slovakian.refresh;
                    break;
                default:
                    Coordinates.Text = English.coordinates;
                    Elevation.Text = English.elevation;
                    Btn.Text = English.refresh;
                    break;
            }
        }
        else
        {
            await SecureStorage.Default.SetAsync("localization", "English");
            Coordinates.Text = English.coordinates;
            Elevation.Text = English.elevation;
            Btn.Text = English.refresh;
        }
    }

    private async void LoadCoordinates()
    {           
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            if (location != null)
            {
                x.Text = location.Latitude.ToString();
                y.Text = location.Longitude.ToString();
                using (var http = new HttpClient())
                {
                    var uri = new Uri("https://maps.googleapis.com/maps/api/elevation/json?locations=");
                    var response = await http.PostAsync($"{uri}{location.Latitude.ToString().Replace(",", ".")}%2C{location.Longitude.ToString().Replace(",", ".")}&key=private_key", null);
                    var r = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(r);

                    dynamic obj = JObject.Parse(r);
                    string elev = obj.results[0].elevation;
                    Console.WriteLine(elev);
                    el.Text = $"{elev.Split(".")[0]} m";
                }
            }
            else
            {
                y.Text = "-";
                x.Text = "-";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private async void ButtonClicked(object sender, EventArgs e)
    {
        CrossMauiMTAdmob.Current.ShowInterstitial();
        LoadCoordinates();

        CrossMauiMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");       
    }

    private async void UkClicked(object sender, EventArgs e)
    {
        await SecureStorage.Default.SetAsync("localization", "English");
        SetLocalization();
    }
    private async void PlClicked(object sender, EventArgs e)
    {
        await SecureStorage.Default.SetAsync("localization", "Polish");
        SetLocalization();
    }
    private async void SkClicked(object sender, EventArgs e)
    {
        await SecureStorage.Default.SetAsync("localization", "Slovakian");
        SetLocalization();
    }


}

