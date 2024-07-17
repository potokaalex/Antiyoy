using System.Threading.Tasks;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressReader<in T> : IProgressActor where T : IProgressData
    {
        Task OnLoad(T progress);
    }
}