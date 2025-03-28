using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.ViewModels
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RefreshModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class OauthSignInModel
    {
        public string AccessToken { get; set; }
        public string Provider { get; set; }
    }

    public class GoogleUserInoModel
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }
}
