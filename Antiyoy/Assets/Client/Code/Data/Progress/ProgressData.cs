using ClientCode.Data.Progress.Player;
using ClientCode.Data.Progress.Project;

namespace ClientCode.Data.Progress
{
    public class ProgressData
    {
        public readonly ProjectProgressData Project = new();
        public readonly PlayerProgressData Player = new();
    }
}