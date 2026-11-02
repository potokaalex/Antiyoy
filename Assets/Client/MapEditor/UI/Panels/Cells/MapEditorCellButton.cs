using Client.Project.Infrastructure;
using Client.Project.Region;
using Client.Project.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.Panels
{
  public class MapEditorCellButton : MonoBehaviour
  {
    [SerializeField] private RegionType _regionType;
    [SerializeField] private CustomButton _button;
    [SerializeField] private Image _image;
    private RegionsService _regionsService;

    private void Awake() => _regionsService = Locator.Get<RegionsService>();

    public void Setup() => _image.color = _regionsService.GetColorFor(_regionType);
  }
}