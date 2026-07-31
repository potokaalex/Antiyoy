using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UITests
{
  public class ToPauseMenuTest : MonoBehaviour
  {
    [SerializeField] private RawImage _gameImage;
    [SerializeField] private Image _menuImage;
    [SerializeField] private Image _background;
    [SerializeField] private float _duration;
    [SerializeField] private float _fadeDur;
    private RenderTexture _rt;

    private void Awake() => _rt = new(Screen.width, Screen.height, 32);

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        StartCoroutine(DoAnim());
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        _gameImage.color = Color.white;
        _gameImage.transform.localScale = Vector3.one;

        _menuImage.color = new Color(1,1,1,0);
        _menuImage.transform.localScale = Vector3.zero;
          
        _background.gameObject.SetActive(false);
        _gameImage.gameObject.SetActive(false);
        _menuImage.gameObject.SetActive(false);
      }
    }

    private IEnumerator DoAnim()
    {
      yield return new WaitForEndOfFrame();
      ScreenCapture.CaptureScreenshotIntoRenderTexture(_rt);

      _background.gameObject.SetActive(true);

      _gameImage.texture = _rt;
      _gameImage.gameObject.SetActive(true);
      _gameImage.transform.DOScale(Vector3.zero, _duration);
      _gameImage.color = Color.white;
      _gameImage.DOFade(0, _fadeDur);

      _menuImage.gameObject.SetActive(true);
      _menuImage.transform.localScale = Vector3.zero;
      _menuImage.transform.DOScale(Vector3.one, _duration);
      _menuImage.color = new Color(1,1,1,0);
      _menuImage.DOFade(1, _duration);
    }
  }
}