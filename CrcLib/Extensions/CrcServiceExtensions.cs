using CrcLib.Interfaces;
using CrcLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CrcLib.Extensions
{
    /// <summary>
    /// Provides extension methods for setting up CrcLib services in an IServiceCollection.
    /// </summary>
    public static class CrcServiceExtensions
    {
        /// <summary>
        /// Adds the file-based ICrcService to the specified IServiceCollection as a scoped service.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the service to.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddCrcService(this IServiceCollection services)
        {
            services.AddScoped<ICrcService, CrcService>();
            return services;
        }

        /// <summary>
        /// Adds the in-memory IMemoryCrcService to the specified IServiceCollection as a scoped service.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the service to.</param>
        /// <returns>The IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddMemoryCrcService(this IServiceCollection services)
        {
            services.AddScoped<IMemoryCrcService, MemoryCrcService>();
            return services;
        }
    }
}
