using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]

[assembly: ExportFont("LemonRegular.ttf", Alias = "Lemon")]
[assembly: ExportFont("ARLRDBD.ttf", Alias = "ArialRoundedMT")]
