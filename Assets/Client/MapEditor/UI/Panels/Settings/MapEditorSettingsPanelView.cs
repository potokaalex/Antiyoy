using Client.MapEditor.UI.AgreeView;
using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using Client.Project.UI.Slider;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.Panels.Settings
{
  public class MapEditorSettingsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _clearButton;
    [SerializeField] private CustomButton _randomizeButton;
    [SerializeField] private CustomSlider _playersSlider;
    [SerializeField] private CustomSlider _difficultySlider;
    [SerializeField] private CustomButton _playerColorButton;
    [SerializeField] private Image _playerRandomColorImage;
    [SerializeField] private Image _playerColorImage;
    [SerializeField] private Image _playerColorBackground;
    private MapEditorUI _mapEditorUI;
    private MapEditorController _mapEditorController;
    private RegionsService _regionsService;

    public float BottomOffsetY => _topAndBottomMenuAnimator.BottomPanelOffsetY;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _mapEditorController = Locator.Get<MapEditorController>();
      _regionsService = Locator.Get<RegionsService>();

      _clearButton.OnClick += OnClearClick;
      _randomizeButton.OnClick += OnRandomizeClick;
      _playersSlider.OnSliderValueChanged += OnPlayersValueChanged;
      _difficultySlider.OnSliderValueChanged += OnDifficultyValueChanged;
      _playerColorButton.OnClick += OnPlayerColorButtonClick;
      _mapEditorController.OnPlayerRegionTypeChanged += ViewPlayerColor;

      _playerRandomColorImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
      _clearButton.OnClick -= OnClearClick;
      _randomizeButton.OnClick -= OnRandomizeClick;
      _playersSlider.OnSliderValueChanged -= OnPlayersValueChanged;
      _difficultySlider.OnSliderValueChanged -= OnDifficultyValueChanged;
      _playerColorButton.OnClick -= OnPlayerColorButtonClick;
      _mapEditorController.OnPlayerRegionTypeChanged -= ViewPlayerColor;
    }

    public void Show()
    {
      _topAndBottomMenuAnimator.PlayShow();
      _playersSlider.SetValueWithoutNotify(_mapEditorController.PlayersValue);
      _difficultySlider.SetValueWithoutNotify(_mapEditorController.DifficultyValue);
      ViewPlayerColor();
    }

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();

    private void OnPlayerColorButtonClick() => _mapEditorUI.ShowPlayerColorsView();

    private void OnClearClick()
    {
      _mapEditorUI.HideCurrentPanel();
      _mapEditorUI.ShowAgreeView(new MapEditorAgreeViewParameters
      {
        AgreeAction = _mapEditorController.ClearMap,
        Header = "Clear map?",
        CancelColor = new Color(0.35686275f, 0.6509804f, 0.43529412f, 1),
        CancelText = "Cancel",
        AgreeColor = new Color(0.39607844f, 0.4509804f, 0.8f, 1),
        AgreeText = "Clear"
      });
    }

    private void OnRandomizeClick()
    {
      _mapEditorUI.HideCurrentPanel();
      _mapEditorUI.ShowAgreeView(new MapEditorAgreeViewParameters
      {
        AgreeAction = _mapEditorController.GenerateRandomMap,
        Header = "Generate random map?",
        CancelColor = new Color(0.67058825f, 0.6392157f, 0.34901962f, 1),
        CancelText = "Cancel",
        AgreeColor = new Color(0.35686275f, 0.6509804f, 0.43529412f, 1),
        AgreeText = "Yes"
      });
    }

    private void OnPlayersValueChanged() => _mapEditorController.PlayersValue = _playersSlider.Value;

    private void OnDifficultyValueChanged() => _mapEditorController.DifficultyValue = _difficultySlider.Value;

    private void ViewPlayerColor()
    {
      var value = _mapEditorController.PlayerRegionType;
      if (value.IsRandom)
      {
        _playerRandomColorImage.gameObject.SetActive(true);
        _playerColorBackground.color = Color.clear;
        return;
      }

      _playerRandomColorImage.gameObject.SetActive(false);
      var color = _regionsService.GetColorFor(value.RegionType);
      _playerColorImage.color = color;
      color.a = 0.1f;
      _playerColorBackground.color = color;
    }
  }
}