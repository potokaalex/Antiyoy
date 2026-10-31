using System;
using System.Collections.Generic;

namespace Client.Project.Map.Yiotro
{
  public class YiotroMapGenerator
  {
    public const int NEUTRAL_FRACTION = 100;
    public const int OBJ_PINE = 1;
    public const int OBJ_PALM = 2;

    protected const int SMALL_PROVINCE_SIZE = 5;
    protected readonly FieldManager FieldManager = new();
    protected Random Random;
    protected LevelSize LevelSize;
    protected int FractionsQuantity;

    private const double _treesSpawnChance = 0.1;
    private readonly DetectorProvince _detectorProvince = new();
    private readonly List<Hex> _tempList = new();
    private int _fWidth;
    private int _fHeight;
    private double _minX;
    private double _maxX;
    private double _minY;
    private double _maxY;
    private Hex[][] _field;
    private List<Hex> _islandCenters;
    private List<Link> _links;

    protected void SetValues(Random random, LevelSize levelSize, int fractionsQuantity)
    {
      _fWidth = FieldManager.FWidth;
      _fHeight = FieldManager.FHeight;
      LevelSize = levelSize;
      FractionsQuantity = fractionsQuantity;

      double scaleX = LevelSize switch
      {
        LevelSize.Medium => 2.0,
        LevelSize.Big => 2.0,
        LevelSize.Huge => 3.0,
        _ => 1.0
      };
      double scaleY = LevelSize switch
      {
        LevelSize.Big => 2.0,
        LevelSize.Huge => 3.0,
        _ => 1.0
      };
      _minX = 0.1 / scaleX * _fWidth;
      _maxX = _fWidth - _minX;
      _minY = 0.15 / scaleY * _fHeight;
      _maxY = _fHeight - 0.1 / scaleY * _fHeight;
      Random = random;
      _field = FieldManager.Field;
    }

    protected virtual void DecreaseProvince(List<Hex> provinceList, double power)
    {
      foreach (var hex in provinceList)
        if (HexHasEnemiesNear(hex) && Random.NextDouble() < power)
          hex.Fraction = GetRandomFraction();
    }

    protected virtual bool ActivateHex(Hex hex, int fraction)
    {
      if (hex.Active)
        return false;
      hex.Active = true;
      hex.SetFraction(fraction);
      FieldManager.ActiveHexes.Add(hex);
      return true;
    }

    protected virtual void ReduceProvinceSize(List<Hex> provinceList)
    {
      var provinceFraction = provinceList[0].Fraction;
      while (provinceList.Count > SMALL_PROVINCE_SIZE)
      {
        var hex = FindHexToExcludeFromProvince(provinceList);
        provinceList.Remove(hex);
        hex.Fraction = getRandomFractionExceptOne(provinceFraction);
      }
    }

    protected void GiveDisadvantageToPlayer(int index, double power)
    {
      ClearGenFlags();
      foreach (var activeHex in FieldManager.ActiveHexes)
      {
        if (!activeHex.GenFlag && activeHex.SameFraction(index))
        {
          var provinceList = _detectorProvince.DetectProvince(activeHex);
          TagProvince(provinceList);
          DecreaseProvince(provinceList, power);
        }
      }
    }

    protected void GiveAdvantageToPlayer(int index, double power)
    {
      ClearGenFlags();
      foreach (var activeHex in FieldManager.ActiveHexes)
      {
        if (!activeHex.GenFlag && activeHex.SameFraction(index))
        {
          var provinceList = _detectorProvince.DetectProvince(activeHex);
          IncreaseProvince(provinceList, power);
          provinceList = _detectorProvince.DetectProvince(activeHex); // detect again because province increased
          TagProvince(provinceList);
        }
      }
    }


    protected void CreateLand()
    {
      var attempts = 0;
      while (!IsGood())
      {
        // Each retry must start with fresh island metadata.
        _islandCenters.Clear();
        _links.Clear();
        DeactivateHexes();
        var n = NumberOfIslandsByLevelSize();
        for (var i = 0; i < n; i++)
        {
          var hex = GetRandomHexInsideBounds();
          _islandCenters.Add(hex);
          SpawnIsland(hex, 7);
        }

        UniteIslandsWithRoads();
        CenterLand();
        CutOffHexesOutsideOfBounds();
        attempts++;
        if (attempts >= 500)
        {
          throw new InvalidOperationException("Map generation failed after 500 attempts. Check field dimensions and level size.");
        }
      }
    }

    protected void EndGeneration() => FieldManager.NullHex.Active = false;

