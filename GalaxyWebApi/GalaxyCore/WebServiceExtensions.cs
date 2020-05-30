using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using GalaxyCore;
using GalaxyCore.Contract;
using GalaxyCore.Contracts;
using GalaxyCore.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebServiceExtensions
    {
        public static IServiceCollection AddWebServices(
            this IServiceCollection services,
            IConfigurationSection BLLOptionsSection,
            IConfigurationSection DALOptionSection)
        {
            if (BLLOptionsSection == null)
            {
                throw new ArgumentNullException(nameof(BLLOptionsSection));
            }

            if (DALOptionSection == null)
            {
                throw new ArgumentNullException(nameof(DALOptionSection));
            }

            services.Configure<CarsBLLOptions>(opt =>
            {
                opt.JwtSecretKey = BLLOptionsSection.GetValue<string>("JwtSecretKey");
                opt.WebApiUrl = BLLOptionsSection.GetValue<string>("WebApiUrl");
            });
            services.Configure<CarsMySqlRepositoryOption>(opt =>
            {
                opt.CarsDbConnectionString = DALOptionSection.GetValue<string>("CarsDbConnectionString");
            });

            services.TryAddSingleton<ICarsRepository, CarsRepository>();

            services.TryAddScoped<ICarsService, CarsService>();
            services.TryAddScoped<IJwtTokenService, JwtTokenService>();

            services.AddHttpClient();
            return services;
        }
    }
}
