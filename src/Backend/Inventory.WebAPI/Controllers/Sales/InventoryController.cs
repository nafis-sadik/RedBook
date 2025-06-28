using Inventory.Domain.Abstraction.Sales;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController(IInventoryService inventoryService) : ControllerBase
    {
        private readonly IInventoryService _inventoryService = inventoryService;

        [HttpGet]
        [Route("Variant/{variantId}")]
        public async Task<IActionResult> GetAvailableLots(int variantId)
            => Ok(await _inventoryService.GetInventoryOfVariant(variantId));
    }
}
