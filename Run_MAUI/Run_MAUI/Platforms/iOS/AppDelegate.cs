using Foundation;

namespace Run_MAUI.Platforms.iOS {
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
