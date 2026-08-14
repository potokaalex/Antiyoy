using Client.Infrastructure;
using Client.UI;
using Client.Unit.Code;
using Client.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class RegionCreationView : MonoBehaviour
  {
    [SerializeField] private CustomButton _createWarriorButton;
    [SerializeField] private CustomButton _createBuildingButton;
    [SerializeField] private RectTransform _variantPanel;
    [SerializeField] private CanvasGroup _variantPanelCanvasGroup;
    [SerializeField] private TextMeshProUGUI _variantCost;
    [SerializeField] private Image _variantIcon;
    private UnitsService _unitsService;
    private UnitType _buildingType;
    private GameplayController _gameplayController;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _unitsService = Locator.Get<UnitsService>();
      _createWarriorButton.OnClick += OnCreateWarrior;
      _createBuildingButton.OnClick += OnCreateBuilding;
    }

    private void OnDestroy()
    {
      _createWarriorButton.OnClick -= OnCreateWarrior;
      _createBuildingButton.OnClick -= OnCreateBuilding;
    }

    public void Clear()
    {
      _buildingType = UnitType.None;
      SetActive(false);
    }

    private void OnCreateWarrior()
    {
      _buildingType = UnitType.Peasant;
      _gameplayController.SetCreateUnitMode(_buildingType);
      View(_buildingType);
    }

    private void OnCreateBuilding()
    {
      if (!_buildingType.IsBuilding())
        _buildingType = UnitType.Farm;
      else if (_buildingType == UnitType.Farm)
        _buildingType = UnitType.Tower;
      else if (_buildingType == UnitType.Tower)
        _buildingType = UnitType.Farm;

      _gameplayController.SetCreateUnitMode(_buildingType);
      View(_buildingType);
    }

    private void View(UnitType unitType)
    {
      _variantCost.SetText($"${_unitsService.GetCost(unitType)}");
      _variantIcon.sprite = _unitsService.GetSprite(unitType);
      SetActive(true);
    }

    private void SetActive(bool isActive)
    {
      DOTween.Kill(this);
      
      if (isActive)
      {
        _variantPanel.gameObject.SetActive(true);
        AnimationsUtilities.DoAnchoredMove(_variantPanel, new Vector2(0, -150), new Vector2(0, 0));
        AnimationsUtilities.DoFade(_variantPanelCanvasGroup, 0, 1);
      }
      else
      {
        AnimationsUtilities.DoAnchoredMove(_variantPanel, new Vector2(0, 0), new Vector2(0, -150));
        AnimationsUtilities.DoFade(_variantPanelCanvasGroup, 1, 0).SetId(this)
          .onComplete += () => _variantPanel.gameObject.SetActive(false);
      }
    }
  }
}