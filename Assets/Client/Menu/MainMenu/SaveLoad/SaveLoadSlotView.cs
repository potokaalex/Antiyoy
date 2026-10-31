using System;
using System.Globalization;
using Client.Project.Map;
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

    public MapController Map { get; private set; }

    public event Action<SaveLoadSlotView> OnClick;

    public event Action<SaveLoadSlotView> OnHold;

    public void Setup(MapController map, Color color)
    {
      Map = map;
      if (map != null)
        SetMap(map);
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
      Map = null;
    }

    public void SetMap(MapController map)
    {
      Map = map;
      _name.SetText(map.IsAutosave ? "Autosave" : map.Name);
      _dateAndTime.SetText(map.CreationTime.ToString("dd.MM.yyyy  HH:mm", CultureInfo.InvariantCulture));
    }

    private void OnButtonRelease()
    {
      if (!_customButton.IsLongPress && !_customButton.WasDragged)
        OnClick?.Invoke(this);
    }

    private void OnButtonLongPress() => OnHold?.Invoke(this);
  }
}