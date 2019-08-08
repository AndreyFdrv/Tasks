using System.Collections.Generic;
using Tasks.DataLayer;

namespace Tasks.Adapter.History
{
    public interface IHistoryService
    {
        HistoryDS.HistoryRow CreateRow();
        void AddHistoryRecord(HistoryDS.HistoryRow entity);
        IEnumerable<HistoryDS.HistoryRow> GetHistory();
    }
}