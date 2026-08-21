using System;
using System.Collections;
using Client.Gameplay;
using Client.Infrastructure;
using Client.Menu.Intro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Project
{
  public class ProjectController : IInitializable
  {
    private ICoroutineRunner _coroutineRunner;
    private GameplayController _gameplayController;

    public void Initialize()
    {
      _coroutineRunner = Locator.Get<ICoroutineRunner>();
      _gameplayController = Locator.Get<GameplayController>();
      var introView = Locator.Get<IntroView>();
      Application.targetFrameRate = 300;
      QualitySettings.vSyncCount = -1;
      //SceneManager.LoadScene(1);
      introView.Play();
    }

    public void LoadGameplay()
    {
      _gameplayController.Setup();
      //_coroutineRunner.StartCoroutine(LoadGameplayCoroutine(onComplete));
    }

    public void Quit()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    private IEnumerator LoadGameplayCoroutine(Action onComplete)
    {
      var operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
      yield return operation;
      var loadedScene = SceneManager.GetSceneByBuildIndex(1);
      SceneManager.SetActiveScene(loadedScene);
      onComplete?.Invoke();
    }
  }
}