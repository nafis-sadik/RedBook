using Inventory.Data.Models.CRM;
using Inventory.Domain.Abstraction.CRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Inventory.WebAPI.Controllers.CRM
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerServices customerServices) : ControllerBase
    {
        private readonly ICustomerServices _customerServices = customerServices;

        [HttpPost]
        [Route("SearchSimilarNumbers")]
        public async Task<IActionResult> SearchSimilarContactNumberFromPurchaseHistory([FromBody] Dictionary<string, object> payload)
        {
            if (payload.ContainsKey("searchString") && payload.ContainsKey("orgId")) {
                string? searchString = payload["searchString"]?.ToString();
                if (!string.IsNullOrWhiteSpace(searchString) && int.TryParse(payload["orgId"].ToString(), out int orgIdOut))
                    return Ok(await _customerServices.SearchByContactNumberFromPurchaseHistory(new string(searchString), orgIdOut));
            }

            throw new ArgumentException("Payload data must have 'searchString' & 'orgId'");
        }

        [HttpPost]
        [Route("SearchCustomer")]
        public async Task<IActionResult> FindCustomerByContactNumber([FromBody] Dictionary<string, object> payload)
        {
            if (payload.ContainsKey("searchString") && payload.ContainsKey("orgId"))
            {
                string? searchString = payload["searchString"]?.ToString();
                if (!string.IsNullOrWhiteSpace(searchString) && int.TryParse(payload["orgId"].ToString(), out int orgIdOut))
                    return Ok(await _customerServices.FindCustomerByContactNumber(searchString, orgIdOut));
            }

            throw new ArgumentException("Payload data must have 'searchString' & 'orgId'");
        }

        [HttpPost]
        public async Task<IActionResult> SyncCustomerInfo(CustomerModel model)
            => Ok(await _customerServices.SyncCustomerInfoAsync(model));
    }
}
