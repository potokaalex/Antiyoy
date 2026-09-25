using UnityEngine;

namespace Client.Unit.Code
{
  public class FarmUnitController : UnitController
  {
    [SerializeField] private Sprite[] _sprites;

    protected override void SetupSprite() => Renderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
  }
}