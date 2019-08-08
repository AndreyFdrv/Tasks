using System.Collections.Generic;
using Tasks.DataLayer;

namespace Tasks.Adapter.History
{
    public class HistoryService : IHistoryService
    {
        private HistoryDS.HistoryDataTable _dt = new HistoryDS.HistoryDataTable();
        private HistoryComponent _component = new HistoryComponent();
        public HistoryDS.HistoryRow CreateRow()
        {
            return _dt.NewHistoryRow();
        }

        public void AddHistoryRecord(HistoryDS.HistoryRow entity)
        {
            _dt.AddHistoryRow(entity);
            _component.Update(_dt);
        }

        public IEnumerable<HistoryDS.HistoryRow> GetHistory()
        {
            _dt = _component.GetAll();

            return _dt;
        }
    }
}