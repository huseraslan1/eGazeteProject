using E_GazeteUyg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_GazeteUyg.Controllers.Base;
namespace E_GazeteUyg.Controllers
{
    [UseAuthorize]

    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            
            if (Request.Cookies["Eposta"]==null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Eposta = Request.Cookies["Eposta"].Value;
            }
            return View();
        }
    }
}