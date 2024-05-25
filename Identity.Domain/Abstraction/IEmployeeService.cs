using Identity.Data.Models;
using RedBook.Core.Models;

namespace Identity.Domain.Abstraction
{
    public interface IEmployeeService
    {
        public Task<UserModel> RegisterEmployee(UserModel user);
        public Task<UserModel> UpdateEmployeeInfo(UserModel user);
        public Task<PagedModel<UserModel>> PagedEmployeeList(PagedModel<UserModel> employeeList, int orgId);
        public Task ResignEmployee(int userId, int roleId);
    }
}
