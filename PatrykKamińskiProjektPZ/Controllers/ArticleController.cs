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
using PagedList;
using System.Web.Routing;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class ArticleController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: Article
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<Article> test = db.Articles.ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var articles = from s in test
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                                       || s.Text.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    articles = articles.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    articles = articles.OrderBy(s => s.SendDate);
                    break;
                case "date_desc":
                    articles = articles.OrderByDescending(s => s.SendDate);
                    break;
                default:
                    articles = articles.OrderBy(s => s.Title);
                    break;
            }


            int pageSize = 5;
            int pageNumber = (page ?? 1);
           //return View(students.ToPagedList(pageNumber, pageSize));
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        // GET: Article/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            var articleKoments = db.ArticleKoments
                .Where(a => a.ArticleID == id).ToList();
            article.ArticleKoments = articleKoments;
            if (article == null)
            {
                return HttpNotFound();
            }
            foreach (var item in articleKoments)
            {
                var profile = db.Profiles
                    .Where(mk => mk.Id == item.ProfileID).FirstOrDefault();
                item.Profile = profile;
            }
            article.Profile = db.Profiles.Find(article.ProfileID);
            return View(article);
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Text")] Article article)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    article.SendDate = DateTime.Now;
                    foreach (var profil in db.Profiles)
                    {
                        if (profil.UserName.Equals(User.Identity.Name))
                        {
                            article.Profile = profil;
                        }
                    }
                    db.Articles.Add(article);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(article);
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            ViewBag.Data = article.SendDate;
            ViewBag.ProfileId = article.ProfileID;
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileId, Id,Title,Text, SendDate")] Article article)
        {
            if (ModelState.IsValid)
            {
                //article.SendDate = db.Articles.Find(article.Id).SendDate;
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddKoment(int id)
        {
            Article article = db.Articles.Find(id);
            var routeValue = new RouteValueDictionary
                (new { action = "Create", controller = "ArticleKoment", id, article.Title});
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
