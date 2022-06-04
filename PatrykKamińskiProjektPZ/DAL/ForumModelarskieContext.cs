using PatrykKamińskiProjektPZ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PatrykKamińskiProjektPZ.DAL
{
    public class ForumModelarskieContext : DbContext
    {
        public ForumModelarskieContext() : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleKoment> ArticleKoments { get; set; }
        public DbSet<ModelKoment> ModelKoments { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RatingAstro> RatingAstroes { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModelKoment>().HasRequired<Profile>(mk => mk.Profile)
                .WithMany(p => p.ModelKoments)
                .HasForeignKey(mk => mk.ProfileID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Model>().HasRequired<Profile>(m => m.Profile)
                .WithMany(p => p.Models)
                .HasForeignKey(mk => mk.ProfileID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Article>().HasRequired<Profile>(a => a.Profile)
                .WithMany(p => p.Articles)
                .HasForeignKey(a => a.ProfileID).WillCascadeOnDelete(false);

            modelBuilder.Entity<ArticleKoment>().HasRequired<Profile>(ak => ak.Profile)
                .WithMany(p => p.ArticleKoments)
                .HasForeignKey(ak => ak.ProfileID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Astro>().HasRequired<Profile>(a => a.Profile)
                .WithMany(p => p.Astros)
                .HasForeignKey(a => a.ProfileID).WillCascadeOnDelete(false);

            modelBuilder.Entity<AstroKoment>().HasRequired<Profile>(ak => ak.Profile)
                .WithMany(p => p.AstroKoments)
                .HasForeignKey(ak => ak.ProfileID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Rating>().HasRequired<Profile>(r => r.Profile)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.ProfileID).WillCascadeOnDelete(false);
            modelBuilder.Entity<RatingAstro>().HasRequired<Profile>(r => r.Profile)
                .WithMany(p => p.RatingsAstro)
                .HasForeignKey(r => r.ProfileID).WillCascadeOnDelete(false);


            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<PatrykKamińskiProjektPZ.Models.Astro> Astroes { get; set; }

        public System.Data.Entity.DbSet<PatrykKamińskiProjektPZ.Models.AstroKoment> AstroKoments { get; set; }

        public System.Data.Entity.DbSet<PatrykKamińskiProjektPZ.Models.Album> Albums { get; set; }
    }
}