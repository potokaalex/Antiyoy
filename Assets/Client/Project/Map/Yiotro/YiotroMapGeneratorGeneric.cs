using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Client.Project.Map.Yiotro
{
  public class YiotroMapGeneratorGeneric : YiotroMapGenerator
  {
    private bool _flagNeutrals;

    public Hex[][] GenerateMap(Random random, LevelSize levelSize, int fractionsQuantity)
    {
      FieldManager.Setup(GetFieldSize(levelSize));
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

    private Vector2Int GetFieldSize(LevelSize levelSize)
    {
      switch (levelSize)
      {
        case LevelSize.Small:
          return new Vector2Int(14, 24);
        case LevelSize.Medium:
          return new Vector2Int(28, 24);
        case LevelSize.Big:
          return new Vector2Int(28, 48);
        case LevelSize.Huge:
          return new Vector2Int(42, 72);
        default:
          throw new ArgumentOutOfRangeException(nameof(levelSize), levelSize, null);
      }
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

    private double[] GetChangesArray()
    {
      switch (FractionsQuantity)
      {
        default:
          return null;
        case 2:
          return new[] { -0.4, 0.3 };
        case 3:
          return new[] { -0.5, 0.2, 0.4 };
        case 4:
          return new[] { -0.6, 0.12, 0.3, 0.6 };
        case 5:
          return new[] { -0.5, -0.35, 0.15, 0.32, 0.5 };
        case 6:
          return new[] { -0.6, -0.75, 0.15, 0.25, 0.4, 0.6 };
        case 7:
          return new[] { -0.4, -0.9, 0.2, 0.3, 0.45, 0.6, 0.8 };
      }
    }

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
      var propagationList = new List<Hex>();
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
      if (AllHexesAreNeutral())
        return GetRandomFreeHex();

      PrepareHexesToFindNewProvincePlace();

      var step = 0;
      while (true)
      {
        var expanded = false;
        foreach (Hex hex in GetActiveHexes())
        {
          if (hex.MoveZoneNumber != step) continue;
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

      return FindHexWithHighestMoveZoneNumber();
    }

    private Hex FindHexWithHighestMoveZoneNumber()
    {
      Hex result = null;

      foreach (Hex hex in GetActiveHexes())
        if (result == null || hex.MoveZoneNumber > result.MoveZoneNumber)
          result = hex;
      return result;
    }

    private void PrepareHexesToFindNewProvincePlace()
    {
      foreach (Hex hex in GetActiveHexes())
      {
        if (hex.IsNeutral())
        {
          hex.MoveZoneNumber = -1;
          continue;
        }

        hex.MoveZoneNumber = 0;
      }
    }

    private List<Hex> GetActiveHexes() => FieldManager.ActiveHexes;

    private bool AllHexesAreNeutral()
    {
      if (!_flagNeutrals) return false;

      foreach (Hex activeHex in GetActiveHexes())
      {
        if (activeHex.IsNeutral())
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