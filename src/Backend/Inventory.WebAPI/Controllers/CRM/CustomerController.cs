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
            if (payload.ContainsKey("contactNumber") && payload.ContainsKey("orgId"))
            {
                string? contactNumber = payload["contactNumber"]?.ToString();
                if (!string.IsNullOrWhiteSpace(contactNumber))
                {
                    if(int.TryParse(payload["orgId"].ToString(), out int orgIdOut))
                    {
                        int orgId = orgIdOut;
                        return Ok(await _customerServices.SearchByContactNumberFromPurchaseHistory(new string(contactNumber), orgId));
                    }
                }
            }

            throw new ArgumentException("Payload data must have 'contactNumber' & 'orgId'");
        }

        [HttpPost]
        [Route("SearchCustomer")]
        public async Task<IActionResult> FindCustomerByContactNumber([FromBody] string contactNumber, int orgId)
            => Ok(_customerServices.FindCustomerByContactNumber(contactNumber, orgId));

        [HttpPost]
        public async Task<IActionResult> SyncCustomerInfo(CustomerModel model) => Ok(await _customerServices.SyncCustomerInfoAsync(model));
    }
}
