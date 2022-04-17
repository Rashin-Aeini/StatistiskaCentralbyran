using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatistiskaCentralbyran.Models.Domains;

namespace StatistiskaCentralbyran.Models.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Name)
                .IsRequired();

            builder.HasIndex(item => item.Name)
                .IsUnique();

            builder.HasMany(item => item.Years)
                .WithOne(year => year.Region)
                .HasForeignKey(year => year.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
