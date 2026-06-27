using Client.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class Hud : MonoBehaviour
  {
    [SerializeField] private Button _nextTurnButton;
    [SerializeField] private RegionView _regionView;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    private GameplayController _gameplayController;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _nextTurnButton.onClick.AddListener(_gameplayController.NextTurn);
    }

    private void OnDestroy() => _nextTurnButton.onClick.RemoveListener(_gameplayController.NextTurn);

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");
  }
}