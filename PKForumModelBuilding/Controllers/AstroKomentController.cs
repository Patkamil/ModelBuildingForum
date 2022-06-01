using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PatrykKamińskiProjektPZ.DAL;
using PatrykKamińskiProjektPZ.Models;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class AstroKomentController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: AstroKoment
        public ActionResult Index()
        {
            var astroKoments = db.AstroKoments.Include(a => a.Astro);
            return View(astroKoments.ToList());
        }

        // GET: AstroKoment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AstroKoment astroKoment = db.AstroKoments.Find(id);
            if (astroKoment == null)
            {
                return HttpNotFound();
            }
            return View(astroKoment);
        }

        // GET: AstroKoment/Create
        public ActionResult Create(int id, string title)
        {
            ViewBag.modelId = id;
            var model = db.Models.Where(a => a.ID == id);
            ViewBag.Name = title;

            //ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: AstroKoment/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AstroID,Text")] AstroKoment astroKoment)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    astroKoment.SendDate = DateTime.Now;
                    astroKoment.Profile = db.Profiles
                        .Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
                    astroKoment.Astro = db.Astroes
                        .Where(a => a.ID == astroKoment.AstroID).FirstOrDefault();

                    db.AstroKoments.Add(astroKoment);
                    db.SaveChanges();
                    var routeValue = new RouteValueDictionary
                        (new { action = "Details", controller = "Astro", id = astroKoment.AstroID });
                    return RedirectToRoute(routeValue);
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            var routeValue2 = new RouteValueDictionary
                             (new { action = "Details", controller = "Astro", id = astroKoment.AstroID });
            return RedirectToRoute(routeValue2);
        }

        // GET: AstroKoment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AstroKoment astroKoment = db.AstroKoments.Find(id);
            if (astroKoment == null)
            {
                return HttpNotFound();
            }
            ViewBag.modelId = new SelectList(db.Astroes, "ID", "Name", astroKoment.AstroID);
            return View(astroKoment);
        }

        // POST: AstroKoment/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AstroID,Text,SendDate")] AstroKoment astroKoment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(astroKoment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.modelId = new SelectList(db.Astroes, "ID", "Name", astroKoment.AstroID);
            return View(astroKoment);
        }

        // GET: AstroKoment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AstroKoment astroKoment = db.AstroKoments.Find(id);
            if (astroKoment == null)
            {
                return HttpNotFound();
            }
            return View(astroKoment);
        }

        // POST: AstroKoment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AstroKoment astroKoment = db.AstroKoments.Find(id);
            db.AstroKoments.Remove(astroKoment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Back(int? id)
        {
            var routeValue = new RouteValueDictionary
                (new { action = "Details", controller = "Astro", id });
            return RedirectToRoute(routeValue);
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
