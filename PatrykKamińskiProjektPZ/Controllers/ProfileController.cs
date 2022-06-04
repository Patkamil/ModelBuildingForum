using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatrykKamińskiProjektPZ.DAL;
using PatrykKamińskiProjektPZ.Models;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class ProfileController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: Profile
        public ActionResult Index()
        {
            return View(db.Profiles.ToList());
        }

        // GET: Profile/Details/5
        public ActionResult Details(int? id)
        {
            ModelController MC = new ModelController();
            AstroController AC = new AstroController();
            Profile profile = db.Profiles.Find(id);
            int count=0;
            int count2= 0;

            List<Model> models = db.Models
                .Where(a => a.ProfileID == id).ToList();
            profile.Models = models;
            profile.Articles = db.Articles
                .Where(a => a.ProfileID == id).ToList();
            profile.AstroKoments = db.AstroKoments
                .Where(a => a.ProfileID == id).ToList();
            profile.Astros = db.Astroes
                .Where(a => a.ProfileID == id).ToList();
            profile.ArticleKoments = db.ArticleKoments
                .Where(a => a.ProfileID == id).ToList();
            foreach (var model in profile.Models)
            {
                ViewData[count.ToString()] = MC.AvarageRating(model.ID).ToString();
                count++;
            }
            foreach (var model in profile.Astros)
            {
                ViewData["Z " + count2.ToString()] = AC.AvarageRating(model.ID).ToString();
                count2++;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName")] Profile profile)
        {
            profile.CreateData = DateTime.Now;


            if (ModelState.IsValid)
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profile/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
