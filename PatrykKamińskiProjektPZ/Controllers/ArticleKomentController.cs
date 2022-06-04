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
    public class ArticleKomentController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: ArticleKoment
        public ActionResult Index()
        {
            var articleKoments = db.ArticleKoments.Include(a => a.Article);
            return View(articleKoments.ToList());
        }

        // GET: ArticleKoment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleKoment articleKoment = db.ArticleKoments.Find(id);
            if (articleKoment == null)
            {
                return HttpNotFound();
            }
            return View(articleKoment);
        }

        // GET: ArticleKoment/Create
        public ActionResult Create(int id, string title)
        {
            ViewBag.articleId = id;
            var article = db.Articles.Where(a => a.Id == id);
            ViewBag.Name = title;

            //ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title");
            return View();
        }

        // POST: ArticleKoment/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleID, Id, Text")] ArticleKoment articleKoment)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    
                    articleKoment.SendDate = DateTime.Now;
                    articleKoment.Profile = db.Profiles
                        .Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
                    articleKoment.Article = db.Articles
                        .Where(a => a.Id == articleKoment.ArticleID).FirstOrDefault();

                    db.ArticleKoments.Add(articleKoment);
                    db.SaveChanges();
                    int id = articleKoment.ArticleID;
                    var routeValue = new RouteValueDictionary
                        (new { action = "Details", controller = "Article", id });
                    return RedirectToRoute(routeValue);
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            var routeValue2 = new RouteValueDictionary
                             (new { action = "Details", controller = "Article", id = articleKoment.ArticleID });
            return RedirectToRoute(routeValue2);
        }

        // GET: ArticleKoment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleKoment articleKoment = db.ArticleKoments.Find(id);
            if (articleKoment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title", articleKoment.ArticleID);
            return View(articleKoment);
        }

        // POST: ArticleKoment/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArticleID,Text,SendDate")] ArticleKoment articleKoment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleKoment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title", articleKoment.ArticleID);
            return View(articleKoment);
        }

        // GET: ArticleKoment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleKoment articleKoment = db.ArticleKoments.Find(id);
            if (articleKoment == null)
            {
                return HttpNotFound();
            }
            return View(articleKoment);
        }

        // POST: ArticleKoment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleKoment articleKoment = db.ArticleKoments.Find(id);
            db.ArticleKoments.Remove(articleKoment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Back(int? id)
        {
            var routeValue = new RouteValueDictionary
                (new { action = "Details", controller = "Article", id });
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
