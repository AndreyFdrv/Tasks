using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using Tasks.Web.Models;
using Tasks.Adapter.History;
using Tasks.Adapter.Users;
using Tasks.DataLayer;

namespace Tasks.Controllers
{
    public class InformationController : Controller
    {
        private IHistoryService _historyService;
        private IUsersService _userService;
        public InformationController()
        {
            _historyService = new HistoryService();
            _userService = new UsersService();
        }

        public ActionResult Index()
        {
            var model = new InformationModel();

            IEnumerable<HistoryDS.HistoryRow> history = _historyService.GetHistory();
            model.History = new List<HistoryRecordModel>();
            foreach (var row in history)
            {
                var historyModel = new HistoryRecordModel();
                FillHistoryModel(historyModel, row);
                model.History.Add(historyModel);
            }

            model.UsersRoles = new List<UserRoles>();
            FillUserRoles(model.UsersRoles, _userService.GetUserRoles());

            return View(model);
        }

        private void FillHistoryModel(HistoryRecordModel model, HistoryDS.HistoryRow row)
        {
            model.Input = row.Input;
            model.Output = row.Output;
            model.User = row.User;
            model.Time = row.Time;
        }

        private void FillUserRoles(List<UserRoles> models, IEnumerable<UsersDS.UserRolesRow> rows)
        {
            foreach(var row in rows)
            {
                var model = models.Where(x => x.User == row.UserName).FirstOrDefault();
                if (model == null)
                    models.Add(new UserRoles
                    {
                        User = row.UserName,
                        Roles = row.IsRoleNameNull() ? new List<string>() : new List<string>(new string[] { row.RoleName })
                    });
                else
                    model.Roles.Add(row.RoleName);
            }
        }
    }
}