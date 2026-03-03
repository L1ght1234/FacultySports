using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FacultySports.Domain.Entities;

namespace FacultySports.Infrastructure.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(e => e.Id).HasName("sections_pkey");
        builder.ToTable("sections");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        builder.Property(e => e.ScheduleId).HasColumnName("schedule_id");

        builder.HasOne(d => d.Schedule).WithMany(p => p.Sections)
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("sections_schedule_id_fkey");
    }
}