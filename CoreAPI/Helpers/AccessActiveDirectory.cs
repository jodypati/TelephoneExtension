using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Options;

namespace CoreAPI.Helpers
{
    public class ActiveDirectory
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
    public class AccessActiveDirectory
    {
        private readonly IOptions<ActiveDirectory> _activeDirectory;
        private static string _domain { get; set; }
        private static string _username { get; set; }
        private static string _password { get; set; }

        public AccessActiveDirectory()
        {
            _domain = Startup.activeDirectoryDomain;
            _username = Startup.activeDirectoryUsername;
            _password = Startup.activeDirectoryPassword;
        }

        protected static PrincipalContext GetPrincipalContext()
        {
            return new PrincipalContext(ContextType.Domain, Startup.activeDirectoryDomain, Startup.activeDirectoryUsername, Startup.activeDirectoryPassword);
        }

        protected static UserPrincipal GetUserPrincipal()
        {
            return new UserPrincipal(GetPrincipalContext());
        }

        protected static PrincipalSearcher GetPrincipalSearcher()
        {
            return new PrincipalSearcher(GetUserPrincipal());
        }

        public static string GetUserEmailByNik(string personNumber)
        {
            bool isFound = false;
            string email = null;

            foreach (var principal in GetPrincipalSearcher().FindAll())
            {
                var result = (UserPrincipal)principal;

                if ((result.Enabled == true && !string.IsNullOrEmpty(result.EmailAddress)) && (result.Description == personNumber && !string.IsNullOrEmpty(result.DisplayName)))
                {
                    email = result.EmailAddress;
                    isFound = true;
                    break;
                }
            }

            return isFound ? email : null;
        }

        public static string GetUserNumberByEmail(string email)
        {
            bool isFound = false;
            string personalNumber = null;
            email = email + "@" + Startup.activeDirectoryDomain;
            foreach (var principal in GetPrincipalSearcher().FindAll())
            {
                var result = (UserPrincipal)principal;
                if ((result.Enabled == true && !string.IsNullOrEmpty(result.Description)) && (result.EmailAddress == email && !string.IsNullOrEmpty(result.DisplayName)))
                {
                    personalNumber = result.Description;
                    isFound = true;
                    break;
                }
            }

            return isFound ? personalNumber : null;
        }

        public static bool ValidateUser(string username, string password)
        {
            PrincipalContext principalContext = GetPrincipalContext();
            bool result = principalContext.ValidateCredentials(username, password);
            return result;
        }
    }
}