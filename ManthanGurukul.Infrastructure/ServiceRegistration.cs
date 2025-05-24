using ManthanGurukul.Application.Interfaces;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Infrastructure.Data;
using ManthanGurukul.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //services.AddScoped<IAsyncRepository<RefreshToken>, AsyncRepository<RefreshToken>>();
            //services.AddScoped<IAsyncRepository<DetailedDeviceInfo>, AsyncRepository<DetailedDeviceInfo>>();
            //services.AddScoped<IAsyncRepository<VerificationCode>, AsyncRepository<VerificationCode>>();
            //services.AddScoped<IAsyncRepository<ResetPasswordLink>, AsyncRepository<ResetPasswordLink>>();
            return services;
        }
    }
}
