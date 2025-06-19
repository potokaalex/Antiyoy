using ClientCode.UI.Windows.Writing;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Client.Code.Gameplay
{
    public class GridController : MonoBehaviour, IInitializable //TODO: rename ?
    {
        public Tilemap Tilemap;
        public TileBase Tile;
        private MapsContainer _mapsContainer;
        private MapController _map;

        [Inject]
        public void Construct(MapsContainer mapsContainer) => _mapsContainer = mapsContainer;

        public void Initialize()
        {
            _map = _mapsContainer.CurrentMap;
            FillByTile(Tile);
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