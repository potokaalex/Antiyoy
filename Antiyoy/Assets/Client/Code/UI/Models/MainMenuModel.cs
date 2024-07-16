using System.Linq;
using System.Threading.Tasks;
using ClientCode.Data.Progress;
using ClientCode.Services.Progress.Actors;
using ClientCode.Utilities;

namespace ClientCode.UI.Models
{
    public class MainMenuModel : IProgressReader, IProgressWriter
    {
        private ProgressData _progress;

        public string SelectedMapKey { get; set; }

        public EventedList<string> MapKeys { get; private set; }

        public void OnLoad(ProgressData progress) => MapKeys = new EventedList<string>(progress.Player.MapKeys.ToList());

        public Task OnSave(ProgressData progress)
        {
            progress.Player.SelectedMapKey = SelectedMapKey;
            progress.Player.MapKeys = MapKeys.ToArray();
            return Task.CompletedTask;
        }
    }
}