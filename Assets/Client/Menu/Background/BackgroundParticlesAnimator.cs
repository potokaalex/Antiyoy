using Client.Project.Utilities;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;

namespace Client.Menu.Background
{
  public class BackgroundParticlesAnimator : MonoBehaviour
  {
    [SerializeField] private UIParticle _uiParticle;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Vector3 _appearStartOffsetCenter;
    [SerializeField] float _appearStartOffsetValue;
    [SerializeField] float _appearDuration = 1f;
    private ParticleSystem.Particle[] _particles;
    private ParticleSystem.MainModule _particleSystemMain;
    private Vector3[] _startPositions;
    private Vector3[] _endPositions;
    private Color _particlesColor;

    private void Awake()
    {
      _particleSystemMain = _particleSystem.main;
      var maxParticles = _particleSystemMain.maxParticles;
      _particles = new ParticleSystem.Particle[maxParticles];
      _startPositions = new Vector3[maxParticles];
      _endPositions = new Vector3[maxParticles];
      _particlesColor = _particleSystemMain.startColor.color;
    }

    public Tween PlayShowAnimation() => PlayCircleMove(true, _appearDuration);

    public Tween PlayShowAnimation(float duration) => PlayCircleMove(true, duration);

    public Tween PlayHideAnimation() => PlayCircleMove(false, _appearDuration);

    public Tween PlayColorTransition(Color color)
    {
      var main = _particleSystemMain;

      return DOVirtual.Float(0, 1, 0.5f, v =>
      {
        var count = _particleSystem.GetParticles(_particles);
        var c = Color.Lerp(_particlesColor, color, v);

        main.startColor = new ParticleSystem.MinMaxGradient(c);
        for (var i = 0; i < count; i++)
          _particles[i].startColor = c;

        _particleSystem.SetParticles(_particles, count);
      });
    }

    private Tween PlayCircleMove(bool moveToCenter, float duration)
    {
      var count = 0;

      return DOTween.Sequence()
        .AppendCallback(() =>
        {
          _uiParticle.Pause();

          count = _particleSystem.GetParticles(_particles);
          _startPositions = new Vector3[count];
          _endPositions = new Vector3[count];

          for (var i = 0; i < count; i++)
          {
            var endPosition = _particles[i].position;
            _endPositions[i] = endPosition;

            var dir = _appearStartOffsetCenter - endPosition;
            dir.y = 0;
            if (dir == Vector3.zero)
              dir = new Vector3(1, 0, 1);
            dir.Normalize();

            var startPosition = endPosition - dir * _appearStartOffsetValue;

            _startPositions[i] = startPosition;
            _particles[i].position = moveToCenter ? startPosition : endPosition;
          }

          _particleSystem.SetParticles(_particles, count);
        })
        .Append(DOVirtual.Float(0, 1, duration, v =>
        {
          var p = moveToCenter ? v : 1 - v;
          for (var i = 0; i < count; i++)
            _particles[i].position = Vector3.Lerp(_startPositions[i], _endPositions[i], p);
          _particleSystem.SetParticles(_particles, count);
        }).AddOnComplete(_uiParticle.Resume));
    }
  }
}