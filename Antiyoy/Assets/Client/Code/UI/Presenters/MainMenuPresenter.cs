using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using Zenject;

namespace ClientCode.UI.Presenters
{
    public class MainMenuPresenter : ILoadSceneButtonHandler
    {
        private IStateMachine _stateMachine;
        private IProgressDataProvider _progressDataProvider;

        [Inject]
        public void Construct(IStateMachine stateMachine, IProgressDataProvider progressDataProvider)
        {
            _progressDataProvider = progressDataProvider;
            _stateMachine = stateMachine;
        }

        public void Handle(SceneType sceneType)
        {
            if (sceneType == SceneType.MapEditor)
            {
                _progressDataProvider.MapEditor.MapKey = "SomeMapKey"; //TODO: user should choose mapKey!
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }
    }
}