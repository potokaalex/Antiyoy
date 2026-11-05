using Client.MapEditor.UI.Panels;
using Client.MapEditor.UI.Panels.Cells;
using Client.MapEditor.UI.Panels.Settings;
using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorHud : MonoBehaviour
  {
    [SerializeField] private CustomButton _pauseButton;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _openCellsPanelButton;
    [SerializeField] private CustomButton _moveButton;
    [SerializeField] private CustomButton _openUnitsPanelButton;
    [SerializeField] private CustomButton _regionSettingsButton;
    [SerializeField] private CustomButton _settingsButton;
    [SerializeField] private MapEditorCellsPanelView _cellsPanelView;
    [SerializeField] private MapEditorUnitsPanelView _unitsPanelView;
    [SerializeField] private MapEditorSettingsPanelView _settingsPanelView;
    private MapEditorUI _mapEditorUI;
    private MapEditorController _mapEditorController;
    private IMapEditorHudPanelView _currentPanelView;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _mapEditorController = Locator.Get<MapEditorController>();

      _pauseButton.OnClick += OnPauseClick;
      _openCellsPanelButton.OnClick += OnCellsPanelButtonClick;
      _openUnitsPanelButton.OnClick += OnUnitsPanelButtonClick;
      _moveButton.OnClick += OnMoveButtonClick;
      _regionSettingsButton.OnClick += OnRegionSettingsButtonClick;
      _settingsButton.OnClick += OnSettingsButtonClick;
    }

    private void OnDestroy()
    {
      _pauseButton.OnClick -= OnPauseClick;
      _openCellsPanelButton.OnClick -= OnCellsPanelButtonClick;
      _openUnitsPanelButton.OnClick -= OnUnitsPanelButtonClick;
      _moveButton.OnClick -= OnMoveButtonClick;
      _regionSettingsButton.OnClick -= OnRegionSettingsButtonClick;
      _settingsButton.OnClick -= OnSettingsButtonClick;
    }

    public void Show()
    {
      _topAndBottomMenuAnimator.BottomPanelOffsetY = _currentPanelView?.BottomOffsetY ?? -200;
      _topAndBottomMenuAnimator.SpeedFactor = 1f;
      _topAndBottomMenuAnimator.PlayShow();
    }

    public void Hide()
    {
      _topAndBottomMenuAnimator.SpeedFactor = 2;
      _topAndBottomMenuAnimator.PlayHide();
    }

    public void Clear() => HideCurrentPanel();

    public void HideCurrentPanel()
    {
      if (_currentPanelView != null)
      {
        _currentPanelView.Hide();
        _currentPanelView = null;
      }
    }

    private void OnPauseClick()
    {
      _mapEditorUI.PlayHide();
      _mapEditorUI.ShowPause();
    }

    private void OnCellsPanelButtonClick() => SelectPanel(_cellsPanelView);

    private void OnUnitsPanelButtonClick() => SelectPanel(_unitsPanelView);

    private void SelectPanel(IMapEditorHudPanelView panelView)
    {
      if (panelView == null)
        return;

      if (_currentPanelView == panelView)
      {
        HideCurrentPanel();
        return;
      }

      _currentPanelView?.Hide();
      _currentPanelView = panelView;
      _currentPanelView.Show();
    }

    private void OnMoveButtonClick()
    {
      HideCurrentPanel();
      _mapEditorController.MoveMode();
    }

    private void OnRegionSettingsButtonClick()
    {
      HideCurrentPanel();
      _mapEditorController.RegionsMode();
    }

    private void OnSettingsButtonClick() => SelectPanel(_settingsPanelView);
  }
}