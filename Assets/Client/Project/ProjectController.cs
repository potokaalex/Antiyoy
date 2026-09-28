using Client.Gameplay;
using Client.Infrastructure;
using Client.Menu.Intro;
using UnityEngine;

namespace Client.Project
{
  public class ProjectController : IInitializable
  {
    private GameplayController _gameplayController;

    public void Initialize()
    {
      _gameplayController = Locator.Get<GameplayController>();
      var introView = Locator.Get<IntroView>();
      Application.targetFrameRate = 300;
      QualitySettings.vSyncCount = -1;
      introView.Play();
    }

    public void LoadGameplay() => _gameplayController.Start();

    public void Quit()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}