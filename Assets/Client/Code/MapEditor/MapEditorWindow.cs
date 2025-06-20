using System;
using System.Collections.Generic;
using System.Linq;
using Client.Code.Gameplay;
using ClientCode.Gameplay.Region;
using ClientCode.Infrastructure.States.MapEditor.MainMenu;
using ClientCode.UI.Controllers;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ClientCode.Infrastructure.Installers
{
    public class MapEditorWindow : SerializedMonoBehaviour, IInitializable, IDisposable, ITickable
    {
        public MapEditorModeSelector ModeSelector;
        private MapEditorMode _mode;
        private readonly CompositeDisposable _disposable = new();
        private TileFactory _tileFactory;
        private CameraController _cameraController;
        private GridController _gridController;

        [Inject]
        public void Construct(TileFactory tileFactory, CameraController cameraController, GridController gridController)
        {
            _cameraController = cameraController;
            _tileFactory = tileFactory;
            _gridController = gridController;
        }

        public void Initialize()
        {
            ModeSelector.Initialize();
            ModeSelector.AddTo(_disposable);
        }

        public void Dispose() => _disposable.Dispose();

        public void Tick()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var hit = _cameraController.GetHitFromMousePoint();

                if (hit && _gridController.TryGetCell(hit.point, out var entity))
                {
                    _tileFactory.Destroy(entity);

                    if (_mode == MapEditorMode.CreateTile)
                        _tileFactory.Create(entity);
                    else if (_mode == MapEditorMode.DestroyTile)
                        _tileFactory.Destroy(entity);
                    else if (_mode == MapEditorMode.CreateRedTile)
                        _tileFactory.Create(entity, RegionType.Red);
                    else if (_mode == MapEditorMode.CreateBlueTile)
                        _tileFactory.Create(entity, RegionType.Blue);
                }
            }
        }
    }
}