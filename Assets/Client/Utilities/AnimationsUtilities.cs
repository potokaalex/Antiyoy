using DG.Tweening;
using UnityEngine;

namespace Client.Utilities
{
  public static class AnimationsUtilities
  {
    public static readonly Ease MenuDefaultEase = Ease.InQuint;
    public static readonly Color GameplayBackgroundColor = new(0.2078431f, 0.2078431f, 0.2078431f, 1);

    public static Tween DoAnchoredMove(RectTransform target, Vector2 from, Vector2 to, float duration = 0.25f)
    {
      target.anchoredPosition = from;
      return DOVirtual.Vector2(target.anchoredPosition, to, duration, x => target.anchoredPosition = x).SetEase(Ease.OutQuad);
    }

    public static Tween DoFade(CanvasGroup canvasGroup, float from, float to, float duration = 0.25f)
    {
      canvasGroup.alpha = from;
      return canvasGroup.DOFade(to, duration).SetEase(Ease.OutQuad);
    }

    public static T AddOnComplete<T>(this T tween, TweenCallback action) where T : Tween
    {
      if (tween == null || !tween.active)
        return tween;
      tween.onComplete += action;
      return tween;
    }
  }
}