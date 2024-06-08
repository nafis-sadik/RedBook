using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.WebAPI.Controllers
{
    /// <summary>
    /// Product Subcategory Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoryController(ISubcategoryService subcategoryService) : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService = subcategoryService;

        /// <summary>
        /// Retrieves all categories and subcategories under category
        /// </summary>
        /// <param name="categoryId">Category Id or unique identifier which is the primary key of the organization</param>
        [HttpGet]
        [Route("{categoryId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetAsync(int categoryId) => Ok(await _subcategoryService.GetSubcategoriesUnderCategory(categoryId));

        /// <summary>
        /// Add new product subcategory under category
        /// </summary>
        /// <param name="category"><see cref="CategoryModel"/></param>
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> AddAsync(CategoryModel category) => Ok(await _subcategoryService.AddSubcategoryAsync(category));

        /// <summary>
        /// Update an existing product subcategory under category
        /// </summary>
        /// <param name="category"><see cref="CategoryModel"/></param>
        [HttpPatch]
        [SwaggerResponse(statusCode: 200, type: typeof(CategoryModel), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateAsync(CategoryModel category) => Ok(await _subcategoryService.UpdateSubcategoryAsync(category));

        /// <summary>
        /// Remove an existing category/subcategory by category id
        /// </summary>
        /// <param name="categoryId"><see cref="int"/></param>
        [HttpDelete]
        [Route("{categoryId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RemoveAsync(int categoryId)
        {
            await _subcategoryService.DeleteSubcategoryAsync(categoryId);
            return Ok();
        }
    }
}
