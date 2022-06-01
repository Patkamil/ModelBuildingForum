using PatrykKamińskiProjektPZ.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatrykKamińskiProjektPZ.DAL;
using PatrykKamińskiProjektPZ.ViewModels;

namespace PatrykKamińskiProjektPZ.Controllers
{
    public class HomeController : Controller
    {
        private ForumModelarskieContext db = new ForumModelarskieContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            IQueryable<ForumDateGroup> data = from article in db.Articles
                                              group article by article.Profile into dateGroup
                                              select new ForumDateGroup()
                                              {
                                                  Name = dateGroup.Key.UserName,
                                                  Count = dateGroup.Count()
                                              };
            //IQueryable<ForumDateGroup> data2 = from model in db.Models
            //                                  group model by model.Profile into dateGroup
            //                                  select new ForumDateGroup()
            //                                  {
            //                                      Name = dateGroup.Key.UserName,
            //                                      Count = dateGroup.Count()
            //                                  };
            //ViewBag.data2 = data2.ToList();
            //IQueryable<ForumDateGroup> data2 = from model in db.Profiles
            //                                  group model by model. into dateGroup
            //                                  select new ForumDateGroup()
            //                                  {
            //                                      RegistrationDate = dateGroup.Key,
            //                                      Count = dateGroup.Count()
            //                                  };
            //ViewBag.Modele = data2.ToList();
            return View(data.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}