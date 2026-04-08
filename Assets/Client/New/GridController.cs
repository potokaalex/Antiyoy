using UnityEngine;
using UnityEngine.Tilemaps;

namespace Client.New
{
  public class GridController : MonoBehaviour
  {
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _backgroundTile;

    public void Initialize(Vector2Int mapSize)
    {
      FillByTile(_backgroundTile, mapSize);
    }

    private void FillByTile(TileBase tile, Vector2Int mapSize)
    {
      for (var y = 0; y < mapSize.y; y++)
      for (var x = 0; x < mapSize.x; x++)
        _tilemap.SetTile(new Vector3Int(x, y), tile);

      _tilemap.CompressBounds();
    }

    public void SetColor(HexCoordinates position, Color color)
    {
      _tilemap.SetColor((Vector3Int)position.ToArrayIndex(), color);
    }

    public Vector3 GetCellCenterWorld(HexCoordinates position)
    {
      return _grid.GetCellCenterWorld((Vector3Int)position.ToArrayIndex());
    }
  }
}