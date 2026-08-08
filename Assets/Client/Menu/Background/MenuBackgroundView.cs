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
        _particlesAnimator.PlayAppearAnimation();
      }).Join(_backgroundTransform.DOScale(25, 1f));
    }

    public Tween PlayColorTransition(Color backgroundColor, Color particlesColor)
    {
      return DOTween.Sequence()
        .Append(_background.DOColor(backgroundColor, 0.5f))
        .Join(_particlesAnimator.PlayColorTransition(particlesColor));
    }
  }
}