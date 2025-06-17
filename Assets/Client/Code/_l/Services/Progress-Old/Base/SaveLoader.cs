using System;
using System.IO;
using UnityEngine;

namespace ClientCode.Services.Progress.Base
{
    public static class SaveLoader
    {
        public static SaveLoaderResultType Save<T>(string path, T data) where T : ISavedData
        {
            var isPathValid = IsValidSavePath(path);

            if (isPathValid != SaveLoaderResultType.Normal)
                return isPathValid;

            using var writer = new StreamWriter(path, false);
            writer.Write(JsonUtility.ToJson(data));

            return SaveLoaderResultType.Normal;
        }

        public static SaveLoaderResultType Overwrite<T>(string path, T data)
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != null)
                Directory.CreateDirectory(directory);

            try
            {
                using var writer = new StreamWriter(path, false);
                writer.Write(JsonUtility.ToJson(data));
            }
            catch
            {
                return SaveLoaderResultType.Error;
            }

            return SaveLoaderResultType.Normal;
        }

        public static SaveLoaderResultType Load<T>(string path, T defaultData, out T result) where T : ISavedData
        {
            result = defaultData;

            if (!File.Exists(path))
                return SaveLoaderResultType.ErrorFileIsNotExist;

            try
            {
                using var reader = new StreamReader(path, false);
                result = JsonUtility.FromJson<T>(reader.ReadToEnd());
            }
            catch
            {
                return SaveLoaderResultType.Error;
            }

            return result == null ? SaveLoaderResultType.ErrorFileIsDamaged : SaveLoaderResultType.Normal;
        }

        public static SaveLoaderResultType Remove(string path)
        {
            if (!File.Exists(path))
                return SaveLoaderResultType.ErrorFileIsNotExist;

            try
            {
                File.Delete(path);
            }
            catch
            {
                return SaveLoaderResultType.Error;
            }

            return SaveLoaderResultType.Normal;
        }

        public static string[] GetFileNames(string path, string extension = "*")
        {
            if (!Directory.Exists(path))
                return Array.Empty<string>();

            var files = Directory.GetFiles(path, $"*.{extension}");

            for (var i = 0; i < files.Length; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);

            return files;
        }

        public static SaveLoaderResultType IsValidSavePath(string path)
        {
            if (File.Exists(path))
                return SaveLoaderResultType.ErrorFileIsExist;

            var directory = Path.GetDirectoryName(path);
            if (directory != null)
                Directory.CreateDirectory(directory);

            try
            {
                using var stream = File.Create(path);
                stream.Close();
                File.Delete(path);
                return SaveLoaderResultType.Normal;
            }
            catch
            {
                return SaveLoaderResultType.Error;
            }
        }

        public static bool IsFileExist(string path) => File.Exists(path);
    }
}