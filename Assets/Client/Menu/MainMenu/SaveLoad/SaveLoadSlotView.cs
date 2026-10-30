using System;
using System.Globalization;
using Client.Project.Map.Data;
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

    public void Initialize(MapData map, Color color)
    {
      _name.SetText(map.Name);
      _dateAndTime.SetText(DateTime.FromBinary(map.CreationTime).ToString("dd.MM.yyyy  HH:mm", CultureInfo.InvariantCulture));
      _image.color = color;
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
  }
}