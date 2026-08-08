using Client.Infrastructure;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu.MainMenu.Options
{
  public class MainMenuOptionsView : MonoBehaviour
  {
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private RectTransform _body;
    [SerializeField] private CanvasGroup _canvasGroup;
    private Vector2 _topPanelStartPosition;
    private MenuView _menuView;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();
      gameObject.SetActive(false);
      _topPanelStartPosition = _topPanel.anchoredPosition;
    }

    public void Show()
    {
      gameObject.SetActive(true);
      _topPanel.anchoredPosition = _topPanelStartPosition + new Vector2(0, 250);
      _body.localScale = Vector3.one * 0.25f;
      _canvasGroup.alpha = 0;

      DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(_topPanelStartPosition, 0.5f))
        .Join(_body.DOScale(1, 0.5f))
        .Join(_canvasGroup.DOFade(1, 0.35f))
        .Join(_menuView.Background.PlayColorTransition(new Color(0.4627451f,0.4862745f, 0.654902f), new Color(0.36078432f, 0.4509804f, 0.50980395f)));
    }
  }
}