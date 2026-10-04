using Client.Project.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI.Hud
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
        _topPanel.DOAnchorPos(new Vector2(0, 0), AnimationsUtilities.GameplayUIDefaultDuration);
        _creationPanel.DOAnchorPos(new Vector2(0, 0), AnimationsUtilities.GameplayUIDefaultDuration);
        _topPanelCanvasGroup.DOFade(1, AnimationsUtilities.GameplayUIDefaultDuration);
        _creationPanelCanvasGroup.DOFade(1, AnimationsUtilities.GameplayUIDefaultDuration);
      }
      else
      {
        _isActive = false;
        DOTween.Sequence().SetId(this)
          .Append(_topPanel.DOAnchorPos(new Vector2(0, 150), AnimationsUtilities.GameplayUIDefaultDuration))
          .Join(_creationPanel.DOAnchorPos(new Vector2(0, -150), AnimationsUtilities.GameplayUIDefaultDuration))
          .Join(_topPanelCanvasGroup.DOFade(0, AnimationsUtilities.GameplayUIDefaultDuration))
          .Join(_creationPanelCanvasGroup.DOFade(0, AnimationsUtilities.GameplayUIDefaultDuration))
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