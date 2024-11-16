using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IEmployeeService
    {
        public Task<UserModel> RegisterEmployee(int orgId, UserModel user);
        public Task<UserModel> UpdateEmployeeRoles(int orgId, UserModel user);
        public Task<PagedModel<UserModel>> PagedEmployeeList(PagedModel<UserModel> employeeList, int orgId);
        public Task ReleaseEmployee(int userId, int roleId);
        Task<List<OrganizationModel>> AdminOrg();
    }
}
