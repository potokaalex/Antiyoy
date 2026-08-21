using Client.ActionsHistory;
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
    [SerializeField] private UnitsService _unitsService;

    public override void Install(Context context)
    {
      context.Register(_configsProvider);
      context.Register(_cameraController);
      context.Register(_tilemapController);
      context.Register(_gridController);
      context.Register(_debugController);
      context.Register(new CapitalsController());
      context.Register(_bordersService);
      context.Register(new RegionsFactory());
      context.Register(new RegionsService());
      context.Register(new GovernmentsService());
      context.Register(new UnitsAreaCalculator());
      context.Register(_unitsService);
      context.Register(_tilesSelectionView);
      context.Register(_gameplayUI);
      context.Register(_protectionView);
      context.Register(_capitalsMarkController);
      context.Register(new ActionsHistoryController());
      context.Register(new GameplayController(transform.parent.gameObject));
    }
  }
}