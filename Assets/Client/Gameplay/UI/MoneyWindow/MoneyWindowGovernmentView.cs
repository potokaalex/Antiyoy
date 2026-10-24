using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Region;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI.MoneyWindow
{
  public class MoneyWindowGovernmentView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private RectTransform _column;
    [SerializeField] private Image _columnImage;
    private RegionsService _regionsService;

    private void Awake() => _regionsService = Locator.Get<RegionsService>();

    public void Initialize(GovernmentController government, float columnSizeFactor)
    {
      var income = government.GetIncome();
      if (income < 0)
        income = 0;
      _moneyCount.SetText(income.ToString());
      _column.sizeDelta = new Vector2(_column.sizeDelta.x, income * columnSizeFactor);
      _columnImage.color = _regionsService.GetColorFor(government.RegionsType);
    }

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);
  }
}