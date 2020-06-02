using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TestsCore.Integration
{
    /// <summary>
    /// Базовый класс интеграционных тестов контроллера.
    /// </summary>
    public abstract class ControllerTestsBase<TEntryPoint> where TEntryPoint : class
    {
        private HttpClient _client;

        /// <summary>
        /// Фабрика клиентов тестируемого WepApi.
        /// </summary>
        protected WebApplicationFactory<TEntryPoint> Factory { get; }

        /// <summary>
        /// Клиент тестируемого WepApi.
        /// </summary>
        protected HttpClient Client
        {
            get
            {
                if (_client != null)
                    return _client;

                _client = Factory.CreateClient();
                _client.Timeout = TimeSpan.FromMinutes(10);

                return _client;
            }
        }

        /// <summary>
        /// Шаблон пути до метода. Например: "[Controller]/[Action]"
        /// </summary>
        protected abstract string RouteTemplate { get; }

        /// <summary>
        /// Название контроллера при формированнии ссылки.
        /// </summary>
        protected abstract string Controller { get; }
        
        protected ControllerTestsBase(WebApplicationFactory<TEntryPoint> factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Возвращает относительный путь до метода контроллера.
        /// </summary>
        /// <param name="action">Метод контроллера.</param>
        protected string GetControllerActionPath(string action = null)
        {
            var route = RouteTemplate
                .Replace("[controller]", Controller, StringComparison.InvariantCultureIgnoreCase);

            if (route.Contains("[action]", StringComparison.InvariantCultureIgnoreCase))
                route = route.Replace("[action]", action ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);

            else if (!string.IsNullOrEmpty(action))
                route = route + "/" + action;

            return route.ToLowerInvariant();
        }

        /// <summary>
        /// Возращает нового клиента.
        /// </summary>
        /// <param name="timeout">Таймаут запросов.</param>
        protected HttpClient GetClient(TimeSpan timeout)
        {
            var client = Factory.CreateClient();
            client.Timeout = timeout;

            return client;
        }
    }
}