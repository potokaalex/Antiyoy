using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.Utilities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI.MoneyWindow
{
  public class MoneyWindowGovernmentView : MonoBehaviour
  {
    [SerializeField] private RectTransform _root;
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private RectTransform _column;
    [SerializeField] private Image _columnImage;
    private RegionsService _regionsService;

    private void Awake() => _regionsService = Locator.Get<RegionsService>();

    public void Initialize(GovernmentController government, float columnSizeFactor, float width)
    {
      var income = government.GetInfoIncome();
      if (income < 0 || !government.IsAlive)
        income = 0;

      _moneyCount.SetText(income.ToString());
      ViewColumn(government, income * columnSizeFactor);
      _root.sizeDelta = new Vector2(width, _root.sizeDelta.y);
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    private void ViewColumn(GovernmentController government, float maxSize)
    {
      DOTween.Kill(this);

      SetColumnSize(0);
      _columnImage.color = _regionsService.GetColorFor(government.RegionsType);

      DOTween.Sequence()
        .AppendInterval(AnimationsUtilities.GameplayUIDefaultDuration)
        .Append(DOVirtual.Float(0, 1, AnimationsUtilities.GameplayUIDefaultDuration, v => SetColumnSize(Mathf.Lerp(0, maxSize, v))))
        .SetId(this);
    }

    private void SetColumnSize(float value) => _column.sizeDelta = new Vector2(_column.sizeDelta.x, value);
  }
}