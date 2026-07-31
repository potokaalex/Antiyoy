using Client._Back;
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
    private BackController _backController;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _backController = Locator.Get<BackController>();
      _nextTurnButton.OnClick += _gameplayController.NextTurn;
      _backButton.OnClick += _backController.Back;
    }

    private void OnDestroy()
    {
      _nextTurnButton.OnClick -= _gameplayController.NextTurn;
      _backButton.OnClick -= _backController.Back;
    }

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");
  }
}