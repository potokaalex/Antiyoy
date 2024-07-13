using ClientCode.Data.Progress.Load;
using ClientCode.Data.Static;
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
                _progressDataProvider.Load = new MapEditorLoadData();
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }
    }
}