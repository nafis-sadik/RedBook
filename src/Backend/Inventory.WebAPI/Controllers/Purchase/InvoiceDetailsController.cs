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
    [Route("api/PurchaseDetails")]
    public class InvoiceDetailsController(IInvoiceDetailsService invoiceDetailsService) : ControllerBase
    {
        private readonly IInvoiceDetailsService _invoiceDetailsService = invoiceDetailsService;

        //[HttpPost]
        //public async Task<IActionResult> AddAsync(List<InvoiceDetailsModel> model) => Ok(await _invoiceDetailsService.AddNewAsync(model));

        //[HttpGet]
        //[Route("PagedAsync")]
        //public async Task<IActionResult> PagedAsync(PagedModel<InvoiceDetailsModel> pagedModel) => Ok(await _invoiceDetailsService.GetPagedAsync(pagedModel));

        [HttpGet]
        [Route("List/{invoiceId}")]
        public async Task<IActionResult> GetListAsync(int invoiceId) => Ok(await _invoiceDetailsService.GetListAsync(invoiceId));
    }
}
