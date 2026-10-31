using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.Menu.MainMenu.Options
{
  public class MainMenuOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _battleButton;
    [SerializeField] private CustomButton _loadButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private MainMenuView _mainMenuView;
    private InputController _inputController;

    public void Show() => _menuAnimator.PlayShow();

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
      _backButton.OnClick += OnBackClick;
      _battleButton.OnClick += OnBattleClick;
      _loadButton.OnClick += OnLoadClick;
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _battleButton.OnClick -= OnBattleClick;
      _loadButton.OnClick -= OnLoadClick;
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

    private void OnBattleClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.GameplayModesView.Show();
    }

    private void OnLoadClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.ShowLoad();
    }
  }
}