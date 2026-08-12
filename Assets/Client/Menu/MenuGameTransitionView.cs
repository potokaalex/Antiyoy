using System.Collections;
using Client.Infrastructure;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client.Menu
{
  public class MenuGameTransitionView : MonoBehaviour
  {
    [SerializeField] private RawImage _gameImage;
    private CameraController _cameraController;
    private MenuView _menuView;

    private void Awake() => _menuView = Locator.Get<MenuView>();

    public void PlayGameTransition()
    {
      StartCoroutine(A());
    }

    public void SetCamera(CameraController cameraController) => _cameraController = cameraController;

    private IEnumerator A()
    {
      //UnityEngine.Debug.Break();
      var _rt = new RenderTexture(Screen.width, Screen.height, 16);
      var _duration = 0.5f;
      var _fadeDur = 0.5f;//< _duration?
      yield return SceneManager.LoadSceneAsync(1);
      yield return null;
      yield return StartCoroutine(_cameraController.CreateScreenshotCoroutine(_rt));

      _gameImage.texture = _rt;

      _gameImage.gameObject.SetActive(true);
      _gameImage.transform.localScale = Vector3.zero;
      _gameImage.color = new Color(1, 1, 1, 0);
      DOTween.Sequence()
        .Append(_gameImage.DOFade(1, _fadeDur))
        .Join(_gameImage.transform.DOScale(Vector3.one, _duration))
        .OnComplete(() =>
        {
          _gameImage.gameObject.SetActive(false);
          _menuView.SetActive(false);
        })
        .SetEase(Ease.InQuint);
    }
  }
}