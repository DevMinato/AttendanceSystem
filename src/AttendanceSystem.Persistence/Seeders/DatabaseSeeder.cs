using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AttendanceSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DatabaseSeeder
{
    public static void SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var memberPasswordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Member>>(); // Assuming Member entity is used
        var pastorPasswordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Pastor>>(); // Assuming Pastor entity is used

        // Apply migrations automatically
        context.Database.Migrate();

        // Seed Fellowships
        if (!context.Fellowships.Any())
        {
            context.Fellowships.AddRange(
                new Fellowship
                {
                    Id = Guid.Parse("f62a5d9d-1672-4b68-a69d-8d5f70e5a5ff"),
                    Name = "Somolu Central, Araromi",
                    PastorId = Guid.Parse("d15356cf-a3a4-4fa6-bc78-3e37f76f53f0"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            context.SaveChanges();
        }

        // Seed Activities
        if (!context.Activities.Any())
        {
            context.Activities.AddRange(
                new Activity
                {
                    Id = Guid.Parse("e4b0c5a7-fd23-4891-8949-857317a51f82"),
                    Name = "Sunday Meeting",
                    Type = ActivityType.Attendance,
                    Description = "Regular Sunday Worship Meeting",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Activity
                {
                    Id = Guid.Parse("6f5c82de-2a59-4d58-85e3-c0cf19b1a528"),
                    Name = "Prayer Meeting",
                    Type = ActivityType.Attendance,
                    Description = "Weekly Prayer Meeting",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Activity
                {
                    Id = Guid.Parse("a7d1e8f4-3498-4fb3-b802-56c86dcfa8b6"),
                    Name = "Outreach",
                    Type = ActivityType.Outreach,
                    Description = "Evangelism and community outreach program",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Activity
                {
                    Id = Guid.Parse("9b0e5c74-c5a1-42ad-b96b-2de9a176a0b3"),
                    Name = "Follow-Up",
                    Type = ActivityType.FollowUp,
                    Description = "Follow-up activity to check on new believers",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }

        // Seed Members
        if (!context.Members.Any())
        {
            var hashedPassword1 = memberPasswordHasher.HashPassword(null, "SecurePassword1");
            context.Members.AddRange(
            new Member
            {
                Id = Guid.Parse("c493fb1d-54d8-44d3-8806-4a2ab6d9f02b"),
                MemberType = MemberType.WorkersInTraining,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                PasswordHash = hashedPassword1,
                FellowshipId= Guid.Parse("f62a5d9d-1672-4b68-a69d-8d5f70e5a5ff"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
            );
        }

        // Seed Pastors
        if (!context.Pastors.Any())
        {
            var hashedPassword2 = pastorPasswordHasher.HashPassword(null, "SecurePassword2");
            context.Pastors.AddRange(
            new Pastor
            {
                Id = Guid.Parse("d15356cf-a3a4-4fa6-bc78-3e37f76f53f0"),
                FirstName = "Seun",
                LastName = "Scott",
                Email = "seunainascott2@gmail.com",
                PhoneNumber = "08107774062",
                PasswordHash = hashedPassword2,
                FellowshipId= Guid.Parse("f62a5d9d-1672-4b68-a69d-8d5f70e5a5ff"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
            );
        }        

        context.SaveChanges();
    }
}
