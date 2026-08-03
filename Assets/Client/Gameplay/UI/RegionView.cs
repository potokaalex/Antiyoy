using Client.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class RegionView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private TextMeshProUGUI _incomeCount;
    [SerializeField] private RegionCreationView _creationView;
    [SerializeField] private RectTransform _topPanel;
    [SerializeField] private CanvasGroup _topPanelCanvasGroup;
    [SerializeField] private RectTransform _creationPanel;
    [SerializeField] private CanvasGroup _creationPanelCanvasGroup;
    private bool _isActive;

    public RegionCreationView Creation => _creationView;

    private void Awake()
    {
      _topPanel.anchoredPosition = new Vector2(0, 150);
      _creationPanel.anchoredPosition = new Vector2(0, -150);
      _topPanelCanvasGroup.alpha = 0;
      _creationPanelCanvasGroup.alpha = 0;
    }

    public void SetActive(bool isActive)
    {
      if (_isActive == isActive)
        return;

      DOTween.Kill(this);

      if (isActive)
      {
        _isActive = true;
        gameObject.SetActive(true);
        AnimationsUtilities.DoAnchoredMove(_topPanel, _topPanel.anchoredPosition, new Vector2(0, 0));
        AnimationsUtilities.DoAnchoredMove(_creationPanel, _creationPanel.anchoredPosition, new Vector2(0, 0));
        AnimationsUtilities.DoFade(_topPanelCanvasGroup, _topPanelCanvasGroup.alpha, 1);
        AnimationsUtilities.DoFade(_creationPanelCanvasGroup, _creationPanelCanvasGroup.alpha, 1);
      }
      else
      {
        _isActive = false;
        DOTween.Sequence().SetId(this)
          .Append(AnimationsUtilities.DoAnchoredMove(_topPanel, _topPanel.anchoredPosition, new Vector2(0, 150)))
          .Join(AnimationsUtilities.DoAnchoredMove(_creationPanel, _creationPanel.anchoredPosition, new Vector2(0, -150)))
          .Join(AnimationsUtilities.DoFade(_topPanelCanvasGroup, _topPanelCanvasGroup.alpha, 0))
          .Join(AnimationsUtilities.DoFade(_creationPanelCanvasGroup, _creationPanelCanvasGroup.alpha, 0))
          .onComplete += () => gameObject.SetActive(false);
      }
    }

    public void ViewMoney(int count) => _moneyCount.SetText(count.ToString());

    public void ViewIncome(int count)
    {
      var sign = string.Empty;
      if (count != 0)
        sign = count > 0 ? "+" : "-";
      _incomeCount.SetText($"{sign}{Mathf.Abs(count)}");
    }
  }
}