using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Project.Utilities
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
    
    public static void SetAlpha(this Image image, float value)
    {
      var color = image.color;
      color.a = value;
      image.color = color;
    }
  }
}