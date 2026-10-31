using System;
using System.Collections.Generic;
using Client.Project.Map.Data;

namespace Client.Project.Progress
{
  [Serializable]
  public class ProgressData
  {
    public MenuProgressData Menu = new();
    public List<MapData> Maps = new();
    public int AutosaveMapIndex = -1;
  }
}