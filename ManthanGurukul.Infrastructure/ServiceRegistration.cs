using ManthanGurukul.Application.Interfaces;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Infrastructure.Data;
using ManthanGurukul.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ManthanGurukul.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ManthanGurukulDBContext>(option =>
                     option.UseNpgsql(configuration.GetConnectionString("ManthanGurukulCS"))
            );
            services.AddScoped<IAsyncRepository<User>, AsyncRepository<User>>();
            services.AddScoped<IAsyncRepository<RefreshToken>, AsyncRepository<RefreshToken>>();
            return services;
        }
    }
}
