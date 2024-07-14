namespace ClientCode.Services.SaveLoader
{
    public interface ISaveLoader
    {
        bool Load<T>(string path, T defaultData, out T result);
        string[] GetFileNames(string path, string extension);
    }
}