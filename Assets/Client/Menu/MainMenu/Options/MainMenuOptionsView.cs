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

      //_menuView.Background.PlayColorTransition()
      
      DOTween.Sequence()
        .Append(_topPanel.DOAnchorPos(_topPanelStartPosition, 0.5f))
        .Join(_body.DOScale(1, 0.5f))
        .Join(_canvasGroup.DOFade(1, 0.35f));
    }
  }
}