using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class GameplayUI : MonoBehaviour
  {
    [SerializeField] private Hud _hud;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Button _winNexButton;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _winNexButton.onClick.AddListener(_gameplayController.EndGameplay);
    }

    private void OnDestroy() => _winNexButton.onClick.RemoveListener(_gameplayController.EndGameplay);

    public void ActiveRegionUI(bool isActive) => _hud.Region.SetActive(isActive);

    public void ViewTurnsCount(int value) => _hud.ViewTurnsCount(value);

    public void ViewRegionData(int money, int income)
    {
      _hud.Region.ViewMoney(money);
      _hud.Region.ViewIncome(income);
    }

    public void ShowEndScreen(RegionType winner)
    {
      _winPanel.SetActive(true);
      _winText.SetText($"Winner: {winner}");
    }

    public void ClearRegionCreation() => _hud.Region.Creation.Clear();
  }
}