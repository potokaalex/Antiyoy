using System.Collections.Generic;
using Client.Project;
using Client.Project.Infrastructure;
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

    private void Load()
    {
      ClearViews();

      for (var i = 0; i < _progressController.Data.Maps.Count; i++)
      {
        var view = _pool.Get();
        view.transform.SetSiblingIndex(i + 1);
        view.Initialize(_progressController.Data.Maps[i], _colors[i % _colors.Count]);
        _activeViews.Add(view);
      }
    }

    private void ClearViews()
    {
      foreach (var view in _activeViews)
        _pool.Release(view);
      _activeViews.Clear();
    }
  }
}