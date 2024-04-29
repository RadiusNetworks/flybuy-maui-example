using Android;
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using FlyBuy.Pickup;

namespace FlybuyExample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const int IRIS_PERMISSIONS_REQUEST = 42;

    protected override void OnStart()
    {
        base.OnStart();
        RequestPermissions();
    }

    private void RequestPermissions()
    {
        var permissions = CheckForMissingPermissions();
        if (permissions.Count > 0)
        {
            ActivityCompat.RequestPermissions(this, permissions.ToArray(), IRIS_PERMISSIONS_REQUEST);
        }
        else
        {
            var Pickup = PickupManager.Manager.GetInstance(null) as PickupManager;
            Pickup.OnPermissionChanged();
        }
    }

    private List<string> CheckForMissingPermissions()
    {
        var permissionsToRequest = new List<string>();

        if (ContextCompat.CheckSelfPermission(this, 
            Manifest.Permission.AccessCoarseLocation) != Permission.Granted)
        {
            permissionsToRequest.Add(Manifest.Permission.AccessCoarseLocation);
        }
        if (ContextCompat.CheckSelfPermission(this,
            Manifest.Permission.AccessFineLocation) != Permission.Granted)
        {
            permissionsToRequest.Add(Manifest.Permission.AccessFineLocation);
        }

        return permissionsToRequest;
    }
}