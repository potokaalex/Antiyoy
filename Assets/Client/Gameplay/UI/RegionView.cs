using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class RegionView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _moneyCount;
    [SerializeField] private TextMeshProUGUI _incomeCount;
    [SerializeField] private RegionCreationView _creationView;

    public RegionCreationView Creation => _creationView;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void ViewMoney(int count) => _incomeCount.SetText(count.ToString());

    public void ViewIncome(int count)
    {
      var sign = count > 0 ? "+" : "-";
      _incomeCount.SetText($"{sign}{Mathf.Abs(count)}");
    }
  }
}