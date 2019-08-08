using System.ComponentModel;
using System;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using Tasks.DataLayer.HistoryDSTableAdapters;

namespace Tasks.DataLayer
{
    public partial class HistoryComponent : Component
    {
        HistoryTableAdapter _ta = new HistoryTableAdapter();
        public HistoryComponent()
        {
            InitializeComponent();
        }

        public HistoryComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public HistoryDS.HistoryDataTable GetAll()
        {
            return _ta.GetData();
        }

        public void Update(HistoryDS.HistoryDataTable data)
        {
            using (var ts = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 2, 0, 0)))
            {
                using (var sqlConnection = new SqlConnection(Settings.Default.ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();

                        _ta.Update(data);

                        ts.Complete();
                    }
                    finally
                    {
                        if (sqlConnection.State != ConnectionState.Closed)
                            sqlConnection.Close();
                    }
                }
            }
        }
    }
}
