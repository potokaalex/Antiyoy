using Client.Menu;
using Client.UI;
using UnityEngine;

namespace Client.Gameplay.UI.Pause
{
  public class PauseView : MonoBehaviour
  {
    [SerializeField] private CustomButton _continueButton;
    [SerializeField] private CustomButton _mainMenuButton;
    [SerializeField] private MenuAnimator _menuAnimator;

    public void Show() => _menuAnimator.PlayShow();

    public void Hide() => _menuAnimator.PlayHide();
  }
}