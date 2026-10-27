using System;

namespace Client.Project.Progress
{
  [Serializable]
  public class GameplayModesProgressData
  {
    public int Difficulty = 1;
    public int MapSize;
    public int Players = 1;
    public int Colors = 1;
  }
}