using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.MainMenu.GameplayModes
{
  [ExecuteAlways]
  public class GameplayModeSlider : MonoBehaviour
  {
    [SerializeField] private Slider _slider;
    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
    [SerializeField] private TextMeshProUGUI _currentValueName;
    [SerializeField] private GameplayModeSliderValue[] _allValues;
    [SerializeField] private float _pointSize;
    [SerializeField] [Min(0)] private int _maxValue;
    private bool _recalculateRequest;

    public event Action OnSliderValueChanged;

    public int Value => (int)_slider.value;

    public int MaxValue
    {
      set
      {
        _maxValue = value;
        _slider.maxValue = value;
        _recalculateRequest = true;
      }
    }

    private void OnValidate() => _recalculateRequest = true;

    private void Awake()
    {
      if (Application.isPlaying && _slider)
      {
        _slider.onValueChanged.AddListener(SliderOnValueChanged);
        ViewValueName(_slider.value);
      }
    }

    private void OnDestroy()
    {
      if (_slider)
        _slider.onValueChanged.RemoveListener(SliderOnValueChanged);
    }

    private void Update()
    {
      if (_recalculateRequest)
      {
        Recalculate();
        _recalculateRequest = false;
      }
    }

    public void SetValueWithoutNotify(int value) => _slider.SetValueWithoutNotify(value);

    private void SliderOnValueChanged(float value)
    {
      ViewValueName(value);
      OnSliderValueChanged?.Invoke();
    }

    private void Recalculate()
    {
      if (!_slider || !_horizontalLayoutGroup || _allValues == null)
        return;

      _slider.maxValue = _maxValue;

      var valuesCount = _maxValue + 1;
      for (var i = 0; i < _allValues.Length; i++)
        if (_allValues[i].Point)
          _allValues[i].Point.SetActive(i < valuesCount);

      var width = ((RectTransform)_horizontalLayoutGroup.transform).rect.width - _pointSize * valuesCount;

      var divValue = Mathf.Max(1, _maxValue);
      _horizontalLayoutGroup.spacing = width / divValue;
    }

    private void ViewValueName(float value)
    {
      var index = (int)value;
      if (_allValues != null && _allValues.Length > index)
        _currentValueName.SetText(_allValues[index].Name);
    }
  }
}