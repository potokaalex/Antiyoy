using System.Collections.Generic;
using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Map.Data;
using Client.Project.Progress;
using Client.Project.UI;
using UnityEngine;
using UnityEngine.Pool;

namespace Client.Menu.MainMenu.SaveLoad
{
  public class SaveLoadView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private SaveLoadSlotView _slotViewPrefab;
    [SerializeField] private Transform _slotsRoot;
    [SerializeField] private List<Color> _colors;
    private readonly List<SaveLoadSlotView> _activeViews = new();
    private InputController _inputController;
    private ProgressController _progressController;
    private ObjectPool<SaveLoadSlotView> _pool;

    protected MapData AutosaveMapData => _progressController.Data.AutosaveMap;

    protected virtual void Awake()
    {
      _inputController = Locator.Get<InputController>();
      _progressController = Locator.Get<ProgressController>();
      _pool = new ObjectPool<SaveLoadSlotView>(() => Instantiate(_slotViewPrefab, _slotsRoot), x => x.SetActive(true), x => x.SetActive(false));
      _backButton.OnClick += OnBackClick;
    }

    private void OnDestroy() => _backButton.OnClick -= OnBackClick;

    private void Update()
    {
      if (_inputController.BackClicked)
        OnBackClick();
    }

    public void Show()
    {
      _menuAnimator.PlayShow();
      Load();
    }

    protected void Hide() => _menuAnimator.PlayHide();

    protected virtual void OnBackClick()
    {
    }

    protected SaveLoadSlotView CreateView(MapData map)
    {
      var view = _pool.Get();
      view.transform.SetSiblingIndex(_activeViews.Count + 1);
      view.Setup(map, _colors[_activeViews.Count % _colors.Count]);
      _activeViews.Add(view);
      view.OnClick += OnViewClick;
      view.OnHold += OnViewHold;
      return view;
    }

    protected virtual void CreateViews()
    {
      if (AutosaveMapData != null)
        CreateView(AutosaveMapData);

      for (var i = 0; i < _progressController.Data.Maps.Count; i++)
        CreateView(_progressController.Data.Maps[i]);
    }

    protected virtual void ClearView(SaveLoadSlotView view)
    {
      view.OnClick -= OnViewClick;
      view.OnHold -= OnViewHold;
      _pool.Release(view);
      view.Clear();
    }

    protected virtual void OnViewClick(MapData map)
    {
    }

    protected virtual void OnViewHold(MapData map)
    {
    }

    private void Load()
    {
      ClearViews();
      CreateViews();
    }

    private void ClearViews()
    {
      foreach (var view in _activeViews)
        ClearView(view);
      _activeViews.Clear();
    }
  }
}