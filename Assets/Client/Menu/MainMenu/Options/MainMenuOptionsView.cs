using Client.Infrastructure;
using Client.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Menu.MainMenu.Options
{
  public class MainMenuOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _battleButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    private MainMenuView _mainMenuView;

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _backButton.OnClick += OnBackClick;
      _battleButton.OnClick += OnBattleClick;
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _battleButton.OnClick -= OnBattleClick;
    }

    public void Show() => _menuAnimator.PlayShow();

    private void OnBackClick()
    {
      _menuAnimator.PlayHide();
      _mainMenuView.ShowStart();
    }

    private void OnBattleClick()
    {
      _menuAnimator.PlayHide();
      SceneManager.LoadScene(1);
    }
  }
}