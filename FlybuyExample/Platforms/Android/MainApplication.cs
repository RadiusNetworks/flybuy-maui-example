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
        
        string appToken = "427.83r3299CtMi8H2LdNy4ZxAFr";
        var configOptions = new ConfigOptions.Builder(appToken).Build();
        Core.Configure(this, configOptions);
        var pickup = PickupManager.Manager.GetInstance(null) as PickupManager;
        pickup!.Configure(this);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}