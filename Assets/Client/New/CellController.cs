using System.Collections.Generic;
using Client.New.Hex;
using TMPro;
using UnityEngine;

namespace Client.New
{
  public class CellController : MonoBehaviour
  {
    public HexCoordinates Position { get; private set; }
    public List<CellController> NeighbourCells { get; } = new();

    [SerializeField] private TextMeshPro _debugText;
    private GridController _gridController;

    public void Initialize(GridController gridController, HexCoordinates position)
    {
      _gridController = gridController;
      Position = position;
      ClearColor();
      _debugText.SetText(position.ToString());
    }

    public void SetColor(Color color)
    {
      _gridController.SetColor(Position, color);
    }

    public void ClearColor()
    {
      _gridController.SetColor(Position, Color.gray);
    }
  }
}