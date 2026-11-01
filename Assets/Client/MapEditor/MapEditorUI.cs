using Client.Menu;
using Client.Project.Infrastructure;
using UnityEngine;

namespace Client.MapEditor
{
  public class MapEditorUI : MonoBehaviour, IInitializable
  {
    [SerializeField] private MapEditorUIView _viewPrefab;
    private MapEditorUIView _view;
    
    public void Initialize() => _view = Locator.Get<MenuView>().Spawn(_viewPrefab);

    public void PlayShow() => _view.PlayShow();
  }
}