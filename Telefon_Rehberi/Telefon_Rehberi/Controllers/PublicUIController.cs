using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi.ViewModel;

namespace Telefon_Rehberi.Controllers
{
    public class PublicUIController : Controller
    {
        // GET: PublicUI
        public ActionResult Index()
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();

            return View(db.Calisanlar.ToList());
        }


        public ActionResult Detay(int id)
        {
            CalisanModel calisan = new CalisanModel();

            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {

               
                

                var v = (from a in db.Calisanlar
                         join b in db.Departmanlar on a.DepartmanID equals b.DepartmanId
                         join c in db.YöneticiBilgileri on a.YöneticiID equals c.YöneticiId
                         where a.CalisanId.Equals(id) 
                         
                         select new {

                           a.Adi,
                           a.Soyadi,
                           a.Telefon,
                           b.DepartmanAdi,
                           c.YöneticiAdi

                         }
                         ).FirstOrDefault();
                if (v!=null)
                {
                    calisan.Adi = v.Adi;
                    calisan.Soyadi = v.Soyadi;
                    calisan.Telefon = v.Telefon;
                    calisan.Departman = v.DepartmanAdi;
                    calisan.Yonetici = v.YöneticiAdi;
                }

                return View(calisan);
                
            }
           
        }
    }
}