using System.Collections;
using Client.Infrastructure;
using Client.Menu;
using Client.Menu.Intro;
using Client.Menu.MainMenu;
using DG.Tweening;
using UnityEngine;

namespace Client.Boot
{
  public class ProjectInstaller : MonoInstaller
  {
    [SerializeField] private IntroView _introView;
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private MenuView _menuView;

    protected override void Install()
    {
      Register(_menuView);
      Register(_mainMenuView);
    }

    protected override void Start()
    {
      base.Start();
      Application.targetFrameRate = 300;
      QualitySettings.vSyncCount = -1;
      DontDestroyOnLoad(this);
      StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
      yield return null; //Wait one frame to avoid the startup frame affecting tween timing.
      DOTween.Sequence().Append(_introView.Play()).Append(_mainMenuView.PlayAppearAnimation());
    }
  }
}