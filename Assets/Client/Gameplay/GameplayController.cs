using System;
using System.Collections.Generic;
using Client.ActionsHistory;
using Client.Borders;
using Client.Gameplay.UI;
using Client.Government;
using Client.Infrastructure;
using Client.Menu.MainMenu;
using Client.Region;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using Cysharp.Threading.Tasks;
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
    private PlayerController _currentPlayer;
    private int _turnsCount;
    private bool _canTick;

    public RegionType CurrentPlayerRegionType => _currentPlayer.RegionType;

    public bool CanTick => _canTick;

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

      _bordersService.ViewRegionsBorders();
      _gameplayUI.ViewTurnsCount(_turnsCount);
      _gameplayUI.PlayShow();
      _canTick = true;
    }

    public void Tick()
    {
      if (!_canTick)
        return;

      _cameraController.Tick();
      _capitalsMarkController.Tick();
      _currentPlayer.Tick();
    }

    public void NextTurn()
    {
      _currentPlayer.Clear();
      _actionsHistoryController.Clear();

      if (CheckWin())
        return;

      if (MoveNextPlayer())
      {
        UpdatePlayerRegions();
        return;
      }

      _turnsCount++;
      _gameplayUI.ViewTurnsCount(_turnsCount);
      SetFirstPlayer();
      UpdatePlayerRegions();
    }

    public void EndGameplay()
    {
      _canTick = false;
      _capitalsMarkController.Disable();
      _unitsService.Clear();
      _regionsService.Clear();
      _actionsHistoryController.Clear();
    }

    public void Pause()
    {
      _currentPlayer.Clear();
      _gameplayUI.ShowPause();
    }

    public void UnPause() => _gameplayUI.HidePause();

    public async void MainMenu()
    {
      _mainMenuView.ShowStart();

      await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
      EndGameplay();
    }

    public void Undo()
    {
      _actionsHistoryController.Undo();
      _currentPlayer.SelectLastSelectedRegion();
    }

    private void SetFirstPlayer() => _currentPlayer = _players[0];

    public void SetCreateUnitMode(UnitType type) => _currentPlayer.SetCreateUnitMode(type);

    private void CreatePlayers()
    {
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
      if (_turnsCount <= 0)
        return;

      foreach (var region in _regionsService.Regions)
        if (region.Type == CurrentPlayerRegionType)
          region.Update();
    }

    private bool MoveNextPlayer()
    {
      var currentIndex = _players.IndexOf(_currentPlayer);
      var maxIndex = _players.Count - 1;
      if (currentIndex < maxIndex)
      {
        _currentPlayer = _players[currentIndex + 1];
        return true;
      }

      return false;
    }

    private bool CheckWin()
    {
      using (ListPool<GovernmentController>.Get(out var governments))
      {
        _governmentsService.GetAllAlive(governments);
        if (governments.Count == 1)
        {
          _gameplayUI.ShowEndScreen(governments[0].RegionsType);
          return true;
        }

        return false;
      }
    }
  }
}