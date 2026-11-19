using Client.Gameplay.ActionsHistory;
using Client.Gameplay.UI;
using Client.Menu.MainMenu;
using Client.Project;
using Client.Project.Borders;
using Client.Project.CameraFeatures;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Player;
using Client.Project.Region;
using Client.Project.Unit.Code;
using Client.Project.Unit.Code.Capital;

namespace Client.Gameplay.Gameplay
{
  public class GameplayController : IInitializable, ITickable
  {
    private CameraController _cameraController;
    private GridController _gridController;
    private UnitsService _unitsService;
    private GameplayUI _gameplayUI;
    private RegionsService _regionsService;
    private GovernmentsService _governmentsService;
    private BordersService _bordersService;
    private ActionsHistoryController _actionsHistoryController;
    private CapitalsMarksController _capitalsMarksController;
    private MainMenuView _mainMenuView;
    private TreesController _treesController;
    private IPlayerController _currentPlayer;
    private WarriorUnitsAnimator _warriorUnitsAnimator;
    private CapitalsController _capitalsController;
    private GameplayRegionsController _gameplayRegionsController;
    private MapsService _mapsService;
    private PlayersService _playersService;
    private int _turnsCount;
    private bool _started;

    private int TurnsCount
    {
      get => _turnsCount;
      set
      {
        _turnsCount = value;
        _gameplayUI.ViewTurnsCount(value);
      }
    }

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _regionsService = Locator.Get<RegionsService>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _bordersService = Locator.Get<BordersService>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
      _mainMenuView = Locator.Get<MainMenuView>();
      _capitalsMarksController = Locator.Get<CapitalsMarksController>();
      _treesController = Locator.Get<TreesController>();
      _warriorUnitsAnimator = Locator.Get<WarriorUnitsAnimator>();
      _capitalsController = Locator.Get<CapitalsController>();
      _gameplayRegionsController = Locator.Get<GameplayRegionsController>();
      _mapsService = Locator.Get<MapsService>();
      _playersService = Locator.Get<PlayersService>();
    }

    public void Start()
    {
      var map = _mapsService.StartMap;
      _warriorUnitsAnimator.Enable();
      _capitalsMarksController.Enable();
      _gridController.InitialCreateCells(map);
      _unitsService.InitialCreateUnits(map);
      _regionsService.InitialCreateRegions(map);
      _playersService.Setup(map);
      _capitalsController.CheckCapitals();
      _gameplayRegionsController.Enable();

      TurnsCount = 0;
      _bordersService.ViewRegionsBorders();
      _gameplayUI.PlayShow();
      _started = true;

      if (SetFirstPlayer())
        _currentPlayer.StartTurn();

      _cameraController.Setup();
    }

    public void Tick()
    {
      if (!_started || !_playersService.HasPlayers())
        return;

      _currentPlayer.Tick();
    }

    public void NextTurn()
    {
      if (!_playersService.HasPlayers())
        return;

      _currentPlayer.EndTurn();

      if (CheckWin())
        return;

      var needAutosave = false;

      if (!MoveNextPlayer())
      {
        TurnsCount++;
        SetFirstPlayer();
        _treesController.UpdateTrees();
        needAutosave = true;
      }

      UpdatePlayerRegions();

      if (needAutosave)
        _mapsService.SaveRuntimeMapInAutosave();

      _currentPlayer.StartTurn();
    }

    public void Pause()
    {
      if (_playersService.HasPlayers())
        _currentPlayer.Pause();
      _gameplayUI.ShowPause();
    }

    public void UnPause() => _gameplayUI.HidePause();

    public void MainMenu()
    {
      _mainMenuView.ShowStart();
      End();
    }

    public void Restart()
    {
      End();
      _mapsService.RestartStartMap();
      Start();
    }

    public void Export() => _mapsService.ExportFromRuntime();

    private void End()
    {
      _started = false;
      _warriorUnitsAnimator.Disable();
      _capitalsMarksController.Disable();
      _gameplayRegionsController.Disable();
      _unitsService.Clear();
      _regionsService.Clear();
      _actionsHistoryController.Clear();
      _governmentsService.Clear();
      _playersService.Clear();
      _gridController.DestroyAllCells();
    }

    private bool SetFirstPlayer()
    {
      if (_playersService.HasPlayers())
      {
        _currentPlayer = _playersService.Players[0];
        return true;
      }

      return false;
    }

    private void UpdatePlayerRegions()
    {
      foreach (var region in _regionsService.Regions)
      {
        if (region.Type == _currentPlayer.RegionType)
        {
          _treesController.UpdateGraves(region);
          _gameplayRegionsController.Update(region);
        }
      }
    }

    private bool MoveNextPlayer()
    {
      if (_playersService.CanGetNextPlayer(_currentPlayer, out var next))
      {
        _currentPlayer = next;
        return true;
      }

      return false;
    }

    private bool CheckWin()
    {
      if (_playersService.IsOneAlive(out var alive))
      {
        _gameplayUI.ShowEndScreen(alive.RegionType);
        return true;
      }

      return false;
    }
  }
}