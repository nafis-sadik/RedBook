using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// User Module
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userServices;

        /// <summary>
        /// User Module Constructor
        /// </summary>
        /// <param name="userServices">An implementation of IUserService injected by IOC Controller</param>
        public AuthController(IUserService userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// Authenticates users
        /// </summary>
        /// <param name="user">User View Model Object</param>
        [HttpPost]
        [Route("LogIn")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Return JWT token")]
        [SwaggerResponse(statusCode: 404, type: typeof(string), description: "User not found")]
        [SwaggerResponse(statusCode: 409, type: typeof(string), description: "Wrong password")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> LogIn(UserModel user)
        {
            string? logInResponse = await _userServices.LogInAsync(user);

            if (logInResponse == null)
                return NotFound(new { Response = CommonConstants.HttpResponseMessages.UserNotFound });
            else if (logInResponse?.Length > 0)
                return Ok(new { Response = logInResponse });
            else
                return Conflict(new { Response = CommonConstants.HttpResponseMessages.PasswordMismatched });
        }

        /// <summary>
        /// Register new users to organization
        /// </summary>
        /// <param name="user">Application Id or unique identifier which is the primary key of the application</param>
        [HttpPost]
        [Route("RegisterUser")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Return JWT token")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RegisterUser(UserModel user) => Ok(await _userServices.RegisterNewUser(user));

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
