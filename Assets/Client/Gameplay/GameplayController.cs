using System.Collections.Generic;
using System.Linq;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.Player;
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
    private CapitalsMarksController _capitalsMarksController;
    private MainMenuView _mainMenuView;
    private PlayerController _currentPlayer;
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
      _capitalsMarksController = Locator.Get<CapitalsMarksController>();
    }

    public void Start()
    {
      _gridController.InitialCreateCells();
      _unitsService.InitialCreateUnits();
      _regionsService.InitialCreateRegions();
      CreatePlayers();
      _capitalsMarksController.Enable();

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
      _capitalsMarksController.Tick();
      _currentPlayer.Tick();
    }

    public void NextTurn()
    {
      _currentPlayer.EndTurn();

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
      _capitalsMarksController.Disable();
      _unitsService.Clear();
      _regionsService.Clear();
      _actionsHistoryController.Clear();
    }

    public void Pause()
    {
      _currentPlayer.Pause();
      _gameplayUI.ShowPause();
    }

    public void UnPause() => _gameplayUI.HidePause();

    public void MainMenu()
    {
      _mainMenuView.ShowStart();
      End();
    }

    private void SetFirstPlayer() => SetCurrentPlayer(_players[0]);

    private void SetCurrentPlayer(PlayerController playerController)
    {
      _currentPlayer = playerController;
      _currentPlayer.StartTurn();
    }

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
        if (region.Type == _currentPlayer.RegionType)
          region.Update();
    }

    private bool MoveNextPlayer()
    {
      var currentIndex = _players.IndexOf(_currentPlayer);
      var maxIndex = _players.Count - 1;
      if (currentIndex < maxIndex)
      {
        SetCurrentPlayer(_players[currentIndex + 1]);
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