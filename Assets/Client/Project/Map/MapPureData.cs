using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Project.Map
{
  [Serializable]
  public class MapPureData
  {
    public List<PlayerPureData> Players = new();
    public Vector2Int Size;

    public IEnumerable<CellPureData> GetCells()
    {
      foreach (var player in Players)
      foreach (var region in player.Regions)
      foreach (var cell in region.Cells)
        yield return cell;
    }
  }
}