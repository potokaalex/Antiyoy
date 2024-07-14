using ClientCode.Data.Progress;
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
            LoadProgress();
            LoadScene();
        }

        private void LoadProgress()
        {
            //при нажатии на кнопку я устанавливаю новый прогресс. Прогрсс MapEditor не сохраняется, но существует. Его нужно загружать 
            //если я использую ресиверы, то:
            //регистрация -> загрузка -> сохранения -> загрузка -> разрегистрация
            //если использую провайдеры:
            //загрузка -> изменение
            _saveLoader.LoadMapEditorProgress()
            var progress = _progressDataProvider.MapEditor;
            progress.Map = _saveLoader.LoadMapProgress(progress.MapKey, new MapProgressData());
        }

        private void LoadScene()
        {
            var scenesConfig = _staticDataProvider.Configs.Scene;
            _sceneLoader.LoadSceneAsync(scenesConfig.MapEditorSceneName);
        }
    }
}