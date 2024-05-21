using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.WebAPI.Controllers
{
    /// <summary>
    /// Product Category Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommonAttributesController : ControllerBase
    {
        private readonly ICommonAttributeService _commonAttrService;

        /// <summary>
        /// Common Attribute Module Constructor
        /// </summary>
        /// <param name="commonAttrService">An implementation of ICommonAttributeService injected by IOC Controller</param>
        public CommonAttributesController(ICommonAttributeService commonAttrService)
        {
            _commonAttrService = commonAttrService;
        }

        /// <summary>
        /// Retrieves all common attributes of a particular type
        /// </summary>
        /// <param name="attrType">Organization Id or unique identifier which is the primary key of the organization</param>
        [HttpGet]
        [Route("{attrType}")]
        [SwaggerResponse(statusCode: 200, type: typeof(CommonAttributeModel), description: "Retrieves all common attributes of a particular type")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetAsync(string attrType) => Ok(await _commonAttrService.GetByTypeAsync(attrType));

        /// <summary>
        /// Add new common attribute
        /// </summary>
        /// <param name="commonAttr"><see cref="CommonAttributeModel"/></param>
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(CommonAttributeModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> AddAsync(CommonAttributeModel commonAttr) => Ok(await _commonAttrService.AddCommonAttributeAsync(commonAttr));

        /// <summary>
        /// Update an existing common attribute
        /// </summary>
        /// <param name="category"><see cref="CommonAttributeModel"/></param>
        [HttpPatch]
        [SwaggerResponse(statusCode: 200, type: typeof(CommonAttributeModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateAsync(CommonAttributeModel category) => Ok(await _commonAttrService.UpdateCommonAttributeAsync(category));

        /// <summary>
        /// Remove a common attribute by attribute id
        /// </summary>
        /// <param name="attrId"><see cref="int"/></param>
        [HttpDelete]
        [Route("{attrId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RemoveAsync(int attrId)
        {
            await _commonAttrService.DeleteCommonAttributeAsync(attrId);
            return Ok();
        }
    }
}
