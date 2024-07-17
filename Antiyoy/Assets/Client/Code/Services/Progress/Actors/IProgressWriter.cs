using System.Threading.Tasks;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressWriter<in T> : IProgressActor where T : IProgressData
    {
        Task OnSave(T progress);
    }
}