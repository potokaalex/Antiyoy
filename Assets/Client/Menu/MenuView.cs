using Client.Menu.Background;
using UnityEngine;

namespace Client.Menu
{
  public class MenuView : MonoBehaviour
  {
    [SerializeField] private MenuBackgroundView _background;

    public MenuBackgroundView Background => _background;
  }
}