using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestsCore.Integration
{
    /// <summary>
    /// Класс методов проверки.
    /// </summary>
    public static class Asserts
    {
        /// <summary>
        /// Выполняет проверку успешности выполненного запроса. 
        /// </summary>
        /// <param name="responseMessage">Сообщение ответа сервера.</param>
        public static async Task SuccessStatusCodeAsync(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            
            Assert.True(responseMessage.IsSuccessStatusCode, $"StatusCode: {responseMessage.StatusCode}{Environment.NewLine}Content: {content}");
        }
    }
}