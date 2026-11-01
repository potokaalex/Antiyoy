using Client.Project;
using Client.Project.Borders;
using Client.Project.CameraFeatures;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;

namespace Client.MapEditor
{
  public class MapEditorController : IInitializable, ITickable
  {
    private MapsService _mapsService;
    private GridController _gridController;
    private UnitsService _unitsService;
    private RegionsService _regionsService;
    private CameraController _cameraController;
    private BordersService _bordersService;
    private MapEditorUI _mapEditorUI;
    private bool _started;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      _cameraController = Locator.Get<CameraController>();
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
      _bordersService = Locator.Get<BordersService>();
      _mapsService = Locator.Get<MapsService>();
      _mapEditorUI = Locator.Get<MapEditorUI>();
    }
    
    public void Start()
    {
      var map = _mapsService.StartMap;
      _gridController.InitialCreateCells(map);
      _unitsService.InitialCreateUnits(map);
      _regionsService.InitialCreateRegions(map);
      _cameraController.Setup();

      _bordersService.ViewRegionsBorders();
      _mapEditorUI.PlayShow();
      _started = true;
    }
    
    public void Tick()
    {
      if (!_started)
        return;

      //_currentPlayer.Tick();
    }

    public void End()
    {
      _started = false;
      _unitsService.Clear();
      _regionsService.Clear();
    }
  }
}