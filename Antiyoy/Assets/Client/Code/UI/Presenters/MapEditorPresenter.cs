using ClientCode.Data.Static;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using Zenject;

namespace ClientCode.UI.Presenters
{
    public class MapEditorPresenter : ILoadSceneButtonHandler
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Handle(SceneType sceneType)
        {
            if (sceneType == SceneType.MainMenu)
                _stateMachine.SwitchTo<MapEditorExitState>();
        }
    }
}