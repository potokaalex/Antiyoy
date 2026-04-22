using System.Collections.Generic;
using Client.Hex;
using Client.Infrastructure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.Gameplay
{
  public class SelectableTilemap : MonoBehaviour
  {
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tile;
    private GridController _gridController;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
      {
        _tilemap.SetTile(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), _tile);
        _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), Color.clear);
      }

      _tilemap.CompressBounds();
    }

    public void ViewSelect(List<HexCoordinates> positions)
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
      {
        var position = HexCoordinates.FromArray2DIndex(new Vector2Int(x, y));
        if (!positions.Contains(position))
        {
          _gridController.HexPositionToWorld(position);
          _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(position.ToArray2DIndex()), new Color(0.1f, 0.1f, 0.1f, 0.5f));
        }
      }
    }

    public void ClearView()
    {
      var size = _gridController.Size;
      for (var y = 0; y < size.y; y++)
      for (var x = 0; x < size.x; x++)
        _tilemap.SetColor(_gridController.GridIndexFrom2DIndex(new Vector2Int(x, y)), Color.clear);
    }
  }
}