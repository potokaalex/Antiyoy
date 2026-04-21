using Client.Hex;
using Client.Infrastructure;
using UnityEngine;

namespace Client.Unit
{
  public class UnitController : MonoBehaviour
  {
    private GridController _gridController;

    private void Awake()
    {
      _gridController = Locator.Get<GridController>();
    }

    public void Move(HexCoordinates position)
    {
      transform.position = _gridController.HexPositionToWorld(position);
    }
  }
}