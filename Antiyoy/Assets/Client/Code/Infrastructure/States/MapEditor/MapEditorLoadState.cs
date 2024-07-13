using ClientCode.Data.Progress;
using ClientCode.Data.Progress.Load;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.SaveLoader.Progress;
using ClientCode.Services.SceneLoader;
using ClientCode.Services.StateMachine;
using ClientCode.Services.StaticDataProvider;

namespace ClientCode.Infrastructure.States.MapEditor
{
    public class MapEditorLoadState : IState
    {
        private readonly IProgressDataProvider _progressDataProvider;
        private readonly IProgressDataSaveLoader _saveLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;

        public MapEditorLoadState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider, IProgressDataSaveLoader saveLoader,
            IProgressDataProvider progressDataProvider)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _saveLoader = saveLoader;
            _progressDataProvider = progressDataProvider;
        }

        public void Enter()
        {
            LoadMapProgress();
            LoadScene();
        }

        private void LoadMapProgress()
        {
            var loadData = (MapEditorLoadData)_progressDataProvider.Load;
            _saveLoader.LoadMapProgress(loadData.MapKey, new MapProgressData());
        }

        private void LoadScene()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
        }
    }
}