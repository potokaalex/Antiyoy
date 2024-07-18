using System.Linq;
using ClientCode.Data.Progress.Project;
using ClientCode.Services.Progress.Actors;
using ClientCode.Utilities;
using Cysharp.Threading.Tasks;

namespace ClientCode.UI.Models
{
    public class MainMenuModel : IProgressReader<ProjectProgressData>, IProgressWriter<ProjectProgressData>
    {
        public MapEditorPreloadData MapEditorPreload { get; } = new();

        public EventedList<string> MapKeys { get; private set; }

        public void OnLoad(ProjectProgressData progress) => MapKeys = new EventedList<string>(progress.MapKeys.ToList());

        public UniTask OnSave(ProjectProgressData progress)
        {
            progress.MapEditorPreload = MapEditorPreload;
            progress.MapKeys = MapKeys.ToArray();
            return UniTask.CompletedTask;
        }
    }
}