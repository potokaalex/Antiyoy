using System;
using System.Collections.Generic;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using UnityEngine;

namespace Client.Project.Map
{
  public class MapController
  {
    private readonly MapsFactory _mapsFactory;
    private readonly MapData _data;

    public MapController(MapData data)
    {
      _mapsFactory = Locator.Get<MapsFactory>();
      _data = data;
    }

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

    public MapController RestartCopy() => new(_mapsFactory.CreateMapData(_data.InitialPlayers, _data.Size, _data.Name, CreationTime));
  }
}