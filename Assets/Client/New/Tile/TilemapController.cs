using Client.New.Hex;
using Client.New.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.New.Tile
{
  public class TilemapController : MonoBehaviour
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
    }

    public void FillByBaseTiles()
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
        _tilemap.SetTile(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), _tile);

      _tilemap.CompressBounds();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(position.ToArray2DIndex()), color);
    }
  }
}