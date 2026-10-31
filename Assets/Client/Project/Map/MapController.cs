using System;
using System.Collections.Generic;
using System.Linq;
using Client.Project.Map.Data;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapController
  {
    private readonly MapData _data;

    public MapController(MapData data) => _data = data;

    public Vector2Int Size => _data.Size;

    public IReadOnlyList<PlayerData> Players => _data.Players;

    public string Name { get => _data.Name; set => _data.Name = value; }

    public DateTime CreationTime => DateTime.FromBinary(_data.CreationTime);

    public bool IsAutosave => _data.IsAutosave;

    public IEnumerable<CellData> GetCells()
    {
      foreach (var player in _data.Players)
      foreach (var region in player.Regions)
      foreach (var cell in region.Cells)
        yield return cell;
    }

    public MapData GetData() => _data;

    public MapController RestartCopy()
    {
      return new MapController(new MapData
      {
        InitialPlayers = _data.InitialPlayers,
        Players = _data.InitialPlayers.Select(x => x.Copy()).ToList(),
        Size = _data.Size,
        Name = _data.Name,
        CreationTime = _data.CreationTime,
        IsAutosave = false
      });
    }
  }
}