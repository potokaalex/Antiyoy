using Client.Infrastructure;
using Client.Region;
using Client.Unit.Code;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay
{
  public class GameplayUI : MonoBehaviour
  {
    [SerializeField] private GameObject _regionPanel;
    [SerializeField] private Button _createPeasantButton;
    [SerializeField] private Button _createBuildingButton;
    [SerializeField] private TextMeshProUGUI _createBuildingButtonText;
    [SerializeField] private TextMeshProUGUI _unitPrice;
    [SerializeField] private Button _nextTurnButton;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private TextMeshProUGUI _incomeCount;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Button _winNexButton;
    private GameplayController _gameplayController;
    private UnitType _currentBuildingType;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _createPeasantButton.onClick.AddListener(OnCreatePeasant);
      _createBuildingButton.onClick.AddListener(OnCreateBuilding);
      _nextTurnButton.onClick.AddListener(_gameplayController.NextTurn);
      _winNexButton.onClick.AddListener(_gameplayController.EndGameplay);
      ViewCurrentBuildingText();
    }

    private void OnDestroy()
    {
      _createPeasantButton.onClick.RemoveListener(OnCreatePeasant);
      _createBuildingButton.onClick.RemoveListener(OnCreateBuilding);
      _nextTurnButton.onClick.RemoveListener(_gameplayController.NextTurn);
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

    public void ViewUnitPrice(int value)
    {
      _unitPrice.SetText(value > 0 ? $"Price: {value}" : string.Empty);
    }

    public void ClearCurrentBuilding()
    {
      _currentBuildingType = UnitType.None;
      ViewCurrentBuildingText();
    }

    private void OnCreatePeasant()
    {
      _gameplayController.SetCreateUnitMode(UnitType.Peasant);
    }

    private void OnCreateBuilding()
    {
      if (_currentBuildingType == UnitType.None)
        _currentBuildingType = UnitType.Farm;
      else if (_currentBuildingType == UnitType.Farm)
        _currentBuildingType = UnitType.Tower;
      else if (_currentBuildingType == UnitType.Tower)
        _currentBuildingType = UnitType.Farm;

      _gameplayController.SetCreateUnitMode(_currentBuildingType);
      ViewCurrentBuildingText();
    }

    private void ViewCurrentBuildingText() =>
      _createBuildingButtonText.SetText(_currentBuildingType == UnitType.None ? "Farm" : _currentBuildingType.ToString());
  }
}