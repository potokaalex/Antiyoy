using Client.Project;
using Client.Project.Infrastructure;
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
    [SerializeField] private GameplayModeSlider _difficulty;
    [SerializeField] private GameplayModeSlider _mapSize;
    [SerializeField] private GameplayModeSlider _players;
    [SerializeField] private GameplayModeSlider _colors;
    private MainMenuView _mainMenuView;
    private InputController _inputController;
    private ProjectController _projectController;

    public void Show() => _menuAnimator.PlayShow();

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
      _projectController = Locator.Get<ProjectController>();
      _backButton.OnClick += OnBackClick;
      _startButton.OnClick += OnStartClick;
      _mapSize.OnSliderValueChanged += UpdateSliders;
      _colors.OnSliderValueChanged += UpdateSliders;
      UpdateSliders();
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _startButton.OnClick -= OnStartClick;
      _mapSize.OnSliderValueChanged -= UpdateSliders;
      _colors.OnSliderValueChanged -= UpdateSliders;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        OnBackClick();
    }

    private void OnBackClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.ShowOptions();
    }

    private void OnStartClick()
    {
      _projectController.LoadGameplay();
      _menuAnimator.PlayHide();
    }

    private void UpdateSliders()
    {
      if (_mapSize.Value == 0)
        _colors.MaxValue = 3;
      else if(_mapSize.Value == 1)
        _colors.MaxValue = 7;
      else if(_mapSize.Value >= 2)
        _colors.MaxValue = 8;

      _players.MaxValue = _colors.Value + 2;
    }
  }
}