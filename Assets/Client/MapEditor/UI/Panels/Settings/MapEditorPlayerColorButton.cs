using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.Panels.Settings
{
  public class MapEditorPlayerColorButton : MonoBehaviour
  {
    [SerializeField] private bool _randomRegionType;
    [SerializeField] [HideIf(nameof(_randomRegionType))] private RegionType _regionType;
    [SerializeField] private Image _image;
    [SerializeField] private CustomButton _button;
    private RegionsService _regionsService;
    private MapEditorController _mapEditorController;
    private MapEditorPlayerColorsView _colorsView;

    private void Awake()
    {
      _regionsService = Locator.Get<RegionsService>();
      _mapEditorController = Locator.Get<MapEditorController>();
      _button.OnClick += OnClick;
    }

    private void OnDestroy() => _button.OnClick -= OnClick;

    public void Setup(MapEditorPlayerColorsView colorsView)
    {
      _colorsView = colorsView;
      if (!_randomRegionType)
        _image.color = _regionsService.GetColorFor(_regionType);
    }

    private void OnClick()
    {
      _mapEditorController.PlayerRegionType = new MapEditorPlayerRegionType(_regionType, _randomRegionType);
      var color = _image.color;
      if (_randomRegionType)
        color = new Color(0.5490196f, 0.5411765f, 0.5490196f, 1);
      _colorsView.PlayColorSelection(color, transform.position);
    }
  }
}