using ClientCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace ClientCode.Services.Startup
{
    public class DelayStartupper : MonoBehaviour
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        public void Startup<T>() where T : IState => _stateMachine.SwitchTo<T>();
    }
}