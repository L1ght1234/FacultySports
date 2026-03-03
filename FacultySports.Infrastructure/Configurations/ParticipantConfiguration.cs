using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(e => e.Id).HasName("participants_pkey");
        builder.ToTable("participants");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FirstName).HasMaxLength(100).HasColumnName("first_name");
        builder.Property(e => e.LastName).HasMaxLength(100).HasColumnName("last_name");
        builder.Property(e => e.Phone).HasMaxLength(50).HasColumnName("phone");
    }
}