using Client.Infrastructure;
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
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _createUnitButton.onClick.AddListener(_gameplayController.SetCreateUnitMode);
      _nextTurnButton.onClick.AddListener(_gameplayController.NexTurn);
    }

    private void OnDestroy()
    {
      _createUnitButton.onClick.RemoveListener(_gameplayController.SetCreateUnitMode);
      _nextTurnButton.onClick.RemoveListener(_gameplayController.NexTurn);
    }

    public void ActiveRegionUI(bool isActive)
    {
      _regionPanel.SetActive(isActive);
    }

    public void ViewTurnsCount(int value)
    {
      _turnsCount.SetText($"TurnsCount: {value}");
    }
  }
}