using Client.Infrastructure;
using Client.Unit.Code;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class RegionCreationView : MonoBehaviour
  {
    [SerializeField] private Button _createWarriorButton;
    [SerializeField] private Button _createBuildingButton;
    [SerializeField] private TextMeshProUGUI _variantCost;
    [SerializeField] private Image _variantIcon;
    [SerializeField] private GameObject _variantPanel;
    private UnitsService _unitsService;
    private UnitType _buildingType;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _unitsService = Locator.Get<UnitsService>();
      _createWarriorButton.onClick.AddListener(OnCreateWarrior);
      _createBuildingButton.onClick.AddListener(OnCreateBuilding);
    }

    private void OnDestroy()
    {
      _createWarriorButton.onClick.RemoveListener(OnCreateWarrior);
      _createBuildingButton.onClick.RemoveListener(OnCreateBuilding);
    }

    public void Clear()
    {
      _buildingType = UnitType.None;
      _variantPanel.SetActive(false);
    }
    
    private void OnCreateWarrior()
    {
      _gameplayController.SetCreateUnitMode(UnitType.Peasant);
      _buildingType = UnitType.None;
    }

    private void OnCreateBuilding()
    {
      if (_buildingType == UnitType.None)
        _buildingType = UnitType.Farm;
      else if (_buildingType == UnitType.Farm)
        _buildingType = UnitType.Tower;
      else if (_buildingType == UnitType.Tower)
        _buildingType = UnitType.Farm;

      _gameplayController.SetCreateUnitMode(_buildingType);
      View(_buildingType);
    }

    public void View(UnitType unitType)
    {
      _variantPanel.SetActive(true);
      _variantCost.SetText($"${_unitsService.GetCost(unitType)}");
      _variantIcon.sprite = _unitsService.GetSprite(unitType);
    }
  }
}