using ClientCode.Data.Scene;
using ClientCode.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Gameplay.Tile;
using ClientCode.UI.Buttons.MapEditor;
using ClientCode.UI.Models;
using UnityEngine;

namespace ClientCode.UI.Controllers
{
    public class MapEditorTouchCellController : TouchCellController
    {
        private readonly MapEditorModel _model;
        private readonly TileFactoryOld _tileFactoryOld;
        private readonly RegionFactory _regionFactory;

        public MapEditorTouchCellController(MapEditorSceneData sceneData, CameraController camera, MapEditorModel model, TileFactoryOld tileFactoryOld,
            RegionFactory regionFactory, GridManager gridManager) : base(sceneData.EventSystem, camera, gridManager)
        {
            _model = model;
            _tileFactoryOld = tileFactoryOld;
            _regionFactory = regionFactory;
        }

        private protected override void OnCellTouch(int cell)
        {
            if (Input.GetMouseButtonDown(0))
                Create(cell);
            else if (Input.GetMouseButtonDown(1))
                Destroy(cell);
        }

        private void Create(int cell)
        {
            switch (_model.ModeType)
            {
                case MapEditorModeButtonType.CreateRegionNone:
                    _tileFactoryOld.Create(cell);
                    _regionFactory.Destroy(cell);
                    break;
                case MapEditorModeButtonType.CreateRegionRed:
                    _tileFactoryOld.Create(cell);
                    _regionFactory.Create(cell, RegionType.Red);
                    break;
                case MapEditorModeButtonType.CreateRegionBlue:
                    _tileFactoryOld.Create(cell);
                    _regionFactory.Create(cell, RegionType.Blue);
                    break;
            }
        }

        private void Destroy(int cell)
        {
            _tileFactoryOld.Destroy(cell);
            _regionFactory.Destroy(cell);
        }
    }
}