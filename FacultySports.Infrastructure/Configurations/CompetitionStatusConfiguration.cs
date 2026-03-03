using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class CompetitionStatusConfiguration : IEntityTypeConfiguration<CompetitionStatus>
{
    public void Configure(EntityTypeBuilder<CompetitionStatus> builder)
    {
        builder.HasKey(e => e.Id).HasName("competition_statuses_pkey");
        builder.ToTable("competition_statuses");
        builder.HasIndex(e => e.Code, "competition_statuses_code_key").IsUnique();
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Code).HasMaxLength(50).HasColumnName("code");
        builder.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
    }
}