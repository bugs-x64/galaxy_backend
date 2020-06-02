using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TestsCore.Integration
{
    /// <summary>
    /// Поставщик настроек.
    /// </summary>
    public class SettingsProvider
    {
        /// <summary>
        /// Путь к файлу настроек по умолчанию.
        /// </summary>
        private const string defaultSettingsPath = "settings.json";

        /// <summary>
        /// Возвращает JObject с конфигурацией тестов.
        /// </summary>
        /// <param name="path">Путь к файлу (относительный или абсолютный).</param>
        public JObject GetSettings(string path = defaultSettingsPath)
        {
            // Get the absolute path to the JSON file
            var filePath = Path.IsPathRooted(path)
                ? path
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), path);

            if (!File.Exists(filePath))
                throw new ArgumentException($"Не удается найти файл по пути: {filePath}");

            // Load the file
            var fileData = File.ReadAllText(path);

            return JObject.Parse(fileData);
        }
    }
}