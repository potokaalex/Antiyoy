using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.ProgressDataProvider;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons.Load;
using ClientCode.UI.Buttons.Map;
using Zenject;

namespace ClientCode.UI.Handlers
{
    public class MainMenuButtonsHandler : ILoadButtonHandler, ISelectMapButtonHandler
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
                _stateMachine.SwitchTo<MapEditorLoadState>();
        }

        void ISelectMapButtonHandler.Handle(string mapKey)
        {
            _progressDataProvider.MainMenu.SelectedMapKey = mapKey;
            _stateMachine.SwitchTo<MapEditorLoadState>();
        }
    }
}