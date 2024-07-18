using ClientCode.Data.Progress.Map;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.Startup;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorLoadState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly IProjectSaveLoader _projectSaveLoader;

        public MapEditorLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider, IMapSaveLoader saveLoader,
            ILogReceiver logReceiver, IProjectSaveLoader projectSaveLoader)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _projectSaveLoader = projectSaveLoader;
        }

        public async void Enter()
        {
            _projectSaveLoader.Load(out var progress);
            
            var scenesConfig = _staticDataProvider.Configs.Scene;
            await _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
            
            LoadMap(progress.MapEditorPreload);

            _sceneLoader.FindInScene<IStartuper>(scenesConfig.MapEditorSceneName).Startup();
        }

        private void LoadMap(MapEditorPreloadData preload)
        {
            var defaultMap = new MapProgressData { Key = preload.MapKey, Height = preload.MapHeight, Width = preload.MapWidth };
            var result = _saveLoader.Load(preload.MapKey, defaultMap);

            if (result == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is not exits!"));
            else if (result == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is damaged!"));
            else if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: unknown reason!"));
        }
    }
}