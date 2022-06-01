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
    public class RatingAstroController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: RatingAstro
        public ActionResult Index()
        {
            var ratingAstroes = db.RatingAstroes.Include(r => r.Astro).Include(r => r.Profile);
            return View(ratingAstroes.ToList());
        }

        // GET: RatingAstro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingAstro ratingAstro = db.RatingAstroes.Find(id);
            if (ratingAstro == null)
            {
                return HttpNotFound();
            }
            return View(ratingAstro);
        }

        // GET: RatingAstro/Create
        public ActionResult Create(int id, string title)
        {
            ViewBag.modelId = id;
            var article = db.Astroes.Where(a => a.ID == id);
            ViewBag.Name = title;
            IEnumerable<int> oceny = new List<int>()
            {
                1, 2, 3, 4, 5, 6
            };


            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1", Value = "1" });

            items.Add(new SelectListItem { Text = "2", Value = "2" });

            items.Add(new SelectListItem { Text = "3", Value = "3" });

            items.Add(new SelectListItem { Text = "4", Value = "4" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            //oceny.Add(1); oceny.Add(2); oceny.Add(3); oceny.Add(4); oceny.Add(5); oceny.Add(6);
            ViewBag.ocena = items;
            //ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: RatingAstro/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Number,ProfileID,AstroID")] RatingAstro ratingAstro)
        {
            try
            {
                ratingAstro.Astro = db.Astroes
                      .Where(a => a.ID == ratingAstro.AstroID).FirstOrDefault();
                if (ModelState.IsValid)
                {

                    ratingAstro.Profile = db.Profiles
                        .Where(p => p.UserName == User.Identity.Name).FirstOrDefault();


                    db.RatingAstroes.Add(ratingAstro);
                    db.SaveChanges();
                    var routeValue = new RouteValueDictionary
                        (new { action = "Details", controller = "Astro", id = ratingAstro.AstroID });
                    return RedirectToRoute(routeValue);
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(ratingAstro);
        }

        // GET: RatingAstro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingAstro ratingAstro = db.RatingAstroes.Find(id);
            if (ratingAstro == null)
            {
                return HttpNotFound();
            }
            ViewBag.AstroID = new SelectList(db.Astroes, "ID", "Name", ratingAstro.AstroID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName", ratingAstro.ProfileID);
            return View(ratingAstro);
        }

        // POST: RatingAstro/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Number,ProfileID,AstroID")] RatingAstro ratingAstro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratingAstro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AstroID = new SelectList(db.Astroes, "ID", "Name", ratingAstro.AstroID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName", ratingAstro.ProfileID);
            return View(ratingAstro);
        }

        // GET: RatingAstro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingAstro ratingAstro = db.RatingAstroes.Find(id);
            if (ratingAstro == null)
            {
                return HttpNotFound();
            }
            return View(ratingAstro);
        }

        // POST: RatingAstro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatingAstro ratingAstro = db.RatingAstroes.Find(id);
            db.RatingAstroes.Remove(ratingAstro);
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
