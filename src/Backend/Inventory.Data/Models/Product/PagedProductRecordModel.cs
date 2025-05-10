using RedBook.Core.Models;

namespace Inventory.Data.Models.Product
{
    public class PagedProductRecordModel : PagedModel<ProductModel>
    {
        public int OrganizationId { get; set; }
    }
}
