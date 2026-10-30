using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Menu
{
  public class MenuAnimator : MonoBehaviour
  {
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _body;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _hasBackgroundAnimation = true;
    [SerializeField] [ShowIf(nameof(_hasBackgroundAnimation))] private Color _backgroundShowColor;
    [SerializeField] [ShowIf(nameof(_hasBackgroundAnimation))] private Color _particlesShowColor;
    [SerializeField] private bool _autoInitialize = true;
    [SerializeField] private float _speedFactor = 1;
    private readonly float _bodyMinScale = 0.25f;
    private MenuView _menuView;
    private InputController _inputController;
    private Vector2 _topPanelStartPosition;
    private Vector2 _topPanelEndPosition;
    private bool _initialized;

    private void Awake()
    {
      if (_autoInitialize)
        Initialize();
    }

    private void OnDestroy() => DOTween.Kill(this);

    public void Initialize()
    {
      if (_initialized)
        return;
      _initialized = true;

      _menuView = Locator.Get<MenuView>();
      _inputController = Locator.Get<InputController>();

      gameObject.SetActive(true);

      if (_topPanel)
      {
        _topPanelStartPosition = _topPanel.anchoredPosition;
        _topPanelEndPosition = _topPanelStartPosition + new Vector2(0, 250);
        _topPanel.anchoredPosition = _topPanelEndPosition;
      }

      gameObject.SetActive(false);
      if (_body)
        _body.localScale = Vector3.one * _bodyMinScale;
      _canvasGroup.alpha = 0;
    }

    public void PlayShow()
    {
      Initialize();
      var tween = MenuAnimation(_topPanelStartPosition, 1, 1).JoinCallback(() => gameObject.SetActive(true));
      if (_hasBackgroundAnimation)
        tween.Join(_menuView.Background.PlayColorTransition(_backgroundShowColor, _particlesShowColor));
    }

    public void PlayHide()
    {
      Initialize();
      MenuAnimation(_topPanelEndPosition, _bodyMinScale, 0).AddOnComplete(() => gameObject.SetActive(false));
    }

    private Sequence MenuAnimation(Vector3 topPosition, float bodyScale, float alpha)
    {
      var sequence = DOTween.Sequence();

      if (_topPanel)
        sequence.Join(_topPanel.DOAnchorPos(topPosition, 0.5f / _speedFactor));

      if (_body)
        sequence.Join(_body.DOScale(bodyScale, 0.5f / _speedFactor));

      sequence
        .Join(_canvasGroup.DOFade(alpha, 0.35f / _speedFactor))
        .JoinCallback(() => _inputController.SetBlockInput(true))
        .AddOnComplete(() => _inputController.SetBlockInput(false))
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);

      return sequence;
    }
  }
}