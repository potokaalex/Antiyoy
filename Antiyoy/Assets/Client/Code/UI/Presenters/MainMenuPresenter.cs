using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using ClientCode.UI.Buttons.Load;
using Zenject;

namespace ClientCode.UI.Presenters
{
    public class MainMenuPresenter : ILoadButtonHandler
    {
        private IStateMachine _stateMachine;
        private IProgressDataProvider _progressDataProvider;

        [Inject]
        public void Construct(IStateMachine stateMachine, IProgressDataProvider progressDataProvider)
        {
            _progressDataProvider = progressDataProvider;
            _stateMachine = stateMachine;
        }

        public void Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MapEditor)
            {
                _progressDataProvider.MainMenu.SelectedMapKey = "SomeMapKey"; //TODO: user should choose mapKey!
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }
    }
}