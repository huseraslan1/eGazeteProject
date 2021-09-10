using System;
using E_GazeteUyg.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using E_GazeteUyg.Controllers.Base;
using System.Net.Mail;
using System.Net;

namespace E_GazeteUyg.Controllers
{
    public class LoginController : BaseController
    {
        Dbo_egazeteEntities db = new Dbo_egazeteEntities();

        public string SystemMail { get; private set; }

        public ActionResult Index() // giris
        {
            return View();
        }

        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Giris model, Kayit model2, Gazete data)
        {
            Giris giris = db.Giris.Where(x => x.Eposta == model.Eposta && x.Parola == model.Parola).FirstOrDefault();
            Kayit kayit = db.Kayit.Where(x => x.Eposta == model2.Eposta && x.Parola == model2.Parola).FirstOrDefault();



            if (giris == null && kayit == null)
            {
                ViewBag.mesaj = "Hatalı Giriş";
                return View("Index");
            }
            else
            {
                if (giris != null && giris.Adminmi == true)
                {
                    FormsAuthentication.SetAuthCookie(giris.Eposta, true);

                    HttpCookie cerez = new HttpCookie("Eposta", giris.Eposta);

                    cerez.Expires.AddDays(1);
                    Response.Cookies.Add(cerez);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (kayit.Eposta == model2.Eposta && kayit.Parola == model2.Parola)
                    {
                        Session.Add("mail", model2.Eposta);
                        return RedirectToAction("aksamgazete", "Gazete");
                    }
                }
                return View();
            }
        }



        public ActionResult Cikis()
        {
            Response.Cookies["Eposta"].Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Login");
        }

        public ActionResult Kayit()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Kayit(Kayit data)
        {
            
            data.activationkey = Guid.NewGuid().ToString();
            
            if (data.Parola == data.ParolaTekrar)
            {
                var epostaKontrol = db.Kayit.Where(x=>x.Eposta == data.Eposta).FirstOrDefault();
                try
                {
                    
                    if (epostaKontrol == null)
                    {
                        var uye = db.Kayit.Add(data);
                        db.SaveChanges();
                        MailSender("http://stjgazete.takip.com/Login/Dogrulama?dogrula=", data);
                    }       
                    
                }
                catch (Exception e)
                {

                    return View();
                }

            }
            else
            {
                return View();
            }
            db.SaveChanges();

            return RedirectToAction("Giris", "Login");
        }


        public static void MailSender(string body, Kayit data)
        {
           
            
            var fromAddress = new MailAddress("projeicindeneme84@gmail.com");
            var toAddress = new MailAddress(data.Eposta);
            const string subject = "Akşam E-Gazete | Üyelik Onaylama";
            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "denemeee")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body + data.activationkey})
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
        }
        public ActionResult Dogrulama(string dogrula)
        {
           var kayitTablo = db.Kayit.FirstOrDefault(t=>t.activationkey == dogrula);
            if (kayitTablo != null)
            {
                kayitTablo.Uyelik = true;
                db.SaveChanges();
                
            }

            return RedirectToAction("Giris", "Login");
        }

    }
}