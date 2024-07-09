using Code.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private IStateMachine _stateMachine;

        [Inject]
        private void Constructor(IStateMachine stateMachine) => _stateMachine = stateMachine;

        private void Start() => _stateMachine.SwitchTo<BootstrapState>();
    }
}