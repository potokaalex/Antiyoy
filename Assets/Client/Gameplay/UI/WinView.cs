using Client.Gameplay.Gameplay;
using Client.Menu;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class WinView : MonoBehaviour
  {
    [SerializeField] private CustomButton _mainMenuButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private Image _winnerImage;
    private GameplayController _gameplayController;
    private RegionsService _regionsService;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _regionsService = Locator.Get<RegionsService>();
      _mainMenuButton.OnClick += ToMainMenu;
    }

    private void OnDestroy() => _mainMenuButton.OnClick -= ToMainMenu;

    public void PlayShow(RegionType winner)
    {
      _winnerImage.color = _regionsService.GetColorFor(winner);
      _menuAnimator.PlayShow();
    }

    private void ToMainMenu()
    {
      _menuAnimator.PlayHide();
      _gameplayController.MainMenu();
    }
  }
}