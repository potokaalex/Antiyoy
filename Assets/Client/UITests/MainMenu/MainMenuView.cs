using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UITests.MainMenu
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private MenuBackgroundView _backgroundView;
    [SerializeField] private Image _fade;
    [SerializeField] private RectTransform _mask;
    [SerializeField] private RectTransform _underMask;
    [SerializeField] private RectTransform _playButton;
    [SerializeField] private RectTransform _topPanel;

    private void Awake() => gameObject.SetActive(false);

    public Tween PlayAppearAnimation()
    {
      return DOTween.Sequence()
        .AppendCallback(() => gameObject.SetActive(true))
        .Append(_backgroundView.PlayAppearAnimation())
        .Join(_fade.DOFade(0, 0.4f))
        .Join(DOTween.Sequence().AppendInterval(0.1f).Append(MoveAnimations()).Join(MaskAnimation()));
    }

    private Tween MoveAnimations()
    {
      return DOTween.Sequence()
        .AppendCallback(() => _topPanel.gameObject.SetActive(true))
        .Append(AnimationsUtilities.DoAnchoredMove(_topPanel, new Vector2(0, 250), new Vector2(0, 0), 0.3f))
        .Join(AnimationsUtilities.DoAnchoredMove(_playButton, _playButton.anchoredPosition + new Vector2(0, 150), _playButton.anchoredPosition,
          0.3f));
    }

    private Tween MaskAnimation()
    {
      var factor = 10f;
      var initialPos = _underMask.anchoredPosition;

      return DOTween.Sequence()
        .AppendCallback(() =>
        {
          _mask.gameObject.SetActive(true);
          _mask.localScale = Vector3.zero;
        })
        .Join(DOVirtual.Float(0, 1, 1, v =>
        {
          var f = factor * v;
          _mask.localScale = Vector3.one * f;
          _underMask.localScale = Vector3.one / f;
          _underMask.anchoredPosition = initialPos / f;
        }));
    }
  }
}