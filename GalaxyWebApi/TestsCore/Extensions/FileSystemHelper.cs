using System;
using System.IO;

namespace TestsCore.Extensions
{
    /// <summary>
    /// Класс вспомогательных методов для работы с файловой системой.
    /// </summary>
    public static class FileSystemHelper
    {
        /// <summary>
        /// Копирует директорию вместе со всем содержимым.
        /// </summary>
        /// <param name="sourceDirName">Путь к источнику.</param>
        /// <param name="destDirName">Путь к получатею.</param>
        /// <param name="copySubDirs">Копировать дочерние дериктории.</param>
        /// <param name="mode">Режим копирования файлов.</param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true, CopyMode mode = CopyMode.CopyIfNewer)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var destinationPath = Path.Combine(destDirName, file.Name);

                if(!File.Exists(destinationPath))
                {
                    file.CopyTo(destinationPath, false);
                    continue;
                }

                switch(mode)
                {
                    case CopyMode.CopyAlways:
                        file.CopyTo(destinationPath, true);
                        break;

                    case CopyMode.CopyIfNewer:
                        var currentFile = File.GetLastWriteTime(destinationPath);
                        var newFile = File.GetLastWriteTime(file.FullName);

                        var isNewerFile = newFile.CompareTo(currentFile) == 1;

                        if(isNewerFile)
                            file.CopyTo(destinationPath, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }


            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs) 
                return;

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true, mode);
            }
        }
    }

    /// <summary>
    /// Режимы копирования файлов.
    /// </summary>
    public enum CopyMode
    {
        /// <summary>
        /// Копировать всегда.
        /// </summary>
        CopyAlways,

        /// <summary>
        /// Копировать, если существующий файл старше копируемого.
        /// </summary>
        CopyIfNewer
    }
}