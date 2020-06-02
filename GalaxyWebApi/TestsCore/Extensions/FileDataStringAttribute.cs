using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace TestsCore.Extensions
{
    /// <summary>
    /// Читает содержимое текстового файла
    /// </summary>
    public class FileDataStringAttribute : DataAttribute
    {
        private readonly string _filePath;

        /// <summary>
        /// Читает содержимое текстового файла
        /// </summary>
        /// <param name="filePath">Абсолютный или относительный путь к файлу.</param>
        public FileDataStringAttribute(string filePath)
        {
            _filePath = filePath;
        }

        /// <inheritDoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // Get the absolute path to the JSON file
            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            // Load the file
            var fileData = File.ReadAllText(_filePath);

            return new []{new object[]{fileData}};
        }
    }
}