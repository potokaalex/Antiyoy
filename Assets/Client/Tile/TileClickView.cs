using Client.Infrastructure;
using Client.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Client.Tile
{
  public class TileClickView : MonoBehaviour, IInitializable
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private GridController _gridController;

    public void Initialize()
    {
      _gridController = Locator.Get<GridController>();
      gameObject.SetActive(false);
    }

    public void View(CellController cell)
    {
      transform.position = _gridController.HexPositionToWorld(cell.Position);
      transform.localScale = Vector3.one;
      _spriteRenderer.SetAlpha(0.75f);
      gameObject.SetActive(true);

      DOTween.Kill(this);

      DOTween.Sequence()
        .Append(transform.DOScale(1.1f, 0.1f))
        .Append(transform.DOScale(1f, 0.15f))
        .Append(_spriteRenderer.DOFade(0, 0.15f))
        .SetId(this).OnComplete(() => gameObject.SetActive(false));
    }
  }
}