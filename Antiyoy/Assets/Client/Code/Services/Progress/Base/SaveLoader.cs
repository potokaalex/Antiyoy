using System;
using System.IO;
using UnityEngine;

namespace ClientCode.Services.Progress.Base
{
    public static class SaveLoader
    {
        public static bool Save<T>(string path, T data) where T : ISavedData
        {
            if (!IsValidSavePath(path))
                return false;

            using var writer = new StreamWriter(path, false);
            writer.Write(JsonUtility.ToJson(data));
            return true;
        }

        public static bool Load<T>(string path, T defaultData, out T result) where T : ISavedData
        {
            if (!File.Exists(path))
            {
                result = defaultData;
                return false;
            }

            using var reader = new StreamReader(path, false);
            result = JsonUtility.FromJson<T>(reader.ReadToEnd());

            if (result == null)
            {
                result = defaultData;
                return false;
            }

            return true;
        }

        public static void Remove(string path) => File.Delete(path);

        public static string[] GetFileNames(string path, string extension = "*")
        {
            if (!Directory.Exists(path))
                return Array.Empty<string>();

            var files = Directory.GetFiles(path, $"*.{extension}");

            for (var i = 0; i < files.Length; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);

            return files;
        }

        public static bool IsValidSavePath(string path)
        {
            if (File.Exists(path))
                return false;

            var directory = Path.GetDirectoryName(path);
            if (directory != null)
                Directory.CreateDirectory(directory);

            try
            {
                using var stream = File.Create(path);
                stream.Close();
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}