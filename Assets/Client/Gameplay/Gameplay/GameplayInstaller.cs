using Client.Gameplay.ActionsHistory;
using Client.Gameplay.DebugFeatures;
using Client.Gameplay.Player;
using Client.Gameplay.Protection;
using Client.Gameplay.Recovery;
using Client.Gameplay.TileSelectionView;
using Client.Gameplay.UI;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Unit.Code;
using Client.Project.Unit.Code.Capital;
using UnityEngine;

namespace Client.Gameplay.Gameplay
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private DebugController _debugController;
    [SerializeField] private TilesSelectionView _tilesSelectionView;
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private ProtectionView _protectionView;
    [SerializeField] private CapitalsMarksController _capitalsMarksController;
    [SerializeField] private UnitSelectionView _unitSelectionView;
    [SerializeField] private UnitsMoveAnimator _unitsMoveAnimator;
    [SerializeField] private TileClickView _tileClickView;

    public override void Install(Context context)
    {
      context.Register(_debugController);
      context.Register(new CapitalsController());
      context.Register(new GameplayRegionsController());
      context.Register(new GovernmentsService());
      context.Register(new WarriorUnitsAnimator());
      context.Register(new TreesController());
      context.Register(_tilesSelectionView);
      context.Register(_gameplayUI);
      context.Register(_protectionView);
      context.Register(_capitalsMarksController);
      context.Register(new RecoveryController());
      context.Register(new ActionsHistoryController());
      context.Register(new GameplayFieldController());
      context.Register(_unitSelectionView);
      context.Register(_unitsMoveAnimator);
      context.Register(_tileClickView);
      context.Register(new PlayerViewController());
      context.Register(new GameplayController());
    }
  }
}