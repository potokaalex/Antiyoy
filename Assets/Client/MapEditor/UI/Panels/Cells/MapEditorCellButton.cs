using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.Panels.Cells
{
  public class MapEditorCellButton : MonoBehaviour
  {
    [SerializeField] private RegionType _regionType;
    [SerializeField] private CustomButton _button;
    [SerializeField] private Image _image;
    private RegionsService _regionsService;
    private MapEditorCellsPanelView _cellsPanelView;

    private void Awake()
    {
      _regionsService = Locator.Get<RegionsService>();
      _button.OnClick += OnButtonClick;
    }

    private void OnDestroy() => _button.OnClick -= OnButtonClick;

    public void Setup(MapEditorCellsPanelView cellsPanelView)
    {
      _cellsPanelView = cellsPanelView;
      _image.color = _regionsService.GetColorFor(_regionType);
    }

    private void OnButtonClick() => _cellsPanelView.CreateCellMode(_regionType);
  }
}