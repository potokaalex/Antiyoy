using Client._Back;
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
      Locator.Set(_configsProvider);
      Locator.Set(new InputController());
      Locator.Set(_cameraController);
      Locator.Set(_tilemapController);
      Locator.Set(_gridController);
      Locator.Set(_debugController);
      Locator.Set(new CapitalsController());
      Locator.Set(_bordersService);
      Locator.Set(new RegionsFactory());
      Locator.Set(new RegionsService());
      Locator.Set(new GovernmentsService());
      Locator.Set(new UnitsAreaCalculator());
      Locator.Set(new UnitsService());
      Locator.Set(_tilesSelectionView);
      Locator.Set(_gameplayUI);
      Locator.Set(_protectionView);
      Locator.Set(_capitalsMarkController);
      Locator.Set(new BackController());
      Locator.Set(new GameplayController());
    }
  }
}