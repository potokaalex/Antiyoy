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
    [SerializeField] private RectTransform _creationPanel;

    public RegionCreationView Creation => _creationView;

    public void SetActive(bool isActive)
    {
      if (gameObject.activeSelf == isActive)
        return;

      if (isActive)
      {
        gameObject.SetActive(true);
        AnimationsUtilities.DoAnchoredMove(_topPanel, new Vector2(0, 150), new Vector2(0, 0));
        AnimationsUtilities.DoAnchoredMove(_creationPanel, new Vector2(0, -150), new Vector2(0, 0));
      }
      else
      {
        DOTween.Sequence()
          .Append(AnimationsUtilities.DoAnchoredMove(_topPanel, new Vector2(0, 0), new Vector2(0, 150)))
          .Join(AnimationsUtilities.DoAnchoredMove(_creationPanel, new Vector2(0, 0), new Vector2(0, -150)))
          .onComplete += () => gameObject.SetActive(false);
      }
    }

    public void ViewMoney(int count) => _incomeCount.SetText(count.ToString());

    public void ViewIncome(int count)
    {
      var sign = count > 0 ? "+" : "-";
      _incomeCount.SetText($"{sign}{Mathf.Abs(count)}");
    }
  }
}