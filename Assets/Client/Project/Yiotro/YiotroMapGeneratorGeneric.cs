using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Client.Project.Map;
using Client.Project.Utilities;
using UnityEngine;
using Random = System.Random;

namespace Client.Project.Yiotro
{
  public class YiotroMapGeneratorGeneric : YiotroMapGenerator
  {
    private static readonly double[][] _changes =
    {
      null,
      null,
      new[] { -0.4, 0.3 },
      new[] { -0.5, 0.2, 0.4 },
      new[] { -0.6, 0.12, 0.3, 0.6 },
      new[] { -0.5, -0.35, 0.15, 0.32, 0.5 },
      new[] { -0.6, -0.75, 0.15, 0.25, 0.4, 0.6 },
      new[] { -0.4, -0.9, 0.2, 0.3, 0.45, 0.6, 0.8 }
    };

    private bool _flagNeutrals;

    public Hex[,] GenerateMap(Random random, LevelSize levelSize, int fractionsQuantity, Vector2Int size)
    {
      FieldManager.Setup(size);
      SetValues(random, levelSize, fractionsQuantity);
      BeginGeneration();
      CreateLand();
      AddTrees();
      while (!HasGreenProvince())
        GenericBalance();
      EndGeneration();
      return FieldManager.Field;
    }

    protected override void DecreaseProvince(List<Hex> provinceList, double power)
    {
      var num = (int)(power * provinceList.Count);
      for (var i = 0; i < num; i++)
      {
        var hex = FindHexToExcludeFromProvince(provinceList);
        provinceList.Remove(hex);
        hex.Fraction = NEUTRAL_FRACTION;
      }
    }

    protected override void ReduceProvinceSize(List<Hex> provinceList)
    {
      while (provinceList.Count > SMALL_PROVINCE_SIZE)
      {
        var hex = FindHexToExcludeFromProvince(provinceList);
        provinceList.Remove(hex);
        hex.Fraction = NEUTRAL_FRACTION;
      }
    }

    protected override bool ActivateHex(Hex hex, int fraction)
    {
      if (hex.Active)
        return false;
      hex.Active = true;
      hex.SetFraction(NEUTRAL_FRACTION);
      GetActiveHexes().Add(hex);
      return true;
    }

    // not actually needed right now
    private bool HasGreenProvince()
    {
      foreach (var activeHex in GetActiveHexes())
      {
        if (activeHex.Fraction != 0 || activeHex.NumberOfFriendlyHexesNearby() < 2)
          continue;
        return true;
      }

      return false;
    }

    private void GenericBalance()
    {
      MakeAllActiveHexesNeutral();
      SpawnProvinces();
      CutProvincesToSmallSizes();
      MakeSingleHexesIntoProvinces();
      ApplyBalanceMeasures();
    }

    private void MakeAllActiveHexesNeutral()
    {
      foreach (var activeHex in GetActiveHexes())
        activeHex.Fraction = NEUTRAL_FRACTION;
      _flagNeutrals = true;
    }

    private void SpawnProvinces()
    {
      var quantity = GetProvincesQuantity();
      for (var i = 0; i < quantity; i++)
      {
        for (var fraction = 0; fraction < FractionsQuantity; fraction++)
        {
          var hex = FindGoodPlaceForNewProvince();
          if (hex == null)
            continue;
          hex.SetFraction(fraction);
          SpawnProvince(hex, 2);
        }
      }
    }

    private void MakeSingleHexesIntoProvinces()
    {
      foreach (var activeHex in GetActiveHexes())
      {
        if (activeHex.IsNeutral() || activeHex.NumberOfFriendlyHexesNearby() > 0)
          continue;
        var c = 3;
        for (var i = 0; i < 6; i++)
        {
          var adjHex = activeHex.GetAdjacentHex(i);
          if (!adjHex.Active || !adjHex.IsNeutral())
            continue;
          adjHex.Fraction = activeHex.Fraction;
          c--;
          if (c == 0)
            break;
        }
      }
    }

    private void ApplyBalanceMeasures()
    {
      var changesArray = GetChangesArray();
      if (changesArray == null)
        return;
      ApplyChanges(changesArray);
    }

