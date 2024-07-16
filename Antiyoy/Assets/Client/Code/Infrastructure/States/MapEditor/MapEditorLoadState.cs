using ClientCode.Data.Progress;
using ClientCode.Services.Progress;
using ClientCode.Services.Progress.Actors;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorLoadState : IState, IProgressReader
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IProgressDataSaveLoader _saveLoader;

        public MapEditorLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider, IProgressDataSaveLoader saveLoader)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
        }

        public void Enter()
        {
            _saveLoader.RegisterActor(this);
            _saveLoader.Load();
            _saveLoader.UnRegisterActor(this);
            
            LoadScene();
        }

        private void LoadScene()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
        }

        public void OnLoad(ProgressData progress) => progress.Player.Map = _saveLoader.LoadMap(progress.Player.SelectedMapKey);
    }
}