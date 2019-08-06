using System;
using System.Web.Mvc;
using System.Linq;
using Tasks.Adapter.History;
using Tasks.DataLayer;

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
            ViewBag.Result1 = Convert.ToString(number, 2);
            var historyRow = GetHistoryRow(number.ToString(), ViewBag.Result1, User.Identity.Name, DateTime.Now);
            _historyService.AddHistoryRecord(historyRow);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Task2(int number)
        {
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