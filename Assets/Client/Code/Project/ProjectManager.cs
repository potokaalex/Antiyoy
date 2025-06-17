using ClientCode.Services.SceneLoader;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectManager : IInitializable
    {
        private readonly DiContainer _container;

        public ProjectManager(DiContainer container) => _container = container;

        public void Initialize() => LoadMenu();

        public void LoadMenu() => _container.Resolve<SceneLoader>().LoadScene(SceneName.MainMenu);

        public void LoadEditor() => _container.Resolve<SceneLoader>().LoadScene(SceneName.MapEditor);

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