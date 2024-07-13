using ClientCode.Data.Static;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using Zenject;

namespace ClientCode.UI.Presenters
{
    public class MainMenuPresenter : ILoadSceneButtonHandler
    {
        private IStateMachine _stateMachine;

        public void Handle(SceneType sceneType)
        {
            if (sceneType == SceneType.MapEditor)
                _stateMachine.SwitchTo<MapEditorLoadState>();
        }

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;
    }
}