using DG.Tweening;
using UnityEngine;

namespace Client.UITests.Menu.Background
{
  public class BackgroundParticlesAnimator : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Vector3 _appearStartOffsetCenter;
    [SerializeField] float _appearStartOffsetValue;
    [SerializeField] float _appearDuration = 1f;
    private ParticleSystem.Particle[] _particles;
    private Vector3[] _startPositions;
    private Vector3[] _endPositions;

    private void Awake()
    {
      var maxParticles = _particleSystem.main.maxParticles;
      _particles = new ParticleSystem.Particle[maxParticles];
      _startPositions = new Vector3[maxParticles];
      _endPositions = new Vector3[maxParticles];
    }

    public void PlayAppearAnimation()
    {
      _particleSystem.Pause();

      _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
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
  }
}