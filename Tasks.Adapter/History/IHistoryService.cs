using Tasks.DataLayer;

namespace Tasks.Adapter.History
{
    public interface IHistoryService
    {
        HistoryDS.HistoryRow CreateRow();
        void AddHistoryRecord(HistoryDS.HistoryRow entity);
    }
}