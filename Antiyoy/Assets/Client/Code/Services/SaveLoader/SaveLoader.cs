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
            return true;
        }
    }
}