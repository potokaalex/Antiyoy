using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.Background
{
  public class MenuBackgroundView : MonoBehaviour
  {
    [SerializeField] private BackgroundParticlesAnimator _particlesAnimator;
    [SerializeField] private RectTransform _backgroundTransform;
    [SerializeField] private Image _background;
    [SerializeField] private Image _fade;

    private void Awake()
    {
      gameObject.SetActive(false);
      _fade.color = Color.clear;
    }

    public Tween PlayAppearAnimation()
    {
      return DOTween.Sequence().AppendCallback(() =>
      {
        gameObject.SetActive(true);
        _particlesAnimator.PlayShowAnimation();
      }).Join(_backgroundTransform.DOScale(25, 1f));
    }

    public Tween PlayColorTransition(Color backgroundColor, Color particlesColor)
    {
      return DOTween.Sequence()
        .Append(_background.DOColor(backgroundColor, 0.5f))
        .Join(_particlesAnimator.PlayColorTransition(particlesColor));
    }

    public void PlayHideAnimation()
    {
      _fade.color = Color.clear;

      DOTween.Sequence()
        .Append(_fade.DOColor(AnimationsUtilities.GameplayBackgroundColor, 0.5f))
        .Join(_particlesAnimator.PlayHideAnimation())
        .SetEase(AnimationsUtilities.MenuDefaultEase);
    }

    public void PlayShowAnimation()
    {
      _fade.color = AnimationsUtilities.GameplayBackgroundColor;

      DOTween.Sequence()
        .Append(_fade.DOColor(Color.clear, 0.5f))
        .Join(_particlesAnimator.PlayShowAnimation())
        .SetEase(AnimationsUtilities.MenuDefaultEase);
    }
  }
}