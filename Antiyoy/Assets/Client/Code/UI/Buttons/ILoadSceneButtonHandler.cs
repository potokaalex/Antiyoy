using ClientCode.Data.Static;

namespace ClientCode.UI.Buttons
{
    public interface ILoadSceneButtonHandler
    {
        void Handle(SceneType sceneType);
    }
}