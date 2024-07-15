namespace ClientCode.Services.SaveLoader.Base
{
    public interface ISaveLoader
    {
        bool Save<T>(string path, T data) where T : ISavedData;
        bool Load<T>(string path, T defaultData, out T result) where T : ISavedData;
        string[] GetFileNames(string path, string extension);
    }
}