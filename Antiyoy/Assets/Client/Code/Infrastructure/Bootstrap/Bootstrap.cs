using ClientCode.Infrastructure.MainMenu;
using ClientCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Bootstrap
{
    public class Bootstrap : MonoBehaviour
    {
        private IStateMachine _stateMachine;

        [Inject]
        private void Constructor(IStateMachine stateMachine) => _stateMachine = stateMachine;

        private void Start() => _stateMachine.SwitchTo<MainMenuLoadState>();
    }
}