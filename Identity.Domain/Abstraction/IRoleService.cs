using Identity.Data.Models;
using RedBook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Abstraction
{
    public interface IRoleService
    {
        Task<RoleModel> AddRoleAsync(RoleModel role);
        Task<RoleModel> GetRoleAsync(int roleId);
        PagedModel<RoleModel> GetRolesAsync(PagedModel<RoleModel> pagedRoleModel);
        Task<RoleModel> UpdateRoleAsync(RoleModel role);
        Task DeleteRoleAsync(int roleId);
    }
}
