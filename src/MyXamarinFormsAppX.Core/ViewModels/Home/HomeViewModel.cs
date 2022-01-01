using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MyXamarinFormsAppX.Core.Services;

namespace MyXamarinFormsAppX.Core.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public MvxAsyncCommand SignInGoogleCommand { get; private set; }
        private string _loginStatus;
        public string LoginStatus
        {
            get => _loginStatus;
            set
            {
                _loginStatus = value;
                RaisePropertyChanged(() => LoginStatus);

            }
        }

        private IFirebaseAuthenticator DaFireBaseAuth;

        public HomeViewModel()
        {
            SignInGoogleCommand = new MvxAsyncCommand(SignInGoogleAsync);
            LoginStatus = "---";
            DaFireBaseAuth = Mvx.IoCProvider.Resolve<IFirebaseAuthenticator>();

        }


        private async Task SignInGoogleAsync()
        {
            LoginStatus = "siging in";
            FirebaseUser user = await DaFireBaseAuth.LoginWithGoogle();
            _ = CheckStatusAsync();
        }
        public override void ViewAppeared()
        {
            _ = CheckStatusAsync();
            base.ViewAppeared();
        }

        private async Task CheckStatusAsync()
        {
            FirebaseUser user = await DaFireBaseAuth.GetUserInfo();
            if (user != null)
            {
                LoginStatus = user.DisplayName + " is logged in";
            }
        }
    }
}
