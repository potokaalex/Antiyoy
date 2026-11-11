using System;
using System.Collections.Generic;

namespace Client.Project.Progress
{
  [Serializable]
  public class ProgressData
  {
    public MenuProgressData Menu = new();
    public List<int> MapsIds = new();
  }
}