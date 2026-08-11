using Client.Infrastructure;
using Client.UI;
using UnityEngine;

namespace Client.Menu.MainMenu.Options
{
  public class MainMenuOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private MainMenuView _mainMenuView;

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _backButton.OnClick += OnBackClick;
    }

    private void OnDestroy() => _backButton.OnClick -= OnBackClick;

    public void Show() => _menuAnimator.PlayShow();

    private void OnBackClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.ShowStart();
    }
  }
}