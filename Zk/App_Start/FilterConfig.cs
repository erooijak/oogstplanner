using System.Web.Mvc;

namespace Zk
{
    static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
            //filters.Add(new RequireHttpsAttribute());
        }
    }
}