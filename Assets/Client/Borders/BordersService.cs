using System.Collections.Generic;
using Client.Region;
using UnityEngine;

namespace Client.Borders
{
  public class BordersService : MonoBehaviour
  {
    [SerializeField] private BordersFactory _bordersFactory;
    [SerializeField] private RegionSelectionBordersFactory _regionSelectionBordersFactory;
    [SerializeField] private TilesSelectionBordersFactory _tilesSelectionBordersFactory;

    public void ClearRegionSelectionBorders() => _regionSelectionBordersFactory.ClearBorders();

    public void ViewRegionSelectionBorders(RegionController region) => _regionSelectionBordersFactory.ViewBorders(region);

    public void ViewRegionsBorders() => _bordersFactory.ViewRegionsBorders();

    public void ViewTilesSelectionBorders(List<CellController> cells) => _tilesSelectionBordersFactory.ViewBorders(cells);

    public void ClearTilesSelectionBorders() => _tilesSelectionBordersFactory.ClearBorders();
  }
}