using System;
using System.Collections.Generic;

namespace Client.Utilities
{
  public static class CollectionsExtensions
  {
    public static void SortByDecreasing<T>(this List<T> list, Func<T, int> getValue)
    {
      for (var i = 0; i < list.Count - 1; i++)
      for (var j = 0; j < list.Count - i - 1; j++)
        if (getValue(list[j]) < getValue(list[j + 1]))
          (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }

    public static void SortByIncreasing<T>(this List<T> list, Func<T, int> getValue)
    {
      for (var i = 0; i < list.Count - 1; i++)
      for (var j = 0; j < list.Count - i - 1; j++)
        if (getValue(list[j]) > getValue(list[j + 1]))
          (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }
  }
}