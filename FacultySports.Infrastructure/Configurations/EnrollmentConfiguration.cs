using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id).HasName("enrollments_pkey");
        builder.ToTable("enrollments");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Date)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp without time zone")
            .HasColumnName("date");
        builder.Property(e => e.ParticipantId).HasColumnName("participant_id");
        builder.Property(e => e.SectionId).HasColumnName("section_id");

        builder.HasOne(d => d.Participant).WithMany(p => p.Enrollments)
            .HasForeignKey(d => d.ParticipantId)
            .HasConstraintName("enrollments_participant_id_fkey");

        builder.HasOne(d => d.Section).WithMany(p => p.Enrollments)
            .HasForeignKey(d => d.SectionId)
            .HasConstraintName("enrollments_section_id_fkey");
    }
}