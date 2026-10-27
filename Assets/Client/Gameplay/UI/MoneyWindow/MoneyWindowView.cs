using System.Collections.Generic;
using System.Linq;
using Client.Gameplay.Player;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using Client.Project.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Gameplay.UI.MoneyWindow
{
  public class MoneyWindowView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private RectTransform _root;
    [SerializeField] private CanvasGroup _canvasGroup;
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
      SetActive(true);
      ClearViews();
      CreateViews();
    }

    private void Close() => SetActive(false);

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
        var maxIncome = Mathf.Max(0, governments.Max(x => x.GetInfoIncome()));
        var columnSizeFactor = maxColumnSize / maxIncome;
        var width = Mathf.Min(875f / governments.Count, 110f);

        for (var i = 0; i < governments.Count; i++) 
          CreateView(governments[i], columnSizeFactor, width, i);
      }
    }

    private void CreateView(GovernmentController government, float columnSizeFactor, float width, int index)
    {
      var instance = _pool.Get();
      instance.transform.SetSiblingIndex(index);
      instance.Initialize(government, columnSizeFactor, width);
      _views.Add(instance);
    }

    private void ClearViews()
    {
      foreach (var view in _views)
        _pool.Release(view);
      _views.Clear();
    }

    private void SetActive(bool isActive) => AnimationsUtilities.GameplayFlashUpAnimation(isActive, -1230, _root, _canvasGroup, this);
  }
}