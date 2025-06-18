using ClientCode.Services.SceneLoader;
using ClientCode.UI.Windows.Writing;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectManager : IInitializable
    {
        private readonly DiContainer _container;

        public ProjectManager(DiContainer container) => _container = container;

        public void Initialize() => LoadMenu();

        public void LoadMenu() => _container.Resolve<SceneLoader>().LoadScene(SceneName.MainMenu);

        public void LoadEditor(MapController map)
        {
            UnityEngine.Debug.Log($"Load: {map.Name}");
            //_container.Resolve<SceneLoader>().LoadScene(SceneName.MapEditor);
        }

        public void LoadBattle()
        {
            //TODO
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}