using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatistiskaCentralbyran.Models.Domains;

namespace StatistiskaCentralbyran.Models.Configurations
{
    public class YearConfiguration : IEntityTypeConfiguration<Year>
    {
        public void Configure(EntityTypeBuilder<Year> builder)
        {
            builder.HasKey(item => item.Id);

            builder.HasIndex(item => item.Number)
                .IsUnique();

            builder.HasMany(item => item.Regions)
                .WithOne(region => region.Year)
                .HasForeignKey(region => region.YearId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
