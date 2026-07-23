using UnityEngine;

namespace Client.Borders
{
  public class BorderController : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public Transform Transform { get; private set; }

    public void Initialize(float width, float height)
    {
      Transform = transform;
      Transform.localScale = new Vector3(height, width, 1);
    }

    public void SetActive(bool isActive) => _spriteRenderer.enabled = isActive;
  }
}