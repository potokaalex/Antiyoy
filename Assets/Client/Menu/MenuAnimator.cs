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
    private readonly float _bodyMinScale = 0.15f;
    private Vector2 _topPanelStartPosition;
    private Vector2 _topPanelEndPosition;
    private MenuView _menuView;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();

      if (_autoInitialize)
        Initialize();
    }

    public void Initialize()
    {
      _topPanelStartPosition = _topPanel.anchoredPosition;
      _topPanelEndPosition = _topPanelStartPosition + new Vector2(0, 250);

      gameObject.SetActive(false);
      _topPanel.anchoredPosition = _topPanelEndPosition;
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
      return DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(topPosition, 0.5f))
        .Join(_body.DOScale(bodyScale, 0.5f))
        .Join(_canvasGroup.DOFade(alpha, 0.35f))
        .JoinCallback(() => _menuView.SetBlockInput(true))
        .AddOnComplete(() => _menuView.SetBlockInput(false))
        .SetEase(Ease.InQuint);
    }
  }
}