using System;
using System.IO;
using UnityEngine;

namespace ClientCode.Services.SaveLoader
{
    public class SaveLoader : ISaveLoader
    {
        public bool Save<T>(string path, T data)
        {
            var directory = Path.GetDirectoryName(path);

            if (directory != null)
                Directory.CreateDirectory(directory);

            try
            {
                using var streamWriter = new StreamWriter(path, false);
                streamWriter.Write(JsonUtility.ToJson(data));
            }
            catch(Exception exc)
            {
                Debug.Log(exc);
                return false;
            }

            return true;
        }

        public bool Load<T>(string path, T defaultData, out T result) //where T : ISavedData
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