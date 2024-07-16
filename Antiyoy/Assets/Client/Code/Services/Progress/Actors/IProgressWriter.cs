using System.Threading.Tasks;
using ClientCode.Data.Progress;

namespace ClientCode.Services.Progress.Actors
{
    public interface IProgressWriter : IProgressActor
    {
        Task OnSave(ProgressData progress);
    }
}