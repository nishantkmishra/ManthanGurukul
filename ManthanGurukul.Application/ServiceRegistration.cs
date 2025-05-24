using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ManthanGurukul.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
