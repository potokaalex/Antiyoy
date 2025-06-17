using System;

namespace ClientCode.Services.StateMachine
{
    public interface IStateMachine
    {
        void SwitchTo<T>() where T : IState;
        void SwitchTo(Type stateType);
    }
}