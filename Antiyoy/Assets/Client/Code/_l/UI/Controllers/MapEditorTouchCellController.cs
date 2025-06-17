using ClientCode.Data.Scene;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.Services.InputService;
using ClientCode.UI.Buttons.MapEditor;
using ClientCode.UI.Models;

namespace ClientCode.UI.Controllers
{
    public class MapEditorTouchCellController : TouchCellController
    {
        private readonly MapEditorModel _model;
        private readonly TileFactory _tileFactory;
        private readonly RegionFactory _regionFactory;
        private readonly IInputService _input;

        public MapEditorTouchCellController(MapEditorSceneData sceneData, CameraController camera, MapEditorModel model, TileFactory tileFactory,
            RegionFactory regionFactory, IInputService input, GridManager gridManager) : base(sceneData.EventSystem, camera, gridManager)
        {
            _model = model;
            _tileFactory = tileFactory;
            _regionFactory = regionFactory;
            _input = input;
        }

        private protected override void OnCellTouch(int cell)
        {
            if (_input.IsMouseButtonDown(MouseType.Left))
                Create(cell);
            else if (_input.IsMouseButtonDown(MouseType.Right))
                Destroy(cell);
        }

        private void Create(int cell)
        {
            switch (_model.ModeType)
            {
                case MapEditorModeButtonType.CreateRegionNone:
                    _tileFactory.Create(cell);
                    _regionFactory.Destroy(cell);
                    break;
                case MapEditorModeButtonType.CreateRegionRed:
                    _tileFactory.Create(cell);
                    _regionFactory.Create(cell, RegionType.Red);
                    break;
                case MapEditorModeButtonType.CreateRegionBlue:
                    _tileFactory.Create(cell);
                    _regionFactory.Create(cell, RegionType.Blue);
                    break;
            }
        }

        private void Destroy(int cell)
        {
            _tileFactory.Destroy(cell);
            _regionFactory.Destroy(cell);
        }
    }
}