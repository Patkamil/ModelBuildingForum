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
    public class ModelKomentController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: ModelKoment
        public ActionResult Index()
        {
            var modelKoments = db.ModelKoments.Include(m => m.Model).Include(m => m.Profile);
            return View(modelKoments.ToList());
        }

        // GET: ModelKoment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelKoment modelKoment = db.ModelKoments.Find(id);
            if (modelKoment == null)
            {
                return HttpNotFound();
            }
            return View(modelKoment);
        }

        // GET: ModelKoment/Create
        public ActionResult Create(int id, string title)
        {
            ViewBag.modelId = id;
            var model = db.Models.Where(a => a.ID == id);
            ViewBag.Name = title;

            //ViewBag.ArticleID = new SelectList(db.Articles, "Id", "Title");
            return View();
        }
        //public ActionResult Create()
        //{
        //    ViewBag.ModelID = new SelectList(db.Models, "ID", "Name");
        //    ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName");
        //    return View();
        //}

        // POST: ModelKoment/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ModelID,Text,SendDate,ProfileID")] ModelKoment modelKoment)
        {
            //if (ModelState.IsValid)
            //{
            //    db.ModelKoments.Add(modelKoment);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.ModelID = new SelectList(db.Models, "ID", "Name", modelKoment.ModelID);
            //ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName", modelKoment.ProfileID);
            //return View(modelKoment);

            try
            {
                if (ModelState.IsValid)
                {

                    modelKoment.SendDate = DateTime.Now;
                    modelKoment.Profile = db.Profiles
                        .Where(p => p.UserName == User.Identity.Name).FirstOrDefault();
                    modelKoment.Model = db.Models
                        .Where(a => a.ID == modelKoment.ModelID).FirstOrDefault();

                    db.ModelKoments.Add(modelKoment);
                    db.SaveChanges();
                    var routeValue = new RouteValueDictionary
                        (new { action = "Details", controller = "Model", id = modelKoment.ModelID });
                    return RedirectToRoute(routeValue);
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            var routeValue2 = new RouteValueDictionary
                             (new { action = "Details", controller = "Model", id = modelKoment.ModelID });
            return RedirectToRoute(routeValue2);
        }

        // GET: ModelKoment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelKoment modelKoment = db.ModelKoments.Find(id);
            if (modelKoment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModelID = new SelectList(db.Models, "ID", "Name", modelKoment.ModelID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName", modelKoment.ProfileID);
            return View(modelKoment);
        }

        // POST: ModelKoment/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ModelID,Text,SendDate,ProfileID")] ModelKoment modelKoment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelKoment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModelID = new SelectList(db.Models, "ID", "Name", modelKoment.ModelID);
            ViewBag.ProfileID = new SelectList(db.Profiles, "Id", "UserName", modelKoment.ProfileID);
            return View(modelKoment);
        }

        // GET: ModelKoment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelKoment modelKoment = db.ModelKoments.Find(id);
            if (modelKoment == null)
            {
                return HttpNotFound();
            }
            return View(modelKoment);
        }

        // POST: ModelKoment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelKoment modelKoment = db.ModelKoments.Find(id);
            db.ModelKoments.Remove(modelKoment);
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
