using TMPro;
using UnityEngine;

namespace Client.New
{
  public class CellController : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _debugText;

    public void Initialize(GridController grid, HexCoordinates position)
    {
      grid.SetColor(position, Color.gray);
      _debugText.SetText(position.ToString());
    }
  }
}