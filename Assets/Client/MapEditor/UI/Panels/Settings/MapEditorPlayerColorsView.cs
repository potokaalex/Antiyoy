using Client.Menu;
using Client.Project.UI;
using UnityEngine;

namespace Client.MapEditor.UI.Panels.Settings
{
  public class MapEditorPlayerColorsView : MonoBehaviour
  {
    [SerializeField] private CustomButton _closeButton;
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private MapEditorPlayerColorButton[] _buttons;

    private void Awake() => _closeButton.OnClick += Hide;

    private void OnDestroy() => _closeButton.OnClick -= Hide;

    public void Show()
    {
      _topAndBottomMenuAnimator.PlayShow();
      foreach (var button in _buttons) 
        button.Setup();
    }

    private void Hide() => _topAndBottomMenuAnimator.PlayHide();
  }
}