using System;
using ClientCode.Data.Static;
using ClientCode.Infrastructure.States;
using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using ClientCode.UI.Buttons;
using Zenject;

namespace ClientCode.UI
{
    public class LoadSceneButtonHandler : ILoadSceneButtonHandler
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Handle(SceneType sceneType)
        {
            switch (sceneType)
            {
                case SceneType.MainMenu:
                    _stateMachine.SwitchTo<MainMenuLoadState>();
                    break;
                case SceneType.MapEditor:
                    _stateMachine.SwitchTo<MapEditorLoadState>();
                    break;
                case SceneType.Gameplay:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
            }
        }
    }
}