using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(e => e.Id).HasName("cities_pkey");
        builder.ToTable("cities");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
    }
}