using System.Collections.Generic;
using System.Linq;
using Client.Gameplay.Player;
using Client.Project.Government;
using Client.Project.Infrastructure;
using Client.Project.Player;
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
    private PlayersService _playersService;
    private PlayerViewController _playerViewController;
    private ObjectPool<MoneyWindowGovernmentView> _pool;

    private void Awake()
    {
      _playerViewController = Locator.Get<PlayerViewController>();
      _governmentsService = Locator.Get<GovernmentsService>();
      _playersService = Locator.Get<PlayersService>();
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
      //получить всех игроков.
      
      using (ListPool<IPlayerController>.Get(out var players))
      {
        players.AddRange(_playersService.Players);
        var playerGovernment = players.First(x => x.RegionType == _playerViewController.SelectedRegion.Type);
        players.Remove(playerGovernment);
        players.Insert(0, playerGovernment);

        var maxColumnSize = 631f;
        var maxIncome = Mathf.Max(0, players.Max(x => _governmentsService.Get(x.RegionType).GetInfoIncome()));
        var columnSizeFactor = maxColumnSize / maxIncome;
        var width = Mathf.Min(875f / players.Count, 110f);

        for (var i = 0; i < players.Count; i++)
          CreateView(_governmentsService.Get(players[i].RegionType), columnSizeFactor, width, i);
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