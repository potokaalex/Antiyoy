using Client.Infrastructure;
using Client.UI;
using Client.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI.Hud
{
  public class HudView : MonoBehaviour
  {
    [SerializeField] private CustomButton _nextTurnButton;
    [SerializeField] private RegionView _regionView;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _bottomPanel;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private CustomButton _pauseButton;
    private GameplayController _gameplayController;
    private Vector2 _topPanelStartPosition;
    private Vector2 _bottomPanelStartPosition;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _nextTurnButton.OnClick += _gameplayController.NextTurn;
      _pauseButton.OnClick += _gameplayController.Pause;
      _canvasGroup.alpha = 0;
      _topPanelStartPosition = _topPanel.anchoredPosition;
      _bottomPanelStartPosition = _bottomPanel.anchoredPosition;
    }

    private void OnDestroy()
    {
      _nextTurnButton.OnClick -= _gameplayController.NextTurn;
      _pauseButton.OnClick -= _gameplayController.Pause;
      DOTween.Kill(this);
    }

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");

    public void PlayShow()
    {
      _topPanel.anchoredPosition = _topPanelStartPosition + new Vector2(0, 150);
      _bottomPanel.anchoredPosition = _bottomPanelStartPosition - new Vector2(0, 150);

      DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(_topPanelStartPosition, 0.5f))
        .Join(_bottomPanel.DOAnchorPos(_bottomPanelStartPosition, 0.5f))
        .Join(_canvasGroup.DOFade(1, 0.5f))
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);
    }

    public void PlayHide()
    {
      DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(_topPanelStartPosition + new Vector2(0, 150), 0.25f))
        .Join(_bottomPanel.DOAnchorPos(_bottomPanelStartPosition - new Vector2(0, 150), 0.25f))
        .Join(_canvasGroup.DOFade(0, 0.25f))
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);
    }
  }
}