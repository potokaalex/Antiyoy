using System.Collections.Generic;
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
    [SerializeField] private Tilemap _tilemapMask;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _tilemapMask.gameObject.SetActive(false);
    }

    public void FillByBaseTiles()
    {
      foreach (var cell in _gridController.GetAllCells())
        CreateTile(cell.Position);
    }

    public void CreateTile(HexCoordinates position)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetTile(tilemapPosition, _tile);
      _tilemapShadows.SetTile(tilemapPosition, _tile);
    }

    public void DestroyTile(HexCoordinates position)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetTile(tilemapPosition, null);
      _tilemapShadows.SetTile(tilemapPosition, null);
    }

    public void ClearAllTiles()
    {
      _tilemap.ClearAllTiles();
      _tilemapShadows.ClearAllTiles();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      var tilemapPosition = _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
      _tilemap.SetColor(tilemapPosition, color);
      _tilemapShadows.SetColor(tilemapPosition, Color.white);
    }

    public void TilemapMaskCreateTiles(List<CellController> cells)
    {
      _tilemapMask.gameObject.SetActive(true);
      foreach (var cell in cells)
      {
        var tilemapPosition = _gridController.GridIndexFrom2DIndex(cell.Position.ToArray2DIndex());
        _tilemapMask.SetTile(tilemapPosition, _tile);
      }
    }

    public void TilemapMaskClearTiles()
    {
      _tilemapMask.ClearAllTiles();
      _tilemapMask.gameObject.SetActive(false);
    }
  }
}