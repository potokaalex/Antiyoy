using DG.Tweening;
using UnityEngine;

namespace Client.Borders
{
  public class BorderController : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Transform Transform { get; private set; }

    private void Awake() => Transform = transform;

    public void SetActive(bool isActive) => _spriteRenderer.enabled = isActive;

    public Tween DoAppearAnimation(Vector3 targetPosition, Vector3 direction)
    {
      Transform.position = targetPosition - direction * 0.05f;
      return Transform.DOMove(targetPosition, 0.2f);
    }
  }
}