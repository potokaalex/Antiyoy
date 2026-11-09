using System;
using Client.Project.Hex;
using Client.Project.Map;
using Client.Project.Region;
using Client.Project.Unit.Code;
using UnityEngine;

namespace Client.Project.Yiotro
{
  public class YiotroUtilities
  {
    public const int OBJ_PINE = 1;
    public const int OBJ_PALM = 2;
    public const int OBJ_TOWN = 3;
    public const int OBJ_TOWER = 4;
    public const int OBJ_GRAVE = 5;
    public const int OBJ_FARM = 6;
    public const int OBJ_STRONG_TOWER = 7;

    public static int UnitTypeToObj(UnitType unitType)
    {
      return unitType switch
      {
        UnitType.Farm => OBJ_FARM,
        UnitType.Capital => OBJ_TOWN,
        UnitType.Tower => OBJ_TOWER,
        UnitType.StrongTower => OBJ_STRONG_TOWER,
        UnitType.Pine => OBJ_PINE,
        UnitType.Palm => OBJ_PALM,
        UnitType.Grave => OBJ_GRAVE,
        _ => 0
      };
    }

    public static UnitType ObjToUnitType(int obj)
    {
      return obj switch
      {
        6 => UnitType.Farm,
        3 => UnitType.Capital,
        4 => UnitType.Tower,
        7 => UnitType.StrongTower,
        1 => UnitType.Pine,
        2 => UnitType.Palm,
        5 => UnitType.Grave,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static int UnitTypeToStrength(UnitType unitType)
    {
      return unitType switch
      {
        UnitType.Peasant => 1,
        UnitType.Spearman => 2,
        UnitType.Infantryman => 3,
        UnitType.Knight => 4,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static UnitType StrengthToUnitType(int strength)
    {
      return strength switch
      {
        1 => UnitType.Peasant,
        2 => UnitType.Spearman,
        3 => UnitType.Infantryman,
        4 => UnitType.Knight,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static int RegionTypeToFraction(RegionType regionType)
    {
      return regionType switch
      {
        RegionType.Neutral => 7,
        RegionType.Red => 5,
        RegionType.Blue => 2,
        RegionType.Green => 8,
        RegionType.Yellow => 4,
        RegionType.Purple => 9,
        RegionType.Brown => 10,
        RegionType.Cyan => 3,
        RegionType.Lime => 0,
        RegionType.Rose => 1,
        RegionType.Mint => 6,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static RegionType FractionToRegionType(int fraction)
    {
      return fraction switch
      {
        7 => RegionType.Neutral,
        5 => RegionType.Red,
        2 => RegionType.Blue,
        8 => RegionType.Green,
        4 => RegionType.Yellow,
        9 => RegionType.Purple,
        10 => RegionType.Brown,
        3 => RegionType.Cyan,
        0 => RegionType.Lime,
        1 => RegionType.Rose,
        6 => RegionType.Mint,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static Vector2Int HexToIndex(HexCoordinates hex, bool isHugeLevelSize)
    {
      return new Vector2Int
      {
        x = hex.R + (isHugeLevelSize ? 20 : 26),
        y = hex.Q
      };
    }

    public static HexCoordinates IndexToHex(int x, int y, bool isHugeLevelSize)
    {
      return new HexCoordinates
      {
        R = x - (isHugeLevelSize ? 20 : 26),
        Q = y
      };
    }

    public static int LevelSizeToCode(LevelSize levelSize)
    {
      return levelSize switch
      {
        LevelSize.Small => 1,
        LevelSize.Medium => 2,
        LevelSize.Big => 4,
        LevelSize.Huge => 9,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static LevelSize CodeToLevelSize(int code)
    {
      return code switch
      {
        1 => LevelSize.Small,
        2 => LevelSize.Medium,
        4 => LevelSize.Big,
        9 => LevelSize.Huge,
        _ => throw new ArgumentOutOfRangeException()
      };
    }
  }
}