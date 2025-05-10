using Identity.Data.Entities;
using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Constants;
using RedBook.Core.Models;

namespace Identity.WebAPI.Controllers
{
    /// <summary>
    /// User Module
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userServices) : ControllerBase
    {
        private readonly IUserService _userServices = userServices;

        /// <summary>
        /// Archive own account only. User may join a different organization under redbook, thus organization admins should not hold the right to archive an user.
        /// </summary>
        /// <param name="userId">User unique identifier<see cref="int"/>.</param>
        [HttpDelete]
        [Authorize]
        [Route("Archive/{userId}")]
        public async Task<IActionResult> ArchiveAccount(int userId)
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
        public async Task<IActionResult> UnArchiveAccount(int userId)
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
        public async Task<IActionResult> Delete(int userId)
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
        public async Task<IActionResult> ResetPassword(int userId)
        {
            await _userServices.ResetPassword(userId);
            return Ok();
        }

        /// <summary>
        /// Update own user information
        /// </summary>
        /// <param name="user">User details object<see cref="UserModel"/>Updated User Data</param>
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

        /// <summary>
        /// Get list of organizations that the user is related to
        /// </summary>
        [HttpGet]
        [Route("Business")]
        public async Task<IActionResult> GetUserOrgs() => Ok(await _userServices.GetUserOrganizationsAsync());

        /// <summary>
        /// Retrieves a paged list of users associated with the specified organization. (Organization admin access only)
        /// </summary>
        /// <param name="pagedModel">The paged model containing the requested page size and page number.</param>
        /// <param name="orgId">The ID of the organization to retrieve users for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the paged list of users.</returns>
        [HttpGet]
        [Authorize]
        [Route("{orgId}")]
        public async Task<IActionResult> GetUserByBusiness([FromQuery] PagedModel<UserModel> pagedModel, int orgId) => Ok(await _userServices.GetUserByOrganizationId(pagedModel, orgId));
    }
}
