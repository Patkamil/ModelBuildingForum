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
    public class RatingController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: Rating
        public ActionResult Index()
        {
            return View(db.Ratings.ToList());
        }

        // GET: Rating/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Rating/Create
        public ActionResult Create(int id, string title)
        {
            ViewBag.modelId = id;
            var article = db.Models.Where(a => a.ID == id);
            ViewBag.Name = title;
            IEnumerable<int> oceny = new List<int>()
            {
                1, 2, 3, 4, 5, 6
            };


            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "1", Value = "1" });

            items.Add(new SelectListItem { Text = "2", Value = "2" });

            items.Add(new SelectListItem { Text = "3", Value = "3"});

            items.Add(new SelectListItem { Text = "4", Value = "4" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            //oceny.Add(1); oceny.Add(2); oceny.Add(3); oceny.Add(4); oceny.Add(5); oceny.Add(6);
            ViewBag.ocena = items;
            //ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: Rating/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ModelID,ID,Number")] Rating rating)
        {
            //try
            //{
            //    foreach (var profil in db.Profiles)
            //    {
            //        if (profil.UserName.Equals(User.Identity.Name))
            //        {
            //            rating.Profile = profil;
            //        }
            //    }
            //    if (ModelState.IsValid)
            //    {
            //        db.Ratings.Add(rating);
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //}
            //catch (DataException /* dex */)
            //{
            //    //Log the error (uncomment dex variable name and add a line here to write a log.
            //    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            //}

            //return View(rating);

            try
            {
                rating.Model = db.Models
                      .Where(a => a.ID == rating.ModelID).FirstOrDefault();
                if (ModelState.IsValid)
                {

                    rating.Profile = db.Profiles
                        .Where(p => p.UserName == User.Identity.Name).FirstOrDefault();


                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    var routeValue = new RouteValueDictionary
                        (new { action = "Details", controller = "Model", id = rating.ModelID });
                    return RedirectToRoute(routeValue);
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(rating);
        }

        // GET: Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Rating/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Number")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: Rating/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Back(int? id)
        {
            var routeValue = new RouteValueDictionary
                (new { action = "Details", controller = "Model", id });
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
