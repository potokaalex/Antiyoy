using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.UI
{
  public class CustomButton : MonoBehaviour, IPointerDownHandler
  {
    [SerializeField] private Image _background;

    public event Action OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
      if (_background)
      {
        DOTween.Kill(this);
        _background.color = new Color(0f, 0f, 0.3f, 0.75f);
        _background.DOFade(0, 0.5f).SetEase(Ease.OutQuad).SetId(this);
      }

      OnClick?.Invoke();
    }
  }
}