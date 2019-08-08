using System.Collections.Generic;
using Tasks.DataLayer;

namespace Tasks.Adapter.Users
{
    public class UsersService : IUsersService
    {
        private UsersDS.UserRolesDataTable _dt = new UsersDS.UserRolesDataTable();
        private UsersComponent _component = new UsersComponent();
        public IEnumerable<UsersDS.UserRolesRow> GetUserRoles()
        {
            _dt = _component.GetUserRoles();

            return _dt;
        }
    }
}