using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce_BackEnd.Const;
using E_Commerce_BackEnd.Models;
using E_Commerce_BackEnd.Utils;
using E_Commerce_BackEnd.ViewModels;

namespace E_Commerce_BackEnd.Services
{
    public class UserServices
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public UserServices(MyDbContext myDbContext, IConfiguration configuration, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _context = myDbContext;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region AuthFunction
        public async Task<IdentityResult> SignUpAsync(SignUpModel signUpModel)
        {
            var user = new Users()
            {
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                Email = signUpModel.Email,
                UserName = signUpModel.Email
            };

            var res = await _userManager.CreateAsync(user, signUpModel.Password);

            var cart = new Carts()
            {
                UserId = user.Id
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return res;
        }

        public async Task<UserVM?> LoginAsync(SignInModel signInModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);

            if (!result.Succeeded)
            {
                return null;
            }

            var user = await _userManager.Users.Where(u => !_context.UserLogins.Any(ul => ul.UserId == u.Id) && u.Email == signInModel.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var res = GetUserToken(user);

            user.RefreshToken = res.RefreshToken;
            user.RefreshTokenExp = res.RefreshTokenExp;
            await _userManager.UpdateAsync(user);

            return res;
        }

        public async Task<bool> SignOutAsync(UserVM userViewModel)
        {

            var user = await _userManager.Users.Where(u => !_context.UserLogins.Any(ul => ul.UserId == u.Id) && u.Email == userViewModel.Email).FirstOrDefaultAsync();

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExp = null;
                await _userManager.UpdateAsync(user);
                return true;
            }

            return true;
        }

        public async Task<UserVM?> RefreshAsync(RefreshModel refreshModel)
        {
            var principal = UserUtils.GetPrincipalFromAccessToken(refreshModel.AccessToken, _configuration);
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null)
            {
                throw new InvalidOperationException($"{nameof(userIdClaim)} is null");
            }
            //int.TryParse(userIdClaim.Value, out int userId);
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id.ToString() == userIdClaim.Value
                    && x.RefreshToken == refreshModel.RefreshToken
                    && x.RefreshTokenExp > DateTime.Now);

            if (user == null)
            {
                return null;
            }

            var res = GetUserToken(user, refreshModel.RefreshToken);

            return res;
        }

        #endregion

        #region TokenFunction
        public UserVM GetUserToken(Users user, string? refreshToken = null, bool autoSigin = false)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var (token, exp) = AccessToken(authClaims);

            refreshToken ??= StringUtils.GenerateRandomToken();

            var res = new UserVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExp = exp,
                RefreshToken = refreshToken,
                RefreshTokenExp = DateTime.Now.AddDays(1)
            };

            return res;
        }

        private (JwtSecurityToken, DateTime) AccessToken(IEnumerable<Claim> claims)
        {
            var exp = DateTime.Now.AddMinutes(30);
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));
            var creds = new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(_configuration["JWT:ValidIssuer"],
                                             _configuration["JWT:ValidAudience"],
                                             claims,
                                             expires: exp,
                                             signingCredentials: creds);
            return (token, exp);
        }
        #endregion

        #region Oauth function
        public async Task<UserVM> OauthLoginAsync(string accessToken, string provider)
        {
            var res = await OAuthLogin(accessToken, provider);

            return res;
        }

        public async Task<(UserVM, string providerKey)> GetUserInfo(string accessToken, string provider)
        {
            if (!OauthURL.UserInfoUrls.ContainsKey(provider))
            {
                throw new ArgumentException("Invalid OAuth provider", nameof(provider));
            }

            string url = OauthURL.UserInfoUrls[provider];

            using (HttpClient client = new HttpClient())
            {
                if (provider == "twitter" || provider == "github" || provider == "linkedin")
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    if (provider == "github")
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
                    }
                }
                else
                {
                    url = $"{url}&access_token={accessToken}";
                }

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var res = new UserVM();
                    string providerId = "";
                    switch (provider)
                    {
                        case "google":
                            var googleInfo = JsonConvert.DeserializeObject<GoogleUserInoModel>(responseBody);
                            if (googleInfo == null)
                            {
                                throw new Exception("Error retrieving user info from Google");
                            }
                            res.FirstName = googleInfo.first_name;
                            res.LastName = googleInfo.last_name;
                            res.Email = googleInfo.email;
                            providerId = googleInfo.id;
                            break;
                        case "facebook":
                            var facebookInfo = JsonConvert.DeserializeObject<GoogleUserInoModel>(responseBody);
                            if (facebookInfo == null)
                            {
                                throw new Exception("Error retrieving user info from Facebook");
                            }
                            res.FirstName = facebookInfo.first_name;
                            res.LastName = facebookInfo.last_name;
                            res.Email = facebookInfo.email;
                            providerId = facebookInfo.id;
                            break;
                        default:
                            break;
                    }
                    return (res, providerId);
                }
                else
                {
                    throw new Exception($"Error retrieving user info from {provider}");
                }
            }
        }

        public async Task<Users?> CreateUserFromOAuthAsync(UserVM userViewModel, string provider, string oauthId)
        {
            var user = new Users()
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Email = userViewModel.Email,
                UserName = userViewModel.Email
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user");
            }

            var loginInfo = new UserLoginInfo(provider, oauthId, provider);
            var loginResult = await _userManager.AddLoginAsync(user, loginInfo);
            if (!loginResult.Succeeded)
            {
                throw new Exception("Failed to add external login");
            }

            var createdUser = await _userManager.FindByLoginAsync(provider, oauthId);

            return createdUser;
        }

        public async Task<UserVM> OAuthLogin(string accessToken, string provider)
        {
            var (userInfo, oauthId) = await GetUserInfo(accessToken, provider);
            var email = userInfo.Email;

            var user = await _userManager.FindByLoginAsync(provider, oauthId);

            if (user == null)
            {
                user = await CreateUserFromOAuthAsync(userInfo, provider, oauthId);
            }

            if (user != null)
            {
                // Generate JWT token or any other login response
                var token = GetUserToken(user);
                return token;
            }
            else
            {
                throw new Exception("Failed to login");
            }
        }
        #endregion
    }
}
