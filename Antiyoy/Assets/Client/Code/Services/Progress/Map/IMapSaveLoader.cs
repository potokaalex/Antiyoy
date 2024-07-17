using System.Threading.Tasks;
using ClientCode.Data.Progress.Map;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Map
{
    public interface IMapSaveLoader : IProgressSaveLoader<MapProgressData>
    {
        string CurrentKey { get; }
        Task<SaveLoaderResultType> Load(string key);
        Task<SaveLoaderResultType> Save();
        SaveLoaderResultType Remove(string key);
        SaveLoaderResultType IsKeyValidToSaveWithoutOverwrite(string key);
        SaveLoaderResultType IsValidToLoad(string key);
    }
}