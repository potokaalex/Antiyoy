using Client.Menu.MainMenu.GameplayModes;
using Client.Menu.MainMenu.Options;
using Client.Menu.MainMenu.SaveLoad;
using Client.Menu.MainMenu.Start;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu.MainMenu
{
  public class MainMenuView : MonoBehaviour
  {
    [SerializeField] private MainMenuStartView _start;
    [SerializeField] private MainMenuOptionsView _options;
    [SerializeField] private MainMenuGameplayModesView _gameplayModesView;
    [SerializeField] private LoadView _loadView;

    public MainMenuGameplayModesView GameplayModesView => _gameplayModesView;

    public Tween PlayAppearAnimation() => _start.PlayAppearAnimation();

    public void ShowOptions() => _options.Show();

    public void ShowStart() => _start.Show();

    public void ShowLoad() => _loadView.Show();
  }
}