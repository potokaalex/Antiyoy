using System.Collections.Generic;
using Client.New.Hex;
using Client.New.Infrastructure;
using Client.New.Region;
using Client.New.Tile;
using TMPro;
using UnityEngine;

namespace Client.New.Cell
{
  public class CellController : MonoBehaviour
  {
    [SerializeField] private TextMeshPro _debugText;
    private TilemapController _tilemapController;
    private RegionController _region;
    
    public HexCoordinates Position { get; private set; }

    public RegionController Region
    {
      get => _region;
      set
      {
        _region = value;
        if (_region == null)
        {
          ClearColor();
        }
        else if (_region.Type == RegionType.Default)
        {
          _tilemapController.SetColor(Position, Color.gray);
        }
      }
    }

    private void Awake()
    {
      _tilemapController = Locator.Get<TilemapController>();
    }

    public void Initialize(HexCoordinates position, RegionType type)
    {
      Position = position;
      Region = new RegionController(new List<CellController> { this }, type); //todo: remove it. add to _regions!
      _debugText.SetText(position.ToString());
    }

    private void ClearColor()
    {
      _tilemapController.SetColor(Position, Color.black);
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