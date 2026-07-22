using UnityEngine;

namespace Client.UI
{
  [ExecuteAlways]
  public class SafeAreaFitter : MonoBehaviour
  {
    [SerializeField] private RectTransform _rectTransform;

    private void Start() => Fit();

#if UNITY_EDITOR
    private void LateUpdate() => Fit();

    private void Reset() => _rectTransform = GetComponent<RectTransform>();
#endif

    [ContextMenu("Fit")]
    private void Fit()
    {
#if !UNITY_EDITOR
      if (!Application.isPlaying) 
        return;
#endif

      var safeArea = Screen.safeArea;
      var display = Display.main;
      var screenSize = new Vector2(display.systemWidth, display.systemHeight);
      var anchorMin = safeArea.position;
      var anchorMax = anchorMin + safeArea.size;

      _rectTransform.anchorMin = anchorMin / screenSize;
      _rectTransform.anchorMax = anchorMax / screenSize;
      _rectTransform.offsetMin = Vector2.zero;
      _rectTransform.offsetMax = Vector2.zero;
    }
  }
}