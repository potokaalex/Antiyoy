using DG.Tweening;
using UnityEngine;

namespace Client.UITests.MainMenu
{
  public class MenuBackgroundView : MonoBehaviour
  {
    [SerializeField] private UIParticlesAnimator _particlesAnimator;
    [SerializeField] private RectTransform _circle;

    public Tween PlayAppearAnimation()
    {
      return DOTween.Sequence().AppendCallback(() =>
      {
        gameObject.SetActive(true);
        _particlesAnimator.PlayAppearAnimation();
      }).Join(_circle.DOScale(50, 2f));
    }
  }
}