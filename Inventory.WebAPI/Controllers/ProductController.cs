using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.WebAPI.Controllers
{
    /// <summary>
    /// Product Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Product Module Constructor
        /// </summary>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves paged list of products under organization
        /// </summary>
        /// <param name="orgId">Unique identifier of Organization</param>
        /// <param name="pagedModel">Pagination structure</param>
        [HttpGet]
        [Route("{orgId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductModel), description: "Retrieves paged list of products under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetAsync([FromQuery] PagedModel<ProductModel> pagedModel, int orgId) => Ok(await _productService.GetProductsUnderOrganizationAsync(pagedModel, orgId));

        /// <summary>
        /// Add new product under organization
        /// </summary>
        /// <param name="productModel"><see cref="ProductModel"/></param>
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductModel), description: "Add new product under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> AddAsync(ProductModel productModel) => Ok(await _productService.AddNewProductAsync(productModel));

        /// <summary>
        /// Update an existing product subcategory under organization
        /// </summary>
        /// <param name="productModel"><see cref="ProductModel"/></param>
        [HttpPatch]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductModel), description: "Update an existing product subcategory under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateAsync(ProductModel productModel) => Ok(await _productService.UpdateProductAsync(productModel));

        /// <summary>
        /// Remove an existing product by product id
        /// </summary>
        /// <param name="productId"><see cref="int"/></param>
        [HttpDelete]
        [Route("{productId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "Retrieves Category List Under Organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> RemoveAsync(int productId)
        {
            await _productService.DeleteProductAsync(productId);
            return Ok();
        }
    }
}
