using Client.Gameplay.Gameplay;
using Client.Menu;
using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class PauseView : MonoBehaviour
  {
    [SerializeField] private CustomButton _continueButton;
    [SerializeField] private CustomButton _mainMenuButton;
    [SerializeField] private CustomButton _saveButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private GameplayController _gameplayController;
    private InputController _inputController;
    private GameplayUI _gameplayUI;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _inputController = Locator.Get<InputController>();
      _continueButton.OnClick += _gameplayController.UnPause;
      _mainMenuButton.OnClick += ToMainMenu;
      _saveButton.OnClick += OnSaveClick;
    }

    private void OnDestroy()
    {
      _continueButton.OnClick -= _gameplayController.UnPause;
      _mainMenuButton.OnClick -= ToMainMenu;
      _saveButton.OnClick -= OnSaveClick;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        ToMainMenu();
    }

    public void PlayShow() => _menuAnimator.PlayShow();

    public void PlayHide() => _menuAnimator.PlayHide();

    private void ToMainMenu()
    {
      PlayHide();
      _gameplayController.MainMenu();
    }

    private void OnSaveClick()
    {
      PlayHide();
      _gameplayUI.OpenSaveView();
    }
  }
}