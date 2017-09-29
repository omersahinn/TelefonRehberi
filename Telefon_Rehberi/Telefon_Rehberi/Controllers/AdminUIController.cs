using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telefon_Rehberi;
using Telefon_Rehberi.ViewModel;

namespace Telefon_Rehberi.Controllers
{
    public class AdminUIController : Controller
    {

        public ActionResult Login()
        {
            Admin adminlogin = new Admin();


            return View(adminlogin);
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {


            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {


                var usr = db.Admin.SingleOrDefault(a => a.AdminEposta == admin.AdminEposta && a.AdminSifre == admin.AdminSifre);
                if (usr != null)
                {
                    Session["userID"] = usr.AdminID.ToString();
                    Session["userEposta"] = usr.AdminEposta.ToString();
                    return RedirectToAction("adminAnaSayfa");
                }
                else
                {
                    ModelState.AddModelError("", "eposta veya şifre yanlış");
                }

            }

            return View();

        }

        public ActionResult adminAnaSayfa()
        {


            if (Session["userID"] != null)
            {
                TelefonRehberiEntities db = new TelefonRehberiEntities();
                var calisan = new List<Calisanlar>();
                calisan = db.Calisanlar.ToList();

                return View(calisan);

            }
            else
            {
                return RedirectToAction("Login");
            }


        }
        public ActionResult AdminHomePage()
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            var calisan = new List<Calisanlar>();
            calisan = db.Calisanlar.ToList();

            return View(calisan);

        }

        public ActionResult CalisanEkle()
        {
            List<Calisanlar> calisan = new List<Calisanlar>();
            List<Departmanlar> departman = new List<Departmanlar>();
            List<YöneticiBilgileri> yonetici = new List<YöneticiBilgileri>();
            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {
                departman = db.Departmanlar.OrderBy(a => a.DepartmanAdi).ToList();
                calisan = db.Calisanlar.ToList();
                yonetici = db.YöneticiBilgileri.OrderBy(a => a.YöneticiAdi).ToList();

            }
            ViewBag.DepartmanId = new SelectList(departman, "DepartmanID", "DepartmanAdi");
            ViewBag.YoneticiId = new SelectList(yonetici, "YöneticiID", "YöneticiAdi");

            return View();
        }
        [HttpPost]
        public ActionResult CalisanEkle(Calisanlar calisan)
        {
            if (ModelState.IsValid)
            {


                using (TelefonRehberiEntities db = new TelefonRehberiEntities())
                {
                    db.Calisanlar.Add(calisan);
                    db.SaveChanges();
                }

                return RedirectToAction("AdminHomePage");
            }

            return View();


        }
        public ActionResult Duzenle(int id=0)
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            List<Departmanlar> departman = new List<Departmanlar>();
            List<YöneticiBilgileri> yonetici = new List<YöneticiBilgileri>();

            departman = db.Departmanlar.OrderBy(a => a.DepartmanAdi).ToList();
            yonetici = db.YöneticiBilgileri.OrderBy(a => a.YöneticiAdi).ToList();

            ViewBag.DepartmanId = new SelectList(departman, "DepartmanID", "DepartmanAdi");
            ViewBag.YoneticiId = new SelectList(yonetici, "YöneticiID", "YöneticiAdi");

            Calisanlar calisan = db.Calisanlar.Single(a => a.CalisanId == id);
            return View(calisan);
        }
        [HttpPost]
        public ActionResult Duzenle(Calisanlar c )
        {

            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {
                var v = db.Calisanlar.Where(a => a.CalisanId.Equals(c.CalisanId)).FirstOrDefault();

                if (v!=null)
                {
                    v.Adi = c.Adi;
                    v.Soyadi = c.Soyadi;
                    v.Telefon = c.Telefon;
                    v.DepartmanID = c.DepartmanID;
                    v.YöneticiID = c.YöneticiID;
                }
                db.SaveChanges();
                return RedirectToAction("AdminHomePage");
            }

                
                //Calisanlar calisan = db.Calisanlar.SingleOrDefault(a=>a.CalisanId==c.CalisanId);
                //db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("AdminHomePage");
            
            
            
        }
        public ActionResult sil(int id = 0)
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            Calisanlar calisan = db.Calisanlar.SingleOrDefault(a => a.CalisanId == id);

            if (calisan == null)
            {
                return HttpNotFound();
            }
            return View(calisan);
        }
        [HttpPost, ActionName("sil")]
        public ActionResult silconfirm(int id)
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            Calisanlar calisan = db.Calisanlar.SingleOrDefault(a => a.CalisanId == id);
            db.Calisanlar.Remove(calisan);
            db.SaveChanges();

            return RedirectToAction("adminHomePage");
        }

        public ActionResult SifreDegistir()
        {
            AdminModel admin = new AdminModel();

            return View(admin);
        }
        [HttpPost]
        public ActionResult SifreDegistir(AdminModel model)
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            if (model.YeniSifre != model.SifreDogrula)
            {
                return View(model);
            }
            var mail = GetUser(model.Eposta);
            if (mail)
            {
                var reset = db.Admin.SingleOrDefault(a => a.AdminEposta == model.Eposta);
                reset.AdminSifre = model.YeniSifre;
                db.SaveChanges();
                ModelState.AddModelError("", "Şifre Başarılı Bir Şekilde Değiştirlmiştir");
                return View(model);

            }

            ModelState.AddModelError("", "E-posta veya Şifre Yanlış");

            return View(model);
        }
        public bool GetUser(string user)
        {
            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {
                if (db.Admin.Any(a => a.AdminEposta == user))
                {
                    return true;
                }
                return false;
            }
        }
        public ActionResult departman()
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();

            return View(db.Departmanlar);
        }
        public ActionResult DepartmanDuzenle(int id)
        {
            using (TelefonRehberiEntities db=new TelefonRehberiEntities())
            {
                var departman = db.Departmanlar.Find(id);
                Departmanlar model = new Departmanlar()
                {
                    DepartmanId = departman.DepartmanId,
                    DepartmanAdi = departman.DepartmanAdi
                };
                return View(model);
            }
        }
        [HttpPost]
        [ActionName("DepartmanDuzenle")]
        public ActionResult DepartmanDuzenlee(int id)
        {
            
            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {
                var departman = db.Departmanlar.Single(a => a.DepartmanId == id);
                UpdateModel(departman);
                if (ModelState.IsValid)
                {
                    db.SaveChanges();

                    return RedirectToAction("departman");
                }
                return View(departman);
            }
              
        }

        public ActionResult DepartmanEkle()
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            var departman= db.Departmanlar.ToList();

            return View();

        }
        [HttpPost]
        public ActionResult DepartmanEkle(Departmanlar d)
        {


            using (TelefonRehberiEntities db = new TelefonRehberiEntities())
            {
                db.Departmanlar.Add(d);
                db.SaveChanges();
            }

            return RedirectToAction("departman");

        }
        public ActionResult DepartmanSil(int id)
        {
            TelefonRehberiEntities db = new TelefonRehberiEntities();
            Departmanlar departman = db.Departmanlar.Single(a => a.DepartmanId == id);
           

            if (departman.Calisanlar.Count==0)
            {
                db.Departmanlar.Remove(departman);
                db.SaveChanges();
                return RedirectToAction("departman");
            }
            else
            {


                return View();
            }
              

           

        }
       
       
       
    }
}