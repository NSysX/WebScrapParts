using Microsoft.EntityFrameworkCore;
using WebScrapParts.Entities;
using WebScrapParts.Helpers;

namespace WebScrapParts.Infraestructure.Persistence.Contexts
{
    public class WebScrapPartsDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(ConnectionStrings.StringConn_PC22);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AplicacionBanco>().HasKey("Id");
            modelBuilder.Entity<AppProductosScrapy>().HasKey("Id");
            modelBuilder.Entity<AppYearMakeModel>().HasKey("Id");
            modelBuilder.Entity<AppYearMakeModelDetails>().HasKey("Id");
        }

        public DbSet<AplicacionBanco> AplicacionBanco => Set<AplicacionBanco>();
        public DbSet<AppProductosScrapy> AppProductosScrapy => Set<AppProductosScrapy>();
        public DbSet<AppYearMakeModel> AppYearMakeModel => Set<AppYearMakeModel>();
        public DbSet<AppYearMakeModelDetails> AppYearMakeModelDetails => Set<AppYearMakeModelDetails>();
    }
}
