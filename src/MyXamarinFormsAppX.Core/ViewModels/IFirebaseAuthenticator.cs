
using System.Threading.Tasks;

namespace MyXamarinFormsAppX.Core.Services
{
    public interface IFirebaseAuthenticator
    {
        Task<FirebaseUser> LoginWithGoogle();
        void LogOut();
        Task<FirebaseUser> GetUserInfo();
    }


    public class FirebaseUser
    {
        public FirebaseUser()
        {
        }

        public FirebaseUser(string id, string displayName, string email, string photoUrl, string token)
        {
            Id = id;
            DisplayName = displayName;
            Email = email;
            PhotoUrl = photoUrl;
            Token = token;
            AuthenticationService = 0;
        }


        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string Token { get; set; }
        public int AuthenticationService { get; set; }
    }
}
