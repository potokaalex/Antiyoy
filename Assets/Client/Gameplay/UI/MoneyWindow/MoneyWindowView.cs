using System;
using System.Collections.Generic;
using System.Linq;
using Client.Gameplay.Player;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Gameplay.UI.MoneyWindow
{
  public class MoneyWindowView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private GameObject _root;
    [SerializeField] private MoneyWindowGovernmentView _viewPrefab;
    [SerializeField] private Transform _viewsRoot;
    private readonly List<MoneyWindowGovernmentView> _views = new();
    private GovernmentsService _governmentsService;
    private PlayerViewController _playerViewController;
    private ObjectPool<MoneyWindowGovernmentView> _pool;

    private void Awake()
    {
      _playerViewController = Locator.Get<PlayerViewController>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _root.gameObject.SetActive(false);
      _pool = new ObjectPool<MoneyWindowGovernmentView>(() => Instantiate(_viewPrefab, _viewsRoot), x => x.SetActive(true), x => x.SetActive(false));
      _closeButton.OnClick += Close;
    }

    private void OnDestroy() => _closeButton.OnClick -= Close;

    public void Open()
    {
      _root.gameObject.SetActive(true);
      ClearViews();
      CreateViews();
    }

    private void Close()
    {
      UniTask.Create(async () =>
      {
        await UniTask.Delay(TimeSpan.FromSeconds(0.15f));
        _root.gameObject.SetActive(false);
      });
    }

    private void CreateViews()
    {
      using (ListPool<GovernmentController>.Get(out var governments))
      {
        _governmentsService.GetAll(governments);
        governments.Remove(_governmentsService.Get(RegionType.Neutral));
        var playerGovernment = _governmentsService.Get(_playerViewController.SelectedRegion.Type);
        governments.Remove(playerGovernment);
        governments.Insert(0, playerGovernment);

        var maxColumnSize = 631f;
        var maxIncome = Mathf.Max(0, governments.Max(x => x.GetIncome()));
        var columnSizeFactor = maxColumnSize / maxIncome;

        foreach (var government in governments)
          CreateView(government, columnSizeFactor);
      }
    }

    private void CreateView(GovernmentController government, float columnSizeFactor)
    {
      var instance = _pool.Get();
      instance.Initialize(government, columnSizeFactor);
      _views.Add(instance);
    }

    private void ClearViews()
    {
      foreach (var view in _views)
        _pool.Release(view);
      _views.Clear();
    }
  }
}