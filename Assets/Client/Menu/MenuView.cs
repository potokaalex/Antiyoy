using Client.Menu.Background;
using UnityEngine;

namespace Client.Menu
{
  public class MenuView : MonoBehaviour
  {
    [SerializeField] private MenuBackgroundView _background;
    [SerializeField] private CanvasGroup _blockInput;

    public MenuBackgroundView Background => _background;

    public void SetBlockInput(bool blocked) => _blockInput.blocksRaycasts = blocked;
  }
}