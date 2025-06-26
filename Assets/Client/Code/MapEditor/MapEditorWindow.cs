using System;
using Client.Code.Gameplay;
using Client.Code.Gameplay.Region;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Client.Code.MapEditor
{
    public class MapEditorWindow : SerializedMonoBehaviour, IInitializable, IDisposable, ITickable
    {
        public MapEditorModeSelector ModeSelector;
        private readonly CompositeDisposable _disposable = new();
        private RegionFactory _regionFactory;
        private CameraController _cameraController;
        private GridController _gridController;

        [Inject]
        public void Construct(RegionFactory regionFactory, CameraController cameraController, GridController gridController)
        {
            _cameraController = cameraController;
            _regionFactory = regionFactory;
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
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && ModeSelector.HasSelection)
            {
                var hit = _cameraController.GetHitFromMousePoint();

                if (hit && _gridController.TryGetCell(hit.point, out var entity))
                {
                    _regionFactory.Destroy(entity);
                    var mode = ModeSelector.SelectedValue;

                    if (mode == MapEditorMode.CreateNeutralRegion)
                        _regionFactory.Create(entity, RegionType.Neutral);
                    else if (mode == MapEditorMode.DestroyRegion)
                        _regionFactory.Destroy(entity);
                    else if (mode == MapEditorMode.CreateRedRegion)
                        _regionFactory.Create(entity, RegionType.Red);
                    else if (mode == MapEditorMode.CreateBlueRegion)
                        _regionFactory.Create(entity, RegionType.Blue);
                }
            }
        }
    }
}