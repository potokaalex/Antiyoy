using Client.Borders;
using Client.Gameplay.UI;
using Client.Infrastructure;
using Client.Protection;
using Client.Region;
using Client.TilesSelection;
using Client.Unit.Code;
using Client.Unit.Code.Capital;
using Client.Utilities;

namespace Client.Gameplay.Player
{
  public class PlayerViewController : IInitializable
  {
    private UnitsService _unitsService;
    private TilesSelectionView _tilesSelectionView;
    private GameplayUI _gameplayUI;
    private BordersService _bordersService;
    private ProtectionView _protectionView;
    private PlayerController _playerController;
    private CapitalsMarksController _capitalsMarksController;
    private UnitSelectionView _unitSelectionViw;
    private WarriorUnitsAnimator _warriorUnitsAnimator;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _tilesSelectionView = Locator.Get<TilesSelectionView>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _bordersService = Locator.Get<BordersService>();
      _protectionView = Locator.Get<ProtectionView>();
      _capitalsMarksController = Locator.Get<CapitalsMarksController>();
      _unitSelectionViw = Locator.Get<UnitSelectionView>();
      _warriorUnitsAnimator = Locator.Get<WarriorUnitsAnimator>();
    }

    public void SetPlayerController(PlayerController playerController)
    {
      _playerController = playerController;
      _warriorUnitsAnimator.StartAnimations(playerController.RegionType);
      _capitalsMarksController.StartAnimations(playerController.RegionType);
    }

    public void ClearPlayerController()
    {
      _playerController = null;
      _warriorUnitsAnimator.StopAnimations();
      _capitalsMarksController.StopAnimations();
    }

    public void ViewUnitCreation(RegionController region, UnitType unitType)
    {
      _unitsService.GetUnitCreationArea(region, GameUtilities.AreaBuffer, unitType);
      _tilesSelectionView.ClearView();

      if (unitType != UnitType.Tower && unitType != UnitType.StrongTower)
        _tilesSelectionView.ViewTiles(GameUtilities.AreaBuffer);
    }

    public void ViewUnitSelection(IUnit unit)
    {
      unit.GetMoveArea(GameUtilities.AreaBuffer);
      _tilesSelectionView.ViewTiles(GameUtilities.AreaBuffer);
      _unitSelectionViw.View(unit);
    }

    public void ViewRegionSelection(RegionController region, bool forceBordersAnim)
    {
      _gameplayUI.ShowRegionUI(region);
      _bordersService.ViewRegionSelectionBorders(region, forceBordersAnim);
    }

    public void ViewViewBuildingsProtection(RegionController region) => _protectionView.ViewBuildingsProtection(region);

    public void Clear(bool clearRegionView)
    {
      _tilesSelectionView.ClearView();
      if (clearRegionView)
      {
        _gameplayUI.HideRegionUI();
        _bordersService.ClearRegionSelectionBorders();
      }

      _gameplayUI.ClearRegionCreation();
      ClearUnitSelectionView();
    }

    public void Undo() => _playerController.Undo();

    public void SetCreateUnitMode(UnitType unitType) => _playerController.SetCreateUnitMode(unitType);

    public void ClearUnitSelectionView() => _unitSelectionViw.Hide();
  }
}