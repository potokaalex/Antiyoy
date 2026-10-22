using Client.Gameplay.Gameplay;
using Client.Menu.Intro;
using Client.Project.Infrastructure;
using UnityEditor;
using UnityEngine;

namespace Client.Project.Project
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
      EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
  }
}