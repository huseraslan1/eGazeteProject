using E_GazeteUyg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_GazeteUyg.Controllers.Base;
namespace E_GazeteUyg.Controllers
{


    public class MusteriController : BaseController
    {
        Dbo_egazeteEntities db = new Dbo_egazeteEntities();

        public ActionResult Index()
        {
            try
            {
                var liste = db.Kayit.ToList();
                return View(liste);
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Index(Kayit data)
        {
            return View();
        }

        
        public ActionResult Abonelik(int id)
        {
          
            Kayit ky = new Kayit();


            var abonelikguncelleme = db.Kayit.Where(x => x.Id == id).FirstOrDefault();

            ky.Abonelik = abonelikguncelleme.Abonelik;
            ky.Id = abonelikguncelleme.Id;

            return View(ky);
        }

        [HttpPost]
        public ActionResult Abonelik(Kayit data)
        {
            var abonelikguncelleme = db.Kayit.Where(x => x.Id == data.Id).FirstOrDefault();

            abonelikguncelleme.Abonelik = data.Abonelik;

            db.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
