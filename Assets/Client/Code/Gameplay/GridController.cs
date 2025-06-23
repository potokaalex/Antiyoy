using ClientCode.UI.Windows.Writing;
using ClientCode.Utilities.Extensions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Client.Code.Gameplay
{
    public class GridController : MonoBehaviour, IInitializable
    {
        public Grid Grid;
        public Tilemap Tilemap;
        public TileBase Tile;
        private MapsContainer _mapsContainer;
        private MapController _map;
        private CellsFactory _cellsFactory;
        private int[] _cells;

        [Inject]
        public void Construct(MapsContainer mapsContainer, CellsFactory cellsFactory)
        {
            _mapsContainer = mapsContainer;
            _cellsFactory = cellsFactory;
        }

        public void Initialize()
        {
            _map = _mapsContainer.CurrentMap;
            FillByTile(Tile);
            _cells = _cellsFactory.CreateEntitiesWithCells(Grid);
        }

        public bool TryGetCell(Vector3 worldPosition, out int entity)
        {
            var cellPosition = (Vector2Int)Grid.WorldToCell(worldPosition);
            var arrayIndex = cellPosition.ToArrayIndex(_map.Size.x);

            if (cellPosition.InRangeExclusive(_map.Size))
            {
                entity = -1;
                return false;
            }

            entity = _cells[arrayIndex];
            return true;
        }

        private void FillByTile(TileBase tile)
        {
            var size = _map.Size;
            for (var y = 0; y < size.y; y++)
            for (var x = 0; x < size.x; x++)
                Tilemap.SetTile(new Vector3Int(x, y), tile);
            Tilemap.CompressBounds();
        }
    }
}