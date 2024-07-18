using System;
using ClientCode.Services.StateMachine;
using UnityEngine;
using Zenject;

namespace ClientCode.Services.Startup
{
    public class Startupper : MonoBehaviour, IStartuper
    {
        private IStateMachine _stateMachine;
        private Type _stateType;

        [Inject]
        public void Construct(IStateMachine stateMachine, Type stateType)
        {
            _stateMachine = stateMachine;
            _stateType = stateType;
        }
        
        public void Startup() => _stateMachine.SwitchTo(_stateType);
    }
}