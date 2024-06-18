using Identity.Data.Models;
using Identity.Domain.Abstraction;
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
        /// <param name="user">Application Id or unique identifier which is the primary key of the application</param>
        [HttpPost]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> Register(UserModel user) => Ok(await _employeeService.RegisterEmployee(user));

        [HttpGet("Paged/{orgId}")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetPaged([FromQuery] PagedModel<UserModel> pagedModel, int orgId) => Ok(await _employeeService.PagedEmployeeList(pagedModel, orgId));

        [HttpPut]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateEmployee(UserModel user) => Ok(await _employeeService.UpdateEmployeeInfo(user));
    }
}
