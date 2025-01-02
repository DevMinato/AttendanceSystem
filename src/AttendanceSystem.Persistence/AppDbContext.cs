using AttendanceSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceSystem.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
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
            /*modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasDiscriminator<string>("Discriminator").HasValue<ApplicationUser>("User");
            modelBuilder.Entity<IdentityUser>().ToTable("Users").HasDiscriminator<string>("Discriminator").HasValue<ApplicationUser>("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").HasDiscriminator<string>("Discriminator").HasValue<IdentityRole>("RoleCore");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasDiscriminator<string>("Discriminator").HasValue<IdentityUserRole<string>>("UserRole");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims").HasDiscriminator<string>("Discriminator").HasValue<IdentityUserClaim<string>>("UserClaim");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims").HasDiscriminator<string>("Discriminator").HasValue<IdentityRoleClaim<string>>("RoleClaim");

            modelBuilder.Entity<ApplicationUser>().Property(f => f.UserType).HasConversion<string>().HasMaxLength(50);*/
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
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
