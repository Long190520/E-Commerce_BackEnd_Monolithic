using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using E_Commerce_BackEnd.Models;
using E_Commerce_BackEnd.Repository;
using E_Commerce_BackEnd.ViewModels;
using System.Net.Http;
using E_Commerce_BackEnd.Services;
using E_Commerce_BackEnd.DAL;
//using E_Commerce_BackEnd.Extensions;

namespace E_Commerce_BackEnd.Controllers
{
    [ApiController]
    public class UserController : GenericController<Users>
    {
        private readonly IUsersRepository _iUsersRepository;
        private readonly UserServices _userServices;
        //private readonly SendMailServices _sendMailServices;

        public UserController(IUnitOfWork unitOfWork, MyDbContext mydb, IUsersRepository iUsersRepository, UserServices userServices/*, SendMailServices sendMailServices*/) : base(unitOfWork)
        {
            _iUsersRepository = iUsersRepository;
            _userServices = userServices;
            //_sendMailServices = sendMailServices;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel signUpModel)
        {
            if (signUpModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userServices.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            if (signInModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userServices.LoginAsync(signInModel);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpPost("signout")]
        public async Task<bool> SignOut([FromBody] UserVM userViewModel)
        {
            if (userViewModel == null || !ModelState.IsValid)
            {
                return false;
            }

            var result = await _userServices.SignOutAsync(userViewModel);

            return result;
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshModel refreshModel)
        {
            if (refreshModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userServices.RefreshAsync(refreshModel);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("oauthLogin")]
        public async Task<IActionResult> oauthLogin([FromBody] OauthSignInModel oauthSignInModel)
        {
            if (oauthSignInModel == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userServices.OauthLoginAsync(oauthSignInModel.AccessToken,oauthSignInModel.Provider);
            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        //[AllowAnonymous]
        //[HttpPost("testSendMail")]
        //public async Task<IActionResult> testSendMail()
        //{
        //    var mailContent = new MailContent()
        //    {
        //        To = "tungxeng56@gmail.com",
        //        Subject = "Test mail service",
        //        Body = "<p>Mail service ASP.NET su dung MailKit<p>"
        //    };

        //    var result = await _sendMailServices.SendMail(mailContent);
        //    if (result == false)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(result);
        //}
    }
}
