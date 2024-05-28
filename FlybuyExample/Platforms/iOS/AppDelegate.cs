using CoreLocation;
using FlyBuy;
using Foundation;
using UIKit;

namespace FlybuyExample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    CLLocationManager locationManager;

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        locationManager = new CLLocationManager();
        locationManager.RequestWhenInUseAuthorization();

        // Configure SDK
        var token = "102.CODE";
        var builder = FlyBuyConfigOptions.BuilderWithToken(token);
        builder.SetDeferredLocationTracking(true);
        FlyBuyCore.ConfigureWithOptions(builder.Build);

        // Configure Pickup Module
        FlyBuyPickupManager.Shared.Configure();

        return base.FinishedLaunching(app, options);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
