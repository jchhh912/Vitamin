namespace Vitamin.Authentication
{
    public class AuthenticationSettings
    {
        //登录方式选择
        public AuthenticationProvider Provider { get; set; }
        public AuthenticationSettings()
        {
            Provider = AuthenticationProvider.Local;
        }
    }
}
