using Inventory.Domain.Abstraction.Sales;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesDetailsController(ISalesDetailsService salesDetailsService) : ControllerBase
    {
        private readonly ISalesDetailsService _salesDetailsService = salesDetailsService; 

        [HttpGet]
        [Route("GetByInvoiceId/{SalesInvouceId}")]
        public async Task<IActionResult> GetBySalesInvoiceId(int SalesInvouceId)
            => Ok(await _salesDetailsService.GetByInvoiceId(SalesInvouceId));
    }
}
