using Client.Gameplay.UI.Hud;
using Client.Gameplay.UI.Pause;
using Client.Infrastructure;
using Client.Menu;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class GameplayUI : MonoBehaviour
  {
    [SerializeField] private HudView _hudView;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Button _winNexButton;
    [SerializeField] private GameTransitionView _gameTransitionView;
    [SerializeField] private PauseView _pauseViewPrefab;
    private GameplayController _gameplayController;
    private PauseView _pauseView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _pauseView = Locator.Get<MenuView>().Spawn(_pauseViewPrefab);
      _winNexButton.onClick.AddListener(_gameplayController.End);
    }

    private void OnDestroy()
    {
      _winNexButton.onClick.RemoveListener(_gameplayController.End);
      if (_pauseView)
        Destroy(_pauseView.gameObject);
    }

    public void PlayShow()
    {
      _hudView.PlayShow();
      _gameTransitionView.PlaToyGameTransition();
    }

    public void ActiveRegionUI(bool isActive) => _hudView.Region.SetActive(isActive);

    public void ViewTurnsCount(int value) => _hudView.ViewTurnsCount(value);

    public void ViewRegionData(int money, int income)
    {
      _hudView.Region.ViewMoney(money);
      _hudView.Region.ViewIncome(income);
    }

    public void ShowEndScreen(RegionType winner)
    {
      _winPanel.SetActive(true);
      _winText.SetText($"Winner: {winner}");
    }

    public void ClearRegionCreation() => _hudView.Region.Creation.Clear();

    public void ShowPause()
    {
      _hudView.PlayHide();
      _pauseView.PlayShow();
      _gameTransitionView.PlaOutGameTransition();
    }

    public void HidePause()
    {
      _pauseView.PlayHide();
      _hudView.PlayShow();
      _gameTransitionView.PlaToyGameTransition();
    }
  }
}