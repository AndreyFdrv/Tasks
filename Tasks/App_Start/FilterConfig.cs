using System.Web.Mvc;
using Tasks.Web.Filters;

namespace Tasks.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthAttribute());
        }
    }
}
