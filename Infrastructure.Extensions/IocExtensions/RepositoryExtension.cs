using Domain.Interfaces.Repository;
using Infrastructure.Data.ReadRepository;
using Infrastructure.Data.WriteRepository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IDaleReadRepository, DaleReadRepository>();
            services.AddScoped<IDaleWriteRepository, DaleWriteRepository>();

            return services;
        }
    }
}
