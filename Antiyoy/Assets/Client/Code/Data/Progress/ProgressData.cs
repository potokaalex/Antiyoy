using ClientCode.Data.Progress.Player;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Data.Progress
{
    public class ProgressData
    {
        public ProjectProgressData Project = new();
        public PlayerProgressData Player = new();
    }
}