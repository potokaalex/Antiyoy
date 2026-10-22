using Client.Project.Infrastructure;
using Client.Project.Unit.Code;
using DG.Tweening;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class UnitSelectionView : MonoBehaviour, IInitializable
  {
    [SerializeField] private SpriteRenderer _icon;
    private UnitsService _unitsService;

    public void Initialize()
    {
      _unitsService = Locator.Get<UnitsService>();
      gameObject.SetActive(false);
    }

    public void View(IUnit unit)
    {
      _icon.sprite = _unitsService.GetSprite(unit.Type);
      transform.position = unit.Cell.WorldPosition;
      gameObject.SetActive(true);
      DOTween.Kill(this);
      transform.localScale = Vector3.one * 0.75f;
      transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad).SetId(this);
    }

    public void Hide() => gameObject.SetActive(false);
  }
}