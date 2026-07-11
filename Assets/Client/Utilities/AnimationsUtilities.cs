using DG.Tweening;
using UnityEngine;

namespace Client.Utilities
{
  public static class AnimationsUtilities
  {
    public static Tween DoAnchoredMove(RectTransform target, Vector2 from, Vector2 to)
    {
      target.anchoredPosition = from;
      return DOVirtual.Vector2(target.anchoredPosition, to, 0.2f, x => target.anchoredPosition = x).SetEase(Ease.OutQuad);
    }
  }
}