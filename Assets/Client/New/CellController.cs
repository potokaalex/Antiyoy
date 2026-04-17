using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Region;
using TMPro;
using UnityEngine;
using Zenject;

namespace Client.New
{
  public class CellController : MonoBehaviour
  {
    public HexCoordinates Position { get; private set; }
    public RegionController Region { get; set; }

    [SerializeField] private TextMeshPro _debugText;
    private TilemapController _tilemapController;

    [Inject]
    public void Construct(TilemapController tilemapController)
    {
      _tilemapController = tilemapController;
    }
    
    public void Initialize(HexCoordinates position, RegionType type)
    {
      Position = position;
      Region = new RegionController(new List<CellController> { this }, type); //todo: remove it. add to _regions!
      ClearColor();
      _debugText.SetText(position.ToString());
    }

    public void SetColor(Color color)
    {
      _tilemapController.SetColor(Position, color);
    }

    public void ClearColor()
    {
      _tilemapController.SetColor(Position, Color.gray);
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