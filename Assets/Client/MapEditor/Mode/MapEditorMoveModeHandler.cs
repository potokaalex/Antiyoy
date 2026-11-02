using Client.Project.CameraFeatures;
using Client.Project.Infrastructure;

namespace Client.MapEditor.Mode
{
  public class MapEditorMoveModeHandler : IMapEditorModeHandler
  {
    private readonly CameraController _cameraController;

    public MapEditorMoveModeHandler() => _cameraController = Locator.Get<CameraController>();

    public void Start() => _cameraController.CanMove = true;

    public void Clear() => _cameraController.CanMove = false;
  }
}