using Client.Gameplay.UI;
using UnityEngine;

namespace Client.MapEditor
{
  public class MapEditorUIView : MonoBehaviour
  {
    [SerializeField] private GameTransitionView _gameTransitionView;
    
    public void PlayShow()
    {
      //_hudView.PlayShow();
      _gameTransitionView.PlayToyGameTransition();
    }
  }
}