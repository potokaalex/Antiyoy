using Client.Infrastructure;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu
{
  public class MenuAnimator : MonoBehaviour
  {
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _body;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Color _backgroundShowColor;
    [SerializeField] private Color _particlesShowColor;
    [SerializeField] private bool _autoInitialize = true;
    private readonly float _bodyMinScale = 0.25f;
    private Vector2 _topPanelStartPosition;
    private Vector2 _topPanelEndPosition;
    private MenuView _menuView;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();

      if (_autoInitialize)
        Initialize();
    }

    private void OnDestroy() => DOTween.Kill(this);

    public void Initialize()
    {
      if (_topPanel)
      {
        _topPanelStartPosition = _topPanel.anchoredPosition;
        _topPanelEndPosition = _topPanelStartPosition + new Vector2(0, 250);
        _topPanel.anchoredPosition = _topPanelEndPosition;
      }

      gameObject.SetActive(false);
      _body.localScale = Vector3.one * _bodyMinScale;
      _canvasGroup.alpha = 0;
    }

    public void PlayShow()
    {
      MenuAnimation(_topPanelStartPosition, 1, 1)
        .JoinCallback(() => gameObject.SetActive(true))
        .Join(_menuView.Background.PlayColorTransition(_backgroundShowColor, _particlesShowColor));
    }

    public void PlayHide() => MenuAnimation(_topPanelEndPosition, _bodyMinScale, 0).AddOnComplete(() => gameObject.SetActive(false));

    private Sequence MenuAnimation(Vector3 topPosition, float bodyScale, float alpha)
    {
      var sequence = DOTween.Sequence();

      if (_topPanel)
        sequence.Join(_topPanel.DOAnchorPos(topPosition, 0.5f));

      sequence.Join(_body.DOScale(bodyScale, 0.5f))
        .Join(_canvasGroup.DOFade(alpha, 0.35f))
        .JoinCallback(() => _menuView.SetBlockInput(true))
        .AddOnComplete(() => _menuView.SetBlockInput(false))
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);

      return sequence;
    }
  }
}