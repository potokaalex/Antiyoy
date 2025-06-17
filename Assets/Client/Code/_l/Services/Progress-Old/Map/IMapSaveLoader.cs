using ClientCode.Data.Progress.Map;
using ClientCode.Services.Progress.Base;
using Cysharp.Threading.Tasks;

namespace ClientCode.Services.Progress.Map
{
    public interface IMapSaveLoader : IProgressSaveLoader
    {
        SaveLoaderResultType Load(string key, MapProgressData defaultData = null);
        UniTask<SaveLoaderResultType> Save(string key = null);
        SaveLoaderResultType Remove(string key);
        SaveLoaderResultType IsKeyValidToSaveWithoutOverwrite(string key);
        SaveLoaderResultType IsValidToLoad(string key);
    }
}