using System;
using System.Threading;
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
using Cysharp.Threading.Tasks;

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
    private WarriorUnitsAnimator _warriorUnitsAnimator;
    private CapitalsController _capitalsController;
    private GameplayRegionsController _gameplayRegionsController;
    private MapsService _mapsService;
    private PlayersService _playersService;
    private CancellationTokenSource _cts;
    private bool _started;
    private bool _isPaused;

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
      _cts = new();
      var map = _mapsService.StartMap;
      _warriorUnitsAnimator.Enable();
      _capitalsMarksController.Enable();
      _gridController.InitialCreateCells(map);
      _unitsService.InitialCreateUnits(map);
      _regionsService.InitialCreateRegions(map);
      _playersService.Setup(map);
      _playersService.CreatePlayers(map);
      _capitalsController.CheckCapitals();
      _gameplayRegionsController.Enable();

      _bordersService.ViewRegionsBorders();
      _gameplayUI.PlayShow();
      _started = true;
      _isPaused = false;

      if (_playersService.HasAlivePlayers())
        _playersService.CurrentPlayer.StartTurn();

      _cameraController.Setup();
    }

    public void Tick()
    {
      if (!_started || !_playersService.HasAlivePlayers())
        return;

      _playersService.CurrentPlayer.Tick();
    }

    public void NextTurn()
    {
      if (!_playersService.HasAlivePlayers() || _isPaused)
        return;

      _playersService.CurrentPlayer.EndTurn();

      if (CheckWin())
        return;

      var needAutosave = false;

      if (!_playersService.SetNextAlivePlayerAsCurrent())
      {
        _playersService.SetFirstAlivePlayerAsCurrent();
        _treesController.UpdateTrees();
        needAutosave = true;
      }

      UpdatePlayerRegions();

      if (needAutosave)
        _mapsService.SaveRuntimeMapInAutosave();

      if (_playersService.IsBotsBattle())
        StartTurnWithDelayAsync().Forget();
      else
      {
        if (_playersService.MoreThenOneHumanPlayer())
        {
          var region = _governmentsService.Get(_playersService.CurrentPlayer.RegionType).Regions[0];
          _cameraController.Focus(_capitalsController.GetCapital(region).WorldPosition);
        }

        _playersService.CurrentPlayer.StartTurn();
      }
    }

    public void Pause()
    {
      if (_playersService.HasAlivePlayers())
        _playersService.CurrentPlayer.Pause();
      _gameplayUI.ShowPause();
      _isPaused = true;
    }

    public void UnPause()
    {
      _gameplayUI.HidePause();
      _isPaused = false;
    }

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
      _cts.Cancel();
      _started = false;
      _warriorUnitsAnimator.Disable();
      _capitalsMarksController.Disable();
      _gameplayRegionsController.Disable();
      _unitsService.Clear();
      _regionsService.Clear();
      _actionsHistoryController.Clear();
      _governmentsService.Clear();
      _playersService.ClearPlayers();
      _gridController.DestroyAllCells();
    }

    private void UpdatePlayerRegions()
    {
      foreach (var region in _regionsService.Regions)
      {
        if (region.Type == _playersService.CurrentPlayer.RegionType)
        {
          _treesController.UpdateGraves(region);
          _gameplayRegionsController.Update(region);
        }
      }
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

    private async UniTaskVoid StartTurnWithDelayAsync()
    {
      try
      {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cts.Token);
        _playersService.CurrentPlayer.StartTurn();
      }
      catch (OperationCanceledException)
      {
      }
    }
  }
}