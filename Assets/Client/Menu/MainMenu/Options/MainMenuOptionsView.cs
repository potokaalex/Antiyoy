using System.Collections;
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
    private InputController _inputController;

    public void Show() => _menuAnimator.PlayShow();

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
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

    private void OnBattleClick()
    {
      StartCoroutine(LoadGameplay());
    }

    private IEnumerator LoadGameplay()
    {
      var operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
      yield return operation;
      var loadedScene = SceneManager.GetSceneByBuildIndex(1);
      SceneManager.SetActiveScene(loadedScene);
      _menuAnimator.PlayHide();
    }
  }
}