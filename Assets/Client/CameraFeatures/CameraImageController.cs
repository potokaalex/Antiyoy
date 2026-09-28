using System.Collections;
using UnityEngine;

namespace Client.CameraFeatures
{
  public class CameraImageController : MonoBehaviour
  {
    private RenderTexture _screenshotRt;
    private RenderTexture _imageRt;
    private bool _returnScreenshotRequest;

    public void SetImageRt(RenderTexture rt) => _imageRt = rt;

    public void ClearImageRt() => _imageRt = null;

    public IEnumerator CreateScreenshotCoroutine(RenderTexture rt)
    {
      _screenshotRt = rt;
      while (!_returnScreenshotRequest)
        yield return null;
      _screenshotRt = null;
      _returnScreenshotRequest = false;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      if (_screenshotRt != null)
      {
        Graphics.Blit(source, _screenshotRt);
        _returnScreenshotRequest = true;
      }

      if (_imageRt != null)
      {
        Graphics.Blit(_imageRt, destination);
        return;
      }
      
      Graphics.Blit(source, destination);
    }
  }
}