using Client.Infrastructure;
using Client.UI;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class Hud : MonoBehaviour
  {
    [SerializeField] private CustomButton _nextTurnButton;
    [SerializeField] private RegionView _regionView;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    private GameplayController _gameplayController;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _nextTurnButton.OnClick += _gameplayController.NextTurn;
    }

    private void OnDestroy() => _nextTurnButton.OnClick -= _gameplayController.NextTurn;

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");
  }
}