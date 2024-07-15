using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Gameplay.Cell;
using ClientCode.Services.Progress.Actors;

namespace ClientCode.Services.Progress.Map
{
    public class MapLoader : IProgressReader
    {
        private readonly CellFactory _cellFactory;

        public MapLoader(CellFactory cellFactory) => _cellFactory = cellFactory;

        public Task OnLoad(ProgressData progress)
        {
            _cellFactory.Create(progress.Player.Map);
            return Task.CompletedTask;
        }
    }
}