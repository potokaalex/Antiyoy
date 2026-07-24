using Client.Region;
using UnityEngine;

namespace Client.Borders
{
  public class BordersService : MonoBehaviour
  {
    [SerializeField] private BordersFactory _bordersFactory;
    [SerializeField] private SelectionBordersFactory _selectionBordersFactory;

    public void ClearRegionSelectionBorders() => _selectionBordersFactory.ClearBorders();

    public void ViewRegionSelectionBorders(RegionController region) => _selectionBordersFactory.ViewBorders(region);

    public void ViewRegionsBorders() => _bordersFactory.ViewRegionsBorders();
  }
}