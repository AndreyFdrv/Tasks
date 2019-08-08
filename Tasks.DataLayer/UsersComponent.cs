using System.ComponentModel;
using System;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using Tasks.DataLayer.UsersDSTableAdapters;

namespace Tasks.DataLayer
{
    public partial class UsersComponent : Component
    {
        UserRolesTableAdapter _taUserRoles = new UserRolesTableAdapter();
        public UsersComponent()
        {
            InitializeComponent();
        }

        public UsersDS.UserRolesDataTable GetUserRoles()
        {
            return _taUserRoles.GetData();
        }

        public UsersComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}