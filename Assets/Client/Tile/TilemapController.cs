using Client.Hex;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.Tile
{
  public class TilemapController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _tilemapShadows;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      FillByBaseTiles();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetColor(tilemapPosition, color);
      _tilemapShadows.SetColor(tilemapPosition, Color.white);
    }

    private void FillByBaseTiles()
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
      {
        var tilemapPosition = _gridController.GridIndexFrom2DIndex(new Vector2Int(x, y));
        _tilemap.SetTile(tilemapPosition, _tile);
        _tilemapShadows.SetTile(tilemapPosition, _tile);
      }

      _tilemap.CompressBounds();
      _tilemapShadows.CompressBounds();
    }
  }
}