using Client.Gameplay.UI.Hud;
using Client.Gameplay.UI.Pause;
using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class GameplayUIView : MonoBehaviour
  {
    [SerializeField] private HudView _hudView;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Button _winNexButton;
    [SerializeField] private GameTransitionView _gameTransitionView;
    [SerializeField] private PauseView _pauseView;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _winNexButton.onClick.AddListener(_gameplayController.End);
    }

    private void OnDestroy() => _winNexButton.onClick.RemoveListener(_gameplayController.End);

    public void PlayShow()
    {
      _hudView.PlayShow();
      _gameTransitionView.PlayToyGameTransition();
    }

    public void ShowRegionUI(RegionController region)
    {
      _hudView.Region.SetActive(true);
      _hudView.Region.ViewMoney(region.Money);
      _hudView.Region.ViewIncome(region.GetIncome());
    }

    public void HideRegionUI() => _hudView.Region.SetActive(false);

    public void ViewTurnsCount(int value) => _hudView.ViewTurnsCount(value);

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
      _gameTransitionView.PlayOutGameTransition();
    }

    public void HidePause()
    {
      _pauseView.PlayHide();
      _hudView.PlayShow();
      _gameTransitionView.PlayToyGameTransition();
    }
  }
}