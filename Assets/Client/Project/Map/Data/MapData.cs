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

    public IEnumerable<CellData> GetCells()
    {
      foreach (var player in Players)
      foreach (var region in player.Regions)
      foreach (var cell in region.Cells)
        yield return cell;
    }
  }
}