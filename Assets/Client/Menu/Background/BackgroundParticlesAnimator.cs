using DG.Tweening;
using UnityEngine;

namespace Client.Menu.Background
{
  public class BackgroundParticlesAnimator : MonoBehaviour
  {
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

    public void PlayAppearAnimation()
    {
      _particleSystem.Pause();

      var count = _particleSystem.GetParticles(_particles);
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
        _particles[i].position = startPosition;
      }

      _particleSystem.SetParticles(_particles, count);

      DOVirtual.Float(0, 1, _appearDuration, v =>
      {
        for (var i = 0; i < count; i++)
          _particles[i].position = Vector3.Lerp(_startPositions[i], _endPositions[i], v);
        _particleSystem.SetParticles(_particles, count);
      }).onComplete += _particleSystem.Play;
    }

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
  }
}