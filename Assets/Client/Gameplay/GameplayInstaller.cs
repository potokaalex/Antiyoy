using Client.Configs;
using Client.DebugFeatures;
using Client.Government;
using Client.Infrastructure;
using Client.Protection;
using Client.Region;
using Client.Tile;
using Client.TilesSelection;
using Client.Unit.Code;
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

    protected override void Install()
    {
      Locator.Set(_configsProvider);
      Locator.Set(_cameraController);
      Locator.Set(_tilemapController);
      Locator.Set(_gridController);
      Locator.Set(_debugController);
      Locator.Set(new CapitalsController());
      Locator.Set(new RegionsFactory());
      Locator.Set(new RegionsService());
      Locator.Set(new GovernmentsService());
      Locator.Set(new UnitsService());
      Locator.Set(_tilesSelectionView);
      Locator.Set(_gameplayUI);
      Locator.Set(_protectionView);
      Locator.Set(new GameplayController());
    }
  }
}