using Client.Menu;
using Client.Project.UI;
using UnityEngine;

namespace Client.MapEditor.UI.Panels
{
  public class MapEditorUnitsPanelView : MonoBehaviour, IMapEditorHudPanelView
  {
    [SerializeField] private TopAndBottomMenuAnimator _topAndBottomMenuAnimator;
    [SerializeField] private CustomButton _deleteUnitButton;
    [SerializeField] private CustomButton _pineButton;
    [SerializeField] private CustomButton _palmButton;
    [SerializeField] private CustomButton _farmButton;
    [SerializeField] private CustomButton _towerButton;
    [SerializeField] private CustomButton _peasantButton;
    [SerializeField] private CustomButton _spearmanButton;

    public void Show() => _topAndBottomMenuAnimator.PlayShow();

    public void Hide() => _topAndBottomMenuAnimator.PlayHide();
  }
}