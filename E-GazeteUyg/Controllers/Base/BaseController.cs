using E_GazeteUyg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_GazeteUyg.Controllers.Base
{
    public class BaseController : Controller
    {
        Dbo_egazeteEntities db = new Dbo_egazeteEntities();

        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();

            ViewData["kayitTablo"] = db.Kayit.ToList();


            //filterContext.Result = new RedirectResult("~/Home/Index");

            base.OnActionExecuting(filterContext);


        }
    }
}