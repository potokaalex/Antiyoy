using DG.Tweening;
using UnityEngine;

namespace Client.Menu.Background
{
  public class MenuBackgroundView : MonoBehaviour
  {
    [SerializeField] private BackgroundParticlesAnimator _particlesAnimator;
    [SerializeField] private RectTransform _circle;

    private void Awake() => gameObject.SetActive(false);

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