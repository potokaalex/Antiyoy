using Client.Infrastructure;
using Client.Unit.Code;
using DG.Tweening;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class UnitSelectionView : MonoBehaviour, IInitializable
  {
    [SerializeField] private SpriteRenderer _icon;
    private UnitsService _unitsService;
    private GridController _gridController;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      _gridController = Locator.Get<GridController>();
      gameObject.SetActive(false);
    }

    public void View(IUnit unit)
    {
      _icon.sprite = _unitsService.GetSprite(unit.Type);
      transform.position = _gridController.HexPositionToWorld(unit.Cell.Position);
      gameObject.SetActive(true);
      DOTween.Kill(this);
      transform.localScale = Vector3.one * 0.75f;
      transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad).SetId(this);
    }

    public void Hide() => gameObject.SetActive(false);
  }
}