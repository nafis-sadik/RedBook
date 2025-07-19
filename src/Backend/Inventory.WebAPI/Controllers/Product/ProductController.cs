using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Inventory.Domain.Implementation.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Inventory.WebAPI.Controllers.Product
{
    /// <summary>
    /// Product Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Retrieves paged list of products under organization
        /// </summary>
        /// <param name="orgId">Unique identifier of Organization</param>
        /// <param name="pagedModel">Pagination structure</param>
        [HttpGet]
        [Route("{orgId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(PagedModel<ProductModel>), description: "Retrieves paged list of products under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetPagedAsync([FromQuery] PagedModel<ProductModel> pagedModel, int orgId)
            => Ok(await _productService.GetPagedAsync(pagedModel, orgId));

        /// <summary>
        /// Retrieves list of products under organization for dropdown
        /// </summary>
        /// <param name="orgId">Unique identifier of Organization</param>
        [HttpGet]
        [Route("List/{orgId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<ProductModel>), description: "Retrieves paged list of products under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> GetListByOrgIdAsync(int orgId) => Ok(await _productService.GetListByOrgIdAsync(orgId));

        /// <summary>
        /// Add new product under organization
        /// </summary>
        /// <param name="productModel"><see cref="ProductModel"/></param>
        [HttpPost]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductModel), description: "Add new product under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> AddAsync(ProductModel productModel) => Ok(await _productService.AddNewAsync(productModel));

        /// <summary>
        /// Update an existing product subcategory under organization
        /// </summary>
        /// <param name="productId">Unique identifier of Product</param>
        /// <param name="productModel"><see cref="ProductModel"/></param>
        [HttpPatch]
        [Route("{productId}")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductModel), description: "Update an existing product subcategory under organization")]
        [SwaggerResponse(statusCode: 401, type: typeof(string), description: "Unauthorized Request")]
        [SwaggerResponse(statusCode: 400, type: typeof(string), description: "Requested operation caused an internal error, read message from the response body.")]
        public async Task<IActionResult> UpdateAsync(int productId, Dictionary<string, object> productModel) => Ok(await _productService.UpdateAsync(productId, productModel));

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
