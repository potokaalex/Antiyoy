using Client.Infrastructure;
using Client.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class Hud : MonoBehaviour
  {
    [SerializeField] private CustomButton _nextTurnButton;
    [SerializeField] private RegionView _regionView;
    [SerializeField] private TextMeshProUGUI _turnsCount;
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _bottomPanel;
    [SerializeField] private CanvasGroup _canvasGroup;
    private GameplayController _gameplayController;

    public RegionView Region => _regionView;

    private void Awake()
    {
      _gameplayController = Locator.Get<GameplayController>();
      _nextTurnButton.OnClick += _gameplayController.NextTurn;
      _canvasGroup.alpha = 0;
    }

    private void OnDestroy() => _nextTurnButton.OnClick -= _gameplayController.NextTurn;

    public void ViewTurnsCount(int value) => _turnsCount.SetText($"Turn {value}");

    public Tween PlayShow()
    {
      var topPanelStartPosition = _topPanel.anchoredPosition;
      _topPanel.anchoredPosition = topPanelStartPosition + new Vector2(0, 150);

      var bottomPanelStartPosition = _bottomPanel.anchoredPosition;
      _bottomPanel.anchoredPosition = bottomPanelStartPosition - new Vector2(0, 150);

      return DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(topPanelStartPosition, 0.5f))
        .Join(_bottomPanel.DOAnchorPos(bottomPanelStartPosition, 0.5f))
        .Join(_canvasGroup.DOFade(1, 0.5f));
    }
  }
}