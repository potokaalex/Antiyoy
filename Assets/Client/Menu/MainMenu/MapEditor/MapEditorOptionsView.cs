using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Progress;
using Client.Project.Project;
using Client.Project.UI;
using Client.Project.UI.Slider;
using UnityEngine;

namespace Client.Menu.MainMenu.MapEditor
{
  public class MapEditorOptionsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _backButton;
    [SerializeField] private CustomButton _createButton;
    [SerializeField] private CustomButton _loadButton;
    [SerializeField] private CustomButton _importButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomSlider _mapSize;
    [SerializeField] private MapEditorLoadView _loadView;
    private MainMenuView _mainMenuView;
    private InputController _inputController;
    private ProgressController _progressController;
    private MapsService _mapsService;
    private ProjectController _projectController;

    private void Awake()
    {
      _mainMenuView = Locator.Get<MainMenuView>();
      _inputController = Locator.Get<InputController>();
      _progressController = Locator.Get<ProgressController>();
      _mapsService = Locator.Get<MapsService>();
      _projectController = Locator.Get<ProjectController>();

      _backButton.OnClick += OnBackClick;
      _createButton.OnClick += OnCreateClick;
      _mapSize.OnSliderValueChanged += SaveData;
      _loadButton.OnClick += OnLoadClick;
      _importButton.OnClick += OnImportClick;
    }

    private void OnDestroy()
    {
      _backButton.OnClick -= OnBackClick;
      _createButton.OnClick -= OnCreateClick;
      _mapSize.OnSliderValueChanged -= SaveData;
      _loadButton.OnClick -= OnLoadClick;
      _importButton.OnClick -= OnImportClick;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        OnBackClick();
    }

    public void Show()
    {
      LoadData();
      _menuAnimator.PlayShow();
      _topAndBottomMenuAnimator.PlayShow();
    }

    private void OnBackClick()
    {
      Hide();
      _mainMenuView.ShowOptions();
    }

    private void Hide()
    {
      _menuAnimator.PlayHide();
      _topAndBottomMenuAnimator.PlayHide();
    }

    private void OnCreateClick()
    {
      _mapsService.GenerateEmptyStartMap((LevelSize)_mapSize.Value, "Editor");
      _projectController.LoadMapEditor();
      Hide();
    }

    private void LoadData()
    {
      var data = _progressController.Data.Menu.MapEditorOptionsProgressData;
      _mapSize.Value = data.MapSize;
    }

    private void SaveData()
    {
      var data = _progressController.Data.Menu.MapEditorOptionsProgressData;
      data.MapSize = _mapSize.Value;
      _progressController.Save();
    }

    private void OnLoadClick()
    {
      Hide();
      _loadView.Show();
    }

    private void OnImportClick()
    {
      if (_mapsService.CanGenerateFromCopyBuffer())
      {
        _mapsService.GenerateStartMapFromCopyBuffer();
        _projectController.LoadMapEditor();
        Hide();
      }
    }
  }
}