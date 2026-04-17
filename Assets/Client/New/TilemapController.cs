using Client.New.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.New
{
  public class TilemapController
  {
    private Tilemap _tilemap;
    private GridController _gridController;
    private MapController _mapController;

    public void Initialize(Tilemap tilemap, GridController gridController, MapController mapController)
    {
      _gridController = gridController;
      _tilemap = tilemap;
      _mapController = mapController;
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(position.ToArray2DIndex()), color);
    }

    public void FillByTile(TileBase tile)
    {
      var mapSize = _mapController.Size;
      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
        _tilemap.SetTile(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), tile);

      _tilemap.CompressBounds();
    }
  }
}