using System.Threading.Tasks;
using ClientCode.Data.Progress;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressReader : IProgressActor
    {
        Task OnLoad(PlayerProgressData progress);
    }
}