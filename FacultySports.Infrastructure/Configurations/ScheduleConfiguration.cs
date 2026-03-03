using FacultySports.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasKey(e => e.Id).HasName("schedules_pkey");
        builder.ToTable("schedules");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
        builder.Property(e => e.EndTime).HasColumnName("end_time");
        builder.Property(e => e.LocationId).HasColumnName("location_id");
        builder.Property(e => e.StartTime).HasColumnName("start_time");

        builder.HasOne(d => d.Location).WithMany(p => p.Schedules)
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("schedules_location_id_fkey");
    }
}