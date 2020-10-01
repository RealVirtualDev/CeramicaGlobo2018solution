using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSite.Models
{
    public class DbModel : DbContext
    {
        public DbSet<PageInfo> PageInfo { get; set; }
        public DbSet<PageFiles> PageFiles { get; set; }
        public DbSet<PageGallery> PageGallery { get; set; }
        public DbSet<PageGallerySetting> PageGallerySetting { get; set; }
        public DbSet<PageModel> PageModel { get; set; }
        public DbSet<PageModelDatas> PageModelDatas { get; set; }

        public DbSet<Resource> Resource { get; set; }
        public DbSet<ResourceModel> ResourceModel { get; set; }
        public DbSet<ResourceGallery> ResourceGallery { get; set; }
        public DbSet<ResourceGallerySetting> ResourceGallerySetting { get; set; }
        public DbSet<ResourceFiles> ResourceFiles { get; set; }

       

        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Adminmenu> Adminmenu { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Impostazioni> Impostazioni { get; set; }
        public DbSet<Emailtemplate> Emailtemplate { get; set; }


        public DbSet<BlocchiHome> BlocchiHome { get; set; }
        public DbSet<MenuTop> MenuTop { get; set; }
        public DbSet<Comunicazione> Comunicazione { get; set; }
        public DbSet<Certificati> Certificati { get; set; }
        public DbSet<Conformita> Conformita { get; set; }
        public DbSet<Prestazione> Prestazione { get; set; }
        public DbSet<SectionSlider> SectionSlider { get; set; }
        public DbSet<RispettoAmbiente> RispettoAmbiente { get; set; }
        public DbSet<Designers> Designers { get; set; }
        public DbSet<Referenze> Referenze { get; set; }
        public DbSet<Cataloghi> Cataloghi { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<NewsProdotto> NewsProdotto { get; set; }
        public DbSet<NewsRassegnaStampa> NewsRassegnaStampa { get; set; }
        public DbSet<NewsEventi> NewsEventi { get; set; }
        public DbSet<NewsComunicatiStampa> NewsComunicatiStampa { get; set; }
        public DbSet<NewsPress> NewsPress { get; set; }
        public DbSet<RivenditeItalia> RivenditeItalia { get; set; }
        public DbSet<Collezioni> Collezioni { get; set; }
        public DbSet<TipologieMenu> TipologieMenu { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Sottocategorie> Sottocategorie { get; set; }
        public DbSet<FinitureGruppi> FinitureGruppi { get; set; }
        public DbSet<Finiture> Finiture { get; set; }
        public DbSet<Prodotti> Prodotti { get; set; }
        public DbSet<Utenti> Utenti { get; set; }
        public DbSet<UtentiNewsletter> UtentiNewsletter { get; set; }
        public DbSet<Transazioni> Transazioni { get; set; }

        public DbSet<tempimport> tempimport { get; set; }


        //public DbSet<Pagine> Pagine { get; set; }


        // public DbSet<LanguageResource> LanguageResource { get; set; }

        // rimuove il plurale automatico delle tabelle del database
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // alternativa all'attribute notation
            //modelBuilder.Entity<PageInfo>()
            //.HasKey(c => new { c.pname });

            //// relazione uno a molti withmany
            //modelBuilder.Entity<PageGallery>()
            //  .HasRequired(p => p.PageInfo)
            //  .WithMany(c => c.gallery)
            //  .HasForeignKey(p => new { p.pname, p.lang });

            //// relazione uno a molti withmany
            //modelBuilder.Entity<ResourceModelDatas>()
            //  .HasRequired(p => p.rname)
            //  .WithMany()
            //  .HasForeignKey(p => new { p.rname });


            //modelBuilder.Entity<Resource>()
            //  .HasMany(p => p.ModelDatas)
            //  .WithMany()
            //  .Map(p => p.MapLeftKey("rname").MapRightKey("rname"));


        }
    }

   
}