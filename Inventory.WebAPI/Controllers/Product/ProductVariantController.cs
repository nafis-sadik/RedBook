using Inventory.Data.Models.Product;
using Inventory.Domain.Abstraction.Product;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers.Product
{
    /// <summary>
    /// Product Variant Module
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController(IProductVariantService productVariantService) : ControllerBase
    {
        private readonly IProductVariantService _productVariantService = productVariantService;

        /// <summary>
        /// Retrieves list of product variants under specific product
        /// </summary>
        /// <param name="productId">Unique identifier of Product</param>
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetList(int productId) => Ok(await _productVariantService.GetVariantListOfProduct(productId));

        /// <summary>
        /// Adds list of product variants under specific product
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> AddList(IEnumerable<ProductVariantModel> productVariants) => Ok(await _productVariantService.SaveNewVariantsAsync(productVariants));

        /// <summary>
        /// Update specific variant informations
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int VariantId, Dictionary<string, object> productVariant)
        {
            await _productVariantService.UpdateAsync(VariantId, productVariant); 
            return Ok();
        }

        /// <summary>
        /// Soft delete variant information
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int productVariantId)
        {
            await _productVariantService.DeleteAsync(productVariantId);
            return Ok();
        }
    }
}
