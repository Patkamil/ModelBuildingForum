using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;
using PatrykKamińskiProjektPZ.DAL;
using PatrykKamińskiProjektPZ.Models;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class ModelController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();

        // GET: Model
        public ActionResult Index(int? page)
        {
            int count = 0;
            float srednia = 0;
            List<Model> model = db.Models.ToList();

            foreach (var model2 in model)
            {
                //AvarageRating(model2.ID);
                model2.Ratings = db.Ratings
                     .Where(r => r.ModelID == model2.ID).ToList();
                srednia = (AvarageRating(model2.ID));
                ViewData[count.ToString()] = srednia;
                count++;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        // GET: Model/Details/5
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
            Model model = db.Models.Find(id);

            var modelKoments = db.ModelKoments
                .Where(a => a.ModelID == id).ToList();
            foreach(var item in modelKoments)
            {
                var profile = db.Profiles
                    .Where(mk => mk.Id == item.ProfileID).FirstOrDefault();
                item.Profile = profile;
            }

            model.Ratings = db.Ratings
                .Where(r => r.ModelID == id).ToList();
            foreach (var item in model.Ratings)
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
        public float AvarageRating(int? id)
        {
            Model model = db.Models.Find(id);
            if (model != null)
            {
                model.Ratings = db.Ratings
                    .Where(r => r.ModelID == id).ToList();
                float srednia = 0;
                int count = 0;
                if (model.Ratings != null)
                {
                    foreach (var ocena in model.Ratings)
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

        public void AvarageRating(int id, string number)
        {
            Model model = db.Models.Find(id);
            model.Ratings = db.Ratings
                 .Where(r => r.ModelID == id).ToList();
            float srednia = 0;
            int count = 0;
            foreach (var ocena in model.Ratings)
            {
                srednia = ocena.Number + srednia;
                count++;
            }
            srednia = srednia / count;
            ViewData[number] = srednia;
        }

        // GET: Model/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Model/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description")] Model model)
        {
            //string pom = ""; 
            //List<string> pictures = new List<string>(); 
            try
            {
                model.SendDate = DateTime.Now;
                HttpPostedFileBase mainFile = Request.Files["PlikZeZdjeciem"];
                if (mainFile != null && mainFile.ContentLength > 0)
                {
                    model.Picture = System.Guid.NewGuid().ToString() + ".JPEG";
                    //Image.FromFile(file.FileName).Save ("jpg");
                    mainFile.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + model.Picture);
                }
                if (Request.Files != null)
                {
                    model.Pictures = new List<Album>();

                    for (int i=0; i<Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        string fileName = System.Guid.NewGuid().ToString() + ".JPEG";
                        model.Pictures.Add(new Album
                        {
                            Picture = fileName
                        });
                        file.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + fileName);
                    }
                }

                model.Profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);


                if (ModelState.IsValid)
                {

                    db.Models.Add(model);
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

        // GET: Model/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Model model = db.Models.Find(id);
            ViewBag.Data = model.SendDate;
            ViewBag.ProfileId = model.ProfileID;
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Model/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id)
        {
            Model model = db.Models.Find(id);
            try
            {
                UpdateModel(model);


                HttpPostedFileBase file = Request.Files["PlikZeZdjeciem"];
                if (file.FileName != null)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        model.Picture = System.Guid.NewGuid().ToString() + ".JPEG";
                        //Image.FromFile(file.FileName).Save ("jpg");
                        file.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + model.Picture);
                    }
                    //if (Request.Files.Count != 0)
                    //{
                    //    int i = 0;
                    //    ICollection<Album> B = new List<Album>();
                    //    foreach (var file2 in Request.Files)
                    //    {
                    //        if (i != 0)
                    //        {
                    //            Album A = new Album();
                    //            HttpPostedFileBase file3 = Request.Files[i];
                    //            A.ModelID = model.ID;
                    //            A.Picture = System.Guid.NewGuid().ToString() + ".JPEG";
                    //            if (model.Pictures == null)
                    //            {
                    //                model.Pictures = new List<Album>();
                    //            }

                    //            A.Model = model;
                    //            B.Add(A);
                    //            db.Entry(A).State = EntityState.Modified;
                    //            //db.Albums.Add(A);
                    //            //pictures.Add(pom);
                    //            if (model.Pictures != model2.Pictures)
                    //            {
                    //                file3.SaveAs(HttpContext.Server.MapPath("~/Zdjecia/") + A.Picture);
                    //            }

                    //        }

                    //        i++;
                    //    }
                    //    //model.Pictures = B;
                    //    //if(i != 0)
                    //    //{
                    //    //    model.Pictures.Add = pictures;
                    //    //}
                    //}
                    //if (Request.Files.Count <= 1 && file.FileName != "" || Request.Files.Count == 0)
                    //{
                    //    //model.Pictures = model2.Pictures;
                    //}
                }
                //if(file.FileName == "")
                //{
                //model.Picture = model2.Picture;
                //model.Pictures = model2.Pictures;
                //}
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(model);
        }

        // GET: Model/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Model model = db.Models.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Model model = db.Models.Find(id);
                db.Models.Remove(model);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Album(int id)
        {
            Model model = db.Models.Find(id);
            var routeValue = new RouteValueDictionary
                (new { action = "Index", controller = "Album", id, model.Name });
            return RedirectToRoute(routeValue);
        }

        public ActionResult AddKoment(int id)
        {
            Model model = db.Models.Find(id);
            string title = model.Name;
            var routeValue = new RouteValueDictionary
                (new { action = "Create", controller = "modelKoment", id, title });
            return RedirectToRoute(routeValue);
        }

        public ActionResult AddRating(int id)
        {
            Model model = db.Models.Find(id);
            string title = model.Name;
            var routeValue = new RouteValueDictionary
                (new { action = "Create", controller = "Rating", id, title });
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
