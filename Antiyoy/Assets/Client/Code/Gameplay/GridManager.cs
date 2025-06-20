using ClientCode.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ClientCode.Gameplay
{
    public class GridManager
    {
        private int[] _cellEntities;
        private Vector2Int _mapSize;
        private GridObject _grid;

        public void Initialize(GridObject grid, int[] cellEntities, Vector2Int mapSize)
        {
            _mapSize = mapSize;
            _cellEntities = cellEntities;
            _grid = grid;
        }

        public bool GetCellEntity(Vector2 position, out int cell)
        {
            var cellPosition = _grid.Grid.WorldToCell(position);
            var worldPosition2Int = cellPosition.ToVector2Int();
            var arrayIndex = worldPosition2Int.ToArrayIndex(_mapSize.x);

            if (worldPosition2Int.x < 0 || worldPosition2Int.y < 0 || worldPosition2Int.x >= _mapSize.x || worldPosition2Int.y >= _mapSize.y)
            {
                cell = -1;
                return false;
            }

            cell = _cellEntities[arrayIndex];
            return true;
        }

        public void SetTile(Vector2Int position, TileBase tile)
        {
            var position3Int = position.ToVector3Int();
            _grid.Tilemap.SetTile(position3Int, tile);
        }

        public void FillByTile(Vector2Int range, TileBase tile)
        {
            for (var y = 0; y < range.y; y++)
            for (var x = 0; x < range.x; x++)
                _grid.Tilemap.SetTile(new Vector3Int(x, y), tile);
            _grid.Tilemap.CompressBounds();
        }

        public void SetColor(Vector2Int position, Color color)
        {
            var position3Int = position.ToVector3Int();
            _grid.Tilemap.SetTileFlags(position3Int, TileFlags.None);
            _grid.Tilemap.SetColor(position3Int, color);
        }

        public Vector3 GetCellWorldPosition(Vector2Int position)
        {
            var position3Int = position.ToVector3Int();
            return _grid.Grid.GetCellCenterWorld(position3Int);
        }
    }
}