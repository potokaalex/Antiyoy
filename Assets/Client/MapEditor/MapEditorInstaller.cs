using Client.MapEditor.UI;
using Client.Project.Infrastructure;
using UnityEngine;

namespace Client.MapEditor
{
  public class GameplayInstaller : MonoInstaller
  {
    [SerializeField] private MapEditorUI _mapEditorUI;

    public override void Install(Context context)
    {
      context.Register(_mapEditorUI);
      context.Register(new MapEditorController());
    }
  }
}