using System.Collections.Generic;
using Tasks.DataLayer;

namespace Tasks.Adapter.Users
{
    public interface IUsersService
    {
        IEnumerable<UsersDS.UserRolesRow> GetUserRoles();
    }
}