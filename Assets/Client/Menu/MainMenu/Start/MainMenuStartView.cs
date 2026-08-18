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
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private CustomButton _quitButton;
    private MenuView _menuView;
    private InputController _inputController;
    private MainMenuView _mainMenuView;
    private Vector2 _playButtonStartPosition;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
      _playButton.OnClick += OnPlayClick;
      _quitButton.OnClick += Quit;
      _menuAnimator.Initialize();
      _fade.gameObject.SetActive(true);
      _mask.gameObject.SetActive(false);
      _playButtonStartPosition = _playButtonTransform.anchoredPosition;
    }

    private void OnDestroy()
    {
      _playButton.OnClick -= OnPlayClick;
      _quitButton.OnClick -= Quit;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        Quit();
    }

    public Tween PlayAppearAnimation()
    {
      return DOTween.Sequence()
        .AppendCallback(() =>
        {
          gameObject.SetActive(true);
          _rootCanvasGroup.alpha = 1;
          _playButtonTransform.transform.localScale = Vector3.one;
        })
        .Append(_menuView.Background.PlayAppearAnimation())
        .Join(_fade.DOFade(0, 0.4f).OnComplete(() => _fade.gameObject.SetActive(false)))
        .Join(DOTween.Sequence().AppendInterval(0.1f).Append(MoveAnimations()).Join(MaskAnimation()));
    }

    public void Show() => _menuAnimator.PlayShow();

    private void OnPlayClick()
    {
      PlayClickAnimation();
      _menuAnimator.PlayHide();
      _mainMenuView.ShowOptions();
    }

    private void PlayClickAnimation()
    {
      _playButtonAnimatedBackground.gameObject.SetActive(true);
      _playButtonAnimatedBackground.transform.localScale = Vector3.one;
      DOTween.Sequence()
        .Append(_playButtonAnimatedBackground.transform.DOScale(1.25f, 0.15f))
        .InsertCallback(0.5f, () => _playButtonAnimatedBackground.gameObject.SetActive(false));
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

    private void Quit()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}