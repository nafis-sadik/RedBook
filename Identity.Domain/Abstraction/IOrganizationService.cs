using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IOrganizationService
    {
        Task<OrganizationModel> AddOrganizationAsync(OrganizationModel Organization);
        Task<OrganizationModel> GetOrganizationAsync(int OrganizationId);
        Task<PagedModel<OrganizationModel>> GetOrganizationsAsync(PagedModel<OrganizationModel> pagedOrganizationModel);
        Task<OrganizationModel> UpdateOrganizationAsync(OrganizationModel Organization);
        Task DeleteOrganizationAsync(int OrganizationId);
    }
}
