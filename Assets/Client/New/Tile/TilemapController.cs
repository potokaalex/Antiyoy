using Client.New.Hex;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.New.Tile
{
  public class TilemapController : MonoBehaviour
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    public void Initialize(GridController gridController)
    {
      _gridController = gridController;
      FillByTile(_tile);
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(position.ToArray2DIndex()), color);
    }

    private void FillByTile(TileBase tile)
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
        _tilemap.SetTile(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), tile);

      _tilemap.CompressBounds();
    }
  }
}