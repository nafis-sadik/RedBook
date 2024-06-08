using Inventory.Data.Models;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly IPurchaseInvoiceService _productPurchaseInvoice;

        /// <summary>
        ///  Product Purchase Module Constructor
        /// </summary>
        public PurchaseController(IPurchaseInvoiceService productPurchaseInvoice)
        {
            _productPurchaseInvoice = productPurchaseInvoice;
        }
    }
}
