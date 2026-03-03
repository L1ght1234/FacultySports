using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.HasKey(e => e.Id).HasName("competitions_pkey");
        builder.ToTable("competitions");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Date).HasColumnName("date");
        builder.Property(e => e.LocationId).HasColumnName("location_id");
        builder.Property(e => e.SectionId).HasColumnName("section_id");
        builder.Property(e => e.StartTime).HasColumnName("start_time");
        builder.Property(e => e.StatusId).HasColumnName("status_id");
        builder.Property(e => e.Title).HasMaxLength(255).HasColumnName("title");

        builder.HasOne(d => d.Location).WithMany(p => p.Competitions)
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("competitions_location_id_fkey");

        builder.HasOne(d => d.Section).WithMany(p => p.Competitions)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("competitions_section_id_fkey");

        builder.HasOne(d => d.Status).WithMany(p => p.Competitions)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("competitions_status_id_fkey");

        builder.HasMany(d => d.Participants).WithMany(p => p.Competitions)
            .UsingEntity<Dictionary<string, object>>(
                "CompetitionParticipant",
                r => r.HasOne<Participant>().WithMany()
                    .HasForeignKey("ParticipantId")
                    .HasConstraintName("competition_participant_participant_id_fkey"),
                l => l.HasOne<Competition>().WithMany()
                    .HasForeignKey("CompetitionId")
                    .HasConstraintName("competition_participant_competition_id_fkey"),
                j =>
                {
                    j.HasKey("CompetitionId", "ParticipantId").HasName("competition_participant_pkey");
                    j.ToTable("competition_participant");
                    j.IndexerProperty<int>("CompetitionId").HasColumnName("competition_id");
                    j.IndexerProperty<int>("ParticipantId").HasColumnName("participant_id");
                });
    }
}