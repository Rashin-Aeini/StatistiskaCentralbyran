using Microsoft.EntityFrameworkCore;
using StatistiskaCentralbyran.Models.Configurations;
using StatistiskaCentralbyran.Models.Domains;

namespace StatistiskaCentralbyran.Models.Data
{
    public class ApplicationDatabase : DbContext
    {
        public ApplicationDatabase(DbContextOptions<ApplicationDatabase> options) : base(options)
        {

        }

        public DbSet<Year> Years { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Population> Populations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new YearConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new PopulationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
