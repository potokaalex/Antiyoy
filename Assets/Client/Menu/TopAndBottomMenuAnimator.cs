using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu
{
  public class TopAndBottomMenuAnimator : MonoBehaviour
  {
    [SerializeField] private RectTransform _root;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private float _topPanelOffsetY;
    [SerializeField] private RectTransform _bottomPanel;
    [SerializeField] private float _bottomPanelOffsetY;
    [SerializeField] private float _speedFactor = 1;
    private InputController _inputController;
    private Vector2 _topPanelStartPosition;
    private Vector2 _topPanelEndPosition;
    private Vector2 _bottomPanelStartPosition;
    private Vector2 _bottomPanelEndPosition;

    public float SpeedFactor { get => _speedFactor; set => _speedFactor = value; }

    public float BottomPanelOffsetY
    {
      get => _bottomPanelOffsetY;
      set
      {
        _bottomPanelEndPosition = _bottomPanelStartPosition + new Vector2(0, _bottomPanelOffsetY);
        _bottomPanelOffsetY = value;
      }
    }

    private void Awake()
    {
      _inputController = Locator.Get<InputController>();

      if (_topPanel)
      {
        _topPanelStartPosition = _topPanel.anchoredPosition;
        _topPanelEndPosition = _topPanelStartPosition + new Vector2(0, _topPanelOffsetY);
        _topPanel.anchoredPosition = _topPanelEndPosition;
      }

      _bottomPanelStartPosition = _bottomPanel.anchoredPosition;
      _bottomPanelEndPosition = _bottomPanelStartPosition + new Vector2(0, _bottomPanelOffsetY);
      _bottomPanel.anchoredPosition = _bottomPanelEndPosition;

      _root.gameObject.SetActive(false);
      if(_canvasGroup)
        _canvasGroup.alpha = 0;
    }

    private void OnDestroy() => DOTween.Kill(this);

    public void PlayShow()
    {
      _root.gameObject.SetActive(true);
      MenuAnimation(_topPanelStartPosition, _bottomPanelStartPosition, 1);
    }

    public void PlayHide() => MenuAnimation(_topPanelEndPosition, _bottomPanelEndPosition, 0).AddOnComplete(() => _root.gameObject.SetActive(false));

    private Sequence MenuAnimation(Vector3 topPosition, Vector3 bottomPosition, float alpha)
    {
      var sequence = DOTween.Sequence();

      if (_topPanel)
        sequence.Join(_topPanel.DOAnchorPos(topPosition, 0.5f / SpeedFactor));
      sequence.Join(_bottomPanel.DOAnchorPos(bottomPosition, 0.5f / SpeedFactor));

      if (_canvasGroup)
        sequence.Join(_canvasGroup.DOFade(alpha, 0.35f / SpeedFactor));

      sequence.JoinCallback(() => _inputController.SetBlockInput(true))
        .AddOnComplete(() => _inputController.SetBlockInput(false))
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);

      return sequence;
    }
  }
}