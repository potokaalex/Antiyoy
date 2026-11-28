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
      {
        var tilemapPosition = GetPosition(cell.Position); 
        _tilemap.SetTile(tilemapPosition, _tile);
        if(_gridController.IsBorderCell(cell))
          CreateShadowTile(tilemapPosition);
      }
    }

    public void CreateTile(HexCoordinates position)
    {
      var tilemapPosition = GetPosition(position); 
      _tilemap.SetTile(tilemapPosition, _tile);
      CreateShadowTile(tilemapPosition);
    }

    public void DestroyTile(HexCoordinates position)
    {
      var tilemapPosition = GetPosition(position);
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
      var tilemapPosition = GetPosition(position);
      _tilemap.SetColor(tilemapPosition, color);
    }

    public void TilemapMaskCreateTiles(List<CellController> cells)
    {
      _tilemapMask.gameObject.SetActive(true);
      foreach (var cell in cells)
      {
        var tilemapPosition = GetPosition(cell.Position);
        _tilemapMask.SetTile(tilemapPosition, _tile);
      }
      _tilemapMask.CompressBounds();
    }

    public void TilemapMaskClearTiles()
    {
      _tilemapMask.ClearAllTiles();
      _tilemapMask.gameObject.SetActive(false);
    }

    private void CreateShadowTile(Vector3Int position)
    {
      _tilemapShadows.SetTile(position, _tile);
      _tilemapShadows.SetColor(position, Color.white);
    }

    private Vector3Int GetPosition(HexCoordinates position) => _gridController.GridIndexFrom2DIndex(position.ToArray2DIndex());
  }
}