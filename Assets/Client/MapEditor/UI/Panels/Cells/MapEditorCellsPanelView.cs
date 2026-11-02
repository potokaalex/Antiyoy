using Client.Menu;
using Client.Project.UI;
using TMPro;
using UnityEngine;

namespace Client.MapEditor.UI.Panels
{
  public class MapEditorCellsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private MapEditorCellButton[] _cellsButtons;
    [SerializeField] private CustomButton _deleteCellButton;
    [SerializeField] private CustomButton _randomCellButton;
    [SerializeField] private CustomButton _filterButton;
    [SerializeField] private TextMeshProUGUI _filterText;

    public float BottomOffsetY => _topAndBottomMenuAnimator.BottomPanelOffsetY;

    public void Show()
    {
      _topAndBottomMenuAnimator.PlayShow();

      foreach (var button in _cellsButtons)
        button.Setup();
    }

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();
  }
}