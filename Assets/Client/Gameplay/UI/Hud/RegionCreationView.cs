using System.Collections.Generic;
using Client.Gameplay.Player;
using Client.Infrastructure;
using Client.UI;
using Client.Unit.Code;
using Client.Utilities;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI.Hud
{
  public class RegionCreationView : SerializedMonoBehaviour
  {
    [SerializeField] private CustomButton _createWarriorButton;
    [SerializeField] private CustomButton _createBuildingButton;
    [SerializeField] private RectTransform _variantPanel;
    [SerializeField] private CanvasGroup _variantPanelCanvasGroup;
    [SerializeField] private TextMeshProUGUI _variantCost;
    [SerializeField] private Dictionary<UnitType, Image> _icons;
    private UnitsService _unitsService;
    private PlayerViewController _playerViewController;
    private UnitType _buildingType;

    private void Awake()
    {
      _playerViewController = Locator.Get<PlayerViewController>();
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
      if (!_buildingType.IsWarrior())
        _buildingType = UnitType.Peasant;
      else if (_buildingType == UnitType.Peasant)
        _buildingType = UnitType.Spearman;
      else if (_buildingType == UnitType.Spearman)
        _buildingType = UnitType.Infantryman;
      else if (_buildingType == UnitType.Infantryman)
        _buildingType = UnitType.Knight;
      else if (_buildingType == UnitType.Knight)
        _buildingType = UnitType.Peasant;

      _playerViewController.SetCreateUnitMode(_buildingType);
      View(_buildingType);
    }

    private void OnCreateBuilding()
    {
      if (!_buildingType.IsBuilding())
        _buildingType = UnitType.Farm;
      else if (_buildingType == UnitType.Farm)
        _buildingType = UnitType.Tower;
      else if (_buildingType == UnitType.Tower)
        _buildingType = UnitType.StrongTower;
      else if (_buildingType == UnitType.StrongTower)
        _buildingType = UnitType.Farm;

      _playerViewController.SetCreateUnitMode(_buildingType);
      View(_buildingType);
    }

    private void View(UnitType unitType)
    {
      if(unitType == UnitType.None)
        return;

      _variantCost.SetText($"${_unitsService.GetCost(unitType)}");
      ActiveIcon(unitType);
      SetActive(true);
    }

    private void ActiveIcon(UnitType unitType)
    {
      foreach (var icon in _icons.Values) 
        icon.gameObject.SetActive(false);

      _icons[unitType].gameObject.SetActive(true);
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