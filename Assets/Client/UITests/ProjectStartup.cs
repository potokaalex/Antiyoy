using System.Collections;
using Client.UITests.Menu.Intro;
using Client.UITests.Menu.MainMenu;
using DG.Tweening;
using UnityEngine;

namespace Client.UITests
{
  public class ProjectStartup : MonoBehaviour
  {
    [SerializeField] private IntroView _introView;
    [SerializeField] private MainMenuView _mainMenuView;

    private void Start()
    {
      Application.targetFrameRate = 300;
      QualitySettings.vSyncCount = -1;
      StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
      yield return null; //Wait one frame to avoid the startup frame affecting tween timing.
      DOTween.Sequence().Append(_introView.Play()).Append(_mainMenuView.PlayAppearAnimation());
    }
  }
}