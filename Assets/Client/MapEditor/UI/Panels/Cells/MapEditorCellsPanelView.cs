using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using TMPro;
using UnityEngine;

namespace Client.MapEditor.UI.Panels.Cells
{
  public class MapEditorCellsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private MapEditorCellButton[] _cellsButtons;
    [SerializeField] private CustomButton _deleteCellButton;
    [SerializeField] private CustomButton _randomCellButton;
    [SerializeField] private CustomButton _filterButton;
    [SerializeField] private TextMeshProUGUI _filterText;
    private MapEditorController _mapEditorController;
    private MapEditorUI _mapEditorUI;
    private bool _onlyLand;

    public float BottomOffsetY => _topAndBottomMenuAnimator.BottomPanelOffsetY;

    private void Awake()
    {
      _mapEditorController = Locator.Get<MapEditorController>();
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _deleteCellButton.OnClick += OnDeleteClick;
      _randomCellButton.OnClick += OnRandomClick;
      _filterButton.OnClick += OnFilterClick;
    }

    private void OnDestroy()
    {
      _deleteCellButton.OnClick -= OnDeleteClick;
      _randomCellButton.OnClick -= OnRandomClick;
      _filterButton.OnClick -= OnFilterClick;
    }

    public void Show()
    {
      _topAndBottomMenuAnimator.PlayShow();
      foreach (var button in _cellsButtons)
        button.Setup(this);
    }

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();

    public void CreateCellMode(RegionType regionType)
    {
      _mapEditorUI.HideCurrentPanel();
      _mapEditorController.CreateCellMode(regionType);
    }

    private void OnRandomClick()
    {
      _mapEditorUI.HideCurrentPanel();
      _mapEditorController.CreateRandomCellMode();
    }

    private void OnDeleteClick()
    {
      _mapEditorUI.HideCurrentPanel();
      _mapEditorController.DestroyCellMode();
    }

    private void OnFilterClick()
    {
      _onlyLand = !_onlyLand;
      _filterText.SetText(_onlyLand ? "Only land" : "No filter");
      _mapEditorController.SetCellModeOnlyLand(_onlyLand);
    }
  }
}