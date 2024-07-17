using ClientCode.Data.Progress.Map;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;
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
            var preload = _projectSaveLoader.Current.MapEditorPreload;
            var defaultMap = new MapProgressData { Key = preload.SelectedMapKey, Height = preload.MapHeight, Width = preload.MapWidth };
            var result = await _saveLoader.Load(preload.SelectedMapKey, defaultMap);

            if (result == SaveLoaderResultType.ErrorFileIsNotExist)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is not exits!"));
            else if (result == SaveLoaderResultType.ErrorFileIsDamaged)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: file is damaged!"));
            else if (result == SaveLoaderResultType.Error)
                _logReceiver.Log(new LogData(LogType.Error, "Impossible to load: unknown reason!"));
            else
                LoadScene();
        }

        private void LoadScene()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
        }
    }
}