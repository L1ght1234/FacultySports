using FacultySports.Domain.Entities;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Context;

public partial class SportsDbContext : DbContext
{
    public SportsDbContext(DbContextOptions<SportsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Competition> Competitions { get; set; }
    public virtual DbSet<CompetitionStatus> CompetitionStatuses { get; set; }
    public virtual DbSet<Enrollment> Enrollments { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Participant> Participants { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Section> Sections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}