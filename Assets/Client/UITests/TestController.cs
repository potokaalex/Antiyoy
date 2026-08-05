using Client.UITests.MainMenu;
using DG.Tweening;
using UnityEngine;

namespace Client.UITests
{
  public class TestController : MonoBehaviour
  {
    [SerializeField] private IntroView _introView;
    [SerializeField] private MainMenuView _mainMenuView;
    
    private void Start()
    {
      Application.targetFrameRate = 300;
      QualitySettings.vSyncCount = -1;
      DOTween.Sequence().Append(_introView.Play()).Append(_mainMenuView.PlayAppearAnimation());
    }
  }
}