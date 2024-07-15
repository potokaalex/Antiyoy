using ClientCode.Infrastructure.States.MapEditor;
using ClientCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace ClientCode.Infrastructure.Startup
{
    public class MapEditorStartup : MonoBehaviour
    {
        private IStateMachine _stateMachine;

        [Inject]
        public void Construct(IStateMachine stateMachine) => _stateMachine = stateMachine;

        private void Start() => _stateMachine.SwitchTo<MapEditorEnterState>();
    }
}