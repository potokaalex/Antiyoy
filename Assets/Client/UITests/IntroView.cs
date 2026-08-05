using Client.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UITests
{
  public class IntroView : MonoBehaviour
  {
    [SerializeField] private RectTransform _textRoot;
    [SerializeField] private Image _fade;

    public Tween Play()
    {
      return DOTween.Sequence()
        .Append(AnimationsUtilities.DoAnchoredMove(_textRoot, new Vector2(0, -50), Vector2.zero))
        .Join(_fade.DOFade(0, 0.25f))
        .AppendInterval(0.5f)
        .AppendCallback(() => gameObject.SetActive(false));
    }
  }
}