using Client.Code.Services.StateMachineCode.State;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Logger.Base;
using ClientCode.Services.Progress.Base;
using ClientCode.Services.Progress.Map;
using ClientCode.Services.Progress.Project;
using ClientCode.Services.SceneLoader;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorLoadState : IStateSimple
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IMapSaveLoader _saveLoader;
        private readonly ILogReceiver _logReceiver;
        private readonly IProjectSaveLoader _projectSaveLoader;

        public MapEditorLoadState(SceneLoader sceneLoader, IMapSaveLoader saveLoader,
            ILogReceiver logReceiver, IProjectSaveLoader projectSaveLoader)
        {
            _sceneLoader = sceneLoader;
            _saveLoader = saveLoader;
            _logReceiver = logReceiver;
            _projectSaveLoader = projectSaveLoader;
        }

        public async void Enter()
        {
            _projectSaveLoader.Load(out var progress);

            //ыvar scenesConfig = _staticData.Configs.Scene;
            await _sceneLoader.LoadSceneAsync(SceneName.MapEditor);

            LoadMap(progress.MapEditorPreload);

            //_sceneLoader.FindInScene<IStartuper>(scenesConfig.MapEditorSceneName).Startup();
        }

        private void LoadMap(MapEditorPreloadData preload)
        {
            var defaultMap = new MapProgressData { Key = preload.MapKey, Size = preload.MapSize};
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