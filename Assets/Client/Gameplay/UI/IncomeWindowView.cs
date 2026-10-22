using System;
using Client.Gameplay.Player;
using Client.Project.Infrastructure;
using Client.Project.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class IncomeWindowView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private GameObject _root;
    [SerializeField] private TextMeshProUGUI _cellsCount;
    [SerializeField] private TextMeshProUGUI _farmsCount;
    [SerializeField] private TextMeshProUGUI _warriorsCount;
    [SerializeField] private TextMeshProUGUI _towersCount;
    [SerializeField] private TextMeshProUGUI _tressCount;
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
      _root.gameObject.SetActive(true);
      _cellsCount.SetText(FormatValue(_playerViewController.SelectedRegion.CellsIncome()));
      _farmsCount.SetText(FormatValue(_playerViewController.SelectedRegion.FarmsIncome()));
      _warriorsCount.SetText(FormatValue(_playerViewController.SelectedRegion.WarriorsIncome()));
      _towersCount.SetText(FormatValue(_playerViewController.SelectedRegion.TowersIncome()));
      _tressCount.SetText(FormatValue(_playerViewController.SelectedRegion.TressIncome()));
    }

    private string FormatValue(int value)
    {
      if (value > 0)
        return $"+{value}";
      return value.ToString();
    }

    private void Close()
    {
      UniTask.Create(async () =>
      {
        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
        _root.gameObject.SetActive(false);
      });
    }
  }
}