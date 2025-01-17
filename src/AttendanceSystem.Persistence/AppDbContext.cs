using AttendanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = /*_loggedInUserService?.User?.Id ??*/ "SYSTEM";
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = /*_loggedInUserService?.User?.Id ??*/ "SYSTEM";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = /*_loggedInUserService?.User?.Id ??*/ "SYSTEM";
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = /*_loggedInUserService?.User?.Id ??*/ "SYSTEM";
                        break;
                }
            }
            return base.SaveChanges();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One Fellowship can have many Pastors
            modelBuilder.Entity<Pastor>()
                .HasOne(p => p.Fellowship)
                .WithMany()
                .HasForeignKey(p => p.FellowshipId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // One Fellowship can have many Pastors
            modelBuilder.Entity<Member>()
                .HasOne(p => p.Fellowship)
                .WithMany()
                .HasForeignKey(p => p.FellowshipId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Activity>().Property(f => f.Type).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<Pastor>().Property(f => f.Gender).HasConversion<string>().HasMaxLength(10);
            modelBuilder.Entity<Member>().Property(f => f.Gender).HasConversion<string>().HasMaxLength(10);
            modelBuilder.Entity<Member>().Property(f => f.MemberType).HasConversion<string>().HasMaxLength(20);
            modelBuilder.Entity<FollowUpReport>().Property(f => f.FollowUpType).HasConversion<string>().HasMaxLength(20);
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Pastor> Pastors { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityReport> ActivityReports { get; set; }
        public DbSet<Fellowship> Fellowships { get; set; }
        public DbSet<FollowUpReport> FollowUpReports { get; set; }
        public DbSet<FollowUpDetail> FollowUpDetails { get; set; }
        public DbSet<OutreachReport> OutreachReports { get; set; }
        public DbSet<OutreachDetail> OutreachDetails { get; set; }
        public DbSet<AttendanceReport> AttendanceReports { get; set; }
    }
}