    protected void BeginGeneration()
    {
      FieldManager.CreateFieldMatrix();
      _islandCenters = new List<Hex>();
      _links = new List<Link>();
    }

    protected void CutProvincesToSmallSizes()
    {
      var loopLimit = 100;
      while (atLeastOneProvinceIsTooBig() && loopLimit > 0)
      {
        foreach (var activeHex in FieldManager.ActiveHexes)
        {
          var provinceList = _detectorProvince.DetectProvince(activeHex);
          if (provinceList.Count > SMALL_PROVINCE_SIZE)
            ReduceProvinceSize(provinceList);
        }

        loopLimit--;
      }
    }

    protected Hex FindHexToExcludeFromProvince(List<Hex> provinceList)
    {
      Hex resultHex = null;
      var minNumber = 0;
      for (int i = 0; i < provinceList.Count; i++)
      {
        var currHex = provinceList[i];
        var currNumber = currHex.NumberOfFriendlyHexesNearby();
        if (resultHex == null || currNumber < minNumber)
        {
          minNumber = currNumber;
          resultHex = currHex;
        }
      }

      return resultHex;
    }

    protected void AddTrees()
    {
      foreach (var activeHex in FieldManager.ActiveHexes)
      {
        if (Random.NextDouble() >= _treesSpawnChance || activeHex.ContainsObject())
          continue;
        FieldManager.SpawnTree(activeHex);
      }
    }

    private bool atLeastOneProvinceIsTooBig()
    {
      foreach (var activeHex in FieldManager.ActiveHexes)
        if (_detectorProvince.DetectProvince(activeHex).Count > SMALL_PROVINCE_SIZE)
          return true;
      return false;
    }

    private int getRandomFractionExceptOne(int excludedFraction)
    {
      while (true)
      {
        int fraction = GetRandomFraction();
        if (fraction != excludedFraction) return fraction;
      }
    }

    private void TagProvince(List<Hex> provinceList)
    {
      foreach (Hex hex in provinceList)
        hex.GenFlag = true;
    }

    private void CenterLand()
    {
      var centerHex = GetCenterHex();
      var utterLeft = centerHex.Index1;
      var utterRight = centerHex.Index1;
      var utterUp = centerHex.Index2;
      var utterDown = centerHex.Index2;
      foreach (var activeHex in FieldManager.ActiveHexes)
      {
        if (activeHex.Index1 < utterLeft)
          utterLeft = activeHex.Index1;
        if (activeHex.Index1 > utterRight)
          utterRight = activeHex.Index1;
        if (activeHex.Index2 < utterDown)
          utterDown = activeHex.Index2;
        if (activeHex.Index2 > utterUp)
          utterUp = activeHex.Index2;
      }

      var averageHorizontal = (utterLeft + utterRight) / 2;
      var averageVertical = (utterDown + utterUp) / 2;
      RelocateMap(centerHex.Index1 - averageHorizontal, centerHex.Index2 - averageVertical);
    }

    private void RelocateMap(int deltaX, int deltaY)
    {
      var wasActive = new bool[_fWidth][];
      var fractions = new int[_fWidth][];
      for (var x = 0; x < _fWidth; x++)
      {
        wasActive[x] = new bool[_fHeight];
        fractions[x] = new int[_fHeight];
        for (var y = 0; y < _fHeight; y++)
        {
          wasActive[x][y] = _field[x][y].Active;
          fractions[x][y] = _field[x][y].Fraction;
        }
      }

      FieldManager.ClearActiveHexesList();
      for (var x = 0; x < _fWidth; x++)
      {
        for (var y = 0; y < _fHeight; y++)
        {
          _field[x][y].Active = false;
          _field[x][y].GenFlag = false;
        }
      }

      for (var x = 0; x < _fWidth; x++)
      {
        for (var y = 0; y < _fHeight; y++)
        {
          if (!wasActive[x][y])
            continue;
          var destX = x + deltaX;
          var destY = y + deltaY;
          if (destX < 0 || destY < 0 || destX >= _fWidth || destY >= _fHeight)
            continue;
          var destination = _field[destX][destY];
          destination.Active = true;
          destination.SetFraction(fractions[x][y]);
          if (!FieldManager.ActiveHexes.Contains(destination))
            FieldManager.ActiveHexes.Add(destination);
        }
      }
    }

    private bool IsGood() => IsLinked() && FieldManager.ActiveHexes.Count > 0.25 * NumberOfAvailableHexes();

    private int NumberOfAvailableHexes()
    {
      var c = 0;
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
        if (IsHexInsideBounds(_field[i][j]))
          c++;
      return c;
    }

