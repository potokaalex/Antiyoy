using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Project.Map.Data
{
  [Serializable]
  public class MapData
  {
    public List<PlayerData> Players = new();
    public Vector2Int Size;
    public string Name;
    public long CreationTime;
  }
}