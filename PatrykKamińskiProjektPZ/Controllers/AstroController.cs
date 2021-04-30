using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using PatrykKamińskiProjektPZ.DAL;
using PatrykKamińskiProjektPZ.Models;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class AstroController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: Astro
        public ActionResult Index(int? page)
        {
            int count = 0;
            float srednia = 0;
            List<Astro> astro = db.Astroes.ToList();

            foreach (var astro2 in astro)
            {
                //AvarageRating(model2.ID);
                astro2.RatingsAstro = db.RatingAstroes
                     .Where(r => r.AstroID == astro2.ID).ToList();
                srednia = (AvarageRating(astro2.ID));
                ViewData[count.ToString()] = srednia;
                count++;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(astro.ToPagedList(pageNumber, pageSize));
        }

        public float AvarageRating(int? id)
        {
            Astro model = db.Astroes.Find(id);
            if (model != null)
            {
                model.RatingsAstro = db.RatingAstroes
                    .Where(r => r.AstroID == id).ToList();
                float srednia = 0;
                int count = 0;
                if (model.RatingsAstro != null)
                {
                    foreach (var ocena in model.RatingsAstro)
                    {
                        srednia = ocena.Number + srednia;
                        count++;
                    }
                }
                if (count != 0)
                    srednia = srednia / count;
                return srednia;
            }
            return 0;
        }

        // GET: Astro/Details/5
        public ActionResult Details(int? id)
        {
            if (AvarageRating(id).Equals("0"))
            {
                ViewBag.srednia = "Nie otrzymał jeszcze żadnej oceny.";
            }
            else
            {
                ViewBag.srednia = AvarageRating(id).ToString();
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Astro model = db.Astroes.Find(id);

            var astroKoments = db.AstroKoments
                .Where(a => a.AstroID == id).ToList();
            foreach (var item in astroKoments)
            {
                var profile = db.Profiles
                    .Where(mk => mk.Id == item.ProfileID).FirstOrDefault();
                item.Profile = profile;
            }

            model.RatingsAstro = db.RatingAstroes
                .Where(r => r.AstroID == id).ToList();
            foreach (var item in model.RatingsAstro)
            {
                item.Profile = db.Profiles
                    .Where(p => p.Id == item.ProfileID).FirstOrDefault();
            }

            if (model == null)
            {
                return HttpNotFound();
            }
            model.Profile = db.Profiles.Find(model.ProfileID);
            return View(model);
        }

        // GET: Astro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Astro/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description")] Astro model)
        {
            //string pom = ""; 
            //List<string> pictures = new List<string>(); 
            try
            {
                model.SendDate = DateTime.Now;
                HttpPostedFileBase file = Request.Files["PlikZeZdjeciem"];
                if (file != null && file.ContentLength > 0)
                {
                    model.Picture = System.Guid.NewGuid().ToString() + ".JPEG";
                    //Image.FromFile(file.FileName).Save ("jpg");
                    file.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + model.Picture);
                }

                foreach (var profil in db.Profiles)
                {
                    if (profil.UserName.Equals(User.Identity.Name))
                    {
                        model.Profile = profil;
                    }
                }

                if (ModelState.IsValid)
                {
                    db.Astroes.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(model);
        }

        // GET: Astro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Astro model = db.Astroes.Find(id);
            ViewBag.Data = model.SendDate;
            ViewBag.ProfileId = model.ProfileID;
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Astro/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, ProfileID,Name,Description,SendDate")] Astro model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Astro model2 = db.Astroes.Find(model.ID);
                    // .Where(m => m.ID == model.ID).find;
                    HttpPostedFileBase file = Request.Files["PlikZeZdjeciem"];
                    if (file.FileName != "")
                    {

                        if (file != null && file.ContentLength > 0)
                        {
                            model.Picture = System.Guid.NewGuid().ToString() + ".JPEG";
                            //Image.FromFile(file.FileName).Save ("jpg");
                            if (model.Picture != model2.Picture)
                            {
                                file.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + model.Picture);
                            }
                        }
                    }
                    if (file.FileName == "")
                    {
                        model.Picture = model2.Picture;
                    }
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }

            }
            return View(model);
        }

        // GET: Astro/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Astro model = db.Astroes.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        // POST: Astro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Astro model = db.Astroes.Find(id);
                db.Astroes.Remove(model);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddKoment(int id)
        {
            Astro model = db.Astroes.Find(id);
            string title = model.Name;
            var routeValue = new RouteValueDictionary
                (new { action = "Create", controller = "astroKoment", id, title });
            return RedirectToRoute(routeValue);
        }

        public ActionResult AddRating(int id)
        {
            Astro model = db.Astroes.Find(id);
            string title = model.Name;
            var routeValue = new RouteValueDictionary
                (new { action = "Create", controller = "RatingAstro", id, title });
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
