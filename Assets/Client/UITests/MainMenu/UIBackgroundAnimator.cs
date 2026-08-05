using DG.Tweening;
using UnityEngine;

namespace Client.UITests.MainMenu
{
  public class UIBackgroundAnimator : MonoBehaviour
  {
    [SerializeField] private UIParticlesAnimator _particlesAnimator;
    [SerializeField] private RectTransform _circle;

    public void PlayAppearAnimation()
    {
      _particlesAnimator.PlayAppearAnimation();
      _circle.DOScale(50, 2f);
    }
  }
}