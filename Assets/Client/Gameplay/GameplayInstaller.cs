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
      context.Register(_configsProvider, typeof(ConfigsProvider));
      context.Register(_cameraController, typeof(CameraController));
      context.Register(_tilemapController, typeof(TilemapController), typeof(IInitializable));
      context.Register(_gridController, typeof(GridController), typeof(IInitializable));
      context.Register(_debugController, typeof(DebugController), typeof(IInitializable));
      context.Register(new CapitalsController(), typeof(CapitalsController), typeof(IInitializable));
      context.Register(_bordersService, typeof(BordersService));
      context.Register(new RegionsFactory(), typeof(RegionsFactory), typeof(IInitializable));
      context.Register(new RegionsService(), typeof(RegionsService), typeof(IInitializable));
      context.Register(new GovernmentsService(), typeof(GovernmentsService));
      context.Register(new UnitsAreaCalculator(), typeof(UnitsAreaCalculator), typeof(IInitializable));
      context.Register(_unitsService, typeof(UnitsService), typeof(IInitializable));
      context.Register(_tilesSelectionView, typeof(TilesSelectionView));
      context.Register(_gameplayUI, typeof(GameplayUI));
      context.Register(_protectionView, typeof(ProtectionView));
      context.Register(_capitalsMarkController, typeof(CapitalsMarkController), typeof(IInitializable));
      context.Register(new ActionsHistoryController(), typeof(ActionsHistoryController), typeof(IInitializable));
      context.Register(new GameplayController(), typeof(GameplayController), typeof(IInitializable), typeof(ITickable));
    }
  }
}