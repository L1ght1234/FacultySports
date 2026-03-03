using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(e => e.Id).HasName("locations_pkey");
        builder.ToTable("locations");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CityId).HasColumnName("city_id");
        builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");

        builder.HasOne(d => d.City).WithMany(p => p.Locations)
            .HasForeignKey(d => d.CityId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("locations_city_id_fkey");
    }
}
