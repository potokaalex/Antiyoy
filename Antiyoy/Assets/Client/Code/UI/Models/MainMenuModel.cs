using System.Linq;
using System.Threading.Tasks;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Actors;
using ClientCode.Utilities;

namespace ClientCode.UI.Models
{
    public class MainMenuModel : IProgressReader<ProjectProgressData>, IProgressWriter<ProjectProgressData>
    {
        private ProjectProgressData _progress;

        public string SelectedMapKey { get; set; }

        public EventedList<string> MapKeys { get; private set; }

        public Task OnLoad(ProjectProgressData progress)
        {
            MapKeys = new EventedList<string>(progress.MapKeys.ToList());
            return Task.CompletedTask;
        }

        public Task OnSave(ProjectProgressData progress)
        {
            progress.SelectedMapKey = SelectedMapKey;
            progress.MapKeys = MapKeys.ToArray();
            return Task.CompletedTask;
        }
    }
}