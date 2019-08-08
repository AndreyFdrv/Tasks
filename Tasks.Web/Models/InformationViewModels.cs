using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasks.Web.Models
{
    public class HistoryRecordModel
    {
        [Display(Name = "Входные данные")]
        public string Input { get; set; }
        [Display(Name = "Выходные данные")]
        public string Output { get; set; }
        [Display(Name = "Пользователь")]
        public string User { get; set; }
        [Display(Name = "Время")]
        public DateTime Time{ get; set; }
    }
    public class UserRoles
    {
        public string User { get; set; }
        public List<string> Roles { get; set; }
    }
    public class InformationModel
    {
        public List<HistoryRecordModel> History { get; set; }
        public List<UserRoles> UsersRoles { get; set; }
    }
}