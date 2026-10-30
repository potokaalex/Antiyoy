using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client.Project.UI
{
  public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
  {
    [SerializeField] private Image _background;
    [SerializeField] private bool _hardFadeAnimation;
    [SerializeField] private Color _backgroundColor = new(0f, 0f, 0.3f, 0.75f);
    [SerializeField] private bool _fadeAnimationOnRelease;
    private readonly float _longPressDuration = 0.25f;
    private float _pressTime;
    private bool _isPressed;
    private Vector2 _pressPosition;

    public event Action OnClick;

    public event Action OnLongPress;

    public event Action OnRelease;

    public bool IsLongPress { get; private set; }

    public bool WasDragged { get; private set; }

    private void Update()
    {
      if (!_isPressed || WasDragged)
        return;

      if (!IsLongPress && Time.unscaledTime - _pressTime >= _longPressDuration)
      {
        IsLongPress = true;
        OnLongPress?.Invoke();
      }
    }

    private void OnDisable()
    {
      ClearPressed();
      DOTween.Kill(this);
      IsLongPress = false;
      WasDragged = false;
      if(_background)
        _background.color = Color.clear;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _isPressed = true;
      _pressTime = Time.unscaledTime;
      _pressPosition = eventData.position;
      IsLongPress = false;
      WasDragged = false;

      if (_background)
      {
        DOTween.Kill(this);
        _background.color = _backgroundColor;
        if (!_fadeAnimationOnRelease)
          FadeAnimation();
      }

      OnClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      CheckDrag(eventData);
      ClearPressed();
      OnRelease?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
      if (!_isPressed || WasDragged)
        return;
      CheckDrag(eventData);
    }

    private void CheckDrag(PointerEventData eventData)
    {
      if (Vector2.Distance(_pressPosition, eventData.position) >= EventSystem.current.pixelDragThreshold)
        WasDragged = true;
    }

    private void FadeAnimation()
    {
      DOTween.Kill(this);

      if (_background)
      {
        if (_hardFadeAnimation)
          DOVirtual.Float(1, 0, 0.5f, _ => { }).OnComplete(() => _background.color = Color.clear).SetId(this);
        else
          _background.DOFade(0, 0.5f).SetEase(Ease.OutQuad).SetId(this);
      }
    }

    private void ClearPressed()
    {
      _isPressed = false;
      if (_fadeAnimationOnRelease)
        FadeAnimation();
    }
  }
}