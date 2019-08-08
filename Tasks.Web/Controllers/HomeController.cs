using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Tasks.Adapter.History;
using Tasks.DataLayer;
using Tasks.Web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Tasks.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHistoryService _historyService;
        public HomeController()
        {
            _historyService = new HistoryService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Task1(int number)
        {
            if (number < 0 || number > 2048)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Число должно быть от 0 до 2048" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Result1 = Convert.ToString(number, 2);
            var historyRow = GetHistoryRow(number.ToString(), ViewBag.Result1, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Task2(int number)
        {
            if (number < 0 || number > 2048)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Число должно быть от 0 до 2048" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Result2 = Convert.ToString(number, 16);
            var historyRow = GetHistoryRow(number.ToString(), ViewBag.Result2, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        private void Sort1(ref int[] array)
        {
            array = array.OrderBy(x => x).ToArray();
        }

        private void Sort2(ref int[] array)
        {
            //пузырьковая сортировка
            for (int j = 0; j <= array.Length - 2; j++)
            {
                for (int i = 0; i <= array.Length - 2; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                    }
                }
            }
        }

        private int Partition(int[] array, int left, int right)
        {
            int pivot = array[left];
            while (true)
            {
                while (array[left] < pivot)
                    left++;

                while (array[right] > pivot)
                    right--;

                if (left < right)
                {
                    if (array[left] == array[right])
                        return right;

                    int temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                }
                else
                    return right;
            }
        }

        private void Sort3(ref int[] array, int left, int right)
        {
            //быстрая сортировка
            if (left < right)
            {
                int pivot = Partition(array, left, right);

                if (pivot > 1)
                    Sort3(ref array, left, pivot - 1);
                if (pivot + 1 < right)
                    Sort3(ref array, pivot + 1, right);
            }
        }

        [HttpPost]
        public ActionResult Task3(int number)
        {
            if (number < 1 || number > 2048)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Число должно быть от 1 до 2048" }, JsonRequestBehavior.AllowGet);
            }
            Random randNum = new Random();
            int[] array = Enumerable.Repeat(0, number).Select(i => randNum.Next(0, 2048)).ToArray();
            Sort1(ref array);
            Sort2(ref array);
            Sort3(ref array, 0, array.Length-1);
            ViewBag.Result3 = array.Where(x => x % 2 == 0).Count();

            var historyRow = GetHistoryRow(number.ToString(), ViewBag.Result3, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        public ActionResult Task4(int number)
        {
            if (number < 0 || number > 30)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Число должно быть от 1 до 30" }, JsonRequestBehavior.AllowGet);
            }
            int[] array = new int[number];
            if (array.Length > 0)
                array[0] = 1;
            if (array.Length > 1)
                array[1] = 1;
            for (int i = 2; i < array.Length; i++)
                array[i] = array[i - 1] + array[i - 2];
            ViewBag.Result4 = array.Where(x => x % 2 == 0).Sum();

            var historyRow = GetHistoryRow(number.ToString(), ViewBag.Result4, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        public async Task<ActionResult> Task5(string input)
        {
            var inputParts = input.Split('/').ToList();
            if(inputParts.Count < 2)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Отсутвует разделитель \"/\"" }, JsonRequestBehavior.AllowGet);
            }
            var login = inputParts[0];
            if(string.IsNullOrEmpty(login))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Не был указан логин" }, JsonRequestBehavior.AllowGet);
            }
            inputParts.RemoveAt(0);
            var password = string.Join("/", inputParts);
            if (string.IsNullOrEmpty(login))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Не был указан пароль" }, JsonRequestBehavior.AllowGet);
            }

            var user = new ApplicationUser { UserName = login };
            var result = await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().CreateAsync(user, password);

            ViewBag.Result5 = password.Length.ToString();

            var historyRow = GetHistoryRow(input, ViewBag.Result5, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        private string GetRoleByLetter(string letter)
        {
            switch(letter)
            {
                case "а":
                    return "Task1";
                case "б":
                    return "Task2";
                case "в":
                    return "Task3";
                case "г":
                    return "Task4";
                case "д":
                    return "Task5";
                case "е":
                    return "Task6";
                case "ё":
                    return "Task7";
                default:
                    return null;
            }
        }

        public async Task<ActionResult> Task6(string input)
        {
            var inputParts = input.Split('/');
            if (inputParts.Length < 2)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Отсутвует разделитель \"/\"" }, JsonRequestBehavior.AllowGet);
            }
            var login = inputParts[0];
            if (string.IsNullOrEmpty(login))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Не был указан логин" }, JsonRequestBehavior.AllowGet);
            }
            var newRoles = string.IsNullOrEmpty(inputParts[1]) ? new List<string>() 
                : inputParts[1].Split(';').Select(x => GetRoleByLetter(x)).ToList();
            if(newRoles.Contains(null))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Неверный формат прав доступа" }, JsonRequestBehavior.AllowGet);
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByNameAsync(login);

            var oldRoles = await userManager.GetRolesAsync(user.Id);
            foreach(var role in oldRoles)
            {
                if (!newRoles.Contains(role))
                    await userManager.RemoveFromRoleAsync(user.Id, role);
            }

            foreach (var role in newRoles)
            {
                if (!oldRoles.Contains(role))
                    await userManager.AddToRoleAsync(user.Id, role);
            }

            ViewBag.Result6 = newRoles.Count.ToString();

            var historyRow = GetHistoryRow(input, ViewBag.Result6, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        private string ReverseWords(string input)
        {
            var stringBuilder = new StringBuilder(input);
            int i = 0;
            while (i < stringBuilder.ToString().Length)
            {
                while (!Char.IsLetter(stringBuilder[i]))
                {
                    i++;
                    if (i >= stringBuilder.ToString().Length)
                        return stringBuilder.ToString();
                }
                int wordBeginIndex = i;
                int wordLength = 0;
                while (i< stringBuilder.ToString().Length && Char.IsLetter(stringBuilder[i]))
                {
                    i++;
                    wordLength++;
                }
                var reversedWord = new String(stringBuilder.ToString().Substring(wordBeginIndex, wordLength).Reverse().ToArray());
                stringBuilder.Remove(wordBeginIndex, wordLength);
                stringBuilder.Insert(wordBeginIndex, reversedWord);
            }
            return stringBuilder.ToString();
        }

        public ActionResult Task7(string input)
        {
            ViewBag.Result7 = ReverseWords(input);

            var historyRow = GetHistoryRow(input, ViewBag.Result7, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        private HistoryDS.HistoryRow GetHistoryRow(string input, string output, string login, DateTime time)
        {
            var row = _historyService.CreateRow();
            row.Input = input;
            row.Output = output;
            row.User = login;
            row.Time = time;
            return row;
        }
    }
}