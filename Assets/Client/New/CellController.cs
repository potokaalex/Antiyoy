using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Infrastructure;
using Client.New.Region;
using Client.New.Tile;
using UnityEngine;

namespace Client.New
{
  public class CellController
  {
    private TilemapController _tilemapController;
    private RegionController _region;

    public HexCoordinates Position { get; private set; }

    public RegionController Region
    {
      get => _region;
      set
      {
        _region = value;
        if (_region == null)
        {
          SetColor(Color.black);
        }
        else if (_region.Type == RegionType.Default)
        {
          SetColor(Color.gray);
        }
      }
    }

    public void Initialize(HexCoordinates position, RegionType type)
    {
      _tilemapController = Locator.Get<TilemapController>();
      Position = position;
      Region = new RegionController(new List<CellController> { this }, type); //todo: remove it. add to _regions!
    }

    private void SetColor(Color color)
    {
      _tilemapController.SetColor(Position, color);
    }
  }
}