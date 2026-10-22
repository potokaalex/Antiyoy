using Client.Project;
using Client.Project.Infrastructure;
using Client.Project.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Client.Gameplay.UI
{
  public class TileClickView : MonoBehaviour, IInitializable
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Initialize() => gameObject.SetActive(false);

    public void View(CellController cell)
    {
      transform.position = cell.WorldPosition;
      transform.localScale = Vector3.one;
      _spriteRenderer.SetAlpha(0.6f);
      gameObject.SetActive(true);

      DOTween.Kill(this);

      DOTween.Sequence()
        .Append(transform.DOScale(1.1f, 0.15f))
        .Append(transform.DOScale(1f, 0.1f))
        .Append(_spriteRenderer.DOFade(0, 0.2f))
        .SetId(this).OnComplete(() => gameObject.SetActive(false));
    }
  }
}