    private bool IsLinked()
    {
      ClearGenFlags();

      var activeHex = FindActiveHex();
      if (activeHex == null)
        return false;

      // flood
      _tempList.Clear();
      _tempList.Add(activeHex);
      while (_tempList.Count > 0)
      {
        var hex = _tempList[0];
        _tempList.RemoveAt(0);
        hex.GenFlag = true;
        for (int i = 0; i < 6; i++)
        {
          var adjHex = hex.GetAdjacentHex(i);
          if (adjHex.Active && !adjHex.GenFlag && !_tempList.Contains(adjHex))
            _tempList.Add(adjHex);
        }
      }

      // check if something is not tagged
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
        if (_field[i][j].Active && !_field[i][j].GenFlag)
          return false;

      return true;
    }

    private Hex FindActiveHex()
    {
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
        if (_field[i][j].Active)
          return _field[i][j];
      return null;
    }

    private void MaybeDeactivateIfPossible(Hex hex)
    {
      if (!hex.Active || Random.NextDouble() > 0.8)
        return;
      var activeNearby = hex.NumberOfActiveHexesNearby();
      if (activeNearby == 4)
        DeactivateHex(hex);
    }

    private void CutOffHexesOutsideOfBounds()
    {
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
      {
        if (_field[i][j].Active && !IsHexInsideBounds(_field[i][j]))
        {
          DeactivateHex(_field[i][j]);
          for (var k = 0; k < 6; k++)
            MaybeDeactivateIfPossible(_field[i][j].GetAdjacentHex(k));
        }
      }
    }

    private void DeactivateHexes()
    {
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
      {
        if (!_field[i][j].Active)
          continue;
        DeactivateHex(_field[i][j]);
      }
    }

    private void ClearGenFlags()
    {
      for (var i = 0; i < _fWidth; i++)
      for (var j = 0; j < _fHeight; j++)
        _field[i][j].GenFlag = false;
    }

    private Hex GetCenterHex() => _field[Math.Min(_fWidth - 1, _fWidth / 2)][Math.Min(_fHeight - 1, _fHeight / 2)];

    private Hex GetHexAtArrayCoordinates(int x, int y)
    {
      if (x < 0 || y < 0 || x >= _fWidth || y >= _fHeight)
        return null;
      return _field[x][y];
    }

    private double GetRandomAngle() => Random.NextDouble() * 2d * Math.PI;

    private Hex GetRandomHexInsideBounds()
    {
      while (true)
      {
        Hex hex;
        if (LevelSize <= LevelSize.Medium)
          hex = _field[Random.Next(_fWidth)][Random.Next(_fHeight)];
        else
        {
          // Preserve the old centre-biased radial sampling, expressed in array space.
          var radiusFactor = Random.NextDouble() * Random.NextDouble() * 0.5;
          var angle = GetRandomAngle();
          var x = (int)Math.Round((_fWidth - 1) / 2.0 + radiusFactor * _fWidth * Math.Cos(angle));
          var y = (int)Math.Round((_fHeight - 1) / 2.0 + radiusFactor * _fHeight * Math.Sin(angle));
          hex = GetHexAtArrayCoordinates(x, y);
        }

        if (!IsHexInsideBounds(hex))
          continue;
        return hex;
      }
    }

    private int GetRandomFraction()
    {
      if (FractionsQuantity == 0)
        return -1;
      while (true)
      {
        var fraction = Random.Next(FractionsQuantity);
        if (fraction == NEUTRAL_FRACTION)
          continue;
        return fraction;
      }
    }

    private void DeactivateHex(Hex hex)
    {
      hex.Active = false;
      FieldManager.ActiveHexes.Remove(hex);
    }

    private void SpawnIsland(Hex startHex, int size)
    {
      ClearGenFlags();
      startHex.GenPotential = size;
      var propagationList = new List<Hex>();
      propagationList.Add(startHex);
      while (propagationList.Count > 0)
      {
        var hex = propagationList[0];
        propagationList.RemoveAt(0);
        hex.GenFlag = true;
        if (Random.Next(size) > hex.GenPotential)
          continue;
        var activated = ActivateHex(hex, GetRandomFraction());
        if (hex.GenPotential == 0 || !activated)
          continue;
        for (var i = 0; i < 6; i++)
        {
          var adjHex = hex.GetAdjacentHex(i);
          if (!adjHex.GenFlag && !adjHex.IsNullHex() && !propagationList.Contains(adjHex))
          {
            adjHex.GenPotential = hex.GenPotential - 1;
            propagationList.Add(adjHex);
          }
        }
      }
    }

