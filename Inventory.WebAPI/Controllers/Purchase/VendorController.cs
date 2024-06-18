using Inventory.Data.Models.Purchase;
using Inventory.Domain.Abstraction.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetByOrgId(int orgId) => Ok(await _vendorService.GetByOrgId(orgId));
    }
}
