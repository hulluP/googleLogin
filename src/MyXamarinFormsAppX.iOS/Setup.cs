using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.IoC;
using MyXamarinFormsAppX.Core.Services;
using Serilog;
using Serilog.Extensions.Logging;

namespace MyXamarinFormsAppX.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, UI.App>
    {
        protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
        protected override IMvxIoCProvider InitializeIoC()
        {
            IMvxIoCProvider mvxIoCProvider = base.InitializeIoC();
            var firebase = new FirebaseAuthenticator();
            Mvx.IoCProvider.RegisterSingleton<IFirebaseAuthenticator>(firebase);
            return mvxIoCProvider;
        }
    }
}