    private void UniteIslandsWithRoads()
    {
      for (var i = 0; i < _islandCenters.Count; i++)
        CreateRoadBetweenIslands(i, GetClosestIslandIndex(i));
    }

    private void CreateRoadBetweenIslands(int islandOne, int islandTwo)
    {
      if (islandTwo == -1) return;
      var start = _islandCenters[islandOne];
      var end = _islandCenters[islandTwo];
      _links.Add(new Link(start, end));

      // The original world-space line was sampled every half-hex. Draw the
      // equivalent line in axial array coordinates at roughly two samples per hex.
      var distance = HexDistance(start.Index1, start.Index2, end.Index1, end.Index2);
      var samples = Math.Max(1, distance * 2);
      for (var i = 0; i <= samples; i++)
      {
        var t = i / (double)samples;
        var hex = GetHexOnAxialLine(start, end, t);
        if (hex != null)
          SpawnIsland(hex, 2);
      }
    }

    private int HexDistance(int x1, int y1, int x2, int y2)
    {
      var dx = x2 - x1;
      var dy = y2 - y1;
      return (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dx + dy)) / 2;
    }

    private Hex GetHexOnAxialLine(Hex start, Hex end, double t)
    {
      var x = start.Index1 + (end.Index1 - start.Index1) * t;
      var y = start.Index2 + (end.Index2 - start.Index2) * t;
      // Axial/cube rounding keeps the path adjacent on a hex grid.
      var z = -x - y;
      var rx = (int)Math.Round(x);
      var ry = (int)Math.Round(y);
      var rz = (int)Math.Round(z);
      var dx = Math.Abs(rx - x);
      var dy = Math.Abs(ry - y);
      var dz = Math.Abs(rz - z);
      if (dx > dy && dx > dz)
        rx = -ry - rz;
      else if (dy > dz)
        ry = -rx - rz;
      return GetHexAtArrayCoordinates(rx, ry);
    }

    private bool AreIslandsAlreadyUnited(Hex p1, Hex p2)
    {
      for (var i = 0; i < _links.Count; i++)
        if (_links[i].Equals(p1, p2))
          return true;
      return false;
    }

    private int GetClosestIslandIndex(int searcherIslandIndex)
    {
      var start = _islandCenters[searcherIslandIndex];
      var closestIslandIndex = -1;
      var minDistance = double.MaxValue;
      // Keep the original generator's candidate range (starts at island 1).
      for (var i = 1; i < _islandCenters.Count; i++)
      {
        if (i == searcherIslandIndex || AreIslandsAlreadyUnited(start, _islandCenters[i]))
          continue;
        var candidate = _islandCenters[i];
        // Euclidean distance in the physical plane of an axial hex grid.
        var dx = (candidate.Index1 - start.Index1) + (candidate.Index2 - start.Index2) * 0.5;
        var dy = (candidate.Index2 - start.Index2) * (Math.Sqrt(3) / 2.0);
        var currentDistance = dx * dx + dy * dy;
        if (currentDistance < minDistance)
        {
          minDistance = currentDistance;
          closestIslandIndex = i;
        }
      }

      return closestIslandIndex;
    }

    private bool IsHexInsideBounds(Hex hex)
    {
      if (hex == null || hex.IsNullHex())
        return false;
      return hex.Index1 > _minX && hex.Index1 < _maxX &&
             hex.Index2 > _minY && hex.Index2 < _maxY;
    }

    private int NumberOfIslandsByLevelSize()
    {
      switch (LevelSize)
      {
        default:
        case LevelSize.Small:
          return 2;
        case LevelSize.Medium:
          return 4;
        case LevelSize.Big:
          return 20;
        case LevelSize.Huge:
          return 35;
      }
    }

    private void IncreaseProvince(List<Hex> provinceList, double power)
    {
      foreach (Hex hex in provinceList)
      {
        for (var i = 0; i < 6; i++)
        {
          var adjHex = hex.GetAdjacentHex(i);
          if (adjHex.Active && !adjHex.SameFraction(hex) && Random.NextDouble() < power)
            adjHex.Fraction = hex.Fraction;
        }
      }
    }

    private bool HexHasEnemiesNear(Hex hex)
    {
      for (var i = 0; i < 6; i++)
      {
        var adjHex = hex.GetAdjacentHex(i);
        if (adjHex.Active && !adjHex.SameFraction(hex))
          return true;
      }

      return false;
    }
  }
}