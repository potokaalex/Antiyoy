using ClientCode.Gameplay.Cell;
using ClientCode.UI.Windows.Writing;
using ClientCode.Utilities.Extensions;
using Leopotam.EcsLite;
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
        private EcsController _ecsController;
        private EcsPool<CellComponent> _cellPool;
        private int[] _cells;

        [Inject]
        public void Construct(MapsContainer mapsContainer, CellsFactory cellsFactory, EcsController ecsController)
        {
            _ecsController = ecsController;
            _mapsContainer = mapsContainer;
            _cellsFactory = cellsFactory;
        }

        public void Initialize()
        {
            _map = _mapsContainer.CurrentMap;
            FillByTile(Tile);
            _cells = _cellsFactory.CreateEntitiesWithCells(Grid);
            _cellPool = _ecsController.World.GetPool<CellComponent>();
        }

        public bool TryGetCell(Vector3 worldPosition, out int entity)
        {
            var cellPosition = (Vector2Int)Grid.WorldToCell(worldPosition);
            var arrayIndex = cellPosition.ToArrayIndex(_map.Size.x);

            if (!cellPosition.InRangeExclusive(_map.Size))
            {
                entity = -1;
                return false;
            }

            entity = _cells[arrayIndex];
            return true;
        }

        public void SetColor(int cellEntity, Color color)
        {
            var cell = _cellPool.Get(cellEntity);
            var position = (Vector3Int)cell.GridPosition;
            Tilemap.SetTileFlags(position, TileFlags.None);
            Tilemap.SetColor(position, color);
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