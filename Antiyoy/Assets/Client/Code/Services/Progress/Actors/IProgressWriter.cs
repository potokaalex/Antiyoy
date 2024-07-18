using ClientCode.Services.Progress.Base;
using Cysharp.Threading.Tasks;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressWriter<in T> : IProgressActor where T : IProgressData
    {
        UniTask OnSave(T progress);
    }
}