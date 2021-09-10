using E_GazeteUyg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using E_GazeteUyg.Controllers.Base;
using System.Configuration;
using PagedList;
using System.Web.UI;
using System.Data.Entity;

namespace E_GazeteUyg.Controllers
{

    public class GazeteController : BaseController
    {

        Dbo_egazeteEntities db = new Dbo_egazeteEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Ekle()
        {
            ViewBag.gazete = db.GazeteEk.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Ekle(HttpPostedFileBase Image, HttpPostedFileBase Pdf, Gazete gelenGZT)
        {

            if (Request.Files.Count > 0)
            {

                var fi = new FileInfo(Image.FileName);
                var fileName = Path.GetFileName(Image.FileName);
                fileName = Guid.NewGuid().ToString() + fi.Extension;
                var path = Path.Combine(Server.MapPath("/img/photo"), fileName);
                Image.SaveAs(path);

                var pdffi = new FileInfo(Pdf.FileName);
                var pdffileName = Path.GetFileName(Pdf.FileName);
                pdffileName = Guid.NewGuid().ToString() + pdffi.Extension;
                var pdfpath = Path.Combine(Server.MapPath("/pdf"), pdffileName);
                Pdf.SaveAs(pdfpath);

                Gazete model = new Gazete();
                model.Adi = gelenGZT.Adi;
                model.Tarih = gelenGZT.Tarih;
                model.Pdf = pdffileName;
                model.Image = fileName;

                db.Gazete.Add(model);

                db.SaveChanges();

            }
            return RedirectToAction("Ekle");
        }

        [HttpGet]
        public ActionResult aksamgazete(int? gazeteID, int page =1)
        {
           
            ViewBag.gazeteID = "gazeteID="+gazeteID;
            var mail = Session["mail"];
  
            if (Session["mail"] == null)
            {
                return RedirectToAction("Kayit", "Login");
            }
            
            var ky = db.Kayit.Where(x => x.Eposta == mail.ToString()).FirstOrDefault();
            if (ky.Uyelik == null || ky.Uyelik == false)
            {
                return RedirectToAction("Index", "Login");
            }
            if (gazeteID == null)
            {
                gazeteID = 1;
                if (ky.abonelikBitis < DateTime.Now)
                {
                    Session["AboneIzin"] = false;
                    ky.Abonelik = false;
                    ky.abonelikBitis = null;
                    ky.AboneTarih = null;
                }
                db.SaveChanges();
                if (ky == null)
                {
                    Session["AboneIzin"] = true;

                }
                else
                {
                    Session["AboneIzin"] = ky.Abonelik;
                    if (Session["AboneIzin"] == null)
                    {
                        return RedirectToAction("Index", "Login");

                    }
                }
            }

            var result = (from gazete in db.Gazete
                          join gazeteEk in db.GazeteEk on gazete.IsimId equals gazeteEk.Id
                          select new Models.DTO.HomeDTO
                          {
                              gazeteAdi = gazete.Adi,
                              gazeteEkID = gazeteEk.Id,
                              gazeteID = gazete.Id,
                              gazeteImage = gazete.Image,
                              gazetePdf = gazete.Pdf,
                              gazeteTarih = gazete.Tarih,
                              gazeteEkAdi = gazeteEk.Adi
                          }).Where(x => x.gazeteEkID == gazeteID).ToList().ToPagedList(page, 8);
            
            if (ky.abonelikBitis < DateTime.Now)
                {
                    Session["AboneIzin"] = false;
                    ky.Abonelik = false;
                    ky.abonelikBitis = null;
                }
                db.SaveChanges();
                if (ky == null)
                {
                    Session["AboneIzin"] = true;

                }
                else
                {
                    Session["AboneIzin"] = ky.Abonelik;
                    if (Session["AboneIzin"] == null)
                    {
                        return RedirectToAction("Kayit", "Login");

                    }
                }


                Pager pager = new Pager(result.Count(),page, 1);

            Session["Pager"] = pager;
                if ((bool)Session["AboneIzin"] == true)
                {
                    return View(result);
                }
                else
                {
                    Session["AboneIzin"] = false;
                }
      
                return View(result);

            

            


        }

        [HttpPost]
        public ActionResult aksamgazete(Gazete model)
        {
            var tarihlistele = db.Gazete.Where(x => x.Tarih == model.Tarih).Select(x => new Models.DTO.HomeDTO
            {
                gazeteAdi = x.Adi,
                gazeteEkID = x.Id,
                gazeteID = x.Id,
                gazeteImage = x.Image,
                gazetePdf = x.Pdf,
                gazeteTarih = x.Tarih,
                gazeteEkAdi = x.Adi

            }).ToList().ToPagedList(1,8);
            if (tarihlistele.Count == 0)
            {
                return RedirectToAction("aksamgazete", "Gazete");
            }
            else
            {             
                return View(tarihlistele);
            }

            
        }



        public ActionResult satinAl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult satinAl(Kayit model)
        {
            var mail = Session["mail"];
            Kayit kayit = db.Kayit.Where(x => x.Eposta == mail.ToString()).FirstOrDefault();

            if (kayit.Abonelik == false)
            {
                kayit.Abonelik = true;
            }
            else
            {
                Session["AboneIzin"] = true;


            }


            if (kayit.AboneTarih != null)
            {
                var oncekiAbonelik = kayit.abonelikBitis - DateTime.Now;
                var yeniAbonelikTarih = model.abonelikBitis.Value.Add((TimeSpan)oncekiAbonelik);
                kayit.abonelikBitis = yeniAbonelikTarih;
            }
            else
            {
                kayit.abonelikBitis = model.abonelikBitis;
            }

            kayit.AboneTarih = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("aksamgazete", "Gazete");

        }

        public ActionResult GazeteDetay(int id)
        {
            var Gzt = db.Gazete.FirstOrDefault(t=>t.Id == id);
            return View(Gzt);
        }


    }
}
