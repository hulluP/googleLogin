using Foundation;
using Google.SignIn;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;

namespace MyXamarinFormsAppX.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.App, UI.App>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // You can get the GoogleService-Info.plist file at https://developers.google.com/mobile/add
            var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
            SignIn.SharedInstance.ClientId = googleServiceDictionary["CLIENT_ID"].ToString();
            Firebase.Core.App.Configure();

            return base.FinishedLaunching(application, launchOptions);
        }
        // For iOS 9 or newer
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);
            return SignIn.SharedInstance.HandleUrl(url);
        }
    }
}
