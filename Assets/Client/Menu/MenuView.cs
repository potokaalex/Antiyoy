using Client.Menu.Background;
using UnityEngine;

namespace Client.Menu
{
  public class MenuView : MonoBehaviour
  {
    [SerializeField] private MenuBackgroundView _background;
    [SerializeField] private CanvasGroup _blockInput;
    [SerializeField] private Transform _viewsRoot;

    public MenuBackgroundView Background => _background;

    public bool BackClicked => Input.GetKeyDown(KeyCode.Escape) && !_blockInput.blocksRaycasts;

    public void SetBlockInput(bool blocked) => _blockInput.blocksRaycasts = blocked;

    public void SetActive(bool active) => gameObject.SetActive(active);

    public T Spawn<T>(T prefab) where T : MonoBehaviour => Instantiate(prefab, _viewsRoot);
  }
}