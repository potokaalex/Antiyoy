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

    private void Awake() => gameObject.SetActive(false);

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
      DOTween.Sequence()
        .Append(PlayColorTransition(new Color(0.2078431f, 0.2078431f, 0.2078431f, 1), new Color(0.2078431f, 0.2078431f, 0.2078431f, 1)))
        .Join(_particlesAnimator.PlayHideAnimation())
        .SetEase(AnimationsUtilities.MenuDefaultEase);
    }
  }
}