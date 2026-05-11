using Client.Hex;
using Client.Infrastructure;
using TMPro;
using UnityEngine;

namespace Client.DebugFeatures
{
  public class CellDebugController : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _debugText;
    private GridController _gridController;
    private HexCoordinates _position;
    private string _currentText;

    public void Initialize(HexCoordinates position)
    {
      _position = position;
      _gridController = Locator.Get<GridController>();
    }

    private void Update()
    {
      if (_gridController.TryGetCell(_position, out var cell))
      {
        if (cell.Region != null)
        {
          SetText(cell.Region.Cells.Count.ToString());
          //SetText(cell.Protection.ToString());
          return;
        }
      }

      SetText(string.Empty);
    }

    private void SetText(string value)
    {
      if (_currentText != value)
      {
        _debugText.SetText(value);
        _currentText = value;
      }
    }
  }
}