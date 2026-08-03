using Client.ActionsHistory;
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
    [SerializeField] private CustomButton _backButton;
    private GameplayController _gameplayController;
    private ActionsHistoryController _actionsHistoryController;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _actionsHistoryController = Locator.Get<ActionsHistoryController>();
      _nextTurnButton.OnClick += _gameplayController.NextTurn;
      _backButton.OnClick += _actionsHistoryController.Undo;
    }

    private void OnDestroy()
    {
      _nextTurnButton.OnClick -= _gameplayController.NextTurn;
      _backButton.OnClick -= _actionsHistoryController.Undo;
    }

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");
  }
}