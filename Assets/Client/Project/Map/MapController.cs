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
    private MapData _data;

    public MapController(MapData data)
    {
      _mapsFactory = Locator.Get<MapsFactory>();
      _data = data;
    }

    public int Id { get => _data.Id; set => _data.Id = value; }

    public Vector2Int Size => _data.Size;

    public IReadOnlyList<PlayerData> Players => _data.Players;

    public string Name { get => _data.Name; set => _data.Name = value; }

    public DateTime CreationTime => DateTime.FromBinary(_data.CreationTime);

    public bool IsAutosave => _data.IsAutosave;

    public bool IsEditor => _data.IsEditor;

    public IEnumerable<CellData> GetCells()
    {
      foreach (var player in _data.Players)
      foreach (var region in player.Regions)
      foreach (var cell in region.Cells)
        yield return cell;
    }

    public MapData GetData() => _data;

    public void Restart() => _data = _mapsFactory.CreateMapData(_data.InitialPlayers, _data.Size, _data.Name, CreationTime, Id);

    public void SetSize(Vector2Int size) => _data.Size = size;

    public void SetPlayers(List<PlayerData> players) => _data.Players = players;
  }
}