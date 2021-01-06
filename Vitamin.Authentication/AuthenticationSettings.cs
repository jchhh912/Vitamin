namespace Vitamin.Authentication
{
    public class AuthenticationSettings
    {
        public AuthenticationProvider Provider { get; set; }
        public AuthenticationSettings()
        {
            Provider = AuthenticationProvider.Local;
        }
    }
}
