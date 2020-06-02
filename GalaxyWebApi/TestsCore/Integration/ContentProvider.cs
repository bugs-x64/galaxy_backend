using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace TestsCore.Integration
{
    /// <summary>
    /// Поставщик контента.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContentProvider
    {
        /// <summary>
        /// Кодировка по умолчанию.
        /// </summary>
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Медиа тип по умолчанию для строковых данных.
        /// </summary>
        private const string defaultMediaTypeString = "application/json";

        /// <summary> 
        /// Преобразует <typeparamref name="T"/> в виде Json строки положенную в <see cref="StringContent"/> с кодировкой и типом контента по умолчанию.
        /// </summary>
        /// <param name="data">Данные.</param>
        public StringContent GetJsonStringContent<T>(T data) =>
            GetStringContent(JsonConvert.SerializeObject(data), DefaultEncoding);

        /// <summary>
        /// Возвращает <see cref="StringContent"/> с кодировкой и типом контента по умолчанию.
        /// </summary>
        /// <param name="textData">Данные в виде строки.</param>
        public StringContent GetStringContent(string textData) =>
            GetStringContent(textData, DefaultEncoding);

        /// <summary>
        /// Возвращает <see cref="StringContent"/> с типом контента по умолчанию.
        /// </summary>
        /// <param name="textData">Данные в виде строки.</param>
        /// <param name="encoding">Кодировка.</param>
        public StringContent GetStringContent(string textData, Encoding encoding) =>
            GetStringContent(textData, encoding, defaultMediaTypeString);

        /// <summary>
        /// Возвращает <see cref="StringContent"/>.
        /// </summary>
        /// <param name="textData">Данные в виде строки.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <param name="mediaType">Тип контента.</param>
        public StringContent GetStringContent(string textData, Encoding encoding, string mediaType) =>
            new StringContent(textData, encoding, mediaType);

    }
}
