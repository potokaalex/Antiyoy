using System.Text;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.Unit.Code;
using Client.Project.Yiotro;

namespace Client.Project.Map
{
  public class MapEncodeController
  {
    private readonly StringBuilder _builder = new();
    private readonly MapsService _mapsService;
    private readonly GovernmentsService _governmentsService;
    private readonly GridController _gridController;
    private readonly UnitsService _unitsService;
    private readonly RegionsService _regionsService;
    private bool _isHuge;

    public MapEncodeController()
    {
      _mapsService = Locator.Get<MapsService>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _gridController = Locator.Get<GridController>();
      _unitsService = Locator.Get<UnitsService>();
      _regionsService = Locator.Get<RegionsService>();
    }

    public string EncodeFromRuntime()
    {
      _isHuge = _gridController.LevelSize == LevelSize.Huge;

      _builder.Clear();
      _builder.Append("antiyoy_level_code");
      EncodeLevelSize();
      EncodeGeneralInfo();
      EncodeMapName();
      EncodeEditorInfo();
      EncodeLand();
      EncodeUnits();
      EncodeProvinces();

      _builder.Append("#");
      return _builder.ToString();
    }

    private void EncodeProvinces()
    {
      StartSection("provinces");

      for (var i = 0; i < _regionsService.Regions.Count; i++)
      {
        var region = _regionsService.Regions[i];
        var index = YiotroUtilities.HexToIndex(region.Cells[0].Position, _isHuge);
        _builder.Append(index.x).Append("@").Append(index.y).Append("@").Append(i).Append("@").Append(i).Append("@").Append(region.Money).Append(",");
      }
    }

    private void EncodeMapName()
    {
      StartSection("map_name");
      _builder.Append(_mapsService.StartMap.Name);
    }

    private void EncodeEditorInfo()
    {
      StartSection("editor_info");
      _builder.Append(YiotroUtilities.PlayerRegionTypeToCode(_governmentsService.FirstPlayer)).Append(" ");
      _builder.Append(false).Append(" ");
      _builder.Append(false).Append(" ");
    }

    private void EncodeUnits()
    {
      StartSection("units");
      foreach (var unit in _unitsService.Units)
      {
        if (unit.Type.IsWarrior())
        {
          var index = YiotroUtilities.HexToIndex(unit.Cell.Position, _isHuge);
          var strength = YiotroUtilities.UnitTypeToStrength(unit.Type);
          _builder.Append(index.x).Append(" ").Append(index.y).Append(" ").Append(strength).Append(" ").Append(unit.HasTurns).Append(",");
        }
      }
    }

    private void EncodeLand()
    {
      StartSection("land");

      foreach (var cell in _gridController.GetAllCells())
      {
        var objectInside = 0;
        if (cell.HasUnit)
          objectInside = YiotroUtilities.UnitTypeToObj(cell.Unit.Type);

        var fraction = YiotroUtilities.RegionTypeToFraction(cell.Region.Type);
        var index = YiotroUtilities.HexToIndex(cell.Position, _isHuge);
        _builder.Append(index.x).Append(" ").Append(index.y).Append(" ").Append(fraction).Append(" ").Append(objectInside).Append(",");
      }
    }

    private void EncodeGeneralInfo()
    {
      StartSection("general");
      _builder.Append(_governmentsService.DifficultyMode).Append(" ");
      _builder.Append(_governmentsService.PlayersMode).Append(" ");
      _builder.Append(_governmentsService.PlayersCount());
    }

    private void EncodeLevelSize()
    {
      StartSection("level_size");
      _builder.Append(YiotroUtilities.LevelSizeToCode(_gridController.LevelSize));
    }

    private void StartSection(string name) => _builder.Append("#").Append(name).Append(":");
  }
}