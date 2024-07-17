using System;
using ClientCode.Data.Progress.Map;
using ClientCode.Data.Scene;
using ClientCode.Services.Progress.Base;

namespace ClientCode.Data.Progress.Project
{
    public class ProjectProgressData : IProgressData
    {
        public ProjectLoadData Load = new();
        public MapEditorPreloadData MapEditorPreload = new();
        public string[] MapKeys = Array.Empty<string>();
    }
}