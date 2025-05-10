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
        /// <param name="ProductId">Unique identifier of Product</param>
        [HttpGet]
        [Route("{ProductId}")]
        public async Task<IActionResult> GetList(int ProductId) => Ok(await _productVariantService.GetVariantListOfProduct(ProductId));

        /// <summary>
        /// Synchronizes list of product variants under specific product
        /// </summary>
        [HttpPatch]
        [Route("{ProductId}")]
        public async Task<IActionResult> AddList(int ProductId, IEnumerable<ProductVariantModel> productVariants) => Ok(await _productVariantService.SaveNewVariantsAsync(ProductId, productVariants));

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
