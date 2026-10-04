using DG.Tweening;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Client.Project.Utilities
{
  public static class AnimationsUtilities
  {
    public static readonly Ease MenuDefaultEase = Ease.InQuart;
    public static readonly Color GameplayBackgroundColor = new(0.2078431f, 0.2078431f, 0.2078431f, 1);
    public static readonly float GameplayUnitsYoyoAnimationOffset = 0.05f;
    public static readonly float GameplayUIDefaultDuration = 0.25f;

    public static T AddOnComplete<T>(this T tween, TweenCallback action) where T : Tween
    {
      if (tween == null || !tween.active)
        return tween;
      tween.onComplete += action;
      return tween;
    }

    public static Tween DOAnchorPos(this RectTransform target, Vector2 from, Vector2 to, float duration)
    {
      target.anchoredPosition = from;
      return target.DOAnchorPos(to, duration);
    }

    public static Tween DOFade(this CanvasGroup target, float from, float to, float duration)
    {
      target.alpha = from;
      return target.DOFade(to, duration);
    }
  }
}