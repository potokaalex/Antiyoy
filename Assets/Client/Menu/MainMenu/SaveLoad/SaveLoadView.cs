using System.Collections.Generic;
using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Map;
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
    [SerializeField] private SaveLoadOptionsView _optionsView;
    [SerializeField] private SaveLoadDeleteView _deleteView;
    private readonly List<SaveLoadSlotView> _activeViews = new();
    private ObjectPool<SaveLoadSlotView> _pool;
    private InputController _inputController;

    protected MapsService MapsService { get; private set; }

    protected SaveLoadSlotView AutosaveSlotView { get; private set; }

    protected virtual void Awake()
    {
      _inputController = Locator.Get<InputController>();
      MapsService = Locator.Get<MapsService>();
      _pool = new ObjectPool<SaveLoadSlotView>(() => Instantiate(_slotViewPrefab, _slotsRoot), x => x.SetActive(true), x => x.SetActive(false));
      _backButton.OnClick += OnBackClick;
      _optionsView.Initialize(this);
      _deleteView.Initialize(this);
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

    public void ShowDeleteView(SaveLoadSlotView slotView)
    {
      Hide();
      _deleteView.Show(slotView);
    }

    public void DeleteSlot(SaveLoadSlotView slotView) => MapsService.DeleteMap(slotView.Map);

    public void Rename(SaveLoadSlotView slotView, string newName)
    {
      MapsService.RenameMap(slotView.Map, newName);
      slotView.Header = newName;
    }

    protected void Hide() => _menuAnimator.PlayHide();

    protected virtual void OnBackClick()
    {
    }

    protected SaveLoadSlotView CreateView(MapController map)
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
      if (MapsService.AutosaveMap != null)
        AutosaveSlotView = CreateView(MapsService.AutosaveMap);

      for (var i = 0; i < MapsService.Maps.Count; i++)
        CreateView(MapsService.Maps[i]);
    }

    protected virtual void ClearView(SaveLoadSlotView view)
    {
      view.OnClick -= OnViewClick;
      view.OnHold -= OnViewHold;
      _pool.Release(view);
      view.Clear();
    }

    protected virtual void OnViewClick(SaveLoadSlotView slotView)
    {
    }

    protected virtual void OnViewHold(SaveLoadSlotView slotView)
    {
      if (slotView != AutosaveSlotView)
        _optionsView.Show(slotView);
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