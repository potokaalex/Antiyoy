using System.Collections;
using Client.Menu;
using Client.Project;
using Client.Project.CameraFeatures;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class GameTransitionView : MonoBehaviour
  {
    [SerializeField] private RawImage _gameImage;
    [SerializeField] private CanvasGroup _canvasGroup;
    private CameraController _cameraController;
    private MenuView _menuView;
    private RenderTexture _screenshotRt;
    private RenderTexture _freezeRt;
    private InputController _inputController;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();
      _inputController = Locator.Get<InputController>();
      _cameraController = Locator.Get<CameraController>();
      _screenshotRt = new RenderTexture(Screen.width, Screen.height, 16);
      _freezeRt = new RenderTexture(Screen.width, Screen.height, 16);
      _gameImage.texture = _screenshotRt;
      _canvasGroup.alpha = 0;
    }

    private void OnDestroy() => DOTween.Kill(this);

    public void PlayToyGameTransition() => StartCoroutine(PlayToyGameTransitionCoroutine());

    public void PlayOutGameTransition() => StartCoroutine(PlayOutGameTransitionCoroutine());

    private IEnumerator PlayToyGameTransitionCoroutine()
    {
      _cameraController.RenderMenu();
      _cameraController.SetImageRt(_freezeRt);
      yield return StartCoroutine(_cameraController.CreateScreenshotCoroutine(_freezeRt));

      _cameraController.RenderGameplay();
      yield return StartCoroutine(_cameraController.CreateScreenshotCoroutine(_screenshotRt));

      _cameraController.ClearImageRt();
      _cameraController.RenderMenu();

      _canvasGroup.transform.localScale = Vector3.zero;
      _canvasGroup.alpha = 0;

      _menuView.Background.PlayHideAnimation();
      _inputController.SetBlockInput(true);

      DOTween.Sequence()
        .Append(_canvasGroup.transform.DOScale(Vector3.one, 0.5f))
        .Join(_canvasGroup.DOFade(1, 0.5f))
        .OnComplete(() =>
        {
          _canvasGroup.alpha = 0;
          _inputController.SetBlockInput(false);
          _menuView.Background.SetActive(false);
          _cameraController.RenderGameplay();
          _cameraController.CanMove = true;
        })
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);
    }

    private IEnumerator PlayOutGameTransitionCoroutine()
    {
      _cameraController.CanMove = false;

      _cameraController.RenderGameplay();
      yield return StartCoroutine(_cameraController.CreateScreenshotCoroutine(_screenshotRt));
      _cameraController.RenderMenu();

      _canvasGroup.alpha = 1;
      _canvasGroup.transform.localScale = Vector3.one;

      _menuView.Background.SetActive(true);
      _menuView.Background.PlayShowAnimation();
      _inputController.SetBlockInput(true);

      DOTween.Sequence()
        .Append(_canvasGroup.transform.DOScale(Vector3.zero, 0.5f))
        .Join(_canvasGroup.DOFade(0, 0.5f))
        .OnComplete(() =>
        {
          _canvasGroup.alpha = 0;
          _inputController.SetBlockInput(false);
        })
        .SetEase(AnimationsUtilities.MenuDefaultEase)
        .SetId(this);
    }
  }
}