using System.Threading;
using Cysharp.Threading.Tasks;

namespace Client.Code.Core.StateMachineCode.State
{
    public interface IStateAsync : IStateBase
    {
        UniTask Enter(CancellationTokenSource cts);
    }
}