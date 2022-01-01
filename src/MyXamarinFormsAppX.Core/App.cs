using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MyXamarinFormsAppX.Core.ViewModels.Home;

namespace MyXamarinFormsAppX.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
