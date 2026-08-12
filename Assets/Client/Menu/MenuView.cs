using Client.Menu.Background;
using UnityEngine;

namespace Client.Menu
{
  public class MenuView : MonoBehaviour
  {
    [SerializeField] private MenuBackgroundView _background;
    [SerializeField] private CanvasGroup _blockInput;
    [SerializeField] private MenuGameTransitionView _gameTransitionView;

    public MenuBackgroundView Background => _background;

    public void SetBlockInput(bool blocked) => _blockInput.blocksRaycasts = blocked;

    public void PlayGameTransition() => _gameTransitionView.PlayGameTransition();

    public void SetGameplayCamera(CameraController cameraController) => _gameTransitionView.SetCamera(cameraController);

    public void SetActive(bool active)
    {
      gameObject.SetActive(active);
    }
  }
}