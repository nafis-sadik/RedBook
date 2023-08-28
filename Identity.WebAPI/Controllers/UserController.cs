using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// User Module
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;

        /// <summary>
        /// User Module Constructor
        /// </summary>
        /// <param name="userServices">An implementation of IUserService injected by IOC Controller</param>
        public UserController(IUserService userServices)
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
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Return JWT token")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RegisterUser(UserModel user) => Ok(await _userServices.RegisterNewUser(user));

        /// <summary>
        /// Archive own account only. User may join a different organization under redbook, thus organization admins should not hold the right to archive an user.
        /// </summary>
        /// <param name="userId">User unique identifier<see cref="string"/>.</param>
        [HttpDelete]
        [Authorize]
        [Route("Archive/{userId}")]
        public async Task<IActionResult> ArchiveAccount(string userId)
        {
            if (await _userServices.ArchiveAccount(userId))
                return new OkObjectResult(new { Response = CommonConstants.HttpResponseMessages.Success });
            else
                return new ForbidResult();
        }

        /// <summary>
        /// Unarchive own account or an user under own organization (admin user only)
        /// </summary>
        /// <param name="userId">Application Id or unique identifier which is the primary key of the application</param>
        [HttpPut]
        [Authorize]
        [Route("Unarchive/{id}")]
        public async Task<IActionResult> UnArchiveAccount(string userId)
        {
            await _userServices.UnArchiveAccount(userId);
            return Ok();
        }

        /// <summary>
        /// Permanantly delete user information (system admin user only)
        /// </summary>
        /// <param name="userId">Application Id or unique identifier which is the primary key of the application</param>
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _userServices.DeleteAccount(userId);
            return Ok();
        }

        /// <summary>
        /// Reset own or user password (system admin user only)
        /// </summary>
        /// <param name="userId">Application Id or unique identifier which is the primary key of the application</param>
        [HttpPut]
        [Authorize]
        [Route("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            await _userServices.ResetPassword(userId);
            return Ok();
        }

        /// <summary>
        /// Update own user information
        /// </summary>
        /// <param name="user">User details object<see cref="UserModel"/>.</param>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UserModel user)
        {
            var userData = await _userServices.UpdateAsync(user);
            if (userData != null)
                return new OkObjectResult(new { Response = userData });
            else
                return new ConflictObjectResult(new { Response = CommonConstants.HttpResponseMessages.Exception });
        }
    }
}
