using ClientCode.Services.SceneLoader;
using ClientCode.UI.Windows.Writing;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class ProjectManager : IInitializable
    {
        private readonly DiContainer _container;
        private readonly MapsContainer _mapsContainer;

        public ProjectManager(DiContainer container, MapsContainer mapsContainer)
        {
            _mapsContainer = mapsContainer;
            _container = container;
        }

        public void Initialize() => LoadMenu();

        public void LoadMenu() => _container.Resolve<SceneLoader>().LoadScene(SceneName.MainMenu);

        public void LoadEditor(MapController map)
        {
            _mapsContainer.CurrentMap = map;
            _container.Resolve<SceneLoader>().LoadScene(SceneName.MapEditor);
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