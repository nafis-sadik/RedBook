using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedBook.Core.Models;

namespace Inventory.WebAPI.Controllers.Purchase
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController(IVendorService vendorService) : ControllerBase
    {
        private readonly IVendorService _vendorService = vendorService;

        [HttpPost]
        public async Task<IActionResult> AddAsync(VendorModel model) => Ok(await _vendorService.AddNewAsync(model));

        [HttpPut]
        [Route("{vendorId}")]
        public async Task<IActionResult> UpdateAsync(int vendorId, Dictionary<string, object> model) => Ok(await _vendorService.UpdateAsync(vendorId, model));

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id) => Ok(await _vendorService.GetByIdAsync(id));

        [HttpGet]
        [Route("List/{orgId}")]
        public async Task<IActionResult> GetByOrgId(int orgId) => Ok(await _vendorService.GetByOrgId(orgId));

        [HttpGet]
        [Route("PagedAsync")]
        public async Task<IActionResult> GetPaged([FromQuery]PagedModel<VendorModel> pagedModel, int orgId) => Ok(await _vendorService.GetPagedAsync(pagedModel));
    }
}
