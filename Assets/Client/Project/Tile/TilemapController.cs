using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
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

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      FillByBaseTiles();
    }

    private void FillByBaseTiles()
    {
      var maxFieldSize = GameUtilities.MaxFieldSize;
      for (var x = 0; x < maxFieldSize.x; x++)
      for (var y = 0; y < maxFieldSize.y; y++)
      {
        var tilemapPosition = _gridController.GridIndexFrom2DIndex(new Vector2Int(x, y));
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

    public void ClearColor(HexCoordinates position)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetColor(tilemapPosition, Color.clear);
      _tilemapShadows.SetColor(tilemapPosition, Color.clear);
    }
  }
}