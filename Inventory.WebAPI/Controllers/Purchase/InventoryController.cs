using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers.Purchase
{
    /// <summary>
    /// Product Purchase Invoice Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController(IInvoiceService purchaseInvoiceService) : ControllerBase
    {
        private readonly IInvoiceService _productPurchaseInvoice = purchaseInvoiceService;

        [HttpPost]
        public async Task<IActionResult> AddPurchaseInvoice(InvoiceModel model) => Ok(await _productPurchaseInvoice.AddNewAsync(model));

        [HttpGet]
        public async Task<IActionResult> PagedInvoice([FromQuery] PagedInvoiceModel pagedModel) => Ok(await _productPurchaseInvoice.GetPagedAsync(pagedModel));
    }
}
