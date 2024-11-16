using Identity.Data.Models;
using Identity.Domain.Abstraction;
using Identity.Domain.Implementation;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;

        /// <summary>
        /// Register new/existing users to organization as employee
        /// </summary>
        /// <param name="orgId">Unique Identifier/Primary Key of the Organization that the user is going to be employed at</param>
        /// <param name="user">Application Id or unique identifier which is the primary key of the application</param>
        [HttpPost("{orgId}")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> Register(int orgId, UserModel user) => Ok(await _employeeService.RegisterEmployee(orgId, user));

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("Paged/{orgId}")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedModel<UserModel> pagedModel, int orgId) => Ok(await _employeeService.PagedEmployeeList(pagedModel, orgId));

        /// <summary>
        /// Manage roles of employees
        /// </summary>
        [HttpPut("UpdateEmployeeRoles/{orgId}")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateEmployeeRoles(int orgId, UserModel user) => Ok(await _employeeService.UpdateEmployeeRoles(orgId, user));

        /// <summary>
        /// Release an employee from an organization you own or are an admin of
        /// </summary>
        [HttpDelete("{orgId}/{userId}")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task ReleaseEmployee(int orgId, int userId) => await _employeeService.ReleaseEmployee(orgId, userId);

        /// <summary>
        /// Get list of organizations the logged, in user has admin access to
        /// </summary>
        [HttpGet]
        [Route("AdminOrg")]
        public async Task<IActionResult> AdminOf() => Ok(await _employeeService.AdminOrg());
    }
}
