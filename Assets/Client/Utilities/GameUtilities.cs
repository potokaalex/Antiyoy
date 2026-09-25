using System.Collections.Generic;
using UnityEngine;

namespace Client.Utilities
{
  public static class GameUtilities
  {
    public static readonly List<CellController> AreaBuffer = new();

    public static void SetAlpha(this SpriteRenderer spriteRenderer, float value)
    {
      var color = spriteRenderer.color;
      color.a = value;
      spriteRenderer.color = color;
    }
  }
}