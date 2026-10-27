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

    public static void GameplayFlashUpAnimation(bool isActive, float minValue, RectTransform root, CanvasGroup canvasGroup, object id)
    {
      DOTween.Kill(id);

      if (isActive)
      {
        root.gameObject.SetActive(true);

        DOTween.Sequence()
          .Append(root.DOAnchorPos(new Vector2(0, minValue), new Vector2(0, 0), GameplayUIDefaultDuration))
          .Join(canvasGroup.DOFade(0, 1, GameplayUIDefaultDuration))
          .SetId(id);
      }
      else
      {
        DOTween.Sequence()
          .Append(root.DOAnchorPos(new Vector2(0, 0), new Vector2(0, minValue), GameplayUIDefaultDuration))
          .Join(canvasGroup.DOFade(1, 0, GameplayUIDefaultDuration))
          .SetId(id).onComplete += () => root.gameObject.SetActive(false);
      }
    }
  }
}