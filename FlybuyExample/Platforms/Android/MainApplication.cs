using Android.App;
using Android.Runtime;

using FlyBuy;
using FlyBuy.Pickup;

namespace FlybuyExample;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();
        
        const string appToken = "426.otMfWCDb4vsYKyZZtociLpJs";
        
        var builder = new ConfigOptions.Builder(appToken);
        builder.SetDeferredLocationTrackingEnabled(true);
        Core.Configure(this, builder.Build());
        
        var pickup = PickupManager.Manager.GetInstance(null) as PickupManager;
        pickup!.Configure(this);
    }
    
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}