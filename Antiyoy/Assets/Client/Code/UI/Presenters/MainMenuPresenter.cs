using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using ClientCode.UI.Buttons.Load;
using Zenject;

namespace ClientCode.UI.Presenters
{
    public class MainMenuPresenter : ILoadButtonHandler, ISelectMapButtonHandler
    {
        private IStateMachine _stateMachine;
        private IProgressDataProvider _progressDataProvider;

        [Inject]
        public void Construct(IStateMachine stateMachine, IProgressDataProvider progressDataProvider)
        {
            _progressDataProvider = progressDataProvider;
            _stateMachine = stateMachine;
        }

        void ILoadButtonHandler.Handle(LoadButtonType loadButtonType)
        {
            if (loadButtonType == LoadButtonType.MapEditor)
            {
                _progressDataProvider.MainMenu.SelectedMapKey = "SomeMapKey"; //TODO: pass NULL to get new Map (?)
                _stateMachine.SwitchTo<MapEditorLoadState>();
            }
        }

        void ISelectMapButtonHandler.Handle(string mapKey)
        {
            _progressDataProvider.MainMenu.SelectedMapKey = mapKey;
            _stateMachine.SwitchTo<MapEditorLoadState>();
        }
    }
}