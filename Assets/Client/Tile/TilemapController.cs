using Client.Hex;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.Tile
{
  public class TilemapController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      FillByBaseTiles();
    }

    public void SetColor(HexCoordinates position, Color color) =>
      _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(position.ToArray2DIndex()), color);

    private void FillByBaseTiles()
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
        _tilemap.SetTile(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), _tile);

      _tilemap.CompressBounds();
    }
  }
}