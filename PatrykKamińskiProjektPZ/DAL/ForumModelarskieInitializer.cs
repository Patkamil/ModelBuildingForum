using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PatrykKamińskiProjektPZ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.DAL
{
    public class ForumModelarskieInitializer : DropCreateDatabaseIfModelChanges<ForumModelarskieContext>
    {

        protected override void Seed(ForumModelarskieContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));

            var user = new ApplicationUser { UserName = "Patkamil@gmail.com" };
            string passwor = "Patkamil123*";
            userManager.Create(user, passwor);
            userManager.AddToRole(user.Id, "Admin");
            var user2 = new ApplicationUser { UserName = "Julia@gmail.com" };
            string passwor2 = "Julia123*";
            userManager.Create(user2, passwor2);
            userManager.AddToRole(user2.Id, "Admin");

            var profiles = new List<Profile>
            {
                 new Profile { Id = 0, UserName = "Patkamil@gmail.com", CreateData = new DateTime(2020, 1, 1, 0, 0, 0)},
                 new Profile { Id = 0, UserName = "Julia@gmail.com", CreateData = new DateTime(2020, 1, 3, 0, 0, 0) }
            };
            profiles.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();

            var models = new List<Model>
            {
                new Model { Name = "T34/85 skala 1:36", Picture = "T34-85 1;36 (2).jpg", Profile = profiles[0], SendDate = new DateTime(2020, 10, 2, 10, 10, 10)}
            };
            models.ForEach(m => context.Models.Add(m));
            context.SaveChanges();

            var modelKoments = new List<ModelKoment>
            {
                new ModelKoment { Model = models[0], Profile = profiles[1], Text = "Ładnie ładnie", SendDate = new DateTime(2020, 10, 2, 11, 10, 10)}
            };
            modelKoments.ForEach(mk => context.ModelKoments.Add(mk));
            context.SaveChanges();

            var articles = new List<Article>
            {
                new Article { Title = "Piosenka Julcia", Text = "Lalala Julcia na sto dwa", Profile = profiles[0], SendDate = new DateTime(2020, 8, 2, 10, 10, 10)},
                new Article { Title = "Piosenka Julcia 2", Text = "Lalala Julcia nfsafsafsafsafsa", Profile = profiles[0], SendDate = new DateTime(2020, 9, 2, 10, 10, 10)},
                new Article { Title = "Jaka jest Julcia?", Text = "Julcia kochana <3", Profile = profiles[0], SendDate = new DateTime(2020, 10, 2, 10, 10, 10)}
            };
            articles.ForEach(a => context.Articles.Add(a));
            context.SaveChanges();

            var articleKoments = new List<ArticleKoment>
            {
                new ArticleKoment { Article = articles[1], Profile = profiles[1], Text = "Dobry ten artykuł" , SendDate = new DateTime(2020, 10, 2, 11, 10, 10)}
            };
            articleKoments.ForEach(ak => context.ArticleKoments.Add(ak));
            context.SaveChanges();

            var ratings = new List<Rating>
            {
                new Rating { Model = models[0], Number = 5, Profile = profiles[0]},
                new Rating { Model = models[0], Number = 4, Profile = profiles[1]}
            };


            ratings.ForEach(r => context.Ratings.Add(r));
            context.SaveChanges();
            //context.Models.Remove(models[0]);
            //context.Models.ElementAt(0).Ratings.Add(ratings[0]);
            //context.Models.ElementAt(0).Ratings.Add(ratings[1]);
            context.Models.First().Ratings = ratings;
            //models.ForEach(m => context.Models.Add(m));
            context.SaveChanges();
        }

    }
}