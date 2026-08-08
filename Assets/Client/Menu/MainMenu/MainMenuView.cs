using Client.Menu.MainMenu.Options;
using Client.Menu.MainMenu.Start;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu.MainMenu
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private MainMenuStartView _start;
    [SerializeField] private MainMenuOptionsView _options;

    public Tween PlayAppearAnimation() => _start.PlayAppearAnimation();

    public void ShowOptions() => _options.Show();
  }
}