using System.Collections.Generic;
using System.Linq;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.UI;
using Client.Government;
using Client.Infrastructure;
using Client.Menu.MainMenu;
using Client.Region;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using UnityEngine.Pool;

namespace Client.Gameplay
{
  public class GameplayController : IInitializable, ITickable
  {
    private readonly List<PlayerController> _players = new();
    private CameraController _cameraController;
    private GridController _gridController;
    private UnitsService _unitsService;
    private GameplayUI _gameplayUI;
    private RegionsService _regionsService;
    private GovernmentsService _governmentsService;
    private BordersService _bordersService;
    private ActionsHistoryController _actionsHistoryController;
    private CapitalsMarkController _capitalsMarkController;
    private MainMenuView _mainMenuView;
    private int _turnsCount;

    private int TurnsCount
    {
      get => _turnsCount;
      set
      {
        _turnsCount = value;
        _gameplayUI.ViewTurnsCount(value);
      }
    }

    public PlayerController CurrentPlayer { get; private set; }

    public bool Started { get; private set; }

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
      _capitalsMarkController = Locator.Get<CapitalsMarkController>();
    }

    public void Start()
    {
      _gridController.InitialCreateCells();
      _unitsService.InitialCreateUnits();
      _regionsService.InitialCreateRegions();
      CreatePlayers();
      _capitalsMarkController.Enable();

      TurnsCount = 0;
      _bordersService.ViewRegionsBorders();
      _gameplayUI.PlayShow();
      Started = true;
    }

    public void Tick()
    {
      if (!Started)
        return;

      _cameraController.Tick();
      _capitalsMarkController.Tick();
      CurrentPlayer.Tick();
    }

    public void NextTurn()
    {
      CurrentPlayer.EndTurn();

      if (CheckWin())
        return;

      if (!MoveNextPlayer())
      {
        TurnsCount++;
        SetFirstPlayer();
      }

      UpdatePlayerRegions();
    }

    public void End()
    {
      Started = false;
      _capitalsMarkController.Disable();
      _unitsService.Clear();
      _regionsService.Clear();
      _actionsHistoryController.Clear();
    }

    public void Pause()
    {
      CurrentPlayer.Pause();
      _gameplayUI.ShowPause();
    }

    public void UnPause() => _gameplayUI.HidePause();

    public void MainMenu()
    {
      _mainMenuView.ShowStart();
      End();
    }

    private void SetFirstPlayer() => CurrentPlayer = _players[0];

    private void CreatePlayers()
    {
      _players.Clear();

      using (ListPool<GovernmentController>.Get(out var governments))
      {
        _governmentsService.GetAllAlive(governments);
        governments.Sort((x, y) => x.RegionsType.CompareTo(y.RegionsType));
        foreach (var government in governments)
          _players.Add(new PlayerController(government.RegionsType));
      }

      SetFirstPlayer();
    }

    private void UpdatePlayerRegions()
    {
      if (TurnsCount <= 0)
        return;

      foreach (var region in _regionsService.Regions)
        if (region.Type == CurrentPlayer.RegionType)
          region.Update();
    }

    private bool MoveNextPlayer()
    {
      var currentIndex = _players.IndexOf(CurrentPlayer);
      var maxIndex = _players.Count - 1;
      if (currentIndex < maxIndex)
      {
        CurrentPlayer = _players[currentIndex + 1];
        return true;
      }

      return false;
    }

    private bool CheckWin()
    {
      if(_players.Count(x => x.Alive) == 1)
      {
        _gameplayUI.ShowEndScreen(_players.First(x => x.Alive).RegionType);
        return true;
      }

      return false;
    }
  }
}