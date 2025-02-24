using AttendanceSystem.API.Filters;
using AttendanceSystem.API.Middleware;
using AttendanceSystem.Application;
using AttendanceSystem.Application.Models;
using AttendanceSystem.Persistence;
using AttendanceSystem.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace AttendanceSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers()
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Attendance and Reporting Service", Version = "v1" });
                c.OperationFilter<FileResultContentTypeOperationFilter>();

            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("UnifiedCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "https://harvest-tracker.vercel.app", "https://ministry-activity-tracker.vercel.app")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Run Database Seeder
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    DatabaseSeeder.SeedDatabase(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Custom Exception Handler - Ensure it is early in the pipeline to catch exceptions
            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("UnifiedCorsPolicy");
            app.UseAuthorization(); // Authorization must come after authentication

            app.MapControllers(); // Map controllers after middleware

            app.Run();

        }
    }
}
