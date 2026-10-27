using Client.Gameplay.Player;
using Client.Project.Infrastructure;
using Client.Project.UI;
using Client.Project.Utilities;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI.IncomeWindow
{
  public class IncomeWindowView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private RectTransform _root;
    [SerializeField] private TextMeshProUGUI _cellsCount;
    [SerializeField] private TextMeshProUGUI _farmsCount;
    [SerializeField] private TextMeshProUGUI _warriorsCount;
    [SerializeField] private TextMeshProUGUI _towersCount;
    [SerializeField] private TextMeshProUGUI _tressCount;
    [SerializeField] private CanvasGroup _canvasGroup;

    private PlayerViewController _playerViewController;

    private void Awake()
    {
      _playerViewController = Locator.Get<PlayerViewController>();
      _root.gameObject.SetActive(false);
      _closeButton.OnClick += Close;
    }

    private void OnDestroy() => _closeButton.OnClick -= Close;

    public void Open()
    {
      SetActive(true);
      _cellsCount.SetText(FormatValue(_playerViewController.SelectedRegion.GetCellsIncome()));
      _farmsCount.SetText(FormatValue(_playerViewController.SelectedRegion.GetFarmsIncome()));
      _warriorsCount.SetText(FormatValue(_playerViewController.SelectedRegion.GetWarriorsIncome()));
      _towersCount.SetText(FormatValue(_playerViewController.SelectedRegion.GetTowersIncome()));
      _tressCount.SetText(FormatValue(_playerViewController.SelectedRegion.GetTressIncome()));
    }

    private string FormatValue(int value)
    {
      if (value > 0)
        return $"+{value}";
      return value.ToString();
    }

    private void Close() => SetActive(false);

    private void SetActive(bool isActive) => AnimationsUtilities.GameplayFlashUpAnimation(isActive, -1230, _root, _canvasGroup, this);
  }
}