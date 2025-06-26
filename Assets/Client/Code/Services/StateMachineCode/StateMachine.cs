using System;
using System.Threading;
using Client.Code.Services.StateMachineCode.State;
using Cysharp.Threading.Tasks;

namespace Client.Code.Services.StateMachineCode
{
    public class StateMachine : IDisposable
    {
        private readonly Instantiator _instantiator;
        private CancellationTokenSource _cts;

        public StateMachine(Instantiator instantiator) => _instantiator = instantiator;

        public IStateBase CurrentState { get; private set; }

        public void SwitchTo<T>() where T : IStateBase
        {
            CurrentState?.Exit();
            CurrentState = _instantiator.Instantiate<T>();

            if (CurrentState is IStateSimple s)
                s.Enter();
            if (CurrentState is IStateAsync sa)
            {
                _cts = new();
                sa.Enter(_cts).AttachExternalCancellation(_cts.Token).Forget();
            }
        }

        public void Dispose() => _cts?.Dispose();
    }
}