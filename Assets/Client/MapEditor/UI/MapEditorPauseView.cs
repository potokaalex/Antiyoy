using Client.Menu;
using Client.Menu.ExportMessage;
using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.UI;
using UnityEngine;

namespace Client.MapEditor.UI
{
  public class MapEditorPauseView : MonoBehaviour
  {
    [SerializeField] private CustomButton _resumeButton;
    [SerializeField] private CustomButton _playButton;
    [SerializeField] private CustomButton _exportButton;
    [SerializeField] private CustomButton _mainMenuButton;
    [SerializeField] private MenuAnimator _menuAnimator;
    [SerializeField] private ExportMessageView _exportMessageView;
    private MapEditorController _mapEditorController;
    private InputController _inputController;
    private MapEditorUI _mapEditorUI;

    private void Awake()
    {
      _mapEditorUI = Locator.Get<MapEditorUI>();
      _inputController = Locator.Get<InputController>();
      _mapEditorController = Locator.Get<MapEditorController>();
      _resumeButton.OnClick += OnResumeClick;
      _playButton.OnClick += OnPlayClick;
      _exportButton.OnClick += OnExportClick;
      _mainMenuButton.OnClick += OnMainMenuClick;
    }

    private void OnDestroy()
    {
      _resumeButton.OnClick -= OnResumeClick;
      _playButton.OnClick -= OnPlayClick;
      _exportButton.OnClick -= OnExportClick;
      _mainMenuButton.OnClick -= OnMainMenuClick;
    }

    private void Update()
    {
      if (_inputController.BackClicked)
        OnMainMenuClick();
    }

    public void Show() => _menuAnimator.PlayShow();

    private void Hide() => _menuAnimator.PlayHide();

    private void OnResumeClick()
    {
      Hide();
      _mapEditorUI.PlayShow();
    }

    private void OnPlayClick()
    {
      if (_mapEditorController.Play())
        Hide();
    }

    private void OnExportClick()
    {
      _mapEditorController.Export();
      _exportMessageView.Show();
    }

    private void OnMainMenuClick()
    {
      Hide();
      _mapEditorController.MainMenu();
    }
  }
}