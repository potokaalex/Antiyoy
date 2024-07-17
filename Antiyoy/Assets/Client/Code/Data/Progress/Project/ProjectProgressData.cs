using System;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Progress.Project
{
    public class ProjectProgressData : IProgressData
    {
        public ProjectLoadData Load = new();
        public string[] MapKeys = Array.Empty<string>();
        public string SelectedMapKey = string.Empty;
    }
}