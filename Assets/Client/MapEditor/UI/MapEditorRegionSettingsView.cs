using System;
using Client.Menu;
using Client.Project.Borders;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using Client.Project.UI.Slider;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorRegionSettingsView : MonoBehaviour
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private CustomSlider _customSlider;
    private readonly int[] _values = { 0, 5, 10, 25, 50, 75, 100, 125, 150, 200, 250, 500, 1000, 2500, 5000, 10000 };
    private BordersService _bordersService;
    private RegionController _region;

    private void Awake()
    {
      _bordersService = Locator.Get<BordersService>();
      _customSlider.OnSliderValueChanged += OnSliderValueChanged;
      _closeButton.OnClick += Hide;
    }

    private void OnDestroy()
    {
      _customSlider.OnSliderValueChanged += OnSliderValueChanged;
      _closeButton.OnClick -= Hide;
    }

    public void Show(RegionController region)
    {
      _region = region;
      _topAndBottomMenuAnimator.PlayShow();
      ViewRegionMoney();
      _customSlider.SetValueWithoutNotify(GetClosestValueIndex(_region.Money));
      _bordersService.ViewRegionSelectionBorders(region, false);
    }

    private void Hide()
    {
      _bordersService.ClearRegionSelectionBorders();
      _topAndBottomMenuAnimator.PlayHide();
    }

    private void OnSliderValueChanged()
    {
      _region.Money = _values[_customSlider.Value];
      ViewRegionMoney();
    }

    private int GetClosestValueIndex(int money)
    {
      var closestIndex = 0;
      var minDifference = Math.Abs(money - _values[0]);

      for (var i = 1; i < _values.Length; i++)
      {
        var difference = Math.Abs(money - _values[i]);
        if (difference < minDifference)
        {
          minDifference = difference;
          closestIndex = i;
        }
      }

      return closestIndex;
    }

    private void ViewRegionMoney() => _customSlider.ValueName = $"${_region.Money}";
  }
}