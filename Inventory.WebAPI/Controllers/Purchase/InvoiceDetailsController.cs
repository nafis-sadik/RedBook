using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers.Purchase
{
    /// <summary>
    /// Product Purchase Invoice Details Module
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceDetailsController(IInvoiceDetailsService invoiceDetailsService) : ControllerBase
    {
        private readonly IInvoiceDetailsService _invoiceDetailsService = invoiceDetailsService;

        [HttpPost]
        [Route("Invoice")]
        public async Task<IActionResult> AddAsync(List<InvoiceDetailsModel> model) => Ok(await _invoiceDetailsService.AddNewAsync(model));
    }
}
