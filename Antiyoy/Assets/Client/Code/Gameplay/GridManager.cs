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
            SetSize(_mapSize);
        }

        public bool GetCellEntity(Vector2 position, out int cell)
        {
            var cellPosition = _grid.Grid.WorldToCell(position);
            var worldPosition2Int = cellPosition.ToVector2Int();
            var arrayIndex = worldPosition2Int.ToArrayIndex(_mapSize.x);

            if (arrayIndex >= 0 && arrayIndex < _mapSize.x * _mapSize.y)
            {
                cell = _cellEntities[arrayIndex];
                return true;
            }

            cell = -1;
            return false;
        }

        public void SetTile(Vector2Int position, TileBase tile)
        {
            var position3Int = position.ToVector3Int();
            _grid.Tilemap.SetTile(position3Int, tile);
        }

        public void FillByTile(Vector2Int range, TileBase tile) => _grid.Tilemap.BoxFill(Vector3Int.zero, tile, 0, 0, range.x, range.y);

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
        
        private void SetSize(Vector2Int size)
        {
            _grid.Tilemap.origin = Vector3Int.zero;
            _grid.Tilemap.size = new Vector3Int(size.x, size.y, 0);
        }
    }
}