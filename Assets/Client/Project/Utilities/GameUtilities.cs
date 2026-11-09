using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Client.Project.Map;
using Client.Project.Region;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Project.Utilities
{
  public static class GameUtilities
  {
    public static readonly List<CellController> AreaBuffer = new();

    public static readonly RegionType[] AllPlayerRegions =
    {
      RegionType.Red, RegionType.Blue, RegionType.Green, RegionType.Yellow, RegionType.Purple, RegionType.Brown, RegionType.Cyan, RegionType.Lime,
      RegionType.Rose, RegionType.Mint
    };

    public static void SetAlpha(this SpriteRenderer spriteRenderer, float value)
    {
      var color = spriteRenderer.color;
      color.a = value;
      spriteRenderer.color = color;
    }

    public static void SetAlpha(this Image image, float value)
    {
      var color = image.color;
      color.a = value;
      image.color = color;
    }

    public static Vector2Int LevelSizeToFieldSize(LevelSize levelSize)
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
          return new Vector2Int(42, 65);
        default:
          throw new ArgumentOutOfRangeException(nameof(levelSize), levelSize, null);
      }
    }

    public static LevelSize FieldSizeToLevelSize(Vector2Int fieldSize)
    {
      if (fieldSize == new Vector2Int(14, 24))
        return LevelSize.Small;
      if (fieldSize == new Vector2Int(28, 24))
        return LevelSize.Medium;
      if (fieldSize == new Vector2Int(28, 48))
        return LevelSize.Big;
      if (fieldSize == new Vector2Int(42, 65))
        return LevelSize.Huge;

      throw new Exception($"Cant find levelSize for fieldSize: {fieldSize}");
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetRef<T>(this List<T> list, int index) => ref ExtractArrayFromList(list)[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T[] ExtractArrayFromList<T>(List<T> list) => UnsafeUtility.As<List<T>, ListPrivateFieldAccess<T>>(ref list)._items;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private class ListPrivateFieldAccess<T>
    {
#pragma warning disable CS0649
#pragma warning disable CS8618
      public T[] _items; // Do not rename (binary serialization)
#pragma warning restore CS8618
      public int _size; // Do not rename (binary serialization)
      public int _version; // Do not rename (binary serialization)
#pragma warning restore CS0649
    }
  }
}