using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.UI;
using Client.Project.Unit.Code;
using UnityEngine;

namespace Client.MapEditor.UI.Panels
{
  public class MapEditorUnitsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _deleteUnitButton;
    [SerializeField] private CustomButton _pineButton;
    [SerializeField] private CustomButton _palmButton;
    [SerializeField] private CustomButton _farmButton;
    [SerializeField] private CustomButton _towerButton;
    [SerializeField] private CustomButton _peasantButton;
    [SerializeField] private CustomButton _spearmanButton;
    private MapEditorController _mapEditorController;

    public float BottomOffsetY => _topAndBottomMenuAnimator.BottomPanelOffsetY;

    private void Awake()
    {
      _mapEditorController = Locator.Get<MapEditorController>();
      _deleteUnitButton.OnClick += OnDeleteClick;
      _pineButton.OnClick += OnPineClick;
      _palmButton.OnClick += OnPalmClick;
      _farmButton.OnClick += OnFarmClick;
      _towerButton.OnClick += OnTowerClick;
      _peasantButton.OnClick += OnPeasantClick;
      _spearmanButton.OnClick += OnSpearmanClick;
    }

    private void OnDestroy()
    {
      _deleteUnitButton.OnClick -= OnDeleteClick;
      _pineButton.OnClick -= OnPineClick;
      _palmButton.OnClick -= OnPalmClick;
      _farmButton.OnClick -= OnFarmClick;
      _towerButton.OnClick -= OnTowerClick;
      _peasantButton.OnClick -= OnPeasantClick;
      _spearmanButton.OnClick -= OnSpearmanClick;
    }

    public void Show() => _topAndBottomMenuAnimator.PlayShow();

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();

    private void OnSpearmanClick() => _mapEditorController.CreateUnitMode(UnitType.Spearman);

    private void OnPeasantClick() => _mapEditorController.CreateUnitMode(UnitType.Peasant);

    private void OnTowerClick() => _mapEditorController.CreateUnitMode(UnitType.Tower);

    private void OnFarmClick() => _mapEditorController.CreateUnitMode(UnitType.Farm);

    private void OnPalmClick() => _mapEditorController.CreateUnitMode(UnitType.Palm);

    private void OnPineClick() => _mapEditorController.CreateUnitMode(UnitType.Pine);

    private void OnDeleteClick() => _mapEditorController.DeleteUnitMode();
  }
}