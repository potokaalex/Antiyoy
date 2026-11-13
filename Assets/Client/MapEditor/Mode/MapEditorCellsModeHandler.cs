using Client.Project;
using Client.Project.Borders;
using Client.Project.Hex;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.Utilities;
using UnityEngine;

namespace Client.MapEditor.Mode
{
  public class MapEditorCellsModeHandler : IMapEditorModeHandler
  {
    private readonly InputController _inputController;
    private readonly GridController _gridController;
    private readonly RegionsService _regionsService;
    private readonly BordersService _bordersService;
    private RegionType _regionType;
    private bool _random;
    private bool _destroy;
    private HexCoordinates _lastPosition;

    public bool OnlyLand { get; set; }

    public MapEditorCellsModeHandler()
    {
      _inputController = Locator.Get<InputController>();
      _gridController = Locator.Get<GridController>();
      _regionsService = Locator.Get<RegionsService>();
      _bordersService = Locator.Get<BordersService>();
    }

    public void Setup(RegionType regionType, bool random = false, bool destroy = false)
    {
      _regionType = regionType;
      _random = random;
      _destroy = destroy;
      _lastPosition = HexCoordinates.FromArray2DIndex(-1, -1);
    }

    public void HandleInput(HexCoordinates position)
    {
      if (_inputController.IsHold && position != _lastPosition)
      {
        if (_destroy)
        {
          _gridController.DestroyCell(position);
          _bordersService.ViewRegionsBorders();
        }
        else
          ReCreateCell(position, _random ? GameUtilities.AllPlayerRegions[Random.Range(0, GameUtilities.AllPlayerRegions.Length)] : _regionType);

        _lastPosition = position;
      }
    }

    private void ReCreateCell(HexCoordinates position, RegionType type)
    {
      if (_gridController.GetCell(position, out var cell))
        _regionsService.SetRegionType(cell, type);
      else if (!OnlyLand)
      {
        cell = _gridController.CreateCell(position);
        _regionsService.AddToBestNeighbourRegion(type, cell);
      }
    }
  }
}