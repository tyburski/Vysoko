using Plugin.MauiMTAdmob;

namespace Elevation;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

        CrossMauiMTAdmob.Current.UserPersonalizedAds = true; 
		CrossMauiMTAdmob.Current.ComplyWithFamilyPolicies = true; 
		CrossMauiMTAdmob.Current.UseRestrictedDataProcessing = true; 
		CrossMauiMTAdmob.Current.AdsId = "ca-app-pub-7867115531406686/4157140034";
    }
}
