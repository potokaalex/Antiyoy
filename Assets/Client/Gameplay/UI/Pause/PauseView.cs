using Client.Infrastructure;
using Client.Menu;
using Client.UI;
using UnityEngine;

namespace Client.Gameplay.UI.Pause
{
  public class PauseView : MonoBehaviour
  {
    [SerializeField] private CustomButton _continueButton;
    [SerializeField] private CustomButton _mainMenuButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private GameplayController _gameplayController;
    private InputController _inputController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _inputController = Locator.Get<InputController>();
      _continueButton.OnClick += _gameplayController.UnPause;
      _mainMenuButton.OnClick += ToMainMenu;
    }

    private void OnDestroy()
    {
      _continueButton.OnClick -= _gameplayController.UnPause;
      _mainMenuButton.OnClick -= ToMainMenu;
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
  }
}