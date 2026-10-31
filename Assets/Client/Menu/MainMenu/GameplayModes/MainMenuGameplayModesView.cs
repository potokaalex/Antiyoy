using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Progress;
using Client.Project.Project;
using Client.Project.UI;
using UnityEngine;

namespace Client.Menu.MainMenu.GameplayModes
{
  public class MainMenuGameplayModesView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _startButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private GameplayModeSlider _difficulty;
    [SerializeField] private GameplayModeSlider _mapSize;
    [SerializeField] private GameplayModeSlider _players;
    [SerializeField] private GameplayModeSlider _colors;
    private MainMenuView _mainMenuView;
    private InputController _inputController;
    private ProjectController _projectController;
    private ProgressController _progressController;
    private MapsService _mapsService;

    public void Show()
    {
      _menuAnimator.PlayShow();
      _topAndBottomMenuAnimator.PlayShow();
    }

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
      _projectController = Locator.Get<ProjectController>();
      _progressController = Locator.Get<ProgressController>();
      _mapsService = Locator.Get<MapsService>();

      LoadData();
      _backButton.OnClick += OnBackClick;
      _startButton.OnClick += OnStartClick;
      _difficulty.OnSliderValueChanged += OnSlidersValueChanged;
      _mapSize.OnSliderValueChanged += OnSlidersValueChanged;
      _players.OnSliderValueChanged += OnSlidersValueChanged;
      _colors.OnSliderValueChanged += OnSlidersValueChanged;
      OnSlidersValueChanged();
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _startButton.OnClick -= OnStartClick;
      _difficulty.OnSliderValueChanged -= OnSlidersValueChanged;
      _mapSize.OnSliderValueChanged -= OnSlidersValueChanged;
      _players.OnSliderValueChanged -= OnSlidersValueChanged;
      _colors.OnSliderValueChanged -= OnSlidersValueChanged;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        OnBackClick();
    }

    private void OnBackClick()
    {
      _menuAnimator.PlayHide();
      _topAndBottomMenuAnimator.PlayHide();
      _mainMenuView.ShowOptions();
    }

    private void OnStartClick()
    {
      _mapsService.GenerateStartMap((LevelSize)_mapSize.Value, _colors.Value + 2);
      _projectController.LoadGameplay();
      _menuAnimator.PlayHide();
      _topAndBottomMenuAnimator.PlayHide();
    }

    private void OnSlidersValueChanged()
    {
      UpdateSlidersMaxValues();
      SaveData();
    }

    private void UpdateSlidersMaxValues()
    {
      if (_mapSize.Value == 0)
        _colors.MaxValue = 3;
      else if (_mapSize.Value == 1)
        _colors.MaxValue = 7;
      else if (_mapSize.Value >= 2)
        _colors.MaxValue = 8;

      _players.MaxValue = _colors.Value + 2;
    }

    private void SaveData()
    {
      var data = _progressController.Data.Menu.GameplayModesProgressData;
      data.Difficulty = _difficulty.Value;
      data.MapSize = _mapSize.Value;
      data.Players = _players.Value;
      data.Colors = _colors.Value;
      _progressController.Save();
    }

    private void LoadData()
    {
      var data = _progressController.Data.Menu.GameplayModesProgressData;
      _difficulty.Value = data.Difficulty;
      _mapSize.Value = data.MapSize;
      _players.Value = data.Players;
      _colors.Value = data.Colors;
    }
  }
}