using Client.Borders;
using Client.Configs;
using Client.DebugFeatures;
using Client.Gameplay.UI;
using Client.Government;
using Client.Infrastructure;
using Client.Protection;
using Client.Region;
using Client.Tile;
using Client.TilesSelection;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using UnityEngine;

namespace Client.Gameplay
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private TilemapController _tilemapController;
    [SerializeField] private GridController _gridController;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private DebugController _debugController;
    [SerializeField] private ConfigsProvider _configsProvider;
    [SerializeField] private TilesSelectionView _tilesSelectionView;
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private ProtectionView _protectionView;
    [SerializeField] private BordersService _bordersService;
    [SerializeField] private CapitalsMarkController _capitalsMarkController;

    protected override void Install()
    {
      Register(_configsProvider);
      Register(new InputController());
      Register(_cameraController);
      Register(_tilemapController);
      Register(_gridController);
      Register(_debugController);
      Register(new CapitalsController());
      Register(_bordersService);
      Register(new RegionsFactory());
      Register(new RegionsService());
      Register(new GovernmentsService());
      Register(new UnitsAreaCalculator());
      Register(new UnitsService());
      Register(_tilesSelectionView);
      Register(_gameplayUI);
      Register(_protectionView);
      Register(_capitalsMarkController);
      Register(new GameplayController());
    }
  }
}