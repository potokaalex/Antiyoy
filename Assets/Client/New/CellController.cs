using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Region;
using TMPro;
using UnityEngine;

namespace Client.New
{
  public class CellController : MonoBehaviour
  {
    public HexCoordinates Position { get; private set; }
    public List<CellController> NeighbourCells { get; } = new();
    public RegionController Region { get; set; }

    [SerializeField] private TextMeshPro _debugText;
    private GridController _gridController;

    public void Initialize(GridController gridController, HexCoordinates position, RegionType type)
    {
      _gridController = gridController;
      Position = position;
      Region = new RegionController(new List<CellController> { this }, type); //todo: remove it. add to _regions!
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

    private void Update()
    {
      if (Region != null)
      {
        _debugText.SetText(Region.Cells.Count.ToString());
      }
    }
  }
}