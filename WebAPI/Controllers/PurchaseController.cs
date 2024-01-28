using Inventory.Domain.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
