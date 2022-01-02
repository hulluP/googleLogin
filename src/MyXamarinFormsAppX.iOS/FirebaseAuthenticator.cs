using System;
using System.Threading.Tasks;
using AuthenticationServices;
using Foundation;
using UIKit;
using Google.SignIn;
using GoogleSignIn = Google.SignIn.SignIn;
using System.Threading;
using Firebase.Core;
using MyXamarinFormsAppX.Core.Services;

namespace MyXamarinFormsAppX.iOS
{
    public class FirebaseAuthenticator : NSObject, IFirebaseAuthenticator, ISignInDelegate
    {

        private TaskCompletionSource<GoogleUser> GoogleLogonTask;
        private UIViewController DaViewController;

        public FirebaseAuthenticator()
        {
        }

        public static bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var result = GoogleSignIn.SharedInstance.HandleUrl(url);

            if (result)
            {
                return result;
            }

            return false;
        }

        private async Task<FirebaseUser> ExtractFirebaseUser(GoogleUser user)
        {
            var newUser = new FirebaseUser
            {
                DisplayName = user.UserId
            };

            return newUser;
        }
        public void LogOut()
        {
        }

        public async Task<FirebaseUser> GetUserInfo()
        {
            DaViewController = GetTopViewController();
            string clientId = App.DefaultInstance.Options.ClientId;
            GoogleSignIn.SharedInstance.ClientId = clientId;
            GoogleSignIn.SharedInstance.Delegate = this;
            if (GoogleSignIn.SharedInstance.HasPreviousSignIn)
            {
                GoogleSignIn.SharedInstance.RestorePreviousSignIn();
            }
            GoogleUser user = GoogleSignIn.SharedInstance.CurrentUser;
            return await ExtractFirebaseUser(user);
        }

        public async Task<FirebaseUser> LoginWithGoogle()
        {
            try
            {

                if (GoogleLogonTask != null)
                {
                    GoogleLogonTask.SetCanceled();
                    GoogleLogonTask = null;
                }
                //DaLogOnToken = new CancellationTokenSource(10000);
                var credential = await GetCredentialAsync();


                GoogleLogonTask?.SetCanceled();
                GoogleLogonTask = null;


                return await ExtractFirebaseUser(credential);

            }
            catch (Exception ex)
            {
                _ = ex;
                return null;
            }
        }
        private Task<GoogleUser> GetCredentialAsync()
        {
            GoogleLogonTask = new TaskCompletionSource<GoogleUser>();
            try
            {
                GoogleSignIn.SharedInstance.PresentingViewController = DaViewController;
                if (GoogleSignIn.SharedInstance.HasPreviousSignIn)
                {
                    GoogleSignIn.SharedInstance.RestorePreviousSignIn();
                    return null;
                }
                GoogleSignIn.SharedInstance.SignInUser();

            }
            catch (Exception ex)
            {
                _ = ex;
                return null;
            }

            return GoogleLogonTask.Task;
        }
        public void DidSignIn(GoogleSignIn signIn, GoogleUser user, NSError error)
        {
            //_ = HandleGoogleLoginResult(user, error);
            if (user != null && error == null)
            {
                GoogleLogonTask?.SetResult(user);
                var authentication = user.Authentication;
            }
            else
            {
                GoogleLogonTask?.SetException(new NSErrorException(error));
            }
        }
        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
            //myActivityIndicator.StopAnimating();
        }

        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            DaViewController?.PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            DaViewController?.DismissViewController(true, null);
        }

        public void SignOut()
        {
            GoogleSignIn.SharedInstance.SignOutUser();
        }
        private UIViewController GetTopViewController()
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            if (rootViewController == null)
            {
                throw new NullReferenceException("RootViewController is null");
            }
            return rootViewController.PresentedViewController ?? rootViewController;
        }

    }



}
