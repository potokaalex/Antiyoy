using System;

namespace Client.Project.Progress
{
  [Serializable]
  public class MenuProgressData
  {
    public GameplayModesProgressData GameplayModesProgressData = new();
    public MapEditorOptionsProgressData MapEditorOptionsProgressData = new();
  }
}