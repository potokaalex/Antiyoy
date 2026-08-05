using DG.Tweening;
using UnityEngine;

namespace Client.UITests.MainMenu
{
  public class UIParticlesAnimator : MonoBehaviour
  {
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Vector3 _appearStartOffsetCenter;
    [SerializeField] float _appearStartOffsetValue;
    [SerializeField] float _appearDuration = 1f;

    public void PlayAppearAnimation()
    {
      _particleSystem.Pause();

      var particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
      var count = _particleSystem.GetParticles(particles);
      var startPositions = new Vector3[count];
      var endPositions = new Vector3[count];

      for (var i = 0; i < count; i++)
      {
        endPositions[i] = particles[i].position;

        var dir = _appearStartOffsetCenter - endPositions[i];
        dir.y = 0;
        if (dir == Vector3.zero)
          dir = new Vector3(1, 0, 1);
        dir.Normalize();

        startPositions[i] = endPositions[i] - dir * _appearStartOffsetValue;
        particles[i].position = startPositions[i];
      }

      _particleSystem.SetParticles(particles, count);

      DOVirtual.Float(0, 1, _appearDuration, v =>
      {
        for (var i = 0; i < count; i++) 
          particles[i].position = Vector3.Lerp(startPositions[i], endPositions[i], v);
        _particleSystem.SetParticles(particles, count);
      }).onComplete += _particleSystem.Play;
    }
  }
}