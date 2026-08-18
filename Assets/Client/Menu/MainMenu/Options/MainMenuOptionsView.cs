using Client.Infrastructure;
using Client.Project;
using Client.UI;
using UnityEngine;

namespace Client.Menu.MainMenu.Options
{
  public class MainMenuOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _battleButton;
    [SerializeField] private MenuAnimator _menuAnimator;
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
      _battleButton.OnClick += OnBattleClick;
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _battleButton.OnClick -= OnBattleClick;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        OnBackClick();
    }

    private void OnBackClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.ShowStart();
    }

    private void OnBattleClick() => _projectController.LoadGameplay(_menuAnimator.PlayHide);
  }
}