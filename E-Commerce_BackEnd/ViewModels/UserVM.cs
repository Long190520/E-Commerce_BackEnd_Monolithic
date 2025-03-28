using System.ComponentModel.DataAnnotations;

namespace E_Commerce_BackEnd.ViewModels
{
    public class UserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExp { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }
    }
}
