using DG.Tweening;
using UnityEngine;

namespace Client.Borders
{
  public class BorderController : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    public Transform Transform { get; private set; }

    private void Awake() => Transform = transform;

    public void SetActive(bool isActive)
    {
      foreach (var r in _spriteRenderers) 
        r.enabled = isActive;
    }

    public Tween DoAppearAnimation(Vector3 targetPosition, Vector3 direction, bool forceAnimation)
    {
      if (forceAnimation)
      {
        Transform.position = targetPosition;
        return null;
      }

      Transform.position = targetPosition - direction * 0.05f;
      return Transform.DOMove(targetPosition, 0.25f);
    }
  }
}