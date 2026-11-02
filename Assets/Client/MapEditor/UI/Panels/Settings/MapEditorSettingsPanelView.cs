using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.UI;
using Client.Project.UI.Slider;
using UnityEngine;

namespace Client.MapEditor.UI.Panels
{
  public class MapEditorSettingsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _clearButton;
    [SerializeField] private CustomButton _randomizeButton;
    [SerializeField] private CustomSlider _playersSlider;
    [SerializeField] private CustomSlider _difficultySlider;
    [SerializeField] private CustomButton _playerColorButton;
    private MapEditorUI _mapEditorUI;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _playerColorButton.OnClick += OnPlayerColorButtonClick;
    }

    private void OnDestroy() => _playerColorButton.OnClick -= OnPlayerColorButtonClick;

    public void Show() => _topAndBottomMenuAnimator.PlayShow();

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();

    private void OnPlayerColorButtonClick() => _mapEditorUI.ShowPlayerColorsView();
  }
}