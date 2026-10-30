using System;
using System.Globalization;
using Client.Project.Map.Data;
using Client.Project.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class SaveLoadSlotView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _dateAndTime;
    [SerializeField] private Image _image;
    [SerializeField] private CustomButton _customButton;

    public string Header { get => _name.text; set => _name.SetText(value); }

    public MapData MapData { get; private set; }

    public event Action<MapData> OnClick;

    public event Action<MapData> OnHold;

    public void Setup(MapData map, Color color)
    {
      MapData = map;
      if (map != null)
      {
        _name.SetText(map.Name);
        _dateAndTime.SetText(DateTime.FromBinary(map.CreationTime).ToString("dd.MM.yyyy  HH:mm", CultureInfo.InvariantCulture));
      }
      else
      {
        _name.SetText(string.Empty);
        _dateAndTime.SetText(string.Empty);
      }

      _image.color = color;
      _customButton.OnRelease += OnButtonRelease;
      _customButton.OnLongPress += OnButtonLongPress;
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void Clear()
    {
      _customButton.OnRelease -= OnButtonRelease;
      _customButton.OnLongPress -= OnButtonLongPress;
    }

    private void OnButtonRelease()
    {
      if (!_customButton.IsLongPress && !_customButton.WasDragged)
        OnClick?.Invoke(MapData);
    }

    private void OnButtonLongPress() => OnHold?.Invoke(MapData);
  }
}