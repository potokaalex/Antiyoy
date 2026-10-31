using Client.Project.Hex;
using Client.Project.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.Project.Tile
{
  public class TilemapController : MonoBehaviour, IInitializable
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _tilemapShadows;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    public void Initialize() => _gridController = Locator.Get<GridController>();

    public void FillByBaseTiles()
    {
      _tilemap.ClearAllTiles();
      _tilemapShadows.ClearAllTiles();

      foreach (var cell in _gridController.GetAllCells())
      {
        var tilemapPosition = _gridController.GridIndexFrom2DIndex(cell.Position.ToArray2DIndex());
        _tilemap.SetTile(tilemapPosition, _tile);
        _tilemapShadows.SetTile(tilemapPosition, _tile);
      }

      _tilemap.CompressBounds();
      _tilemapShadows.CompressBounds();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetColor(tilemapPosition, color);
      _tilemapShadows.SetColor(tilemapPosition, Color.white);
    }
  }
}