using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace TestsCore.Integration
{
    /// <summary>
    /// Фабрика web приложения.
    /// </summary>
    /// <typeparam name="TStartup">Startup запускаемого приложения.</typeparam>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                //Добавляем свои сеттинги, работает так же как и с обычными. Если чего-то не хватает - добавит, если есть в наличии - заменит.
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Testing.json", true)
                    .Build();

                config.AddConfiguration(integrationConfig);
            });
        }
    }
}