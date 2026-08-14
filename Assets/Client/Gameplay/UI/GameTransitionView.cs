using System.Collections;
using Client.Infrastructure;
using Client.Menu;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Gameplay.UI
{
  public class GameTransitionView : MonoBehaviour
  {
    [SerializeField] private RawImage _gameImage;
    private CameraController _cameraController;
    private MenuView _menuView;
    private GameplayUI _gameplayUI;
    private RenderTexture _rt;

    private void Awake()
    {
      _menuView = Locator.Get<MenuView>();
      _cameraController = Locator.Get<CameraController>();
      _gameplayUI = Locator.Get<GameplayUI>();
      _rt = new RenderTexture(Screen.width, Screen.height, 16);
      _gameImage.texture = _rt;
    }

    public void PlayGameTransition() => StartCoroutine(PlayGameTransitionCoroutine());

    private IEnumerator PlayGameTransitionCoroutine()
    {
      yield return StartCoroutine(_cameraController.CreateScreenshotCoroutine(_rt));

      _gameImage.gameObject.SetActive(true);
      _gameImage.transform.localScale = Vector3.zero;
      _gameImage.color = new Color(1, 1, 1, 0);

      _menuView.Background.PlayHideAnimation();

      DOTween.Sequence()
        .Append(_gameImage.transform.DOScale(Vector3.one, 0.5f))
        .Join(_gameImage.DOFade(1, 0.5f))
        .Join(_gameplayUI.Hud.PlayShow())
        .OnComplete(() =>
        {
          _gameImage.gameObject.SetActive(false);
          _menuView.SetActive(false);
        })
        .SetEase(AnimationsUtilities.MenuDefaultEase);
    }
  }
}

