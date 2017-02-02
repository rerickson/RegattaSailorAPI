using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegattaSailorAPI.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;

namespace RegattaSailorAPI.DAL
{
    public class RegattaSailorContext : DbContext
    {
        public RegattaSailorContext() : base("RegattaSailorContext")
        {
            Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<YachtModel> Yachts { get; set; }
        public DbSet<DivisionModel> Divisions { get; set; }
        public DbSet<RaceModel> Races { get; set; }
        public DbSet<RaceLegModel> RaceLegs { get; set; }
        public DbSet<LegResultModel> LegResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Entity<YachtModel>()
                .HasMany<DivisionModel>(y => y.Divisions)
                .WithMany(d => d.Yachts)
                .Map(dy =>
                {
                    dy.MapLeftKey("YachtRefId"); 
                    dy.MapRightKey("DivisionRefId");
                    dy.ToTable("DivisionYacht");
                });
        }
    }
}