using AttendanceSystem.Application.Contracts.Infrastructure;
using AttendanceSystem.Infrastructure.FileExport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceSystem.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExcelExporter, ExcelExporter>();
            services.AddTransient<IPdfExporter, PdfExporter>();
            services.AddTransient<IWordExporter, WordExporter>();

            return services;
        }
    }
}