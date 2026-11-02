using Client.Project.Infrastructure;
using Client.Project.Region;
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
    private RegionsService _regionsService;

    private void Awake() => _regionsService = Locator.Get<RegionsService>();

    public void Setup()
    {
      if (!_randomRegionType)
        _image.color = _regionsService.GetColorFor(_regionType);
    }
  }
}