    private double[] GetChangesArray() => FractionsQuantity < _changes.Length && FractionsQuantity >= 2 ? _changes[FractionsQuantity] : null;

    private void ApplyChanges(double[] changes)
    {
      for (var i = 0; i < changes.Length; i++)
      {
        if (changes[i] == 0)
          continue;
        if (changes[i] > 0)
          GiveAdvantageToPlayer(i, changes[i]);
        else
          GiveDisadvantageToPlayer(i, changes[i]);
      }
    }

    private void SpawnProvince(Hex spawnHex, int startingPotential)
    {
      spawnHex.GenPotential = startingPotential;
      var propagationList = TempList;
      propagationList.Clear();
      propagationList.Add(spawnHex);
      while (propagationList.Count > 0)
      {
        var hex = propagationList[0];
        propagationList.RemoveAt(0);
        if (Random.Next(startingPotential) > hex.GenPotential)
          continue;
        hex.Fraction = spawnHex.Fraction;
        if (hex.GenPotential == 0)
          continue;
        for (var i = 0; i < 6; i++)
        {
          var adjHex = hex.GetAdjacentHex(i);
          if (!propagationList.Contains(adjHex) && adjHex.Active && adjHex.Fraction == NEUTRAL_FRACTION)
          {
            adjHex.GenPotential = hex.GenPotential - 1;
            propagationList.Add(adjHex);
          }
        }
      }
    }

    private Hex FindGoodPlaceForNewProvince()
    {
      var activeHexesList = GetActiveHexes();
      var activeHexesArray = activeHexesList.ExtractArray();
      var activeHexesCount = activeHexesList.Count;

      if (AllHexesAreNeutral(activeHexesArray, activeHexesCount))
        return GetRandomFreeHex();

      var step = 0;

      PrepareHexesToFindNewProvincePlace(activeHexesArray, activeHexesCount);

      while (true)
      {
        var expanded = false;
        for (var i = 0; i < activeHexesCount; i++)
        {
          var hex = activeHexesArray[i];
          if (hex.MoveZoneNumber != step)
            continue;

          for (var dir = 0; dir < 6; dir++)
          {
            var adj = hex.GetAdjacentHex(dir);
            if (adj == null) continue;
            if (adj.IsNullHex()) continue;
            if (!adj.Active) continue;
            if (adj.MoveZoneNumber != -1) continue;

            adj.MoveZoneNumber = step + 1;
            expanded = true;
          }
        }

        if (!expanded)
          break;
        step++;
      }

      return FindHexWithHighestMoveZoneNumber(activeHexesArray, activeHexesCount);
    }

    private Hex FindHexWithHighestMoveZoneNumber(Hex[] hexes, int hexesCount)
    {
      Hex result = null;

      for (var i = 0; i < hexesCount; i++)
      {
        var hex = hexes[i];
        if (result == null || hex.MoveZoneNumber > result.MoveZoneNumber)
          result = hex;
      }

      return result;
    }

    private void PrepareHexesToFindNewProvincePlace(Hex[] hexes, int hexesCount)
    {
      for (var i = 0; i < hexesCount; i++)
      {
        var hex = hexes[i];
        if (hex.IsNeutral())
        {
          hex.MoveZoneNumber = -1;
          continue;
        }

        hex.MoveZoneNumber = 0;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private List<Hex> GetActiveHexes() => FieldManager.ActiveHexes;

    private bool AllHexesAreNeutral(Hex[] hexes, int hexesCount)
    {
      if (!_flagNeutrals)
        return false;

      for (var i = 0; i < hexesCount; i++)
      {
        var hex = hexes[i];
        if (hex.IsNeutral())
          continue;
        _flagNeutrals = false;
        return false;
      }

      return true;
    }

    private Hex GetRandomFreeHex() => GetActiveHexes()[Random.Next(GetActiveHexes().Count)];

    private int GetProvincesQuantity()
    {
      switch (LevelSize)
      {
        default:
        case LevelSize.Small:
          return 1;
        case LevelSize.Medium:
          return 2;
        case LevelSize.Big:
          return 3;
        case LevelSize.Huge:
          return 4;
      }
    }
  }
}