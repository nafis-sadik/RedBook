using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// Auth Module
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userServices;

        /// <summary>
        /// Authentication Module Constructor
        /// </summary>
        /// <param name="userService">An implementation of IUserService injected by IOC Controller</param>
        public AuthController(IUserService userService)
        {
            _userServices = userService;
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
    }
}
