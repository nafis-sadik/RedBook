using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;

namespace Identity.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        public UserController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn(UserModel user)
        {
            string? logInResponse = await _userServices.LogInAsync(user);

            if (logInResponse == null)
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.UserNotFound });
            else if (logInResponse?.Length > 0)
                return new OkObjectResult(new { Response = logInResponse });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.PasswordMismatched });
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserModel user)
        {
            //UserModel user;
            //using (StreamReader reader = new StreamReader(Request.Body))
            //{
            //    string body = await reader.ReadToEndAsync();
            //    user = JsonSerializer.Deserialize<UserModel>(body);
            //}

            string? signUpResponse = await _userServices.SignUpAsync(user);
            if (signUpResponse == null)
                return new ConflictObjectResult(new { Response = user.UserName + " " + CommonConstants.HttpResponseMessages.KeyExists });
            else if (signUpResponse?.Length > 0)
                return new OkObjectResult(new { Response = signUpResponse });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpDelete]
        [Route("Archive/{id}")]
        public async Task<IActionResult> ArchiveAccount(string id)
        {
            if (await _userServices.ArchiveAccount(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> PermanantDelete(string id)
        {
            if (await _userServices.DeleteAccount(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpPut]
        [Route("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (await _userServices.ResetPassword(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }
    }
}
