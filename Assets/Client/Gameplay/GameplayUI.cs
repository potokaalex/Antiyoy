using Client.Infrastructure;
using Client.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay
{
  public class GameplayUI : MonoBehaviour
  {
    [SerializeField] private GameObject _regionPanel;
    [SerializeField] private Button _createUnitButton;
    [SerializeField] private Button _nextTurnButton;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private TextMeshProUGUI _incomeCount;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Button _winNexButton;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _createUnitButton.onClick.AddListener(_gameplayController.SetCreateUnitMode);
      _nextTurnButton.onClick.AddListener(_gameplayController.NexTurn);
      _winNexButton.onClick.AddListener(_gameplayController.EndGameplay);
    }

    private void OnDestroy()
    {
      _createUnitButton.onClick.RemoveListener(_gameplayController.SetCreateUnitMode);
      _nextTurnButton.onClick.RemoveListener(_gameplayController.NexTurn);
      _winNexButton.onClick.RemoveListener(_gameplayController.EndGameplay);
    }

    public void ActiveRegionUI(bool isActive)
    {
      _regionPanel.SetActive(isActive);
    }

    public void ViewTurnsCount(int value)
    {
      _turnsCount.SetText($"TurnsCount: {value}");
    }

    public void ViewRegionData(int money, int income)
    {
      _moneyCount.SetText($"Currency: {money}");
      _incomeCount.SetText($"Income: {income}");
    }

    public void ShowEndScreen(RegionType winner)
    {
      _winPanel.SetActive(true);
      _winText.SetText($"Winner: {winner}");
    }
  }
}