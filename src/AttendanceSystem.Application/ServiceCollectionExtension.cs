using AttendanceSystem.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AttendanceSystem.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}