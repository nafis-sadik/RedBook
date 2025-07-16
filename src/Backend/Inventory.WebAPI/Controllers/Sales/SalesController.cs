using Inventory.Data.Models.Sales;
using Inventory.Domain.Abstraction.Sales;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Inventory.WebAPI.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(ISalesService salesService) : ControllerBase
    {
        private readonly ISalesService _salesService = salesService;

        [HttpPost]
        public async Task<IActionResult> SaveSale(SalesInvoiceModel invoice)
            => Ok(await _salesService.Save(invoice));

        [HttpGet]
        [Route("PagedAsync")]
        public async Task<IActionResult> GetPaged([FromQuery]PagedModel<SalesInvoiceModel> pagedInvoice)
            => Ok(await _salesService.GetPaged(pagedInvoice));
    }
}
