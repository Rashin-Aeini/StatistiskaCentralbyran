using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatistiskaCentralbyran.Models.Domains;
using System;

namespace StatistiskaCentralbyran.Models.Configurations
{
    public class PopulationConfiguration : IEntityTypeConfiguration<Population>
    {
        public void Configure(EntityTypeBuilder<Population> builder)
        {
            builder.HasKey(item => item.Id);

            builder.HasIndex(item => new { item.RegionId, item.YearId, item.Gender })
                .IsUnique();

            builder.Property(item => item.Gender)
                .HasConversion(
                value => value.ToString(),
                value => Enum.Parse<Gender>(value)
                );
        }
    }
}
