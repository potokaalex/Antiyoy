using Client.Infrastructure;
using Client.UI;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Menu.MainMenu.Start
{
  public class MainMenuStartView : MonoBehaviour
  {
    [SerializeField] private Image _fade;
    [SerializeField] private RectTransform _mask;
    [SerializeField] private RectTransform _underMask;
    [SerializeField] private RectTransform _playButtonTransform;
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private CustomButton _playButton;
    [SerializeField] private Image _playButtonAnimatedBackground;
    [SerializeField] private CanvasGroup _rootCanvasGroup;
    private MenuView _menuView;
    private MainMenuView _mainMenuView;
    private Vector2 _playButtonStartPosition;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();
      _mainMenuView = Locator.Get<MainMenuView>();
      _playButton.OnClick += OnPlayClick;
      gameObject.SetActive(false);
      _topPanel.gameObject.SetActive(false);
      _fade.gameObject.SetActive(true);
      _mask.gameObject.SetActive(false);
      _playButtonStartPosition = _playButtonTransform.anchoredPosition;
    }

    private void OnDestroy() => _playButton.OnClick -= OnPlayClick;

    private void OnPlayClick()
    {
      //UnityEngine.Debug.Break();
      _playButtonAnimatedBackground.gameObject.SetActive(true);
      _playButtonAnimatedBackground.transform.localScale = Vector3.one;
      DOTween.Sequence()
        .Append(_playButtonAnimatedBackground.transform.DOScale(1.25f, 0.1f))
        .Append(HideMoveAnimation())
        .Join(_playButtonTransform.transform.DOScale(0.5f, 0.5f))
        .JoinCallback(_mainMenuView.ShowOptions);
    }

    public Tween PlayAppearAnimation()
    {
      return DOTween.Sequence()
        .AppendCallback(() => gameObject.SetActive(true))
        .Append(_menuView.Background.PlayAppearAnimation())
        .Join(_fade.DOFade(0, 0.4f).OnComplete(() => _fade.gameObject.SetActive(false)))
        .Join(DOTween.Sequence().AppendInterval(0.1f).Append(MoveAnimations()).Join(MaskAnimation()));
    }

    private Tween MoveAnimations()
    {
      return DOTween.Sequence()
        .AppendCallback(() => _topPanel.gameObject.SetActive(true))
        .Append(AnimationsUtilities.DoAnchoredMove(_topPanel, new Vector2(0, 250), new Vector2(0, 0), 0.3f))
        .Join(AnimationsUtilities.DoAnchoredMove(_playButtonTransform, _playButtonStartPosition + new Vector2(0, 150), 
          _playButtonStartPosition, 0.3f));
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

    private Tween HideMoveAnimation()
    {
      return DOTween.Sequence()
        .Append(_topPanel.DOAnchorPosY(250f, 0.5f))
        .Join(_rootCanvasGroup.DOFade(0, 0.35f));
    }
  }
}