using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;

namespace Identity.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userServices;
        public AuthController(IUserService userServices)
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
            string? signUpResponse = await _userServices.SignUpAsync(user);
            if (signUpResponse == null)
                return new ConflictObjectResult(new { Response = user.UserName + " " + CommonConstants.HttpResponseMessages.KeyExists });
            else if (signUpResponse?.Length > 0)
                return new OkObjectResult(new { Response = signUpResponse });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpDelete]
        [Authorize]
        [Route("Archive")]
        public async Task<IActionResult> ArchiveAccount()
        {
            if (await _userServices.ArchiveAccount())
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ForbidResult();
        }

        [HttpPut]
        [Authorize]
        [Route("Unarchive/{id}")]
        public async Task<IActionResult> UnArchiveAccount(string id)
        {
            if (await _userServices.UnArchiveAccount(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ForbidResult();
        }

        [HttpDelete]
        [Authorize]
        [Route("Delete/{id}")]
        public async Task<IActionResult> PermanantDelete(string id)
        {
            if (await _userServices.DeleteAccount(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }
        
        [HttpPut]
        [Authorize]
        [Route("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (await _userServices.ResetPassword(id))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpGet]
        [Authorize]
        [Route("GetOwnInformation")]
        public async Task<IActionResult> GetOwnInformation()
        {
            var userData = await _userServices.GetOwnInformation();
            if (userData != null)
                return new OkObjectResult(new { Response = userData });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateOwnInformation")]
        public async Task<IActionResult> UpdateOwnInformation(UserModel user)
        {
            var userData = await _userServices.UpdateOwnInformation(user);
            if (userData != null)
                return new OkObjectResult(new { Response = userData });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdminToOrganization(UserModel user)
        {
            user = await _userServices.RegisterAdminUser(user);
            if (user != null)
                return Ok(new { Response = user });

            return Conflict(new { Response = user });
        }
    }
}
