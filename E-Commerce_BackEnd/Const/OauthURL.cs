namespace E_Commerce_BackEnd.Const
{
    public static class OauthURL
    {
        public static readonly Dictionary<string, string> UserInfoUrls = new Dictionary<string, string>
        {
            { "facebook", "https://graph.facebook.com/me?fields=id,first_name,last_name,email" },
            { "google", "https://www.googleapis.com/oauth2/v1/userinfo?alt=json" },
            { "twitter", "https://api.twitter.com/1.1/account/verify_credentials.json" },
            { "github", "https://api.github.com/user" },
            { "linkedin", "https://api.linkedin.com/v2/me" }
        };
    }
}
