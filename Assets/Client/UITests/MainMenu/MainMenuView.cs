using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UITests.MainMenu
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private UIBackgroundAnimator _backgroundAnimator;
    [SerializeField] private Image _fade;
    [SerializeField] private RectTransform _mask;
    [SerializeField] private RectTransform _underMask;
    [SerializeField] private RectTransform _playButton;
    [SerializeField] private RectTransform _topPanel;

    private void Awake()
    {
      _backgroundAnimator.PlayAppearAnimation();
      _fade.DOFade(0, 0.4f);
      DOTween.Sequence().AppendInterval(0.1f).Append(MoveAnimations()).Join(MaskAnimation());
    }

    private Tween MoveAnimations()
    {
      _topPanel.gameObject.SetActive(true);
      return DOTween.Sequence()
        .Append(AnimationsUtilities.DoAnchoredMove(_topPanel, new Vector2(0, 250), new Vector2(0, 0), 0.3f))
        .Join(AnimationsUtilities.DoAnchoredMove(_playButton, _playButton.anchoredPosition + new Vector2(0, 150), _playButton.anchoredPosition,
          0.3f));
    }

    private Tween MaskAnimation()
    {
      var factor = 10f;
      var initialPos = _underMask.anchoredPosition;
      _mask.gameObject.SetActive(true);
      _mask.localScale = Vector3.zero;

      return DOVirtual.Float(0, 1, 1, v =>
      {
        var f = factor * v;
        _mask.localScale = Vector3.one * f;
        _underMask.localScale = Vector3.one / f;
        _underMask.anchoredPosition = initialPos / f;
      });
    }
  }
}