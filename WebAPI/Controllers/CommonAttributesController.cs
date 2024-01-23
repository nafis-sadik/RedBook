using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Common Attribute Module Constructor
        /// </summary>
        /// <param name="categoryService">An implementation of ICategoryService injected by IOC Controller</param>
        public CommonAttributesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories and subcategories under organization
        /// </summary>
        /// <param name="orgId">Organization Id or unique identifier which is the primary key of the organization</param>
        [HttpGet]
        [Route("{orgId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetAsync(int orgId) => Ok(await _categoryService.GetByOrganizationAsync(orgId));

        /// <summary>
        /// Add new product catagory under organization
        /// </summary>
        /// <param name="category"><see cref="CategoryModel"/></param>
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> AddAsync(CategoryModel category) => Ok(await _categoryService.AddCategoryAsync(category));

        /// <summary>
        /// Update an existing product catagory under organization
        /// </summary>
        /// <param name="category"><see cref="CategoryModel"/></param>
        [HttpPatch]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateAsync(CategoryModel category) => Ok(await _categoryService.UpdateCategoryAsync(category));

        /// <summary>
        /// Remove an existing category by category id
        /// </summary>
        /// <param name="categoryId"><see cref="int"/></param>
        [HttpDelete]
        [Route("{categoryId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RemoveAsync(int categoryId)
        {
            await _categoryService.DeleteCategoryAsync(categoryId);
            return Ok();
        }
    }
}
