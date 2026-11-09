using DG.Tweening;
using UnityEngine;

namespace Client.Menu.ExportMessage
{
  public class ExportMessageView : MonoBehaviour
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;

    public void Show()
    {
      DOTween.Kill(this);
      _topAndBottomMenuAnimator.Clear(true);
      _topAndBottomMenuAnimator.PlayShow();
      DOTween.Sequence().AppendInterval(1).AppendCallback(() => _topAndBottomMenuAnimator.PlayHide()).SetId(this);
    }
  }
}