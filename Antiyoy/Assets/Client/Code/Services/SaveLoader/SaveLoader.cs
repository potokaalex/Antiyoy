using System;
using System.IO;
using UnityEngine;

namespace ClientCode.Services.SaveLoader
{
    public class SaveLoader : ISaveLoader
    {
        public bool Load<T>(string path, T defaultData, out T result)
        {
            if (!File.Exists(path))
            {
                result = defaultData;
                return false;
            }

            using var streamReader = new StreamReader(path, false);
            result = JsonUtility.FromJson<T>(streamReader.ReadToEnd());

            if (result == null)
            {
                result = defaultData;
                return false;
            }

            return true;
        }

        public string[] GetFileNames(string path, string extension = "*")
        {
            if (!Directory.Exists(path))
                return Array.Empty<string>();

            var files = Directory.GetFiles(path, $"*.{extension}");

            for (var i = 0; i < files.Length; i++)
                files[i] = Path.GetFileNameWithoutExtension(files[i]);

            return files;
        }
    }
}