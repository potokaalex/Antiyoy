using Client.MapEditor.UI.Panels;
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
    [SerializeField] private MapEditorPlayerColorsView _playerColorsView;
    private MapEditorUI _mapEditorUI;
    private IMapEditorHudPanelView _currentPanelView;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
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

    private void HideCurrentPanel()
    {
      if (_currentPanelView != null)
      {
        _currentPanelView.Hide();
        _currentPanelView = null;
      }
    }

    private void OnMoveButtonClick()
    {
      HideCurrentPanel();
    }

    private void OnRegionSettingsButtonClick()
    {
      HideCurrentPanel();
    }

    private void OnSettingsButtonClick() => SelectPanel(_settingsPanelView);

    public void ShowPlayerColorsView() => _playerColorsView.Show();
  }
}