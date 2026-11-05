using Client.Menu;
using Client.Project.UI;
using Client.Project.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Client.MapEditor.UI.Panels.Settings
{
  public class MapEditorPlayerColorsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private MapEditorPlayerColorButton[] _buttons;
    [SerializeField] private Image _animationImage;

    private void Awake()
    {
      _closeButton.OnClick += Hide;
      _animationImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
      _closeButton.OnClick -= Hide;
      DOTween.Kill(this);
    }

    public void Show()
    {
      _topAndBottomMenuAnimator.PlayShow();
      foreach (var button in _buttons)
        button.Setup(this);
    }

    public void PlayColorSelection(Color color, Vector3 position)
    {
      _animationImage.gameObject.SetActive(true);
      _animationImage.color = color;
      _animationImage.transform.position = position;
      _animationImage.transform.localScale = Vector3.one;
      _animationImage.transform.DOScale(20, 0.5f / 1.5f).OnComplete(() => _animationImage.gameObject.SetActive(false)).SetId(this)
        .SetEase(AnimationsUtilities.MenuDefaultEase);
      Hide();
    }

    private void Hide() => _topAndBottomMenuAnimator.PlayHide();
  }
}