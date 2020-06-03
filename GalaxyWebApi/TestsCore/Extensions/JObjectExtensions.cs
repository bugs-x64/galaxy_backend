using System;
using Newtonsoft.Json.Linq;

namespace TestsCore.Extensions
{
    public static class JObjectExtensions
    {
        /// <summary>
        /// Возвращает значение по указанному JsonPath.
        /// </summary>
        /// <typeparam name="TOutput">Возвращаемый тип.</typeparam>
        /// <param name="jObject"></param>
        /// <param name="caseSensitiveJsonPath">Путь к ключу JSON.(Регистрозависимый)</param>
        public static TOutput GetTokenValue<TOutput>(this JObject jObject, string caseSensitiveJsonPath)
        {
            try
            {
                return jObject.SelectToken(caseSensitiveJsonPath)!.Value<TOutput>();
            }
            catch(ArgumentNullException e)
            {
                throw new Exception($"Не удалось обнаружить значение JToken по пути: {caseSensitiveJsonPath}{Environment.NewLine}Json строка:{Environment.NewLine}{jObject}",e);
            }
        }
    }
}