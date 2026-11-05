using Client.MapEditor.UI;
using Client.Project;
using Client.Project.Hex;
using Client.Project.Infrastructure;

namespace Client.MapEditor.Mode
{
  public class MapEditorRegionsModeHandler : IMapEditorModeHandler
  {
    private readonly InputController _inputController;
    private readonly GridController _gridController;
    private readonly MapEditorUI _mapEditorUI;

    public MapEditorRegionsModeHandler()
    {
      _inputController = Locator.Get<InputController>();
      _gridController = Locator.Get<GridController>();
      _mapEditorUI = Locator.Get<MapEditorUI>();
    }

    public void HandleInput(HexCoordinates position)
    {
      if (_inputController.IsClick)
      {
        if (_gridController.GetCell(position, out var cell))
        {
          var region = cell.Region;
          if(region.IsAlive) 
            _mapEditorUI.ShowRegionSettings(region);
        }
      }
    }
  }
}