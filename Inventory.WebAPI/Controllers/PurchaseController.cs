using Inventory.Data.Models;
using Inventory.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Inventory.WebAPI.Controllers
{
    /// <summary>
    /// Product Purchase Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IProductPurchaseInvoice _productPurchaseInvoice;

        /// <summary>
        ///  Product Purchase Module Constructor
        /// </summary>
        public PurchaseController(IProductPurchaseInvoice productPurchaseInvoice)
        {
            _productPurchaseInvoice = productPurchaseInvoice;
        }

        [HttpGet]
        public async Task<IActionResult> PurchaseProduct([FromQuery] PagedPurchaseInvoiceModel pagedModel) => Ok(await _productPurchaseInvoice.GetPagedInvoiceAsync(pagedModel));
    }
}
