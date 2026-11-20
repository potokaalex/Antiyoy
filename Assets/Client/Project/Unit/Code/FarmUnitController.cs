using Client.Project.Infrastructure;
using Client.Project.Map;
using Client.Project.Yiotro;
using UnityEngine;

namespace Client.Project.Unit.Code
{
  public class FarmUnitController : UnitController
  {
    [SerializeField] private Sprite[] _sprites;

    protected override void SetupSprite()
    {
      var gridController = Locator.Get<GridController>();
      var index = YiotroUtilities.HexToIndex(Cell.Position, gridController.LevelSize == LevelSize.Huge);
      Renderer.sprite = _sprites[(101 * index.x * index.y + 7 * index.y) % 3];
    }
  }
}