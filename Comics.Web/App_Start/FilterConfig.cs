using System.Web.Mvc;

using RollbarSharp;

namespace Comics.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RollbarExceptionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }

    public class RollbarExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            (new RollbarClient()).SendException(filterContext.Exception);
        }
    }
}
